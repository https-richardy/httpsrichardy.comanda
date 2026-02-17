namespace Comanda.Orders.Domain.Filtering;

public sealed class OrderFilters : Filters
{
    public string? Code { get; set; }
    public string? MerchantId { get; set; }
    public string? ConsumerId { get; set; }

    public Priority? Priority { get; set; }
    public Fulfillment? Fulfillment { get; set; }
    public Status? Status { get; set; }

    public static OrderFiltersBuilder WithSpecifications() => new();
}
