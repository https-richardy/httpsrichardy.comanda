namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record OwnerCreationScheme : IMessage<Result<OwnerScheme>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    public string UserId { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
}
