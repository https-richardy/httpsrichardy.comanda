namespace Comanda.Profiles.Domain.Filtering.Builders;

public sealed class OwnerFiltersBuilder :
    FiltersBuilderBase<OwnerFilters, OwnerFiltersBuilder>
{
    public OwnerFiltersBuilder WithName(string? name)
    {
        _filters.Name = name;
        return this;
    }

    public OwnerFiltersBuilder WithEmail(string? email)
    {
        _filters.Email = email;
        return this;
    }

    public OwnerFiltersBuilder WithPhoneNumber(string? phoneNumber)
    {
        _filters.PhoneNumber = phoneNumber;
        return this;
    }

    public OwnerFiltersBuilder WithUserId(string? userId)
    {
        _filters.UserId = userId;
        return this;
    }
}
