namespace Comanda.Stores.Domain.Filtering;

public sealed class ProductFilters : Filters
{
    public string? EstablishmentId { get; set; }
    public string? Title { get; set; }

    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public static ProductFiltersBuilder WithSpecifications() => new();
}
