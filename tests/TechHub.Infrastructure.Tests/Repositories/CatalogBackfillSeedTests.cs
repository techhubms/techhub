using System.Text.Json;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

public class CatalogBackfillSeedTests : IClassFixture<DatabaseFixture<CatalogBackfillSeedTests>>
{
    private readonly DatabaseFixture<CatalogBackfillSeedTests> _fixture;

    public CatalogBackfillSeedTests(DatabaseFixture<CatalogBackfillSeedTests> fixture) => _fixture = fixture;

    // The complete 151-name review snapshot from 10 November 2026. "-" means deliberately not a feature.
    private const string ReviewSnapshot = """
        .NET MAUI|Blazor Hybrid|Blazor Hybrid
        ASP.NET Core|Interactive Auto render mode|Interactive Auto rendering
        ASP.NET Core|Interactive Server render mode|Interactive Server rendering
        ASP.NET Core|Interactive WebAssembly render mode|Interactive WebAssembly rendering
        ASP.NET Core|Static Server rendering|Static server-side rendering
        Azure AI Services|AI threat protection|-
        Azure AI Speech|SIP|SIP
        Azure API Management|Managed identity|Managed identity authentication
        Azure API Management|Private networking|Private networking
        Azure App Service|Java 8, 11, and 17 support|-
        Azure Arc|Arc gateway|Arc gateway
        Azure Arc|Database migration|Database migration
        Azure Backup|Vaulted backup|Vaulted backup
        Azure Container Registry|Repository-level permissions|ABAC repository permissions
        Azure Databricks|External locations|External locations
        Azure Databricks|Managed storage|Managed storage
        Azure Databricks|Materialized views|Materialized views
        Azure Databricks|Streaming tables|Streaming tables
        Azure Front Door|Private Link|Private Link origins
        Azure Functions|Java 8, 11, and 17 support|-
        Azure Pipelines|Job containers|Job containers
        Azure Pipelines|Pipelines|-
        Azure Policy|IP address and CIDR expressions|IP address expressions
        Azure Policy|Policy overrides|Policy effect overrides
        Azure Policy|Request context identity|Identity-aware policy evaluation
        Azure Route Server|Downstream policy enforcement|Downstream policy enforcement
        Azure Route Server|Network isolation|Network isolation
        Azure Route Server|Path selection|Path selection
        Azure Route Server|Route filtering|Route filtering
        Azure Route Server|Route maps|Route maps
        Azure Route Server|Route summarization|Route summarization
        Azure Route Server|SD-WAN interoperability|SD-WAN interoperability
        Azure SDK|Authentication|Authentication
        Azure SDK|Azure Cosmos DB client for Rust|-
        Azure SQL Database|Auditing|Audit logging
        Azure SQL Database|Long-term retention|Long-term retention backups
        Azure SQL Managed Instance|Auditing|Audit logging
        Azure SQL Managed Instance|Long-term retention|Long-term retention backups
        Azure SRE Agent|Alerts|Alerts
        Azure SRE Agent|Live Reports|Live Reports
        Azure Skills|Azure Advisor|-
        Azure Skills|Azure Cost|-
        Azure Virtual Machines|Azure Cobalt 200|-
        CodeQL CLI|JVM diagnostics|JVM diagnostics
        CodeQL CLI|Pack publishing checks|Pack publishing checks
        CodeQL CLI|Python support|Python support
        ComfyUI|Custom nodes|Custom nodes
        Dev Proxy|Dev Proxy API|Dev Proxy API
        Dev Proxy|System proxy|System proxy
        Docker Desktop|WSL 2 integration|WSL 2 integration
        Foundry Local|Speculative decoding|Speculative decoding
        GitHub|Contributor exemptions|Contributor exemptions
        GitHub|Garbage collection|Garbage collection
        GitHub|Merge readiness summary|Merge readiness summary
        GitHub|Native Citation support|Native Citation support
        GitHub|Newest-first comment sorting|Newest-first comment sorting
        GitHub|ORCID ID integration|ORCID ID integration
        GitHub|Ref-CAS|Ref-CAS
        GitHub|Repository compaction|Repository compaction
        GitHub|Repository pull request views|Repository pull request views
        GitHub|Virtual replicas|Virtual replicas
        GitHub Actions|Ephemeral runners|Ephemeral runners
        GitHub Actions|Manually triggered workflows|Manual workflow runs
        GitHub CodeQL|Security queries|Security queries
        GitHub Copilot|AI-detected secret push protection|-
        GitHub Copilot|Agents|-
        GitHub Copilot|Assisted approvals|Assisted permissions
        GitHub Copilot|Canvas|GitHub Canvas
        GitHub Copilot|Conversation context|Conversation context
        GitHub Copilot|Enterprise managed settings|Enterprise managed settings
        GitHub Copilot|GPT-6.1 Terra|-
        GitHub Copilot|GitHub app for Microsoft Teams|@GitHub in Slack and Teams
        GitHub Copilot|Global default policy|Global default policy
        GitHub Copilot|Repository plugin defaults and enterprise-managed plugin controls|Plugin management
        GitHub Copilot|Security canvas|Copilot app Security canvas|GitHub Copilot app
        GitHub Copilot|Session persistence|Session continuity
        GitHub Copilot|Slack and Teams integration|@GitHub in Slack and Teams
        GitHub Copilot CLI|Copilot CLI hooks|Hooks
        GitHub Dependabot|Dependency updates|Version updates
        GitHub Enterprise|Cost centers|Cost centers|GitHub Enterprise Cloud
        Microsoft Azure|Run Command|Run Command|Azure Virtual Machines
        Microsoft Build of OpenJDK|Java 11 and 17 support|-
        Microsoft Copilot|Copilot Cowork|-
        Microsoft Defender|Defender Experts investigations|Defender Experts investigations|Microsoft Defender Experts
        Microsoft Defender|Local agent discovery|Local AI agent discovery
        Microsoft Defender|SQL Server agent|SQL Server agent|Microsoft Defender for Cloud
        Microsoft Execution Container|AI agent containment|Containment
        Microsoft Execution Container|Agentic process flagging|Agentic process flagging
        Microsoft Execution Container|Containment|Containment
        Microsoft Execution Container|Granular policies|Granular policies
        Microsoft Execution Container|Learn modes|Learn modes
        Microsoft Fabric|Automatic synchronization|Automatic synchronization
        Microsoft Fabric|Copilot|-
        Microsoft Fabric|Copilot capacity billing|Copilot capacity billing
        Microsoft Fabric|Copilot in Microsoft Fabric|Copilot in Microsoft Fabric
        Microsoft Fabric|Data agents|Data agent
        Microsoft Fabric|Data connection policies|Data connection policies
        Microsoft Fabric|Database Hub alerts for Azure SQL Database|Database Hub alerts
        Microsoft Fabric|Delta shortcuts|Delta shortcuts
        Microsoft Fabric|External locations|OneLake external-location support
        Microsoft Fabric|Fabric Policies|Fabric Policies
        Microsoft Fabric|Hosted MCP server|OneLake hosted MCP server
        Microsoft Fabric|Iceberg REST Catalog writes|Iceberg writes
        Microsoft Fabric|Iceberg shortcuts|Iceberg shortcuts
        Microsoft Fabric|Internal OneLake shortcuts|Internal shortcuts
        Microsoft Fabric|Materialized views|Materialized views
        Microsoft Fabric|OneLake File APIs|OneLake File APIs
        Microsoft Fabric|OneLake Table APIs|OneLake Table APIs
        Microsoft Fabric|Project Osmos|Data engineering agent
        Microsoft Fabric|Query Insights|Query Insights
        Microsoft Fabric|Salesforce Data 360 shortcuts|Salesforce Data 360 integration
        Microsoft Fabric|Shortcuts|OneLake shortcuts
        Microsoft Fabric|Streaming tables|Streaming tables
        Microsoft Foundry|Agents|-
        Microsoft Foundry|Cohere Rerank|-
        Microsoft Foundry|Foundry SDK|-
        Microsoft Foundry|GPT-6.1 Terra|-
        Microsoft Foundry|Keyless authentication|Microsoft Entra ID authentication
        Microsoft Foundry|MAI-Image-2.6|-
        Microsoft Foundry|MAI-Image-2.6-Flash|-
        Microsoft Foundry|Microsoft Foundry Skill|Microsoft Foundry Skill
        Microsoft Foundry|Model deployment|Model deployment
        Microsoft Foundry|Model deployments|Model deployment
        Microsoft Foundry|Sensitive-content protection for traces|Sensitive-content protection for traces
        Microsoft Foundry|Subagents|Native subagents
        Microsoft Foundry Agent Service|Agent identity|Agent identity
        Microsoft Foundry Toolkit|Foundry Toolkit|-
        Microsoft Power BI|DirectQuery|DirectQuery
        Microsoft Security Exposure Management|Secure Now|-
        Microsoft Windows|Hotpatch updates|Hotpatching
        Microsoft Windows Server|Hotpatch updates|Hotpatching
        Oracle AI Database@Azure|Azure Government|-
        Oracle AI Database@Azure|EU Data Boundary support|EU Data Boundary support
        Pgpool-II|Connection pooling|Connection pooling
        Pgpool-II|Health checks|Health checks
        Pgpool-II|Load balancing|Load balancing
        Pgpool-II|Pgpool Control Protocol|Pgpool Control Protocol
        Traefik|Hostname routing|Hostname routing
        Traefik|Middleware|Middleware
        Traefik|Path routing|Path routing
        Traefik|TLS termination|TLS termination
        Visual Studio Code|Add Context|Add Context
        Visual Studio Code|Agents Window|Agents Window
        Visual Studio Code|Canvases|Canvases
        Visual Studio Code|Chat view modes|Chat view modes
        Visual Studio Code|Local automations|Local automations
        Visual Studio Code|MCP servers|MCP server configuration
        Visual Studio Code|PostgreSQL extension|-
        Visual Studio Code|Remote browser support|Remote browser support
        Visual Studio Code|WSL extension|-
        Windows Subsystem for Linux|Windows Subsystem for Linux|-
        """;

