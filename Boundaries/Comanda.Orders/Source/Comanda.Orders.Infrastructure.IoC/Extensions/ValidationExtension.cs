namespace Comanda.Orders.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class ValidationExtension
{
    public static void AddValidation(this IServiceCollection services)
    {
        services.AddTransient<IValidator<OrderCreationScheme>, OrderCreationSchemeValidator>();
        services.AddTransient<IValidator<OrderModificationScheme>, OrderModificationSchemeValidator>();
    }
}
