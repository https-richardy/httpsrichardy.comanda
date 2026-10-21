namespace Comanda.Subscriptions.Infrastructure.IoC.Configurations;

public sealed record StripeSettings
{
    public string Secret { get; init; } = default!;
}