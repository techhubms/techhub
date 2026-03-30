using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="RssFeedConfigRepository"/>.
/// Uses PostgreSQL via Testcontainers to verify CRUD operations and column mapping.
/// </summary>
public class RssFeedConfigRepositoryTests
    : IClassFixture<DatabaseFixture<RssFeedConfigRepositoryTests>>
{
    private readonly RssFeedConfigRepository _repository;

    public RssFeedConfigRepositoryTests(DatabaseFixture<RssFeedConfigRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _repository = new RssFeedConfigRepository(
            fixture.Connection,
            NullLogger<RssFeedConfigRepository>.Instance);
    }

    // ── CreateAsync & GetByIdAsync ─────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ReturnsPositiveId()
    {
        // Arrange
        var feed = new FeedConfig { Name = "Test Feed", Url = "https://example.com/create-test.xml", OutputDir = "_blogs", Enabled = true };

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
    public async Task DeleteAsync_NonExistentId_ReturnsFalse()
    {
        // Act
        var success = await _repository.DeleteAsync(999999, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
    }
}
