namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record EditOwnerScheme : IDispatchable<Result<OwnerScheme>>
{
    [property: JsonIgnore] // to be set from route parameter
    public string OwnerId { get; init; } = default!;

    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
