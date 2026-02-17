namespace Comanda.Payments.Application.Handlers.Payment;

public sealed class CreateCheckoutSessionHandler(
    IPaymentGateway paymentGateway,
    IPaymentCollection paymentCollection,
    IActivityCollection activityCollection
) : IDispatchHandler<CheckoutSessionCreationScheme, Result<CheckoutSession>>
{
    public async Task<Result<CheckoutSession>> HandleAsync(
        CheckoutSessionCreationScheme parameters, CancellationToken cancellation = default)
    {
        var payment = await paymentCollection.InsertAsync(parameters.AsPayment(), cancellation: cancellation);
        var activity = new Activity
        {
            Action = "comanda.actions.payment.creation",
            Description = $"customer '{parameters.Payer.Username}' started a new payment process.",
            Resource = Resource.From(payment.Id, nameof(Payment)),
            User = new User(parameters.Payer.Identifier, parameters.Payer.Username),
            Metadata = new Dictionary<string, string>
            {
                { "payer.identifier", payment.Payer.Identifier },
                { "payment.method", payment.Method.ToString() },
                { "payment.amount", payment.Amount.ToString() }
            }
        };

        await activityCollection.InsertAsync(activity, cancellation: cancellation);

        return await paymentGateway.CreateCheckoutSessionAsync(parameters, payment, cancellation);
    }
}
