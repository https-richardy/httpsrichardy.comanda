namespace Comanda.Internal.Contracts.Transport.Internal.Payments;

public sealed record OfflinePaymentScheme : IDispatchable<Result<PaymentScheme>>
{
    public string Reference { get; init; } = default!;
    public decimal Amount { get; init; } = default!;
    public Method Method { get; init; } = Method.Unspecified;
}
