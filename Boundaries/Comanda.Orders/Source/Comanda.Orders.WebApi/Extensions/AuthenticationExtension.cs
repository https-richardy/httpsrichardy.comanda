namespace Comanda.Orders.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class AuthenticationExtension
{
    public static void AddBearerAuthentication(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<ISettings>();

        // see more: https://www.keycloak.org/docs/latest/authorization_services/index.html

        services.AddScoped<AuthenticationClaimTransformer>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = settings.Authentication.Authority;
                options.Audience = settings.Authentication.Audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = JwtValidationParameters.DefaultParameters;
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var transformer = context.HttpContext.RequestServices
                            .GetRequiredService<AuthenticationClaimTransformer>();

                        await transformer.HandleAsync(context);
                    }
                };
            });
    }
}
