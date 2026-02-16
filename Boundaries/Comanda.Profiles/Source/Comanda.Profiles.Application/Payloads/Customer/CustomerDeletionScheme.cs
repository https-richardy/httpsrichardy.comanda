namespace Comanda.Profiles.Application.Payloads.Customer;

public sealed record CustomerDeletionScheme : IDispatchable<Result>
{
    [property: JsonIgnore]
    public string CustomerId { get; init; } = default!;
}
