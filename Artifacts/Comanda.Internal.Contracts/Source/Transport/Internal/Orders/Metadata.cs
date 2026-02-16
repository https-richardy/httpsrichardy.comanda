namespace Comanda.Internal.Contracts.Transport.Internal.Orders;

public sealed record Metadata(string MerchantId, string ConsumerId)
{
    public string MerchantId { get; init; } = MerchantId;
    public string ConsumerId { get; init; } = ConsumerId;
}

