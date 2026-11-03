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
        products[0].TextContent.Should().Contain("Microsoft Foundry").And.Contain("Azure AI Foundry, Foundry").And.Contain("12");
        products[0].QuerySelector("a")!.GetAttribute("href").Should().Be("/admin/products/1");
        cut.Markup.Should().Contain("Products (2)");
        cut.Find(".admin-page-subtitle a").GetAttribute("href").Should().Be("/admin/reviews#review-product-names");
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

    private static ProductSummary Product(long id, string name, int items = 0, int events = 0, string[]? aliases = null) => new()
    {
        Id = id,
        Name = name,
        ItemCount = items,
        EventCount = events,
        Aliases = (aliases ?? []).Select((alias, index) => new ProductAlias { Id = id * 100 + index, Alias = alias }).ToList()
    };

    private void Setup(IReadOnlyList<ProductSummary> products) =>
        _api.Setup(x => x.GetProductsAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>())).ReturnsAsync(products);
}