    public static TheoryData<string, string, string?, string?> ReviewCases
    {
        get
        {
            var data = new TheoryData<string, string, string?, string?>();
            foreach (var line in ReviewSnapshot.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var fields = line.Split('|');
                var feature = fields[2] == "-" ? null : fields[2];
                data.Add(fields[0], fields[1], feature, fields.Length == 4 ? fields[3] : null);
            }

            return data;
        }
    }

    [Theory]
    [MemberData(nameof(ReviewCases))]
    [InlineData("PIX", "Crash postmortems", "Crash postmortems", null)]
    public async Task ResolveEventCatalog_BackfillSnapshot_LinksOnlyReviewedCapabilities(
        string product, string name, string? expectedFeature, string? expectedProduct)
    {
        var result = await _fixture.Connection.QuerySingleAsync<(string? Product, string? Feature)>(
            """
            SELECT p.name AS Product, f.name AS Feature
            FROM resolve_event_catalog(@Product, @Name) r
            LEFT JOIN products p ON p.id = r.out_product_id
            LEFT JOIN product_features f ON f.id = r.out_feature_id
            """,
            new { Product = product, Name = name });

        result.Feature.Should().Be(expectedFeature);
        if (expectedFeature is not null)
        {
            result.Product.Should().Be(expectedProduct ?? product);
        }
    }

