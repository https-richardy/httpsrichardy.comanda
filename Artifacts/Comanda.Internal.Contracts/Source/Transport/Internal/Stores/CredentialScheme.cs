namespace Comanda.Internal.Contracts.Transport.Internal.Stores;

public sealed record CredentialScheme
{
    public string Identifier { get; init; } = default!;
    public string Provider { get; init; } = default!;
    public string SecretKey { get; init; } = default!;
}
