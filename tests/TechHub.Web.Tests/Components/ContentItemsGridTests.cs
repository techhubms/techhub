using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.DTOs;
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
            .Setup(x => x.GetContentAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync([]);

        // Initialize SectionCache with test data
        _sectionCache = new SectionCache();
        _sectionCache.Initialize(
        [
            new SectionDto
            {
                Name = "github-copilot",
                Title = "GitHub Copilot",
                Description = "GitHub Copilot content",
                Url = "/github-copilot",
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
                Collections =
                [
                    new CollectionReferenceDto { Name = "news", Title = "News", Url = "/all/news", Description = "All news", DisplayName = "News" }
                ]
            }
        ]);

        Services.AddSingleton(_mockApiClient.Object);
        Services.AddSingleton(_sectionCache);
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
}
