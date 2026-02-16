namespace Comanda.Internal.Contracts.Transport.Internal.Products;

public sealed record ProductDeletionScheme : IDispatchable<Result>
{
    [property: JsonIgnore]
    public string EstablishmentId { get; init; } = default!;

    [property: JsonIgnore]
    public string ProductId { get; init; } = default!;
}
