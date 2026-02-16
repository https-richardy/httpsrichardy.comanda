namespace Comanda.Stores.Application.Handlers.Establishment;

public sealed class EstablishmentEditionHandler(IEstablishmentCollection collection) :
    IDispatchHandler<EstablishmentEditionScheme, Result<EstablishmentScheme>>
{
    public async Task<Result<EstablishmentScheme>> HandleAsync(
        EstablishmentEditionScheme parameters, CancellationToken cancellation = default)
    {
        var filters = EstablishmentFilters.WithSpecifications()
            .WithIdentifier(parameters.EstablishmentId)
            .Build();

        var establishments = await collection.FilterEstablishmentsAsync(filters, cancellation);
        var existingEstablishment = establishments.FirstOrDefault();

        if (existingEstablishment is null)
        {
            return Result<EstablishmentScheme>.Failure(EstablishmentErrors.EstablishmentDoesNotExist);
        }

        var establishment = await collection.UpdateAsync(existingEstablishment.AsEstablishment(parameters), cancellation);
        var response = establishment.AsResponse();

        return Result<EstablishmentScheme>.Success(response);
    }
}
