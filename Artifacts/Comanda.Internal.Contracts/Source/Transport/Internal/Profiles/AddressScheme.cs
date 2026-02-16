namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record Address
{
    public string Street { get; init; } = default!;
    public string Number { get; init; } = default!;
    public string City { get; init; } = default!;

    public string State { get; init; } = default!;
    public string ZipCode { get; init; } = default!;
    public string Neighborhood { get; init; } = default!;

    public string? Complement { get; init; }
    public string? Reference { get; init; }
}
