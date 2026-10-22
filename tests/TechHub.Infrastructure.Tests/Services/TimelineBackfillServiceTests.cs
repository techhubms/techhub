using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services.ContentProcessing;
using TechHub.Infrastructure.Tests.Repositories;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Integration tests for <see cref="TimelineBackfillService"/> against PostgreSQL, with a mocked AI service.
/// Every test works in its own collection so tests do not see each other's articles.
/// </summary>
public class TimelineBackfillServiceTests : IClassFixture<DatabaseFixture<TimelineBackfillServiceTests>>
{
    private readonly DatabaseFixture<TimelineBackfillServiceTests> _fixture;
    private readonly TimelineEventRepository _timelineRepo;
    private readonly Mock<IAiCategorizationService> _ai = new();
    private readonly List<string> _log = [];
    private readonly string _collection = $"bf-{Guid.NewGuid():N}";

    public TimelineBackfillServiceTests(DatabaseFixture<TimelineBackfillServiceTests> fixture)
    {
        _fixture = fixture;
        _timelineRepo = new TimelineEventRepository(fixture.Connection);
    }

    private TimelineBackfillService CreateSut() => new(
        _timelineRepo,
        _ai.Object,
        Options.Create(new AiCategorizationOptions { TimelineBackfillDelaySeconds = 0 }),
        NullLogger<TimelineBackfillService>.Instance);

    private Task LogAsync(string line)
    {
        _log.Add(line);
        return Task.CompletedTask;
    }

