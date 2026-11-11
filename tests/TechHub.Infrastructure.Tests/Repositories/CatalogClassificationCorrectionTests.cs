using Dapper;
using FluentAssertions;
using Npgsql;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

public class CatalogClassificationCorrectionTests : IClassFixture<DatabaseFixture<CatalogClassificationCorrectionTests>>
{
    private readonly DatabaseFixture<CatalogClassificationCorrectionTests> _fixture;

    public CatalogClassificationCorrectionTests(DatabaseFixture<CatalogClassificationCorrectionTests> fixture) => _fixture = fixture;

    [Theory]
    [InlineData("Agent Package Manager", "Agent Package Manager", null)]
    [InlineData("Azure Arc resource bridge", "Azure Arc resource bridge", "Azure Arc")]
    [InlineData("Work IQ", "Work IQ", "Microsoft 365")]
    [InlineData("Microsoft 365 Work IQ", "Work IQ", "Microsoft 365")]
    [InlineData("Microsoft 365 Copilot Work IQ", "Work IQ", "Microsoft 365")]
    [InlineData("Windows Autopatch", "Windows Autopatch", "Microsoft Windows")]
    [InlineData("ChatGPT", "ChatGPT", null)]
    [InlineData("Claude", "Claude", null)]
    [InlineData("Google Gemini", "Google Gemini", null)]
    [InlineData("Salesforce Data 360", "Salesforce Data 360", null)]
    [InlineData("Snowflake", "Snowflake", null)]
    [InlineData("Microsoft Learn", "Microsoft Learn", null)]
    public async Task FindProductId_ReviewedProducts_ResolveAndAreNotIgnored(string name, string expected, string? parent)
    {
        var result = await _fixture.Connection.QuerySingleAsync<(string Name, string? Parent)>(
            """
            SELECT p.name, parent.name AS Parent FROM products p
            LEFT JOIN products parent ON parent.id = p.parent_product_id
            WHERE p.id = find_product_id(@Name)
            """, new { Name = name });

        result.Should().Be((expected, parent));
        (await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM ignored_product_names WHERE lower(name) = lower(@Name)", new { Name = name })).Should().Be(0);
    }

    [Theory]
    [InlineData("Voice Live API", "Azure AI Speech", "Voice Live API")]
    [InlineData("Microsoft Intune Endpoint Privilege Management", "Microsoft Intune", "Endpoint Privilege Management")]
    [InlineData("Azure Arc-enabled SQL Server", "Azure Arc", "Azure Arc-enabled SQL Server")]
    public async Task ResolveEventCatalog_GenuineCapabilities_ResolveAsFeaturesAndAreNotIgnored(string name, string product, string feature)
    {
        var result = await _fixture.Connection.QuerySingleAsync<(string Product, string Feature)>(
            """
            SELECT p.name AS Product, f.name AS Feature
            FROM resolve_event_catalog(@Name, NULL) r
            JOIN products p ON p.id = r.out_product_id
            JOIN product_features f ON f.id = r.out_feature_id
            """, new { Name = name });

        result.Should().Be((product, feature));
        (await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM ignored_product_names WHERE name = @Name", new { Name = name })).Should().Be(0);
    }

    [Theory]
    [InlineData("Anthropic")]
    [InlineData("OpenAI")]
    [InlineData("Cohere Rerank")]
    [InlineData("Microsoft-Decision-1")]
    [InlineData("GitHub Secure Open Source Fund")]
    [InlineData("GitHub Security Lab")]
    [InlineData("GitHub Universe")]
    [InlineData("Microsoft Build")]
    [InlineData("Microsoft Ignite")]
    [InlineData("Microsoft Research")]
    public async Task Seed_OrganizationsModelsAndPrograms_RemainIgnored(string name)
    {
        (await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM ignored_product_names WHERE name = @Name", new { Name = name })).Should().Be(1);
        (await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM products WHERE name = @Name", new { Name = name })).Should().Be(0);
    }

    [Theory]
    [InlineData("Azure Arc", "Azure Arc resource bridge", "Azure Arc resource bridge")]
    [InlineData("Microsoft 365", "Work IQ", "Work IQ")]
    [InlineData("Microsoft Windows", "Windows Autopatch", "Windows Autopatch")]
    public async Task ResolveEventCatalog_FormerFeatures_LinkToTheProductWithoutDuplicateFeatures(string parent, string name, string expected)
    {
        var result = await _fixture.Connection.QuerySingleAsync<(string Product, long? Feature)>(
            """
            SELECT p.name AS Product, r.out_feature_id AS Feature
            FROM resolve_event_catalog(@Parent, @Name) r
            JOIN products p ON p.id = r.out_product_id
            """, new { Parent = parent, Name = name });

        result.Should().Be((expected, (long?)null));
        (await _fixture.Connection.ExecuteScalarAsync<long>(
            """
            SELECT count(*) FROM product_features f JOIN products p ON p.id = f.product_id
            WHERE p.name = @Parent AND f.name = @Name
            """, new { Parent = parent, Name = name })).Should().Be(0);
    }

    [Fact]
    public async Task Migration_EarlierSeededDatabase_RemovesObsoleteFeaturesAndExclusions()
    {
        using var transaction = _fixture.Connection.BeginTransaction();
        await _fixture.Connection.ExecuteAsync(
            """
            INSERT INTO product_features (product_id, name)
            SELECT p.id, v.feature FROM (VALUES
                ('Azure Arc', 'Azure Arc resource bridge'),
                ('Microsoft 365 Copilot', 'Work IQ'),
                ('Microsoft Intune', 'Windows Autopatch')
            ) v(product, feature) JOIN products p ON p.name = v.product;
            INSERT INTO product_feature_aliases (feature_id, alias)
            SELECT f.id, 'Microsoft 365 Work IQ'
            FROM product_features f JOIN products p ON p.id = f.product_id
            WHERE p.name = 'Microsoft 365 Copilot' AND f.name = 'Work IQ';
            INSERT INTO ignored_product_names (name) VALUES
                ('Agent Package Manager'), ('Azure Arc resource bridge'), ('Microsoft 365 Work IQ'),
                ('Windows Autopatch'), ('Voice Live API'), ('Microsoft Intune Endpoint Privilege Management'),
                ('Azure Arc-enabled SQL Server'), ('ChatGPT'), ('Claude'), ('Google Gemini'),
                ('Salesforce Data 360'), ('Snowflake'), ('Microsoft Learn');
            """, transaction: transaction);

        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync(), transaction: transaction);

        (await _fixture.Connection.ExecuteScalarAsync<long>(
            """
            SELECT count(*) FROM product_features f JOIN products p ON p.id = f.product_id
            WHERE (p.name, f.name) IN (
                ('Azure Arc', 'Azure Arc resource bridge'),
                ('Microsoft 365 Copilot', 'Work IQ'),
                ('Microsoft Intune', 'Windows Autopatch'))
            """, transaction: transaction)).Should().Be(0);
        (await _fixture.Connection.ExecuteScalarAsync<long>(
            """
            SELECT count(*) FROM ignored_product_names
            WHERE name IN ('Agent Package Manager', 'Azure Arc resource bridge', 'Microsoft 365 Work IQ',
                'Windows Autopatch', 'Voice Live API', 'Microsoft Intune Endpoint Privilege Management',
                'Azure Arc-enabled SQL Server', 'ChatGPT', 'Claude', 'Google Gemini',
                'Salesforce Data 360', 'Snowflake', 'Microsoft Learn')
            """, transaction: transaction)).Should().Be(0);
    }

    [Fact]
    public async Task Migration_FormerFeatureHasCuratedData_RefusesToDiscardIt()
    {
        using var transaction = _fixture.Connection.BeginTransaction();
        await _fixture.Connection.ExecuteAsync(
            """
            INSERT INTO product_features (product_id, name, description)
            SELECT id, 'Work IQ', 'Curated feature description' FROM products WHERE name = 'Microsoft 365 Copilot'
            """, transaction: transaction);
        var sql = await ReadMigrationAsync();

        var action = async () => await _fixture.Connection.ExecuteAsync(sql, transaction: transaction);

        await action.Should().ThrowAsync<PostgresException>().WithMessage("*Catalog promotion needs manual review*");
    }

    [Fact]
    public async Task Migration_RunAgain_PreservesExistingParentsAndFeatures()
    {
        using var transaction = _fixture.Connection.BeginTransaction();
        await _fixture.Connection.ExecuteAsync(
            """
            UPDATE products SET parent_product_id = (SELECT id FROM products WHERE name = 'Microsoft Azure')
            WHERE name = 'Agent Package Manager';
            UPDATE product_features SET description = 'Curated voice description'
            WHERE product_id = (SELECT id FROM products WHERE name = 'Azure AI Speech') AND name = 'Voice Live API';
            """, transaction: transaction);
        const string Counts = """
            SELECT (SELECT count(*) FROM products) + (SELECT count(*) FROM product_aliases)
                 + (SELECT count(*) FROM product_features) + (SELECT count(*) FROM product_feature_aliases)
                 + (SELECT count(*) FROM ignored_product_names)
            """;
        var before = await _fixture.Connection.ExecuteScalarAsync<long>(Counts, transaction: transaction);
        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync(), transaction: transaction);

        (await _fixture.Connection.ExecuteScalarAsync<long>(Counts, transaction: transaction)).Should().Be(before);
        (await _fixture.Connection.ExecuteScalarAsync<string>(
            "SELECT parent.name FROM products p JOIN products parent ON parent.id = p.parent_product_id WHERE p.name = 'Agent Package Manager'",
            transaction: transaction)).Should().Be("Microsoft Azure");
        (await _fixture.Connection.ExecuteScalarAsync<string>(
            """
            SELECT description FROM product_features
            WHERE product_id = (SELECT id FROM products WHERE name = 'Azure AI Speech') AND name = 'Voice Live API'
            """, transaction: transaction)).Should().Be("Curated voice description");
    }

    private static async Task<string> ReadMigrationAsync()
    {
        var assembly = typeof(ProductRepository).Assembly;
        var resource = assembly.GetManifestResourceNames().Single(n => n.EndsWith("057_catalog_classification_corrections.sql", StringComparison.Ordinal));
        await using var stream = assembly.GetManifestResourceStream(resource)!;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
