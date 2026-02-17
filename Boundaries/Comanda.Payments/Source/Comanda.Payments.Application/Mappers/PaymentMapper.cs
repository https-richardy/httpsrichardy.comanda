namespace Comanda.Payments.Application.Mappers;

public static class PaymentMapper
{
    public static Payment AsPayment(this CheckoutSessionCreationScheme parameters) => new()
    {
        Amount = parameters.Amount,
        Status = Status.Pending,
        Method = Method.Pix,
        Metadata = new PaymentMetadata(Identifier: string.Empty, Reference: parameters.Reference),
        Payer = new User(parameters.Payer.Identifier, parameters.Payer.Username),
    };

    public static Payment AsPayment(this OfflinePaymentChargeScheme parameters) => new()
    {
        Amount = parameters.Amount,
        Method = parameters.Method,
        Status = Status.Paid,

        Payer = new User(
            Identifier: string.Empty,
            Username: string.Empty
        ),

        Metadata = new PaymentMetadata(
            Identifier: string.Empty,
            Reference: parameters.Reference
        )
    };

    public static PaymentScheme AsResponse(this Payment payment) => new()
    {
        Identifier = payment.Id,
        Reference = payment.Metadata.Reference,
        Amount = payment.Amount,
        Payer = payment.Payer,
        Status = payment.Status,
        Method = payment.Method
    };
}
