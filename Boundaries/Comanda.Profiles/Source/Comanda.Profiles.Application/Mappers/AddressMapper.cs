namespace Comanda.Profiles.Application.Mappers;

public static class AddressMapper
{
    public static Address AsAddress(this AssignCustomerAddressScheme address) => new()
    {
        Street = address.Street,
        Number = address.Number,
        Neighborhood = address.Neighborhood,
        City = address.City,
        State = address.State,
        ZipCode = address.ZipCode,
        Complement = address.Complement,
        Reference = address.Reference
    };
}
