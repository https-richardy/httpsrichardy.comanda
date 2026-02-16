namespace Comanda.Profiles.Application.Handlers.Customer;

public sealed class FetchCustomersHandler(ICustomerCollection collection) :
    IDispatchHandler<FetchCustomersParameters, Result<PaginationScheme<CustomerScheme>>>
{
    public async Task<Result<PaginationScheme<CustomerScheme>>> HandleAsync(
        FetchCustomersParameters parameters, CancellationToken cancellation = default)
    {
        var filters = CustomerFilters.WithSpecifications()
            .WithIdentifier(parameters.CustomerId)
            .WithUserId(parameters.UserId)
            .WithEmail(parameters.Email)
            .WithPhoneNumber(parameters.PhoneNumber)
            .WithIsDeleted(parameters.IsDeleted)
            .WithPagination(parameters.Pagination)
            .WithSort(parameters.Sort)
            .WithCreatedAfter(parameters.CreatedAfter)
            .WithCreatedBefore(parameters.CreatedBefore)
            .Build();

        var customers = await collection.GetCustomersAsync(filters, cancellation);
        var totalCount = await collection.CountCustomersAsync(filters, cancellation);

        var pagination = new PaginationScheme<CustomerScheme>
        {
            Items = [.. customers.Select(customer => CustomerMapper.AsResponse(customer))],
            Total = (int)totalCount,

            PageSize = parameters.Pagination?.PageSize ?? 0,
            PageNumber = parameters.Pagination?.PageNumber ?? 0
        };

        return Result<PaginationScheme<CustomerScheme>>.Success(pagination);
    }
}
