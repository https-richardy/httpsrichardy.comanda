namespace Comanda.Orchestrator.WebApi.Extensions;

public static class ClientsExtension
{
    public static void AddHttpClients(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var settings = serviceProvider.GetRequiredService<ISettings>();

        // registers the header propagation service
        // essential for receiving an authenticated request and forwarding it to another service
        services.AddHeaderPropagation(options =>
        {
            options.Headers.Add(Headers.Authorization);
        });

        var customersClient = services.AddHttpClient<ICustomerClient, CustomerClient>(client =>
        {
            client.BaseAddress = new Uri(settings.Downstream.ProfilesUrl);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        var ownersClient = services.AddHttpClient<IOwnerClient, OwnerClient>(client =>
        {
            client.BaseAddress = new Uri(settings.Downstream.ProfilesUrl);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        var paymentsClient = services.AddHttpClient<IPaymentClient, PaymentClient>(client =>
        {
            client.BaseAddress = new Uri(settings.Downstream.PaymentsUrl);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        var storesClient = services.AddHttpClient<IEstablishmentClient, EstablishmentClient>(client =>
        {
            client.BaseAddress = new Uri(settings.Downstream.StoresUrl);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        var productsClient = services.AddHttpClient<IProductClient, ProductClient>(client =>
        {
            client.BaseAddress = new Uri(settings.Downstream.StoresUrl);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        var credentialsClient = services.AddHttpClient<ICredentialsClient, CredentialsClient>(client =>
        {
            client.BaseAddress = new Uri(settings.Downstream.StoresUrl);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        var subscriptionsClient = services.AddHttpClient<ISubscriptionClient, SubscriptionClient>(client =>
        {
            client.BaseAddress = new Uri(settings.Downstream.SubscriptionsUrl);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        var ordersClient = services.AddHttpClient<IOrderClient, OrderClient>(client =>
        {
            client.BaseAddress = new Uri(settings.Downstream.OrdersUrl);
            client.Timeout = TimeSpan.FromMinutes(minutes: 1, seconds: 30);
        });

        customersClient.AddHeaderPropagation();
        ownersClient.AddHeaderPropagation();

        paymentsClient.AddHeaderPropagation();
        storesClient.AddHeaderPropagation();
        productsClient.AddHeaderPropagation();

        subscriptionsClient.AddHeaderPropagation();
        ordersClient.AddHeaderPropagation();
        credentialsClient.AddHeaderPropagation();
    }
}
