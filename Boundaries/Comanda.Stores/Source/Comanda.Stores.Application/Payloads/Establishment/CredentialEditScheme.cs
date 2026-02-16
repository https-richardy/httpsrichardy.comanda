namespace Comanda.Stores.Application.Payloads.Establishment;

public sealed record CredentialEditScheme :
    IDispatchable<Result<CredentialScheme>>
{
    [property: JsonIgnore]
    public string EstablishmentId { get; init; } = default!;

    [property: JsonIgnore]
    public string CredentialId { get; init; } = default!;

    public string SecretKey { get; init; } = default!;

    // represents the integration target as a payment gateway or communication channel
    // determines which external service this credential should authenticate against
    public IntegrationTarget Provider { get; init; } = default!;
}
