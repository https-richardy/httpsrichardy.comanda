namespace Comanda.Orchestrator.Application.Payloads.Payments;

public sealed record OnlineChargeScheme(CheckoutSessionCreationScheme Payload, string SecretKey) :
    IDispatchable<Result<CheckoutSession>>;
