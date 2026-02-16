namespace Comanda.Stores.Application.Payloads.Product;

public sealed record ProductCreationScheme : IDispatchable<Result<ProductScheme>>
{
    [property: JsonIgnore]
    // this will be ignored in the payload, but will be set by the route.
    public string EstablishmentId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
}