    [Theory]
    [InlineData("Claude Code", null)]
    [InlineData("PIX", null)]
    [InlineData("dotcraft", null)]
    [InlineData("Microsoft Foundry SDK", "Microsoft Foundry")]
    [InlineData("PostgreSQL extension for Visual Studio Code", "Visual Studio Code")]
    [InlineData("WSL extension for Visual Studio Code", "Visual Studio Code")]
    [InlineData("Azure Advisor plugin", "Azure Skills")]
    [InlineData("Azure Cost plugin", "Azure Skills")]
    public async Task Seed_StandaloneTools_AreProductsWithTheCorrectParent(string name, string? parent)
    {
        var actual = await _fixture.Connection.QuerySingleAsync<(string Name, string? Parent)>(
            "SELECT p.name, parent.name AS Parent FROM products p LEFT JOIN products parent ON parent.id = p.parent_product_id WHERE p.name = @Name",
            new { Name = name });

        actual.Name.Should().Be(name);
        actual.Parent.Should().Be(parent);
    }

    [Theory]
    [InlineData("Copilot", null)]
    [InlineData("Azure Advisor", "Azure Advisor")]
    [InlineData("Azure Advisor plugin", "Azure Advisor plugin")]
    [InlineData("azure-advisor", "Azure Advisor")]
    [InlineData("Microsoft Azure Advisor", "Azure Advisor")]
    [InlineData("Private Link", "Azure Private Link")]
    [InlineData("Microsoft Azure Private Link", "Azure Private Link")]
    [InlineData("Azure Cost", "Azure Cost plugin")]
    [InlineData("Foundry SDK", "Microsoft Foundry SDK")]
    [InlineData("PostgreSQL extension", "PostgreSQL extension for Visual Studio Code")]
    [InlineData("WSL extension", "WSL extension for Visual Studio Code")]
    public async Task FindProductId_QualifiedToolAliases_DoNotHijackOtherProducts(string name, string? expected)
    {
        var product = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT name FROM products WHERE id = find_product_id(@Name)", new { Name = name });

        product.Should().Be(expected);
    }

