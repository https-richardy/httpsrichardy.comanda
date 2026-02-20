namespace Comanda.Merchants.WebUI.Http.Payloads.Identity;

public sealed record PrincipalScheme
{
    public string Id { get; init; } = default!;
    public string Username { get; init; } = default!;

    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public IReadOnlyCollection<PermissionDetails> Permissions { get; init; } = [];
    public IReadOnlyCollection<GroupBasicDetails> Groups { get; init; } = [];
}
