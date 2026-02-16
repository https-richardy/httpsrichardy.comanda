namespace Comanda.Internal.Contracts.Errors;

public static class PaymentErrors
{
    public static readonly Error MethodNotAllowed = new(
        Code: "#COMANDA-ERROR-947B5",
        Description: "the specified payment method is not allowed for this type of transaction"
    );

    public static readonly Error PaymentDoesNotExist = new(
        Code: "#COMANDA-ERROR-4A42E",
        Description: "the specified payment does not exist"
    );
}
