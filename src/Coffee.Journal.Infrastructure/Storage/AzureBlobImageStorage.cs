using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Coffee.Journal.Domain.Contracts.Storage;
using Microsoft.Extensions.Logging;

namespace Coffee.Journal.Infrastructure.Storage;

internal sealed class AzureBlobImageStorage : IImageStorage
{
    private static readonly Dictionary<string, string> Extensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["image/jpeg"] = ".jpg",
        ["image/png"] = ".png",
        ["image/webp"] = ".webp",
    };

    private readonly BlobContainerClient _container;
    private readonly ILogger<AzureBlobImageStorage> _logger;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private bool _initialized;

    public AzureBlobImageStorage(BlobContainerClient container, ILogger<AzureBlobImageStorage> logger)
    {
        _container = container;
        _logger = logger;
    }

    public async Task<string> UploadAsync(Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        if (!Extensions.TryGetValue(contentType, out string? extension))
        {
            throw new ArgumentException($"Content type '{contentType}' is not supported.", nameof(contentType));
        }

        await EnsureContainerAsync(cancellationToken);

        BlobClient blob = _container.GetBlobClient($"{Guid.NewGuid()}{extension}");

        await blob.UploadAsync(
            content,
            new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } },
            cancellationToken);

        return blob.Uri.AbsoluteUri;
    }

    public async Task DeleteAsync(string url, CancellationToken cancellationToken = default)
    {
        if (!IsStorageUrl(url))
        {
            return;
        }

        string blobName = Uri.UnescapeDataString(new Uri(url).Segments[^1]);

        try
        {
            await _container.DeleteBlobIfExistsAsync(blobName, cancellationToken: cancellationToken);
        }
        catch (RequestFailedException ex)
        {
            // The database is already updated at this point, so an orphaned blob must not fail the request.
            _logger.LogWarning(ex, "Could not delete image blob {BlobName}", blobName);
        }
    }

    public bool IsStorageUrl(string url)
    {
        string prefix = _container.Uri.AbsoluteUri.TrimEnd('/') + "/";

        return Uri.TryCreate(url, UriKind.Absolute, out Uri? uri)
            && uri.AbsoluteUri.StartsWith(prefix, StringComparison.Ordinal)
            && string.IsNullOrEmpty(uri.Query)
            && uri.AbsoluteUri.IndexOf('/', prefix.Length) < 0;
    }

    private async Task EnsureContainerAsync(CancellationToken cancellationToken)
    {
        if (_initialized)
        {
            return;
        }

        await _initLock.WaitAsync(cancellationToken);
        try
        {
            if (!_initialized)
            {
                await _container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);
                _initialized = true;
            }
        }
        finally
        {
            _initLock.Release();
        }
    }
}
