namespace Comanda.Payments.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/activities")]
public sealed class ActivitiesController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> GetActivitiesAsync(
        [FromQuery] ActivityFetchParameters request, CancellationToken cancellation)
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
