namespace Comanda.Payments.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class HttpClientsExtension
{
    public static void AddHttpClients(this IServiceCollection services, ISettings settings)
    {
        // register transient lifetime interceptors here
        // https://learn.microsoft.com/en-us/aspnet/web-api/overview/advanced/httpclient-parameters-handlers
        services.AddTransient<CredentialInterceptor>();

        var paymentClient = services.AddHttpClient<IAbacatePayClient, AbacatePayClient>(client =>
        {
            client.BaseAddress = new Uri(settings.AbacatePay.Url);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        paymentClient.AddHttpMessageHandler<CredentialInterceptor>();
    }
}
