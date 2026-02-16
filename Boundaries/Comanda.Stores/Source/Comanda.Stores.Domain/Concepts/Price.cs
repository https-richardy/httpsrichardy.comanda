namespace Comanda.Stores.Domain.Concepts;

public sealed record Price(decimal Value) : IValueObject<Price>
{
    public decimal Value { get; init; } = Value;
}
