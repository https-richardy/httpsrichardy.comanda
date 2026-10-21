namespace Comanda.Profiles.CrossCutting.Exceptions;

public sealed class SettingsNotConfiguredException : Exception
{
    public SettingsNotConfiguredException()
        : base("Settings section is missing or invalid in configuration.")
    {

    }
}
