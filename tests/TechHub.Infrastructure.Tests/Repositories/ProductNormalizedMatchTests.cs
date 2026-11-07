using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for the normalized tier of the catalog matching (migration 061): names that differ from a catalog name
/// only in a vendor prefix, a trailing "Service", an abbreviation in parentheses or punctuation link to that product, and
/// a name that points at several products or at none stays unmatched.
/// </summary>
public class ProductNormalizedMatchTests : IClassFixture<DatabaseFixture<ProductNormalizedMatchTests>>
{
    private readonly DatabaseFixture<ProductNormalizedMatchTests> _fixture;
    private readonly ProductRepository _products;

    public ProductNormalizedMatchTests(DatabaseFixture<ProductNormalizedMatchTests> fixture)
    {
        _fixture = fixture;
        _products = new ProductRepository(fixture.Connection, NullLogger<ProductRepository>.Instance);
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    private async Task<string?> MatchAsync(string name)
    {
        var id = await _fixture.Connection.ExecuteScalarAsync<long?>("SELECT find_product_id(@Name)", new { Name = name });
        return id is null ? null : await _fixture.Connection.ExecuteScalarAsync<string>("SELECT name FROM products WHERE id = @Id", new { Id = id });
    }

    [Theory]
    [InlineData("Azure Kubernetes Service (AKS)", "Azure Kubernetes Service")]
    [InlineData("Microsoft Azure Kubernetes Service", "Azure Kubernetes Service")]
    [InlineData("azure   kubernetes  SERVICE", "Azure Kubernetes Service")]
    [InlineData("Azure Functions.", "Azure Functions")]
    [InlineData("Microsoft Azure Functions", "Azure Functions")]
    [InlineData("Microsoft Entra ID Service", "Microsoft Entra ID")]
    [InlineData("Microsoft Fabric Data Warehouse", "Microsoft Fabric")]
    [InlineData("Azure Container Registry Service", "Azure Container Registry")]
    [InlineData("GitHub  Actions", "GitHub Actions")]
    [InlineData("Entra ID", "Microsoft Entra ID")]
    [InlineData("Azure Database for MySQL Flexible", "Azure Database for MySQL")]
    public async Task FindProductId_NameThatDiffersOnlyInFormat_LinksToTheProduct(string aiName, string expected)
    {
        var matched = await MatchAsync(aiName);

        matched.Should().Be(expected);
    }

    [Theory]
    [InlineData("Azure Virtual", null)]
    [InlineData("GitHub", "GitHub")]
    [InlineData("Copilot", null)]
    [InlineData("", null)]
    [InlineData("   ", null)]
    public async Task FindProductId_NameThatIsNotTheSameProduct_StaysUnmatched(string aiName, string? expected)
    {
        var matched = await MatchAsync(aiName);

        matched.Should().Be(expected);
    }

    [Fact]
    public async Task FindProductId_NormalizedNameThatPointsAtTwoProducts_StaysUnmatched()
    {
        await _fixture.Connection.ExecuteAsync("INSERT INTO products (name) VALUES ('Contoso Widget Service'), ('Contoso Widget (Preview)') ON CONFLICT DO NOTHING");

        var matched = await MatchAsync("Contoso Widget");

        matched.Should().BeNull("two products have this normalized name, so it is not clear which one is meant");
    }

    [Fact]
    public async Task FindProductId_ExactNameWinsOverANormalizedMatch()
    {
        await _fixture.Connection.ExecuteAsync("INSERT INTO products (name) VALUES ('Fabrikam Gadget Service'), ('Fabrikam Gadget') ON CONFLICT DO NOTHING");

        (await MatchAsync("Fabrikam Gadget")).Should().Be("Fabrikam Gadget");
        (await MatchAsync("Fabrikam Gadget Service")).Should().Be("Fabrikam Gadget Service");
    }

    [Fact]
    public async Task FindProductId_NameOfAFeature_LinksToTheProductOfTheFeature()
    {
        var copilot = (await _products.GetProductsAsync("GitHub Copilot", Ct)).Single(p => p.Name == "GitHub Copilot");
        await _fixture.Connection.ExecuteAsync(
            "INSERT INTO product_features (product_id, name) VALUES (@Id, 'Normalized Feature') ON CONFLICT DO NOTHING",
            new { copilot.Id });

        (await MatchAsync("GitHub Copilot Normalized Feature")).Should().BeNull();
        (await MatchAsync("normalized feature")).Should().Be("GitHub Copilot");
    }

    [Fact]
    public async Task RelinkItemProducts_NormalizedName_IsLinkedAndStillReportedByTheName()
    {
        var collection = $"nm-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash)
            VALUES ('a', @Collection, 'Title', 'Text', '', 1750000000, 'azure', @Url, 'Author', 'Feed', '', 'hash');
            INSERT INTO content_item_products (collection_name, item_slug, product_name, position)
            VALUES (@Collection, 'a', 'Microsoft Azure Kubernetes Service (AKS)', 0);
            """,
            new { Collection = collection, Url = $"https://example.com/{collection}" });

        await _fixture.Connection.ExecuteAsync("SELECT relink_item_products(@Collection, 'a')", new { Collection = collection });

        var link = await _fixture.Connection.QuerySingleAsync<(string ProductName, string? Product)>("""
            SELECT l.product_name AS ProductName, p.name AS Product
            FROM content_item_products l LEFT JOIN products p ON p.id = l.product_id
            WHERE l.collection_name = @Collection
            """,
            new { Collection = collection });
        link.ProductName.Should().Be("Microsoft Azure Kubernetes Service (AKS)");
        link.Product.Should().Be("Azure Kubernetes Service");
    }

    [Fact]
    public async Task StartingCatalog_HasTheProductsUnderTheirParents()
    {
        var all = await _products.GetProductsAsync(null, Ct);

        all.Single(p => p.Name == "Azure Migrate").ParentName.Should().Be("Microsoft Azure");
        all.Single(p => p.Name == "GitHub Desktop").ParentName.Should().Be("GitHub");
        (await MatchAsync("Fabric Data Warehouse")).Should().Be("Microsoft Fabric");
        (await MatchAsync("AI Toolkit")).Should().Be("Microsoft Foundry Toolkit");
    }
}
