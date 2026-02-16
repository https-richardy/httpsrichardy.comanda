namespace Comanda.Stores.Domain.Concepts;

public sealed record Properties(string Title, string Description) : IValueObject<Properties>
{
    public string Title { get; init; } = Title;
    public string Description { get; init; } = Description;
}