    private async Task SeedAsync(string slug, long dateEpoch = 1_750_000_000, string? collection = null, string excerpt = "")
    {
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, @Collection, @Title, 'Stored text of the article', @Excerpt, @DateEpoch, 'ai',
                    @Url, 'Author', 'Feed', '', 'hash')
            """,
            new { Slug = slug, Collection = collection ?? _collection, Title = $"Title {slug}", Excerpt = excerpt, DateEpoch = dateEpoch, Url = $"https://example.com/{collection ?? _collection}/{slug}" });
    }

    private static TimelineEvent Event(string kind = "ga") => new()
    {
        Kind = kind,
        Product = "Product",
        Description = $"{kind} happens",
        Date = new DateOnly(2026, 3, 10),
        DatePrecision = "day",
        DateSource = "explicit"
    };

    private void SetupAi(params TimelineEvent[] events) =>
        _ai.Setup(s => s.ExtractTimelineEventsAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TimelineExtractionResult { Events = events });

    private async Task<int> ExtractionCountAsync() => await _fixture.Connection.ExecuteScalarAsync<int>(
        "SELECT COUNT(*) FROM content_timeline_extractions WHERE collection_name = @Collection", new { Collection = _collection });

    [Fact]
    public async Task RunAsync_ScansUnscannedArticlesAndStoresEventsFromStoredContent()
    {
        await SeedAsync("a1");
        await SeedAsync("a2");
        SetupAi(Event());

        var result = await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        result.Candidates.Should().Be(2);
        result.Scanned.Should().Be(2);
        result.WithEvents.Should().Be(2);
        result.EventsStored.Should().Be(2);
        result.Failed.Should().Be(0);
        var stored = await _timelineRepo.GetEventsForItemAsync(_collection, "a1", TestContext.Current.CancellationToken);
        stored.Should().ContainSingle().Which.ExtractedFrom.Should().Be("stored-content");
    }

    [Fact]
    public async Task RunAsync_PassesTheStoredTextAndPublicationDateToTheAi()
    {
        await SeedAsync("a1", dateEpoch: 1_750_000_000);
        RawFeedItem? sent = null;
        _ai.Setup(s => s.ExtractTimelineEventsAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .Callback<RawFeedItem, CancellationToken>((raw, _) => sent = raw)
            .ReturnsAsync(new TimelineExtractionResult());

        await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        sent.Should().NotBeNull();
        sent!.FullContent.Should().Be("Stored text of the article");
        sent.PublishedAt.ToUnixTimeSeconds().Should().Be(1_750_000_000);
        sent.IsInteractive.Should().BeFalse("the backfill runs in the background and may use Flex");
    }

    [Fact]
    public async Task RunAsync_IncludesTheStoredExcerptBeforeTheStoredText()
    {
        await SeedAsync("a1", excerpt: "General availability starts on 1 June 2026.");
        RawFeedItem? sent = null;
        _ai.Setup(s => s.ExtractTimelineEventsAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .Callback<RawFeedItem, CancellationToken>((raw, _) => sent = raw)
            .ReturnsAsync(new TimelineExtractionResult());

        await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        sent.Should().NotBeNull();
        sent!.FullContent.Should().Be("General availability starts on 1 June 2026.\n\nStored text of the article");
    }

    [Fact]
    public async Task RunAsync_ArticleEditedDuringExtraction_DiscardsTheStaleResultAndLeavesItUnscanned()
    {
        await SeedAsync("a1");
        var edited = false;
        _ai.Setup(s => s.ExtractTimelineEventsAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .Callback<RawFeedItem, CancellationToken>((_, _) =>
            {
                // Simulates an admin applying a transcript while the extraction of the old text is running
                if (!edited)
                {
                    edited = true;
                    _fixture.Connection.Execute(
                        "UPDATE content_items SET content = 'New text from a transcript', updated_at = clock_timestamp() WHERE collection_name = @Collection AND slug = 'a1'",
                        new { Collection = _collection });
                }
            })
            .ReturnsAsync(new TimelineExtractionResult { Events = [Event("preview")] });

        var first = await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        first.Scanned.Should().Be(0);
        first.EventsStored.Should().Be(0);
        (await _timelineRepo.GetEventsForItemAsync(_collection, "a1", TestContext.Current.CancellationToken)).Should().BeEmpty();
        (await ExtractionCountAsync()).Should().Be(0, "a stale result must not be recorded as a current-version scan");
        _log.Should().Contain(l => l.StartsWith("SKIPPED", StringComparison.Ordinal) && l.Contains("a1", StringComparison.Ordinal));

        var second = await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        second.Scanned.Should().Be(1);
        (await _timelineRepo.GetEventsForItemAsync(_collection, "a1", TestContext.Current.CancellationToken)).Should().ContainSingle();
    }

    [Fact]
    public async Task RunAsync_ArticleWithoutEvents_IsStillMarkedAsScanned()
    {
        await SeedAsync("a1");
        SetupAi();

        var result = await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        result.Scanned.Should().Be(1);
        result.WithEvents.Should().Be(0);
        (await ExtractionCountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task RunAsync_SecondRun_SkipsAlreadyScannedArticles()
    {
        await SeedAsync("a1");
        SetupAi(Event());
        var request = new TimelineBackfillRequest { Collection = _collection };
        await CreateSut().RunAsync(request, LogAsync, TestContext.Current.CancellationToken);
        _ai.Invocations.Clear();

        var second = await CreateSut().RunAsync(request, LogAsync, TestContext.Current.CancellationToken);

        second.Candidates.Should().Be(0);
        _ai.Verify(s => s.ExtractTimelineEventsAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RunAsync_WithForce_ScansAgainAndReplacesTheEvents()
    {
        await SeedAsync("a1");
        SetupAi(Event("preview"));
        await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);
        SetupAi(Event("retirement"));

        var result = await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection, Force = true }, LogAsync, TestContext.Current.CancellationToken);

        result.Scanned.Should().Be(1);
        (await _timelineRepo.GetEventsForItemAsync(_collection, "a1", TestContext.Current.CancellationToken))
            .Should().ContainSingle().Which.Kind.Should().Be("retirement");
    }

    [Fact]
    public async Task RunAsync_ArticleScannedWithOlderPromptVersion_IsScannedAgain()
    {
        await SeedAsync("a1");
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_timeline_extractions (collection_name, item_slug, prompt_version, events_found, extracted_from)
            VALUES (@Collection, 'a1', @Version, 0, 'stored-content')
            """, new { Collection = _collection, Version = ITimelineEventRepository.CurrentPromptVersion - 1 });
        SetupAi(Event());

        var result = await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        result.Scanned.Should().Be(1);
    }

    [Fact]
    public async Task RunAsync_ArticleExtractedWhileProcessing_IsNotScannedAgain()
    {
        await SeedAsync("a1");
        await _timelineRepo.ReplaceEventsAsync(_collection, "a1", [Event()], ITimelineEventRepository.ExtractedFromSource, ITimelineEventRepository.CurrentPromptVersion);
        SetupAi(Event("retirement"));

        var result = await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        result.Candidates.Should().Be(0);
        (await _timelineRepo.GetEventsForItemAsync(_collection, "a1", TestContext.Current.CancellationToken))
            .Should().ContainSingle().Which.Kind.Should().Be("ga");
    }

    [Fact]
    public async Task RunAsync_FiltersByPublishedAfterAndLimitsNewestFirst()
    {
        await SeedAsync("old", dateEpoch: 1_600_000_000);        // 2020
        await SeedAsync("mid", dateEpoch: 1_740_000_000);        // 2025
        await SeedAsync("new", dateEpoch: 1_760_000_000);        // 2025
        SetupAi();
        var titles = new List<string>();
        _ai.Setup(s => s.ExtractTimelineEventsAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .Callback<RawFeedItem, CancellationToken>((raw, _) => titles.Add(raw.Title))
            .ReturnsAsync(new TimelineExtractionResult());

        var result = await CreateSut().RunAsync(
            new TimelineBackfillRequest { Collection = _collection, PublishedAfter = new DateOnly(2024, 1, 1), Limit = 1 },
            LogAsync, TestContext.Current.CancellationToken);

        result.Candidates.Should().Be(1);
        titles.Should().Equal("Title new");
    }

    [Fact]
    public async Task RunAsync_OnlyScansTheRequestedCollection()
    {
        await SeedAsync("a1");
        await SeedAsync("other", collection: $"{_collection}-other");
        SetupAi();

        var result = await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        result.Candidates.Should().Be(1);
    }

    [Fact]
    public async Task RunAsync_NeverScansRoundups()
    {
        await SeedAsync("roundup-1", collection: "roundups");
        SetupAi();

        var candidates = await _timelineRepo.GetBackfillCandidatesAsync(
            new TimelineBackfillRequest { Collection = "roundups" }, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken);

        candidates.Should().BeEmpty();
        await _fixture.Connection.ExecuteAsync("DELETE FROM content_items WHERE collection_name = 'roundups' AND slug = 'roundup-1'");
    }

    [Fact]
    public async Task RunAsync_FailedExtraction_LeavesArticleUnscannedAndContinuesWithTheRest()
    {
        await SeedAsync("bad", dateEpoch: 1_760_000_000);
        await SeedAsync("good", dateEpoch: 1_750_000_000);
        _ai.Setup(s => s.ExtractTimelineEventsAsync(It.Is<RawFeedItem>(r => r.Title == "Title bad"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TimelineExtractionResult { IsFailure = true, Explanation = "content filter" });
        _ai.Setup(s => s.ExtractTimelineEventsAsync(It.Is<RawFeedItem>(r => r.Title == "Title good"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TimelineExtractionResult { Events = [Event()] });

        var result = await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        result.Failed.Should().Be(1);
        result.Scanned.Should().Be(1);
        _log.Should().Contain(l => l.Contains("FAILED", StringComparison.Ordinal) && l.Contains("bad", StringComparison.Ordinal) && l.Contains("content filter", StringComparison.Ordinal));

        // The failed article is picked up by the next run.
        var retry = await _timelineRepo.GetBackfillCandidatesAsync(new TimelineBackfillRequest { Collection = _collection }, ITimelineEventRepository.CurrentPromptVersion, TestContext.Current.CancellationToken);
        retry.Should().ContainSingle().Which.Slug.Should().Be("bad");
    }

    [Fact]
    public async Task RunAsync_WhenTheAiThrows_CountsItAsFailedAndContinues()
    {
        await SeedAsync("a1", dateEpoch: 1_760_000_000);
        await SeedAsync("a2", dateEpoch: 1_750_000_000);
        var calls = 0;
        _ai.Setup(s => s.ExtractTimelineEventsAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .Returns(() => ++calls == 1
                ? throw new HttpRequestException("boom")
                : Task.FromResult(new TimelineExtractionResult()));

        var result = await CreateSut().RunAsync(new TimelineBackfillRequest { Collection = _collection }, LogAsync, TestContext.Current.CancellationToken);

        result.Failed.Should().Be(1);
        result.Scanned.Should().Be(1);
    }

    [Fact]
    public async Task RunAsync_WhenCancelled_StopsAndTheNextRunResumesWithTheRest()
    {
        await SeedAsync("a1", dateEpoch: 1_760_000_000);
        await SeedAsync("a2", dateEpoch: 1_750_000_000);
        await SeedAsync("a3", dateEpoch: 1_740_000_000);
        using var cts = new CancellationTokenSource();
        var calls = 0;
        _ai.Setup(s => s.ExtractTimelineEventsAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                if (++calls == 2)
                {
                    cts.Cancel();
                }

                return Task.FromResult(new TimelineExtractionResult());
            });
        var request = new TimelineBackfillRequest { Collection = _collection };

        var act = () => CreateSut().RunAsync(request, LogAsync, cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();

        // The article being scanned when the cancellation arrived is not stored; the next run picks it up.
        (await ExtractionCountAsync()).Should().Be(1);

        var resumed = await CreateSut().RunAsync(request, LogAsync, TestContext.Current.CancellationToken);
        resumed.Candidates.Should().Be(2);
        (await ExtractionCountAsync()).Should().Be(3);
    }
}
