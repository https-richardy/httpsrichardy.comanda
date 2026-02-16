namespace Comanda.Internal.Contracts.Errors;

public static class CommonErrors
{
    public static readonly Error InvalidContent = new(
        Code: "#COMANDA-ERROR-F6945",
        Description: "Service has returned invalid or malformed content."
    );

    public static readonly Error OperationFailed = new(
        Code: "#COMANDA-ERROR-60A10",
        Description: "An error occurred while performing the operation with the service."
    );

    public static readonly Error UnauthorizedAccess = new(
        Code: "#COMANDA-ERROR-61CC0",
        Description: "Access denied due to invalid credentials or insufficient permissions."
    );

    public static readonly Error RateLimitExceeded = new(
        Code: "#COMANDA-ERROR-B6688",
        Description: "Too many operations attempted in a short period. Please try again later."
    );
}
