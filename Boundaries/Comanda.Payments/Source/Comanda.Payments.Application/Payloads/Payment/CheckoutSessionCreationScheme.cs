namespace Comanda.Payments.Application.Payloads.Payment;

public sealed record CheckoutSessionCreationScheme :
    IDispatchable<Result<CheckoutSession>>
{
    public decimal Amount { get; init; } = default!;
    public string Reference { get; init; } = default!;
    public User Payer { get; init; } = default!;
}
