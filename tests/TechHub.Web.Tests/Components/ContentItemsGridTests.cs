using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for ContentItemsGrid.razor component page titles
/// </summary>
public class ContentItemsGridTests : BunitContext
{
    private readonly Mock<ITechHubApiClient> _mockApiClient;
    private readonly SectionCache _sectionCache;

    public ContentItemsGridTests()
    {
        _mockApiClient = new Mock<ITechHubApiClient>();

        // Default setup - return empty list
        _mockApiClient
            .Setup(x => x.GetCollectionItemsAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<int?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<bool>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TechHub.Core.Models.CollectionItemsResponse([], 0));

        // Initialize SectionCache with test data
        _sectionCache = new SectionCache();
        _sectionCache.Initialize(
        [
            new Section(
                "github-copilot",
                "GitHub Copilot",
                "GitHub Copilot content",
                "/github-copilot",
                "GitHub Copilot",
                [
                    new Collection("news", "News", "/github-copilot/news", "News", "News", false),
                    new Collection("community", "Community", "/github-copilot/community", "Community", "Community Posts", false),
                    new Collection("videos", "Videos", "/github-copilot/videos", "Videos", "Videos", false)
                ]
            ),
            new Section(
                "all",
                "All",
                "All content",
                "/all",
                "All",
                [
                    new Collection("news", "News", "/all/news", "All news", "News", false)
                ]
            )
        ]);

        Services.AddSingleton(_mockApiClient.Object);
        Services.AddSingleton(_sectionCache);
        Services.AddScoped<ContentGridStateCache>();
        AddBunitPersistentComponentState();

        // ContentItemsGrid uses RendererInfo.IsInteractive in OnAfterRenderAsync
        SetRendererInfo(new RendererInfo("Server", true));

        // JS interop is not under test — allow all calls (infinite-scroll module import, etc.)
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void ContentItemsGrid_GitHubCopilot_AllCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = Render<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "all"));

        // Assert - Use WaitForAssertion to wait for async rendering
        cut.WaitForAssertion(() =>
        {
            var h1 = cut.Find("h1.page-h1");
            h1.TextContent.Should().Be("Browse All GitHub Copilot Content (0)");
        });
    }

    [Fact]
    public void ContentItemsGrid_GitHubCopilot_NewsCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = Render<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "news"));

        // Assert - Use WaitForAssertion to wait for async rendering
        cut.WaitForAssertion(() =>
        {
            var h1 = cut.Find("h1.page-h1");
            h1.TextContent.Should().Be("Browse GitHub Copilot News (0)");
        });
    }

    [Fact]
    public void ContentItemsGrid_GitHubCopilot_CommunityCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = Render<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "community"));

        // Assert - Use WaitForAssertion to wait for async rendering
        cut.WaitForAssertion(() =>
        {
            var h1 = cut.Find("h1.page-h1");
            h1.TextContent.Should().Be("Browse GitHub Copilot Community Posts (0)");
        });
    }

    [Fact]
    public void ContentItemsGrid_GitHubCopilot_VideosCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = Render<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "videos"));

        // Assert - Use WaitForAssertion to wait for async rendering
        cut.WaitForAssertion(() =>
        {
            var h1 = cut.Find("h1.page-h1");
            h1.TextContent.Should().Be("Browse GitHub Copilot Videos (0)");
        });
    }

    [Fact]
    public void ContentItemsGrid_AllSection_AllCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = Render<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        // Assert - Use WaitForAssertion to wait for async rendering
        cut.WaitForAssertion(() =>
        {
            var h1 = cut.Find("h1.page-h1");
            h1.TextContent.Should().Be("Browse All Posts (0)");
        });
    }

    [Fact]
    public void ContentItemsGrid_AllSection_NewsCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = Render<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "news"));

        // Assert - Use WaitForAssertion to wait for async rendering
        cut.WaitForAssertion(() =>
        {
            var h1 = cut.Find("h1.page-h1");
            h1.TextContent.Should().Be("Browse All News (0)");
        });
    }

    [Fact]
    public async Task ContentItemsGrid_DisposeAsync_DoesNotThrow()
    {
        // Arrange
        var cut = Render<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "news"));

        // Wait for component to initialize
        cut.WaitForAssertion(() => cut.Find("h1.page-h1").TextContent.Should().NotBeNullOrEmpty(), timeout: TimeSpan.FromSeconds(5));

        // Act & Assert - Disposal should not throw
        var disposeAction = async () => await cut.Instance.DisposeAsync();
        await disposeAction.Should().NotThrowAsync("disposal should always be safe to call");
    }

    [Fact]
    public void ContentItemsGrid_RestoresFromCircuitCache_WhenAvailable()
    {
        // Arrange - Pre-populate the circuit-scoped cache (simulates back-navigation
        // after the user had scrolled through multiple batches)
        var cache = Services.GetRequiredService<ContentGridStateCache>();
        var cachedItems = Enumerable.Range(1, 60).Select(i =>
            new ContentItemBuilder().WithSlug($"item-{i}").WithTitle($"Item {i}")
                .WithPrimarySectionName("github-copilot").WithCollectionName("news").Build()
        ).ToList();
        cache.Set("ContentItemsGrid_github-copilot_news", cachedItems, currentBatch: 3, hasMoreContent: true, totalCount: 100);

        // Act - Render the component (should restore from cache, not call API)
        var cut = Render<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "news"));

        // Assert - All 60 cached items are rendered, and API was NOT called
        cut.WaitForAssertion(() =>
        {
            var cards = cut.FindAll(".content-grid > *");
            cards.Count.Should().Be(60, "all cached items should be restored on back-navigation");
        });
        _mockApiClient.Verify(x => x.GetCollectionItemsAsync(
            It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<int?>(), It.IsAny<int?>(),
            It.IsAny<string?>(), It.IsAny<string?>(),
            It.IsAny<string?>(), It.IsAny<int?>(),
            It.IsAny<string?>(), It.IsAny<string?>(),
            It.IsAny<bool>(), It.IsAny<string?>(),
            It.IsAny<CancellationToken>()), Times.Never,
            "grid should use circuit cache instead of calling API");
    }

    [Fact]
    public void ContentItemsGrid_ShowsEndOfContent_WhenCacheHasNoMore()
    {
        // Arrange - Cache indicates no more content available
        var cache = Services.GetRequiredService<ContentGridStateCache>();
        var cachedItems = Enumerable.Range(1, 15).Select(i =>
            new ContentItemBuilder().WithSlug($"item-{i}").WithTitle($"Item {i}")
                .WithPrimarySectionName("github-copilot").WithCollectionName("news").Build()
        ).ToList();
        cache.Set("ContentItemsGrid_github-copilot_news", cachedItems, currentBatch: 1, hasMoreContent: false, totalCount: 15);

        // Act
        var cut = Render<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "news"));

        // Assert - End of content shown, no scroll trigger
        cut.WaitForAssertion(() =>
        {
            cut.Find(".end-of-content").TextContent.Should().Contain("End of content");
            cut.FindAll("#scroll-trigger").Should().BeEmpty("no scroll trigger when all content loaded");
        });
    }
}
