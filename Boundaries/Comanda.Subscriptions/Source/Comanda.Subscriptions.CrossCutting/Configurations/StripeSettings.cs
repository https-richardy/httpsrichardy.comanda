namespace Comanda.Subscriptions.CrossCutting.Configurations;

public sealed record StripeSettings
{
    public string Secret { get; init; } = default!;
}