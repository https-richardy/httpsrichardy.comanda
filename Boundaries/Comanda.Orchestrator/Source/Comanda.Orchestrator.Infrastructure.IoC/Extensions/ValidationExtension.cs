namespace Comanda.Orchestrator.Infrastructure.IoC.Extensions;

public static class ValidationExtension
{
    public static void AddValidation(this IServiceCollection services)
    {
        services.AddTransient<IValidator<EstablishmentCreationScheme>, EstablishmentCreationSchemeValidator>();
        services.AddTransient<IValidator<EstablishmentModificationScheme>, EstablishmentModificationSchemeValidator>();

        services.AddTransient<IValidator<CredentialCreationScheme>, CredentialCreationSchemeValidator>();
        services.AddTransient<IValidator<CredentialModificationScheme>, CredentialModificationSchemeValidator>();

        services.AddTransient<IValidator<ProductCreationScheme>, ProductCreationSchemeValidator>();
        services.AddTransient<IValidator<ProductModificationScheme>, ProductModificationSchemeValidator>();
        services.AddTransient<IValidator<ProductImageStreamScheme>, ProductImageUploadSchemeValidator>();

        services.AddTransient<IValidator<CustomerCreationScheme>, CustomerCreationSchemeValidator>();
        services.AddTransient<IValidator<AssignCustomerAddressScheme>, AssignCustomerAddressSchemeValidator>();
        services.AddTransient<IValidator<EditCustomerAddressScheme>, EditCustomerAddressSchemeValidator>();

        services.AddTransient<IValidator<EditCustomerScheme>, EditCustomerSchemeValidator>();
        services.AddTransient<IValidator<DeleteCustomerAddressScheme>, DeleteCustomerAddressSchemeValidator>();
        services.AddTransient<IValidator<Address>, AddressSchemeValidator>();

        services.AddTransient<IValidator<OwnerCreationScheme>, OwnerCreationSchemeValidator>();
        services.AddTransient<IValidator<EditOwnerScheme>, OwnerModificationSchemeValidator>();

        services.AddTransient<IValidator<CheckoutSessionCreationScheme>, CheckoutSessionCreationSchemeValidator>();
        services.AddTransient<IValidator<OfflinePaymentScheme>, OfflinePaymentSchemeValidator>();

        services.AddTransient<IValidator<CreateCheckoutScheme>, CheckoutSchemeValidator>();
        services.AddTransient<IValidator<OrderCreationScheme>, OrderCreationSchemeValidator>();
        services.AddTransient<IValidator<OrderModificationScheme>, OrderModificationSchemeValidator>();
        services.AddTransient<IValidator<SubscriptionCheckoutSessionCreationScheme>, SubscriptionCheckoutSessionCreationSchemeValidator>();
    }
}
