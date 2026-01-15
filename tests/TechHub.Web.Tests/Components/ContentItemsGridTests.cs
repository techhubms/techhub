using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.DTOs;
using TechHub.Web.Components;
using TechHub.Web.Services;
using FluentAssertions;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for ContentItemsGrid.razor component page titles
/// </summary>
public class ContentItemsGridTests : TestContext
{
    private readonly Mock<TechHubApiClient> _mockApiClient;

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

        // Configure WebAppSettings - no longer needs CollectionDisplayNames (comes from API)
        var appSettings = new WebAppSettings
        {
            Seo = new SeoSettings
            {
                BaseUrl = "https://tech.hub.ms",
                SiteTitle = "Microsoft Tech Hub",
                SiteDescription = "Test description"
            }
        };

        Services.AddSingleton(Options.Create(appSettings));
        Services.AddSingleton(_mockApiClient.Object);
    }

    [Fact]
    public async Task ContentItemsGrid_GitHubCopilot_AllCollection_DisplaysCorrectTitle()
    {
        // Arrange & Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "all")
            .Add(p => p.SectionTitle, "GitHub Copilot")
            .Add(p => p.Collections, []));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse All GitHub Copilot Content");
    }

    [Fact]
    public async Task ContentItemsGrid_GitHubCopilot_NewsCollection_DisplaysCorrectTitle()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto
            {
                Name = "news",
                Title = "News",
                Url = "/github-copilot/news",
                Description = "News items",
                DisplayName = "News"
            }
        };

        // Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "news")
            .Add(p => p.SectionTitle, "GitHub Copilot")
            .Add(p => p.Collections, collections));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse GitHub Copilot News");
    }

    [Fact]
    public async Task ContentItemsGrid_GitHubCopilot_CommunityCollection_DisplaysCorrectTitle()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto
            {
                Name = "community",
                Title = "Community",
                Url = "/github-copilot/community",
                Description = "Community items",
                DisplayName = "Community Posts"
            }
        };

        // Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "community")
            .Add(p => p.SectionTitle, "GitHub Copilot")
            .Add(p => p.Collections, collections));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse GitHub Copilot Community Posts");
    }

    [Fact]
    public async Task ContentItemsGrid_GitHubCopilot_VideosCollection_DisplaysCorrectTitle()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto
            {
                Name = "videos",
                Title = "Videos",
                Url = "/github-copilot/videos",
                Description = "Videos",
                DisplayName = "Videos"
            }
        };

        // Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "github-copilot")
            .Add(p => p.CollectionName, "videos")
            .Add(p => p.SectionTitle, "GitHub Copilot")
            .Add(p => p.Collections, collections));

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
            .Add(p => p.CollectionName, "all")
            .Add(p => p.SectionTitle, "All")
            .Add(p => p.Collections, []));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse All Posts");
    }

    [Fact]
    public async Task ContentItemsGrid_AllSection_NewsCollection_DisplaysCorrectTitle()
    {
        // Arrange
        var collections = new List<CollectionReferenceDto>
        {
            new CollectionReferenceDto
            {
                Name = "news",
                Title = "News",
                Url = "/all/news",
                Description = "All news",
                DisplayName = "News"
            }
        };

        // Act
        var cut = RenderComponent<ContentItemsGrid>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "news")
            .Add(p => p.SectionTitle, "All")
            .Add(p => p.Collections, collections));

        // Wait for async rendering
        await Task.Delay(100);

        // Assert
        var h1 = cut.Find("h1.page-h1");
        h1.TextContent.Should().Be("Browse All News");
    }
}
