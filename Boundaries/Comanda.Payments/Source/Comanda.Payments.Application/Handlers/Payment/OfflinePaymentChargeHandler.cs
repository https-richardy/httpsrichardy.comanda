namespace Comanda.Payments.Application.Handlers.Payment;

public sealed class OfflinePaymentChargeHandler(IPaymentCollection paymentCollection) :
    IDispatchHandler<OfflinePaymentChargeScheme, Result<PaymentScheme>>
{
    public async Task<Result<PaymentScheme>> HandleAsync(
        OfflinePaymentChargeScheme parameters, CancellationToken cancellation = default)
    {
        if (parameters.Method == Method.Pix)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-947B5 */
            return Result<PaymentScheme>.Failure(PaymentErrors.MethodNotAllowed);
        }

        var payment = await paymentCollection.InsertAsync(parameters.AsPayment(), cancellation: cancellation);

        return Result<PaymentScheme>.Success(payment.AsResponse());
    }
}
