namespace Comanda.Internal.Contracts.Transport.Internal.Stores;

public sealed record EstablishmentModificationScheme :
    IMessage<Result<EstablishmentScheme>>
{
    [property: JsonIgnore]
    public string EstablishmentId { get; set; } = default!;
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public Branding Branding { get; init; } = default!;
}
