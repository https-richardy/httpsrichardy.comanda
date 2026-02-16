namespace Comanda.Internal.Contracts.Transport.External.AbacatePay;

public sealed record BillingPaidData
{
    public PixQrCodeData PixQrCode { get; init; } = default!;
    public PaymentData Payment { get; init; } = default!;
}
