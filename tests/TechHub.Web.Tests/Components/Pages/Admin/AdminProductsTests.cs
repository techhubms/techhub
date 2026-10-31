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
/// bUnit tests for AdminProducts.razor: the review queue of unmatched names, the product list and the actions on both.
/// </summary>
public class AdminProductsTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public AdminProductsTests()
    {
        Services.AddSingleton(_api.Object);
        Setup(products: [], unmatched: [], ignored: []);
    }

    [Fact]
    public void Render_ShowsUnmatchedNamesAndProducts()
    {
        // Arrange
        Setup(
            products: [Product(1, "Microsoft Foundry", items: 12, events: 4, aliases: ["Azure AI Foundry", "Foundry"]), Product(2, "Azure Functions")],
            unmatched: [new UnmatchedProductName { Name = "Flint-interactive", ItemCount = 3, EventCount = 1 }],
            ignored: []);

        // Act
        var cut = Render<AdminProducts>();

        // Assert
        var unmatched = cut.FindAll("#unmatched-table tbody tr").Should().ContainSingle().Subject;
        unmatched.TextContent.Should().Contain("Flint-interactive").And.Contain("3").And.Contain("1");
        var products = cut.FindAll("#products-table tbody tr");
        products.Should().HaveCount(2);
        products[0].TextContent.Should().Contain("Microsoft Foundry").And.Contain("Azure AI Foundry, Foundry").And.Contain("12");
        products[0].QuerySelector("a")!.GetAttribute("href").Should().Be("/admin/products/1");
        cut.Markup.Should().Contain("To review (1)").And.Contain("Products (2)");
    }

    [Fact]
    public void Render_NothingToReview_ShowsEmptyMessage()
    {
        // Act
        var cut = Render<AdminProducts>();

        // Assert
        cut.Find("#unmatched-empty").TextContent.Should().Contain("Nothing to review.");
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
    public void Search_FiltersOnNameAndAlias()
    {
        // Arrange
        Setup(
            products: [Product(1, "Microsoft Foundry", aliases: ["Azure AI Foundry"]), Product(2, "Azure Functions")],
            unmatched: [],
            ignored: []);
        var cut = Render<AdminProducts>();

        // Act
        cut.Find("#productSearch").Input("ai foundry");

        // Assert
        cut.FindAll("#products-table tbody tr").Should().ContainSingle().Which.TextContent.Should().Contain("Microsoft Foundry");
    }

    [Fact]
    public void AddAsProduct_CreatesTheProductAndReloads()
    {
        // Arrange
        Setup(products: [], unmatched: [Unmatched("Flint")], ignored: []);
        _api.Setup(x => x.CreateProductAsync("Flint", It.IsAny<CancellationToken>())).ReturnsAsync(new ProductDetail { Id = 9, Name = "Flint" });
        var cut = Render<AdminProducts>();

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
        Setup(products: [Product(7, "Microsoft Foundry")], unmatched: [Unmatched("Azure AI Foundry")], ignored: []);
        _api.Setup(x => x.AddProductAliasAsync(7, "Azure AI Foundry", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductDetail { Id = 7, Name = "Microsoft Foundry" });
        var cut = Render<AdminProducts>();
        cut.Find("#unmatched-table input").Change("microsoft foundry");

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
        Setup(products: [Product(7, "Microsoft Foundry")], unmatched: [Unmatched("Azure AI Foundry")], ignored: []);

        // Act
        var cut = Render<AdminProducts>();

        // Assert
        cut.FindAll("#unmatched-table button").Single(b => b.TextContent == "Add as alias").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void AddAsAlias_ToUnknownProduct_ShowsErrorWithoutCallingTheApi()
    {
        // Arrange
        Setup(products: [Product(7, "Microsoft Foundry")], unmatched: [Unmatched("Azure AI Foundry")], ignored: []);
        var cut = Render<AdminProducts>();
        cut.Find("#unmatched-table input").Change("Nonexistent");

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
        Setup(products: [], unmatched: [Unmatched("Microsoft Ignite")], ignored: []);
        var cut = Render<AdminProducts>();

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
        Setup(products: [], unmatched: [Unmatched("Flint")], ignored: []);
        _api.Setup(x => x.CreateProductAsync("Flint", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("'Flint' is already used by 'Flint Pro'."));
        var cut = Render<AdminProducts>();

        // Act
        ClickInRow(cut, "Add as product");

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("'Flint' is already used by 'Flint Pro'.");
    }

    [Fact]
    public void Restore_PutsAnIgnoredNameBack()
    {
        // Arrange
        Setup(products: [], unmatched: [], ignored: ["Microsoft Azure"]);
        var cut = Render<AdminProducts>();

        // Act
        cut.Find(".admin-plain-list .admin-link-btn").Click();

        // Assert
        _api.Verify(x => x.UnignoreProductNameAsync("Microsoft Azure", It.IsAny<CancellationToken>()), Times.Once);
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

    private static void ClickInRow(IRenderedComponent<AdminProducts> cut, string buttonText) =>
        cut.FindAll("#unmatched-table button").Single(b => b.TextContent == buttonText).Click();

    private static UnmatchedProductName Unmatched(string name) => new() { Name = name, ItemCount = 1, EventCount = 0 };

    private static ProductSummary Product(long id, string name, int items = 0, int events = 0, string[]? aliases = null) => new()
    {
        Id = id,
        Name = name,
        ItemCount = items,
        EventCount = events,
        Aliases = (aliases ?? []).Select((alias, index) => new ProductAlias { Id = id * 100 + index, Alias = alias }).ToList()
    };

    private void Setup(IReadOnlyList<ProductSummary> products, IReadOnlyList<UnmatchedProductName> unmatched, IReadOnlyList<string> ignored)
    {
        _api.Setup(x => x.GetProductsAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>())).ReturnsAsync(products);
        _api.Setup(x => x.GetUnmatchedProductNamesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(unmatched);
        _api.Setup(x => x.GetIgnoredProductNamesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(ignored);
    }
}
