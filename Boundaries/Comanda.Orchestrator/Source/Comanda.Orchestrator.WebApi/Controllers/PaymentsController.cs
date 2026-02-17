namespace Comanda.Orchestrator.WebApi.Controllers;

#pragma warning disable S6960

[ApiController]
[Route("api/v1/payments")]
public sealed class PaymentsController(IDispatcher dispatcher, IEventDispatcher eventDispatcher) : ControllerBase
{
    [HttpPost("online")]
    [Authorize(Roles = Permissions.MakePayment)]
    public async Task<IActionResult> CreateOnlineChargeAsync(
        [FromBody] CheckoutSessionCreationScheme request, [FromHeader(Name = Headers.Credential)] string credential, CancellationToken cancellation)
    {
        var charge = new OnlineChargeScheme(request, credential);
        var result = await dispatcher.DispatchAsync(charge, cancellation);

        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* returning 502 Bad Gateway because an unexpected or invalid response was received from the external provider */
            /* https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status/502 */
            { IsFailure: true } when result.Error == AbacatePayErrors.OperationFailed =>
                StatusCode(StatusCodes.Status502BadGateway, result.Error),

            { IsFailure: true } when result.Error == CommonErrors.OperationFailed =>
                StatusCode(StatusCodes.Status500InternalServerError, result.Error),

            { IsFailure: true } when result.Error == CommonErrors.RateLimitExceeded =>
                StatusCode(StatusCodes.Status429TooManyRequests, result.Error)
        };
    }

    [HttpPost("offline")]
    [Authorize(Roles = Permissions.MakePayment)]
    public async Task<IActionResult> CreateOfflinePaymentAsync([FromBody] OfflinePaymentScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == CommonErrors.OperationFailed =>
                StatusCode(StatusCodes.Status500InternalServerError, result.Error),

            { IsFailure: true } when result.Error == CommonErrors.RateLimitExceeded =>
                StatusCode(StatusCodes.Status429TooManyRequests, result.Error)
        };
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> OnWebhookNotificationAsync(
        [FromBody] BillingPaidNotificationScheme request, CancellationToken cancellation)
    {
        await eventDispatcher.DispatchAsync(request, cancellation);

        return StatusCode(StatusCodes.Status200OK);
    }
}
