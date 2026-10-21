namespace Comanda.Stores.WebApi.Authentication;

public sealed class AuthenticationClaimTransformer(ISettings settings)
{
    public Task HandleAsync(TokenValidatedContext context)
    {
        if (context.Principal?.Identity is not ClaimsIdentity principal)
            return Task.CompletedTask;

        // see more: https://www.keycloak.org/docs/latest/authorization_services/index.html
        var resourceAccess = context.Principal?.FindFirst("resource_access")?.Value;

        if (string.IsNullOrWhiteSpace(resourceAccess))
            return Task.CompletedTask;

        using var document = JsonDocument.Parse(resourceAccess);

        if (!document.RootElement.TryGetProperty(settings.Authentication.Audience, out var audienceElement))
            return Task.CompletedTask;

        if (!audienceElement.TryGetProperty("roles", out var rolesElement))
            return Task.CompletedTask;

        foreach (var roleName in rolesElement.EnumerateArray()
            .Select(role => role.GetString())
            .Where(roleName => !string.IsNullOrWhiteSpace(roleName)))
        {
            principal.AddClaim(new Claim(ClaimTypes.Role, roleName ?? string.Empty));
        }

        return Task.CompletedTask;
    }
}
