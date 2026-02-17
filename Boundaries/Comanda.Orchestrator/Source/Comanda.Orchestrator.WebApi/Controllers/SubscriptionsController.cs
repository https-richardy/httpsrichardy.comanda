namespace Comanda.Orchestrator.WebApi.Controllers;

[ApiController]
[Route("api/v1/subscriptions")]
public sealed class SubscriptionsController(IDispatcher dispatcher) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Permissions.Subscribe)]
    public async Task<IActionResult> CreateCheckoutSessionAsync(
        [FromBody] SubscriptionCheckoutSessionCreationScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == SubscriptionErrors.PlanNotSupported =>
                StatusCode(StatusCodes.Status422UnprocessableEntity, result.Error),

            { IsFailure: true } when result.Error == CommonErrors.RateLimitExceeded =>
                StatusCode(StatusCodes.Status429TooManyRequests, result.Error),

            { IsFailure: true } when result.Error == CommonErrors.OperationFailed =>
                StatusCode(StatusCodes.Status500InternalServerError, result.Error),
        };
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Permissions.CancelSubscription)]
    public async Task<IActionResult> CancelSubscriptionAsync(
        [FromQuery] SubscriptionCancelationScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        // we’re not actually receiving the subscription id as a query parameter, it’s taken from the route.
        // this is just an elegant way to instantiate the request record without explicitly using new()

        var result = await dispatcher.DispatchAsync(request with { SubscriptionId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == SubscriptionErrors.SubscriptionDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == CommonErrors.RateLimitExceeded =>
                StatusCode(StatusCodes.Status429TooManyRequests, result.Error),

            { IsFailure: true } when result.Error == CommonErrors.OperationFailed =>
                StatusCode(StatusCodes.Status500InternalServerError, result.Error),
        };
    }

    [HttpGet("callback/success")]
    [AllowAnonymous]
    public async Task<IActionResult> OnSuccessCallbackAsync(
        [FromQuery] CallbackSuccessfulCheckoutParameters request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == CommonErrors.RateLimitExceeded =>
                StatusCode(StatusCodes.Status429TooManyRequests, result.Error),

            { IsFailure: true } when result.Error == CommonErrors.OperationFailed =>
                StatusCode(StatusCodes.Status500InternalServerError, result.Error),
        };
    }

    [HttpGet("callback/fail")]
    [AllowAnonymous]
    public async Task<IActionResult> OnCancelCallbackAsync(
        [FromQuery] CallbackFailedCheckoutParameters request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == CommonErrors.RateLimitExceeded =>
                StatusCode(StatusCodes.Status429TooManyRequests, result.Error),

            { IsFailure: true } when result.Error == CommonErrors.OperationFailed =>
                StatusCode(StatusCodes.Status500InternalServerError, result.Error),
        };
    }
}
