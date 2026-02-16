namespace Comanda.Stores.Domain.Filtering;

public sealed class EstablishmentFilters : Filters
{
    public string? OwnerId { get; set; }
    public string? Title { get; set; }

    public static EstablishmentFiltersBuilder WithSpecifications() => new();
}
