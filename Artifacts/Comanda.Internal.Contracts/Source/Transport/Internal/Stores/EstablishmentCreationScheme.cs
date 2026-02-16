namespace Comanda.Internal.Contracts.Transport.Internal.Stores;

public sealed record EstablishmentCreationScheme :
    IMessage<Result<EstablishmentScheme>>
{
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public User Owner { get; init; } = default!;
}
