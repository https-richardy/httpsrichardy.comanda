namespace Comanda.Profiles.Application.Payloads.Owner;

public sealed record OwnerDeletionScheme : IDispatchable<Result>
{
    [property: JsonIgnore]
    public string OwnerId { get; init; } = default!;
}
