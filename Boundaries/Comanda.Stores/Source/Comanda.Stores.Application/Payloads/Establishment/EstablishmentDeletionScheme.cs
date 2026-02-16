namespace Comanda.Stores.Application.Payloads.Establishment;

public sealed record EstablishmentDeletionScheme : IDispatchable<Result>
{
    [property: JsonIgnore]
    public string EstablishmentId { get; init; } = default!;
}
