namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record PatchUserScheme
{
    public string Id { get; init; }
    public string? Username { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public bool? Enabled { get; init; }

    public PatchUserScheme WithId(string id) =>
        this with { Id = id };

    public PatchUserScheme WithUsername(string? username) =>
        this with { Username = username };

    public PatchUserScheme WithFirstName(string? firstName) =>
        this with { FirstName = firstName };

    public PatchUserScheme WithLastName(string? lastName) =>
        this with { LastName = lastName };

    public PatchUserScheme WithEmail(string? email) =>
        this with { Email = email };

    public PatchUserScheme WithEnabled(bool? enabled) =>
        this with { Enabled = enabled };
}
