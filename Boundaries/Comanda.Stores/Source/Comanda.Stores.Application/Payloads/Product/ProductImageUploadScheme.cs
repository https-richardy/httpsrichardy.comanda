namespace Comanda.Stores.Application.Payloads.Product;

public sealed record ProductImageUploadScheme : IDispatchable<Result>
{
    public string ProductId { get; init; } = default!;
    public string EstablishmentId { get; init; } = default!;
    public Stream Stream { get; init; } = default!;
}
