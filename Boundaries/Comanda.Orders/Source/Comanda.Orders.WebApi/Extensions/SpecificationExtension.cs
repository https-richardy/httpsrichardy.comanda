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

            options.AddPreferredSecuritySchemes(SecuritySchemes.OAuth2);
            options.AddClientCredentialsFlow(SecuritySchemes.OAuth2, flow =>
            {
                flow.WithCredentialsLocation(CredentialsLocation.Body);
            });
        });
    }
}
