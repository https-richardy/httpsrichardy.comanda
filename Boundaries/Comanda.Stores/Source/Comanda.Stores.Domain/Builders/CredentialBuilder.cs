namespace Comanda.Stores.Domain.Builders;

public sealed class CredentialBuilder(Credential credential)
{
    private readonly Credential _credential = credential;

    public CredentialBuilder SetProvider(IntegrationTarget provider)
    {
        _credential.Provider = provider;
        return this;
    }

    public CredentialBuilder SetSecretKey(string secretKey)
    {
        _credential.SecretKey = secretKey;
        return this;
    }

    public Credential Build()
    {
        return _credential;
    }
}
