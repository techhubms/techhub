using System.Globalization;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for the vocabulary of plans and surfaces (<see cref="ProductQualifierRepository"/>) and for the
/// qualifiers that articles, product aliases and timeline events carry.
/// </summary>
public class ProductQualifierTests : IClassFixture<DatabaseFixture<ProductQualifierTests>>
{
    private readonly DatabaseFixture<ProductQualifierTests> _fixture;
    private readonly ProductQualifierRepository _sut;
    private readonly ProductRepository _products;
    private readonly ContentItemWriteRepository _writeRepository;
    private readonly TimelineEventRepository _timelineRepository;

    public ProductQualifierTests(DatabaseFixture<ProductQualifierTests> fixture)
    {
        _fixture = fixture;
        _sut = new ProductQualifierRepository(fixture.Connection, NullLogger<ProductQualifierRepository>.Instance);
        _products = new ProductRepository(fixture.Connection, NullLogger<ProductRepository>.Instance);
        _writeRepository = new ContentItemWriteRepository(fixture.Connection, NullLogger<ContentItemWriteRepository>.Instance);
        _timelineRepository = new TimelineEventRepository(fixture.Connection);
    }

    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    // ── Starting vocabulary ───────────────────────────────────────────────────

    [Fact]
    public async Task StartingVocabulary_HasThePlansAndSurfacesOfGitHubCopilotWithAliases()
    {
        var qualifiers = await _sut.GetQualifiersAsync(Ct);

        qualifiers.Where(q => q.Kind == QualifierKinds.Plan).Select(q => q.Name)
            .Should().Contain(["Free", "Pro", "Pro+", "Business", "Enterprise"]);
        var vsCode = qualifiers.Should().ContainSingle(q => q.Kind == QualifierKinds.Surface && q.Name == "Visual Studio Code").Subject;
        vsCode.Aliases.Select(a => a.Alias).Should().Contain("VS Code");
    }

    [Fact]
    public async Task StartingCatalog_TheTierNamesOfGitHubCopilotAreAliasesThatImplyThePlan()
    {
        var copilot = (await _products.GetProductsAsync("GitHub Copilot", Ct)).Single(p => p.Name == "GitHub Copilot");

        var detail = (await _products.GetDetailAsync(copilot.Id, Ct))!;

        var business = detail.Aliases.Should().ContainSingle(a => a.Alias == "GitHub Copilot Business").Subject;
        business.QualifierName.Should().Be("Business");
        business.QualifierKind.Should().Be(QualifierKinds.Plan);
    }

    // ── Product names that imply a qualifier ──────────────────────────────────

    [Fact]
    public async Task UpdateAiMetadata_ANameThatImpliesAPlan_LinksTheProductAndThePlan()
    {
        var slug = await SeedItemAsync();

        await WriteProductsAsync(slug, "GitHub Copilot Business");

        var link = (await GetArticleLinksAsync(slug)).Should().ContainSingle().Subject;
        link.ProductName.Should().Be("GitHub Copilot");
        link.QualifierName.Should().Be("Business");
    }

