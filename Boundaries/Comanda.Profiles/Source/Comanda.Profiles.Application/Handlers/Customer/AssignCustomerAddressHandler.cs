namespace Comanda.Profiles.Application.Handlers.Customer;

public sealed class AssignCustomerAddressHandler(ICustomerCollection collection) :
    IDispatchHandler<AssignCustomerAddressScheme, Result<Address>>
{
    public async Task<Result<Address>> HandleAsync(
        AssignCustomerAddressScheme parameters, CancellationToken cancellation = default)
    {
        var filters = CustomerFilters.WithSpecifications()
            .WithIdentifier(parameters.CustomerId)
            .Build();

        var customers = await collection.GetCustomersAsync(filters, cancellation);
        var customer = customers.FirstOrDefault();

        var address = parameters.AsAddress();

        if (customer is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            /* this allows us to quickly locate all occurrences of this error in the codebase. */

            return Result<Address>.Failure(CustomerErrors.CustomerDoesNotExist);
        }

        /* we compare addresses by value (using the record's Equals) instead of by reference as with a regular class. */

        if (customer.Addresses.Any(existing => existing.Equals(address)))
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-4901F */
            /* this allows us to quickly locate all occurrences of this error in the codebase. */

            return Result<Address>.Failure(CustomerErrors.AddressAlreadyAssigned);
        }

        customer.Addresses.Add(address);

        await collection.UpdateAsync(customer, cancellation);

        return Result<Address>.Success(address);
    }
}
