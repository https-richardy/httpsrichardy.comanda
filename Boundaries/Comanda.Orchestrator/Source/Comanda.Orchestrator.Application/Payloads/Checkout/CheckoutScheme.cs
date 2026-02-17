namespace Comanda.Orchestrator.Application.Payloads.Checkout;

public sealed record CheckoutScheme
{
    public string OrderId { get; init; } = default!;
    public string OrderCode { get; init; } = default!;

    public string PixCode { get; init; } = default!;
    public string QrCode { get; init; } = default!;
}
