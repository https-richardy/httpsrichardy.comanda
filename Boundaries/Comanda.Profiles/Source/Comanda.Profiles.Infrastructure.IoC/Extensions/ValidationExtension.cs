namespace Comanda.Profiles.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class ValidationExtension
{
    public static void AddValidation(this IServiceCollection services)
    {
        services.AddTransient<IValidator<CustomerCreationScheme>, CustomerCreationSchemeValidator>();
        services.AddTransient<IValidator<AssignCustomerAddressScheme>, AssignCustomerAddressSchemeValidator>();
        services.AddTransient<IValidator<EditCustomerAddressScheme>, EditCustomerAddressSchemeValidator>();

        services.AddTransient<IValidator<EditCustomerScheme>, EditCustomerSchemeValidator>();
        services.AddTransient<IValidator<DeleteCustomerAddressScheme>, DeleteCustomerAddressSchemeValidator>();
        services.AddTransient<IValidator<Address>, AddressSchemeValidator>();

        services.AddTransient<IValidator<OwnerCreationScheme>, OwnerCreationSchemeValidator>();
        services.AddTransient<IValidator<EditOwnerScheme>, EditOwnerSchemeValidator>();
    }
}
