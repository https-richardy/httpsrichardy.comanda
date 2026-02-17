namespace Comanda.Orchestrator.Infrastructure.Gateways;

public sealed class ProfilesGateway(ICustomerClient customerClient, IOwnerClient ownerClient, ILogger<IProfilesGateway> logger) : IProfilesGateway
{
    private readonly AsyncPolicyWrap<Result<CustomerScheme>> _customerPolicy =
        PollyPolicies.CreatePolicy<CustomerScheme>(logger);

    private readonly AsyncPolicyWrap<Result<OwnerScheme>> _ownerPolicy =
        PollyPolicies.CreatePolicy<OwnerScheme>(logger);

    private readonly AsyncPolicyWrap<Result<PaginationScheme<OwnerScheme>>> _fetchOwnersPolicy =
        PollyPolicies.CreatePolicy<PaginationScheme<OwnerScheme>>(logger);

    private readonly AsyncPolicyWrap<Result<OwnerScheme>> _editOwnerPolicy =
        PollyPolicies.CreatePolicy<OwnerScheme>(logger);

    private readonly AsyncPolicyWrap<Result> _deleteOwnerPolicy =
        PollyPolicies.CreatePolicy(logger);

    private readonly AsyncPolicyWrap<Result<PaginationScheme<CustomerScheme>>> _fetchCustomersPolicy =
        PollyPolicies.CreatePolicy<PaginationScheme<CustomerScheme>>(logger);

    private readonly AsyncPolicyWrap<Result<CustomerScheme>> _editCustomerPolicy =
        PollyPolicies.CreatePolicy<CustomerScheme>(logger);

    private readonly AsyncPolicyWrap<Result> _deleteCustomerPolicy =
        PollyPolicies.CreatePolicy(logger);

    private readonly AsyncPolicyWrap<Result<IReadOnlyCollection<Address>>> _fetchCustomerAddressesPolicy =
        PollyPolicies.CreatePolicy<IReadOnlyCollection<Address>>(logger);

    private readonly AsyncPolicyWrap<Result<Address>> _assignCustomerAddressPolicy =
        PollyPolicies.CreatePolicy<Address>(logger);

    private readonly AsyncPolicyWrap<Result<Address>> _editCustomerAddressPolicy =
        PollyPolicies.CreatePolicy<Address>(logger);

    private readonly AsyncPolicyWrap<Result> _deleteCustomerAddressPolicy =
        PollyPolicies.CreatePolicy(logger);

    public async Task<Result<CustomerScheme>> CreateCustomerAsync(
        CustomerCreationScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/

        return await _customerPolicy.ExecuteAsync(token =>
            customerClient.CreateCustomerAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<OwnerScheme>> CreateOwnerAsync(
        OwnerCreationScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/

        return await _ownerPolicy.ExecuteAsync(token =>
            ownerClient.CreateOwnerAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<PaginationScheme<OwnerScheme>>> GetOwnersAsync(
        FetchOwnersParameters parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/

        return await _fetchOwnersPolicy.ExecuteAsync(token =>
            ownerClient.GetOwnersAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<OwnerScheme>> UpdateOwnerAsync(
        EditOwnerScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/

        return await _editOwnerPolicy.ExecuteAsync(token =>
            ownerClient.UpdateOwnerAsync(parameters, token), cancellation
        );
    }

    public async Task<Result> DeleteOwnerAsync(
        OwnerDeletionScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _deleteOwnerPolicy.ExecuteAsync(token =>
            ownerClient.DeleteOwnerAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<PaginationScheme<CustomerScheme>>> GetCustomersAsync(
        FetchCustomersParameters parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _fetchCustomersPolicy.ExecuteAsync(token =>
            customerClient.GetCustomersAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<CustomerScheme>> EditCustomerAsync(
        EditCustomerScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _editCustomerPolicy.ExecuteAsync(token =>
            customerClient.EditCustomerAsync(parameters, token), cancellation
        );
    }

    public async Task<Result> DeleteCustomerAsync(
        CustomerDeletionScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _deleteCustomerPolicy.ExecuteAsync(token =>
            customerClient.DeleteCustomerAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<IReadOnlyCollection<Address>>> GetCustomerAddressAsync(
        FetchCustomerAddressesParameters parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _fetchCustomerAddressesPolicy.ExecuteAsync(token =>
            customerClient.GetCustomerAddressAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<Address>> AssignCustomerAddressAsync(
        AssignCustomerAddressScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _assignCustomerAddressPolicy.ExecuteAsync(token =>
            customerClient.AssignCustomerAddressAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<Address>> EditCustomerAddressAsync(
        EditCustomerAddressScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _editCustomerAddressPolicy.ExecuteAsync(token =>
            customerClient.EditCustomerAddressAsync(parameters, token), cancellation
        );
    }

    public async Task<Result> DeleteCustomerAddressAsync(
        DeleteCustomerAddressScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _deleteCustomerAddressPolicy.ExecuteAsync(token =>
            customerClient.DeleteCustomerAddressAsync(parameters, token), cancellation
        );
    }
}
