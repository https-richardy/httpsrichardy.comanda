namespace Comanda.Stores.Application.Payloads.Establishment;

public sealed record EstablishmentCreationScheme :
    IDispatchable<Result<EstablishmentScheme>>
{
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;

    // we need to associate owner information with the establishment created
    // therefore, the username and the user identifieer will be passed here.
    public User Owner { get; init; } = default!;
}
