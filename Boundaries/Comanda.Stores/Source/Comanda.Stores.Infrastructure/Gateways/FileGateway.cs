namespace Comanda.Stores.Infrastructure.Gateways;

public sealed class FileGateway(StorageOptions options) : IFileGateway
{
    public async Task<Image> UploadFileAsync(Stream stream, CancellationToken cancellation = default)
    {
        Directory.CreateDirectory(options.RootPath);

        var fileName = $"{Guid.NewGuid()}.jpg";
        var filePath = Path.Combine(options.RootPath, fileName);

        using var fileStream = new FileStream(
            path: filePath,
            mode: FileMode.Create,
            access: FileAccess.Write,
            share: FileShare.None
        );

        await stream.CopyToAsync(fileStream, cancellation);

        return new Image(Path: $"{options.AssetsDirectory}/{fileName}");
    }

    public async Task<bool> DeleteFileAsync(Image image, CancellationToken cancellation = default)
    {
        if (image is null || string.IsNullOrWhiteSpace(image.Path))
        {
            return false;
        }

        var filePath = Path.Combine(options.RootPath, Path.GetFileName(image.Path));
        if (!File.Exists(filePath))
        {
            return false;
        }

        await Task.Run(() => File.Delete(filePath), cancellation);

        return true;
    }
}
