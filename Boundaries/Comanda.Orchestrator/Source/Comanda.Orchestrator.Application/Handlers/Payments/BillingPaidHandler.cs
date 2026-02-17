namespace Comanda.Orchestrator.Application.Handlers.Payments;

public sealed class BillingPaidHandler(IPaymentGateway paymentGateway, IOrdersGateway ordersGateway) :
    IEventHandler<BillingPaidNotificationScheme>
{
    public async Task HandleAsync(BillingPaidNotificationScheme message, CancellationToken cancellation = default)
    {
        var metadata = message.Data.PixQrCode.Metadata;
        var identifier = metadata.GetValueOrDefault("payment.identifier");

        if (identifier is null)
            return;

        var payments = await paymentGateway.GetPaymentsAsync(identifier.AsFilters(), cancellation);
        var payment = payments.Data?.Items.FirstOrDefault();

        if (payment is null)
            return;

        var orders = await ordersGateway.GetOrdersAsync(payment.AsFilters(), cancellation);
        var order = orders.Data?.Items.FirstOrDefault();

        if (order is null)
            return;

        var result = await ordersGateway.UpdateOrderAsync(order.AsPatch(), cancellation);
        if (result.IsFailure || result.Data is null)
            return;

        var billingPaid = await paymentGateway.UpdatePaymentStatusAsync(payment.AsMutation(), cancellation);
        if (billingPaid.IsFailure || billingPaid.Data is null)
            return;
    }
}
