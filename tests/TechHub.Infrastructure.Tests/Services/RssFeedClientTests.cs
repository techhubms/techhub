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
        xml.Failed.Should().BeFalse();
        xml.Xml.Should().Be("<rss />");
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound, "HTTP 404 (NotFound)")]
    [InlineData(HttpStatusCode.InternalServerError, "HTTP 500 (InternalServerError)")]
    public async Task FetchFeedXmlAsync_ErrorStatus_ReturnsFailureWithStatus(HttpStatusCode status, string expectedError)
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(status));
        var client = new RssFeedClient(new HttpClient(handler), NullLogger<RssFeedClient>.Instance);

        // Act
        var xml = await client.FetchFeedXmlAsync("https://example.com/feed.xml", TestContext.Current.CancellationToken);

        // Assert
        xml.Failed.Should().BeTrue();
        xml.Error.Should().Be(expectedError);
    }

    [Fact]
    public async Task FetchFeedXmlAsync_ConnectionFailure_ReturnsFailure()
    {
        // Arrange
        using var handler = new StubHttpMessageHandler(_ => throw new HttpRequestException("blocked"));
        var client = new RssFeedClient(new HttpClient(handler), NullLogger<RssFeedClient>.Instance);

        // Act
        var xml = await client.FetchFeedXmlAsync("http://169.254.169.254/", TestContext.Current.CancellationToken);

        // Assert
        xml.Error.Should().Be("Connection failed: blocked");
    }

    [Fact]
    public async Task FetchFeedXmlAsync_ResilienceTimeout_ReturnsTimedOut()
    {
        // Arrange: the Polly timeout of the resilience handler is not an HttpRequestException
        using var handler = new StubHttpMessageHandler(_ => throw new TimeoutRejectedException("The operation didn't complete within the allowed timeout of '00:01:30'."));
        var client = new RssFeedClient(new HttpClient(handler), NullLogger<RssFeedClient>.Instance);

        // Act
        var xml = await client.FetchFeedXmlAsync("https://example.com/feed.xml", TestContext.Current.CancellationToken);

        // Assert
        xml.Error.Should().Be("Timed out");
    }

    [Fact]
    public async Task FetchFeedXmlAsync_HttpClientTimeout_ReturnsTimedOut()
    {
        // Arrange: HttpClient.Timeout surfaces as a TaskCanceledException that the caller did not request
        using var handler = new StubHttpMessageHandler(_ => throw new TaskCanceledException("timed out", new TimeoutException()));
        var client = new RssFeedClient(new HttpClient(handler), NullLogger<RssFeedClient>.Instance);

        // Act
        var xml = await client.FetchFeedXmlAsync("https://example.com/feed.xml", TestContext.Current.CancellationToken);

        // Assert
        xml.Error.Should().Be("Timed out");
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
