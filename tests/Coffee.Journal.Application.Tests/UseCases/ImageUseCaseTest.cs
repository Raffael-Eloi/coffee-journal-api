using Coffee.Journal.Application.Contracts.Models;
using Coffee.Journal.Application.Exceptions;
using Coffee.Journal.Application.UseCases;
using Coffee.Journal.Domain.Contracts.Storage;
using Moq;

namespace Coffee.Journal.Application.Tests.UseCases;

public class ImageUseCaseTest
{
    private readonly Mock<IImageStorage> _storageMock;
    private readonly ImageUseCase _useCase;

    public ImageUseCaseTest()
    {
        _storageMock = new Mock<IImageStorage>();
        _useCase = new ImageUseCase(_storageMock.Object);
    }

    [Theory]
    [InlineData("image/jpeg")]
    [InlineData("image/png")]
    [InlineData("IMAGE/WEBP")]
    public async Task GivenValidImage_WhenUpload_ThenTheStorageUrlShouldBeReturned(string contentType)
    {
        #region Arrange

        using MemoryStream stream = new([1, 2, 3]);

        _storageMock
            .Setup(storage => storage.UploadAsync(stream, contentType, It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage/coffee-images/image.png");

        #endregion

        #region Act

        ImageResponse response = await _useCase.UploadAsync(stream, contentType, stream.Length);

        #endregion

        #region Assert

        Assert.Equal("https://storage/coffee-images/image.png", response.Url);

        #endregion
    }

    [Theory]
    [InlineData("application/pdf", 10)]
    [InlineData("image/gif", 10)]
    [InlineData("image/png", 0)]
    [InlineData("image/png", ImageUseCase.MaxLengthInBytes + 1)]
    public async Task GivenInvalidImage_WhenUpload_ThenAValidationExceptionShouldBeThrownAndNothingShouldBeUploaded(
        string contentType,
        long length)
    {
        #region Arrange

        using MemoryStream stream = new([1, 2, 3]);

        #endregion

        #region Act

        RequestValidationException exception = await Assert.ThrowsAsync<RequestValidationException>(
            () => _useCase.UploadAsync(stream, contentType, length));

        #endregion

        #region Assert

        Assert.True(exception.Errors.ContainsKey("file"));

        _storageMock
            .Verify(storage => storage.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());

        #endregion
    }
}
