namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record FetchCustomersParameters :
    IDispatchable<Result<PaginationScheme<CustomerScheme>>>
{
    public string? CustomerId { get; init; }
    public string? UserId { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public bool? IsDeleted { get; init; }

    public PaginationFilters? Pagination { get; init; }
    public SortFilters? Sort { get; init; }

    public DateOnly? CreatedAfter { get; init; }
    public DateOnly? CreatedBefore { get; init; }
}
