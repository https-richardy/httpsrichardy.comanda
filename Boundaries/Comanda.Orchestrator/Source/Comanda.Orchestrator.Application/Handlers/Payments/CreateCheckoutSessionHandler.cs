namespace Comanda.Orchestrator.Application.Handlers.Payments;

public sealed class CreateCheckoutSessionHandler(IPaymentGateway paymentGateway) :
    IDispatchHandler<OnlineChargeScheme, Result<CheckoutSession>>
{
    public async Task<Result<CheckoutSession>> HandleAsync(
        OnlineChargeScheme parameters, CancellationToken cancellation = default)
    {
        var credential = new Credential(parameters.SecretKey);

        return await paymentGateway.CreateOnlineChargeAsync(parameters.Payload, credential, cancellation);
    }
}
