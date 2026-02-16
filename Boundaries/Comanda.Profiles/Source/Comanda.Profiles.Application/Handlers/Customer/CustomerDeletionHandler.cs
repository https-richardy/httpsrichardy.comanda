namespace Comanda.Profiles.Application.Handlers.Customer;

public sealed class CustomerDeletionHandler(ICustomerCollection collection) :
    IDispatchHandler<CustomerDeletionScheme, Result>
{
    public async Task<Result> HandleAsync(CustomerDeletionScheme parameters, CancellationToken cancellation = default)
    {
        var filters = CustomerFilters.WithSpecifications()
             .WithIdentifier(parameters.CustomerId)
             .WithIsDeleted(false)
             .Build();

        var matchingCustomers = await collection.GetCustomersAsync(filters, cancellation);
        var existingCustomer = matchingCustomers.FirstOrDefault();

        if (existingCustomer is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            return Result.Failure(CustomerErrors.CustomerDoesNotExist);
        }

        await collection.DeleteAsync(existingCustomer, cancellation: cancellation);

        return Result.Success();
    }
}
