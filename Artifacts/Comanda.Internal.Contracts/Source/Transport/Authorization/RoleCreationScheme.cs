namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record RoleCreationScheme
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string ClientIdentifier { get; init; }

    public RoleCreationScheme WithName(string name) =>
        this with { Name = name };

    public RoleCreationScheme WithDescription(string description) =>
        this with { Description = description };

    public RoleCreationScheme WithClientIdentifier(string clientIdentifier) =>
        this with { ClientIdentifier = clientIdentifier };
}
