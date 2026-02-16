namespace Comanda.Stores.Domain.Concepts;

public sealed record Branding(string PrimaryColor, string SecondaryColor, string Logo) :
    IValueObject<Branding>
{
    public string PrimaryColor { get; init; } = PrimaryColor;
    public string SecondaryColor { get; init; } = SecondaryColor;
    public string Logo { get; init; } = Logo;
}
