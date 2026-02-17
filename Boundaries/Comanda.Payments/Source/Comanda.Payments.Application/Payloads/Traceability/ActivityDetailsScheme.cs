namespace Comanda.Payments.Application.Payloads.Traceability;

public sealed record ActivityDetailsScheme
{
    public string Action { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string OccurredAt { get; init; } = default!;

    public Resource? Resource { get; init; } = default!;
    public Dictionary<string, string> Metadata { get; init; } = [];
}
