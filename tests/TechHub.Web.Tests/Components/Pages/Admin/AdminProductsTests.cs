using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models.Admin;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminProducts.razor: the product list, its search and adding a product.
/// </summary>
public class AdminProductsTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminProductsTests()
    {
        Services.AddSingleton(_api.Object);
        Setup(products: []);
    }

    [Fact]
    public void Render_ShowsTheProductsWithTheirAliasesAndCounts()
    {
        // Arrange
        Setup(
            products: [Product(1, "Microsoft Foundry", items: 12, events: 4, aliases: ["Azure AI Foundry", "Foundry"]), Product(2, "Azure Functions")]);

        // Act
        var cut = Render<AdminProducts>();

        // Assert
        var products = cut.FindAll("#products-table tbody tr");
        products.Should().HaveCount(2);
        products[1].TextContent.Should().Contain("Microsoft Foundry").And.Contain("Azure AI Foundry, Foundry").And.Contain("12");
        products[1].QuerySelector("a")!.GetAttribute("href").Should().Be("/admin/products/1");
        cut.Markup.Should().Contain("Products (2)");
        cut.Find(".admin-page-subtitle a").GetAttribute("href").Should().Be("/admin/reviews#review-product-names");
    }

    [Fact]
    public void Render_ShowsTheNumberOfChildProductsFeaturesArticlesAndDatesOfEachProduct()
    {
        // Arrange
        Setup(products: [Product(1, "GitHub Copilot", items: 12, events: 4, features: 7, children: 3)]);

        // Act
        var cut = Render<AdminProducts>();

        // Assert
        cut.FindAll("#products-table thead th").Select(h => h.TextContent).Should().Equal(
            "Name", "Aliases", "Child products", "Features", "Articles", "Dates", "Actions");
        var cells = cut.FindAll("#products-table tbody tr td").Select(c => c.TextContent).ToList();
        cells.Skip(2).Take(4).Should().Equal("3", "7", "12", "4");
    }

    [Fact]
    public void Render_ExplainsHowProductsAndFeaturesFitTogether()
    {
        // Act
        var cut = Render<AdminProducts>();

        // Assert
        cut.Find("#catalog-glossary").TextContent.Should().Contain("Product").And.Contain("Feature").And.NotContain("Surface");
    }

    [Fact]
    public void Render_ProductWithManyAliases_ShowsTheFirstFewAndTheNumberOfOthers()
    {
        // Arrange
        Setup(products: [Product(1, "GitHub Copilot", aliases: ["A1", "A2", "A3", "A4", "A5", "A6"])]);

        // Act
        var cut = Render<AdminProducts>();

        // Assert
        var cell = cut.Find("#products-table tbody tr .admin-aliases");
        cell.TextContent.Should().Be("A1, A2, A3 and 3 more");
        cell.GetAttribute("title").Should().Be("A1, A2, A3, A4, A5, A6");
    }

    [Fact]
    public void Render_NoProducts_ShowsEmptyMessage()
    {
        // Act
        var cut = Render<AdminProducts>();

        // Assert
        cut.Find("#products-empty").TextContent.Should().Contain("No products found.");
    }

    [Fact]
    public void Render_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        _api.Setup(x => x.GetProductsAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("down"));

        // Act
        var cut = Render<AdminProducts>();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to load products: down");
    }

    [Fact]
    public void Render_NestsChildProductsBelowTheirParent()
    {
        // Arrange
        Setup(products:
        [
            Product(3, "GitHub Copilot CLI", parentId: 2),
            Product(1, "Azure Functions"),
            Product(4, "GitHub Copilot CLI extensions", parentId: 3),
            Product(2, "GitHub Copilot", children: 1),
            Product(5, "GitHub Copilot Fusion", parentId: 2)
        ]);

        // Act
        var cut = Render<AdminProducts>();

        // Assert
        var rows = cut.FindAll("#products-table tbody tr");
        rows.Select(r => r.QuerySelector("td")!.TextContent).Should().Equal(
            "Azure Functions", "GitHub Copilot", "GitHub Copilot CLI", "GitHub Copilot CLI extensions", "GitHub Copilot Fusion");
        rows.Select(r => r.QuerySelector("td")!.GetAttribute("style")).Should().Equal(
            "--tree-depth: 0", "--tree-depth: 0", "--tree-depth: 1", "--tree-depth: 2", "--tree-depth: 1");
    }

    [Fact]
    public void Search_KeepsTheParentsOfTheMatchesDimmedSoTheNestingStays()
    {
        // Arrange
        Setup(products: [Product(1, "GitHub Copilot"), Product(2, "GitHub Copilot CLI", parentId: 1), Product(3, "Azure Functions")]);
        var cut = Render<AdminProducts>();

        // Act
        cut.Find("#productSearch").Input("cli");

        // Assert
        var rows = cut.FindAll("#products-table tbody tr");
        rows.Select(r => r.QuerySelector("td")!.TextContent).Should().Equal("GitHub Copilot", "GitHub Copilot CLI");
        rows[0].ClassList.Should().Contain("admin-row-context");
        rows[1].ClassList.Should().NotContain("admin-row-context");
    }

    [Fact]
    public void Search_FiltersOnNameAndAlias()
    {
        // Arrange
        Setup(
            products: [Product(1, "Microsoft Foundry", aliases: ["Azure AI Foundry"]), Product(2, "Azure Functions")]);
        var cut = Render<AdminProducts>();

        // Act
        cut.Find("#productSearch").Input("ai foundry");

        // Assert
        cut.FindAll("#products-table tbody tr").Should().ContainSingle().Which.TextContent.Should().Contain("Microsoft Foundry");
    }

    [Fact]
    public void AddProduct_Save_CreatesTheProductFromTheForm()
    {
        // Arrange
        _api.Setup(x => x.CreateProductAsync("Azure Foo", It.IsAny<CancellationToken>())).ReturnsAsync(new ProductDetail { Id = 3, Name = "Azure Foo" });
        var cut = Render<AdminProducts>();
        cut.Find(".admin-actions .btn").Click();
        cut.Find("#productName").Change("Azure Foo");

        // Act
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        _api.Verify(x => x.CreateProductAsync("Azure Foo", It.IsAny<CancellationToken>()), Times.Once);
        cut.FindAll(".admin-modal").Should().BeEmpty();
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Product 'Azure Foo' added.");
    }

    [Fact]
    public void AddProduct_Refused_KeepsTheFormOpen()
    {
        // Arrange
        _api.Setup(x => x.CreateProductAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("A name is required."));
        var cut = Render<AdminProducts>();
        cut.Find(".admin-actions .btn").Click();

        // Act
        cut.Find(".admin-modal-footer .btn").Click();

        // Assert
        cut.FindAll(".admin-modal").Should().ContainSingle();
        cut.Find(".admin-alert-error").TextContent.Should().Contain("A name is required.");
    }

    [Fact]
    public void AddProduct_EscapeKey_ClosesTheForm()
    {
        // Arrange
        var cut = Render<AdminProducts>();
        cut.Find(".admin-actions .btn").Click();

        // Act
        cut.Find(".admin-modal-backdrop").TriggerEvent("onkeydown", new KeyboardEventArgs { Key = "Escape" });

        // Assert
        cut.FindAll(".admin-modal").Should().BeEmpty();
    }

    private static ProductSummary Product(long id, string name, int items = 0, int events = 0, string[]? aliases = null, int features = 0, int children = 0, long? parentId = null) => new()
    {
        Id = id,
        Name = name,
        ParentId = parentId,
        ItemCount = items,
        EventCount = events,
        FeatureCount = features,
        ChildCount = children,
        Aliases = (aliases ?? []).Select((alias, index) => new ProductAlias { Id = id * 100 + index, Alias = alias }).ToList()
    };

    private void Setup(IReadOnlyList<ProductSummary> products) =>
        _api.Setup(x => x.GetProductsAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>())).ReturnsAsync(products);
}
