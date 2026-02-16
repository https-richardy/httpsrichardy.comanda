namespace Comanda.Stores.Domain.Filtering.Builders;

public sealed class EstablishmentFiltersBuilder :
    FiltersBuilderBase<EstablishmentFilters, EstablishmentFiltersBuilder>
{
    public EstablishmentFiltersBuilder WithOwnerId(string? ownerId)
    {
        _filters.OwnerId = ownerId;
        return this;
    }

    public EstablishmentFiltersBuilder WithTitle(string? title)
    {
        _filters.Title = title;
        return this;
    }
}
