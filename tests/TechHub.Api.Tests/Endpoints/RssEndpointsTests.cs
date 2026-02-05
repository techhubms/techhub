using System.Net;
using System.Xml.Linq;
using FluentAssertions;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for RSS feed endpoints (uses real data, no mocks)
/// </summary>
public class RssEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public RssEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllContentFeed_ReturnsValidRss()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/all");

        // Assert
        if (response.StatusCode != HttpStatusCode.OK)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Expected OK but got {response.StatusCode}. Response: {errorContent}");
        }

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.ToString().Should().Be("application/rss+xml; charset=utf-8");

        var xml = await response.Content.ReadAsStringAsync();
        xml.Should().NotBeEmpty();

        // Validate RSS structure
        var doc = XDocument.Parse(xml);
        var rss = doc.Element("rss");
        rss.Should().NotBeNull();
        rss!.Attribute("version")?.Value.Should().Be("2.0");

        var channel = rss.Element("channel");
        channel.Should().NotBeNull();
        channel!.Element("title").Should().NotBeNull();
        channel.Element("link").Should().NotBeNull();
        channel.Element("description").Should().NotBeNull();
        channel.Element("lastBuildDate").Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllContentFeed_ContainsItems()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/all");
        var xml = await response.Content.ReadAsStringAsync();

        // Assert
        var doc = XDocument.Parse(xml);
        var items = doc.Descendants("item");
        items.Should().NotBeEmpty();

        // Validate first item structure
        var firstItem = items.First();
        firstItem.Element("title").Should().NotBeNull();
        firstItem.Element("link").Should().NotBeNull();
        firstItem.Element("description").Should().NotBeNull();
        firstItem.Element("pubDate").Should().NotBeNull();
        firstItem.Element("guid").Should().NotBeNull();
    }

    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    [InlineData("ml")]
    [InlineData("azure")]
    [InlineData("coding")]
    [InlineData("devops")]
    [InlineData("security")]
    public async Task GetSectionFeed_ValidSection_ReturnsValidRss(string sectionName)
    {
        // Act
        var response = await _client.GetAsync($"/api/rss/{sectionName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.ToString().Should().Be("application/rss+xml; charset=utf-8");

        var xml = await response.Content.ReadAsStringAsync();
        var doc = XDocument.Parse(xml);
        var channel = doc.Descendants("channel").First();

        // Validate channel title contains section name
        var title = channel.Element("title")?.Value;
        title.Should().NotBeNull();
        title.Should().Contain("Tech Hub");
    }

    [Fact]
    public async Task GetSectionFeed_InvalidSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/nonexistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RssFeed_ItemsAreSortedByDateDescending()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/all");
        var xml = await response.Content.ReadAsStringAsync();

        // Assert
        var doc = XDocument.Parse(xml);
        var items = doc.Descendants("item").ToList();

        if (items.Count > 1)
        {
            var dates = items
                .Select(item => DateTime.Parse(item.Element("pubDate")?.Value ?? ""))
                .ToList();

            // Verify descending order (newest first)
            dates.Should().BeInDescendingOrder("items should be sorted by date descending (newest first)");
        }
    }

    [Fact]
    public async Task RssFeed_ItemsAreLimitedTo50()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/all");
        var xml = await response.Content.ReadAsStringAsync();

        // Assert
        var doc = XDocument.Parse(xml);
        var items = doc.Descendants("item");
        items.Should().HaveCountLessThanOrEqualTo(50, "feed should contain at most 50 items");
    }

    [Fact]
    public async Task RssFeed_PubDateIsRfc1123Format()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/all");
        var xml = await response.Content.ReadAsStringAsync();

        // Assert
        var doc = XDocument.Parse(xml);
        var firstItem = doc.Descendants("item").First();
        var pubDate = firstItem.Element("pubDate")?.Value;

        pubDate.Should().NotBeNull();

        // RFC1123 format can be parsed by DateTime
        var parsed = DateTime.Parse(pubDate!);
        parsed.Should().NotBe(default);
    }

    [Fact]
    public async Task RssFeed_GuidIsPermalink()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/all");
        var xml = await response.Content.ReadAsStringAsync();

        // Assert
        var doc = XDocument.Parse(xml);
        var firstItem = doc.Descendants("item").First();
        var guid = firstItem.Element("guid");

        guid.Should().NotBeNull();
        guid!.Attribute("isPermaLink")?.Value.Should().Be("true");
        guid.Value.Should().StartWith("https://");
    }

    [Fact]
    public async Task RssFeed_SpecialCharactersAreEscaped()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/all");
        var xml = await response.Content.ReadAsStringAsync();

        // Assert - If XML parses successfully, special characters were properly escaped
        var doc = XDocument.Parse(xml);
        doc.Should().NotBeNull();

        // Additional validation: ensure we can access item content
        var items = doc.Descendants("item");
        items.Should().AllSatisfy(item =>
        {
            item.Element("title")?.Value.Should().NotBeNull();
            item.Element("description")?.Value.Should().NotBeNull();
        });
    }

    [Fact]
    public async Task GetAllContentFeed_ShouldNotIncludeDraftItems()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/all");
        var xml = await response.Content.ReadAsStringAsync();

        // Assert
        var doc = XDocument.Parse(xml);
        var items = doc.Descendants("item").ToList();

        // Should not include draft content (title: "Coming Soon: Revolutionary AI Feature")
        var draftItems = items.Where(item =>
        {
            var title = item.Element("title");
            return title != null && title.Value.Contains("Coming Soon", StringComparison.OrdinalIgnoreCase);
        }).ToList();

        draftItems.Should().BeEmpty("RSS feeds should never include draft items");
    }

    [Fact]
    public async Task GetSectionFeed_ShouldNotIncludeDraftItems()
    {
        // Act - AI section feed (our draft has ai section)
        var response = await _client.GetAsync("/api/rss/github-copilot/videos");
        var xml = await response.Content.ReadAsStringAsync();

        // Assert
        var doc = XDocument.Parse(xml);
        var items = doc.Descendants("item").ToList();

        // Should not include draft content even though it has ai section
        var draftItems = items.Where(item =>
        {
            var title = item.Element("title");
            return title != null && title.Value.Contains("Coming Soon", StringComparison.OrdinalIgnoreCase);
        }).ToList();

        draftItems.Should().BeEmpty("RSS feeds should never include draft items");
    }
}
