namespace Comanda.Payments.WebApi.Interceptors;

public sealed class CorrelationInterceptor(IHttpContextAccessor accessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var correlation = accessor.HttpContext?.Items[Headers.Correlation]?.ToString();

        if (!string.IsNullOrWhiteSpace(correlation))
        {
            request.Headers.Remove(Headers.Correlation);
            request.Headers.Add(Headers.Correlation, correlation);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
