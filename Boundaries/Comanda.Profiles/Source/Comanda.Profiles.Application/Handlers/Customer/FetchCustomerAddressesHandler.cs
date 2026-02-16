namespace Comanda.Profiles.Application.Handlers.Customer;

public sealed class FetchCustomerAddressesHandler(ICustomerCollection collection) :
    IDispatchHandler<FetchCustomerAddressesParameters, Result<IReadOnlyCollection<Address>>>
{
    public async Task<Result<IReadOnlyCollection<Address>>> HandleAsync(
        FetchCustomerAddressesParameters parameters, CancellationToken cancellation = default)
    {
        var filters = CustomerFilters.WithSpecifications()
            .WithIdentifier(parameters.CustomerId)
            .Build();

        var matchingCustomers = await collection.GetCustomersAsync(filters, cancellation);
        var customer = matchingCustomers.FirstOrDefault();

        if (customer is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            /* this allows us to quickly locate all occurrences of this error in the codebase. */

            return Result<IReadOnlyCollection<Address>>.Failure(CustomerErrors.CustomerDoesNotExist);
        }

        return Result<IReadOnlyCollection<Address>>.Success([.. customer.Addresses]);
    }
}
