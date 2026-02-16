namespace Comanda.Profiles.Application.Payloads.Customer;

public sealed record AssignCustomerAddressScheme : IDispatchable<Result<Address>>
{
    [property: JsonIgnore]
    public string CustomerId { get; init; } = default!;

    public string Street { get; init; } = default!;
    public string Number { get; init; } = default!;

    public string City { get; init; } = default!;
    public string State { get; init; } = default!;

    public string ZipCode { get; init; } = default!;
    public string Neighborhood { get; init; } = default!;

    public string? Complement { get; init; } = default!;
    public string? Reference { get; init; } = default!;
}
