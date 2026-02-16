namespace Comanda.Profiles.Domain.Filtering;

public sealed class OwnerFilters : Filters
{
    public string? Name { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public static OwnerFiltersBuilder WithSpecifications() => new();
}
