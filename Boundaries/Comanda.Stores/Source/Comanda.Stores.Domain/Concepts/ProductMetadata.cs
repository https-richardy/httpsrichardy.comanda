namespace Comanda.Stores.Domain.Concepts;

public sealed record ProductMetadata(string EstablishmentId) : IValueObject<ProductMetadata>
{
    public string EstablishmentId { get; init; } = EstablishmentId;
}
