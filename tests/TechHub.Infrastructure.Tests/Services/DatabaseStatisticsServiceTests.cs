using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Integration tests for <see cref="DatabaseStatisticsService"/>.
/// Uses PostgreSQL via Testcontainers to verify statistical queries return valid data.
/// </summary>
public class DatabaseStatisticsServiceTests
    : IClassFixture<DatabaseFixture<DatabaseStatisticsServiceTests>>
{
    private readonly DatabaseStatisticsService _service;
    private readonly DatabaseFixture<DatabaseStatisticsServiceTests> _fixture;

    public DatabaseStatisticsServiceTests(DatabaseFixture<DatabaseStatisticsServiceTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
        _service = new DatabaseStatisticsService(
            fixture.Connection,
            NullLogger<DatabaseStatisticsService>.Instance);
    }

    [Fact]
    public async Task GetStatisticsAsync_ReturnsValidStatistics()
    {
        // Act
        var stats = await _service.GetStatisticsAsync(CancellationToken.None);

        // Assert
        stats.Should().NotBeNull();
        stats.TotalContentItems.Should().BeGreaterThanOrEqualTo(0);
        stats.ItemsByCollection.Should().NotBeNull();
        stats.ItemsBySection.Should().NotBeNull();
        stats.TotalUniqueTags.Should().BeGreaterThanOrEqualTo(0);
        stats.LatestItems.Should().NotBeNull();
        stats.DatabaseSize.Should().NotBeNull();
        stats.TableSizes.Should().NotBeNull();
        stats.GeneratedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task GetStatisticsAsync_DatabaseSize_HasExpectedFields()
    {
        // Act
        var stats = await _service.GetStatisticsAsync(CancellationToken.None);

        // Assert
        stats.DatabaseSize.DatabaseName.Should().NotBeNullOrEmpty();
        stats.DatabaseSize.TotalSize.Should().NotBeNullOrEmpty();
        stats.DatabaseSize.DataSize.Should().NotBeNullOrEmpty();
        stats.DatabaseSize.IndexSize.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetStatisticsAsync_TableSizes_ContainsKnownTables()
    {
        // Act
        var stats = await _service.GetStatisticsAsync(CancellationToken.None);

        // Assert — migration tables should exist
        stats.TableSizes.Should().Contain(t => t.TableName == "content_items");
        stats.TableSizes.Should().Contain(t => t.TableName == "content_processing_jobs");
    }

    [Fact]
    public async Task GetStatisticsAsync_Processing_ReturnsValidCounts()
    {
        // Arrange — insert a test job so we have at least one
        await _fixture.Connection.ExecuteAsync(
            "INSERT INTO content_processing_jobs (started_at, status, trigger_type) VALUES (NOW(), 'completed', 'manual')");

        // Act
        var stats = await _service.GetStatisticsAsync(CancellationToken.None);

        // Assert
        stats.Processing.Should().NotBeNull();
        stats.Processing.TotalJobs.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task GetStatisticsAsync_AfterInsertingContent_ReflectsCount()
    {
        // Arrange — insert a content item
        await _fixture.Connection.ExecuteAsync("""
            INSERT INTO content_items (slug, collection_name, title, content, excerpt, date_epoch, primary_section_name, external_url, author, feed_name, tags_csv, sections_bitmask, content_hash)
            VALUES ('test-stats-item', 'blogs', 'Stats Test', '', 'An excerpt', 1711699200, 'ai', 'https://example.com/stats-test', 'Test', 'Test Feed', '', 1, 'hash123')
            ON CONFLICT (collection_name, slug) DO NOTHING
            """);

        // Act
        var stats = await _service.GetStatisticsAsync(CancellationToken.None);

        // Assert
        stats.TotalContentItems.Should().BeGreaterThanOrEqualTo(1);
        stats.ItemsByCollection.Should().Contain(c => c.CollectionName == "blogs");
    }
}
