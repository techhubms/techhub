using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

public class YouTubeTagClientTests
{
    private const string ApiBaseUrl = "https://ytapi.apps.mattw.io/v3/videos?part=snippet&id=";

    [Fact]
    public void Constructor_NullHttpClient_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new YouTubeTagClient(null!, NullLogger<YouTubeTagClient>.Instance);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("httpClient");
    }

    [Fact]
    public void Constructor_NullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK));
        using var httpClient = new HttpClient(handler);

        // Act
        var act = () => new YouTubeTagClient(httpClient, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("logger");
    }

    [Fact]
    public async Task FetchVideoSnippetAsync_SuccessResponse_ReturnsBodyUnchanged()
    {
        // Arrange
        const string Body = """{"items":[{"snippet":{"tags":["csharp","dotnet"]}}]}""";
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(Body)
        });
        var client = new YouTubeTagClient(new HttpClient(handler), NullLogger<YouTubeTagClient>.Instance);

        // Act
        var result = await client.FetchVideoSnippetAsync("abc123", TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(Body);
    }

    [Fact]
    public async Task FetchVideoSnippetAsync_VideoId_IsAppendedToSnippetApiUrl()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}")
        });
        var client = new YouTubeTagClient(new HttpClient(handler), NullLogger<YouTubeTagClient>.Instance);

        // Act
        await client.FetchVideoSnippetAsync("dQw4w9WgXcQ", TestContext.Current.CancellationToken);

        // Assert
        handler.RequestedUrls.Should().ContainSingle().Which.Should().Be($"{ApiBaseUrl}dQw4w9WgXcQ");
    }

    [Fact]
    public async Task FetchVideoSnippetAsync_VideoIdWithReservedCharacters_IsUrlEscaped()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}")
        });
        var client = new YouTubeTagClient(new HttpClient(handler), NullLogger<YouTubeTagClient>.Instance);

        // Act
        await client.FetchVideoSnippetAsync("a b&part=evil#x", TestContext.Current.CancellationToken);

        // Assert — the id cannot inject extra query parameters or fragments
        handler.RequestedUrls.Should().ContainSingle().Which.Should().Be($"{ApiBaseUrl}a%20b%26part%3Devil%23x");
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.TooManyRequests)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task FetchVideoSnippetAsync_ErrorStatus_ReturnsNull(HttpStatusCode status)
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(status)
        {
            Content = new StringContent("error body")
        });
        var client = new YouTubeTagClient(new HttpClient(handler), NullLogger<YouTubeTagClient>.Instance);

        // Act
        var result = await client.FetchVideoSnippetAsync("abc123", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task FetchVideoSnippetAsync_ConnectionFailure_ReturnsNull()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => throw new HttpRequestException("connection refused"));
        var client = new YouTubeTagClient(new HttpClient(handler), NullLogger<YouTubeTagClient>.Instance);

        // Act
        var result = await client.FetchVideoSnippetAsync("abc123", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }
}
