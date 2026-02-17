namespace Comanda.Orchestrator.Application.Handlers.Profiles;

public sealed class CustomerCreationHandler(IProfilesGateway profilesGateway, IUsersClient usersClient, IGroupsClient groupsClient) :
    IDispatchHandler<CustomerCreationScheme, Result<CustomerScheme>>
{
    public async Task<Result<CustomerScheme>> HandleAsync(
        CustomerCreationScheme parameters, CancellationToken cancellation = default)
    {
        var filters = new UsersFetchParameters
        {
            Username = parameters.Username,
        };

        var users = await usersClient.GetUsersAsync(filters, cancellation);
        var user = users.Data?.Items.FirstOrDefault();

        if (user is null)
        {
            return Result<CustomerScheme>.Failure(UserErrors.UserDoesNotExist);
        }

        var groups = await groupsClient.GetGroupsAsync(new() { Name = Groups.Customers }, cancellation);
        var group = groups.Data?.Items.FirstOrDefault();

        if (group is null)
        {
            return Result<CustomerScheme>.Failure(GroupErrors.GroupDoesNotExist);
        }

        var assignment = await usersClient.AssignUserGroupAsync(user.Id, group.Id, cancellation);
        if (assignment.IsFailure)
        {
            return Result<CustomerScheme>.Failure(assignment.Error);
        }

        return await profilesGateway.CreateCustomerAsync(parameters, cancellation);
    }
}
