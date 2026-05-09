namespace Comanda.Subscriptions.CrossCutting.Configurations;

public sealed record FederationSettings
{
    public string ClientId { get; init; } = default!;
    public string ClientSecret { get; init; } = default!;
    public string Realm { get; init; } = default!;
    public string Authority { get; init; } = default!;
    public string[] Audiences { get; init; } = default!;
}
