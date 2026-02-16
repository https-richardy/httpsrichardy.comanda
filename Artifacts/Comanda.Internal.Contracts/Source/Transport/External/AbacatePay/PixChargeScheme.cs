namespace Comanda.Internal.Contracts.Transport.External.AbacatePay;

public sealed record PixChargeScheme
{
    public decimal Amount { get; init; }
    public int ExpiresIn { get; init; }
    public Dictionary<string, string> Metadata { get; init; } = [];
}
