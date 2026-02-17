namespace Comanda.Orders.Domain.Concepts;

public sealed record Metadata(string MerchantId, string ConsumerId) : IValueObject<Metadata>
{
    public string MerchantId { get; init; } = MerchantId;
    public string ConsumerId { get; init; } = ConsumerId;
}
