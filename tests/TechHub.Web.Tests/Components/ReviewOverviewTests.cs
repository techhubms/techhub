using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Web.Components;
using TechHub.Web.Services;
using TechHub.Web.Tests.Components.Pages.Admin;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// bUnit tests for ReviewOverview.razor — the counts of everything that needs a review, the product names to review
/// (with their actions), and the features to review.
/// </summary>
public class ReviewOverviewTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public ReviewOverviewTests()
    {
        Services.AddSingleton(_api.Object);
        Setup();
    }

    // ── Overview cards ──────────────────────────────────────────────────────

    [Fact]
    public void Render_ShowsACardPerQueueWithItsCountAndLink()
    {
        // Arrange
        Setup(
            unmatched: [Unmatched("Flint"), Unmatched("Osmos")],
            features: [Features(1, "GitHub Copilot", names: 3, events: 5), Features(2, "Azure Functions", names: 1, events: 1)],
            pendingFixes: 7,
            failedUrls: 5);

        // Act
        var cut = Render<ReviewOverview>();

        // Assert
        var cards = cut.FindAll(".review-card");
        cards.Select(c => c.GetAttribute("href")).Should().Equal("/admin/reviews#review-product-names", "/admin/reviews#review-features", "/admin/reviews#review-content-fixes", "/admin/processed-urls?status=failed");
        cards.Select(c => c.QuerySelector(".review-card-count")!.TextContent).Should().Equal("2", "4", "7", "5");
        cards.Select(c => c.QuerySelector(".review-card-label")!.TextContent).Should().Equal("Product names", "Feature names", "Content fixes", "Failed URLs");
        cards.Should().OnlyContain(c => !c.ClassList.Contains("review-card-clear"));
    }

    [Fact]
    public void Render_NothingToReview_MarksTheCardsClearAndShowsEmptyMessages()
    {
        // Act
        var cut = Render<ReviewOverview>();

        // Assert
        cut.FindAll(".review-card").Should().HaveCount(4).And.OnlyContain(c => c.ClassList.Contains("review-card-clear"));
        cut.Find("#review-product-names-empty").TextContent.Should().Contain("Nothing to review.");
        cut.Find("#review-features-empty").TextContent.Should().Contain("Nothing to review.");
    }

    [Fact]
    public void Render_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        _api.Setup(x => x.GetUnmatchedProductNamesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("down"));

        // Act
        var cut = Render<ReviewOverview>();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load the review queues: down");
        cut.FindAll(".review-card").Should().BeEmpty();
    }

    [Fact]
    public async Task Refresh_ReloadsTheQueues()
    {
        // Arrange
        var cut = Render<ReviewOverview>();
        Setup(unmatched: [Unmatched("Flint")]);

        // Act
        await cut.InvokeAsync(() => cut.Instance.RefreshAsync());

        // Assert
        cut.Find("#review-product-names-table").TextContent.Should().Contain("Flint");
    }

    // ── Product names ───────────────────────────────────────────────────────

    [Fact]
    public void Render_ShowsTheUnmatchedProductNamesWithTheirCounts()
    {
        // Arrange
        Setup(unmatched: [new UnmatchedProductName { Name = "Flint-interactive", ItemCount = 3, EventCount = 1 }]);

        // Act
        var cut = Render<ReviewOverview>();

        // Assert
        var row = cut.FindAll("#review-product-names-table tbody tr").Should().ContainSingle().Subject;
        row.TextContent.Should().Contain("Flint-interactive").And.Contain("3").And.Contain("1");
        cut.Find("#review-product-names h2").TextContent.Should().Be("Product names to review (1)");
    }

    [Fact]
    public void AddAsProduct_CreatesTheProductAndReloads()
    {
        // Arrange
        Setup(unmatched: [Unmatched("Flint")]);
        _api.Setup(x => x.CreateProductAsync("Flint", It.IsAny<CancellationToken>())).ReturnsAsync(new ProductDetail { Id = 9, Name = "Flint" });
        var cut = Render<ReviewOverview>();

        // Act
        ClickInRow(cut, "Add as product");

        // Assert
        _api.Verify(x => x.CreateProductAsync("Flint", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Product 'Flint' added.");
        _api.Verify(x => x.GetProductsAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public void AddAsAlias_AddsTheNameToTheChosenProduct()
    {
        // Arrange
        Setup(products: [Product(7, "Microsoft Foundry")], unmatched: [Unmatched("Azure AI Foundry")]);
        _api.Setup(x => x.AddProductAliasAsync(7, "Azure AI Foundry", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductDetail { Id = 7, Name = "Microsoft Foundry" });
        var cut = Render<ReviewOverview>();
        cut.Find("#review-product-names-table input").Change("microsoft foundry");

        // Act
        ClickInRow(cut, "Add as alias");

        // Assert
        _api.Verify(x => x.AddProductAliasAsync(7, "Azure AI Foundry", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("'Azure AI Foundry' is now an alias of 'Microsoft Foundry'.");
    }

    [Fact]
    public void AddAsAlias_WithoutChosenProduct_DisablesTheButton()
    {
        // Arrange
        Setup(products: [Product(7, "Microsoft Foundry")], unmatched: [Unmatched("Azure AI Foundry")]);

        // Act
        var cut = Render<ReviewOverview>();

        // Assert
        cut.FindAll("#review-product-names-table button").Single(b => b.TextContent == "Add as alias").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void AddAsAlias_ToUnknownProduct_ShowsErrorWithoutCallingTheApi()
    {
        // Arrange
        Setup(products: [Product(7, "Microsoft Foundry")], unmatched: [Unmatched("Azure AI Foundry")]);
        var cut = Render<ReviewOverview>();
        cut.Find("#review-product-names-table input").Change("Nonexistent");

        // Act
        ClickInRow(cut, "Add as alias");

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("'Nonexistent' is not a product.");
        _api.Verify(x => x.AddProductAliasAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void NotAProduct_IgnoresTheName()
    {
        // Arrange
        Setup(unmatched: [Unmatched("Microsoft Ignite")]);
        var cut = Render<ReviewOverview>();

        // Act
        ClickInRow(cut, "Not a product");

        // Assert
        _api.Verify(x => x.IgnoreProductNameAsync("Microsoft Ignite", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("'Microsoft Ignite' is marked as not a product.");
    }

    [Fact]
    public void ChangeRefused_ShowsTheServerMessage()
    {
        // Arrange
        Setup(unmatched: [Unmatched("Flint")]);
        _api.Setup(x => x.CreateProductAsync("Flint", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("'Flint' is already used by 'Flint Pro'."));
        var cut = Render<ReviewOverview>();

        // Act
        ClickInRow(cut, "Add as product");

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("'Flint' is already used by 'Flint Pro'.");
    }

    [Fact]
    public void Restore_PutsAnIgnoredNameBack()
    {
        // Arrange
        Setup(ignored: ["Microsoft Azure"]);
        var cut = Render<ReviewOverview>();

        // Act
        cut.Find(".admin-entry-list .admin-link-btn").Click();

        // Assert
        _api.Verify(x => x.UnignoreProductNameAsync("Microsoft Azure", It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── Features ───────────────────────────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void Render_ListsTheProductsWithFeaturesToReviewWithALinkToTheirFeaturesTab()
    {
        // Arrange
        Setup(features: [Features(88, "GitHub Copilot", names: 20, events: 21, content: 3)]);

        // Act
        var cut = Render<ReviewOverview>();

        // Assert
        var row = cut.FindAll("#review-features-table tbody tr").Should().ContainSingle().Subject;
        row.QuerySelectorAll("td").Select(c => c.TextContent).Take(4).Should().Equal("GitHub Copilot", "20", "3", "21");
        row.QuerySelector("a")!.GetAttribute("href").Should().Be("/admin/products/88/features");
        row.QuerySelector("a")!.TextContent.Should().Be("Review 20 names");
        cut.Find("#review-features h2").TextContent.Should().Be("Feature names to review (20)");
        cut.Find("#review-features p").TextContent.Should().Contain("grouped by product");
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static void ClickInRow(IRenderedComponent<ReviewOverview> cut, string buttonText) =>
        cut.FindAll("#review-product-names-table button").Single(b => b.TextContent == buttonText).Click();

    private static UnmatchedProductName Unmatched(string name) => new() { Name = name, ItemCount = 1, EventCount = 0 };

    private static ProductUnmatchedFeatures Features(long productId, string productName, int names, int events, int content = 0) =>
        new() { ProductId = productId, ProductName = productName, NameCount = names, EventCount = events, ContentCount = content };

    private static ProductSummary Product(long id, string name) => new() { Id = id, Name = name };

    /// <summary>Configures the API mock; arguments left <c>null</c> mean an empty queue.</summary>
    private void Setup(
        IReadOnlyList<ProductSummary>? products = null,
        IReadOnlyList<UnmatchedProductName>? unmatched = null,
        IReadOnlyList<string>? ignored = null,
        IReadOnlyList<ProductUnmatchedFeatures>? features = null,
        int pendingFixes = 0,
        int failedUrls = 0)
    {
        _api.Setup(x => x.GetProductsAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>())).ReturnsAsync(products ?? []);
        _api.Setup(x => x.GetUnmatchedProductNamesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(unmatched ?? []);
        _api.Setup(x => x.GetIgnoredProductNamesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(ignored ?? []);
        _api.Setup(x => x.GetUnmatchedFeaturesSummaryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(features ?? []);
        _api.Setup(x => x.GetContentReviewSummaryAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TechHub.Core.Models.Admin.ContentReviewSummary { Pending = pendingFixes });
        _api.Setup(x => x.GetProcessedUrlsAsync(1, 1, "failed", null, null, null, null, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<ProcessedUrlListItem> { Items = [], TotalCount = failedUrls });
    }
}
