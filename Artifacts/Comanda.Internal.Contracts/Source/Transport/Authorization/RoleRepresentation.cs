namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record RoleRepresentation
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ContainerId { get; init; }
}
