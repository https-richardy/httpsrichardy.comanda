namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record ClientScheme
{
    public string Id { get; init; }
    public string ClientId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}
