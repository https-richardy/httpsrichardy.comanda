namespace Comanda.Subscriptions.Infrastructure.Gateways.Mappers;

public static class StripeMapper
{
    public static SessionCreateOptions ToOptions(this CheckoutSessionCreationScheme scheme)
    {
        var successUrl = scheme.Callbacks.SuccessUrl.WithoutTrailingSlash();
        var cancelUrl = scheme.Callbacks.CancelUrl.WithoutTrailingSlash();

        return new SessionCreateOptions
        {
            PaymentMethodTypes = ["card"],
            Mode = "subscription",
            SuccessUrl = $"{successUrl}?sessionId={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{cancelUrl}?sessionId={{CHECKOUT_SESSION_ID}}",
            Metadata = new Dictionary<string, string>
            {
                { "subscriber.identifier", scheme.Subscriber.Identifier },
                { "subscriber.username", scheme.Subscriber.Username }
            },
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = "price_1SPVkxH5XTdzSAquQEpr7J9i",
                    Quantity = 1
                }
            ]
        };
    }
}