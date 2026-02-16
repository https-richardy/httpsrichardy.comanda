namespace Comanda.Profiles.Application.Mappers;

public static class ActivityMapper
{
    public static ActivityDetailsScheme AsResponse(Activity activity) => new()
    {
        Action = activity.Action,
        Description = activity.Description,
        OccurredAt = activity.CreatedAt.ToString(),
        Metadata = activity.Metadata,
        Resource = activity.Resource
    };
}
