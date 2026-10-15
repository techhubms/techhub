using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests for <see cref="CustomPageDataRepository"/>.
/// Every test starts with an empty <c>custom_page_data</c> table (migrations seed it) so ordering and
/// emptiness checks are deterministic.
/// </summary>
public class CustomPageDataRepositoryTests
    : IClassFixture<DatabaseFixture<CustomPageDataRepositoryTests>>, IAsyncLifetime
{
    private readonly DatabaseFixture<CustomPageDataRepositoryTests> _fixture;
    private readonly CustomPageDataRepository _repository;

    public CustomPageDataRepositoryTests(DatabaseFixture<CustomPageDataRepositoryTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
        _repository = new CustomPageDataRepository(
            fixture.Connection,
            NullLogger<CustomPageDataRepository>.Instance);
    }

    public async ValueTask InitializeAsync() =>
        await _fixture.Connection.ExecuteAsync("DELETE FROM custom_page_data");

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    // ── UpsertAsync & GetByKeyAsync ────────────────────────────────────────

    [Fact]
    public async Task UpsertAsync_NewKey_InsertsEntry()
    {
        // Act
        await _repository.UpsertAsync("levels", "Levels page", """{"levels":[1,2,3]}""", TestContext.Current.CancellationToken);
        var entry = await _repository.GetByKeyAsync("levels", TestContext.Current.CancellationToken);

        // Assert
        entry.Should().NotBeNull();
        entry!.Key.Should().Be("levels");
        entry.Description.Should().Be("Levels page");
        entry.JsonData.Should().Be("""{"levels":[1,2,3]}""");
        entry.UpdatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(30));
    }

    [Fact]
    public async Task UpsertAsync_ExistingKey_ReplacesJsonAndRefreshesUpdatedAt()
    {
        // Arrange
        await _repository.UpsertAsync("handbook", "Handbook", """{"v":1}""", TestContext.Current.CancellationToken);
        await _fixture.Connection.ExecuteAsync(
            "UPDATE custom_page_data SET updated_at = NOW() - INTERVAL '2 days' WHERE key = 'handbook'");

        // Act
        await _repository.UpsertAsync("handbook", "Handbook", """{"v":2}""", TestContext.Current.CancellationToken);
        var entry = await _repository.GetByKeyAsync("handbook", TestContext.Current.CancellationToken);

        // Assert
        entry!.JsonData.Should().Be("""{"v":2}""");
        entry.UpdatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(30));
        (await _repository.GetAllAsync(TestContext.Current.CancellationToken)).Should().ContainSingle();
    }

    [Fact]
    public async Task UpsertAsync_ExistingKey_UpdatesDescription()
    {
        // Arrange
        await _repository.UpsertAsync("handbook", "Original description", "{}", TestContext.Current.CancellationToken);

        // Act
        await _repository.UpsertAsync("handbook", "Different description", """{"v":2}""", TestContext.Current.CancellationToken);
        var entry = await _repository.GetByKeyAsync("handbook", TestContext.Current.CancellationToken);

        // Assert — the conflict clause also replaces description
        entry!.Description.Should().Be("Different description");
    }

    [Fact]
    public async Task UpsertAsync_UnicodeQuotesAndLargePayload_RoundTripsUnchanged()
    {
        // Arrange
        var json = "{\"text\":\"é — 日本語 \\\"quoted\\\"\",\"padding\":\"" + new string('x', 200_000) + "\"}";

        // Act
        await _repository.UpsertAsync("big", "", json, TestContext.Current.CancellationToken);
        var entry = await _repository.GetByKeyAsync("big", TestContext.Current.CancellationToken);

        // Assert
        entry!.JsonData.Should().Be(json);
        entry.Description.Should().BeEmpty();
    }

    [Fact]
    public async Task UpsertAsync_MultipleKeys_KeepsEntriesIndependent()
    {
        // Act
        await _repository.UpsertAsync("one", "First", """{"n":1}""", TestContext.Current.CancellationToken);
        await _repository.UpsertAsync("two", "Second", """{"n":2}""", TestContext.Current.CancellationToken);
        await _repository.UpsertAsync("one", "First", """{"n":10}""", TestContext.Current.CancellationToken);

        // Assert
        (await _repository.GetByKeyAsync("one", TestContext.Current.CancellationToken))!.JsonData.Should().Be("""{"n":10}""");
        (await _repository.GetByKeyAsync("two", TestContext.Current.CancellationToken))!.JsonData.Should().Be("""{"n":2}""");
    }

    [Fact]
    public async Task GetByKeyAsync_UnknownKey_ReturnsNull()
    {
        // Act
        var entry = await _repository.GetByKeyAsync("does-not-exist", TestContext.Current.CancellationToken);

        // Assert
        entry.Should().BeNull();
    }

    [Fact]
    public async Task GetByKeyAsync_DifferentCase_ReturnsNull()
    {
        // Arrange
        await _repository.UpsertAsync("levels", "Levels", "{}", TestContext.Current.CancellationToken);

        // Act
        var entry = await _repository.GetByKeyAsync("LEVELS", TestContext.Current.CancellationToken);

        // Assert
        entry.Should().BeNull("keys are matched exactly");
    }

    // ── GetAllAsync ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_MultipleEntries_ReturnsThemOrderedByKey()
    {
        // Arrange
        await _repository.UpsertAsync("charlie-page", "C", "{}", TestContext.Current.CancellationToken);
        await _repository.UpsertAsync("alpha-page", "A", "{}", TestContext.Current.CancellationToken);
        await _repository.UpsertAsync("bravo-page", "B", "{}", TestContext.Current.CancellationToken);

        // Act
        var entries = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        entries.Select(e => e.Key).Should().Equal("alpha-page", "bravo-page", "charlie-page");
        entries.Select(e => e.Description).Should().Equal("A", "B", "C");
    }

    [Fact]
    public async Task GetAllAsync_EmptyTable_ReturnsEmptyList()
    {
        // Act
        var entries = await _repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        entries.Should().BeEmpty();
    }

    // ── IsEmptyAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task IsEmptyAsync_EmptyTable_ReturnsTrue()
    {
        // Act
        var isEmpty = await _repository.IsEmptyAsync(TestContext.Current.CancellationToken);

        // Assert
        isEmpty.Should().BeTrue();
    }

    [Fact]
    public async Task IsEmptyAsync_AfterUpsert_ReturnsFalse()
    {
        // Arrange
        await _repository.UpsertAsync("levels", "Levels", "{}", TestContext.Current.CancellationToken);

        // Act
        var isEmpty = await _repository.IsEmptyAsync(TestContext.Current.CancellationToken);

        // Assert
        isEmpty.Should().BeFalse();
    }
}
