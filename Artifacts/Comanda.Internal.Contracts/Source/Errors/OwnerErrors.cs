namespace Comanda.Internal.Contracts.Errors;

public static class OwnerErrors
{
    public static readonly Error OwnerDoesNotExist = new(
        Code: "#COMANDA-ERROR-0831D",
        Description: "The specified owner does not exist."
    );
}
