namespace Comanda.Orchestrator.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class SettingsExtension
{
    public static ISettings AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration
            .GetSection("Settings")
            .Get<Settings>() ?? throw new SettingsNotConfiguredException();

        services.AddSingleton<ISettings>(settings);

        return settings;
    }
}
