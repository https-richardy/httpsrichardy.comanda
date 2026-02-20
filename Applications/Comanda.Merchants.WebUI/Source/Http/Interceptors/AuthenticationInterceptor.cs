namespace Comanda.Merchants.WebUI.Http.Interceptors;

public sealed class AuthenticationInterceptor(ILocalStorageGateway localStorage) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await localStorage.GetAsStringAsync(Storage.SecurityToken, cancellationToken);

        /* https://www.rfc-editor.org/rfc/rfc6750 */
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new(AuthenticationDefaults.Scheme, token);

        return await base.SendAsync(request, cancellationToken);
    }
}
