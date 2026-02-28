namespace Comanda.Merchants.WebUI.Extensions;

public static class GatewaysExtension
{
    public static void AddGateways(this IServiceCollection services, IConfiguration configuration)
    {
        var address = configuration.GetValue<string>("Settings:IdentityProvider")!;
        var realm = configuration.GetValue<string>("Settings:Realm")!;

        services.AddScoped<ILocalStorageGateway, LocalStorageGateway>();
        services.AddHttpClient<IIdentityClient, IdentityClient>(client =>
        {
            client.BaseAddress = new Uri(address);
            client.DefaultRequestHeaders.Add(Headers.Realm, realm);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        var principalGateway = services.AddHttpClient<IPrincipalProvider, PrincipalProvider>(client =>
        {
            client.BaseAddress = new Uri(address);
            client.DefaultRequestHeaders.Add(Headers.Realm, realm);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        // the authentication interceptor will add the access token to each request
        // register always after the http client registration

        principalGateway.AddHttpMessageHandler<AuthenticationInterceptor>();
    }
}
