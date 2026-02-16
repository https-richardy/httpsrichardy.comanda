namespace Comanda.Internal.Contracts.Transport.Internal.Products;

public sealed record ProductModificationScheme : IMessage<Result<ProductScheme>>
{
    [property: JsonIgnore]
    public string EstablishmentId { get; set; } = default!;

    [property: JsonIgnore]
    public string ProductId { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}
