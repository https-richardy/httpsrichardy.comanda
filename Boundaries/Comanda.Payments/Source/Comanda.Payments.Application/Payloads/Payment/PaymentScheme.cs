namespace Comanda.Payments.Application.Payloads.Payment;

public sealed record PaymentScheme
{
    public string Identifier { get; init; } = default!;
    public string Reference { get; init; } = default!;
    public decimal Amount { get; init; } = default!;

    public User Payer { get; init; } = default!;
    public Status Status { get; init; } = default!;
    public Method Method { get; init; } = default!;
}
