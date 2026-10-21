namespace Comanda.Subscriptions.WebApi.Extensions;

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
            options.Servers = [];

            options.WithTitle("Comanda Subscriptions | Reference");
            options.ExpandAllTags();

            options.AddPreferredSecuritySchemes(SecuritySchemes.OAuth2);
            options.AddClientCredentialsFlow(SecuritySchemes.OAuth2, flow =>
            {
                flow.WithCredentialsLocation(CredentialsLocation.Body);
            });
        });
    }
}
