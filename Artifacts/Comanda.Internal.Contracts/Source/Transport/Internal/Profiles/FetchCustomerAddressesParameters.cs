namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record FetchCustomerAddressesParameters :
    IDispatchable<Result<IReadOnlyCollection<Address>>>
{
    public string CustomerId { get; init; } = default!;
}
