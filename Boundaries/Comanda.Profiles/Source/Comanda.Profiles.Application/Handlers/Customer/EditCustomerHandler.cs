namespace Comanda.Profiles.Application.Handlers.Customer;

public sealed class EditCustomerHandler(ICustomerCollection collection) :
    IDispatchHandler<EditCustomerScheme, Result<CustomerScheme>>
{
    public async Task<Result<CustomerScheme>> HandleAsync(
        EditCustomerScheme parameters, CancellationToken cancellation = default)
    {
        var filters = CustomerFilters.WithSpecifications()
             .WithIdentifier(parameters.CustomerId)
             .Build();

        var matchingCustomers = await collection.GetCustomersAsync(filters, cancellation);
        var existingCustomer = matchingCustomers.FirstOrDefault();

        if (existingCustomer is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            return Result<CustomerScheme>.Failure(CustomerErrors.CustomerDoesNotExist);
        }

        var customer = await collection.UpdateAsync(parameters.AsCustomer(existingCustomer), cancellation);

        return Result<CustomerScheme>.Success(customer.AsResponse());
    }
}
