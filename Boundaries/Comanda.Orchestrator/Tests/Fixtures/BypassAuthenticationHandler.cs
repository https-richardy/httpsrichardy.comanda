using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

namespace Comanda.Orchestrator.TestSuite.Fixtures;

public sealed class BypassAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) :
    AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, Defaults.User),
            new Claim(ClaimTypes.NameIdentifier, Defaults.UserId),

            new Claim(ClaimTypes.Role, Permissions.ViewActivities),
            new Claim(ClaimTypes.Role, Permissions.ViewPayments),
            new Claim(ClaimTypes.Role, Permissions.MakePayment),
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);

        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
