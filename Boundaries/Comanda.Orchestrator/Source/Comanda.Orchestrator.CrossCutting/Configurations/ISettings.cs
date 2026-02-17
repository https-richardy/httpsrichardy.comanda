namespace Comanda.Orchestrator.CrossCutting.Configurations;

public interface ISettings
{
    public FederationSettings Federation { get; }
    public DownstreamSettings Downstream { get; }
    public ObservabilitySettings Observability { get; }
}
