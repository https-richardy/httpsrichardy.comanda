namespace Comanda.Profiles.CrossCutting.Configurations;

public sealed record FederationSettings
{
    public string ClientId { get; init; } = default!;
    public string ClientSecret { get; init; } = default!;
    public string Realm { get; init; } = default!;
    public string BaseUrl { get; init; } = default!;
}
