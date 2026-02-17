namespace Comanda.Payments.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class ApplicationServicesExtension
{
    public static void AddServices(this IServiceCollection services, ISettings settings)
    {
        services.AddTransient<IPaymentGateway, PaymentGateway>();
    }
}
