namespace Comanda.Stores.Infrastructure.Options;

public sealed record StorageOptions
{
    public string RootPath { get; init; } = default!;
    public string AssetsDirectory { get; init; } = default!;
}
