namespace Comanda.Internal.Contracts.Transport.Internal.Profiles;

public sealed record CustomerDeletionScheme : IMessage<Result>
{
    [property: JsonIgnore]
    public string CustomerId { get; init; } = default!;
}
