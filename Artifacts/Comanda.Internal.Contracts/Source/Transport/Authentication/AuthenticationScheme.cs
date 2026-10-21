namespace Comanda.Internal.Contracts.Transport.Authentication;

public sealed record AuthenticationScheme
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public string TokenType { get; init; }

    public string SessionState { get; init; }
    public string Scope { get; init; }

    public int NotBeforePolicy { get; init; }
    public int ExpiresIn { get; init; }
    public int RefreshExpiresIn { get; init; }
}
