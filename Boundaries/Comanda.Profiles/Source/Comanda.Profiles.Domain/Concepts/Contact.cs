namespace Comanda.Profiles.Domain.Concepts;

public sealed record Contact(string Email, string PhoneNumber) : IValueObject<Contact>
{
    public string Email { get; init; } = Email;
    public string PhoneNumber { get; init; } = PhoneNumber;
}
