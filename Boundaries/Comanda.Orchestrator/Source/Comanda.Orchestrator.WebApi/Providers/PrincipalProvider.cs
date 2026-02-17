namespace Comanda.Orchestrator.WebApi.Providers;

public sealed class PrincipalProvider : IPrincipalProvider
{
    private User? _currentPrincipal;

    public User GetCurrentPrincipal() =>
        _currentPrincipal!;

    public void SetPrincipal(User user) =>
        _currentPrincipal = user;

    public void Clear() =>
        _currentPrincipal = null;
}
