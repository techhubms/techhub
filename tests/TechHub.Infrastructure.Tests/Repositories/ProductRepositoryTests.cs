using System.Globalization;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="ProductRepository"/> and for the product links that article and timeline event
/// writes maintain. Every test uses unique product names and slugs, because the database is shared by the tests of
/// this class and already holds the starting catalog.
/// </summary>
public class ProductRepositoryTests : IClassFixture<DatabaseFixture<ProductRepositoryTests>>
{
    private readonly DatabaseFixture<ProductRepositoryTests> _fixture;
    private readonly ProductRepository _sut;
    private readonly ContentItemWriteRepository _writeRepository;
    private readonly TimelineEventRepository _timelineRepository;

    public ProductRepositoryTests(DatabaseFixture<ProductRepositoryTests> fixture)
    {
        _fixture = fixture;
        _sut = new ProductRepository(fixture.Connection, NullLogger<ProductRepository>.Instance);
        _writeRepository = new ContentItemWriteRepository(fixture.Connection, NullLogger<ContentItemWriteRepository>.Instance);
        _timelineRepository = new TimelineEventRepository(fixture.Connection);
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    // ── Starting catalog ──────────────────────────────────────────────────────

    [Fact]
    public async Task StartingCatalog_ContainsOfficialNamesWithTheirAliases()
    {
        var products = await _sut.GetProductsAsync("Microsoft Foundry", Ct);

        var foundry = products.Should().Contain(p => p.Name == "Microsoft Foundry").Subject;
        foundry.Aliases.Select(a => a.Alias).Should().Contain("Azure AI Foundry");
    }

    [Fact]
    public async Task StartingCatalog_IgnoresNamesThatAreNotProducts()
    {
        var ignored = await _sut.GetIgnoredNamesAsync(Ct);

        ignored.Should().Contain(["Microsoft Ignite", "Microsoft SQL", "Azure Accelerated Networking"]);
        ignored.Should().NotContain("Microsoft Azure");
    }

    [Theory]
    [InlineData("Microsoft Azure", "Azure")]
    [InlineData("GitHub Enterprise Local", null)]
    [InlineData("Agent Host Protocol", "AHP")]
    [InlineData("Azure SDK", null)]
    public async Task StartingCatalog_ContainsProductsThatAreNotAliasesOfAnotherProduct(string name, string? alias)
    {
        var products = await _sut.GetProductsAsync(name, Ct);

        var product = products.Should().Contain(p => p.Name == name).Subject;
        product.ParentName.Should().BeNull();
        if (alias is not null)
        {
            product.Aliases.Select(a => a.Alias).Should().Contain(alias);
        }
    }

    [Theory]
    [InlineData("Azure Identity")]
    [InlineData("Azure SDK for .NET")]
    [InlineData("Azure SDK for Python")]
    public async Task StartingCatalog_PutsAzureIdentityAndTheLanguageSdksUnderAzureSdk(string name)
    {
        var products = await _sut.GetProductsAsync(name, Ct);

        products.Should().Contain(p => p.Name == name).Which.ParentName.Should().Be("Azure SDK");
    }

    [Fact]
    public async Task StartingCatalog_DoesNotMapBareCopilotNamesToGitHubCopilot()
    {
        var products = await _sut.GetProductsAsync("GitHub Copilot", Ct);

        var copilot = products.Should().Contain(p => p.Name == "GitHub Copilot").Subject;
        var aliases = copilot.Aliases.Select(a => a.Alias).ToList();
        aliases.Should().Contain("GitHub Copilot Business");
        aliases.Should().NotContain(["Copilot Free", "Copilot Pro", "Copilot Pro+", "Copilot Business", "Copilot Enterprise"]);
    }

    // ── Links follow the AI output ────────────────────────────────────────────

    [Fact]
    public async Task UpdateAiMetadata_LinksProductNamesByNameAndAliasInClassifierOrder()
    {
        var unknown = UniqueName("Unknown");
        var slug = await SeedItemAsync();

        await WriteProductsAsync(slug, "Microsoft Foundry", "azure ai  foundry", unknown);

        var links = await GetLinksAsync(slug);
        links.Should().HaveCount(3);
        var foundryId = await FindProductIdAsync("Microsoft Foundry");
        links[0].Should().Be(("Microsoft Foundry", 0, foundryId));
        links[1].Should().Be(("azure ai foundry", 1, foundryId));
        links[2].Should().Be((unknown, 2, (long?)null));
    }

    [Fact]
    public async Task UpdateAiMetadata_ListsEachNameOnceIgnoringCase()
    {
        var name = UniqueName("Dup");
        var slug = await SeedItemAsync();

        await WriteProductsAsync(slug, name, name.ToUpperInvariant(), " ", "");

        var links = await GetLinksAsync(slug);
        links.Should().ContainSingle().Which.Name.Should().Be(name);
    }

    [Fact]
    public async Task UpdateAiMetadata_AgainReplacesTheLinks()
    {
        var first = UniqueName("First");
        var second = UniqueName("Second");
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, first);

        await WriteProductsAsync(slug, second);

        var links = await GetLinksAsync(slug);
        links.Should().ContainSingle().Which.Name.Should().Be(second);
    }

