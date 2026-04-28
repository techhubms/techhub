using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

public class ContentGridStateCacheTests
{
    private readonly ContentGridStateCache _cache = new();

    [Fact]
    public void Get_WithNoEntry_ReturnsNull()
    {
        // Act
        var result = _cache.Get("nonexistent-key");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Set_ThenGet_ReturnsCachedState()
    {
        // Arrange
        var items = CreateItems(3);

        // Act
        _cache.Set("test-key", items, currentBatch: 2, hasMoreContent: true, totalCount: 50);
        var result = _cache.Get("test-key");

        // Assert
        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(3);
        result.CurrentBatch.Should().Be(2);
        result.HasMoreContent.Should().BeTrue();
        result.TotalCount.Should().Be(50);
    }

    [Fact]
    public void Get_ReturnsCopyOfItemsList_NotSameReference()
    {
        // Arrange
        var items = CreateItems(2);
        _cache.Set("test-key", items, currentBatch: 1, hasMoreContent: true, totalCount: 10);

        // Act
        var result1 = _cache.Get("test-key");
        var result2 = _cache.Get("test-key");

        // Assert - different list instances so callers can mutate independently
        result1!.Items.Should().NotBeSameAs(result2!.Items);
        result1.Items.Should().BeEquivalentTo(result2.Items);
    }

    [Fact]
    public void Get_MutatingReturnedList_DoesNotAffectCache()
    {
        // Arrange
        var items = CreateItems(3);
        _cache.Set("test-key", items, currentBatch: 1, hasMoreContent: true, totalCount: 10);

        // Act - mutate the returned list
        var result = _cache.Get("test-key");
        result!.Items.Add(new ContentItemBuilder().WithSlug("extra").Build());

        // Assert - cache still has original 3 items
        var freshResult = _cache.Get("test-key");
        freshResult!.Items.Should().HaveCount(3);
    }

    [Fact]
    public void Set_OverwritesExistingEntry()
    {
        // Arrange
        _cache.Set("test-key", CreateItems(2), currentBatch: 1, hasMoreContent: true, totalCount: 10);

        // Act - overwrite with new data
        _cache.Set("test-key", CreateItems(5), currentBatch: 3, hasMoreContent: false, totalCount: 50);
        var result = _cache.Get("test-key");

        // Assert
        result!.Items.Should().HaveCount(5);
        result.CurrentBatch.Should().Be(3);
        result.HasMoreContent.Should().BeFalse();
        result.TotalCount.Should().Be(50);
    }

    [Fact]
    public void Set_DifferentKeys_StoredIndependently()
    {
        // Arrange & Act
        _cache.Set("key-a", CreateItems(2), currentBatch: 1, hasMoreContent: true, totalCount: 20);
        _cache.Set("key-b", CreateItems(4), currentBatch: 2, hasMoreContent: false, totalCount: 40);

        // Assert
        _cache.Get("key-a")!.Items.Should().HaveCount(2);
        _cache.Get("key-b")!.Items.Should().HaveCount(4);
    }

    [Fact]
    public void Set_SnapshotsInputList_MutatingOriginalDoesNotAffectCache()
    {
        // Arrange
        var items = CreateItems(3);
        _cache.Set("test-key", items, currentBatch: 1, hasMoreContent: true, totalCount: 10);

        // Act - mutate the original list after caching
        items.Add(new ContentItemBuilder().WithSlug("extra").Build());

        // Assert - cache still has 3 items
        _cache.Get("test-key")!.Items.Should().HaveCount(3);
    }

    [Fact]
    public void Set_BeyondMaxEntries_EvictsOldestEntry()
    {
        // Arrange - fill cache to capacity
        for (var i = 0; i < ContentGridStateCache.MaxEntries; i++)
        {
            _cache.Set($"key-{i}", CreateItems(1), currentBatch: 1, hasMoreContent: false, totalCount: 1);
        }

        // Act - add one more entry, which should evict key-0 (oldest)
        _cache.Set("key-new", CreateItems(2), currentBatch: 2, hasMoreContent: true, totalCount: 99);

        // Assert - oldest entry evicted, new entry present
        _cache.Get("key-0").Should().BeNull("oldest entry should have been evicted");
        _cache.Get("key-new").Should().NotBeNull("newly added entry should be present");
    }

    [Fact]
    public void Set_ExistingKey_DoesNotEvict()
    {
        // Arrange - fill cache to capacity
        for (var i = 0; i < ContentGridStateCache.MaxEntries; i++)
        {
            _cache.Set($"key-{i}", CreateItems(1), currentBatch: 1, hasMoreContent: false, totalCount: 1);
        }

        // Act - update an existing key (should NOT evict anything)
        _cache.Set("key-0", CreateItems(5), currentBatch: 3, hasMoreContent: true, totalCount: 50);

        // Assert - all original keys (including key-0) still present, and key-0 updated
        for (var i = 0; i < ContentGridStateCache.MaxEntries; i++)
        {
            _cache.Get($"key-{i}").Should().NotBeNull($"key-{i} should still be present");
        }

        _cache.Get("key-0")!.Items.Should().HaveCount(5, "key-0 should have updated items");
    }

    [Fact]
    public void Set_MultipleEvictions_MaintainsFifoOrder()
    {
        // Arrange - fill to capacity
        for (var i = 0; i < ContentGridStateCache.MaxEntries; i++)
        {
            _cache.Set($"key-{i}", CreateItems(1), currentBatch: 1, hasMoreContent: false, totalCount: 1);
        }

        // Act - add 3 more entries, evicting key-0, key-1, key-2
        _cache.Set("extra-1", CreateItems(1), currentBatch: 1, hasMoreContent: false, totalCount: 1);
        _cache.Set("extra-2", CreateItems(1), currentBatch: 1, hasMoreContent: false, totalCount: 1);
        _cache.Set("extra-3", CreateItems(1), currentBatch: 1, hasMoreContent: false, totalCount: 1);

        // Assert - oldest three evicted, newest three present
        _cache.Get("key-0").Should().BeNull();
        _cache.Get("key-1").Should().BeNull();
        _cache.Get("key-2").Should().BeNull();
        _cache.Get("extra-1").Should().NotBeNull();
        _cache.Get("extra-2").Should().NotBeNull();
        _cache.Get("extra-3").Should().NotBeNull();
    }

    private static List<ContentItem> CreateItems(int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => new ContentItemBuilder().WithSlug($"item-{i}").WithTitle($"Title for item-{i}").Build())
            .ToList();
    }
}
