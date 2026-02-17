namespace Comanda.Payments.Application.Payloads.Payment;

public sealed record CheckoutSession
{
    public string Code { get; init; } = default!;
    public string QrCode { get; init; } = default!;
}
