namespace Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

public sealed record Metadata
{
    public string Identifier { get; init; } = default!;
}
