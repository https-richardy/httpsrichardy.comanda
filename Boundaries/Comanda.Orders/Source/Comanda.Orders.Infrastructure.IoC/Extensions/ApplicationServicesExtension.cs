namespace Comanda.Orders.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class ApplicationServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IOrderService, OrderService>();
    }
}
