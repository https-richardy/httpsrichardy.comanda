namespace Comanda.Internal.Contracts.Transport.Internal.Stores;

public sealed record CredentialCreationScheme :
    IDispatchable<Result<CredentialScheme>>
{
    [property: JsonIgnore]
    public string EstablishmentId { get; set; } = default!;
    public string SecretKey { get; init; } = default!;

    // represents the integration target as a payment gateway or communication channel
    // determines which external service this credential should authenticate against
    public IntegrationTarget Provider { get; init; } = default!;
}
