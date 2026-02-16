namespace Comanda.Internal.Contracts.Transport.Internal.Stores;

public sealed record CredentialsFetchParameters :
    IMessage<Result<IEnumerable<CredentialScheme>>>
{
    public string EstablishmentId { get; init; } = default!;
}

