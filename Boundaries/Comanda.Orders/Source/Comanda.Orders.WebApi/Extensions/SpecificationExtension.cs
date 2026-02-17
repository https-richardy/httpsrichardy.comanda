namespace Comanda.Orders.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class SpecificationsExtension
{
    public static void UseSpecification(this IEndpointRouteBuilder app, IWebHostEnvironment environment)
    {
        app.MapScalarApiReference(options =>
        {
            options.DarkMode = false;
            options.HideDarkModeToggle = true;
            options.HideClientButton = true;
            options.HideModels = true;

            options.WithTitle("Comanda Orders | Reference");
            options.ExpandAllTags();

            if (environment.IsProduction())
            {
                options.AddPreferredSecuritySchemes(SecuritySchemes.OAuth2);
                options.AddClientCredentialsFlow(SecuritySchemes.OAuth2, flow =>
                {
                    flow.WithCredentialsLocation(CredentialsLocation.Body);
                });

                return;
            }

            var settings = app.ServiceProvider.GetService<ISettings>()!;

            options.AddPreferredSecuritySchemes(SecuritySchemes.OAuth2);
            options.AddClientCredentialsFlow(SecuritySchemes.OAuth2, flow =>
            {
                flow.ClientId = settings.Federation.ClientId;
                flow.ClientSecret = settings.Federation.ClientSecret;
                flow.WithCredentialsLocation(CredentialsLocation.Body);
            });
        });
    }
}
