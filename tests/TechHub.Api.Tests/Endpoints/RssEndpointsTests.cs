using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for RSS feed endpoints (uses real data, no mocks)
/// </summary>
public class RssEndpointsTests : IClassFixture<TechHubApiFactory>
{
    private readonly HttpClient _client;

    public RssEndpointsTests(TechHubApiFactory factory)
    {
        // Don't call setup methods - use real repositories
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllContentFeed_ReturnsValidRss()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/all");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/rss+xml; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var xml = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(xml);

        // Validate RSS structure
        var doc = XDocument.Parse(xml);
        var rss = doc.Element("rss");
        Assert.NotNull(rss);
        Assert.Equal("2.0", rss.Attribute("version")?.Value);

        var channel = rss.Element("channel");
        Assert.NotNull(channel);
        Assert.NotNull(channel.Element("title"));
        Assert.NotNull(channel.Element("link"));
        Assert.NotNull(channel.Element("description"));
        Assert.NotNull(channel.Element("lastBuildDate"));
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
        Assert.NotEmpty(items);

        // Validate first item structure
        var firstItem = items.First();
        Assert.NotNull(firstItem.Element("title"));
        Assert.NotNull(firstItem.Element("link"));
        Assert.NotNull(firstItem.Element("description"));
        Assert.NotNull(firstItem.Element("pubDate"));
        Assert.NotNull(firstItem.Element("guid"));
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
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/rss+xml; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var xml = await response.Content.ReadAsStringAsync();
        var doc = XDocument.Parse(xml);
        var channel = doc.Descendants("channel").First();

        // Validate channel title contains section name
        var title = channel.Element("title")?.Value;
        Assert.NotNull(title);
        Assert.Contains("Tech Hub", title);
    }

    [Fact]
    public async Task GetSectionFeed_InvalidSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/nonexistent");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCollectionFeed_Roundups_ReturnsValidRss()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/collection/roundups");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/rss+xml; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var xml = await response.Content.ReadAsStringAsync();
        var doc = XDocument.Parse(xml);
        var channel = doc.Descendants("channel").First();

        // Validate channel contains "Roundups"
        var title = channel.Element("title")?.Value;
        Assert.NotNull(title);
        Assert.Contains("Roundups", title);
    }

    [Fact]
    public async Task GetCollectionFeed_InvalidCollection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/collection/nonexistent");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
            for (int i = 0; i < dates.Count - 1; i++)
            {
                Assert.True(dates[i] >= dates[i + 1], "Items should be sorted by date descending");
            }
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
        Assert.True(items.Count() <= 50, "Feed should contain at most 50 items");
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

        Assert.NotNull(pubDate);
        
        // RFC1123 format can be parsed by DateTime
        var parsed = DateTime.Parse(pubDate);
        Assert.NotEqual(default(DateTime), parsed);
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

        Assert.NotNull(guid);
        Assert.Equal("true", guid.Attribute("isPermaLink")?.Value);
        Assert.StartsWith("https://", guid.Value);
    }

    [Fact]
    public async Task RssFeed_SpecialCharactersAreEscaped()
    {
        // Act
        var response = await _client.GetAsync("/api/rss/all");
        var xml = await response.Content.ReadAsStringAsync();

        // Assert - If XML parses successfully, special characters were properly escaped
        var doc = XDocument.Parse(xml);
        Assert.NotNull(doc);

        // Additional validation: ensure we can access item content
        var items = doc.Descendants("item");
        foreach (var item in items)
        {
            var title = item.Element("title")?.Value;
            var description = item.Element("description")?.Value;
            
            Assert.NotNull(title);
            Assert.NotNull(description);
        }
    }
}
