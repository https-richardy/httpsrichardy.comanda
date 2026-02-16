namespace Comanda.Profiles.Application.Handlers.Customer;

public sealed class DeleteCustomerAddressHandler(ICustomerCollection collection) :
    IDispatchHandler<DeleteCustomerAddressScheme, Result>
{
    public async Task<Result> HandleAsync(
        DeleteCustomerAddressScheme parameters, CancellationToken cancellation = default)
    {
        var filters = CustomerFilters.WithSpecifications()
             .WithIdentifier(parameters.CustomerId)
             .Build();

        var customers = await collection.GetCustomersAsync(filters, cancellation);
        var customer = customers.FirstOrDefault();

        if (customer is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            return Result.Failure(CustomerErrors.CustomerDoesNotExist);
        }

        var existingAddress = customer.Addresses.FirstOrDefault(address => address.Equals(parameters.Target));
        if (existingAddress is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-2616B */
            return Result.Failure(CustomerErrors.AddressDoesNotExist);
        }

        customer.Addresses.Remove(existingAddress);

        await collection.UpdateAsync(customer, cancellation);

        return Result.Success();
    }
}
