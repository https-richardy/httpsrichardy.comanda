namespace Comanda.Internal.Contracts.Transport.External.AbacatePay;

public sealed record PaymentData
{
    public decimal Amount { get; init; }
    public decimal Fee { get; init; }
    public string Method { get; init; } = default!;
}
