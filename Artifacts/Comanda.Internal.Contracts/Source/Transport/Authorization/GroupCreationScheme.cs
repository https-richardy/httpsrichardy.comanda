namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record GroupCreationScheme
{
    public string Name { get; init; }

    public GroupCreationScheme WithName(string name) =>
        this with { Name = name };
}
