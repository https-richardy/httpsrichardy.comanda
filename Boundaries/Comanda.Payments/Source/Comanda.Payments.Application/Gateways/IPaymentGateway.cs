namespace Comanda.Payments.Application.Gateways;

public interface IPaymentGateway
{
    public Task<Result<CheckoutSession>> CreateCheckoutSessionAsync(
        CheckoutSessionCreationScheme parameters,
        Payment payment,
        CancellationToken cancellation = default
    );
}