    [Theory]
    [InlineData(".NET Conf")]
    [InlineData("MAI-Transcribe-2-Streaming")]
    [InlineData("MAI-Voice-2.1")]
    [InlineData("MAI-Voice-2.1-Flash")]
    public async Task Seed_ConferenceAndModels_AreIgnoredRatherThanProducts(string name)
    {
        (await _fixture.Connection.ExecuteScalarAsync<long>(
            "SELECT count(*) FROM ignored_product_names WHERE name = @Name", new { Name = name })).Should().Be(1);
        (await _fixture.Connection.ExecuteScalarAsync<long?>(
            "SELECT find_product_id(@Name)", new { Name = name })).Should().BeNull();
    }

    [Fact]
    public async Task GetUnmatchedSummary_BackfillSnapshot_LeavesOnlyExcludedNames()
    {
        var rows = ReviewSnapshot.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(line => line.Split('|')).ToArray();
        rows.Should().HaveCount(151);
        var metadata = JsonSerializer.Serialize(new
        {
            product_features = rows.Select(fields => new { product = fields[0], feature = fields[1] })
                .Append(new { product = "PIX", feature = "Crash postmortems" })
        });
        var names = JsonSerializer.Serialize(new[]
        {
            ".NET Conf", "Claude Code", "MAI-Transcribe-2-Streaming", "MAI-Voice-2.1",
            "MAI-Voice-2.1-Flash", "PIX", "dotcraft"
        });
        using var transaction = _fixture.Connection.BeginTransaction();
        await _fixture.Connection.ExecuteAsync(
            """
            DELETE FROM content_items;
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash, sections_bitmask, created_at, ai_metadata)
            VALUES ('backfill-review', 'news', 'Backfill review', '', '', 1794308400, 'ai',
                    'https://example.com/backfill-review', 'Author', 'Feed', '', 'hash', 1, NOW(), CAST(@Metadata AS jsonb));
            INSERT INTO content_item_products (collection_name, item_slug, product_name, position)
            SELECT 'news', 'backfill-review', name, ordinal::int
            FROM jsonb_array_elements_text(CAST(@Names AS jsonb)) WITH ORDINALITY AS n(name, ordinal);
            """,
            new { Metadata = metadata, Names = names }, transaction);

        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync(), transaction: transaction);

        var products = new ProductRepository(_fixture.Connection, NullLogger<ProductRepository>.Instance);
        (await products.GetUnmatchedNamesAsync(TestContext.Current.CancellationToken)).Should().BeEmpty();
        var features = new ProductFeatureRepository(_fixture.Connection, NullLogger<ProductFeatureRepository>.Instance);
        var unmatched = await features.GetUnmatchedSummaryAsync(TestContext.Current.CancellationToken);
        unmatched.Sum(p => p.NameCount).Should().Be(26);
        unmatched.Sum(p => p.EventCount).Should().Be(0, "all 151 names were replayed as undated article mentions");
    }

    [Fact]
    public async Task Migration_RunAgain_IsIdempotentAndPreservesCuratedData()
    {
        using var transaction = _fixture.Connection.BeginTransaction();
        await _fixture.Connection.ExecuteAsync(
            "UPDATE product_features SET description = 'Curated description' WHERE name = 'Canvases';",
            transaction: transaction);
        const string Counts = """
            SELECT (SELECT count(*) FROM products) + (SELECT count(*) FROM product_aliases)
                 + (SELECT count(*) FROM product_features) + (SELECT count(*) FROM product_feature_aliases)
                 + (SELECT count(*) FROM ignored_product_names)
            """;
        var before = await _fixture.Connection.ExecuteScalarAsync<long>(Counts, transaction: transaction);
        await _fixture.Connection.ExecuteAsync(await ReadMigrationAsync(), transaction: transaction);

        (await _fixture.Connection.ExecuteScalarAsync<long>(Counts, transaction: transaction)).Should().Be(before);
        (await _fixture.Connection.ExecuteScalarAsync<string>(
            "SELECT description FROM product_features WHERE name = 'Canvases'", transaction: transaction)).Should().Be("Curated description");
    }

    private static async Task<string> ReadMigrationAsync()
    {
        var assembly = typeof(ProductRepository).Assembly;
        var resource = assembly.GetManifestResourceNames().Single(n => n.EndsWith("056_backfill_catalog_seed.sql", StringComparison.Ordinal));
        await using var stream = assembly.GetManifestResourceStream(resource)!;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
