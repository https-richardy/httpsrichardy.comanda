namespace Comanda.Stores.Domain.Aggregates;

public sealed class Product : Aggregate
{
    public Image Image { get; set; } = default!;
    public Price Price { get; set; } = default!;
    public Properties Properties { get; set; } = default!;
    public ProductMetadata Metadata { get; set; } = default!;
}
