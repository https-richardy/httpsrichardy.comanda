namespace Comanda.Stores.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage(Justification = "only contains dependency injection registration with no business logic")]
public static class MediatorExtension
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddDispatcher(options =>
        {
            options.ScanAssembly<EstablishmentCreationHandler>();
        });
    }
}
