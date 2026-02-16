namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record DeleteCustomerAddressScheme : IMessage<Result>
{
    [property: JsonIgnore]
    public string CustomerId { get; init; } = default!;

    public Address Target { get; init; } = default!;
}
