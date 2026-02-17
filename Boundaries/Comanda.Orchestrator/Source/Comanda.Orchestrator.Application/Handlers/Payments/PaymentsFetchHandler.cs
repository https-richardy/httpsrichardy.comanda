namespace Comanda.Orchestrator.Application.Handlers.Payments;

public sealed class PaymentsFetchHandler(IPaymentGateway paymentGateway) :
    IDispatchHandler<PaymentsFetchParameters, Result<PaginationScheme<PaymentScheme>>>
{
    public async Task<Result<PaginationScheme<PaymentScheme>>> HandleAsync(
        PaymentsFetchParameters parameters, CancellationToken cancellation = default)
    {
        return await paymentGateway.GetPaymentsAsync(parameters, cancellation);
    }
}
