namespace Comanda.Orchestrator.CrossCutting.Configurations;

public sealed record Settings : ISettings
{
    public FederationSettings Federation { get; init; } = default!;
    public DownstreamSettings Downstream { get; init; } = default!;
    public ObservabilitySettings Observability { get; init; } = default!;
}
