namespace Comanda.Stores.Domain.Builders;

public sealed class EstablishmentBuilder(Establishment establishment)
{
    private readonly Establishment _establishment = establishment;

    public EstablishmentBuilder SetCredential(Credential credential)
    {
        _establishment.Credentials.Add(credential);
        return this;
    }

    public EstablishmentBuilder SetBranding(Branding branding)
    {
        _establishment.Branding = branding;
        return this;
    }

    public Establishment Build()
    {
        return _establishment;
    }
}
