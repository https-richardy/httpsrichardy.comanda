namespace Comanda.Subscriptions.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "this class is responsible for configuring the dependency injection container and does not contain any logic that can be tested.")]
public static class ReferenceExtension
{
    public static void UseReference(this IEndpointRouteBuilder app)
    {
        var settings = app.ServiceProvider.GetRequiredService<ISettings>();

        app.MapScalarApiReference(options =>
        {
            options.DarkMode = false;
            options.HideDarkModeToggle = true;
            options.HideClientButton = true;
            options.HideModels = true;
            options.Servers = [];

            options.WithTitle("Comanda Subscriptions | Reference");
            options.ExpandAllTags();
            options.AddAuthorizationCodeFlow(SecuritySchemes.OAuth2, flow =>
            {
                flow.ClientId = settings.Authentication.Audience;
            });
        });
    }
}
