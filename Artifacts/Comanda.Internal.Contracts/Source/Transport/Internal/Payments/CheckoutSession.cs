namespace Comanda.Internal.Contracts.Transport.Internal.Payments;

public sealed record CheckoutSession
{
    public string Code { get; init; } = default!;
    public string QrCode { get; init; } = default!;
}
