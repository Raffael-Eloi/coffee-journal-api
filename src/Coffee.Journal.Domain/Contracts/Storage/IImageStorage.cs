namespace Coffee.Journal.Domain.Contracts.Storage;

public interface IImageStorage
{
    /// <summary>Uploads the image and returns its public URL.</summary>
    Task<string> UploadAsync(Stream content, string contentType, CancellationToken cancellationToken = default);

    /// <summary>Deletes the image. Does nothing when it does not exist.</summary>
    Task DeleteAsync(string url, CancellationToken cancellationToken = default);

    /// <summary>Checks whether the URL points to an image of this storage.</summary>
    bool IsStorageUrl(string url);
}
