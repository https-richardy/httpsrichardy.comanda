namespace Comanda.Stores.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/establishments")]
public sealed class EstablishmentsController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> GetEstablishmentsAsync(
        [FromQuery] EstablishmentsFetchParameters request, CancellationToken cancellation)
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
    public async Task<IActionResult> CreateEstablishmentAsync(
        [FromBody] EstablishmentCreationScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == EstablishmentErrors.OwnerAlreadyHasEstablishment =>
                StatusCode(StatusCodes.Status409Conflict, result.Error)
        };
    }

    [HttpPut("{id}")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> UpdateEstablishmentAsync(
        [FromBody] EstablishmentEditionScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { EstablishmentId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-0A2DF */
            { IsFailure: true } when result.Error == EstablishmentErrors.EstablishmentDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error)
        };
    }

    [HttpDelete("{id}")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> DeleteEstablishmentAsync(
        [FromQuery] EstablishmentDeletionScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { EstablishmentId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status204NoContent),

            /* for tracking purposes: raise error #COMANDA-ERROR-0A2DF */
            { IsFailure: true } when result.Error == EstablishmentErrors.EstablishmentDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error)
        };
    }

    [HttpGet("{id}/products")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> GetProductsAsync(
        [FromQuery] ProductsFetchParameters request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { EstablishmentId = id }, cancellation);

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

    [HttpPost("{id}/products")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> CreateProductAsync(
        [FromBody] ProductCreationScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { EstablishmentId = id }, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status201Created, result.Data),
        };
    }

    [HttpPut("{id}/products/{productId}")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> UpdateProductAsync(
        [FromBody] ProductEditionScheme request, [FromRoute] string id, [FromRoute] string productId, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { EstablishmentId = id, ProductId = productId }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-0A2DF */
            { IsFailure: true } when result.Error == EstablishmentErrors.EstablishmentDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-C2C1B */
            { IsFailure: true } when result.Error == ProductErrors.ProductDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-9FF68 */
            { IsFailure: true } when result.Error == ProductErrors.ProductBelongsToAnotherEstablishment =>
                StatusCode(StatusCodes.Status422UnprocessableEntity, result.Error)
        };
    }

    [HttpPut("{id}/products/{productId}/image")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> UploadProductImageAsync(
        [FromRoute] string id, [FromRoute] string productId, [FromForm] IFormFile file, CancellationToken cancellation)
    {
        var stream = file.OpenReadStream();
        var result = await dispatcher.DispatchAsync(stream.AsImage(productId, id), cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status204NoContent),

            /* for tracking purposes: raise error #COMANDA-ERROR-0A2DF */
            { IsFailure: true } when result.Error == EstablishmentErrors.EstablishmentDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-C2C1B */
            { IsFailure: true } when result.Error == ProductErrors.ProductDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpDelete("{id}/products/{productId}")]
    [Stability(Stability.Stable)]
    public async Task<IActionResult> DeleteProductAsync(
        [FromQuery] ProductDeletionScheme request, [FromRoute] string id, [FromRoute] string productId, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { EstablishmentId = id, ProductId = productId }, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status204NoContent),

            /* for tracking purposes: raise error #COMANDA-ERROR-0A2DF */
            { IsFailure: true } when result.Error == EstablishmentErrors.EstablishmentDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-C2C1B */
            { IsFailure: true } when result.Error == ProductErrors.ProductDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-9FF68 */
            { IsFailure: true } when result.Error == ProductErrors.ProductBelongsToAnotherEstablishment =>
                StatusCode(StatusCodes.Status422UnprocessableEntity, result.Error)
        };
    }

    [HttpGet("{id}/integrations/credentials")]
    [Stability(Stability.Experimental)]
    public async Task<IActionResult> GetIntegrationCredentialAsync(
        [FromQuery] CredentialsFetchParameters request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { EstablishmentId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-0A2DF */
            { IsFailure: true } when result.Error == EstablishmentErrors.EstablishmentDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error)
        };
    }

    [HttpPost("{id}/integrations/credentials")]
    [Stability(Stability.Experimental)]
    public async Task<IActionResult> AssignIntegrationCredentialAsync(
        [FromBody] CredentialCreationScheme request, [FromRoute] string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { EstablishmentId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status201Created, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-0A2DF */
            { IsFailure: true } when result.Error == EstablishmentErrors.EstablishmentDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error)
        };
    }

    [HttpPut("{id}/integrations/credentials/{credentialId}")]
    [Stability(Stability.Experimental)]
    public async Task<IActionResult> UpdateCredentialAsync(
        [FromBody] CredentialEditScheme request, [FromRoute] string id, [FromRoute] string credentialId, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { EstablishmentId = id, CredentialId = credentialId }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            /* for tracking purposes: raise error #COMANDA-ERROR-0A2DF */
            { IsFailure: true } when result.Error == EstablishmentErrors.EstablishmentDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            /* for tracking purposes: raise error #COMANDA-ERROR-2147F */
            { IsFailure: true } when result.Error == CredentialErrors.CredentialDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error)
        };
    }
}
