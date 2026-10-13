namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Stub <see cref="HttpMessageHandler"/> that answers each request via a delegate and records the requested URLs.
/// </summary>
internal sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;

    public StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
    {
        ArgumentNullException.ThrowIfNull(responder);
        _responder = responder;
    }

    public List<string> RequestedUrls { get; } = [];

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        RequestedUrls.Add(request.RequestUri!.AbsoluteUri);
        return Task.FromResult(_responder(request));
    }
}
