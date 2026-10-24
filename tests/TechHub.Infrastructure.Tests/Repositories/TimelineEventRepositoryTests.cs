using Dapper;
using FluentAssertions;
using Npgsql;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="TimelineEventRepository"/> (content_timeline_events and content_timeline_extractions).
/// Each test uses a unique slug to avoid cross-test contamination in the shared database.
/// </summary>
public class TimelineEventRepositoryTests : IClassFixture<DatabaseFixture<TimelineEventRepositoryTests>>
{
    private readonly DatabaseFixture<TimelineEventRepositoryTests> _fixture;
    private readonly TimelineEventRepository _sut;

    public TimelineEventRepositoryTests(DatabaseFixture<TimelineEventRepositoryTests> fixture)
    {
        _fixture = fixture;
        _sut = new TimelineEventRepository(fixture.Connection);
    }

    private async Task<string> SeedItemAsync()
    {
        var slug = $"timeline-{Guid.NewGuid():N}";
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items
                (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name,
                 external_url, author, feed_name, tags_csv, content_hash)
            VALUES (@Slug, 'news', 'Timeline test', '', '', 1743278400, 'ai',
                    @Url, 'Author', 'Feed', '', 'hash')
            """,
            new { Slug = slug, Url = $"https://example.com/{slug}" });
        return slug;
    }

    private static TimelineEvent Event(
        string kind = "ga",
        string product = "GitHub Copilot",
        string? feature = "coding agent",
        string date = "2026-03-10",
        string precision = "day",
        string source = "explicit",
        string? quote = "It is now GA.") => new()
        {
            Kind = kind,
            Product = product,
            Feature = feature,
            Description = $"{kind} of {feature ?? product}",
            Date = DateOnly.Parse(date, System.Globalization.CultureInfo.InvariantCulture),
            DatePrecision = precision,
            DateSource = source,
            Quote = quote
        };

    [Fact]
    public async Task ReplaceEventsAsync_StoresAllFieldsAndRecordsExtraction()
    {
        var slug = await SeedItemAsync();

        await _sut.ReplaceEventsAsync("news", slug,
            [Event(), Event(kind: "retirement", feature: null, date: "2026-12-01", precision: "month", source: "explicit", quote: null)],
            ITimelineEventRepository.ExtractedFromSource, promptVersion: 3);

        var stored = await _sut.GetEventsForItemAsync("news", slug);
        stored.Should().HaveCount(2);
        var ga = stored[0];
        ga.Kind.Should().Be("ga");
        ga.Product.Should().Be("GitHub Copilot");
        ga.Feature.Should().Be("coding agent");
        ga.EventDate.Should().Be(new DateOnly(2026, 3, 10));
        ga.DatePrecision.Should().Be("day");
        ga.DateSource.Should().Be("explicit");
        ga.Quote.Should().Be("It is now GA.");
        ga.ExtractedFrom.Should().Be("source");
        stored[1].Feature.Should().BeNull();
        stored[1].Quote.Should().BeNull();
        stored[1].DatePrecision.Should().Be("month");

        var extraction = await _fixture.Connection.QuerySingleAsync<(int PromptVersion, int EventsFound)>(
            "SELECT prompt_version AS PromptVersion, events_found AS EventsFound FROM content_timeline_extractions WHERE item_slug = @Slug",
            new { Slug = slug });
        extraction.PromptVersion.Should().Be(3);
        extraction.EventsFound.Should().Be(2);
    }

    [Fact]
    public async Task ReplaceEventsAsync_CalledAgain_ReplacesTheEvents()
    {
        var slug = await SeedItemAsync();
        await _sut.ReplaceEventsAsync("news", slug, [Event(), Event(kind: "preview")], "source", 1);

        await _sut.ReplaceEventsAsync("news", slug, [Event(kind: "launch")], "stored-content", 2);

        var stored = await _sut.GetEventsForItemAsync("news", slug);
        stored.Should().ContainSingle().Which.Kind.Should().Be("launch");
        stored[0].ExtractedFrom.Should().Be("stored-content");
        var count = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM content_timeline_extractions WHERE item_slug = @Slug", new { Slug = slug });
        count.Should().Be(1);
    }

    [Fact]
    public async Task ReplaceEventsAsync_WithNoEvents_RemovesOldEventsAndRecordsZeroFound()
    {
        var slug = await SeedItemAsync();
        await _sut.ReplaceEventsAsync("news", slug, [Event()], "source", 1);

        await _sut.ReplaceEventsAsync("news", slug, [], "source", 1);

        (await _sut.GetEventsForItemAsync("news", slug)).Should().BeEmpty();
        var found = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT events_found FROM content_timeline_extractions WHERE item_slug = @Slug", new { Slug = slug });
        found.Should().Be(0);
    }

    [Fact]
    public async Task ReplaceEventsAsync_OnlyAffectsTheGivenItem()
    {
        var first = await SeedItemAsync();
        var second = await SeedItemAsync();
        await _sut.ReplaceEventsAsync("news", first, [Event()], "source", 1);
        await _sut.ReplaceEventsAsync("news", second, [Event(kind: "preview")], "source", 1);

        await _sut.ReplaceEventsAsync("news", first, [], "source", 1);

        (await _sut.GetEventsForItemAsync("news", second)).Should().ContainSingle();
    }

    [Fact]
    public async Task ReplaceEventsAsync_WhileAnotherTransactionHoldsTheArticle_WaitsForIt()
    {
        var slug = await SeedItemAsync();
        await using var other = await _fixture.DataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        await using var otherTransaction = await other.BeginTransactionAsync(TestContext.Current.CancellationToken);
        await other.ExecuteAsync(
            "SELECT 1 FROM content_items WHERE collection_name = 'news' AND slug = @Slug FOR NO KEY UPDATE",
            new { Slug = slug }, otherTransaction);

        await using var connection = await _fixture.DataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        var replace = new TimelineEventRepository(connection)
            .ReplaceEventsAsync("news", slug, [Event()], "source", 1, TestContext.Current.CancellationToken);

        await Task.Delay(500, TestContext.Current.CancellationToken);
        replace.IsCompleted.Should().BeFalse("replacements of the same article must run one after the other");

        await otherTransaction.CommitAsync(TestContext.Current.CancellationToken);
        await replace;
        (await _sut.GetEventsForItemAsync("news", slug, TestContext.Current.CancellationToken)).Should().ContainSingle();
    }

    [Fact]
    public async Task ReplaceEventsAsync_ConcurrentReplacements_LeaveExactlyOneEventSet()
    {
        var slug = await SeedItemAsync();

        var replacements = Enumerable.Range(0, 8).Select(async i =>
        {
            await using var connection = await _fixture.DataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
            await new TimelineEventRepository(connection).ReplaceEventsAsync(
                "news", slug, [Event(kind: "ga", feature: $"first {i}"), Event(kind: "preview", feature: $"second {i}")],
                "source", 1, TestContext.Current.CancellationToken);
        });
        await Task.WhenAll(replacements);

        var stored = await _sut.GetEventsForItemAsync("news", slug, TestContext.Current.CancellationToken);
        stored.Should().HaveCount(2);
        stored.Select(e => e.Feature!.Split(' ')[1]).Distinct().Should().ContainSingle("both events must come from the same replacement");
        var found = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT events_found FROM content_timeline_extractions WHERE item_slug = @Slug", new { Slug = slug });
        found.Should().Be(stored.Count);
    }

    [Fact]
    public async Task ReplaceEventsAsync_ForUnknownItem_ThrowsAndStoresNothing()
    {
        var slug = $"missing-{Guid.NewGuid():N}";

        var act = () => _sut.ReplaceEventsAsync("news", slug, [Event()], "source", 1);

        await act.Should().ThrowAsync<PostgresException>();
        (await _sut.GetEventsForItemAsync("news", slug)).Should().BeEmpty();
    }

    [Fact]
    public async Task DeletingTheItem_DeletesItsEventsAndExtraction()
    {
        var slug = await SeedItemAsync();
        await _sut.ReplaceEventsAsync("news", slug, [Event()], "source", 1);

        await _fixture.Connection.ExecuteAsync("DELETE FROM content_items WHERE slug = @Slug", new { Slug = slug });

        (await _sut.GetEventsForItemAsync("news", slug)).Should().BeEmpty();
        var extractions = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM content_timeline_extractions WHERE item_slug = @Slug", new { Slug = slug });
        extractions.Should().Be(0);
    }
}
