namespace Comanda.Stores.Domain.Errors;

public sealed record CredentialErrors
{
    public static readonly Error CredentialDoesNotExist = new(
        Code: "#COMANDA-ERROR-2147F",
        Description: "the specified credential does not exist."
    );
}
