using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.DTOs;
using TechHub.Web.Components;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for ContentItemsGrid.razor component page titles
/// </summary>
public class ContentItemsGridTests : TestContext
{
    private readonly Mock<TechHubApiClient> _mockApiClient;
    private readonly SectionCache _sectionCache;

    public ContentItemsGridTests()
    {
        _mockApiClient = new Mock<TechHubApiClient>(
            MockBehavior.Loose,
            new HttpClient { BaseAddress = new Uri("http://localhost") },
            Mock.Of<ILogger<TechHubApiClient>>()
        );

        // Default setup - return empty list
        _mockApiClient
            .Setup(x => x.GetContentAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync([]);

        // Configure WebAppSettings
        var appSettings = new WebAppSettings
        {
            Seo = new SeoSettings
            {
                BaseUrl = "https://tech.hub.ms",
                SiteTitle = "Microsoft Tech Hub",
                SiteDescription = "Test description"
            }
        };

        // Initialize SectionCache with test data
        _sectionCache = new SectionCache();
        _sectionCache.Initialize(new List<SectionDto>
        {
            new SectionDto
            {
                Name = "github-copilot",
                Title = "GitHub Copilot",
                Description = "GitHub Copilot content",
                Url = "/github-copilot",
                BackgroundImage = "/images/github-copilot-bg.jpg",
                Collections =
                [
                    new CollectionReferenceDto { Name = "news", Title = "News", Url = "/github-copilot/news", Description = "News", DisplayName = "News" },
                    new CollectionReferenceDto { Name = "community", Title = "Community", Url = "/github-copilot/community", Description = "Community", DisplayName = "Community Posts" },
                    new CollectionReferenceDto { Name = "videos", Title = "Videos", Url = "/github-copilot/videos", Description = "Videos", DisplayName = "Videos" }
                ]
            },
            new SectionDto
            {
                Name = "all",
                Title = "All",
                Description = "All content",
                Url = "/all",
                BackgroundImage = "/images/all-bg.jpg",
                Collections =
                [
                    new CollectionReferenceDto { Name = "news", Title = "News", Url = "/all/news", Description = "All news", DisplayName = "News" }
                ]
            }
        });

        Services.AddSingleton(Options.Create(appSettings));
        Services.AddSingleton(_mockApiClient.Object);
        Services.AddSingleton(_sectionCache);
    }

    [Fact]
    public async Task ContentItemsGrid_GitHubCopilot_AllCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "all"));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse All GitHub Copilot Content");
    }

    [Fact]
    public async Task ContentItemsGrid_GitHubCopilot_NewsCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "news"));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse GitHub Copilot News");
    }

    [Fact]
    public async Task ContentItemsGrid_GitHubCopilot_CommunityCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "community"));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse GitHub Copilot Community Posts");
    }

    [Fact]
    public async Task ContentItemsGrid_GitHubCopilot_VideosCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "videos"));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse GitHub Copilot Videos");
    }

    [Fact]
    public async Task ContentItemsGrid_AllSection_AllCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse All Posts");
    }

    [Fact]
    public async Task ContentItemsGrid_AllSection_NewsCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "news"));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse All News");
    }
}
