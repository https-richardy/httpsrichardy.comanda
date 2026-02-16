namespace Comanda.Stores.Application.Gateways;

public interface IFileGateway
{
    public Task<Image> UploadFileAsync(
        Stream stream,
        CancellationToken cancellation = default
    );

    public Task<bool> DeleteFileAsync(
        Image image,
        CancellationToken cancellation = default
    );
}
