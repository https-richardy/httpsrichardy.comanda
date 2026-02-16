namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record FetchCustomerAddressesParameters :
    IMessage<Result<IReadOnlyCollection<Address>>>
{
    public string CustomerId { get; init; } = default!;
}
