namespace Comanda.Payments.TestSuite.Unit.Intecerptors;

public sealed class CredentialInterceptorTests
{
    [Fact(DisplayName = "[interceptor] - when x-credential header is provided then bearer authorization should be injected")]
    public async Task When_CredentialHeaderIsProvided_ThenBearerAuthorizationShouldBeInjected()
    {
        var credential = Identifier.Generate<Payment>();
        var httpContext = new DefaultHttpContext();

        httpContext.Request.Headers[WebApi.Constants.Headers.Credential] = credential;

        var accessor = new HttpContextAccessor
        {
            HttpContext = httpContext
        };

        var probe = new ProbeHandler();
        var interceptor = new CredentialInterceptor(accessor)
        {
            InnerHandler = probe
        };

        var invoker = new HttpMessageInvoker(interceptor);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/payments");

        _ = await invoker.SendAsync(request, TestContext.Current.CancellationToken);

        Assert.NotNull(probe.CapturedRequest);
        Assert.NotNull(probe.CapturedRequest.Headers.Authorization);

        Assert.Equal("Bearer", probe.CapturedRequest.Headers.Authorization.Scheme);
        Assert.Equal(credential, probe.CapturedRequest.Headers.Authorization.Parameter);
    }

    private sealed class ProbeHandler : HttpMessageHandler
    {
        public HttpRequestMessage? CapturedRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CapturedRequest = request;

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}
