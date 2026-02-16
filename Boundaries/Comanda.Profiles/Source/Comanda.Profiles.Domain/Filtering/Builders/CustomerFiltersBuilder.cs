namespace Comanda.Profiles.Domain.Filtering.Builders;

public sealed class CustomerFiltersBuilder :
    FiltersBuilderBase<CustomerFilters, CustomerFiltersBuilder>
{
    public CustomerFiltersBuilder WithName(string? name)
    {
        _filters.Name = name;
        return this;
    }

    public CustomerFiltersBuilder WithEmail(string? email)
    {
        _filters.Email = email;
        return this;
    }

    public CustomerFiltersBuilder WithPhoneNumber(string? phoneNumber)
    {
        _filters.PhoneNumber = phoneNumber;
        return this;
    }

    public CustomerFiltersBuilder WithUserId(string? userId)
    {
        _filters.UserId = userId;
        return this;
    }
}
