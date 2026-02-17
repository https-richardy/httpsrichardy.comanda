namespace Comanda.Payments.Application.Payloads.Events.Billing;

public sealed record PaymentData
{
    public decimal Amount { get; init; }
    public decimal Fee { get; init; }
    public string Method { get; init; } = default!;
}
