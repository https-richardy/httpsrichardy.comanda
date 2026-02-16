namespace Comanda.Stores.Infrastructure.Persistence;

public sealed class CredentialCollection(IMongoDatabase database) :
    AggregateCollection<Credential>(database, Collections.Credentials),
    ICredentialCollection;
