using Dapper;
using FluentAssertions;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for the product catalog migration (050_product_catalog.sql). The fixture database already ran the migration on an empty
/// catalog, so these tests check the catalog it left (products, parents, features, and what is not catalogued) and that running the script again changes nothing.
/// </summary>
public class ProductCatalogMigrationTests : IClassFixture<DatabaseFixture<ProductCatalogMigrationTests>>
{
    private readonly DatabaseFixture<ProductCatalogMigrationTests> _fixture;

    public ProductCatalogMigrationTests(DatabaseFixture<ProductCatalogMigrationTests> fixture) => _fixture = fixture;

    private async Task<string?> ParentOfAsync(string product) =>
        await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT p.name FROM products c JOIN products p ON p.id = c.parent_product_id WHERE lower(c.name) = lower(@Name)", new { Name = product });

    private async Task<string?> MatchProductAsync(string name)
    {
        var id = await _fixture.Connection.ExecuteScalarAsync<long?>("SELECT find_product_id(@Name)", new { Name = name });
        return id is null ? null : await _fixture.Connection.ExecuteScalarAsync<string>("SELECT name FROM products WHERE id = @Id", new { Id = id });
    }

    [Theory]
    [InlineData("Azure SQL", "Microsoft Azure")]
    [InlineData("Azure SQL Database", "Azure SQL")]
    [InlineData("Azure SQL Managed Instance", "Azure SQL")]
    [InlineData("Azure AI Speech", "Azure AI Services")]
    [InlineData("Azure AI Vision", "Azure AI Services")]
    [InlineData("Microsoft Active Directory Certificate Services", "Active Directory")]
    [InlineData("NuGet MCP Server", "NuGet")]
    [InlineData("Microsoft Defender for Cloud", "Microsoft Defender")]
    public async Task Seed_ProductsAndTheirParents_AreInTheCatalog(string product, string parent)
    {
        (await ParentOfAsync(product)).Should().Be(parent);
    }

    [Theory]
    [InlineData("Active Directory", "Active Directory")]
    [InlineData("Azure SQL", "Azure SQL")]
    [InlineData("Kubernetes", "Kubernetes")]
    [InlineData("NuGet MCP Server", "NuGet MCP Server")]
    public async Task FindProductId_SeededProduct_MatchesExactly(string name, string expected)
    {
        (await MatchProductAsync(name)).Should().Be(expected);
    }

    [Fact]
    public async Task Seed_IgnoredNames_NeverCollideWithProductsOrAliases()
    {
        var ignored = await _fixture.Connection.ExecuteScalarAsync<long>("SELECT count(*) FROM ignored_product_names");
        ignored.Should().BeGreaterThan(0);

        var collisions = await _fixture.Connection.ExecuteScalarAsync<long>(
            """
            SELECT count(*) FROM ignored_product_names i
            WHERE EXISTS (SELECT 1 FROM products p WHERE lower(p.name) = lower(i.name))
               OR EXISTS (SELECT 1 FROM product_aliases a WHERE lower(a.alias) = lower(i.name))
            """);
        collisions.Should().Be(0);
    }

    [Fact]
    public async Task Seed_NoTwoProductsShareAName_AndNoAliasEqualsAProductName()
    {
        var duplicateNames = await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM (SELECT lower(name) FROM products GROUP BY 1 HAVING count(*) > 1) d");
        duplicateNames.Should().Be(0);

        var aliasClashes = await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM product_aliases a JOIN products p ON lower(p.name) = lower(a.alias) AND p.id <> a.product_id");
        aliasClashes.Should().Be(0);
    }

    [Fact]
    public async Task Seed_ParentChains_AreNeverCyclic()
    {
        var cyclic = await _fixture.Connection.ExecuteScalarAsync<long>(
            """
            WITH RECURSIVE chain (id, ancestor, depth) AS (
                SELECT id, parent_product_id, 1 FROM products WHERE parent_product_id IS NOT NULL
                UNION ALL
                SELECT c.id, p.parent_product_id, c.depth + 1 FROM chain c JOIN products p ON p.id = c.ancestor
                WHERE p.parent_product_id IS NOT NULL AND c.depth < 20
            )
            SELECT count(*) FROM chain WHERE id = ancestor
            """);
        cyclic.Should().Be(0);
    }

    [Fact]
    public async Task Migration_RunAgain_ChangesNothing()
    {
        const string CountSql = "SELECT (SELECT count(*) FROM products) + (SELECT count(*) FROM product_aliases) + (SELECT count(*) FROM product_features) + (SELECT count(*) FROM ignored_product_names)";
        var before = await _fixture.Connection.ExecuteScalarAsync<long>(CountSql);

        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync());

