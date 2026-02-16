namespace Comanda.Profiles.Application.Payloads.Customer;

public sealed record EditCustomerAddressScheme : IDispatchable<Result<Address>>
{
    [property: JsonIgnore] // to be set from route parameter
    public string CustomerId { get; init; } = default!;

    public Address Target { get; init; } = default!;
    public Address Replacement { get; init; } = default!;
}
