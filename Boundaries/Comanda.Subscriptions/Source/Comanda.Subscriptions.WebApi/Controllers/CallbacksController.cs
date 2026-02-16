namespace Comanda.Subscriptions.WebApi.Controllers;

[ApiController]
[Route("api/v1/callback")]
public sealed class CallbacksController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet("success")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> OnSuccessCallbackAsync(
        [FromQuery] CallbackSuccessfulCheckoutParameters request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),
        };
    }

    [HttpGet("cancel")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> OnCancelCallbackAsync(
        [FromQuery] CallbackFailedCheckoutParameters request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),
        };
    }
}