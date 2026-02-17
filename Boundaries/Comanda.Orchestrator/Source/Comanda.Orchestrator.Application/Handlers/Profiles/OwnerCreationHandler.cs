namespace Comanda.Orchestrator.Application.Handlers.Profiles;

public sealed class OwnerCreationHandler(IProfilesGateway profilesGateway, IUsersClient usersClient, IGroupsClient groupsClient) :
    IDispatchHandler<OwnerCreationScheme, Result<OwnerScheme>>
{
    public async Task<Result<OwnerScheme>> HandleAsync(
        OwnerCreationScheme parameters, CancellationToken cancellation = default)
    {
        var filters = new UsersFetchParameters
        {
            Username = parameters.Username,
        };

        var users = await usersClient.GetUsersAsync(filters, cancellation);
        var user = users.Data?.Items.FirstOrDefault();

        if (user is null)
        {
            return Result<OwnerScheme>.Failure(users.Error);
        }

        var groups = await groupsClient.GetGroupsAsync(new() { Name = Groups.Owners }, cancellation);
        var group = groups.Data?.Items.FirstOrDefault();

        if (group is null)
        {
            return Result<OwnerScheme>.Failure(groups.Error);
        }

        var assignment = await usersClient.AssignUserGroupAsync(user.Id, group.Id, cancellation);
        if (assignment.IsFailure)
        {
            return Result<OwnerScheme>.Failure(assignment.Error);
        }

        return await profilesGateway.CreateOwnerAsync(parameters, cancellation);
    }
}
