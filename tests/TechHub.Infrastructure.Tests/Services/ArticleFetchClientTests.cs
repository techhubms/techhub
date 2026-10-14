using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

public class ArticleFetchClientTests
{
    private static ArticleFetchClient CreateClient(StubHttpMessageHandler handler)
        => new(new HttpClient(handler), NullLogger<ArticleFetchClient>.Instance);

    private static HttpResponseMessage Redirect(string location, HttpStatusCode status = HttpStatusCode.Found)
    {
        var response = new HttpResponseMessage(status);
        response.Headers.Location = new Uri(location, UriKind.RelativeOrAbsolute);
        return response;
    }

    [Fact]
    public async Task FetchHtmlAsync_SuccessResponse_ReturnsBody()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("<html>ok</html>")
        });
        var client = CreateClient(handler);

        // Act
        var html = await client.FetchHtmlAsync("https://example.com/a", TestContext.Current.CancellationToken);

        // Assert
        html.Should().Be("<html>ok</html>");
    }

    [Fact]
    public async Task FetchHtmlAsync_NotFound_ReturnsNull()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.NotFound));
        var client = CreateClient(handler);

        // Act
        var html = await client.FetchHtmlAsync("https://example.com/missing", TestContext.Current.CancellationToken);

        // Assert
        html.Should().BeNull();
    }

    [Fact]
    public async Task FetchHtmlAsync_RelativeRedirect_IsResolvedAgainstRequestUrl()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(request =>
            request.RequestUri!.AbsolutePath == "/old"
                ? Redirect("/new")
                : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("moved") });
        var client = CreateClient(handler);

        // Act
        var html = await client.FetchHtmlAsync("https://example.com/old", TestContext.Current.CancellationToken);

        // Assert
        html.Should().Be("moved");
        handler.RequestedUrls.Should().Equal("https://example.com/old", "https://example.com/new");
    }

    [Fact]
    public async Task FetchHtmlAsync_HttpsToHttpDowngradeRedirect_IsFollowed()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(request =>
            request.RequestUri!.Scheme == "https" && request.RequestUri.Host == "www.example.com"
                ? Redirect("http://example.com/")
                : request.RequestUri.Scheme == "http"
                    ? Redirect("https://example.com/")
                    : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("final") });
        var client = CreateClient(handler);

        // Act
        var html = await client.FetchHtmlAsync("https://www.example.com/", TestContext.Current.CancellationToken);

        // Assert
        html.Should().Be("final");
    }

    [Fact]
    public async Task FetchHtmlAsync_RedirectLoop_StopsAfterFiveRedirectsAndReturnsNull()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => Redirect("https://example.com/loop"));
        var client = CreateClient(handler);

        // Act
        var html = await client.FetchHtmlAsync("https://example.com/loop", TestContext.Current.CancellationToken);

        // Assert
        html.Should().BeNull();
        handler.RequestedUrls.Should().HaveCount(6, "the initial request plus five followed redirects");
    }

    [Fact]
    public async Task FetchHtmlAsync_HttpRequestException_ReturnsNull()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => throw new HttpRequestException("boom"));
        var client = CreateClient(handler);

        // Act
        var html = await client.FetchHtmlAsync("https://example.com/", TestContext.Current.CancellationToken);

        // Assert
        html.Should().BeNull();
    }
}
