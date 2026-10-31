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
