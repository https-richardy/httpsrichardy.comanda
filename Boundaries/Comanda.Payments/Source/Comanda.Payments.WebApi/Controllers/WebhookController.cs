namespace Comanda.Payments.WebApi.Controllers;

[ApiController]
[Route("webhook")]
public sealed class WebhookController(IEventDispatcher dispatcher) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> OnWebhookNotificationAsync(
        [FromBody] BillingPaidNotificationScheme request, CancellationToken cancellation)
    {
        await dispatcher.DispatchAsync(request, cancellation);

        return StatusCode(StatusCodes.Status200OK);
    }
}
