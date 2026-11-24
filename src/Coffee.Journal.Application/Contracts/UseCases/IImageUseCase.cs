using Coffee.Journal.Application.Contracts.Models;

namespace Coffee.Journal.Application.Contracts.UseCases;

public interface IImageUseCase
{
    Task<ImageResponse> UploadAsync(
        Stream content,
        string contentType,
        long length,
        CancellationToken cancellationToken = default);
}
