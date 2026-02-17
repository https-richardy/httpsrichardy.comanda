namespace Comanda.Orchestrator.Application.Mappers;

public static class CheckoutMapper
{
    public static OrderCreationScheme AsOrder(this CreateCheckoutScheme parameters, User principal) => new()
    {
        Fulfillment = parameters.Fulfillment,
        Items = parameters.Items,
        Priority = Priority.Normal,
        Metadata = new Internal.Contracts.Transport.Internal.Orders.Metadata(parameters.EstablishmentId, principal.Identifier)
    };

    public static EstablishmentsFetchParameters AsFilters(this CreateCheckoutScheme parameters) => new()
    {
        Id = parameters.EstablishmentId
    };

    public static CredentialsFetchParameters AsCredentialFilters(this CreateCheckoutScheme parameters) => new()
    {
        EstablishmentId = parameters.EstablishmentId
    };

    public static CheckoutSessionCreationScheme AsCharge(this CreateCheckoutScheme parameters, OrderScheme order, User principal) => new()
    {
        Amount = order.Items.Sum(item => item.UnitPrice * item.Quantity),
        Reference = order.Identifier,
        Payer = new User(principal.Identifier, principal.Username)
    };

    public static CheckoutScheme AsCheckout(this CheckoutSession charge, OrderScheme order) => new()
    {
        OrderId = order.Identifier,
        OrderCode = order.Code,
        PixCode = charge.Code,
        QrCode = charge.QrCode,
    };


    public static Credential AsCredential(this CredentialScheme credential)
    {
        return new Credential(credential.SecretKey);
    }
}
