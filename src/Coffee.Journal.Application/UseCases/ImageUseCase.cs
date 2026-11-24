using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Contracts.UseCases;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Domain.Contracts.Storage;

namespace Coffee.Journal.Application.UseCases;

public sealed class ImageUseCase : IImageUseCase
{
    public const long MaxLengthInBytes = 5 * 1024 * 1024;

    private static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png", "image/webp"];

    private readonly IImageStorage _storage;

    public ImageUseCase(IImageStorage storage)
    {
        _storage = storage;
    }

    public async Task<ImageResponse> UploadAsync(
        Stream content,
        string contentType,
        long length,
        CancellationToken cancellationToken = default)
    {
        if (!AllowedContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
        {
            throw new RequestValidationException(
                "file",
                "The image must be a JPEG, PNG or WebP file.");
        }

        if (length is <= 0 or > MaxLengthInBytes)
        {
            throw new RequestValidationException(
                "file",
                $"The image must have between 1 byte and {MaxLengthInBytes / 1024 / 1024} MB.");
        }

        string url = await _storage.UploadAsync(content, contentType, cancellationToken);

        return new ImageResponse { Url = url };
    }
}
