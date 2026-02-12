using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.Models;
using TechHub.Web.Components;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for ContentItemsGrid.razor component page titles
/// </summary>
public class ContentItemsGridTests : BunitContext
{
    private readonly Mock<TechHubApiClient> _mockApiClient;
    private readonly SectionCache _sectionCache;

    public ContentItemsGridTests()
    {
        _mockApiClient = new Mock<TechHubApiClient>(
            MockBehavior.Loose,
            new HttpClient { BaseAddress = new Uri("https://localhost") },
            Mock.Of<ILogger<TechHubApiClient>>()
        );

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
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

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
        AddBunitPersistentComponentState();

        // ContentItemsGrid uses RendererInfo.IsInteractive in OnAfterRenderAsync
        SetRendererInfo(new RendererInfo("Server", true));
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
            h1.TextContent.Should().Be("Browse All GitHub Copilot Content");
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
            h1.TextContent.Should().Be("Browse GitHub Copilot News");
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
            h1.TextContent.Should().Be("Browse GitHub Copilot Community Posts");
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
            h1.TextContent.Should().Be("Browse GitHub Copilot Videos");
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
            h1.TextContent.Should().Be("Browse All Posts");
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
            h1.TextContent.Should().Be("Browse All News");
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
}
