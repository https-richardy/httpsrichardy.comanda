namespace Comanda.Orchestrator.Application.Handlers.Checkout;

public sealed class CreateCheckoutHandler(
    IOrdersGateway ordersGateway,
    IPaymentGateway paymentGateway,
    IPrincipalProvider principalProvider,
    IEstablishmentGateway establishmentGateway,
    ICredentialsGateway credentialsGateway
) : IDispatchHandler<CreateCheckoutScheme, Result<CheckoutScheme>>
{
    public async Task<Result<CheckoutScheme>> HandleAsync(
        CreateCheckoutScheme parameters, CancellationToken cancellation = default)
    {
        var principal = principalProvider.GetCurrentPrincipal();
        var filters = parameters.AsFilters();

        var establishments = await establishmentGateway.GetEstablishmentsAsync(filters, cancellation);
        if (establishments.IsFailure || establishments.Data is null)
        {
            return Result<CheckoutScheme>.Failure(establishments.Error);
        }

        var order = await ordersGateway.CreateOrderAsync(parameters.AsOrder(principal), cancellation);
        if (order.IsFailure || order.Data is null)
        {
            return Result<CheckoutScheme>.Failure(order.Error);
        }

        var credentials = await credentialsGateway.GetCredentialsAsync(parameters.AsCredentialFilters(), cancellation);
        var credential = credentials.Data?.FirstOrDefault(credential => credential.Provider == IntegrationTarget.PaymentGateway.ToString());

        if (credentials.IsFailure || credentials.Data is null)
        {
            return Result<CheckoutScheme>.Failure(credentials.Error);
        }

        if (credential is null)
        {
            return Result<CheckoutScheme>.Failure(CredentialErrors.CredentialDoesNotExist);
        }

        var payment = await paymentGateway.CreateOnlineChargeAsync(parameters.AsCharge(order.Data, principal), credential.AsCredential(), cancellation);
        if (payment.IsFailure || payment.Data is null)
        {
            return Result<CheckoutScheme>.Failure(payment.Error);
        }

        return Result<CheckoutScheme>.Success(payment.Data.AsCheckout(order.Data));
    }
}
