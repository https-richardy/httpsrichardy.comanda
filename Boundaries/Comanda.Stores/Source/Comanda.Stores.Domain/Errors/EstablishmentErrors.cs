namespace Comanda.Stores.Domain.Errors;

public static class EstablishmentErrors
{
    public static readonly Error OwnerAlreadyHasEstablishment = new(
        Code: "#COMANDA-ERROR-84F47",
        Description: "the specified owner already has a registered establishment."
    );

    public static readonly Error EstablishmentDoesNotExist = new(
        Code: "#COMANDA-ERROR-0A2DF",
        Description: "the specified establishment does not exist."
    );
}
