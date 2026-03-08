namespace Comanda.Payments.TestSuite.Mocks;

public sealed class PaymentGatewayMock : IPaymentGateway
{
    public Task<Result<CheckoutSession>> CreateCheckoutSessionAsync(
        CheckoutSessionCreationScheme parameters, Payment payment, CancellationToken cancellation = default)
    {
        var session = new CheckoutSession
        {
            Code = $"brcode_{Guid.NewGuid():N}",
            QrCode = $"qrcode_{Guid.NewGuid():N}"
        };

        return Task.FromResult(Result<CheckoutSession>.Success(session));
    }
}
