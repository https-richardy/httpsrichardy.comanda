namespace Comanda.Internal.Contracts.Transport.Internal.Payments;

public sealed record CheckoutSessionCreationScheme :
    IDispatchable<Result<CheckoutSession>>
{
    public decimal Amount { get; init; } = default!;
    public string Reference { get; init; } = default!;
    public User Payer { get; init; } = default!;
}
