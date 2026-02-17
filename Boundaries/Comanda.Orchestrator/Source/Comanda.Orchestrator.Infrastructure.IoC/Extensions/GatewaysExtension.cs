namespace Comanda.Orchestrator.Infrastructure.IoC.Extensions;

public static class GatewaysExtension
{
    public static void AddGateways(this IServiceCollection services)
    {
        // gateways are registered as singleton because they encapsulate polly resilience policies
        // (timeout, retry, fallback, and circuit breaker).

        // the circuit breaker relies on shared internal state to work correctly,
        // therefore the singleton lifetime is required to preserve this behavior.

        services.AddSingleton<IProfilesGateway, ProfilesGateway>();
        services.AddSingleton<IPaymentGateway, PaymentGateway>();
        services.AddSingleton<IEstablishmentGateway, EstablishmentGateway>();
        services.AddSingleton<IProductGateway, ProductGateway>();
        services.AddSingleton<ISubscriptionGateway, SubscriptionGateway>();
        services.AddSingleton<IOrdersGateway, OrdersGateway>();
        services.AddSingleton<ICredentialsGateway, CredentialsGateway>();
    }
}
