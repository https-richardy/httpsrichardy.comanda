namespace Comanda.Stores.TestSuite.Extensions;

public static class AuthenticationSetup
{
    private const string _bypassScheme = "vinder.internal.bypass.authentication";

    public static void AddBypassAuthentication(this IServiceCollection services)
    {
        var policyBuilder = new AuthorizationPolicyBuilder(_bypassScheme);
        var authenticationBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = _bypassScheme;
            options.DefaultChallengeScheme = _bypassScheme;
            options.DefaultScheme = _bypassScheme;
        });

        var authorizationPolicy = policyBuilder
            .RequireAuthenticatedUser()
            .Build();

        authenticationBuilder.AddScheme<AuthenticationSchemeOptions, BypassAuthenticationHandler>(_bypassScheme, _ => { });
        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(authorizationPolicy);
    }
}
