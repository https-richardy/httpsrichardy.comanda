namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record UserScheme
{
    public string Id { get; init; }
    public string Username { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }

    public bool Enabled { get; init; }
    public bool EmailVerified { get; init; }
}
