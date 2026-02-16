namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record EditCustomerScheme : IMessage<Result<CustomerScheme>>
{
    [property: JsonIgnore]
    public string CustomerId { get; init; } = default!;

    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
