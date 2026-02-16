namespace Comanda.Stores.Application.Payloads.Product;

public sealed record ProductScheme
{
    public string Identifier { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Image { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}
