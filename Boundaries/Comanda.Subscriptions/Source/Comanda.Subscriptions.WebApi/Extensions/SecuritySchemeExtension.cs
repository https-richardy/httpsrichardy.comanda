namespace Comanda.Subscriptions.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "this class is responsible for configuring the dependency injection container and does not contain any logic that can be tested.")]
public static class SecuritySchemeExtension
{
    public static void AddSecuritySchemes(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<ISettings>();

        services.AddOpenApi(options =>
        {
            options.AddScalarTransformers();
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();
                document.Components.SecuritySchemes[SecuritySchemes.OAuth2] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{settings.Authentication.Authority}/protocol/openid-connect/auth"),
                            TokenUrl = new Uri($"{settings.Authentication.Authority}/protocol/openid-connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                ["openid"] = "open-id connect scope",
                                ["profile"] = "user profile information",
                                ["email"] = "user email information"
                            }
                        }
                    }
                };

                return Task.CompletedTask;
            });
        });
    }
}
