namespace Comanda.Payments.WebApi.Authentication;

public sealed class JwtValidationParameters : TokenValidationParameters
{
    public static TokenValidationParameters DefaultParameters => new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
}
