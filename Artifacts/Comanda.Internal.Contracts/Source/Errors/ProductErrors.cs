namespace Comanda.Internal.Contracts.Errors;

public static class ProductErrors
{
    public static readonly Error ProductDoesNotExist = new(
        Code: "#COMANDA-ERROR-C2C1B",
        Description: "the specified product does not exist."
    );

    public static readonly Error ProductBelongsToAnotherEstablishment = new(
        Code: "#COMANDA-ERROR-9FF68",
        Description: "the specified product belongs to another establishment."
    );
}
