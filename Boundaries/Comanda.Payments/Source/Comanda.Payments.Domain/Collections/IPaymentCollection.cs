namespace Comanda.Payments.Domain.Collections;

public interface IPaymentCollection : IAggregateCollection<Payment>
{
    public Task<IReadOnlyCollection<Payment>> FilterPaymentsAsync(
        PaymentFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountPaymentsAsync(
        PaymentFilters filters,
        CancellationToken cancellation = default
    );
}
