using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Npgsql;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// An article write and the replacement of its timeline events commit or roll back together: when the event write
/// fails, the previous article state stays intact and the article stays eligible for a retry.
/// </summary>
public class TimelineAtomicityTests : IClassFixture<DatabaseFixture<TimelineAtomicityTests>>
{
    private const string Collection = "news";

    private readonly DatabaseFixture<TimelineAtomicityTests> _fixture;
    private readonly ContentItemWriteRepository _writeRepo;
    private readonly ContentRepository _contentRepo;
    private readonly TimelineEventRepository _timelineRepo;

    public TimelineAtomicityTests(DatabaseFixture<TimelineAtomicityTests> fixture)
    {
        _fixture = fixture;
        _writeRepo = new ContentItemWriteRepository(fixture.Connection, NullLogger<ContentItemWriteRepository>.Instance);
        _timelineRepo = new TimelineEventRepository(fixture.Connection);
        _contentRepo = new ContentRepository(
            fixture.Connection,
            new PostgresDialect(),
            new MemoryCache(new MemoryCacheOptions()),
            Mock.Of<IMarkdownService>(),
            Options.Create(new AppSettings
            {
                BaseUrl = "https://localhost:7245",
                Content = new ContentSettings { Sections = new Dictionary<string, SectionConfig>() }
            }));
    }

    private static TimelineEvent Event(string kind = "ga") => new()
    {
        Kind = kind,
        Product = "GitHub Copilot",
        Description = $"{kind} happens",
        Date = new DateOnly(2026, 3, 10),
        DatePrecision = "day",
        DateSource = "explicit"
    };

    // A null product violates the NOT NULL constraint, so writing the event fails.
    private static TimelineEvent BrokenEvent() => Event() with { Product = null! };

    private static ProcessedContentItem Item(string slug, params TimelineEvent[] events) => new()
    {
        Slug = slug,
        Title = "Atomic article",
        Excerpt = "Excerpt",
        Content = "Body",
        DateEpoch = 1743278400,
        CollectionName = Collection,
        ExternalUrl = $"https://example.com/{slug}",
        FeedName = "Feed",
        PrimarySectionName = "ai",
        Sections = ["ai"],
        Tags = ["AI"],
        ContentHash = "hash",
        TimelineEvents = events
    };

