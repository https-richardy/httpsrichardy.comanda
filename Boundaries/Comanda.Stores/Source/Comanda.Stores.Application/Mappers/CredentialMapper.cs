namespace Comanda.Stores.Application.Mappers;

public static class CredentialMapper
{
    public static Credential AsCredential(this CredentialCreationScheme credential) => new()
    {
        Provider = credential.Provider,
        SecretKey = credential.SecretKey,
    };

    public static CredentialScheme AsResponse(this Credential credential) => new()
    {
        Identifier = credential.Id,
        Provider = credential.Provider.ToString(),
        SecretKey = credential.SecretKey,
    };
}
