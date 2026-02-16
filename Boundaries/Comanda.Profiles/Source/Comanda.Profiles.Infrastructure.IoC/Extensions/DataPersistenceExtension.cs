namespace Comanda.Profiles.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class DataPersistenceExtension
{
    public static void AddDataPersistence(this IServiceCollection services, ISettings settings)
    {
        services.AddSingleton<IMongoDatabase>(provider =>
        {
            var mongoClient = new MongoClient(settings.Database.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Database.DatabaseName);

            return database;
        });

        services.AddTransient<IActivityCollection, ActivityCollection>();
        services.AddTransient<ICustomerCollection, CustomerCollection>();
        services.AddTransient<IOwnerCollection, OwnerCollection>();
    }
}
