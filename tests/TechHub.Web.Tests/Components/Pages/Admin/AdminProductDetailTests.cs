using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
        _api.Setup(x => x.GetQualifiersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([new ProductQualifier { Id = 21, Kind = "plan", Name = "Business" }, new ProductQualifier { Id = 22, Kind = "surface", Name = "JetBrains IDEs" }]);
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(new ProductFeatureOverview());
    }

    [Fact]
    public void Render_ShowsAliasesDatesAndArticlesWithLinksToTheArticles()
    {
        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("h1").TextContent.Should().Be("Microsoft Foundry");
        cut.Find("#aliases-list").TextContent.Should().Contain("Azure AI Foundry");

        var eventRow = cut.FindAll("#events-table tbody tr").Should().ContainSingle().Subject;
        eventRow.TextContent.Should().Contain("2027-06-01").And.Contain("retirement").And.Contain("Foundry Classic retires");
        eventRow.QuerySelector("a")!.GetAttribute("href").Should().Be("/admin/content-preview/news/retirement-post");
        eventRow.QuerySelector("a")!.TextContent.Should().Be("Retirement post");

        var articleRow = cut.FindAll("#articles-table tbody tr").Should().ContainSingle().Subject;
        articleRow.TextContent.Should().Contain("Azure AI Foundry");
        articleRow.QuerySelector("a")!.GetAttribute("href").Should().Be("/admin/content-preview/news/intro-post");
        cut.Markup.Should().Contain("Dates (1)").And.Contain("Articles (1)");
    }

    [Theory]
    [InlineData("day", "2027-06-01")]
    [InlineData("month", "2027-06")]
    [InlineData("quarter", "2027-Q2")]
    public void Render_FormatsTheDateByPrecision(string precision, string expected)
    {
        // Arrange
        var detail = Detail();
        detail.Events[0].DatePrecision = precision;
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("#events-table tbody tr td").TextContent.Should().Be(expected);
    }

    [Fact]
    public void Render_ProductWithoutLinks_ShowsEmptyMessages()
    {
        // Arrange
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(new ProductDetail { Id = 5, Name = "Empty" });

        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("#aliases-empty").Should().NotBeNull();
        cut.Find("#events-empty").Should().NotBeNull();
        cut.Find("#articles-empty").Should().NotBeNull();
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
        _api.Setup(x => x.AddProductAliasAsync(5, "Foundry", null, It.IsAny<CancellationToken>())).ReturnsAsync(updated);
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
        detail.Articles = [.. detail.Articles, new ProductArticleLink { CollectionName = "news", Slug = "agent-post", Title = "Agent post", DateEpoch = 1_767_225_600, ProductName = "Foundry Agent Service", CatalogName = "Microsoft Foundry Agent Service" }];
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("#articles-table thead").TextContent.Should().Contain("Product");
        cut.FindAll("#articles-table tbody tr").Select(r => r.TextContent).Should().Contain(t => t.Contains("Microsoft Foundry Agent Service"));
    }

    [Fact]
    public void Render_WhenAllLinksAreAboutTheProductItself_HidesTheProductColumn()
    {
        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("#articles-table thead").TextContent.Should().NotContain("Product");
        cut.Find("#events-table thead").TextContent.Should().NotContain("Product");
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
    public void Render_ShowsThePlanOrSurfaceOfAliasesArticlesAndDates()
    {
        // Arrange
        var detail = Detail();
        detail.Aliases = [new ProductAlias { Id = 1, Alias = "GitHub Copilot Business", QualifierId = 21, QualifierName = "Business", QualifierKind = "plan" }];
        detail.Articles[0].QualifierName = "Business";
        detail.Events[0].Qualifiers = [new QualifierReference { Id = 21, Kind = "plan", Name = "Business" }, new QualifierReference { Id = 22, Kind = "surface", Name = "JetBrains IDEs" }];
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("#aliases-list select").GetAttribute("value").Should().Be("21");
        cut.Find("#events-table thead").TextContent.Should().Contain("Plans and surfaces");
        cut.Find("#events-table tbody").TextContent.Should().Contain("Business, JetBrains IDEs");
        cut.Find("#articles-table thead").TextContent.Should().Contain("Plan or surface");
        cut.Find("#articles-table tbody").TextContent.Should().Contain("Business");
    }

    [Fact]
    public void Render_WithoutQualifiers_HidesTheQualifierColumns()
    {
        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("#events-table thead").TextContent.Should().NotContain("Plans and surfaces");
        cut.Find("#articles-table thead").TextContent.Should().NotContain("Plan or surface");
    }

    [Fact]
    public void AddAlias_WithAPlanOrSurface_PassesTheQualifierToTheApi()
    {
        // Arrange
        _api.Setup(x => x.AddProductAliasAsync(5, "Foundry Business", 21, It.IsAny<CancellationToken>())).ReturnsAsync(Detail());
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#newAlias").Change("Foundry Business");
        cut.Find("#newAliasQualifier").Change("21");

        // Act
        cut.FindAll(".admin-inline-form button").Single(b => b.TextContent == "Add alias").Click();

        // Assert
        _api.Verify(x => x.AddProductAliasAsync(5, "Foundry Business", 21, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void SetAliasQualifier_CallsTheApiAndClearsWhenNoneIsChosen()
    {
        // Arrange
        _api.Setup(x => x.SetProductAliasQualifierAsync(5, 1, 22, It.IsAny<CancellationToken>())).ReturnsAsync(Detail());
        _api.Setup(x => x.SetProductAliasQualifierAsync(5, 1, null, It.IsAny<CancellationToken>())).ReturnsAsync(Detail());
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Act
        cut.Find("#aliases-list select").Change("22");
        cut.Find("#aliases-list select").Change("");

        // Assert
        _api.Verify(x => x.SetProductAliasQualifierAsync(5, 1, 22, It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.SetProductAliasQualifierAsync(5, 1, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Render_ShowsTheFeaturesWithAliasesAndTheNamesToReview()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(Overview());

        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

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
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("#features-empty").Should().NotBeNull();
        cut.Find("#features-unmatched-empty").Should().NotBeNull();
    }

    [Fact]
    public void FeatureGrid_ListsTheDatesPerFeatureUnderTheirSurfaceOrAllSurfaces()
    {
        // Arrange
        var detail = Detail();
        detail.Events =
        [
            Event(1, "preview", "Agent mode", "Agent mode", "2026-03-10", [new QualifierReference { Id = 22, Kind = "surface", Name = "JetBrains IDEs" }]),
            Event(2, "ga", "agent mode", "Agent mode", "2026-04-20", [new QualifierReference { Id = 20, Kind = "surface", Name = "Visual Studio Code" }, new QualifierReference { Id = 21, Kind = "plan", Name = "Business" }]),
            Event(3, "launch", "Unmatched text", null, "2026-05-01", []),
            Event(4, "ga", null, null, "2026-06-01", [])
        ];
        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.FindAll("#feature-grid thead th").Select(h => h.TextContent).Should().Equal("Feature", "All surfaces", "JetBrains IDEs", "Visual Studio Code");
        var rows = cut.FindAll("#feature-grid tbody tr");
        rows.Should().HaveCount(2);
        var agent = rows.Single(r => r.QuerySelector("td")!.TextContent == "Agent mode").QuerySelectorAll("td");
        agent[1].TextContent.Trim().Should().BeEmpty();
        agent[2].TextContent.Trim().Should().Be("preview 2026-03-10");
        agent[3].TextContent.Trim().Should().Be("ga 2026-04-20 (Business)");
        var unmatched = rows.Single(r => r.QuerySelector("td")!.TextContent == "Unmatched text").QuerySelectorAll("td");
        unmatched[1].TextContent.Trim().Should().Be("launch 2026-05-01");
    }

    [Fact]
    public void FeatureGrid_WithoutDatesThatHaveAFeature_ShowsEmptyMessage()
    {
        // Arrange
        var detail = Detail();
        foreach (var item in detail.Events)
        {
            item.Feature = null;
        }

        _api.Setup(x => x.GetProductAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        // Act
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

        // Assert
        cut.Find("#feature-grid-empty").Should().NotBeNull();
    }

    [Fact]
    public void AddFeature_CallsTheApiAndShowsTheOverview()
    {
        // Arrange
        _api.Setup(x => x.CreateProductFeatureAsync(5, "Agent mode", It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
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
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
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
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
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
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
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
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

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
    public void ReviewQueue_AddAsFeatureAndAsAlias_CallTheApi()
    {
        // Arrange
        _api.Setup(x => x.GetProductFeaturesAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        _api.Setup(x => x.CreateProductFeatureAsync(5, "Plan mode", It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        _api.Setup(x => x.AddProductFeatureAliasAsync(5, 31, "Plan mode", It.IsAny<CancellationToken>())).ReturnsAsync(Overview());
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));

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
        var cut = Render<AdminProductDetail>(p => p.Add(x => x.Id, 5));
        cut.Find("#features-unmatched-table input").Change("Nonexistent");

        // Act
        cut.FindAll("#features-unmatched-table button").Single(b => b.TextContent == "Add as alias").Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("'Nonexistent' is not a feature of this product.");
        _api.Verify(x => x.AddProductFeatureAliasAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
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

    private static ProductEventLink Event(long id, string kind, string? feature, string? featureName, string date, IReadOnlyList<QualifierReference> qualifiers) => new()
    {
        Id = id,
        Kind = kind,
        Feature = feature,
        FeatureName = featureName,
        Description = $"{kind} of {feature}",
        EventDate = DateOnly.Parse(date, System.Globalization.CultureInfo.InvariantCulture),
        DatePrecision = "day",
        CollectionName = "news",
        ItemSlug = $"post-{id}",
        ItemTitle = $"Post {id}",
        ProductName = "Microsoft Foundry",
        CatalogName = "Microsoft Foundry",
        Qualifiers = qualifiers
    };

    private static ProductDetail Detail() => new()
    {
        Id = 5,
        Name = "Microsoft Foundry",
        Aliases = [new ProductAlias { Id = 1, Alias = "Azure AI Foundry" }],
        Articles =
        [
            new ProductArticleLink { CollectionName = "news", Slug = "intro-post", Title = "Intro post", DateEpoch = 1_767_225_600, ProductName = "Azure AI Foundry", CatalogName = "Microsoft Foundry" }
        ],
        Events =
        [
            new ProductEventLink
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
            }
        ]
    };
}
