namespace Comanda.Orchestrator.Application.Gateways;

public interface IEstablishmentGateway
{
    public Task<Result<PaginationScheme<EstablishmentScheme>>> GetEstablishmentsAsync(
        EstablishmentsFetchParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<EstablishmentScheme>> CreateEstablishmentAsync(
        EstablishmentCreationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<EstablishmentScheme>> UpdateEstablishmentAsync(
        EstablishmentModificationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result> DeleteEstablishmentAsync(
        EstablishmentDeletionScheme parameters,
        CancellationToken cancellation = default
    );
}
