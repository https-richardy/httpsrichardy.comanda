namespace Comanda.Profiles.Application.Payloads.Customer;

public sealed record FetchCustomerAddressesParameters :
    IDispatchable<Result<IReadOnlyCollection<Address>>>
{
    public string CustomerId { get; init; } = default!;
}
