namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record EditCustomerAddressScheme : IDispatchable<Result<Address>>
{
    [property: JsonIgnore]
    public string CustomerId { get; init; } = default!;

    public Address Target { get; init; } = default!;
    public Address Replacement { get; init; } = default!;
}
