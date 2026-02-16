namespace Comanda.Profiles.Application.Handlers.Customer;

public sealed class EditCustomerAddressHandler(ICustomerCollection collection) :
    IDispatchHandler<EditCustomerAddressScheme, Result<Address>>
{
    public async Task<Result<Address>> HandleAsync(
        EditCustomerAddressScheme parameters, CancellationToken cancellation = default)
    {
        var filters = CustomerFilters.WithSpecifications()
             .WithIdentifier(parameters.CustomerId)
             .Build();

        var customers = await collection.GetCustomersAsync(filters, cancellation);
        var customer = customers.FirstOrDefault();

        if (customer is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            return Result<Address>.Failure(CustomerErrors.CustomerDoesNotExist);
        }

        var existingAddress = customer.Addresses.FirstOrDefault(address => address.Equals(parameters.Target));
        if (existingAddress is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-2616B */
            return Result<Address>.Failure(CustomerErrors.AddressDoesNotExist);
        }

        if (existingAddress.Equals(parameters.Replacement))
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-4901F */
            return Result<Address>.Failure(CustomerErrors.AddressAlreadyAssigned);
        }

        customer.Addresses.Remove(existingAddress);
        customer.Addresses.Add(parameters.Replacement);

        await collection.UpdateAsync(customer, cancellation);

        return Result<Address>.Success(parameters.Replacement);
    }
}
