using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Web.Components;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminReviews.razor — the page that brings the review queues and the content fixes together.
/// The behaviour of both parts is tested with <see cref="ReviewOverviewTests"/> and <see cref="ContentFixReviewsTests"/>.
/// </summary>
public class AdminReviewsTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminReviewsTests()
    {
        ComponentFactories.AddStub<ContentItemEditorModal>();
        Services.AddSingleton(_api.Object);

        _api.Setup(x => x.GetProductsAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);
        _api.Setup(x => x.GetUnmatchedProductNamesAsync(It.IsAny<CancellationToken>())).ReturnsAsync([new UnmatchedProductName { Name = "Flint", ItemCount = 1 }]);
        _api.Setup(x => x.GetIgnoredProductNamesAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        _api.Setup(x => x.GetUnmatchedFeaturesSummaryAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        _api.Setup(x => x.GetContentReviewSummaryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new ContentReviewSummary { Pending = 1 });
        _api.Setup(x => x.GetProcessedUrlsAsync(1, 1, "failed", null, null, null, null, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<ProcessedUrlListItem> { Items = [], TotalCount = 0 });
        _api.Setup(x => x.GetContentReviewsAsync(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);
        _api.Setup(x => x.GetFeedConfigsAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);
    }

    [Fact]
    public void Render_ShowsTheOverviewAndTheContentFixesUnderOneHeading()
    {
        // Act
        var cut = Render<AdminReviews>();

        // Assert
        cut.Find("h1").TextContent.Should().Be("Reviews");
        cut.FindAll(".review-card").Should().HaveCount(4);
        cut.Find("#review-product-names-table").TextContent.Should().Contain("Flint");
        cut.Find("#review-content-fixes h2").TextContent.Should().Be("Content fixes");
        cut.Find("#run-cleanup").Should().NotBeNull();
    }

    [Fact]
    public void Refresh_ReloadsTheOverviewAndTheContentFixes()
    {
        // Arrange
        var cut = Render<AdminReviews>();

        // Act
        cut.Find(".admin-page-header .admin-actions button").Click();

        // Assert
        _api.Verify(x => x.GetUnmatchedProductNamesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        _api.Verify(x => x.GetContentReviewsAsync(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }
}
