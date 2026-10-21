namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record PatchGroupScheme
{
    public string Id { get; init; }
    public string? Name { get; init; }

    public PatchGroupScheme WithId(string id) =>
        this with { Id = id };

    public PatchGroupScheme WithName(string? name) =>
        this with { Name = name };
}
