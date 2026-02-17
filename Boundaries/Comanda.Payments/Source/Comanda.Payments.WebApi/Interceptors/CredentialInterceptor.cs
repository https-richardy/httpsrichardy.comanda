namespace Comanda.Payments.WebApi.Interceptors;

public sealed class CredentialInterceptor(IHttpContextAccessor accessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // we receive the credential used for payment processing via the "x-credential" header
        // this interceptor adds the authorization header with the appropriate value

        // this is required because we now support multiple merchants
        // payments are processed on behalf of the establishment

        var httpContext = accessor.HttpContext;
        if (httpContext is null)
        {
            return base.SendAsync(request, cancellationToken);
        }

        if (httpContext.Request.Headers.TryGetValue(Headers.Credential, out var credentials))
        {
            var credential = credentials.FirstOrDefault();
            var authorization = new AuthenticationHeaderValue("Bearer", credential);

            // the credential captured from the incoming request is used as a bearer token for authorization
            // https://docs.abacatepay.com/pages/authentication
            if (!string.IsNullOrWhiteSpace(credential))
            {
                request.Headers.Authorization = authorization;
            }

        }

        return base.SendAsync(request, cancellationToken);
    }
}
