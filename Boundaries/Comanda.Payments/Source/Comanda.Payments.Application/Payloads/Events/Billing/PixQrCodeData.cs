namespace Comanda.Payments.Application.Payloads.Events.Billing;

public sealed record PixQrCodeData
{
    public string Id { get; init; } = default!;
    public decimal Amount { get; init; }
    public string Kind { get; init; } = default!;
    public string Status { get; init; } = default!;
    public Dictionary<string, string> Metadata { get; init; } = [];
}
