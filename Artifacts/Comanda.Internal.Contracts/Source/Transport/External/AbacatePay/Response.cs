namespace Comanda.Internal.Contracts.Transport.External.AbacatePay;

public sealed record Response<TData>
{
    public TData Data { get; init; } = default!;
}
