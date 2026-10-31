using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Polly.Timeout;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

public class RssFeedClientTests
{
    [Fact]
    public async Task FetchFeedXmlAsync_SuccessResponse_ReturnsXml()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("<rss />")
        });
        var client = new RssFeedClient(new HttpClient(handler), NullLogger<RssFeedClient>.Instance);

        // Act
        var xml = await client.FetchFeedXmlAsync("https://example.com/feed.xml", TestContext.Current.CancellationToken);

        // Assert
        xml.Should().Be("<rss />");
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task FetchFeedXmlAsync_ErrorStatus_ReturnsNull(HttpStatusCode status)
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(status));
        var client = new RssFeedClient(new HttpClient(handler), NullLogger<RssFeedClient>.Instance);

        // Act
        var xml = await client.FetchFeedXmlAsync("https://example.com/feed.xml", TestContext.Current.CancellationToken);

        // Assert
        xml.Should().BeNull();
    }

    [Fact]
    public async Task FetchFeedXmlAsync_ConnectionFailure_ReturnsNull()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => throw new HttpRequestException("blocked"));
        var client = new RssFeedClient(new HttpClient(handler), NullLogger<RssFeedClient>.Instance);

        // Act
        var xml = await client.FetchFeedXmlAsync("http://169.254.169.254/", TestContext.Current.CancellationToken);

        // Assert
        xml.Should().BeNull();
    }

    [Fact]
    public async Task FetchFeedXmlAsync_ResilienceTimeout_ReturnsNull()
    {
        // Arrange: the Polly timeout of the resilience handler is not an HttpRequestException
        using var handler = new StubHttpMessageHandler(_ => throw new TimeoutRejectedException("The operation didn't complete within the allowed timeout of '00:01:30'."));
        var client = new RssFeedClient(new HttpClient(handler), NullLogger<RssFeedClient>.Instance);

        // Act
        var xml = await client.FetchFeedXmlAsync("https://example.com/feed.xml", TestContext.Current.CancellationToken);

        // Assert
        xml.Should().BeNull();
    }

    [Fact]
    public async Task FetchFeedXmlAsync_HttpClientTimeout_ReturnsNull()
    {
        // Arrange: HttpClient.Timeout surfaces as a TaskCanceledException that the caller did not request
        using var handler = new StubHttpMessageHandler(_ => throw new TaskCanceledException("timed out", new TimeoutException()));
        var client = new RssFeedClient(new HttpClient(handler), NullLogger<RssFeedClient>.Instance);

        // Act
        var xml = await client.FetchFeedXmlAsync("https://example.com/feed.xml", TestContext.Current.CancellationToken);

        // Assert
        xml.Should().BeNull();
    }

    [Fact]
    public async Task FetchFeedXmlAsync_CallerCancellation_Propagates()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK));
        var client = new RssFeedClient(new HttpClient(handler), NullLogger<RssFeedClient>.Instance);

        // Act
        var act = () => client.FetchFeedXmlAsync("https://example.com/feed.xml", cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
