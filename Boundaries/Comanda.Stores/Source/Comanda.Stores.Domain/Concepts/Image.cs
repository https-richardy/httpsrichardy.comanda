namespace Comanda.Stores.Domain.Concepts;

public sealed record Image(string Path) : IValueObject<Image>
{
    public string Path { get; init; } = Path;
}
