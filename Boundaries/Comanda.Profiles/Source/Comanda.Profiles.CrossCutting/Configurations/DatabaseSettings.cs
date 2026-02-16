namespace Comanda.Profiles.CrossCutting.Configurations;

public sealed record DatabaseSettings
{
    public string ConnectionString { get; init; } = default!;
    public string DatabaseName { get; init; } = default!;
}
