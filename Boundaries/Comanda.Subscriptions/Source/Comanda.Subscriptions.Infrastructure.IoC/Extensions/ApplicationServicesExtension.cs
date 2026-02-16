namespace Comanda.Subscriptions.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class ApplicationServicesExtension
{
    public static void AddServices(this IServiceCollection services, ISettings settings)
    {
        StripeConfiguration.ApiKey = settings.Stripe.Secret;
        services.AddTransient<ISubscriptionGateway, SubscriptionGateway>();
    }
}