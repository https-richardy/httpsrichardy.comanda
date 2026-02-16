namespace Comanda.Internal.Contracts.Transport.Internal.Products;

public sealed record ProductImageStreamScheme : IDispatchable<Result>
{
    public string ProductId { get; init; } = default!;
    public string EstablishmentId { get; init; } = default!;
    public Stream Stream { get; init; } = default!;
}
