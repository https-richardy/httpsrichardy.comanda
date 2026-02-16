namespace Comanda.Stores.Application.Handlers.Establishment;

public sealed class EstablishmentDeletionHandler(IEstablishmentCollection collection) :
    IDispatchHandler<EstablishmentDeletionScheme, Result>
{
    public async Task<Result> HandleAsync(
        EstablishmentDeletionScheme parameters, CancellationToken cancellation = default)
    {
        var filters = EstablishmentFilters.WithSpecifications()
            .WithIdentifier(parameters.EstablishmentId)
            .Build();

        var establishments = await collection.FilterEstablishmentsAsync(filters, cancellation);
        var existingEstablishment = establishments.FirstOrDefault();

        if (existingEstablishment is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-0A2DF */
            return Result.Failure(EstablishmentErrors.EstablishmentDoesNotExist);
        }

        await collection.DeleteAsync(existingEstablishment, cancellation: cancellation);

        return Result.Success();
    }
}
