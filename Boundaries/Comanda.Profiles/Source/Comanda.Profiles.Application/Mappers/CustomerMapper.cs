namespace Comanda.Profiles.Application.Mappers;

public static class CustomerMapper
{
    public static Customer AsCustomer(this CustomerCreationScheme customer) => new()
    {
        FirstName = customer.FirstName.Trim(),
        LastName = customer.LastName.Trim(),

        User = new User(
            Identifier: customer.UserId,
            Username: customer.Username.Trim().ToLowerInvariant()
        ),

        Contact = new Contact(
            Email: customer.Email.Trim().ToLowerInvariant(),
            PhoneNumber: customer.PhoneNumber.Trim().SanitizeNumbers()
        )
    };

    public static Customer AsCustomer(this EditCustomerScheme scheme, Customer existingCustomer)
    {
        existingCustomer.FirstName = scheme.FirstName.Trim();
        existingCustomer.LastName = scheme.LastName.Trim();

        existingCustomer.Contact = new Contact(
            Email: scheme.Email.Trim().ToLowerInvariant(),
            PhoneNumber: scheme.PhoneNumber.Trim().SanitizeNumbers()
        );

        return existingCustomer;
    }

    public static CustomerScheme AsResponse(this Customer customer) => new()
    {
        Identifier = customer.Id,

        FirstName = customer.FirstName,
        LastName = customer.LastName,

        Email = customer.Contact.Email,
        PhoneNumber = customer.Contact.PhoneNumber,

        UserId = customer.User.Identifier,
        Username = customer.User.Username
    };
}
