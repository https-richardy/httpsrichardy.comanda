namespace Comanda.Internal.Contracts.Transport.Internal.Stores;

public sealed record EstablishmentDeletionScheme : IMessage<Result>
{
    [property: JsonIgnore]
    public string EstablishmentId { get; init; } = default!;
}
