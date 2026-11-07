using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="RssFeedConfigRepository"/>.
/// Uses PostgreSQL via Testcontainers to verify CRUD operations and column mapping.
/// </summary>
public class RssFeedConfigRepositoryTests
    : IClassFixture<DatabaseFixture<RssFeedConfigRepositoryTests>>
{
    private readonly RssFeedConfigRepository _repository;
    private readonly DatabaseFixture<RssFeedConfigRepositoryTests> _fixture;

    public RssFeedConfigRepositoryTests(DatabaseFixture<RssFeedConfigRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
        _repository = new RssFeedConfigRepository(
            fixture.Connection,
            NullLogger<RssFeedConfigRepository>.Instance);
    }

    // ── CreateAsync & GetByIdAsync ─────────────────────────────────────────

    [Fact]
    public async Task PollingInterval_CreateUpdateAndRead_PersistsAcrossAllQueries()
    {
        // Arrange
        var feed = new FeedConfig { Name = "Polling feed", Url = "https://example.com/polling", OutputDir = "_community" };
        feed.Id = await _repository.CreateAsync(feed, CancellationToken.None);
        (await _repository.GetByIdAsync(feed.Id, CancellationToken.None))!.PollingIntervalMinutes.Should().Be(60);
        feed.PollingIntervalMinutes = 15;
        await _repository.UpdateAsync(feed, CancellationToken.None);
        await _repository.RecordCheckAsync(feed.Id, "HTTP 503", CancellationToken.None);

        // Act
        var byId = await _repository.GetByIdAsync(feed.Id, CancellationToken.None);
        var all = await _repository.GetAllAsync(CancellationToken.None);
        var enabled = await _repository.GetEnabledAsync(CancellationToken.None);

        // Assert
        byId!.PollingIntervalMinutes.Should().Be(15);
        all.Single(f => f.Id == feed.Id).PollingIntervalMinutes.Should().Be(15);
        var active = enabled.Single(f => f.Id == feed.Id);
        active.PollingIntervalMinutes.Should().Be(15);
        active.LastCheckedAt.Should().Be(byId.LastCheckedAt);
        active.LastCheckError.Should().Be("HTTP 503");
    }

    [Theory]
    [InlineData("https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure", 15)]
    [InlineData("https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=AzureDatabases", 15)]
    [InlineData("https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/board?board.id=copilot-studio", 60)]
    [InlineData("https://devblogs.microsoft.com/feed", 60)]
    [InlineData("https://devblogs.microsoft.com/aspire/feed/", 60)]
    public async Task Migration_ConfiguredFeeds_HaveExpectedPollingInterval(string url, int expected)
    {
        // Act
        var interval = await _fixture.Connection.ExecuteScalarAsync<int>(
            "SELECT polling_interval_minutes FROM rss_feed_configs WHERE url = @Url", new { Url = url });

        // Assert
        interval.Should().Be(expected);
    }

    [Fact]
    public async Task CreateAsync_ReturnsPositiveId()
    {
        // Arrange
        var feed = new FeedConfig { Name = "CRUD Test Feed", Url = "https://example.com/create-test.xml", OutputDir = "_blogs", Enabled = true };

        // Act
        var id = await _repository.CreateAsync(feed, CancellationToken.None);

        // Assert
        id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdAsync_AfterCreate_ReturnsCorrectData()
    {
        // Arrange
        var feed = new FeedConfig { Name = "Get Feed", Url = "https://example.com/get-test.xml", OutputDir = "_news", Enabled = false };
        var id = await _repository.CreateAsync(feed, CancellationToken.None);

        // Act
        var result = await _repository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Name.Should().Be("Get Feed");
        result.Url.Should().Be("https://example.com/get-test.xml");
        result.OutputDir.Should().Be("_news");
        result.Enabled.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999999, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    // ── GetEnabledAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetEnabledAsync_ReturnsOnlyEnabledFeeds()
    {
        // Arrange
        var enabled = new FeedConfig { Name = "Enabled Feed", Url = "https://example.com/enabled.xml", OutputDir = "_blogs", Enabled = true };
        var disabled = new FeedConfig { Name = "Disabled Feed", Url = "https://example.com/disabled.xml", OutputDir = "_blogs", Enabled = false };
        await _repository.CreateAsync(enabled, CancellationToken.None);
        await _repository.CreateAsync(disabled, CancellationToken.None);

        // Act
        var result = await _repository.GetEnabledAsync(CancellationToken.None);

        // Assert
        result.Should().Contain(f => f.Name == "Enabled Feed");
        result.Should().NotContain(f => f.Name == "Disabled Feed");
    }

    // ── GetAllAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_ReturnsBothEnabledAndDisabledFeeds()
    {
        // Arrange
        var enabled = new FeedConfig { Name = "All-Enabled", Url = "https://example.com/all-e.xml", OutputDir = "_blogs", Enabled = true };
        var disabled = new FeedConfig { Name = "All-Disabled", Url = "https://example.com/all-d.xml", OutputDir = "_blogs", Enabled = false };
        await _repository.CreateAsync(enabled, CancellationToken.None);
        await _repository.CreateAsync(disabled, CancellationToken.None);

        // Act
        var result = await _repository.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().Contain(f => f.Name == "All-Enabled");
        result.Should().Contain(f => f.Name == "All-Disabled");
    }

    // ── UpdateAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_ChangesAllFields()
    {
        // Arrange
        var feed = new FeedConfig { Name = "Original", Url = "https://example.com/original.xml", OutputDir = "_blogs", Enabled = true };
        var id = await _repository.CreateAsync(feed, CancellationToken.None);

        var updated = new FeedConfig { Id = id, Name = "Updated", Url = "https://example.com/updated.xml", OutputDir = "_news", Enabled = false };

        // Act
        var success = await _repository.UpdateAsync(updated, CancellationToken.None);
        var result = await _repository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated");
        result.Url.Should().Be("https://example.com/updated.xml");
        result.OutputDir.Should().Be("_news");
        result.Enabled.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_NonExistentId_ReturnsFalse()
    {
        // Arrange
        var feed = new FeedConfig { Id = 999999, Name = "Ghost", Url = "https://example.com/ghost.xml", OutputDir = "_blogs", Enabled = true };

        // Act
        var success = await _repository.UpdateAsync(feed, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
    }

    // ── DeleteAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_RemovesFeed()
    {
        // Arrange
        var feed = new FeedConfig { Name = "Delete Me", Url = "https://example.com/delete.xml", OutputDir = "_blogs", Enabled = true };
        var id = await _repository.CreateAsync(feed, CancellationToken.None);

        // Act
        var success = await _repository.DeleteAsync(id, CancellationToken.None);
        var result = await _repository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        success.Should().BeTrue();
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WithoutContent_HasContentIsFalse()
    {
        // Arrange
        var id = await _repository.CreateAsync(
            new FeedConfig { Name = "Empty Feed", Url = "https://example.com/empty.xml", OutputDir = "_blogs", Enabled = true },
            CancellationToken.None);

        // Act
        var result = await _repository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result!.HasContent.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAndGetAll_WithContentItem_HasContentIsTrue()
    {
        // Arrange
        const string FeedName = "Feed With Items";
        var id = await _repository.CreateAsync(
            new FeedConfig { Name = FeedName, Url = "https://example.com/items.xml", OutputDir = "_blogs", Enabled = true },
            CancellationToken.None);
        await _fixture.Connection.ExecuteAsync(@"
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, external_url, author, feed_name, tags_csv, content_hash)
            VALUES ('feed-with-items', 'blogs', 'Item', '', '', 0, 'ai', 'https://example.com/feed-with-items', '', @FeedName, '', 'hash-feed-items')",
            new { FeedName });

        // Act
        var byId = await _repository.GetByIdAsync(id, CancellationToken.None);
        var all = await _repository.GetAllAsync(CancellationToken.None);

        // Assert
        byId!.HasContent.Should().BeTrue();
        all.Single(f => f.Id == id).HasContent.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_WithOnlyProcessedUrl_HasContentIsTrue()
    {
        // Arrange — skipped/failed URLs have no content item but still block deletion
        const string FeedName = "Feed With Processed Url";
        var id = await _repository.CreateAsync(
            new FeedConfig { Name = FeedName, Url = "https://example.com/processed.xml", OutputDir = "_blogs", Enabled = true },
            CancellationToken.None);
        await _fixture.Connection.ExecuteAsync(@"
            INSERT INTO processed_urls (external_url, status, feed_name, collection_name)
            VALUES ('https://example.com/feed-processed-only', 'failed', @FeedName, 'blogs')",
            new { FeedName });

        // Act
        var result = await _repository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result!.HasContent.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_NeverCheckedAndNoContent_HasNoCheckOrContentDate()
    {
        // Arrange
        var id = await _repository.CreateAsync(
            new FeedConfig { Name = "Unchecked Feed", Url = "https://example.com/unchecked.xml", OutputDir = "_blogs", Enabled = true },
            CancellationToken.None);

        // Act
        var result = await _repository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result!.LastCheckedAt.Should().BeNull();
        result.LastCheckError.Should().BeNull();
        result.LastContentAt.Should().BeNull();
    }

    [Fact]
    public async Task RecordCheckAsync_WithError_StoresTimeAndError()
    {
        // Arrange
        var id = await _repository.CreateAsync(
            new FeedConfig { Name = "Broken Check Feed", Url = "https://example.com/broken-check.xml", OutputDir = "_blogs", Enabled = true },
            CancellationToken.None);
        var before = DateTimeOffset.UtcNow.AddMinutes(-1);

        // Act
        var found = await _repository.RecordCheckAsync(id, "Failed to download feed: HTTP 404 (NotFound)", CancellationToken.None);
        var result = await _repository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        found.Should().BeTrue();
        result!.LastCheckedAt.Should().BeAfter(before);
        result.LastCheckError.Should().Be("Failed to download feed: HTTP 404 (NotFound)");
    }

    [Fact]
    public async Task RecordCheckAsync_AfterFailure_SuccessClearsTheError()
    {
        // Arrange
        var id = await _repository.CreateAsync(
            new FeedConfig { Name = "Recovered Feed", Url = "https://example.com/recovered.xml", OutputDir = "_blogs", Enabled = true },
            CancellationToken.None);
        await _repository.RecordCheckAsync(id, "HTTP 404 (NotFound)", CancellationToken.None);

        // Act
        await _repository.RecordCheckAsync(id, null, CancellationToken.None);
        var result = await _repository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result!.LastCheckedAt.Should().NotBeNull();
        result.LastCheckError.Should().BeNull();
    }

    [Fact]
    public async Task RecordCheckAsync_NonExistentId_ReturnsFalse()
    {
        // Act
        var found = await _repository.RecordCheckAsync(999999, null, CancellationToken.None);

        // Assert
        found.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_KeepsTheRecordedCheck()
    {
        // Arrange
        var id = await _repository.CreateAsync(
            new FeedConfig { Name = "Edited Check Feed", Url = "https://example.com/edited-check.xml", OutputDir = "_blogs", Enabled = true },
            CancellationToken.None);
        await _repository.RecordCheckAsync(id, "HTTP 404 (NotFound)", CancellationToken.None);

        // Act
        await _repository.UpdateAsync(
            new FeedConfig { Id = id, Name = "Edited Check Feed", Url = "https://example.com/edited-check.xml", OutputDir = "_news", Enabled = true },
            CancellationToken.None);
        var result = await _repository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result!.LastCheckError.Should().Be("HTTP 404 (NotFound)");
    }

    [Fact]
    public async Task GetByIdAndGetAll_WithContentItems_LastContentAtIsTheNewestCreatedAt()
    {
        // Arrange
        const string FeedName = "Feed With Dated Items";
        var id = await _repository.CreateAsync(
            new FeedConfig { Name = FeedName, Url = "https://example.com/dated.xml", OutputDir = "_blogs", Enabled = true },
            CancellationToken.None);
        await _fixture.Connection.ExecuteAsync(@"
            INSERT INTO content_items (slug, collection_name, title, excerpt, content, date_epoch,
                primary_section_name, external_url, author, feed_name, tags_csv, content_hash, created_at)
            VALUES ('dated-old', 'blogs', 'Old', '', '', 0, 'ai', 'https://example.com/dated-old', '', @FeedName, '', 'hash-dated-old', '2026-01-01T10:00:00Z'),
                   ('dated-new', 'blogs', 'New', '', '', 0, 'ai', 'https://example.com/dated-new', '', @FeedName, '', 'hash-dated-new', '2026-03-05T12:30:00Z')",
            new { FeedName });

        // Act
        var byId = await _repository.GetByIdAsync(id, CancellationToken.None);
        var all = await _repository.GetAllAsync(CancellationToken.None);

        // Assert
        var expected = new DateTimeOffset(2026, 3, 5, 12, 30, 0, TimeSpan.Zero);
        byId!.LastContentAt.Should().Be(expected);
        all.Single(f => f.Id == id).LastContentAt.Should().Be(expected);
    }

    [Fact]
    public async Task FeedNameLookups_HaveIndexes()
    {
        var indexes = (await _fixture.Connection.QueryAsync<string>(
            "SELECT indexdef FROM pg_indexes WHERE tablename IN ('content_items', 'processed_urls')")).ToList();

        indexes.Should().Contain(i => i.Contains("ON public.content_items", StringComparison.Ordinal) && i.Contains("(feed_name, created_at)", StringComparison.Ordinal));
        indexes.Should().Contain(i => i.Contains("ON public.processed_urls", StringComparison.Ordinal) && i.Contains("(feed_name)", StringComparison.Ordinal));
    }

    [Fact]
    public async Task DeleteAsync_NonExistentId_ReturnsFalse()
    {
        // Act
        var success = await _repository.DeleteAsync(999999, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
    }
}
