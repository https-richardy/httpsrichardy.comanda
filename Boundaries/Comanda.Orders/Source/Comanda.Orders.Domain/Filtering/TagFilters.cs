namespace Comanda.Orders.Domain.Filtering;

public sealed class TagFilters : Filters
{
    public string? Code { get; set; } = default!;
    public string? MerchantId { get; set; } = default!;

    public static TagFilters WithoutFilters => new();
    public static TagFilters WithSpecifications() => new();
}
