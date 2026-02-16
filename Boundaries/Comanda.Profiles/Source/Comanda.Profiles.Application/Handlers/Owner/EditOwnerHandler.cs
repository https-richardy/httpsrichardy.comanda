namespace Comanda.Profiles.Application.Handlers.Owner;

public sealed class EditOwnerHandler(IOwnerCollection collection) :
    IDispatchHandler<EditOwnerScheme, Result<OwnerScheme>>
{
    public async Task<Result<OwnerScheme>> HandleAsync(EditOwnerScheme parameters, CancellationToken cancellation = default)
    {
        var filters = OwnerFilters.WithSpecifications()
             .WithIdentifier(parameters.OwnerId)
             .WithIsDeleted(false)
             .Build();

        var matchingOwners = await collection.GetOwnersAsync(filters, cancellation);
        var existingOwner = matchingOwners.FirstOrDefault();

        if (existingOwner is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-0831D */
            return Result<OwnerScheme>.Failure(OwnerErrors.OwnerDoesNotExist);
        }

        var owner = await collection.UpdateAsync(parameters.AsOwner(existingOwner), cancellation);

        return Result<OwnerScheme>.Success(owner.AsResponse());
    }
}
