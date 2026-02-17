namespace Comanda.Orders.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/orders")]
public sealed class OrdersController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> GetOrdersAsync([FromQuery] OrdersFetchParameters request, CancellationToken cancellation)
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
                StatusCode(StatusCodes.Status200OK, result.Data.Items),
        };
    }

    [HttpPost]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> CreateOrderAsync([FromBody] OrderCreationScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status201Created, result.Data),
        };
    }

    [HttpPut("{id}")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> UpdateOrderAsync(
        [FromBody] OrderModificationScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { Id = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-2D7A5 */
            { IsFailure: true } when result.Error == OrderErrors.OrderDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error)
        };
    }
}
