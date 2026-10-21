namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record RoleScheme
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ClientIdentifier { get; init; }
}
