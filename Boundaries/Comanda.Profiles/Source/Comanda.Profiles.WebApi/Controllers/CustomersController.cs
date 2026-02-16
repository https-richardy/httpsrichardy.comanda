namespace Comanda.Profiles.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/customers")]
public sealed class CustomersController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> GetCustomersAsync(
        [FromQuery] FetchCustomersParameters request, CancellationToken cancellation)
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
    public async Task<IActionResult> CreateCustomerAsync(
        [FromBody] CustomerCreationScheme request, CancellationToken cancellation)
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
    public async Task<IActionResult> EditCustomerAsync(
        [FromBody] EditCustomerScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { CustomerId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            { IsFailure: true } when result.Error == CustomerErrors.CustomerDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error)
        };
    }

    [HttpDelete("{id}")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> DeleteCustomerAsync(
        [FromQuery] CustomerDeletionScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { CustomerId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status204NoContent),

            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            { IsFailure: true } when result.Error == CustomerErrors.CustomerDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error)
        };
    }

    [HttpGet("{id}/addresses")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> GetCustomerAddressAsync(
        [FromQuery] FetchCustomerAddressesParameters request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { CustomerId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            { IsFailure: true } when result.Error == CustomerErrors.CustomerDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpPost("{id}/addresses")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> AssignCustomerAddressAsync(
        [FromBody] AssignCustomerAddressScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { CustomerId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status201Created, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            { IsFailure: true } when result.Error == CustomerErrors.CustomerDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-4901F */
            { IsFailure: true } when result.Error == CustomerErrors.AddressAlreadyAssigned =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }

    [HttpPut("{id}/addresses")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> EditCustomerAddressAsync(
        [FromBody] EditCustomerAddressScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { CustomerId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            { IsFailure: true } when result.Error == CustomerErrors.CustomerDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-2616B */
            { IsFailure: true } when result.Error == CustomerErrors.AddressDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-4901F */
            { IsFailure: true } when result.Error == CustomerErrors.AddressAlreadyAssigned =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }

    [HttpDelete("{id}/addresses")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> DeleteCustomerAddressAsync(
        [FromBody] DeleteCustomerAddressScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { CustomerId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status204NoContent),

            /* for tracking purposes: raise error #COMANDA-ERROR-AF04C */
            { IsFailure: true } when result.Error == CustomerErrors.CustomerDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-2616B */
            { IsFailure: true } when result.Error == CustomerErrors.AddressDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }
}
