namespace Comanda.Profiles.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/owners")]
public sealed class OwnersController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> GetOwnersAsync(
        [FromQuery] FetchOwnersParameters request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        /* applies pagination navigation links according to RFC 8288 (web linking) */
        /* https://datatracker.ietf.org/doc/html/rfc8288 */
        if (result.IsSuccess && result.Data is not null)
        {
            Response.WithPagination(result.Data);
            Response.WithWebLinking(result.Data, Request);
        }

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status200OK, result.Data.Items)
        };
    }

    [HttpPost]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> CreateOwnerAsync(
        [FromBody] OwnerCreationScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status201Created, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-76A71 */
            { IsFailure: true } when result.Error == ProfileErrors.ProfileAlreadyExists =>
                StatusCode(StatusCodes.Status409Conflict, result.Error)
        };
    }

    [HttpPut("{id}")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> EditOwnerAsync(
        [FromBody] EditOwnerScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { OwnerId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-0831D */
            { IsFailure: true } when result.Error == OwnerErrors.OwnerDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error)
        };
    }

    [HttpDelete("{id}")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> DeleteOwnerAsync(
        [FromQuery] OwnerDeletionScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { OwnerId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status204NoContent),

            /* for tracking purposes: raise error #COMANDA-ERROR-0831D */
            { IsFailure: true } when result.Error == OwnerErrors.OwnerDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error)
        };
    }
}
