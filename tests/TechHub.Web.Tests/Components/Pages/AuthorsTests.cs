using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;
using ContentItemModel = TechHub.Core.Models.ContentItem;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// Tests for Authors.razor component - sidebar with author list and main content grid.
/// </summary>
public class AuthorsTests : BunitContext
{
    private readonly Mock<TechHubApiClient> _mockApiClient;

    public AuthorsTests()
    {
        // SidebarToggle requires IHttpContextAccessor to read cookie during SSR
        var httpContextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
        Services.AddSingleton<IHttpContextAccessor>(httpContextAccessor);

        // SeoMetaTags requires BrandingService
        var config = new ConfigurationBuilder().Build();
        Services.AddScoped<BrandingService>(_ => new BrandingService(httpContextAccessor, config));

        _mockApiClient = new Mock<TechHubApiClient>(
            MockBehavior.Loose,
            new HttpClient { BaseAddress = new Uri("https://localhost") },
            Mock.Of<ILogger<TechHubApiClient>>()
        );

        // Default: return authors list
        _mockApiClient
            .Setup(x => x.GetAuthorsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AuthorSummary>
            {
                new() { Name = "Alice Smith", ItemCount = 5 },
                new() { Name = "Bob Jones", ItemCount = 3 },
                new() { Name = "Charlie Brown", ItemCount = 1 }
            });

        // Default: return empty items for any author
        _mockApiClient
            .Setup(x => x.GetAuthorItemsAsync(
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse([], 0));

        Services.AddSingleton(_mockApiClient.Object);

        // ContentItemsGrid (child component) requires SectionCache and logger
        var sectionCache = new SectionCache();
        sectionCache.Initialize([]);
        Services.AddSingleton(sectionCache);
        Services.AddSingleton<ILogger<ContentItemsGrid>>(Mock.Of<ILogger<ContentItemsGrid>>());

        this.AddBunitPersistentComponentState();
        SetRendererInfo(new RendererInfo("Server", true));
    }

    [Fact]
    public void Authors_UsesPageWithSidebarLayout()
    {
        // Act
        var cut = Render<Authors>();
        cut.WaitForState(() => cut.Find(".page-with-sidebar") != null);

        // Assert
        var main = cut.Find("main");
        main.ClassList.Should().Contain("page-with-sidebar");
    }

    [Fact]
    public void Authors_RendersSidebar_WithAuthorList()
    {
        // Act
        var cut = Render<Authors>();
        cut.WaitForState(() => cut.FindAll(".author-sidebar-link").Count > 0);

        // Assert
        var sidebar = cut.Find("aside.sidebar");
        sidebar.Should().NotBeNull();

        var authorLinks = cut.FindAll(".author-sidebar-link");
        authorLinks.Should().HaveCount(3);
    }

    [Fact]
    public void Authors_SidebarShowsAuthorNamesWithCounts()
    {
        // Act
        var cut = Render<Authors>();
        cut.WaitForState(() => cut.FindAll(".author-sidebar-link").Count > 0);

        // Assert
        var authorLinks = cut.FindAll(".author-sidebar-link");
        authorLinks[0].TextContent.Should().Contain("Alice Smith");
        authorLinks[0].TextContent.Should().Contain("(5)");
        authorLinks[1].TextContent.Should().Contain("Bob Jones");
        authorLinks[1].TextContent.Should().Contain("(3)");
        authorLinks[2].TextContent.Should().Contain("Charlie Brown");
        authorLinks[2].TextContent.Should().Contain("(1)");
    }

    [Fact]
    public void Authors_NoAuthorSelected_ShowsSelectPrompt()
    {
        // Act
        var cut = Render<Authors>();
        cut.WaitForState(() => cut.FindAll(".author-sidebar-link").Count > 0);

        // Assert
        var prompt = cut.Find(".authors-select-prompt");
        prompt.Should().NotBeNull();
        prompt.TextContent.Should().Contain("Select an author");
    }

    [Fact]
    public void Authors_WithAuthorSelected_ShowsContentGrid()
    {
        // Arrange
        IReadOnlyList<ContentItemModel> items =
        [
            A.ContentItem
                .WithTitle("Test Post 1")
                .WithAuthor("Alice Smith")
                .Build(),
            A.ContentItem
                .WithTitle("Test Post 2")
                .WithAuthor("Alice Smith")
                .Build()
        ];

        _mockApiClient
            .Setup(x => x.GetAuthorItemsAsync(
                "Alice Smith",
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse(items, 2));

        // Act
        var cut = Render<Authors>(parameters => parameters
            .Add(p => p.AuthorName, "Alice Smith"));
        cut.WaitForState(() => cut.FindAll(".content-grid").Count > 0);

        // Assert
        var grid = cut.Find(".content-grid");
        grid.Should().NotBeNull();
    }

    [Fact]
    public void Authors_WithAuthorSelected_HighlightsActiveAuthor()
    {
        // Arrange
        _mockApiClient
            .Setup(x => x.GetAuthorItemsAsync(
                "Alice Smith",
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse([], 0));

        // Act
        var cut = Render<Authors>(parameters => parameters
            .Add(p => p.AuthorName, "Alice Smith"));
        cut.WaitForState(() => cut.FindAll(".author-sidebar-link").Count > 0);

        // Assert
        var activeLink = cut.Find(".author-sidebar-link.active");
        activeLink.TextContent.Should().Contain("Alice Smith");
    }

    [Fact]
    public void Authors_WithAuthorSelected_ShowsAuthorNameInHeading()
    {
        // Arrange
        IReadOnlyList<ContentItemModel> items =
        [
            A.ContentItem
                .WithTitle("Test Post 1")
                .WithAuthor("Alice Smith")
                .Build()
        ];

        _mockApiClient
            .Setup(x => x.GetAuthorItemsAsync(
                "Alice Smith",
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse(items, 1));

        // Act
        var cut = Render<Authors>(parameters => parameters
            .Add(p => p.AuthorName, "Alice Smith"));
        cut.WaitForState(() => cut.FindAll(".content-grid").Count > 0);

        // Assert
        var h1 = cut.Find("h1");
        h1.TextContent.Should().Contain("Content by Alice Smith");
    }

    [Fact]
    public void Authors_EmptyAuthorList_ShowsNoAuthorsMessage()
    {
        // Arrange
        _mockApiClient
            .Setup(x => x.GetAuthorsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AuthorSummary>());

        // Act
        var cut = Render<Authors>();
        cut.WaitForState(() => !cut.Markup.Contains("aria-busy"));

        // Assert
        var noContent = cut.Find(".no-content");
        noContent.Should().NotBeNull();
    }

    [Fact]
    public void Authors_SidebarLinksPointToAuthorRoutes()
    {
        // Act
        var cut = Render<Authors>();
        cut.WaitForState(() => cut.FindAll(".author-sidebar-link").Count > 0);

        // Assert
        var links = cut.FindAll(".author-sidebar-link");
        links[0].GetAttribute("href").Should().Be("/all/authors/Alice%20Smith");
        links[1].GetAttribute("href").Should().Be("/all/authors/Bob%20Jones");
        links[2].GetAttribute("href").Should().Be("/all/authors/Charlie%20Brown");
    }
}
