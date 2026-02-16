namespace Comanda.Profiles.Application.Payloads.Customer;

public sealed record EditCustomerScheme : IDispatchable<Result<CustomerScheme>>
{
    [property: JsonIgnore] // to be set from route parameter
    public string CustomerId { get; init; } = default!;

    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