        (await _fixture.Connection.ExecuteScalarAsync<long>(CountSql)).Should().Be(before);
    }

    [Theory]
    [InlineData("GitHub Copilot Business", "GitHub Copilot")]
    [InlineData("GitHub Copilot Pro+", "GitHub Copilot")]
    [InlineData("Microsoft Defender Plan 2", "Microsoft Defender")]
    public async Task FindProductId_ANameThatIncludesAPlan_MatchesTheProduct(string name, string expected)
    {
        (await MatchProductAsync(name)).Should().Be(expected);
    }

    // A product can be used, bought or deployed by itself; a feature is only used through a product.
    [Theory]
    [InlineData("GitHub Actions", "Hosted runners")]
    [InlineData("GitHub Secret Protection", "Secret scanning")]
    [InlineData("GitHub Code Security", "Code scanning")]
    [InlineData("GitHub Copilot", "Copilot Fusion")]
    [InlineData("Microsoft Fabric", "Data Warehouse")]
    [InlineData("Microsoft Fabric", "OneLake")]
    [InlineData("Azure Monitor", "Logs")]
    [InlineData("Microsoft Windows", "BitLocker")]
    [InlineData("ASP.NET Core", "Blazor")]
    public async Task Seed_ThingsThatAreOnlyUsedThroughAProduct_AreFeaturesAndNotProducts(string product, string feature)
    {
        var features = await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM product_features f JOIN products p ON p.id = f.product_id WHERE p.name = @Product AND lower(f.name) = lower(@Feature)",
            new { Product = product, Feature = feature });
        features.Should().Be(1);

        var asProduct = await _fixture.Connection.ExecuteScalarAsync<long>("SELECT count(*) FROM products WHERE lower(name) = lower(@Feature)", new { Feature = feature });
        asProduct.Should().Be(0);
    }

    [Theory]
    [InlineData("GitHub Hosted Runners", "GitHub Actions")]
    [InlineData("GitHub Secret Scanning", "GitHub Secret Protection")]
    [InlineData("GitHub Copilot Fusion", "GitHub Copilot")]
    [InlineData("Microsoft Fabric Data Warehouse", "Microsoft Fabric")]
    [InlineData("Azure Monitor Agent", "Azure Monitor")]
    [InlineData("Microsoft Defender for Storage", "Microsoft Defender for Cloud")]
    public async Task FindProductId_TheOldProductNameOfAFeature_LinksToTheProductOfTheFeature(string name, string expected)
    {
        (await MatchProductAsync(name)).Should().Be(expected);
    }

    [Theory]
    [InlineData("GitHub Actions")]
    [InlineData("GitHub CLI")]
    [InlineData("GitHub Codespaces")]
    [InlineData("GitHub Spark")]
    [InlineData("GitHub Copilot CLI")]
    [InlineData("Foundry Local")]
    [InlineData("Azure Functions Core Tools")]
    public async Task Seed_ThingsThatCanBeUsedByThemselves_AreProducts(string product)
    {
        (await MatchProductAsync(product)).Should().Be(product);
    }

    [Fact]
    public async Task Seed_ProgramsAreIgnoredAndNotProducts()
    {
        var ignored = await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM ignored_product_names WHERE name IN ('GitHub Security Lab', 'GitHub Secure Open Source Fund')");
        ignored.Should().Be(2);
        (await MatchProductAsync("GitHub Security Lab")).Should().BeNull();
    }

    [Theory]
    [InlineData("product_qualifiers")]
    [InlineData("product_qualifier_aliases")]
    [InlineData("content_timeline_event_qualifiers")]
    public async Task PlansAndSurfaces_AreNotPartOfTheCatalog_Tables(string table)
    {
        (await _fixture.Connection.ExecuteScalarAsync<string?>("SELECT to_regclass(@Table)::text", new { Table = table })).Should().BeNull();
    }

    [Theory]
    [InlineData("content_timeline_events", "plans")]
    [InlineData("content_timeline_events", "surfaces")]
    [InlineData("product_aliases", "qualifier_id")]
    [InlineData("content_item_products", "qualifier_id")]
    public async Task PlansAndSurfaces_AreNotPartOfTheCatalog_Columns(string table, string column)
    {
        var count = await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM information_schema.columns WHERE table_name = @Table AND column_name = @Column", new { Table = table, Column = column });
        count.Should().Be(0);
    }

    [Theory]
    [InlineData("Microsoft Entra ID")]
    [InlineData("Microsoft Foundry")]
    [InlineData("Microsoft Cost Management")]
    [InlineData("Microsoft Sentinel")]
    [InlineData("Microsoft Purview")]
    [InlineData("Azure Local")]
    public async Task Seed_ProductsThatAreOrWereCalledAzure_AreUnderAzure(string product)
    {
        (await ParentOfAsync(product)).Should().Be("Microsoft Azure");
    }

    [Fact]
    public async Task Migration_ParentSetInTheAdmin_IsNeverOverwritten()
    {
        var otherParent = await _fixture.Connection.ExecuteScalarAsync<long>(
            "INSERT INTO products (name) VALUES ('A parent chosen in the admin') RETURNING id");
        await _fixture.Connection.ExecuteAsync("UPDATE products SET parent_product_id = @Parent WHERE name = 'Azure Batch'", new { Parent = otherParent });

        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync());

        (await ParentOfAsync("Azure Batch")).Should().Be("A parent chosen in the admin");
    }

    private static async Task<string> ReadMigrationAsync()
    {
        var assembly = typeof(ProductRepository).Assembly;
        var name = assembly.GetManifestResourceNames().Single(n => n.EndsWith("050_product_catalog.sql", StringComparison.Ordinal));
        await using var stream = assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
