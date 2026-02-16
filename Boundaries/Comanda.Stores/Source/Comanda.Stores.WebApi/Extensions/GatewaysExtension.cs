namespace Comanda.Stores.WebApi.Extensions;

public static class GatewaysExtension
{
    public static void AddGateways(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddSingleton<IFileGateway, FileGateway>(provider =>
        {
            var options = new StorageOptions
            {
                RootPath = Path.Combine(environment.WebRootPath, Storage.StaticAssetsDirectory),
                AssetsDirectory = Storage.StaticAssetsDirectory,
            };

            return new FileGateway(options: options);
        });
    }
}
