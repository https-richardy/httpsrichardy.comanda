namespace Comanda.Profiles.Application.Payloads.Customer;

public sealed record CustomerCreationScheme : IDispatchable<Result<CustomerScheme>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    public string UserId { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
}
