namespace Comanda.Subscriptions.WebApi.Controllers;

[ApiController]
[Route("api/v1/subscriptions")]
public sealed class SubscriptionsController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> GetSubscriptionsAsync(
        [FromQuery] SubscriptionsFetchParameters request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),
        };
    }

    [HttpPost]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> CreateCheckoutSessionAsync(
        [FromBody] CheckoutSessionCreationScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-D910F */
            { IsFailure: true } when result.Error == SubscriptionErrors.PlanNotSupported =>
                StatusCode(StatusCodes.Status422UnprocessableEntity, result.Error)
        };
    }

    [HttpDelete("{id}")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> CancelSubscriptionAsync(
        [FromQuery] SubscriptionCancelationScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        // we’re not actually receiving the subscription id as a query parameter — it’s taken from the route.
        // this is just an elegant way to instantiate the request record without explicitly using new()

        var result = await dispatcher.DispatchAsync(request with { SubscriptionId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-DF352 */
            { IsFailure: true } when result.Error == SubscriptionErrors.SubscriptionDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-06F07 */
            { IsFailure: true } when result.Error == SubscriptionErrors.SubscriptionAlreadyCanceled =>
                StatusCode(StatusCodes.Status422UnprocessableEntity, result.Error)
        };
    }
}
