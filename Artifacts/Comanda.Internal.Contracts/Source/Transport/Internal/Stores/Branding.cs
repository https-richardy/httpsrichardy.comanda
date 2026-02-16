namespace Comanda.Internal.Contracts.Transport.Internal.Stores;

public sealed record Branding(string PrimaryColor, string SecondaryColor, string Logo)
{
    public string PrimaryColor { get; init; } = PrimaryColor;
    public string SecondaryColor { get; init; } = SecondaryColor;
    public string Logo { get; init; } = Logo;
}
