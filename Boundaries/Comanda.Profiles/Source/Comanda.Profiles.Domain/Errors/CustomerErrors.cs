namespace Comanda.Profiles.Domain.Errors;

public static class CustomerErrors
{
    public static readonly Error CustomerDoesNotExist = new(
        Code: "#COMANDA-ERROR-AF04C",
        Description: "The specified customer does not exist."
    );

    public static readonly Error AddressAlreadyAssigned = new(
        Code: "#COMANDA-ERROR-4901F",
        Description: "This address is already assigned to the customer."
    );

    public static readonly Error AddressDoesNotExist = new(
        Code: "#COMANDA-ERROR-2616B",
        Description: "The target address does not exist for this customer."
    );
}
