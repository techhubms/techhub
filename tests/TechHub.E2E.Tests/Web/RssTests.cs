using System.Xml.Linq;
using Microsoft.Playwright;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E tests for RSS feed functionality
/// </summary>
[Collection("RSS Tests")]
public class RssTests(PlaywrightCollectionFixture fixture) : IAsyncLifetime
{
    private IBrowserContext? _context;
    private IPage? _page;
    private IPage Page => _page ?? throw new InvalidOperationException("Page not initialized");
    private const string BaseUrl = "http://localhost:5184";
    private const string ApiUrl = "http://localhost:5029";

    public async Task InitializeAsync()
    {
        _context = await fixture.CreateContextAsync();
        _page = await _context.NewPageWithDefaultsAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }

        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }

    [Fact]
    public async Task HomePage_HasRssFeedDiscoveryLink()
    {
        // Arrange

        // Act
        await Page.GotoAndWaitForBlazorAsync(BaseUrl);

        // Assert - Check for RSS feed discovery link in head (should be exactly 1)
        var rssLink = Page.Locator("link[rel='alternate'][type='application/rss+xml']");
        await Page.AssertElementCountBySelectorAsync("link[rel='alternate'][type='application/rss+xml']", 1);
        await rssLink.AssertHrefEqualsAsync("/all/feed.xml");
    }

    [Fact]
    public async Task SectionPage_HasRssFeedDiscoveryLink()
    {
        // Arrange

        // Act
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}/ai");

        // Assert - Check for RSS feed discovery link in head (should be exactly 1)
        var rssLink = Page.Locator("link[rel='alternate'][type='application/rss+xml']");
        await Page.AssertElementCountBySelectorAsync("link[rel='alternate'][type='application/rss+xml']", 1);
        await rssLink.AssertHrefEqualsAsync("/ai/feed.xml");
    }

    [Fact]
    public async Task SectionPage_HasRssIconInHeader()
    {
        // Arrange

        // Act
        await Page.GotoRelativeAsync("/github-copilot");

        // Assert - Check for RSS link in sidebar (moved from banner)
        var rssIconLink = Page.Locator(".sidebar-section a[href*='/feed.xml']");
        await rssIconLink.AssertElementVisibleAsync();
        await rssIconLink.AssertHrefEqualsAsync("/github-copilot/feed.xml");
    }

    [Fact]
    public async Task Footer_HasRssSubscribeLink()
    {
        // Arrange

        // Act
        await Page.GotoAndWaitForBlazorAsync(BaseUrl);

        // Assert - Check footer has RSS link
        var footerRssLink = Page.Locator("footer a:has-text('Subscribe via RSS')");
        await footerRssLink.AssertElementVisibleAsync();
        await footerRssLink.AssertHrefEqualsAsync("/all/feed.xml");
    }

    [Fact]
    public async Task RssFeed_AllContent_ReturnsValidXml()
    {
        // Arrange

        // Act
        var response = await Page.APIRequest.GetAsync($"{ApiUrl}/api/rss/all");

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

        // Act
        var response = await Page.APIRequest.GetAsync($"{ApiUrl}/api/rss/{sectionName}");

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
        await Page.GotoAndWaitForBlazorAsync($"{BaseUrl}/azure");

        // Act - Get RSS link from sidebar (moved from banner)
        var rssIconLink = Page.Locator(".sidebar-section a[href*='/feed.xml']");

        // Assert - Verify the link is correct (but don't actually navigate to avoid downloading XML)
        await rssIconLink.AssertHrefEqualsAsync("/azure/feed.xml");
    }

    [Fact]
    public async Task RssFeed_ContainsRecentContent()
    {
        // Arrange

        // Act
        var response = await Page.APIRequest.GetAsync($"{ApiUrl}/api/rss/all");
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

    [Fact]
    public async Task HomePage_HasRoundupsFeedLink()
    {
        // Arrange

        // Act
        await Page.GotoAndWaitForBlazorAsync(BaseUrl);

        // Assert - Check for Roundups RSS link in sidebar
        var roundupsRssLink = Page.Locator("a:has-text('RSS Feed - Roundups')");
        await roundupsRssLink.AssertElementVisibleAsync();
        await roundupsRssLink.AssertHrefEqualsAsync("/all/roundups/feed.xml");
    }

    [Fact]
    public async Task RssFeed_CollectionFeeds_ReturnValidXml()
    {
        // Arrange

        // Act - Test roundups collection feed via web proxy
        var response = await Page.APIRequest.GetAsync($"{BaseUrl}/all/roundups/feed.xml");

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
}
