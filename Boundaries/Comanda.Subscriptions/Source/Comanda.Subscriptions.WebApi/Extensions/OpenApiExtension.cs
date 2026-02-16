namespace Comanda.Subscriptions.WebApi.Extensions;

public static class OpenApiExtension
{
    public static void AddOpenApiSpecification(this IServiceCollection services)
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
                document.Components.SecuritySchemes[SecuritySchemes.Bearer] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter 'Bearer' and then your valid token."
                };

                document.Components.SecuritySchemes[SecuritySchemes.OAuth2] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(settings.Federation.BaseUrl + "/api/v1/protocol/open-id/connect/token")
                        }
                    }
                };

                document.SecurityRequirements ??= [];
                document.SecurityRequirements.Add(new OpenApiSecurityRequirement
                {
                    [document.Components.SecuritySchemes[SecuritySchemes.Bearer]] = Array.Empty<string>(),
                });

                document.Info.Contact = new OpenApiContact
                {
                    Name = "vinder.io",
                    Email = "vinder.desenvolvimento@gmail.com",
                };

                return Task.CompletedTask;
            });
        });
    }
}
