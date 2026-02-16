namespace Comanda.Profiles.Application.Mappers;

public static class OwnerMapper
{
    public static Owner AsOwner(this OwnerCreationScheme owner)
    {
        var user = new User(
            Identifier: owner.UserId,
            Username: owner.Username.Trim().ToLowerInvariant()
        );

        var contact = new Contact(
            Email: owner.Email.Trim().ToLowerInvariant(),
            PhoneNumber: owner.PhoneNumber.Trim().SanitizeNumbers()
        );

        return new Owner
        {
            FirstName = owner.FirstName.Trim(),
            LastName = owner.LastName.Trim(),
            User = user,
            Contact = contact
        };
    }

    public static Owner AsOwner(this EditOwnerScheme scheme, Owner existingOwner)
    {
        existingOwner.FirstName = scheme.FirstName.Trim();
        existingOwner.LastName = scheme.LastName.Trim();

        existingOwner.Contact = new Contact(
            Email: scheme.Email.Trim().ToLowerInvariant(),
            PhoneNumber: scheme.PhoneNumber.Trim().SanitizeNumbers()
        );

        return existingOwner;
    }

    public static OwnerScheme AsResponse(this Owner owner) => new()
    {
        Identifier = owner.Id,

        FirstName = owner.FirstName,
        LastName = owner.LastName,

        Email = owner.Contact.Email,
        PhoneNumber = owner.Contact.PhoneNumber,

        UserId = owner.User.Identifier,
        Username = owner.User.Username
    };
}
