namespace Comanda.Internal.Contracts.Errors;

public static class AbacatePayErrors
{
    public static readonly Error OperationFailed = new(
        Code: "#COMANDA-EXTERNAL-ERROR-8032F",
        Description: "An error occurred while performing the operation with AbacatePay."
    );

    public static readonly Error InvalidContent = new(
        Code: "#COMANDA-EXTERNAL-ERROR-F6945",
        Description: "AbacatePay returned invalid or malformed content."
    );

    public static readonly Error Unknown = new(
        Code: "#COMANDA-EXTERNAL-ERROR-77854",
        Description: "An unknown error occurred with AbacatePay."
    );
}
