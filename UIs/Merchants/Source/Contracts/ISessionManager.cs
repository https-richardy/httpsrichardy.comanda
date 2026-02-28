namespace Comanda.Merchants.WebUI.Contracts;

public interface ISessionManager
{
    Task SignInAsync(string token, string refreshToken);
    Task SignOutAsync();
}
