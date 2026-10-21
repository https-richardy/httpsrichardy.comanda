namespace Comanda.Orders.Infrastructure.IoC.Configurations;

public sealed record Settings : ISettings
{
    public DatabaseSettings Database { get; init; } = default!;
    public AuthenticationSettings Authentication { get; init; } = default!;
    public ObservabilitySettings Observability { get; init; } = default!;
}
