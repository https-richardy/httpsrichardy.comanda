namespace Comanda.Stores.Application.Payloads.Establishment;

public sealed record CredentialScheme
{
    public string Identifier { get; init; } = default!;
    public string Provider { get; init; } = default!;
    public string SecretKey { get; init; } = default!;
}