    [Fact]
    public async Task UpdateAiMetadata_WithSchemaVersion1Metadata_LeavesTheItemWithoutLinks()
    {
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, UniqueName("Old"));

        await _writeRepository.UpdateAiMetadataAsync("news", slug, """{"roundup_summary":"x","key_topics":["a"]}""", Ct);

        (await GetLinksAsync(slug)).Should().BeEmpty();
    }

    [Fact]
    public async Task ReplaceEvents_LinksTheEventToTheProductByNameOrAlias()
    {
        var unknown = UniqueName("EventUnknown");
        var slug = await SeedItemAsync();

        await _timelineRepository.ReplaceEventsAsync("news", slug,
            [Event("Azure AI Foundry"), Event(unknown)], "source", 2, Ct);

        var events = await GetEventLinksAsync(slug);
        events.Should().HaveCount(2);
        var foundryId = await FindProductIdAsync("Microsoft Foundry");
        events.Should().Contain(("Azure AI Foundry", foundryId));
        events.Should().Contain((unknown, null));
    }

    // ── Catalog changes relink ────────────────────────────────────────────────

    [Fact]
    public async Task Create_LinksTheArticlesAndEventsThatUseTheName()
    {
        var name = UniqueName("NewProduct");
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, name);
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(name)], "source", 2, Ct);

        var result = await _sut.CreateAsync(name.ToUpperInvariant(), Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        (await GetLinksAsync(slug)).Should().ContainSingle().Which.ProductId.Should().Be(result.ProductId);
        (await GetEventLinksAsync(slug)).Should().ContainSingle().Which.ProductId.Should().Be(result.ProductId);
        (await _sut.GetUnmatchedNamesAsync(Ct)).Should().NotContain(n => n.Name == name);
    }

    [Fact]
    public async Task AddAlias_LinksNamesThatUseTheAlias_AndRemoveAliasUnlinksThem()
    {
        var product = await CreateProductAsync();
        var alias = UniqueName("Alias");
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, alias);
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(alias)], "source", 2, Ct);

        var added = await _sut.AddAliasAsync(product.Id, alias, Ct);

        added.Succeeded.Should().BeTrue(added.Message);
        (await GetLinksAsync(slug)).Single().ProductId.Should().Be(product.Id);
        (await GetEventLinksAsync(slug)).Single().ProductId.Should().Be(product.Id);

        var aliasId = (await _sut.GetDetailAsync(product.Id, Ct))!.Aliases.Single().Id;
        var removed = await _sut.RemoveAliasAsync(product.Id, aliasId, Ct);

        removed.Succeeded.Should().BeTrue(removed.Message);
        (await GetLinksAsync(slug)).Single().ProductId.Should().BeNull();
        (await GetEventLinksAsync(slug)).Single().ProductId.Should().BeNull();
    }

    [Fact]
    public async Task Rename_KeepsTheOldNameAsAliasSoLinksStay()
    {
        var product = await CreateProductAsync();
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, product.Name);
        var newName = UniqueName("Renamed");

        var result = await _sut.RenameAsync(product.Id, newName, Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        var detail = (await _sut.GetDetailAsync(product.Id, Ct))!;
        detail.Name.Should().Be(newName);
        detail.Aliases.Select(a => a.Alias).Should().Equal(product.Name);
        detail.Articles.Should().ContainSingle().Which.ProductName.Should().Be(product.Name);
    }

    [Fact]
    public async Task Rename_ToCaseVariantOfTheSameName_OnlyChangesTheName()
    {
        var product = await CreateProductAsync();

        var result = await _sut.RenameAsync(product.Id, product.Name.ToUpperInvariant(), Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        var detail = (await _sut.GetDetailAsync(product.Id, Ct))!;
        detail.Name.Should().Be(product.Name.ToUpperInvariant());
        detail.Aliases.Should().BeEmpty();
    }

    [Fact]
    public async Task Rename_ToAnAliasOfTheSameProduct_PromotesTheAlias()
    {
        var product = await CreateProductAsync();
        var alias = UniqueName("Promote");
        (await _sut.AddAliasAsync(product.Id, alias, Ct)).Succeeded.Should().BeTrue();

        var result = await _sut.RenameAsync(product.Id, alias, Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        var detail = (await _sut.GetDetailAsync(product.Id, Ct))!;
        detail.Name.Should().Be(alias);
        detail.Aliases.Select(a => a.Alias).Should().Equal(product.Name);
    }

    [Fact]
    public async Task Merge_MovesArticlesEventsAndAliasesToTheTarget()
    {
        var source = await CreateProductAsync();
        var target = await CreateProductAsync();
        var sourceAlias = UniqueName("SourceAlias");
        (await _sut.AddAliasAsync(source.Id, sourceAlias, Ct)).Succeeded.Should().BeTrue();
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, source.Name);
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(sourceAlias)], "source", 2, Ct);

        var result = await _sut.MergeAsync(source.Id, target.Id, Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        (await _sut.GetDetailAsync(source.Id, Ct)).Should().BeNull();
        var detail = (await _sut.GetDetailAsync(target.Id, Ct))!;
        detail.Aliases.Select(a => a.Alias).Should().BeEquivalentTo(source.Name, sourceAlias);
        detail.Articles.Should().ContainSingle(a => a.Slug == slug);
        detail.Events.Should().ContainSingle(e => e.ItemSlug == slug);
    }

    [Fact]
    public async Task Merge_IntoItself_IsInvalid()
    {
        var product = await CreateProductAsync();

        var result = await _sut.MergeAsync(product.Id, product.Id, Ct);

        result.Status.Should().Be(ProductOperationStatus.Invalid);
    }

    [Fact]
    public async Task Merge_WithUnknownProduct_IsNotFound()
    {
        var product = await CreateProductAsync();

        var result = await _sut.MergeAsync(product.Id, long.MaxValue, Ct);

        result.Status.Should().Be(ProductOperationStatus.NotFound);
    }

    [Fact]
    public async Task Delete_UnlinksArticlesAndEvents_AndTheNameIsUnmatchedAgain()
    {
        var product = await CreateProductAsync();
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, product.Name);
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(product.Name)], "source", 2, Ct);

        var result = await _sut.DeleteAsync(product.Id, Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        (await GetLinksAsync(slug)).Single().ProductId.Should().BeNull();
        (await GetEventLinksAsync(slug)).Single().ProductId.Should().BeNull();
        (await _sut.GetUnmatchedNamesAsync(Ct)).Should().Contain(n => n.Name == product.Name);
    }

    [Fact]
    public async Task Delete_UnknownProduct_IsNotFound()
    {
        var result = await _sut.DeleteAsync(long.MaxValue, Ct);

        result.Status.Should().Be(ProductOperationStatus.NotFound);
    }

    // ── Names that are not allowed ────────────────────────────────────────────

    [Fact]
    public async Task Create_WithNameOfAnExistingProductIgnoringCase_IsAConflict()
    {
        var product = await CreateProductAsync();

        var result = await _sut.CreateAsync($"  {product.Name.ToUpperInvariant()}  ", Ct);

        result.Status.Should().Be(ProductOperationStatus.Conflict);
    }

    [Fact]
    public async Task Create_WithAnAliasOfAnotherProduct_IsAConflict()
    {
        var result = await _sut.CreateAsync("azure ai foundry", Ct);

        result.Status.Should().Be(ProductOperationStatus.Conflict);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_WithEmptyName_IsInvalid(string name)
    {
        var result = await _sut.CreateAsync(name, Ct);

        result.Status.Should().Be(ProductOperationStatus.Invalid);
    }

    [Fact]
    public async Task Create_WithTooLongName_IsInvalid()
    {
        var result = await _sut.CreateAsync(new string('x', ProductRepository.MaxNameLength + 1), Ct);

        result.Status.Should().Be(ProductOperationStatus.Invalid);
    }

    [Fact]
    public async Task AddAlias_WithNameOfAnotherProduct_IsAConflict()
    {
        var first = await CreateProductAsync();
        var second = await CreateProductAsync();

        var result = await _sut.AddAliasAsync(second.Id, first.Name, Ct);

        result.Status.Should().Be(ProductOperationStatus.Conflict);
    }

    [Fact]
    public async Task AddAlias_ToUnknownProduct_IsNotFound()
    {
        var result = await _sut.AddAliasAsync(long.MaxValue, UniqueName("Alias"), Ct);

        result.Status.Should().Be(ProductOperationStatus.NotFound);
    }

    [Fact]
    public async Task Rename_ToNameOfAnotherProduct_IsAConflict()
    {
        var first = await CreateProductAsync();
        var second = await CreateProductAsync();

        var result = await _sut.RenameAsync(second.Id, first.Name, Ct);

        result.Status.Should().Be(ProductOperationStatus.Conflict);
    }

    [Fact]
    public async Task RemoveAlias_OfAnotherProduct_IsNotFound()
    {
        var first = await CreateProductAsync();
        var second = await CreateProductAsync();
        (await _sut.AddAliasAsync(first.Id, UniqueName("Alias"), Ct)).Succeeded.Should().BeTrue();
        var aliasId = (await _sut.GetDetailAsync(first.Id, Ct))!.Aliases.Single().Id;

        var result = await _sut.RemoveAliasAsync(second.Id, aliasId, Ct);

        result.Status.Should().Be(ProductOperationStatus.NotFound);
    }

    // ── Review queue ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetUnmatchedNames_CountsDistinctArticlesAndEvents_MostUsedFirst()
    {
        var common = UniqueName("Common");
        var rare = UniqueName("Rare");
        var first = await SeedItemAsync();
        var second = await SeedItemAsync();
        await WriteProductsAsync(first, common, rare);
        await WriteProductsAsync(second, common);
        await _timelineRepository.ReplaceEventsAsync("news", first, [Event(common), Event(common)], "source", 2, Ct);

        var unmatched = await _sut.GetUnmatchedNamesAsync(Ct);

        var commonRow = unmatched.Should().ContainSingle(n => n.Name == common).Subject;
        commonRow.ItemCount.Should().Be(2);
        commonRow.EventCount.Should().Be(2);
        unmatched.Single(n => n.Name == rare).ItemCount.Should().Be(1);
        unmatched.ToList().FindIndex(n => n.Name == common).Should().BeLessThan(unmatched.ToList().FindIndex(n => n.Name == rare));
    }

    [Fact]
    public async Task IgnoreName_RemovesItFromTheReviewQueue_AndUnignorePutsItBack()
    {
        var name = UniqueName("NotAProduct");
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, name);

        (await _sut.IgnoreNameAsync(name.ToLowerInvariant(), Ct)).Succeeded.Should().BeTrue();

        (await _sut.GetUnmatchedNamesAsync(Ct)).Should().NotContain(n => n.Name == name);
        (await _sut.GetIgnoredNamesAsync(Ct)).Should().Contain(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase));

        (await _sut.UnignoreNameAsync(name, Ct)).Succeeded.Should().BeTrue();

        (await _sut.GetUnmatchedNamesAsync(Ct)).Should().Contain(n => n.Name == name);
    }

    [Fact]
    public async Task IgnoreName_OfAProduct_IsAConflict()
    {
        var product = await CreateProductAsync();

        var result = await _sut.IgnoreNameAsync(product.Name, Ct);

        result.Status.Should().Be(ProductOperationStatus.Conflict);
    }

    [Fact]
    public async Task Unignore_OfNameThatIsNotIgnored_IsNotFound()
    {
        var result = await _sut.UnignoreNameAsync(UniqueName("Never"), Ct);

        result.Status.Should().Be(ProductOperationStatus.NotFound);
    }

    [Fact]
    public async Task Create_ForAnIgnoredName_RemovesItFromTheIgnoredNames()
    {
        var name = UniqueName("Changed");
        (await _sut.IgnoreNameAsync(name, Ct)).Succeeded.Should().BeTrue();

        (await _sut.CreateAsync(name, Ct)).Succeeded.Should().BeTrue();

        (await _sut.GetIgnoredNamesAsync(Ct)).Should().NotContain(name);
    }

    // ── Detail and list ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetDetail_ReturnsLinkedArticlesNewestFirstAndEventsByDateWithTheirArticle()
    {
        var product = await CreateProductAsync();
        var older = await SeedItemAsync(title: "Older article", dateEpoch: 1_700_000_000);
        var newer = await SeedItemAsync(title: "Newer article", dateEpoch: 1_800_000_000);
        await WriteProductsAsync(older, product.Name);
        await WriteProductsAsync(newer, product.Name);
        await _timelineRepository.ReplaceEventsAsync("news", older,
            [Event(product.Name, kind: "retirement", date: "2027-06-01")], "source", 2, Ct);
        await _timelineRepository.ReplaceEventsAsync("news", newer,
            [Event(product.Name, kind: "ga", date: "2026-03-10")], "source", 2, Ct);

        var detail = (await _sut.GetDetailAsync(product.Id, Ct))!;

        detail.Articles.Select(a => a.Title).Should().Equal("Newer article", "Older article");
        detail.Events.Select(e => (e.Kind, e.ItemTitle)).Should().Equal(("ga", "Newer article"), ("retirement", "Older article"));
        detail.Events[0].EventDate.Should().Be(new DateOnly(2026, 3, 10));
    }

    [Fact]
    public async Task GetDetail_UnknownProduct_ReturnsNull()
    {
        (await _sut.GetDetailAsync(long.MaxValue, Ct)).Should().BeNull();
    }

    [Fact]
    public async Task GetProducts_CountsArticlesOncePerProductAndFiltersOnNameOrAlias()
    {
        var product = await CreateProductAsync();
        var alias = UniqueName("Searchable");
        (await _sut.AddAliasAsync(product.Id, alias, Ct)).Succeeded.Should().BeTrue();
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, product.Name, alias);

        var byAlias = await _sut.GetProductsAsync(alias.ToLowerInvariant(), Ct);

        var found = byAlias.Should().ContainSingle().Subject;
        found.Id.Should().Be(product.Id);
        found.ItemCount.Should().Be(1);
        found.Aliases.Select(a => a.Alias).Should().Equal(alias);
    }

    [Fact]
    public async Task GetProducts_SearchTreatsWildcardsAsText()
    {
        var product = await CreateProductAsync();

        var result = await _sut.GetProductsAsync("%", Ct);

        result.Should().NotContain(p => p.Id == product.Id);
    }

    // ── Roundup reads the catalog ─────────────────────────────────────────────

    [Fact]
    public async Task RoundupArticles_UseTheCanonicalProductNames()
    {
        var slug = await SeedItemAsync(createdAt: new DateTime(2025, 3, 25, 10, 0, 0, DateTimeKind.Utc), sectionBitmask: 2);
        await WriteProductsAsync(slug, "Azure AI Foundry", UniqueName("Unmatched"));
        var roundups = new SectionRoundupRepository(_fixture.Connection, NullLogger<SectionRoundupRepository>.Instance);

        var articles = await roundups.GetArticlesForSectionForWeekAsync("azure", new DateOnly(2025, 3, 24), new DateOnly(2025, 3, 30), Ct);

        var article = articles.Should().ContainSingle(a => a.Slug == slug).Subject;
        article.Products[0].Should().Be("Microsoft Foundry");
        article.Products.Should().HaveCount(2);
    }

    [Fact]
    public async Task RoundupDateEvents_UseTheCanonicalProductName()
    {
        var slug = await SeedItemAsync(createdAt: new DateTime(2025, 3, 26, 10, 0, 0, DateTimeKind.Utc), sectionBitmask: 2);
        await _timelineRepository.ReplaceEventsAsync("news", slug,
            [Event("Azure AI Foundry", kind: "retirement", date: "2027-06-01")], "source", 2, Ct);
        var roundups = new SectionRoundupRepository(_fixture.Connection, NullLogger<SectionRoundupRepository>.Instance);

        var events = await roundups.GetUpcomingDateEventsAsync("azure", new DateOnly(2025, 3, 24), new DateOnly(2025, 3, 30), Ct);

        events.Should().Contain(e => e.Product == "Microsoft Foundry" && e.Date == new DateOnly(2027, 6, 1));
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string UniqueName(string prefix) => $"{prefix}-{Guid.NewGuid():N}";

    private static TimelineEvent Event(string product, string kind = "ga", string date = "2026-03-10") => new()
    {
        Kind = kind,
        Product = product,
        Feature = null,
        Description = $"{kind} of {product}",
        Date = DateOnly.Parse(date, CultureInfo.InvariantCulture),
        DatePrecision = "day",
        DateSource = "explicit",
        Quote = null
    };

    private async Task<string> SeedItemAsync(string title = "Product test", long dateEpoch = 1743278400, DateTime? createdAt = null, int sectionBitmask = 1)
    {
        var slug = $"product-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash, sections_bitmask, created_at)
            VALUES (@Slug, 'news', @Title, '', '', @DateEpoch, 'ai',
                    @Url, 'Author', 'Feed', '', 'hash', @Bitmask, COALESCE(@CreatedAt, NOW()))
            """,
            new { Slug = slug, Title = title, DateEpoch = dateEpoch, Url = $"https://example.com/{slug}", Bitmask = sectionBitmask, CreatedAt = createdAt });
        return slug;
    }

    private Task WriteProductsAsync(string slug, params string[] products) =>
        _writeRepository.UpdateAiMetadataAsync(
            "news",
            slug,
            ContentItemWriteRepository.SerializeAiMetadata(new RoundupMetadata { SchemaVersion = 2, Products = products }),
            Ct);

    private async Task<ProductSummary> CreateProductAsync()
    {
        var name = UniqueName("Product");
        var result = await _sut.CreateAsync(name, Ct);
        result.Succeeded.Should().BeTrue(result.Message);
        return new ProductSummary { Id = result.ProductId!.Value, Name = name };
    }

    private async Task<long?> FindProductIdAsync(string name) =>
        await _fixture.Connection.ExecuteScalarAsync<long?>("SELECT find_product_id(@Name)", new { Name = name });

    private async Task<List<(string Name, int Position, long? ProductId)>> GetLinksAsync(string slug) =>
        (await _fixture.Connection.QueryAsync<(string Name, int Position, long? ProductId)>(
            """
            SELECT product_name AS Name, position AS Position, product_id AS ProductId
            FROM content_item_products
            WHERE collection_name = 'news' AND item_slug = @Slug
            ORDER BY position
            """,
            new { Slug = slug })).ToList();

    private async Task<List<(string Name, long? ProductId)>> GetEventLinksAsync(string slug) =>
        (await _fixture.Connection.QueryAsync<(string Name, long? ProductId)>(
            "SELECT product AS Name, product_id AS ProductId FROM content_timeline_events WHERE collection_name = 'news' AND item_slug = @Slug ORDER BY id",
            new { Slug = slug })).ToList();
}
