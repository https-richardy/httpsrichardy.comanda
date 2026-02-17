namespace Comanda.Orders.Domain.Concepts;

public sealed record Item : IValueObject<Item>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}
