namespace Comanda.Subscriptions.Infrastructure.IoC.Configurations;

public sealed record DatabaseSettings
{
    public string ConnectionString { get; init; } = default!;
    public string DatabaseName { get; init; } = default!;
}