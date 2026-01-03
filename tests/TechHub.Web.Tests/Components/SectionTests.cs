using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.DTOs;
using TechHub.Web.Components;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;
using Xunit;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for Section.razor component with skeleton layout architecture
/// </summary>
public class SectionTests : TestContext
{
    [Fact]
    public void Section_RendersWithSkeletonLayout()
    {
        // Arrange
        var mockApiClient = new Mock<TechHubApiClient>(
            MockBehavior.Loose,
            new HttpClient { BaseAddress = new Uri("http://localhost") },
            Mock.Of<ILogger<TechHubApiClient>>()
        );
        
        // Setup delayed response to keep components in loading state
        var tcs = new TaskCompletionSource<SectionDto?>();
        mockApiClient
            .Setup(x => x.GetSectionAsync(It.IsAny<string>()))
            .Returns(tcs.Task);

        Services.AddSingleton(mockApiClient.Object);
        Services.AddSingleton(Mock.Of<Microsoft.JSInterop.IJSRuntime>());

        // Act - Render Section component
        var cut = RenderComponent<Section>(parameters => parameters
            .Add(p => p.SectionName, "ai"));

        // Assert - Verify skeleton layout structure is present
        var grid = cut.Find(".section-page-grid");
        Assert.NotNull(grid);
        
        // Verify all three skeleton components are present
        var skeletons = cut.FindAll(".skeleton");
        Assert.True(skeletons.Count > 0, "Expected skeleton placeholders to be visible during loading");
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
        
        var sectionDto = new SectionDto
        {
            Id = "ai",
            Title = "Artificial Intelligence",
            Description = "AI and machine learning content",
            Url = "/ai",
            Category = "ai",
            BackgroundImage = "/images/ai-bg.jpg",
            Collections = new List<CollectionReferenceDto>
            {
                new CollectionReferenceDto
                {
                    Title = "News",
                    Collection = "news",
                    Url = "/ai/news",
                    Description = "Latest AI news"
                }
            }
        };

        mockApiClient
            .Setup(x => x.GetSectionAsync("ai"))
            .ReturnsAsync(sectionDto);
            
        mockApiClient
            .Setup(x => x.GetContentAsync(It.IsAny<string?>(), "all"))
            .ReturnsAsync(Array.Empty<ContentItemDto>());

        Services.AddSingleton(mockApiClient.Object);
        Services.AddSingleton(Mock.Of<Microsoft.JSInterop.IJSRuntime>());

        // Act
        var cut = RenderComponent<Section>(parameters => parameters
            .Add(p => p.SectionName, "ai"));

        // Wait for async rendering
        await Task.Delay(200);

        // Assert - Verify section header is displayed
        var markup = cut.Markup;
        Assert.Contains("Artificial Intelligence", markup);
    }
}
