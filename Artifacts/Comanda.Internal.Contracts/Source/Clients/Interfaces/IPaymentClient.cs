namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface IPaymentClient
{
    public Task<Result<PaginationScheme<PaymentScheme>>> GetPaymentsAsync(
        PaymentsFetchParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<CheckoutSession>> CreateOnlineChargeAsync(
        CheckoutSessionCreationScheme parameters,
        Credential credential,
        CancellationToken cancellation = default
    );

    public Task<Result<PaymentScheme>> CreateLocalPaymentAsync(
        OfflinePaymentScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<PaymentScheme>> UpdatePaymentStatusAsync(
        PaymentStatusUpdateScheme parameters,
        CancellationToken cancellation = default
    );
}
