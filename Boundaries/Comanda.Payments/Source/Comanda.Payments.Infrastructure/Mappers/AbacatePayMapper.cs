namespace Comanda.Payments.Infrastructure.Mappers;

public static class AbacatePayMapper
{
    public static PixChargeScheme ToRequest(this CheckoutSessionCreationScheme parameters, Payment payment)
    {
        return new PixChargeScheme
        {
            Amount = Math.Round(parameters.Amount * 100, MidpointRounding.AwayFromZero),
            ExpiresIn = 600,
            Metadata = new Dictionary<string, string>
            {
                { "payment.identifier", payment.Id },
                { "payer.identifier", parameters.Payer.Identifier },
                { "payer.email", parameters.Payer.Username }
            }
        };
    }
}
