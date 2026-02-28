namespace Comanda.Merchants.WebUI.Extensions;

public static class AuthenticationExtension
{
    public static void AddAuthentication(this IServiceCollection services)
    {
        services.AddAuthorizationCore();
        services.AddCascadingAuthenticationState();

        services.AddScoped<ISessionManager, SessionManager>();
        services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
        services.AddScoped<AuthenticationInterceptor, AuthenticationInterceptor>();
    }
}
