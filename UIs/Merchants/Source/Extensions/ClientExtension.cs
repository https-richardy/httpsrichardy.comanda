namespace Comanda.Merchants.WebUI.Extensions;

public static class ClientExtension
{
    public static void AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        var address = configuration.GetValue<string>("Settings:Gateway")!;
        var storeClient = services.AddHttpClient<IStoreClient, StoreClient>(client =>
        {
            client.BaseAddress = new Uri(address);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        var orderClient = services.AddHttpClient<IOrderClient, OrderClient>(client =>
        {
            client.BaseAddress = new Uri(address);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        var profilesClient = services.AddHttpClient<IProfilesClient, ProfilesClient>(client =>
        {
            client.BaseAddress = new Uri(address);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        // ensure an authentication interceptor is always registered for http clients
        // every http client must include a message handler responsible for authentication

        storeClient.AddHttpMessageHandler<AuthenticationInterceptor>();
        orderClient.AddHttpMessageHandler<AuthenticationInterceptor>();
        profilesClient.AddHttpMessageHandler<AuthenticationInterceptor>();
    }
}
