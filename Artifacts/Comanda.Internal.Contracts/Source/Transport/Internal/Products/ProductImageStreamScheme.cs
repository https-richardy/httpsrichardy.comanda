namespace Comanda.Internal.Contracts.Transport.Internal.Products;

public sealed record ProductImageStreamScheme : IMessage<Result>
{
    public string ProductId { get; init; } = default!;
    public string EstablishmentId { get; init; } = default!;
    public Stream Stream { get; init; } = default!;
}
