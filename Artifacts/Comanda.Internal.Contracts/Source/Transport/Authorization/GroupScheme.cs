namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record GroupScheme
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Path { get; init; }
}
