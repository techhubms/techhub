using System.Globalization;
using Dapper;
using FluentAssertions;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="TimelineEventRepository.GetTimelineAsync"/>: the timeline of a section built from the
/// events of its articles. Every test uses its own product names so the shared database never leaks into the assertions.
/// </summary>
public class TimelineSectionQueryTests : IClassFixture<DatabaseFixture<TimelineSectionQueryTests>>
{
    private const int AiBit = 1;
    private const int AzureBit = 2;

    private readonly DatabaseFixture<TimelineSectionQueryTests> _fixture;
    private readonly TimelineEventRepository _sut;

    public TimelineSectionQueryTests(DatabaseFixture<TimelineSectionQueryTests> fixture)
    {
        _fixture = fixture;
        _sut = new TimelineEventRepository(fixture.Connection);
    }

    private static string NewProduct() => $"Timeline Product {Guid.NewGuid():N}";

    private async Task<string> SeedItemAsync(int sectionsBitmask = AiBit, string collection = "news", string title = "Timeline article", long dateEpoch = 1743278400, string primarySection = "ai")
    {
        var slug = $"timeline-section-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, sections_bitmask, content_hash)
            VALUES (@Slug, @Collection, @Title, '', '', @DateEpoch, @PrimarySection,
                    @Url, 'Author', 'Feed', '', @Bitmask, 'hash')
            """,
            new { Slug = slug, Collection = collection, Title = title, DateEpoch = dateEpoch, PrimarySection = primarySection, Url = $"https://example.com/{slug}", Bitmask = sectionsBitmask });
        return slug;
    }

    private static TimelineEvent Event(
        string product,
        string? feature = "Agent mode",
        string kind = "ga",
        string date = "2026-03-10",
        string precision = "day",
        string? description = null) => new()
        {
            Kind = kind,
            Product = product,
            Feature = feature,
            Description = description ?? $"{kind} of {feature ?? product}",
            Date = DateOnly.Parse(date, CultureInfo.InvariantCulture),
            DatePrecision = precision,
            DateSource = "explicit"
        };

    private Task StoreAsync(string collection, string slug, params TimelineEvent[] events) =>
        _sut.ReplaceEventsAsync(collection, slug, events, ITimelineEventRepository.ExtractedFromSource, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken);

    private Task<IReadOnlyList<TimelineEntry>> TimelineAsync(string section, string product, DateOnly? from = null, DateOnly? to = null, IReadOnlyList<string>? kinds = null) =>
        // The shared database holds events of other tests, so only the entries of this test's product are looked at.
        FilterAsync(_sut.GetTimelineAsync(new TimelineRequest(section, from, to, kinds), TestContext.Current.CancellationToken), product);

    private static async Task<IReadOnlyList<TimelineEntry>> FilterAsync(Task<IReadOnlyList<TimelineEntry>> entries, string product) =>
        (await entries).Where(e => e.Product.Equals(product, StringComparison.OrdinalIgnoreCase)).ToList();

    [Fact]
    public async Task GetTimelineAsync_ReturnsTheEventsOfTheSectionNewestFirst()
    {
        var product = NewProduct();
        var aiItem = await SeedItemAsync(AiBit);
        var azureItem = await SeedItemAsync(AzureBit, primarySection: "azure");
        await StoreAsync("news", aiItem, Event(product, "Older", date: "2026-01-05"), Event(product, "Newer", date: "2026-04-20"));
        await StoreAsync("news", azureItem, Event(product, "Elsewhere", date: "2026-02-01"));

        var entries = await TimelineAsync("ai", product);

        entries.Select(e => e.Feature).Should().Equal("Newer", "Older");
    }

    [Fact]
    public async Task GetTimelineAsync_OnlyUsesArticlesWhosePrimarySectionIsTheSection()
    {
        var product = NewProduct();
        var aiItem = await SeedItemAsync(AiBit | AzureBit, primarySection: "ai");
        var azureItem = await SeedItemAsync(AiBit | AzureBit, primarySection: "azure");
        await StoreAsync("news", aiItem, Event(product, "Primary"));
        await StoreAsync("news", azureItem, Event(product, "Secondary"));

        var entries = await TimelineAsync("ai", product);

        entries.Select(e => e.Feature).Should().Equal(["Primary"], "an article that only touches a section states events about other products");
    }

    [Fact]
    public async Task GetTimelineAsync_ExcludesRoundups()
    {
        var product = NewProduct();
        var roundup = await SeedItemAsync(AiBit, collection: "roundups");
        await StoreAsync("roundups", roundup, Event(product));

        var entries = await TimelineAsync("ai", product);

        entries.Should().BeEmpty("a roundup only repeats what its articles state");
    }

    [Fact]
    public async Task GetTimelineAsync_MergesTheSameEventStatedByTwoArticles()
    {
        var product = NewProduct();
        var first = await SeedItemAsync(AiBit, title: "The announcement", dateEpoch: 1743278400);
        var second = await SeedItemAsync(AiBit, title: "The follow-up", dateEpoch: 1745000000);
        await StoreAsync("news", second, Event(product, description: "Follow-up wording"));
        await StoreAsync("news", first, Event(product, description: "Original wording"));

        var entries = await TimelineAsync("ai", product);

        var entry = entries.Should().ContainSingle().Subject;
        entry.Description.Should().Be("Original wording", "the oldest article is the source of the description");
        entry.Sources.Select(s => s.Title).Should().Equal("The announcement", "The follow-up");
        entry.Sources[0].Slug.Should().Be(first);
        entry.Sources[0].CollectionName.Should().Be("news");
        entry.Sources[0].GetHref().Should().Be($"https://example.com/{first}", "news links to the original article");
    }

    [Fact]
    public async Task GetTimelineAsync_KeepsDifferentKindsFeaturesAndDatesApart()
    {
        var product = NewProduct();
        var item = await SeedItemAsync();
        await StoreAsync("news", item,
            Event(product, "Agent mode", "ga", "2026-03-10"),
            Event(product, "Agent mode", "preview", "2026-03-10"),
            Event(product, "Chat", "ga", "2026-03-10"),
            Event(product, "Agent mode", "ga", "2026-04-10"));

        var entries = await TimelineAsync("ai", product);

        entries.Should().HaveCount(4);
    }

    [Fact]
    public async Task GetTimelineAsync_FiltersByDateRangeAndKinds()
    {
        var product = NewProduct();
        var item = await SeedItemAsync();
        await StoreAsync("news", item,
            Event(product, "A", "ga", "2026-01-10"),
            Event(product, "B", "ga", "2026-03-10"),
            Event(product, "C", "retirement", "2026-03-20"),
            Event(product, "D", "ga", "2026-06-10"));

        var inRange = await TimelineAsync("ai", product, new DateOnly(2026, 2, 1), new DateOnly(2026, 4, 30));
        var onlyRetirements = await TimelineAsync("ai", product, kinds: ["retirement"]);

        inRange.Select(e => e.Feature).Should().Equal("C", "B");
        onlyRetirements.Select(e => e.Feature).Should().Equal("C");
    }

    [Fact]
    public async Task GetTimelineAsync_UsesCatalogNames()
    {
        var product = NewProduct();
        await _fixture.Connection.ExecuteAsync("INSERT INTO products (name) VALUES (@Name)", new { Name = product });
        var productId = await _fixture.Connection.ExecuteScalarAsync<long>("SELECT id FROM products WHERE name = @Name", new { Name = product });
        await _fixture.Connection.ExecuteAsync(
            "INSERT INTO product_features (product_id, name) VALUES (@ProductId, 'Agent mode')", new { ProductId = productId });
        var item = await SeedItemAsync();
        await StoreAsync("news", item, Event(product.ToUpperInvariant(), "agent MODE"));

        var entry = (await TimelineAsync("ai", product)).Should().ContainSingle().Subject;

        entry.Product.Should().Be(product, "the catalog spelling replaces the AI's text");
        entry.ProductId.Should().Be(productId);
        entry.Feature.Should().Be("Agent mode");
    }

    [Fact]
    public async Task GetTimelineAsync_ForAProductNotInTheCatalog_KeepsTheTextAndHasNoProductId()
    {
        var product = NewProduct();
        var item = await SeedItemAsync();
        await StoreAsync("news", item, Event(product, feature: null));

        var entry = (await TimelineAsync("ai", product)).Should().ContainSingle().Subject;

        entry.ProductId.Should().BeNull();
        entry.Feature.Should().BeNull();
    }

    [Fact]
    public async Task GetTimelineAsync_ForAllSections_CoversEverySection()
    {
        var product = NewProduct();
        var aiItem = await SeedItemAsync(AiBit);
        var azureItem = await SeedItemAsync(AzureBit, primarySection: "azure");
        await StoreAsync("news", aiItem, Event(product, "In AI"));
        await StoreAsync("news", azureItem, Event(product, "In Azure"));

        var entries = await TimelineAsync("all", product);

        entries.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetTimelineAsync_ForAnUnknownSection_ReturnsNothing()
    {
        var product = NewProduct();
        var item = await SeedItemAsync();
        await StoreAsync("news", item, Event(product));

        var entries = await _sut.GetTimelineAsync(new TimelineRequest("no-such-section"), TestContext.Current.CancellationToken);

        entries.Should().BeEmpty();
    }
}
