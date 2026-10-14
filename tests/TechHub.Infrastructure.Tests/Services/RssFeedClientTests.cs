using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
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
}
