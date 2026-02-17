namespace Comanda.Payments.Application.Handlers.Payment;

public sealed class PaymentsFetchHandler(IPaymentCollection paymentCollection) :
    IDispatchHandler<PaymentsFetchParameters, Result<PaginationScheme<PaymentScheme>>>
{
    public async Task<Result<PaginationScheme<PaymentScheme>>> HandleAsync(
        PaymentsFetchParameters parameters, CancellationToken cancellation = default)
    {
        var filters = PaymentFilters.WithSpecifications()
            .WithIdentifier(parameters.Id)
            .WithPayerId(parameters.PayerId)
            .WithExternalId(parameters.ExternalId)
            .WithReferenceId(parameters.ReferenceId)
            .WithStatus(parameters.Status)
            .WithMethod(parameters.Method)
            .WithSort(parameters.Sort)
            .WithCursor(parameters.Cursor)
            .WithMinAmount(parameters.MinAmount)
            .WithMaxAmount(parameters.MaxAmount)
            .WithCreatedAfter(parameters.CreatedAfter)
            .WithCreatedBefore(parameters.CreatedBefore)
            .Build();

        var payments = await paymentCollection.FilterPaymentsAsync(filters, cancellation);
        var nextCursor = payments.Count == filters.Cursor?.Limit
            ? CursorEncoder.Encode(payments.Last())
            : string.Empty;

        var pagination = new PaginationScheme<PaymentScheme>
        {
            Items = payments.Select(payment => payment.AsResponse()).ToArray(),
            Next = nextCursor,
            Limit = filters.Cursor?.Limit ?? 20,
        };

        return Result<PaginationScheme<PaymentScheme>>.Success(pagination);
    }
}
