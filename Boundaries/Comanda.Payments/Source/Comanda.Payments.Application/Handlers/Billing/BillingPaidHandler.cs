#pragma warning disable S3626

namespace Comanda.Payments.Application.Handlers.Billing;

public sealed class BillingPaidHandler(IPaymentCollection paymentCollection, IActivityCollection activityCollection) :
    IEventHandler<BillingPaidNotificationScheme>
{
    public async Task HandleAsync(BillingPaidNotificationScheme parameters, CancellationToken cancellation = default)
    {
        var metadata = parameters.Data.PixQrCode.Metadata;
        var paymentId = metadata.GetValueOrDefault("payment.identifier");

        var filters = PaymentFilters.WithSpecifications()
            .WithIdentifier(paymentId)
            .Build();

        var payments = await paymentCollection.FilterPaymentsAsync(filters, cancellation);
        var payment = payments.FirstOrDefault();

        if (payment is null)
        {
            return;
        }

        payment.MarkAsPaid();
        payment.Metadata = new PaymentMetadata(
            Identifier: parameters.Data.PixQrCode.Id,
            Reference: payment.Metadata.Reference
        );

        var activity = new Activity
        {
            Action = "comanda.actions.payment.paid",
            Description = $"customer '{payment.Payer.Username}' completed a payment successfully.",
            Resource = Resource.From(payment.Id, nameof(Payment)),
            User = new User(payment.Payer.Identifier, payment.Payer.Username),
            Metadata = new Dictionary<string, string>
            {
                { "payer.identifier", payment.Payer.Identifier },
                { "payment.method", payment.Method.ToString() },
                { "payment.amount", payment.Amount.ToString("F2") }
            }
        };

        await paymentCollection.UpdateAsync(payment, cancellation: cancellation);
        await activityCollection.InsertAsync(activity, cancellation: cancellation);
    }
}
