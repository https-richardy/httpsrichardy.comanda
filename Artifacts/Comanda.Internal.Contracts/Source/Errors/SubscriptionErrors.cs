namespace Comanda.Internal.Contracts.Errors;

public static class SubscriptionErrors
{
    public static readonly Error PlanNotSupported = new(
        Code: "#COMANDA-ERROR-D910F",
        Description: "The selected plan is currently not available."
    );

    public static readonly Error SubscriptionDoesNotExist = new(
        Code: "#COMANDA-ERROR-DF352",
        Description: "No subscription record was found for the specified criteria."
    );

    public static readonly Error SessionDoesNotExist = new(
        Code: "#COMANDA-ERROR-7F89A",
        Description: "The specified checkout session could not be found or is no longer available."
    );

    public static readonly Error SubscriptionAlreadyCanceled = new(
        Code: "#COMANDA-ERROR-06F07",
        Description: "The subscription has already been canceled and cannot be modified."
    );
}
