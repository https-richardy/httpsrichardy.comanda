namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record OwnerDeletionScheme : IMessage<Result>
{
    [property: JsonIgnore]
    public string OwnerId { get; init; } = default!;
}
