namespace Comanda.Orchestrator.Application.Payloads.Checkout;

public sealed record CreateCheckoutScheme : IDispatchable<Result<CheckoutScheme>>
{
    [property: JsonIgnore]
    public string EstablishmentId { get; init; } = string.Empty;

    public Fulfillment Fulfillment { get; init; } = Fulfillment.Unspecified;
    public IEnumerable<Item> Items { get; init; } = [];
}
