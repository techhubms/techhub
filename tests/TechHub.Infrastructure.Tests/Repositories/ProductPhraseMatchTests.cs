using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for migration 063: the phrase matching of product names and of the product and feature text of events.
/// The tests use products of the seeded catalog for the realistic cases and products of their own for the rules, so the rules are
/// shown to work for any name and not for the names of today.
/// </summary>
public class ProductPhraseMatchTests : IClassFixture<DatabaseFixture<ProductPhraseMatchTests>>
{
    private readonly DatabaseFixture<ProductPhraseMatchTests> _fixture;
    private readonly ProductRepository _products;

    public ProductPhraseMatchTests(DatabaseFixture<ProductPhraseMatchTests> fixture)
    {
        _fixture = fixture;
        _products = new ProductRepository(fixture.Connection, NullLogger<ProductRepository>.Instance);
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    private async Task<string?> MatchProductAsync(string name)
    {
        var id = await _fixture.Connection.ExecuteScalarAsync<long?>("SELECT find_product_id(@Name)", new { Name = name });
        return id is null ? null : await _fixture.Connection.ExecuteScalarAsync<string>("SELECT name FROM products WHERE id = @Id", new { Id = id });
    }

    private async Task<(string? Product, string? Feature)> ResolveEventAsync(string product, string? feature)
    {
        var row = await _fixture.Connection.QuerySingleAsync<(long? ProductId, long? FeatureId)>(
            "SELECT out_product_id AS ProductId, out_feature_id AS FeatureId FROM resolve_event_catalog(@Product, @Feature)",
            new { Product = product, Feature = feature });

        return (
            row.ProductId is null ? null : await _fixture.Connection.ExecuteScalarAsync<string>("SELECT name FROM products WHERE id = @Id", new { Id = row.ProductId }),
            row.FeatureId is null ? null : await _fixture.Connection.ExecuteScalarAsync<string>("SELECT name FROM product_features WHERE id = @Id", new { Id = row.FeatureId }));
    }

    private async Task<long> AddProductAsync(string name, long? parentId = null, params string[] aliases)
    {
        var id = await _fixture.Connection.ExecuteScalarAsync<long>(
            "INSERT INTO products (name, parent_product_id) VALUES (@Name, @ParentId) RETURNING id", new { Name = name, ParentId = parentId });
        foreach (var alias in aliases)
        {
            await _fixture.Connection.ExecuteAsync("INSERT INTO product_aliases (product_id, alias) VALUES (@Id, @Alias)", new { Id = id, Alias = alias });
        }

        return id;
    }

    private async Task<long> AddFeatureAsync(long productId, string name, params string[] aliases)
    {
        var id = await _fixture.Connection.ExecuteScalarAsync<long>(
            "INSERT INTO product_features (product_id, name) VALUES (@ProductId, @Name) RETURNING id", new { ProductId = productId, Name = name });
        foreach (var alias in aliases)
        {
            await _fixture.Connection.ExecuteAsync("INSERT INTO product_feature_aliases (feature_id, alias) VALUES (@Id, @Alias)", new { Id = id, Alias = alias });
        }

        return id;
    }

    // ── Products: words missing ───────────────────────────────────────────────

    [Theory]
    [InlineData("Dependabot", "GitHub Dependabot")]
    [InlineData("Surface Laptop Ultra", "Microsoft Surface Laptop Ultra")]
    [InlineData("Intelligent Terminal", "Microsoft Intelligent Terminal")]
    [InlineData("On-premises data gateway", "Microsoft on-premises data gateway")]
    public async Task FindProductId_NameWithoutTheVendorPrefix_LinksToTheOnlyProductWithThatPhrase(string aiName, string expected)
    {
        (await MatchProductAsync(aiName)).Should().Be(expected);
    }

    [Fact]
    public async Task FindProductId_MissingWords_WorkForAProductThatIsAddedLater()
    {
        await AddProductAsync("Contoso Quantum Gateway");

        (await MatchProductAsync("Quantum Gateway")).Should().Be("Contoso Quantum Gateway");
    }

    [Fact]
    public async Task FindProductId_NameThatIsOnlyTheStartOfALongerName_StaysUnmatched()
    {
        await AddProductAsync("Contoso Zephyr Ledger Tools");

        (await MatchProductAsync("Zephyr Ledger")).Should().BeNull("the words are in the middle of the name, not the end without the vendor");
        (await MatchProductAsync("Contoso Zephyr")).Should().BeNull("the name is only the start of a longer name");
    }

    [Theory]
    [InlineData("Copilot")]
    [InlineData("Server")]
    [InlineData("Studio")]
    [InlineData("Gateway")]
    public async Task FindProductId_WordThatFitsSeveralUnrelatedProducts_StaysUnmatched(string aiName)
    {
        (await MatchProductAsync(aiName)).Should().BeNull();
    }

    [Theory]
    [InlineData("Fabric", "Microsoft Fabric")]
    [InlineData("Defender", "Microsoft Defender")]
    public async Task FindProductId_WordThatFitsAProductAndItsChildren_LinksToTheParent(string aiName, string expected)
    {
        (await MatchProductAsync(aiName)).Should().Be(expected);
    }

    [Fact]
    public async Task FindProductId_WordThatFitsTwoSiblings_StaysUnmatched()
    {
        var parent = await AddProductAsync("Contoso Cloud");
        await AddProductAsync("Contoso Cloud Alpha Pipeline", parent);
        await AddProductAsync("Contoso Cloud Beta Pipeline", parent);

        (await MatchProductAsync("Pipeline")).Should().BeNull("the name fits two products and neither is the parent of the other");
    }

    // ── Products: release versions ────────────────────────────────────────────

    [Theory]
    [InlineData("Windows Server 2022", "Microsoft Windows Server")]
    [InlineData(".NET 10", ".NET")]
    [InlineData("Visual Studio 2026", "Visual Studio")]
    public async Task FindProductId_NameWithAReleaseVersion_LinksToTheProduct(string aiName, string expected)
    {
        (await MatchProductAsync(aiName)).Should().Be(expected);
    }

    [Fact]
    public async Task FindProductId_ANumberThatIsPartOfAProductName_StillMatchesExactly()
    {
        await AddProductAsync("Contoso Office 365X");

        (await MatchProductAsync("Contoso Office 365X")).Should().Be("Contoso Office 365X");
    }

    // ── Products: extra words ─────────────────────────────────────────────────

    [Fact]
    public async Task FindProductId_ExtraWordsAroundAKnownProduct_LinkToIt()
    {
        (await MatchProductAsync("Microsoft Fabric Eventstream")).Should().Be("Microsoft Fabric");
    }

    [Fact]
    public async Task FindProductId_ExtraWordsThatMakeTheKnownProductLessThanMostOfTheName_StayUnmatched()
    {
        (await MatchProductAsync("Microsoft Azure Widgetron Orchestrator")).Should().BeNull("the known part covers only half of the name");
    }

    [Fact]
    public async Task FindProductId_ExactNameBeatsAPhrase()
    {
        await AddProductAsync("Contoso Gadget Studio");
        await AddProductAsync("Gadget Studio");

        (await MatchProductAsync("Gadget Studio")).Should().Be("Gadget Studio");
    }

    // ── Events ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task ResolveEventCatalog_FeatureTextThatIsAChildProduct_BecomesAnEventOfThatProduct()
    {
        var parent = await AddProductAsync("Contoso Copilot");
        await AddProductAsync("Contoso Copilot Fusion", parent);

        var (product, feature) = await ResolveEventAsync("Contoso Copilot", "Copilot Fusion");

        product.Should().Be("Contoso Copilot Fusion");
        feature.Should().BeNull();
    }

    [Fact]
    public async Task ResolveEventCatalog_FeatureTextWithWordsAroundAFeature_LinksToTheFeature()
    {
        var product = await AddProductAsync("Contoso Studio Pro");
        await AddFeatureAsync(product, "Smart Chat");

        var (resolvedProduct, feature) = await ResolveEventAsync("Contoso Studio Pro", "Smart Chat experience");

        resolvedProduct.Should().Be("Contoso Studio Pro");
        feature.Should().Be("Smart Chat");
    }

    [Fact]
    public async Task ResolveEventCatalog_FeatureThatIsAnAliasInsideALongerText_LinksToTheFeature()
    {
        var product = await AddProductAsync("Contoso Billing Suite");
        await AddFeatureAsync(product, "Usage credits", "Billing credits");

        var (_, feature) = await ResolveEventAsync("Contoso Billing Suite", "Enterprise access through billing credits");

        feature.Should().Be("Usage credits");
    }

    [Fact]
    public async Task ResolveEventCatalog_FeatureOfTheParentProduct_AppliesToAChildProduct()
    {
        var parent = await AddProductAsync("Contoso Platform");
        await AddProductAsync("Contoso Platform Runner", parent);
        await AddFeatureAsync(parent, "Remote caching");

        var (product, feature) = await ResolveEventAsync("Contoso Platform Runner", "Remote caching improvements");

        product.Should().Be("Contoso Platform Runner");
        feature.Should().Be("Remote caching");
    }

    [Fact]
    public async Task ResolveEventCatalog_FeatureInAChildProduct_MovesTheEventToTheChildProduct()
    {
        var parent = await AddProductAsync("Contoso Data");
        var child = await AddProductAsync("Contoso Data Warehouse", parent);
        await AddFeatureAsync(child, "Result caching");

        var (product, feature) = await ResolveEventAsync("Contoso Data", "Data Warehouse result caching");

        product.Should().Be("Contoso Data Warehouse");
        feature.Should().Be("Result caching");
    }

    [Fact]
    public async Task ResolveEventCatalog_ProductNameAndFeatureWordsInDifferentPlaces_AreNotAPhrase()
    {
        var parent = await AddProductAsync("Contoso Foundry");
        await AddProductAsync("Contoso Foundry Agent Service", parent);

        var (product, feature) = await ResolveEventAsync("Contoso Foundry", "Fabric data agent custom-tool integration");

        product.Should().Be("Contoso Foundry", "'foundry' and 'agent' are not next to each other in the text");
        feature.Should().BeNull();
    }

    [Fact]
    public async Task ResolveEventCatalog_ASingleWordAliasInALongText_IsTooLooseToLink()
    {
        var product = await AddProductAsync("Contoso Keys");
        await AddFeatureAsync(product, "Bring your own key", "BYOK");

        var (_, feature) = await ResolveEventAsync("Contoso Keys", "BYOK and custom model reporting");

        feature.Should().BeNull("one word covers one fifth of the feature text");
    }

    [Fact]
    public async Task ResolveEventCatalog_ASingleWordFeatureThatIsMostOfTheText_Links()
    {
        var product = await AddProductAsync("Contoso Notes");
        await AddFeatureAsync(product, "Topics");

        var (_, feature) = await ResolveEventAsync("Contoso Notes", "Topics improved");

        feature.Should().Be("Topics");
    }

    [Fact]
    public async Task ResolveEventCatalog_TwoFeaturesOfTheSameLength_AreAmbiguous()
    {
        var product = await AddProductAsync("Contoso Modes");
        await AddFeatureAsync(product, "Alpha mode");
        await AddFeatureAsync(product, "Beta mode");

        var (_, feature) = await ResolveEventAsync("Contoso Modes", "Alpha mode and beta mode together");

        feature.Should().BeNull();
    }

    [Fact]
    public async Task ResolveEventCatalog_ALongerFeatureNameWinsOverAShorterOne()
    {
        var product = await AddProductAsync("Contoso Review");
        await AddFeatureAsync(product, "Review comments");
        await AddFeatureAsync(product, "Review comments summary");

        var (_, feature) = await ResolveEventAsync("Contoso Review", "Review comments summary is new");

        feature.Should().Be("Review comments summary");
    }

    [Fact]
    public async Task ResolveEventCatalog_ExactFeatureNameWinsOverAPhrase()
    {
        var product = await AddProductAsync("Contoso Exact");
        await AddFeatureAsync(product, "Fast mode");
        await AddFeatureAsync(product, "Fast mode with caching");

        var (_, feature) = await ResolveEventAsync("Contoso Exact", "Fast mode");

        feature.Should().Be("Fast mode");
    }

    [Fact]
    public async Task ResolveEventCatalog_WithoutAFeatureText_KeepsTheProduct()
    {
        var (product, feature) = await ResolveEventAsync("GitHub Copilot", null);

        product.Should().Be("GitHub Copilot");
        feature.Should().BeNull();
    }

    [Fact]
    public async Task ResolveEventCatalog_PhraseThatComesFromTheProductNameOnly_DoesNotLinkAFeature()
    {
        var product = await AddProductAsync("Contoso Pilot Console");
        await AddFeatureAsync(product, "Pilot Console");

        var (_, feature) = await ResolveEventAsync("Contoso Pilot Console", "Launch");

        feature.Should().BeNull("the phrase comes from the product name, not from the feature text");
    }

    [Fact]
    public async Task RelinkTimelineEvents_StoresTheResolvedProductAndKeepsItAfterACatalogChange()
    {
        var parent = await AddProductAsync("Contoso Relink");
        var child = await AddProductAsync("Contoso Relink Fusion", parent);
        var slug = $"phrase-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync(
            """
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, 'news', 'Fusion', '', '', 1743278400, 'ai', @Url, 'Author', 'Feed', '', 'hash')
            """,
            new { Slug = slug, Url = $"https://example.com/{slug}" });

        await new TimelineEventRepository(_fixture.Connection).ReplaceEventsAsync(
            "news",
            slug,
            [new TimelineEvent { Kind = "ga", Product = "Contoso Relink", Feature = "Relink Fusion", Description = "GA", Date = new DateOnly(2026, 3, 10), DatePrecision = "day", DateSource = "explicit" }],
            ITimelineEventRepository.ExtractedFromSource,
            ITimelineEventRepository.CurrentPromptVersion,
            Ct);

        var row = await _fixture.Connection.QuerySingleAsync<(long? ProductId, long? FeatureId)>(
            "SELECT product_id AS ProductId, feature_id AS FeatureId FROM content_timeline_events WHERE collection_name = 'news' AND item_slug = @Slug", new { Slug = slug });
        row.ProductId.Should().Be(child);
        row.FeatureId.Should().BeNull();

        // A catalog change relinks the existing events; the old name stays as an alias, so the event keeps its product.
        (await _products.RenameAsync(child, "Contoso Relink Fusion Pro", Ct)).Succeeded.Should().BeTrue();
        (await _fixture.Connection.ExecuteScalarAsync<long?>(
            "SELECT product_id FROM content_timeline_events WHERE collection_name = 'news' AND item_slug = @Slug", new { Slug = slug })).Should().Be(child);
    }

    // ── Seeds ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Migration063_AddsTheProductsTheAiNamedAndRenamesCostManagement()
    {
        var all = await _products.GetProductsAsync(null, Ct);

        all.Single(p => p.Name == "Azure Skills").ParentName.Should().Be("Microsoft Azure");
        all.Should().Contain(p => p.Name == "JetBrains Air").And.Contain(p => p.Name == "KEDA");
        all.Should().Contain(p => p.Name == "Microsoft Cost Management").And.NotContain(p => p.Name == "Azure Cost Management");
        (await MatchProductAsync("Azure Cost Management")).Should().Be("Microsoft Cost Management", "the old name is an alias");
    }
}
