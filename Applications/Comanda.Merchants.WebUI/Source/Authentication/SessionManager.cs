namespace Comanda.Merchants.WebUI.Authentication;

public sealed class SessionManager(ILocalStorageGateway localStorage, IIdentityClient identityClient, AuthenticationStateProvider provider) : ISessionManager
{
    public async Task SignInAsync(string token, string refreshToken)
    {
        await localStorage.SetAsStringAsync(Storage.SecurityToken, token);
        await localStorage.SetAsStringAsync(Storage.RefreshToken, refreshToken);

        // inform provider of signin because blazor does not automatically detect localstorage updates
        if (provider is JwtAuthenticationStateProvider authenticationState)
            authenticationState.NotifyAuthenticationStateChanged();
    }

    public async Task SignOutAsync()
    {
        var refreshToken = await localStorage.GetAsStringAsync(Storage.RefreshToken);
        if (string.IsNullOrWhiteSpace(refreshToken))
            return;

        await localStorage.RemoveAsync(Storage.SecurityToken);
        await identityClient.InvalidateSessionAsync(new() { RefreshToken =  refreshToken });

        // notify provider of logout because blazor does not detect localstorage changes automatically
        if (provider is JwtAuthenticationStateProvider authenticationState)
            authenticationState.NotifyAuthenticationStateChanged();
    }
}
