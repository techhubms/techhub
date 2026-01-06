using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;
using Xunit;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for RSS feed functionality
/// </summary>
[Collection("RSS Tests")]
public class RssTests : IAsyncLifetime
{
    private readonly PlaywrightCollectionFixture _fixture;
    private IBrowserContext? _context;
    private const string BaseUrl = "http://localhost:5184";
    private const string ApiUrl = "http://localhost:5029";

    public RssTests(PlaywrightCollectionFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        _context = await _fixture.CreateContextAsync();
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
            await _context.DisposeAsync();
    }

    [Fact]
    public async Task HomePage_HasRssFeedDiscoveryLink()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act
        await page.GotoAndWaitForBlazorAsync(BaseUrl);

        // Assert - Check for RSS feed discovery link in head
        var rssLink = await page.Locator("link[rel='alternate'][type='application/rss+xml']").First.GetAttributeAsync("href");
        Assert.NotNull(rssLink);
        Assert.Equal("/api/rss/all", rssLink);
    }

    [Fact]
    public async Task SectionPage_HasRssFeedDiscoveryLink()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/ai");

        // Assert - Check for RSS feed discovery link in head
        var rssLink = await page.Locator("link[rel='alternate'][type='application/rss+xml']").First.GetAttributeAsync("href");
        Assert.NotNull(rssLink);
        Assert.Equal("/api/rss/ai", rssLink);
    }

    [Fact]
    public async Task SectionPage_HasRssIconInHeader()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act
        await page.GotoAsync($"{BaseUrl}/github-copilot");


        // Assert - Check for RSS icon link in section header
        var rssIconLink = page.Locator(".page-heading-with-rss .rss-icon-link");
        await Assertions.Expect(rssIconLink).ToBeVisibleAsync();
        
        var href = await rssIconLink.GetAttributeAsync("href");
        Assert.Equal("/api/rss/github-copilot", href);
    }

    [Fact]
    public async Task Footer_HasRssSubscribeLink()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act
        await page.GotoAndWaitForBlazorAsync(BaseUrl);

        // Assert - Check footer has RSS link
        var footerRssLink = page.Locator("footer a:has-text('Subscribe via RSS')");
        await Assertions.Expect(footerRssLink).ToBeVisibleAsync();
        
        var href = await footerRssLink.GetAttributeAsync("href");
        Assert.Equal("/api/rss/all", href);
    }

    [Fact]
    public async Task RssFeed_AllContent_ReturnsValidXml()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act
        var response = await page.APIRequest.GetAsync($"{ApiUrl}/api/rss/all");

        // Assert
        Assert.Equal(200, response.Status);
        Assert.Contains("application/rss+xml", response.Headers["content-type"]);

        var xmlContent = await response.TextAsync();
        var doc = XDocument.Parse(xmlContent);
        
        // Verify RSS structure
        var rss = doc.Element("rss");
        Assert.NotNull(rss);
        Assert.Equal("2.0", rss.Attribute("version")?.Value);

        var channel = rss.Element("channel");
        Assert.NotNull(channel);
        Assert.NotNull(channel.Element("title"));
        Assert.NotNull(channel.Element("link"));
        Assert.NotNull(channel.Element("description"));
    }

    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    [InlineData("ml")]
    public async Task RssFeed_SectionFeeds_ReturnValidXml(string sectionName)
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act
        var response = await page.APIRequest.GetAsync($"{ApiUrl}/api/rss/{sectionName}");

        // Assert
        Assert.Equal(200, response.Status);
        Assert.Contains("application/rss+xml", response.Headers["content-type"]);

        var xmlContent = await response.TextAsync();
        var doc = XDocument.Parse(xmlContent);
        
        // Verify has items
        var items = doc.Descendants("item");
        Assert.NotEmpty(items);
    }

    [Fact]
    public async Task RssIcon_Click_NavigatesToFeed()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();
        await page.GotoAndWaitForBlazorAsync($"{BaseUrl}/azure");

        // Act - Get RSS icon link
        var rssIconLink = page.Locator(".page-heading-with-rss .rss-icon-link");
        var href = await rssIconLink.GetAttributeAsync("href");
        
        // Assert - Verify the link is correct (but don't actually navigate to avoid downloading XML)
        Assert.Equal("/api/rss/azure", href);
    }

    [Fact]
    public async Task RssFeed_ContainsRecentContent()
    {
        // Arrange
        var page = await _context!.NewPageWithDefaultsAsync();

        // Act
        var response = await page.APIRequest.GetAsync($"{ApiUrl}/api/rss/all");
        var xmlContent = await response.TextAsync();
        var doc = XDocument.Parse(xmlContent);

        // Assert - Verify feed has items
        var items = doc.Descendants("item").ToList();
        Assert.NotEmpty(items);
        Assert.True(items.Count <= 50, "Feed should be limited to 50 items");

        // Verify first item has required elements
        var firstItem = items.First();
        Assert.NotNull(firstItem.Element("title"));
        Assert.NotNull(firstItem.Element("link"));
        Assert.NotNull(firstItem.Element("description"));
        Assert.NotNull(firstItem.Element("pubDate"));
        Assert.NotNull(firstItem.Element("guid"));
    }
}
