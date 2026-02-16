namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface ICustomerClient
{
    public Task<Result<PaginationScheme<CustomerScheme>>> GetCustomersAsync(
        FetchCustomersParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<CustomerScheme>> CreateCustomerAsync(
        CustomerCreationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<CustomerScheme>> EditCustomerAsync(
        EditCustomerScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result> DeleteCustomerAsync(
        CustomerDeletionScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<IReadOnlyCollection<Address>>> GetCustomerAddressAsync(
        FetchCustomerAddressesParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<Address>> AssignCustomerAddressAsync(
        AssignCustomerAddressScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<Address>> EditCustomerAddressAsync(
        EditCustomerAddressScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result> DeleteCustomerAddressAsync(
        DeleteCustomerAddressScheme parameters,
        CancellationToken cancellation = default
    );
}