    private async Task<string> SeedAsync()
    {
        var slug = $"atomic-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, @Collection, 'Original title', 'Original text', 'Original excerpt', 1743278400, 'ai',
                    @Url, 'Author', 'Feed', '', 'hash')
            """,
            new { Slug = slug, Collection, Url = $"https://example.com/{slug}" });
        return slug;
    }

    private Task<string?> TitleAsync(string slug) => _fixture.Connection.ExecuteScalarAsync<string?>(
        "SELECT title FROM content_items WHERE collection_name = @Collection AND slug = @Slug", new { Collection, Slug = slug });

    [Fact]
    public async Task UpsertProcessedItemAsync_StoresTheItemAndItsEventsTogether()
    {
        var slug = $"atomic-{Guid.NewGuid():N}";

        await _writeRepo.UpsertProcessedItemAsync(Item(slug, Event("preview"), Event("ga")), TestContext.Current.CancellationToken);

        (await TitleAsync(slug)).Should().Be("Atomic article");
        var events = await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken);
        events.Select(e => e.Kind).Should().BeEquivalentTo("preview", "ga");
        events.Should().OnlyContain(e => e.ExtractedFrom == ITimelineEventRepository.ExtractedFromSource);
    }

    [Fact]
    public async Task UpsertProcessedItemAsync_WhenTheEventWriteFails_StoresNothingSoARetryCanSucceed()
    {
        var slug = $"atomic-{Guid.NewGuid():N}";

        var act = () => _writeRepo.UpsertProcessedItemAsync(Item(slug, BrokenEvent()), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<PostgresException>();
        (await _writeRepo.ExistsByExternalUrlAsync($"https://example.com/{slug}", TestContext.Current.CancellationToken)).Should().BeFalse();

        await _writeRepo.UpsertProcessedItemAsync(Item(slug, Event()), TestContext.Current.CancellationToken);
        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken)).Should().ContainSingle();
    }

    [Fact]
    public async Task UpdateAiMetadataIfUnchangedAsync_WhenTheEventWriteFails_KeepsThePreviousMetadataAndEvents()
    {
        var slug = await SeedAsync();
        await _timelineRepo.ReplaceEventsAsync(Collection, slug, [Event("preview")], ITimelineEventRepository.ExtractedFromSource, 1, TestContext.Current.CancellationToken);
        var revision = (await _timelineRepo.GetItemForExtractionAsync(Collection, slug, TestContext.Current.CancellationToken))!.Revision;

        var act = () => _writeRepo.UpdateAiMetadataIfUnchangedAsync(
            Collection, slug, """{"schema_version":2}""",
            new TimelineEventReplacement([BrokenEvent()], ITimelineEventRepository.ExtractedFromStoredContent, 2),
            revision,
            TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<PostgresException>();
        var metadata = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT ai_metadata::text FROM content_items WHERE collection_name = @Collection AND slug = @Slug", new { Collection, Slug = slug });
        metadata.Should().BeNull("the metadata version must not advance when its events were not stored");
        var events = await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken);
        events.Should().ContainSingle().Which.Kind.Should().Be("preview");
    }

    [Fact]
    public async Task UpdateAiMetadataIfUnchangedAsync_WithEvents_StoresBothTogether()
    {
        var slug = await SeedAsync();
        var revision = (await _timelineRepo.GetItemForExtractionAsync(Collection, slug, TestContext.Current.CancellationToken))!.Revision;

        var stored = await _writeRepo.UpdateAiMetadataIfUnchangedAsync(
            Collection, slug, """{"schema_version":2}""",
            new TimelineEventReplacement([Event("retirement")], ITimelineEventRepository.ExtractedFromStoredContent, 1),
            revision,
            TestContext.Current.CancellationToken);

        stored.Should().BeTrue();
        var metadata = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT ai_metadata::text FROM content_items WHERE collection_name = @Collection AND slug = @Slug", new { Collection, Slug = slug });
        metadata.Should().Contain("schema_version");
        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken))
            .Should().ContainSingle().Which.ExtractedFrom.Should().Be(ITimelineEventRepository.ExtractedFromStoredContent);
    }

    [Fact]
    public async Task UpdateAiMetadataIfUnchangedAsync_WithoutTimelineReplacement_LeavesExistingEventsUnchanged()
    {
        var slug = await SeedAsync();
        await _timelineRepo.ReplaceEventsAsync(Collection, slug, [Event("preview")], ITimelineEventRepository.ExtractedFromSource, 1, TestContext.Current.CancellationToken);
        var revision = (await _timelineRepo.GetItemForExtractionAsync(Collection, slug, TestContext.Current.CancellationToken))!.Revision;

        var stored = await _writeRepo.UpdateAiMetadataIfUnchangedAsync(
            Collection, slug, """{"schema_version":2}""",
            null,
            revision,
            TestContext.Current.CancellationToken);

        stored.Should().BeTrue();
        var metadata = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT ai_metadata::text FROM content_items WHERE collection_name = @Collection AND slug = @Slug", new { Collection, Slug = slug });
        metadata.Should().Contain("schema_version");
        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken))
            .Should().ContainSingle().Which.Kind.Should().Be("preview");
    }

    [Fact]
    public async Task UpdateAiMetadataIfUnchangedAsync_WhenTheArticleWasEditedMeanwhile_StoresNothing()
    {
        var slug = await SeedAsync();
        await _timelineRepo.ReplaceEventsAsync(Collection, slug, [Event("preview")], ITimelineEventRepository.ExtractedFromSource, 1, TestContext.Current.CancellationToken);
        var revision = (await _timelineRepo.GetItemForExtractionAsync(Collection, slug, TestContext.Current.CancellationToken))!.Revision;
        await _contentRepo.UpdateEditDataAsync(Collection, slug, EditData(slug), TestContext.Current.CancellationToken);

        var stored = await _writeRepo.UpdateAiMetadataIfUnchangedAsync(
            Collection, slug, """{"schema_version":2}""",
            new TimelineEventReplacement([Event("retirement")], ITimelineEventRepository.ExtractedFromStoredContent, 2),
            revision,
            TestContext.Current.CancellationToken);

        stored.Should().BeFalse();
        var metadata = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT ai_metadata::text FROM content_items WHERE collection_name = @Collection AND slug = @Slug", new { Collection, Slug = slug });
        metadata.Should().BeNull("metadata classified from outdated text must not be stored");
        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken))
            .Should().BeEmpty("the edit removed the events of the previous text and the stale classification must not bring new ones");
    }

    [Fact]
    public async Task UpdateAiMetadataKeepingStoredProductsIfUnchangedAsync_KeepsProductsStoredAfterTheArticleWasRead()
    {
        var slug = await SeedAsync();
        var revision = (await _timelineRepo.GetItemForExtractionAsync(Collection, slug, TestContext.Current.CancellationToken))!.Revision;

        // A timeline backfill stores products without changing the revision while the roundup classifies the article.
        var timelineStored = await _timelineRepo.ReplaceEventsIfUnchangedAsync(
            Collection, slug, [], ITimelineEventRepository.ExtractedFromStoredContent, 1, revision, ["Azure Functions"],
            [new ProductFeatureMention { Product = "Azure Functions", Feature = "Durable Functions" }],
            TestContext.Current.CancellationToken);
        timelineStored.Should().BeTrue();

        var stored = await _writeRepo.UpdateAiMetadataKeepingStoredProductsIfUnchangedAsync(
            Collection, slug, """{"schema_version":3,"significance":"low","products":[],"product_features":[]}""", revision, TestContext.Current.CancellationToken);

        stored.Should().BeTrue();
        var metadata = await _fixture.Connection.ExecuteScalarAsync<string?>(
            "SELECT ai_metadata::text FROM content_items WHERE collection_name = @Collection AND slug = @Slug", new { Collection, Slug = slug });
        metadata.Should().Contain("\"significance\": \"low\"").And.Contain("Azure Functions");
        metadata.Should().Contain("Durable Functions", "neutral metadata updates preserve stored feature mentions too");
        var links = await _fixture.Connection.QueryAsync<string>(
            "SELECT product_name FROM content_item_products WHERE collection_name = @Collection AND item_slug = @Slug", new { Collection, Slug = slug });
        links.Should().ContainSingle().Which.Should().Be("Azure Functions");
    }

    [Fact]
    public async Task UpdateEditDataAsync_WhenTheEventWriteFails_KeepsThePreviousTextAndEvents()
    {
        var slug = await SeedAsync();
        await _timelineRepo.ReplaceEventsAsync(Collection, slug, [Event("preview")], ITimelineEventRepository.ExtractedFromSource, 1, TestContext.Current.CancellationToken);
        var edit = EditData(slug);

        var act = () => _contentRepo.UpdateEditDataAsync(
            Collection, slug, edit,
            new TimelineEventReplacement([BrokenEvent()], ITimelineEventRepository.ExtractedFromSource, 1),
            TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<PostgresException>();
        (await TitleAsync(slug)).Should().Be("Original title");
        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken))
            .Should().ContainSingle().Which.Kind.Should().Be("preview");
    }

    [Fact]
    public async Task UpdateEditDataAsync_WithEvents_StoresBothTogether()
    {
        var slug = await SeedAsync();

        var updated = await _contentRepo.UpdateEditDataAsync(
            Collection, slug, EditData(slug),
            new TimelineEventReplacement([Event("ga")], ITimelineEventRepository.ExtractedFromSource, 1),
            TestContext.Current.CancellationToken);

        updated.Should().BeTrue();
        (await TitleAsync(slug)).Should().Be("Edited title");
        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken))
            .Should().ContainSingle().Which.Kind.Should().Be("ga");
    }

    [Fact]
    public async Task UpdateEditDataAsync_ForUnknownItem_ReturnsFalseAndWritesNoEvents()
    {
        var slug = $"missing-{Guid.NewGuid():N}";

        var updated = await _contentRepo.UpdateEditDataAsync(
            Collection, slug, EditData(slug),
            new TimelineEventReplacement([Event()], ITimelineEventRepository.ExtractedFromSource, 1),
            TestContext.Current.CancellationToken);

        updated.Should().BeFalse();
        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken)).Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateEditDataAsync_WithoutReplacement_WhenTheTextChanged_InvalidatesTheEventsSoTheBackfillScansItAgain()
    {
        var slug = await SeedAsync();
        await _timelineRepo.ReplaceEventsAsync(Collection, slug, [Event("preview")], ITimelineEventRepository.ExtractedFromSource, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken);
        var request = new TimelineBackfillRequest { Collection = Collection };
        (await _timelineRepo.GetBackfillCandidatesAsync(request, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken))
            .Should().NotContain(c => c.Slug == slug, "a scanned article is not a candidate");

        var updated = await _contentRepo.UpdateEditDataAsync(Collection, slug, EditData(slug), TestContext.Current.CancellationToken);

        updated.Should().BeTrue();
        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken)).Should().BeEmpty();
        (await _timelineRepo.GetBackfillCandidatesAsync(request, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken))
            .Should().Contain(c => c.Slug == slug, "an edited article must be scanned again by an ordinary backfill run");
    }

    [Fact]
    public async Task UpdateEditDataAsync_WithoutReplacement_WhenOnlyOtherFieldsChanged_KeepsTheEventsAndMarker()
    {
        var slug = await SeedAsync();
        await _timelineRepo.ReplaceEventsAsync(Collection, slug, [Event("preview")], ITimelineEventRepository.ExtractedFromSource, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken);
        var unchangedText = EditData(slug) with { Title = "Original title", Excerpt = "Original excerpt", Content = "Original text", DateEpoch = 1743278400, Author = "Someone else" };

        var updated = await _contentRepo.UpdateEditDataAsync(Collection, slug, unchangedText, TestContext.Current.CancellationToken);

        updated.Should().BeTrue();
        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken)).Should().ContainSingle();
        (await _timelineRepo.GetBackfillCandidatesAsync(new TimelineBackfillRequest { Collection = Collection }, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken))
            .Should().NotContain(c => c.Slug == slug);
    }

    [Fact]
    public async Task ReplaceEventsAsync_KeepsTheEventsOfTheCatalogAndReplacesTheOthers()
    {
        var slug = await SeedAsync();
        await InsertCatalogEventAsync(slug);
        await _timelineRepo.ReplaceEventsAsync(Collection, slug, [Event("preview")], ITimelineEventRepository.ExtractedFromSource, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken);

        await _timelineRepo.ReplaceEventsAsync(Collection, slug, [Event("ga")], ITimelineEventRepository.ExtractedFromStoredContent, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken);

        var events = await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken);
        events.Select(e => e.Kind).Should().BeEquivalentTo(["launch", "ga"], "the AI replaced its own preview event and left the curated one");
    }

    [Fact]
    public async Task ReplaceEventsAsync_WithNoEvents_StillKeepsTheEventsOfTheCatalog()
    {
        var slug = await SeedAsync();
        await InsertCatalogEventAsync(slug);

        await _timelineRepo.ReplaceEventsAsync(Collection, slug, [], ITimelineEventRepository.ExtractedFromStoredContent, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken);

        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken)).Should().ContainSingle().Which.Kind.Should().Be("launch");
    }

    [Fact]
    public async Task UpdateEditDataAsync_WhenTheTextChanged_RemovesTheEventsOfTheTextButKeepsTheEventsOfTheCatalog()
    {
        var slug = await SeedAsync();
        await InsertCatalogEventAsync(slug);
        await _timelineRepo.ReplaceEventsAsync(Collection, slug, [Event("preview")], ITimelineEventRepository.ExtractedFromSource, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken);

        await _contentRepo.UpdateEditDataAsync(Collection, slug, EditData(slug), TestContext.Current.CancellationToken);

        (await _timelineRepo.GetEventsForItemAsync(Collection, slug, TestContext.Current.CancellationToken)).Should().ContainSingle().Which.Kind.Should().Be("launch");
    }

    private async Task InsertCatalogEventAsync(string slug) => await _fixture.Connection.ExecuteAsync(
        """
        INSERT INTO content_timeline_events
            (collection_name, item_slug, kind, product, feature, description, event_date, date_precision, date_source, extracted_from)
        VALUES (@Collection, @Slug, 'launch', 'GitHub Copilot', 'Agent mode', 'Curated', DATE '2025-04-15', 'day', 'explicit', @ExtractedFrom)
        """,
        new { Collection, Slug = slug, ExtractedFrom = ITimelineEventRepository.ExtractedFromCatalog });

    private static ContentItemEditData EditData(string slug) => new()
    {
        CollectionName = Collection,
        Slug = slug,
        DateEpoch = 1743278400,
        Title = "Edited title",
        Author = "Author",
        Excerpt = "Edited excerpt",
        Content = "Edited text",
        PrimarySectionName = "ai",
        Tags = ["AI"],
        Sections = ["ai"]
    };
}
