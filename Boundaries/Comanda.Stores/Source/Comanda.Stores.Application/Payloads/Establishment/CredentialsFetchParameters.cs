namespace Comanda.Stores.Application.Payloads.Establishment;

public sealed record CredentialsFetchParameters :
    IDispatchable<Result<IEnumerable<CredentialScheme>>>
{
    public string EstablishmentId { get; init; } = default!;
}
