namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record RoleAssignmentScheme
{
    public string RoleId { get; init; }
    public string SubjectId { get; init; }

    public RoleAssignmentScheme WithRoleId(string roleId) =>
        this with { RoleId = roleId };

    public RoleAssignmentScheme WithSubjectId(string subjectId) =>
        this with { SubjectId = subjectId };
}
