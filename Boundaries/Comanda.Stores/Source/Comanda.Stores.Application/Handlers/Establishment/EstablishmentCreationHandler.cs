namespace Comanda.Stores.Application.Handlers.Establishment;

public sealed class EstablishmentCreationHandler(IEstablishmentCollection collection) :
    IDispatchHandler<EstablishmentCreationScheme, Result<EstablishmentScheme>>
{
    public async Task<Result<EstablishmentScheme>> HandleAsync(
        EstablishmentCreationScheme parameters, CancellationToken cancellation = default)
    {
        var filters = EstablishmentFilters.WithSpecifications()
            .WithOwnerId(parameters.Owner.Identifier)
            .Build();

        var establishments = await collection.FilterEstablishmentsAsync(filters, cancellation);
        var existingEstablishment = establishments.FirstOrDefault();

        if (existingEstablishment is not null)
        {
            return Result<EstablishmentScheme>.Failure(EstablishmentErrors.OwnerAlreadyHasEstablishment);
        }

        var establishment = await collection.InsertAsync(parameters.AsEstablishment(), cancellation: cancellation);
        var response = establishment.AsResponse();

        return Result<EstablishmentScheme>.Success(response);
    }
}
