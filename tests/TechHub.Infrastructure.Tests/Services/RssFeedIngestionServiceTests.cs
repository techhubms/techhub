using System.Xml;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using TechHub.Core.Configuration;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Tests for RssFeedIngestionService XML parsing safety and feed ingestion behavior.
/// Validates XXE protection and feed parsing.
/// </summary>
public class RssFeedIngestionServiceTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHandler;
    private readonly HttpClient _httpClient;
    private readonly RssFeedIngestionService _service;
    private bool _disposed;

    public RssFeedIngestionServiceTests()
    {
        _mockHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHandler.Object);
        var options = Options.Create(new ContentProcessorOptions
        {
            RequestTimeoutSeconds = 10,
            ItemAgeLimitDays = 365
        });
        _service = new RssFeedIngestionService(
            _httpClient,
            options,
            NullLogger<RssFeedIngestionService>.Instance);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _httpClient.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task IngestAsync_WithDtdInXml_RejectsAndReturnsEmpty()
    {
        // Arrange - XML with DTD that should be rejected
        var xmlWithDtd = """
            <?xml version="1.0" encoding="UTF-8"?>
            <!DOCTYPE foo [<!ENTITY xxe SYSTEM "file:///etc/passwd">]>
            <rss version="2.0">
                <channel>
                    <title>Test Feed</title>
                    <item>
                        <title>&xxe;</title>
                        <link>https://example.com/article</link>
                        <pubDate>Mon, 24 Mar 2026 12:00:00 GMT</pubDate>
                    </item>
                </channel>
            </rss>
            """;

        SetupHttpResponse("https://example.com/feed.xml", xmlWithDtd);
        var feedConfig = new FeedConfig { Name = "Test", Url = "https://example.com/feed.xml", OutputDir = "_blogs" };

        // Act
        var items = await _service.IngestAsync(feedConfig, TestContext.Current.CancellationToken);

        // Assert - DTD processing is prohibited, so parsing should fail and return empty
        items.Should().BeEmpty();
    }

    [Fact]
    public async Task IngestAsync_WithValidRssFeed_ParsesItems()
    {
        // Arrange
        var validRss = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <rss version="2.0">
                <channel>
                    <title>Test Feed</title>
                    <item>
                        <title>Test Article</title>
                        <link>https://example.com/article</link>
                        <description>Article description</description>
                        <pubDate>{DateTimeOffset.UtcNow.AddDays(-5):R}</pubDate>
                    </item>
                </channel>
            </rss>
            """;

        SetupHttpResponse("https://example.com/feed.xml", validRss);
        var feedConfig = new FeedConfig { Name = "Test", Url = "https://example.com/feed.xml", OutputDir = "_blogs" };

        // Act
        var items = await _service.IngestAsync(feedConfig, TestContext.Current.CancellationToken);

        // Assert
        items.Should().ContainSingle();
        items[0].Title.Should().Be("Test Article");
        items[0].ExternalUrl.Should().Be("https://example.com/article");
    }

    [Fact]
    public async Task IngestAsync_WithValidAtomFeed_ParsesItems()
    {
        // Arrange
        var validAtom = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <feed xmlns="http://www.w3.org/2005/Atom">
                <title>Test Feed</title>
                <entry>
                    <title>Atom Article</title>
                    <link rel="alternate" href="https://example.com/atom-article"/>
                    <summary>Atom description</summary>
                    <updated>{DateTimeOffset.UtcNow.AddDays(-5):O}</updated>
                </entry>
            </feed>
            """;

        SetupHttpResponse("https://example.com/atom.xml", validAtom);
        var feedConfig = new FeedConfig { Name = "AtomTest", Url = "https://example.com/atom.xml", OutputDir = "_blogs" };

        // Act
        var items = await _service.IngestAsync(feedConfig, TestContext.Current.CancellationToken);

        // Assert
        items.Should().ContainSingle();
        items[0].Title.Should().Be("Atom Article");
    }

    private void SetupHttpResponse(string url, string content)
    {
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(m => m.RequestUri != null && m.RequestUri.ToString() == url),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(content)
            });
    }
}
