namespace Comanda.Profiles.Application.Handlers.Owner;

public sealed class OwnerDeletionHandler(IOwnerCollection collection) :
    IDispatchHandler<OwnerDeletionScheme, Result>
{
    public async Task<Result> HandleAsync(OwnerDeletionScheme parameters, CancellationToken cancellation = default)
    {
        var filters = OwnerFilters.WithSpecifications()
             .WithIdentifier(parameters.OwnerId)
             .Build();

        var matchingOwners = await collection.GetOwnersAsync(filters, cancellation);
        var existingOwner = matchingOwners.FirstOrDefault();

        if (existingOwner is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-0831D */
            return Result.Failure(OwnerErrors.OwnerDoesNotExist);
        }

        await collection.DeleteAsync(existingOwner, cancellation: cancellation);

        return Result.Success();
    }
}
