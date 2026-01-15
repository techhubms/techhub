using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.DTOs;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for Section.razor component with skeleton layout architecture
/// </summary>
public class SectionTests : TestContext
{
    [Fact]
    public async Task Section_RendersWithPageStructure()
    {
        // Arrange
        var mockApiClient = new Mock<TechHubApiClient>(
            MockBehavior.Loose,
            new HttpClient { BaseAddress = new Uri("http://localhost") },
            Mock.Of<ILogger<TechHubApiClient>>()
        );

        var sectionCache = new SectionCache();
        sectionCache.Initialize(
        [
            new SectionDto
            {
                Name = "ai",
                Title = "Artificial Intelligence",
                Description = "AI and machine learning content",
                Url = "/ai",
                BackgroundImage = "/images/ai-bg.jpg",
                Collections =
                [
                    new CollectionReferenceDto
                    {
                        Title = "News",
                        Name = "news",
                        Url = "/ai/news",
                        Description = "Latest AI news",
                        DisplayName = "News"
                    }
                ]
            }
        ]);

        mockApiClient
            .Setup(x => x.GetContentAsync(It.IsAny<string?>(), "all"))
            .ReturnsAsync([]);

        Services.AddSingleton(mockApiClient.Object);
        Services.AddSingleton(Mock.Of<Microsoft.JSInterop.IJSRuntime>());
        Services.AddSingleton(sectionCache);

        // Act - Render Section component
        var cut = RenderComponent<Section>(parameters => parameters
            .Add(p => p.SectionName, "ai"));

        // Wait for async rendering
        await Task.Delay(200);

        // Assert - Verify page structure is rendered with section data
        var pageStructure = cut.Find(".page-with-sidebar");
        pageStructure.Should().NotBeNull();

        var sidebar = cut.Find(".sidebar");
        sidebar.Should().NotBeNull();

        var mainContent = cut.Find(".page-main-content");
        mainContent.Should().NotBeNull();

        // Verify section header is displayed
        var markup = cut.Markup;
        markup.Should().Contain("Artificial Intelligence");
    }

    [Fact]
    public async Task Section_DisplaysContent_WhenLoaded()
    {
        // Arrange
        var mockApiClient = new Mock<TechHubApiClient>(
            MockBehavior.Loose,
            new HttpClient { BaseAddress = new Uri("http://localhost") },
            Mock.Of<ILogger<TechHubApiClient>>()
        );

        var sectionCache = new SectionCache();
        sectionCache.Initialize(
        [
            new SectionDto
            {
                Name = "ai",
                Title = "Artificial Intelligence",
                Description = "AI and machine learning content",
                Url = "/ai",
                BackgroundImage = "/images/ai-bg.jpg",
                Collections =
                [
                    new CollectionReferenceDto
                    {
                        Title = "News",
                        Name = "news",
                        Url = "/ai/news",
                        Description = "Latest AI news",
                        DisplayName = "News"
                    }
                ]
            }
        ]);

        mockApiClient
            .Setup(x => x.GetContentAsync(It.IsAny<string?>(), "all"))
            .ReturnsAsync([]);

        Services.AddSingleton(mockApiClient.Object);
        Services.AddSingleton(Mock.Of<Microsoft.JSInterop.IJSRuntime>());
        Services.AddSingleton(sectionCache);

        // Act
        var cut = RenderComponent<Section>(parameters => parameters
            .Add(p => p.SectionName, "ai"));

        // Wait for async rendering
        await Task.Delay(200);

        // Assert - Verify section header is displayed
        var markup = cut.Markup;
        markup.Should().Contain("Artificial Intelligence");
    }
}
