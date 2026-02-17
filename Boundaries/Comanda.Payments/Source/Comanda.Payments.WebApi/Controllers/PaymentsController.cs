namespace Comanda.Payments.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/payments")]
public sealed class PaymentsController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> GetPaymentsAsync(
        [FromQuery] PaymentsFetchParameters request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),
        };
    }

    [HttpPost("offline")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> CreateOfflinePaymentChargeAsync(
        [FromBody] OfflinePaymentChargeScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status201Created, result.Data),

            { IsFailure: true } when result.Error == PaymentErrors.MethodNotAllowed =>
                StatusCode(StatusCodes.Status422UnprocessableEntity, result.Error)
        };
    }

    [HttpPost("online")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> CreateCheckoutSessionAsync(
        [FromBody] CheckoutSessionCreationScheme request, [FromHeader(Name = Headers.Credential)] string credential, CancellationToken cancellation)
    {
        // this endpoint requires the payment gateway credential to be provided via request headers
        // if it is not present, a 400 response is returned

        if (string.IsNullOrWhiteSpace(credential))
        {
            return StatusCode(StatusCodes.Status400BadRequest, PaymentErrors.CredentialNotProvided);
        }

        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } =>
                StatusCode(StatusCodes.Status400BadRequest, result.Error)
        };
    }

    [HttpPut("{paymentId}/status")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> UpdatePaymentStatusAsync(
        [FromRoute] string paymentId, [FromBody] PaymentStatusUpdateScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { Identifier = paymentId }, cancellation);

        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == PaymentErrors.PaymentDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }
}
