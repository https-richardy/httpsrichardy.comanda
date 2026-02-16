namespace Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

public sealed record CallbacksScheme
{
    public string SuccessUrl { get; init; } = default!;
    public string CancelUrl { get; init; } = default!;
}
