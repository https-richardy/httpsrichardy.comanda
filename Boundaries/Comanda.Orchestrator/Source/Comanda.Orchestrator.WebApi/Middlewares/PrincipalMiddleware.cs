namespace Comanda.Orchestrator.WebApi.Middlewares;

public sealed class PrincipalMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var principalProvider = context.RequestServices.GetRequiredService<IPrincipalProvider>();

        principalProvider.Clear();

        var endpoint = context.GetEndpoint();
        var requiresAuth = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() != null;

        if (!requiresAuth || context.User.Identity?.IsAuthenticated != true)
        {
            await next(context);
            return;
        }

        var userName = context.User.FindFirst(ClaimTypes.Name);
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userId == null || string.IsNullOrWhiteSpace(userId.Value))
        {
            await next(context);
            return;
        }

        if (userName == null || string.IsNullOrWhiteSpace(userName.Value))
        {
            await next(context);
            return;
        }

        principalProvider.SetPrincipal(new User(userId.Value, userName.Value));

        await next(context);
    }
}
