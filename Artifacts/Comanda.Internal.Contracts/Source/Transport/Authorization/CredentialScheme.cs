namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record CredentialScheme
{
    public string Type { get; init; } = "password";
    public string Value { get; init; }
    public bool Temporary { get; init; } = false;

    public CredentialScheme WithType(string type) =>
        this with { Type = type };

    public CredentialScheme WithValue(string value) =>
        this with { Value = value };

    public CredentialScheme WithTemporary(bool temporary) =>
        this with { Temporary = temporary };
}
