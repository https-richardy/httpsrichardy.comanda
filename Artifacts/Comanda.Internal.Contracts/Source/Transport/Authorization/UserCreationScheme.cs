namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record UserCreationScheme
{
    public string Username { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public bool Enabled { get; init; } = true;

    public IReadOnlyCollection<CredentialScheme> Credentials { get; init; } = [];
    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Attributes { get; init; } =
        new Dictionary<string, IReadOnlyCollection<string>>();

    public UserCreationScheme WithUsername(string username) =>
        this with { Username = username };

    public UserCreationScheme WithFirstName(string firstName) =>
        this with { FirstName = firstName };

    public UserCreationScheme WithLastName(string lastName) =>
        this with { LastName = lastName };

    public UserCreationScheme WithEmail(string email) =>
        this with { Email = email };

    public UserCreationScheme WithEnabled(bool enabled) =>
        this with { Enabled = enabled };

    public UserCreationScheme WithCredentials(IReadOnlyCollection<CredentialScheme> credentials) =>
        this with { Credentials = credentials };

    public UserCreationScheme WithPassword(string password) =>
        this with { Credentials = [new CredentialScheme().WithValue(password)] };

    public UserCreationScheme WithOrganizationId(string organizationId) =>
        this with
        {
            Attributes = new Dictionary<string, IReadOnlyCollection<string>>(Attributes)
            {
                ["organization_id"] = [organizationId]
            }
        };
}
