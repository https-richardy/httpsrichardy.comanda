namespace Comanda.Stores.Application.Mappers;

public static class EstablishmentMapper
{
    public static Domain.Aggregates.Establishment AsEstablishment(this EstablishmentCreationScheme establishment)
    {
        var branding = new Branding(
            PrimaryColor: string.Empty,
            SecondaryColor: string.Empty,
            Logo: string.Empty
        );

        var properties = new Properties(
            Title: establishment.Title,
            Description: establishment.Description
        );

        return new Domain.Aggregates.Establishment
        {
            Properties = properties,
            Branding = branding,
            Owner = establishment.Owner
        };
    }

    public static Domain.Aggregates.Establishment AsEstablishment(this Domain.Aggregates.Establishment establishment, EstablishmentEditionScheme edition)
    {
        establishment.Properties = new Properties(
            Title: edition.Title,
            Description: edition.Description
        );

        establishment.Branding = new Branding(
            PrimaryColor: edition.Branding.PrimaryColor,
            SecondaryColor: edition.Branding.SecondaryColor,
            Logo: edition.Branding.Logo
        );

        return establishment;
    }

    public static EstablishmentScheme AsResponse(this Domain.Aggregates.Establishment establishment) => new()
    {
        Identifier = establishment.Id,
        Title = establishment.Properties.Title,
        Description = establishment.Properties.Description,
        Owner = establishment.Owner,
        Branding = establishment.Branding
    };
}
