namespace Comanda.Orders.Domain.Errors;

public static class OrderErrors
{
    public static readonly Error OrderDoesNotExist = new(
        Code: "#COMANDA-ERROR-2D7A5",
        Description: "The specified order does not exist."
    );
}
