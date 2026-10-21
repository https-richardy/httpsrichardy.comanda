namespace Comanda.Payments.CrossCutting.Configurations;

public sealed record AbacatePaySettings
{
    public string Url { get; init; } = default!;
}
