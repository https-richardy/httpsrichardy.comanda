namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record PatchRoleScheme
{
    public string Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }

    public PatchRoleScheme WithId(string id) =>
        this with { Id = id };

    public PatchRoleScheme WithName(string? name) =>
        this with { Name = name };

    public PatchRoleScheme WithDescription(string? description) =>
        this with { Description = description };
}
