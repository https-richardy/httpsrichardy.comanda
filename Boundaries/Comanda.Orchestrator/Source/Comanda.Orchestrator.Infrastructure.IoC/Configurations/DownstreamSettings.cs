namespace Comanda.Orchestrator.CrossCutting.Configurations;

public sealed class DownstreamSettings
{
    public string ProfilesUrl { get; init; } = default!;
    public string StoresUrl { get; init; } = default!;
    public string PaymentsUrl { get; init; } = default!;
    public string SubscriptionsUrl { get; init; } = default!;
    public string OrdersUrl { get; init; } = default!;
}
