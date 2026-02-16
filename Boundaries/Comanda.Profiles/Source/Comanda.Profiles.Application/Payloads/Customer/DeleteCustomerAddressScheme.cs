namespace Comanda.Profiles.Application.Payloads.Customer;

public sealed record DeleteCustomerAddressScheme : IDispatchable<Result>
{
    [property: JsonIgnore] // to be set from route parameter
    public string CustomerId { get; init; } = default!;
    public Address Target { get; init; } = default!;
}
