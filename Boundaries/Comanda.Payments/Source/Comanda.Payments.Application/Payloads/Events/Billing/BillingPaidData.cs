namespace Comanda.Payments.Application.Payloads.Events.Billing;

public sealed record BillingPaidData
{
    public PixQrCodeData PixQrCode { get; init; } = default!;
    public PaymentData Payment { get; init; } = default!;
}
