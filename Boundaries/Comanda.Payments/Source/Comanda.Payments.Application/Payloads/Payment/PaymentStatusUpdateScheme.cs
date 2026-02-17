namespace Comanda.Payments.Application.Payloads.Payment;

public sealed record PaymentStatusUpdateScheme : IDispatchable<Result<PaymentScheme>>
{
    [property: JsonIgnore]
    public string Identifier { get; init; } = default!;
    public Status Status { get; init; }
}
