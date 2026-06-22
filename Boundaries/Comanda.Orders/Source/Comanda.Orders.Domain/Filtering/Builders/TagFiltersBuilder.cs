namespace Comanda.Orders.Domain.Filtering.Builders;

public sealed class TagFiltersBuilder :
    FiltersBuilderBase<TagFilters, TagFiltersBuilder>
{
    public TagFiltersBuilder WithCode(string? code)
    {
        _filters.Code = code;
        return this;
    }

    public TagFiltersBuilder WithMerchantId(string? merchantId)
    {
        _filters.MerchantId = merchantId;
        return this;
    }
}