    [Fact]
    public async Task ReplaceEvents_ANameThatImpliesAPlan_LinksTheProductAndAddsThePlanToTheEvent()
    {
        var slug = await SeedItemAsync();

        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event("GitHub Copilot Enterprise")], "source", 3, Ct);

        (await GetEventProductNamesAsync(slug)).Should().Equal("GitHub Copilot");
        (await GetEventQualifiersAsync(slug)).Should().BeEquivalentTo([(QualifierKinds.Plan, "Enterprise")]);
    }

    // ── Plans and surfaces of events ──────────────────────────────────────────

    [Fact]
    public async Task ReplaceEvents_StoresThePlansAndSurfacesAsReturnedAndLinksThemByNameOrAlias()
    {
        var slug = await SeedItemAsync();

        await _timelineRepository.ReplaceEventsAsync("news", slug,
            [Event("GitHub Copilot", plans: ["business", "Copilot Enterprise"], surfaces: ["VS Code", "Unknown surface"])], "source", 3, Ct);

        var stored = (await _timelineRepository.GetEventsForItemAsync("news", slug, Ct)).Should().ContainSingle().Subject;
        stored.Plans.Should().Equal("business", "Copilot Enterprise");
        stored.Surfaces.Should().Equal("VS Code", "Unknown surface");
        (await GetEventQualifiersAsync(slug)).Should().BeEquivalentTo(
            [(QualifierKinds.Plan, "Business"), (QualifierKinds.Plan, "Enterprise"), (QualifierKinds.Surface, "Visual Studio Code")]);
    }

    [Fact]
    public async Task ReplaceEvents_AgainReplacesTheQualifiersOfTheEvents()
    {
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event("GitHub Copilot", plans: ["Pro"])], "source", 3, Ct);

        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event("GitHub Copilot", surfaces: ["Xcode"])], "source", 3, Ct);

        (await GetEventQualifiersAsync(slug)).Should().BeEquivalentTo([(QualifierKinds.Surface, "Xcode")]);
    }

    [Fact]
    public async Task GetDetail_ShowsTheQualifiersOfEventsAndTheQualifierOfArticleNames()
    {
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, "GitHub Copilot Pro+");
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event("GitHub Copilot", plans: ["Pro"], surfaces: ["JetBrains"])], "source", 3, Ct);
        var copilot = (await _products.GetProductsAsync("GitHub Copilot", Ct)).Single(p => p.Name == "GitHub Copilot");

        var detail = (await _products.GetDetailAsync(copilot.Id, Ct))!;

        detail.Articles.Should().Contain(a => a.Slug == slug && a.QualifierName == "Pro+");
        var events = detail.Events.Where(e => e.ItemSlug == slug).Should().ContainSingle().Subject;
        events.Qualifiers.Select(q => (q.Kind, q.Name)).Should().BeEquivalentTo([(QualifierKinds.Plan, "Pro"), (QualifierKinds.Surface, "JetBrains IDEs")]);
    }

    // ── Review queue ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetUnmatchedNames_ListsThePlansAndSurfacesThatAreNotInTheVocabulary()
    {
        var plan = UniqueName("NewPlan");
        var surface = UniqueName("NewSurface");
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug,
            [Event("GitHub Copilot", plans: [plan, "Pro"], surfaces: [surface]), Event("GitHub Copilot", kind: "preview", plans: [plan])], "source", 3, Ct);

        var unmatched = await _sut.GetUnmatchedNamesAsync(Ct);

        unmatched.Should().ContainSingle(n => n.Kind == QualifierKinds.Plan && n.Name == plan).Which.EventCount.Should().Be(2);
        unmatched.Should().ContainSingle(n => n.Kind == QualifierKinds.Surface && n.Name == surface).Which.EventCount.Should().Be(1);
        unmatched.Should().NotContain(n => n.Name == "Pro");
    }

    [Fact]
    public async Task Create_LinksTheEventsThatUseTheName_AndLeavesTheReviewQueue()
    {
        var name = UniqueName("Surface");
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event("GitHub Copilot", surfaces: [name])], "source", 3, Ct);

        var result = await _sut.CreateAsync(QualifierKinds.Surface, name.ToUpperInvariant(), Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        (await GetEventQualifiersAsync(slug)).Should().ContainSingle().Which.Name.Should().Be(name.ToUpperInvariant());
        (await _sut.GetUnmatchedNamesAsync(Ct)).Should().NotContain(n => n.Name == name);
    }

    [Fact]
    public async Task AddAlias_LinksTheEventsThatUseTheAlias_AndRemoveAliasUnlinksThem()
    {
        var alias = UniqueName("Alias");
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event("GitHub Copilot", surfaces: [alias])], "source", 3, Ct);
        var xcode = (await _sut.GetQualifiersAsync(Ct)).Single(q => q.Name == "Xcode");

        (await _sut.AddAliasAsync(xcode.Id, alias, Ct)).Succeeded.Should().BeTrue();

        (await GetEventQualifiersAsync(slug)).Should().BeEquivalentTo([(QualifierKinds.Surface, "Xcode")]);

        var aliasId = (await _sut.GetQualifiersAsync(Ct)).Single(q => q.Id == xcode.Id).Aliases.Single(a => a.Alias == alias).Id;
        (await _sut.RemoveAliasAsync(xcode.Id, aliasId, Ct)).Succeeded.Should().BeTrue();

        (await GetEventQualifiersAsync(slug)).Should().BeEmpty();
    }

    [Fact]
    public async Task Rename_KeepsTheOldNameAsAliasSoLinksStay()
    {
        var name = UniqueName("Old");
        var created = await _sut.CreateAsync(QualifierKinds.Plan, name, Ct);
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event("GitHub Copilot", plans: [name])], "source", 3, Ct);
        var newName = UniqueName("New");

        var result = await _sut.RenameAsync(created.ProductId!.Value, newName, Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        var renamed = (await _sut.GetQualifiersAsync(Ct)).Single(q => q.Id == created.ProductId);
        renamed.Name.Should().Be(newName);
        renamed.Aliases.Select(a => a.Alias).Should().Equal(name);
        (await GetEventQualifiersAsync(slug)).Should().BeEquivalentTo([(QualifierKinds.Plan, newName)]);
    }

    [Fact]
    public async Task Delete_RemovesTheQualifierFromEventsAndFromProductAliases()
    {
        var created = await _sut.CreateAsync(QualifierKinds.Plan, UniqueName("Doomed"), Ct);
        var qualifierId = created.ProductId!.Value;
        var product = await CreateProductAsync();
        var alias = UniqueName("TierAlias");
        (await _products.AddAliasAsync(product.Id, alias, qualifierId, Ct)).Succeeded.Should().BeTrue();
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(alias)], "source", 3, Ct);
        (await GetEventQualifiersAsync(slug)).Should().ContainSingle();

        (await _sut.DeleteAsync(qualifierId, Ct)).Succeeded.Should().BeTrue();

        (await GetEventQualifiersAsync(slug)).Should().BeEmpty();
        (await _products.GetDetailAsync(product.Id, Ct))!.Aliases.Should().ContainSingle(a => a.Alias == alias).Which.QualifierId.Should().BeNull();
    }

    // ── Aliases that imply a qualifier ────────────────────────────────────────

    [Fact]
    public async Task AddAlias_WithAQualifier_LinksArticlesAndEventsToTheProductAndTheQualifier()
    {
        var product = await CreateProductAsync();
        var alias = UniqueName("Tier");
        var business = (await _sut.GetQualifiersAsync(Ct)).Single(q => q.Name == "Business");
        var slug = await SeedItemAsync();
        await WriteProductsAsync(slug, alias);
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(alias)], "source", 3, Ct);

        var result = await _products.AddAliasAsync(product.Id, alias, business.Id, Ct);

        result.Succeeded.Should().BeTrue(result.Message);
        var detail = (await _products.GetDetailAsync(product.Id, Ct))!;
        detail.Aliases.Should().ContainSingle().Which.QualifierName.Should().Be("Business");
        detail.Articles.Should().ContainSingle().Which.QualifierName.Should().Be("Business");
        detail.Events.Should().ContainSingle().Which.Qualifiers.Should().ContainSingle().Which.Name.Should().Be("Business");
    }

    [Fact]
    public async Task SetAliasQualifier_ChangesAndClearsTheQualifierOfAnAlias()
    {
        var product = await CreateProductAsync();
        var alias = UniqueName("Tier");
        (await _products.AddAliasAsync(product.Id, alias, Ct)).Succeeded.Should().BeTrue();
        var aliasId = (await _products.GetDetailAsync(product.Id, Ct))!.Aliases.Single().Id;
        var pro = (await _sut.GetQualifiersAsync(Ct)).Single(q => q.Name == "Pro");
        var slug = await SeedItemAsync();
        await _timelineRepository.ReplaceEventsAsync("news", slug, [Event(alias)], "source", 3, Ct);

        (await _products.SetAliasQualifierAsync(product.Id, aliasId, pro.Id, Ct)).Succeeded.Should().BeTrue();

        (await GetEventQualifiersAsync(slug)).Should().BeEquivalentTo([(QualifierKinds.Plan, "Pro")]);

        (await _products.SetAliasQualifierAsync(product.Id, aliasId, null, Ct)).Succeeded.Should().BeTrue();

        (await GetEventQualifiersAsync(slug)).Should().BeEmpty();
    }

    [Fact]
    public async Task AddAlias_WithUnknownQualifier_IsNotFound()
    {
        var product = await CreateProductAsync();

        var result = await _products.AddAliasAsync(product.Id, UniqueName("Alias"), long.MaxValue, Ct);

        result.Status.Should().Be(ProductOperationStatus.NotFound);
        (await _products.GetDetailAsync(product.Id, Ct))!.Aliases.Should().BeEmpty();
    }

    [Fact]
    public async Task SetAliasQualifier_WithUnknownAliasOrQualifier_IsNotFound()
    {
        var product = await CreateProductAsync();
        var alias = UniqueName("Alias");
        (await _products.AddAliasAsync(product.Id, alias, Ct)).Succeeded.Should().BeTrue();
        var aliasId = (await _products.GetDetailAsync(product.Id, Ct))!.Aliases.Single().Id;

        (await _products.SetAliasQualifierAsync(product.Id, long.MaxValue, null, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _products.SetAliasQualifierAsync(product.Id, aliasId, long.MaxValue, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
    }

    // ── Names that are not allowed ────────────────────────────────────────────

    [Fact]
    public async Task Create_WithUnknownKind_IsInvalid()
    {
        var result = await _sut.CreateAsync("tier", UniqueName("Name"), Ct);

        result.Status.Should().Be(ProductOperationStatus.Invalid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_WithEmptyName_IsInvalid(string name)
    {
        var result = await _sut.CreateAsync(QualifierKinds.Plan, name, Ct);

        result.Status.Should().Be(ProductOperationStatus.Invalid);
    }

    [Fact]
    public async Task Create_WithTheNameOrAliasOfAnExistingQualifier_IsAConflict()
    {
        (await _sut.CreateAsync(QualifierKinds.Plan, "  BUSINESS ", Ct)).Status.Should().Be(ProductOperationStatus.Conflict);
        (await _sut.CreateAsync(QualifierKinds.Surface, "vs code", Ct)).Status.Should().Be(ProductOperationStatus.Conflict);
    }

    [Fact]
    public async Task Create_TheSameNameForAnotherKind_IsAllowed()
    {
        var name = UniqueName("Shared");
        (await _sut.CreateAsync(QualifierKinds.Plan, name, Ct)).Succeeded.Should().BeTrue();

        (await _sut.CreateAsync(QualifierKinds.Surface, name, Ct)).Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Rename_ToTheNameOfAnotherQualifier_IsAConflict()
    {
        var first = await _sut.CreateAsync(QualifierKinds.Plan, UniqueName("First"), Ct);
        var second = await _sut.CreateAsync(QualifierKinds.Plan, UniqueName("Second"), Ct);
        var firstName = (await _sut.GetQualifiersAsync(Ct)).Single(q => q.Id == first.ProductId).Name;

        var result = await _sut.RenameAsync(second.ProductId!.Value, firstName, Ct);

        result.Status.Should().Be(ProductOperationStatus.Conflict);
    }

    [Fact]
    public async Task Operations_OnUnknownQualifiers_AreNotFound()
    {
        (await _sut.RenameAsync(long.MaxValue, "Name", Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.DeleteAsync(long.MaxValue, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.AddAliasAsync(long.MaxValue, "Name", Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
        (await _sut.RemoveAliasAsync(long.MaxValue, 1, Ct)).Status.Should().Be(ProductOperationStatus.NotFound);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string UniqueName(string prefix) => $"{prefix}-{Guid.NewGuid():N}";

    private static TimelineEvent Event(string product, string kind = "ga", string[]? plans = null, string[]? surfaces = null) => new()
    {
        Kind = kind,
        Product = product,
        Feature = null,
        Plans = plans ?? [],
        Surfaces = surfaces ?? [],
        Description = $"{kind} of {product}",
        Date = DateOnly.Parse("2026-03-10", CultureInfo.InvariantCulture),
        DatePrecision = "day",
        DateSource = "explicit",
        Quote = null
    };

    private async Task<string> SeedItemAsync()
    {
        var slug = $"qualifier-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash, sections_bitmask, created_at)
            VALUES (@Slug, 'news', 'Qualifier test', '', '', 1743278400, 'ai',
                    @Url, 'Author', 'Feed', '', 'hash', 1, NOW())
            """,
            new { Slug = slug, Url = $"https://example.com/{slug}" });
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
        var result = await _products.CreateAsync(name, Ct);
        result.Succeeded.Should().BeTrue(result.Message);
        return new ProductSummary { Id = result.ProductId!.Value, Name = name };
    }

    private async Task<List<(string ProductName, string? QualifierName)>> GetArticleLinksAsync(string slug) =>
        (await _fixture.Connection.QueryAsync<(string ProductName, string? QualifierName)>(
            """
            SELECT p.name AS ProductName, q.name AS QualifierName
            FROM content_item_products l
            JOIN products p ON p.id = l.product_id
            LEFT JOIN product_qualifiers q ON q.id = l.qualifier_id
            WHERE l.collection_name = 'news' AND l.item_slug = @Slug
            ORDER BY l.position
            """,
            new { Slug = slug })).ToList();

    private async Task<List<string>> GetEventProductNamesAsync(string slug) =>
        (await _fixture.Connection.QueryAsync<string>(
            """
            SELECT p.name FROM content_timeline_events e JOIN products p ON p.id = e.product_id
            WHERE e.collection_name = 'news' AND e.item_slug = @Slug ORDER BY e.id
            """,
            new { Slug = slug })).ToList();

    private async Task<List<(string Kind, string Name)>> GetEventQualifiersAsync(string slug) =>
        (await _fixture.Connection.QueryAsync<(string Kind, string Name)>(
            """
            SELECT DISTINCT q.kind AS Kind, q.name AS Name
            FROM content_timeline_events e
            JOIN content_timeline_event_qualifiers eq ON eq.event_id = e.id
            JOIN product_qualifiers q ON q.id = eq.qualifier_id
            WHERE e.collection_name = 'news' AND e.item_slug = @Slug
            """,
            new { Slug = slug })).ToList();
}
