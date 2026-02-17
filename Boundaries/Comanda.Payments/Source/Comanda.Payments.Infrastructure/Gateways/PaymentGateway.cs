namespace Comanda.Payments.Infrastructure.Gateways;

public sealed class PaymentGateway(IAbacatePayClient paymentClient) : IPaymentGateway
{
    public async Task<Result<CheckoutSession>> CreateCheckoutSessionAsync(
        CheckoutSessionCreationScheme parameters, Payment payment, CancellationToken cancellation = default)
    {
        var operation = await paymentClient.CreateChargeSessionAsync(parameters.ToRequest(payment), cancellation);
        if (operation.IsFailure || operation.Data is null)
        {
            return Result<CheckoutSession>.Failure(operation.Error);
        }

        var session = operation.Data;
        var result = new CheckoutSession
        {
            Code = session.BrCode,
            QrCode = session.BrCodeBase64
        };

        return Result<CheckoutSession>.Success(result);
    }
}
