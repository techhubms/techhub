using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminProductDetail.razor: aliases, rename, merge/delete confirmation, and the linked articles and dates.
/// </summary>
public class AdminProductDetailTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminProductDetailTests()
    {
        Services.AddSingleton(_api.Object);
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(Detail());
        _api.Setup(x => x.GetProductsAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new ProductSummary { Id = 5, Name = "Microsoft Foundry" }, new ProductSummary { Id = 6, Name = "Azure Functions" }]);
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(new ProductFeatureOverview());
        SetEvents(DefaultEvent());
        SetArticles(DefaultArticle());
    }

    private IRenderedComponent<AdminProductDetail> RenderDetail(string? tab = null) =>
        Render<AdminProductDetail>(p =>
        {
            p.Add(x => x.Id, 5);
            if (tab is not null)
            {
                p.Add(x => x.Tab, tab);
            }
        });

    [Fact]
    public void Render_ShowsTheNameAndAliasesOnTheGeneralTab()
    {
        // Act
        var cut = RenderDetail();

        // Assert
        cut.Find("h1").TextContent.Should().Be("Microsoft Foundry");
        cut.Find("#aliases-list").TextContent.Should().Contain("Azure AI Foundry");
    }

    [Fact]
    public void Render_ShowsTheDatesWithLinksToTheArticlesOnTheDatesTab()
    {
        // Act
        var cut = RenderDetail("dates");

        // Assert
        var eventRow = cut.FindAll("#events-table tbody tr").Should().ContainSingle().Subject;
        eventRow.TextContent.Should().Contain("2027-06-01").And.Contain("retirement").And.Contain("Foundry Classic retires");
        eventRow.QuerySelector("a")!.GetAttribute("href").Should().Be("/admin/content-preview/news/retirement-post");
        eventRow.QuerySelector("a")!.TextContent.Should().Be("Retirement post");
        cut.FindAll("#articles-table").Should().BeEmpty("the articles are on their own tab");
        cut.FindAll("#aliases-list").Should().BeEmpty("the names are on the general tab");
    }

    [Fact]
    public void Render_ShowsTheArticlesWithLinksToThemOnTheArticlesTab()
    {
        // Act
        var cut = RenderDetail("articles");

        // Assert
        var articleRow = cut.FindAll("#articles-table tbody tr").Should().ContainSingle().Subject;
        articleRow.TextContent.Should().Contain("Azure AI Foundry");
        articleRow.QuerySelector("a")!.GetAttribute("href").Should().Be("/admin/content-preview/news/intro-post");
        cut.FindAll("#events-table").Should().BeEmpty("the dates are on their own tab");
    }

    [Fact]
    public void Render_LongListsArePaged_AndNextAndPreviousLoadTheOtherPages()
    {
        // Arrange
        var detail = Detail();
        detail.ArticleCount = 60;
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(detail);
        _api.Setup(x => x.GetProductArticlesAsync(5, true, 1, 25, It.IsAny<CancellationToken>())).ReturnsAsync(Page<ProductArticleLink>([DefaultArticle()], 60));
        _api.Setup(x => x.GetProductArticlesAsync(5, true, 2, 25, It.IsAny<CancellationToken>())).ReturnsAsync(Page<ProductArticleLink>([DefaultArticle()], 60));
        var cut = RenderDetail("articles");

        // Assert — first page
        cut.Find("#product-pager .admin-pagination-info").TextContent.Should().Contain("Page 1 of 3").And.Contain("60");
        cut.FindAll("#product-pager button").First().HasAttribute("disabled").Should().BeTrue();

        // Act
        cut.FindAll("#product-pager button").Last().Click();

        // Assert
        _api.Verify(x => x.GetProductArticlesAsync(5, true, 2, 25, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find("#product-pager .admin-pagination-info").TextContent.Should().Contain("Page 2 of 3");
    }

    [Fact]
    public void Render_ShortListsHaveNoPager()
    {
        // Act
        var cut = RenderDetail("dates");

        // Assert
        cut.FindAll("#product-pager").Should().BeEmpty();
    }

    [Fact]
    public void ScopeFilter_IsOnlyOfferedForAProductWithChildren_AndExcludingThemReloadsThePage()
    {
        // Arrange
        RenderDetail("dates").FindAll("#includeChildren").Should().BeEmpty();
        _api.Invocations.Clear();

        var detail = Detail();
        detail.Children = [new ProductReference { Id = 7, Name = "Foundry Agent Service" }];
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(detail);
        var cut = RenderDetail("dates");
        _api.Verify(x => x.GetProductEventsAsync(5, true, 1, 25, It.IsAny<CancellationToken>()), Times.Once);

        // Act
        cut.Find("#includeChildren").Change(false);

        // Assert
        _api.Verify(x => x.GetProductEventsAsync(5, false, 1, 25, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Render_OnlyLoadsTheDataOfTheActiveTab()
    {
        // Act
        RenderDetail();

        // Assert
        _api.Verify(x => x.GetProductEventsAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _api.Verify(x => x.GetProductArticlesAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Render_TheOldActivityAddressIsNoLongerATab()
    {
        // Act
        var cut = RenderDetail("activity");

        // Assert
        cut.FindAll(".admin-tab.active").Select(a => a.TextContent).Should().Equal("General");
    }

    [Theory]
    [InlineData("day", "2027-06-01")]
    [InlineData("month", "2027-06")]
    [InlineData("quarter", "2027-Q2")]
    public void Render_FormatsTheDateByPrecision(string precision, string expected)
    {
        // Arrange
        var dateEvent = DefaultEvent();
        dateEvent.DatePrecision = precision;
        SetEvents(dateEvent);

        // Act
        var cut = RenderDetail("dates");

        // Assert
        cut.Find("#events-table tbody tr td").TextContent.Should().Be(expected);
    }

    [Fact]
    public void Render_ProductWithoutLinks_ShowsEmptyMessages()
    {
        // Arrange
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(new ProductDetail { Id = 5, Name = "Empty" });
        SetEvents();
        SetArticles();

        // Act
        var general = RenderDetail();
        var dates = RenderDetail("dates");
        var articles = RenderDetail("articles");

        // Assert
        general.Find("#aliases-empty").Should().NotBeNull();
        dates.Find("#events-empty").Should().NotBeNull();
        articles.Find("#articles-empty").Should().NotBeNull();
    }

    [Theory]
    [InlineData(null, "General")]
    [InlineData("features", "Features (0)")]
    [InlineData("dates", "Dates (1)")]
    [InlineData("articles", "Articles (1)")]
    [InlineData("nonsense", "General")]
    public void Render_MarksTheCurrentTabAndLinksToTheOthers(string? tab, string expectedActive)
    {
        // Act
        var cut = RenderDetail(tab);

        // Assert
        var tabs = cut.FindAll(".admin-tabs a");
        tabs.Select(a => a.GetAttribute("href")).Should().Equal(
            "/admin/products/5", "/admin/products/5/features", "/admin/products/5/dates", "/admin/products/5/articles");
        tabs.Select(a => a.TextContent).Should().Equal("General", "Features (0)", "Dates (1)", "Articles (1)");
        cut.FindAll(".admin-tab.active").Select(a => a.TextContent).Should().Equal(expectedActive);
    }

    [Fact]
    public void Render_OnlyShowsTheSectionsOfTheCurrentTab()
    {
        // Act
        var general = RenderDetail();
        var features = RenderDetail("features");
        var dates = RenderDetail("dates");
        var articles = RenderDetail("articles");

        // Assert
        general.FindAll(".admin-section-title").Select(e => e.TextContent).Should().Equal("Name and aliases", "Parent and child products", "Convert, merge or delete");
        features.FindAll(".admin-section-title").Select(e => e.TextContent).Should().Equal("Features (0)", "Features to review (0)");
        dates.FindAll(".admin-section-title").Select(e => e.TextContent).Should().Equal("Dates (1)");
        articles.FindAll(".admin-section-title").Select(e => e.TextContent).Should().Equal("Articles (1)");
    }

    [Fact]
    public void Render_ExplainsWhatMergeIntoDoes()
    {
        // Act
        var cut = RenderDetail();

        // Assert
        var description = cut.Find("#merge-description").TextContent;
        description.Should().Contain("Merge into").And.Contain("added twice").And.Contain("become aliases").And.Contain("Delete");
    }

    [Fact]
    public void Render_ExplainsHowProductsAndFeaturesFitTogether()
    {
        // Act
        var cut = RenderDetail();

        // Assert
        var glossary = cut.Find("#catalog-glossary").TextContent;
        glossary.Should().Contain("Product").And.Contain("Feature").And.Contain("Date").And.NotContain("Surface");
    }
    [Fact]
    public void Render_UnknownProduct_ShowsNotFound()
    {
        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 999));

        // Assert
        cut.Find("#product-not-found").TextContent.Should().Contain("Product not found.");
    }

    [Fact]
    public void Render_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("down"));

        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load product: down");
    }

    [Fact]
    public void Rename_CallsTheApiAndShowsTheNewName()
    {
        // Arrange
        _api.Setup(x => x.RenameProductAsync(5, "Foundry", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductDetail { Id = 5, Name = "Foundry" });
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#productName").Change("Foundry");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Rename").Click();

        // Assert
        cut.Find("h1").TextContent.Should().Be("Foundry");
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Renamed to 'Foundry'.");
    }

    [Fact]
    public void Rename_Refused_ShowsTheServerMessage()
    {
        // Arrange
        _api.Setup(x => x.RenameProductAsync(5, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("'Azure Functions' is already used by another product."));
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#productName").Change("Azure Functions");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Rename").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("already used by another product");
        cut.Find("h1").TextContent.Should().Be("Microsoft Foundry");
    }

    [Fact]
    public void AddAlias_CallsTheApiAndShowsTheUpdatedProduct()
    {
        // Arrange
        var updated = Detail();
        updated.Aliases = [.. updated.Aliases, new ProductAlias { Id = 2, Alias = "Foundry" }];
        _api.Setup(x => x.AddProductAliasAsync(5, "Foundry", It.IsAny<CancellationToken>())).ReturnsAsync(updated);
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#newAlias").Change("Foundry");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Add alias").Click();

        // Assert
        cut.Find("#aliases-list").TextContent.Should().Contain("Foundry");
        cut.FindAll("#aliases-list li").Should().HaveCount(2);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Alias 'Foundry' added.");
    }

    [Fact]
    public void RemoveAlias_CallsTheApiAndReloads()
    {
        // Arrange
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(new ProductDetail { Id = 5, Name = "Microsoft Foundry" });

        // Act
        cut.Find("#aliases-list .admin-link-btn").Click();

        // Assert
        _api.Verify(x => x.RemoveProductAliasAsync(5, 1, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find("#aliases-empty").Should().NotBeNull();
    }

    [Fact]
    public void Render_ShowsTheParentAndTheChildrenOfTheProduct()
    {
        // Arrange
        var detail = Detail();
        detail.Parent = new ProductReference { Id = 6, Name = "Azure Functions" };
        detail.Children = [new ProductReference { Id = 7, Name = "Foundry Agent Service" }];
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("#parentName").GetAttribute("value").Should().Be("Azure Functions");
        cut.Find("#children-list a").GetAttribute("href").Should().Be("/admin/products/7");
        cut.Find("#children-list a").TextContent.Should().Be("Foundry Agent Service");
    }

    [Fact]
    public void Render_ProductWithoutChildren_ShowsEmptyMessage()
    {
        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("#children-empty").Should().NotBeNull();
        cut.Find("#parentName").GetAttribute("value").Should().BeNullOrEmpty();
    }

    [Fact]
    public void Render_LinksOfChildProducts_ShowWhichProductTheyAreAbout()
    {
        // Arrange
        var detail = Detail();
        detail.Children = [new ProductReference { Id = 7, Name = "Microsoft Foundry Agent Service" }];
        SetArticles(DefaultArticle(), new ProductArticleLink { CollectionName = "news", Slug = "agent-post", Title = "Agent post", DateEpoch = 1_767_225_600, ProductName = "Foundry Agent Service", CatalogName = "Microsoft Foundry Agent Service" });
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        // Act
        var cut = RenderDetail("articles");

        // Assert
        cut.Find("#articles-table thead").TextContent.Should().Contain("Product");
        cut.FindAll("#articles-table tbody tr").Select(r => r.TextContent).Should().Contain(t => t.Contains("Microsoft Foundry Agent Service"));
    }

    [Fact]
    public void Render_WhenTheProductHasNoChildren_HidesTheProductColumn()
    {
        // Act
        var articles = RenderDetail("articles");
        var dates = RenderDetail("dates");

        // Assert
        articles.Find("#articles-table thead").TextContent.Should().NotContain("Product");
        dates.Find("#events-table thead").TextContent.Should().NotContain("Product");
    }

    [Fact]
    public void SetParent_CallsTheApiWithTheParentAndShowsTheResult()
    {
        // Arrange
        var updated = Detail();
        updated.Parent = new ProductReference { Id = 6, Name = "Azure Functions" };
        _api.Setup(x => x.SetProductParentAsync(5, 6, It.IsAny<CancellationToken>())).ReturnsAsync(updated);
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#parentName").Change("azure functions");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Set parent").Click();

        // Assert
        _api.Verify(x => x.SetProductParentAsync(5, 6, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("belongs to 'Azure Functions'");
    }

    [Fact]
    public void SetParent_WithEmptyName_MakesTheProductTopLevel()
    {
        // Arrange
        _api.Setup(x => x.SetProductParentAsync(5, null, It.IsAny<CancellationToken>())).ReturnsAsync(Detail());
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#parentName").Change(" ");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Set parent").Click();

        // Assert
        _api.Verify(x => x.SetProductParentAsync(5, null, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("top-level product");
    }

    [Fact]
    public void SetParent_ToUnknownProduct_ShowsErrorWithoutCallingTheApi()
    {
        // Arrange
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#parentName").Change("Nonexistent");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Set parent").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("'Nonexistent' is not a product.");
        _api.Verify(x => x.SetProductParentAsync(It.IsAny<long>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Render_ShowsTheFeaturesWithAliasesAndTheNamesToReview()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(Overview());

        // Act
        var cut = RenderDetail("features");

        // Assert
        cut.Markup.Should().Contain("Features (1)").And.Contain("Features to review (1)");
        var summary = cut.Find("#feature-31 summary").TextContent;
        summary.Should().Contain("Agent mode").And.Contain("4 dates").And.Contain("autopilot");
        cut.FindAll("#features-unmatched-table tbody tr").Should().ContainSingle().Which.TextContent.Should().Contain("Plan mode").And.Contain("2");
    }

    [Fact]
    public void Render_WithoutFeatures_ShowsEmptyMessages()
    {
        // Act
        var cut = RenderDetail("features");

        // Assert
        cut.Find("#features-empty").Should().NotBeNull();
        cut.Find("#features-unmatched-empty").Should().NotBeNull();
    }

    [Fact]
    public void AddFeature_CallsTheApiAndShowsTheOverview()
    {
        // Arrange
        _api.Setup(x => x.CreateProductFeatureAsync(5, "Agent mode", It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        var cut = RenderDetail("features");
        cut.Find("#newFeature").Change("Agent mode");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Add feature").Click();

        // Assert
        _api.Verify(x => x.CreateProductFeatureAsync(5, "Agent mode", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Feature 'Agent mode' added.");
        cut.Find("#feature-31").Should().NotBeNull();
    }

    [Fact]
    public void AddFeature_Refused_ShowsTheServerMessage()
    {
        // Arrange
        _api.Setup(x => x.CreateProductFeatureAsync(5, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("'Agent mode' is already used by 'Agent mode'."));
        var cut = RenderDetail("features");
        cut.Find("#newFeature").Change("Agent mode");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Add feature").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("already used");
    }

    [Fact]
    public void RenameFeature_CallsTheApi()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        _api.Setup(x => x.RenameProductFeatureAsync(5, 31, "Agent", It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        var cut = RenderDetail("features");
        cut.Find("#feature-name-31").Change("Agent");

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Rename").Click();

        // Assert
        _api.Verify(x => x.RenameProductFeatureAsync(5, 31, "Agent", It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Renamed to 'Agent'.");
    }

    [Fact]
    public void FeatureAlias_AddAndRemoveCallTheApi()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        _api.Setup(x => x.AddProductFeatureAliasAsync(5, 31, "auto", It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        var cut = RenderDetail("features");
        cut.Find("#feature-alias-31").Change("auto");

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Add alias").Click();
        cut.FindAll("#feature-31 li button").Single(b => b.TextContent == "Remove").Click();

        // Assert
        _api.Verify(x => x.AddProductFeatureAliasAsync(5, 31, "auto", It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.RemoveProductFeatureAliasAsync(5, 31, 41, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void DeleteFeature_AsksForConfirmationBeforeCallingTheApi()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        var cut = RenderDetail("features");

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Delete…").Click();

        // Assert — nothing happens until confirmed
        cut.Find("#feature-31 .admin-alert-error").TextContent.Should().Contain("Delete 'Agent mode'?");
        _api.Verify(x => x.DeleteProductFeatureAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Confirm delete").Click();

        // Assert
        _api.Verify(x => x.DeleteProductFeatureAsync(5, 31, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void FeatureDetails_ShowTheDescriptionTheReleaseDateAndTheLinkedContent()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());

        // Act
        var cut = RenderDetail("features");

        // Assert
        cut.Find("#feature-description-31").GetAttribute("value").Should().Be("Works on its own");
        cut.Find("#feature-release-31").GetAttribute("value").Should().Be("2026-03-10");
        var rows = cut.FindAll("#feature-content-31 tbody tr");
        rows.Should().HaveCount(2);
        rows[0].QuerySelector("a")!.GetAttribute("href").Should().Be("/admin/content-preview/videos/intro");
        rows[0].QuerySelector("a")!.TextContent.Should().Be("Intro video");
        rows[0].QuerySelector(".badge-success").Should().NotBeNull("the first link is the thumbnail");
        rows[1].QuerySelector(".badge-success").Should().BeNull();
        cut.Markup.Should().Contain("Content (2)");
    }

    [Fact]
    public void SaveDetails_SendsTheDescriptionAndTheDate()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        _api.Setup(x => x.UpdateProductFeatureDetailsAsync(5, 31, "New text", new DateOnly(2026, 4, 1), It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        var cut = RenderDetail("features");
        cut.Find("#feature-description-31").Change("  New text ");
        cut.Find("#feature-release-31").Change("2026-04-01");

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Save details").Click();

        // Assert
        _api.Verify(x => x.UpdateProductFeatureDetailsAsync(5, 31, "New text", new DateOnly(2026, 4, 1), It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Details of 'Agent mode' saved.");
    }

    [Fact]
    public void SaveDetails_WithAnEmptyDate_ClearsTheReleaseDate()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        _api.Setup(x => x.UpdateProductFeatureDetailsAsync(5, 31, "Works on its own", null, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        var cut = RenderDetail("features");
        cut.Find("#feature-release-31").Change(string.Empty);

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Save details").Click();

        // Assert
        _api.Verify(x => x.UpdateProductFeatureDetailsAsync(5, 31, "Works on its own", null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void SaveDetails_WithAnInvalidDate_ShowsErrorWithoutCallingTheApi()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        var cut = RenderDetail("features");
        cut.Find("#feature-release-31").Change("31-12-2025");

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Save details").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("'31-12-2025' is not a date.");
        _api.Verify(x => x.UpdateProductFeatureDetailsAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void SaveDetails_ApiFails_ShowsTheError()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        _api.Setup(x => x.UpdateProductFeatureDetailsAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("The description can be at most 4000 characters."));
        var cut = RenderDetail("features");

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Save details").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("at most 4000 characters");
    }

    [Fact]
    public void FeatureContent_SearchListsTheResults_AndLinkingCallsTheApi()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        _api.Setup(x => x.GetContentItemsAsync(1, 20, "agent", "videos", It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<ContentItemListItem>
            {
                Items = [CreateItem("videos", "intro", "Intro video"), CreateItem("videos", "new-video", "New video")],
                TotalCount = 2
            });
        _api.Setup(x => x.AddProductFeatureContentAsync(5, 31, "videos", "new-video", true, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        var cut = RenderDetail("features");
        cut.Find("#feature-content-search-31").Change("agent");
        cut.Find("#feature-31 select").Change("videos");

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Search").Click();

        // Assert — an item that is linked already cannot be linked again
        var results = cut.FindAll("#feature-content-results-31 tbody tr");
        results.Should().HaveCount(2);
        results[0].TextContent.Should().Contain("linked");
        results[0].QuerySelectorAll("button").Should().BeEmpty();

        // Act
        results[1].QuerySelectorAll("button").Single(b => b.TextContent.Contains("Thumbnail")).Click();

        // Assert
        _api.Verify(x => x.AddProductFeatureContentAsync(5, 31, "videos", "new-video", true, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("'New video' linked to 'Agent mode' as its thumbnail.");
    }

    [Fact]
    public void FeatureContent_SearchWithoutResults_ShowsAMessage()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        _api.Setup(x => x.GetContentItemsAsync(1, 20, "nothing", null, It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<ContentItemListItem> { Items = [], TotalCount = 0 });
        var cut = RenderDetail("features");
        cut.Find("#feature-content-search-31").Change("nothing");

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Search").Click();

        // Assert
        cut.Find("#feature-content-results-empty-31").TextContent.Should().Contain("No content found.");
    }

    [Fact]
    public void FeatureContent_SearchFails_ShowsTheError()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        _api.Setup(x => x.GetContentItemsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));
        var cut = RenderDetail("features");
        cut.Find("#feature-content-search-31").Change("agent");

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Search").Click();

        // Assert
        cut.Find("#feature-31 .admin-alert-error").TextContent.Should().Contain("Search failed: boom");
    }

    [Fact]
    public void FeatureContent_SearchWithAnEmptyQuery_DoesNothing()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        var cut = RenderDetail("features");

        // Act
        cut.FindAll("#feature-31 button").Single(b => b.TextContent == "Search").Click();

        // Assert
        _api.Verify(x => x.GetContentItemsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void FeatureContent_RemoveAndMakeThumbnail_CallTheApiAndReload()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(OverviewWithDetails());
        var cut = RenderDetail("features");

        // Act
        cut.FindAll("#feature-content-31 tbody tr")[1].QuerySelector("button[title='Make this the thumbnail']")!.Click();
        cut.FindAll("#feature-content-31 tbody tr")[1].QuerySelectorAll("button").Single(b => b.TextContent == "Remove").Click();

        // Assert
        _api.Verify(x => x.SetProductFeatureThumbnailAsync(5, 31, "videos", "demo", It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.RemoveProductFeatureContentAsync(5, 31, "videos", "demo", It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Fact]
    public void ReviewQueue_AddAsFeatureAndAsAlias_CallTheApi()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        _api.Setup(x => x.CreateProductFeatureAsync(5, "Plan mode", It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        _api.Setup(x => x.AddProductFeatureAliasAsync(5, 31, "Plan mode", It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        var cut = RenderDetail("features");

        // Act
        cut.FindAll("#features-unmatched-table button").Single(b => b.TextContent == "Add as feature").Click();
        cut.Find("#features-unmatched-table input").Change("agent MODE");
        cut.FindAll("#features-unmatched-table button").Single(b => b.TextContent == "Add as alias").Click();

        // Assert
        _api.Verify(x => x.CreateProductFeatureAsync(5, "Plan mode", It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.AddProductFeatureAliasAsync(5, 31, "Plan mode", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void ReviewQueue_AddAsAliasOfUnknownFeature_ShowsErrorWithoutCallingTheApi()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        var cut = RenderDetail("features");
        cut.Find("#features-unmatched-table input").Change("Nonexistent");

        // Act
        cut.FindAll("#features-unmatched-table button").Single(b => b.TextContent == "Add as alias").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("'Nonexistent' is not a feature of this product.");
        _api.Verify(x => x.AddProductFeatureAliasAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Convert_IsOnlyOfferedForAChildProduct_AsksForConfirmation_AndThenOpensTheFeaturesOfTheParent()
    {
        // Arrange
        var withoutParent = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        withoutParent.FindAll("#convert-button").Should().BeEmpty();

        var child = new ProductDetail { Id = 5, Name = "GitHub Copilot coding agent", Parent = new ProductReference { Id = 6, Name = "GitHub Copilot" } };
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(child);
        _api.Setup(x => x.ConvertProductToFeatureAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(new ProductDetail { Id = 6, Name = "GitHub Copilot" });
        _api.Setup(x => x.GetProductAsync(6, It.IsAny<CancellationToken>())).ReturnsAsync(new ProductDetail { Id = 6, Name = "GitHub Copilot" });
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Act
        cut.Find("#convert-button").Click();

        // Assert — nothing happens until confirmed
        cut.Find("#convert-description").TextContent.Should().Contain("capability").And.Contain("aliases of that feature");
        cut.Find("#confirm-convert").TextContent.Should().Contain("Convert 'GitHub Copilot coding agent' into a feature of 'GitHub Copilot'?");
        _api.Verify(x => x.ConvertProductToFeatureAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);

        // Act
        cut.Find("#confirm-convert .btn").Click();

        // Assert
        _api.Verify(x => x.ConvertProductToFeatureAsync(5, It.IsAny<CancellationToken>()), Times.Once);
        Services.GetRequiredService<NavigationManager>().Uri.Should().EndWith("/admin/products/6/features");
    }

    [Fact]
    public void Merge_AsksForConfirmationBeforeCallingTheApi_AndThenNavigatesToTheTarget()
    {
        // Arrange
        _api.Setup(x => x.MergeProductAsync(5, 6, It.IsAny<CancellationToken>())).ReturnsAsync(new ProductDetail { Id = 6, Name = "Azure Functions" });
        _api.Setup(x => x.GetProductAsync(6, It.IsAny<CancellationToken>())).ReturnsAsync(new ProductDetail { Id = 6, Name = "Azure Functions" });
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#mergeTarget").Change("azure functions");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Merge…").Click();

        // Assert — nothing happens until confirmed
        cut.Find("#confirm-merge").TextContent.Should().Contain("Merge 'Microsoft Foundry' into 'azure functions'?");
        _api.Verify(x => x.MergeProductAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);

        // Act
        cut.Find("#confirm-merge .btn").Click();

        // Assert
        _api.Verify(x => x.MergeProductAsync(5, 6, It.IsAny<CancellationToken>()), Times.Once);
        Services.GetRequiredService<NavigationManager>().Uri.Should().EndWith("/admin/products/6");
    }

    [Fact]
    public void Merge_IntoUnknownProduct_ShowsErrorWithoutCallingTheApi()
    {
        // Arrange
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#mergeTarget").Change("Nonexistent");
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Merge…").Click();

        // Act
        cut.Find("#confirm-merge .btn").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("'Nonexistent' is not a product.");
        _api.Verify(x => x.MergeProductAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Merge_Cancel_HidesTheConfirmation()
    {
        // Arrange
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#mergeTarget").Change("Azure Functions");
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Merge…").Click();

        // Act
        cut.Find("#confirm-merge .btn-secondary").Click();

        // Assert
        cut.FindAll("#confirm-merge").Should().BeEmpty();
    }

    [Fact]
    public void Delete_AsksForConfirmationAndThenDeletesAndGoesBackToTheList()
    {
        // Arrange
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Delete…").Click();

        // Assert — nothing happens until confirmed
        cut.Find("#confirm-delete").TextContent.Should().Contain("Its article links (1) and dates (1) become unmatched again.");
        _api.Verify(x => x.DeleteProductAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);

        // Act
        cut.Find("#confirm-delete .btn").Click();

        // Assert
        _api.Verify(x => x.DeleteProductAsync(5, It.IsAny<CancellationToken>()), Times.Once);
        Services.GetRequiredService<NavigationManager>().Uri.Should().EndWith("/admin/products");
    }

    private static ProductFeatureOverview Overview() => new()
    {
        Features =
        [
            new ProductFeature
            {
                Id = 31,
                ProductId = 5,
                Name = "Agent mode",
                EventCount = 4,
                Aliases = [new FeatureAlias { Id = 41, Alias = "autopilot" }]
            }
        ],
        Unmatched = [new UnmatchedFeatureName { Name = "Plan mode", EventCount = 2 }]
    };

    private static ProductFeatureOverview OverviewWithDetails() => new()
    {
        Features =
        [
            new ProductFeature
            {
                Id = 31,
                ProductId = 5,
                Name = "Agent mode",
                Description = "Works on its own",
                ReleaseDate = new DateOnly(2026, 3, 10),
                Content =
                [
                    new ProductFeatureContent { CollectionName = "videos", ItemSlug = "intro", Title = "Intro video", IsThumbnail = true, SortOrder = 0 },
                    new ProductFeatureContent { CollectionName = "videos", ItemSlug = "demo", Title = "Demo video", IsThumbnail = false, SortOrder = 1 }
                ]
            }
        ]
    };

    private static ContentItemListItem CreateItem(string collection, string slug, string title) => new()
    {
        Slug = slug,
        CollectionName = collection,
        Title = title,
        Author = "Author",
        FeedName = "Feed",
        ExternalUrl = "https://example.com/item",
        PrimarySectionName = "ai"
    };

    private static ProductDetail Detail() => new()
    {
        Id = 5,
        Name = "Microsoft Foundry",
        Aliases = [new ProductAlias { Id = 1, Alias = "Azure AI Foundry" }],
        ArticleCount = 1,
        EventCount = 1
    };

    private static ProductArticleLink DefaultArticle() =>
        new() { CollectionName = "news", Slug = "intro-post", Title = "Intro post", DateEpoch = 1_767_225_600, ProductName = "Azure AI Foundry", CatalogName = "Microsoft Foundry" };

    private static ProductEventLink DefaultEvent() => new()
    {
        Id = 11,
        Kind = "retirement",
        Feature = "Classic",
        Description = "Foundry Classic retires",
        EventDate = new DateOnly(2027, 6, 1),
        DatePrecision = "day",
        CollectionName = "news",
        ItemSlug = "retirement-post",
        ItemTitle = "Retirement post",
        ProductName = "Azure AI Foundry",
        CatalogName = "Microsoft Foundry"
    };

    private static PagedResult<T> Page<T>(IReadOnlyList<T> items, int? total = null) => new() { Items = items, TotalCount = total ?? items.Count };

    private void SetEvents(params ProductEventLink[] events) =>
        _api.Setup(x => x.GetProductEventsAsync(5, It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(Page<ProductEventLink>(events));

    private void SetArticles(params ProductArticleLink[] articles) =>
        _api.Setup(x => x.GetProductArticlesAsync(5, It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(Page<ProductArticleLink>(articles));
}
