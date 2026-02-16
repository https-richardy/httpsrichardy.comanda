namespace Comanda.Stores.Application.Payloads.Establishment;

public sealed record EstablishmentScheme
{
    public string Identifier { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public User Owner { get; set; } = default!;
    public Branding Branding { get; set; } = default!;
}
