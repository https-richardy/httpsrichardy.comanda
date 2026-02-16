namespace Comanda.Stores.Domain.Filtering.Builders;

public sealed class ProductFiltersBuilder :
    FiltersBuilderBase<ProductFilters, ProductFiltersBuilder>
{
    public ProductFiltersBuilder WithEstablishmentId(string? establishmentId)
    {
        _filters.EstablishmentId = establishmentId;
        return this;
    }

    public ProductFiltersBuilder WithTitle(string? title)
    {
        _filters.Title = title;
        return this;
    }

    public ProductFiltersBuilder WithMinPrice(decimal? minPrice)
    {
        _filters.MinPrice = minPrice;
        return this;
    }

    public ProductFiltersBuilder WithMaxPrice(decimal? maxPrice)
    {
        _filters.MaxPrice = maxPrice;
        return this;
    }
}
