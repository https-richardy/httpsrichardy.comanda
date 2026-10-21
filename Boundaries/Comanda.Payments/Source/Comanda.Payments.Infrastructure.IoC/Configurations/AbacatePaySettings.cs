namespace Comanda.Payments.Infrastructure.IoC.Configurations;

public sealed record AbacatePaySettings
{
    public string Url { get; init; } = default!;
}
