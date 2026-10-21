namespace Comanda.Internal.Contracts.Transport.Authentication;

public sealed record ClientCredentials
{
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }

    public ClientCredentials WithIdentifier(string identifier) =>
        this with { ClientId = identifier };

    public ClientCredentials WithSecret(string secret) =>
        this with { ClientSecret = secret };
}
