namespace Comanda.Profiles.Application.Handlers.Owner;

public sealed class OwnerCreationHandler(IOwnerCollection collection, IActivityCollection activityCollection) :
    IDispatchHandler<OwnerCreationScheme, Result<OwnerScheme>>
{
    public async Task<Result<OwnerScheme>> HandleAsync(OwnerCreationScheme parameters, CancellationToken cancellation = default)
    {
        var filters = OwnerFilters.WithSpecifications()
            .WithEmail(parameters.Email)
            .Build();

        var matchingOwners = await collection.GetOwnersAsync(filters, cancellation);
        var existingOwner = matchingOwners.FirstOrDefault();

        if (existingOwner is not null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-76A71 */
            /* this allows us to quickly locate all occurrences of this error in the codebase. */

            return Result<OwnerScheme>.Failure(ProfileErrors.ProfileAlreadyExists);
        }

        var owner = await collection.InsertAsync(parameters.AsOwner(), cancellation: cancellation);
        var activity = new Activity
        {
            Action = "comanda.actions.owner.creation",
            Description = $"owner '{owner.FirstName} {owner.LastName}' created.",

            /* we can filter activities by resource using resource property */
            /* therefore, always register the resource to help us track it. */

            Resource = Resource.From(owner.Id, nameof(Owner)),
            Metadata = new Dictionary<string, string>
            {
                { "owner.identifier", owner.Id },
                { "owner.user.identifier", owner.User.Identifier }
            }
        };

        await activityCollection.InsertAsync(activity, cancellation: cancellation);

        return Result<OwnerScheme>.Success(owner.AsResponse());
    }
}
