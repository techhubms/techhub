using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;
using CollectionModel = TechHub.Core.Models.Collection;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SectionCollection.razor component with skeleton layout architecture.
/// SectionCollection handles both /{sectionName} and /{sectionName}/{collectionName} routes,
/// defaulting to "all" when no collection is specified.
/// </summary>
public class SectionTests : BunitContext
{
    public SectionTests()
    {
        // Section renders child components (DateRangeSlider, ContentItemsGrid) that use JS interop
        JSInterop.Mode = JSRuntimeMode.Loose;
    }
    [Fact]
    public void Section_RendersWithPageStructure()
    {
        // Arrange
        var mockApiClient = new Mock<TechHubApiClient>(
            MockBehavior.Loose,
            new HttpClient { BaseAddress = new Uri("https://localhost") },
            Mock.Of<ILogger<TechHubApiClient>>()
        );

        var sectionCache = new SectionCache();
        sectionCache.Initialize(
        [
            new TechHub.Core.Models.Section(
                "ai",
                "Artificial Intelligence",
                "AI and machine learning content",
                "/ai",
                "AI",
                [
                    new CollectionModel("news", "News", "/ai/news", "Latest AI news", "News", false)
                ]
            )
        ]);

        mockApiClient
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

        // Mock ITechHubApiClient for SidebarTagCloud component
        var mockApiInterface = new Mock<ITechHubApiClient>();
        mockApiInterface
            .Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        Services.AddSingleton(mockApiClient.Object);
        Services.AddSingleton(mockApiInterface.Object);
        Services.AddSingleton(Mock.Of<Microsoft.JSInterop.IJSRuntime>());
        Services.AddSingleton(sectionCache);
        AddBunitPersistentComponentState();
        SetRendererInfo(new RendererInfo("Server", true));

        // Act - Render SectionCollection component (replaces Section.razor)
        var cut = Render<SectionCollection>(parameters => parameters
            .Add(p => p.SectionName, "ai"));

        // Assert - Use WaitForAssertion to wait for async rendering
        cut.WaitForAssertion(() =>
        {
            // Verify page structure is rendered with section data
            var pageStructure = cut.Find(".page-with-sidebar");
            pageStructure.Should().NotBeNull();

            var sidebar = cut.Find(".sidebar");
            sidebar.Should().NotBeNull();

            // Main content is now a section element (styled via .page-with-sidebar > :is(article, section))
            var mainContent = cut.Find(".page-with-sidebar > section");
            mainContent.Should().NotBeNull();

            // Verify section header is displayed
            var markup = cut.Markup;
            markup.Should().Contain("Artificial Intelligence");
        });
    }

    [Fact]
    public void Section_DisplaysContent_WhenLoaded()
    {
        // Arrange
        var mockApiClient = new Mock<TechHubApiClient>(
            MockBehavior.Loose,
            new HttpClient { BaseAddress = new Uri("https://localhost") },
            Mock.Of<ILogger<TechHubApiClient>>()
        );

        var sectionCache = new SectionCache();
        sectionCache.Initialize(
        [
            new TechHub.Core.Models.Section(
                "ai",
                "Artificial Intelligence",
                "AI and machine learning content",
                "/ai",
                "AI",
                [
                    new CollectionModel("news", "News", "/ai/news", "Latest AI news", "News", false)
                ]
            )
        ]);

        mockApiClient
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

        // Mock ITechHubApiClient for SidebarTagCloud component
        var mockApiInterface = new Mock<ITechHubApiClient>();
        mockApiInterface
            .Setup(x => x.GetTagCloudAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<List<string>?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        Services.AddSingleton(mockApiClient.Object);
        Services.AddSingleton(mockApiInterface.Object);
        Services.AddSingleton(Mock.Of<Microsoft.JSInterop.IJSRuntime>());
        Services.AddSingleton(sectionCache);
        AddBunitPersistentComponentState();
        SetRendererInfo(new RendererInfo("Server", true));

        // Act
        var cut = Render<SectionCollection>(parameters => parameters
            .Add(p => p.SectionName, "ai"));

        // Assert - Use WaitForAssertion to wait for async rendering
        cut.WaitForAssertion(() =>
        {
            // Verify section header is displayed
            var markup = cut.Markup;
            markup.Should().Contain("Artificial Intelligence");
        });
    }
}
