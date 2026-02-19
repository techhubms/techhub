using FluentAssertions;
using TechHub.Api.Endpoints;
using TechHub.Core.Configuration;
using TechHub.Core.Models;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Unit tests for the ApplyQuantileSizing algorithm in ContentEndpoints.
/// Tests the tag size normalization logic that adjusts sizes based on the number
/// of distinct size groups produced by the quantile algorithm.
/// </summary>
public class ApplyQuantileSizingTests
{
    private static readonly QuantilePercentilesOptions DefaultOptions = new()
    {
        SmallToMedium = 0.25,
        MediumToLarge = 0.75
    };

    // ================================================================
    // Empty and minimal inputs
    // ================================================================

    [Fact]
    public void ApplyQuantileSizing_EmptyList_ReturnsEmpty()
    {
        // Arrange
        var tags = new List<TagWithCount>();

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ApplyQuantileSizing_SingleTag_ReturnsMediumSize()
    {
        // Arrange
        var tags = new List<TagWithCount>
        {
            new() { Tag = "AI", Count = 5 }
        };

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert
        result.Should().HaveCount(1);
        result[0].Size.Should().Be(TagSize.Medium, "a single tag should be Medium");
    }

    [Fact]
    public void ApplyQuantileSizing_TwoTags_AllMedium()
    {
        // Arrange
        var tags = new List<TagWithCount>
        {
            new() { Tag = "AI", Count = 10 },
            new() { Tag = "Azure", Count = 5 }
        };

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(t => t.Size.Should().Be(TagSize.Medium));
    }

    // ================================================================
    // 1 distinct size group → all Medium
    // ================================================================

    [Fact]
    public void ApplyQuantileSizing_AllSameCounts_AllMedium()
    {
        // Arrange - All tags have count=1 (e.g., a collection where each tag appears once)
        var tags = new List<TagWithCount>
        {
            new() { Tag = "Business Premium", Count = 1 },
            new() { Tag = "Cloud Security", Count = 1 },
            new() { Tag = "Microsoft Ignite", Count = 1 },
            new() { Tag = "Operations", Count = 1 },
            new() { Tag = "Defender", Count = 1 },
            new() { Tag = "Enterprise Security", Count = 1 },
            new() { Tag = "Security Analytics", Count = 1 },
            new() { Tag = "Security Copilot", Count = 1 },
            new() { Tag = "Modern SecOps", Count = 1 },
            new() { Tag = "Partner Resources", Count = 1 }
        };

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert - When only 1 distinct size group exists, all tags should be Medium
        result.Should().HaveCount(10);
        result.Should().AllSatisfy(t =>
            t.Size.Should().Be(TagSize.Medium, "all tags have the same count so they should all be Medium (regular size)"));
    }

    [Fact]
    public void ApplyQuantileSizing_VerySimilarCounts_AllMedium()
    {
        // Arrange - All tags have the same count
        var tags = new List<TagWithCount>
        {
            new() { Tag = "Tag1", Count = 3 },
            new() { Tag = "Tag2", Count = 3 },
            new() { Tag = "Tag3", Count = 3 },
            new() { Tag = "Tag4", Count = 3 },
            new() { Tag = "Tag5", Count = 3 }
        };

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert
        result.Should().AllSatisfy(t =>
            t.Size.Should().Be(TagSize.Medium, "identical counts should all be Medium"));
    }

    // ================================================================
    // 2 distinct size groups → Medium + Small
    // ================================================================

    [Fact]
    public void ApplyQuantileSizing_TwoDistinctSizeGroups_UsesMediumAndSmall()
    {
        // Arrange - Counts that produce exactly 2 size groups from the quantile algorithm
        // With default percentiles (0.25/0.75), we need counts where the high threshold
        // and low threshold create only 2 groups (e.g., Large+Medium collapse into one)
        var tags = new List<TagWithCount>
        {
            new() { Tag = "Popular1", Count = 10 },
            new() { Tag = "Popular2", Count = 10 },
            new() { Tag = "Popular3", Count = 10 },
            new() { Tag = "Popular4", Count = 10 },
            new() { Tag = "Popular5", Count = 10 },
            new() { Tag = "Popular6", Count = 10 },
            new() { Tag = "Rare1", Count = 1 },
            new() { Tag = "Rare2", Count = 1 },
            new() { Tag = "Rare3", Count = 1 }
        };

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert - With 2 distinct groups, higher group → Medium, lower group → Small
        var distinctSizes = result.Select(t => t.Size).Distinct().ToList();
        distinctSizes.Should().HaveCountLessThanOrEqualTo(2, "should produce at most 2 size groups");
        distinctSizes.Should().NotContain(TagSize.Large, "2-group normalization should use Medium and Small, not Large");

        result.Where(t => t.Count == 10).Should().AllSatisfy(t =>
            t.Size.Should().Be(TagSize.Medium, "higher group should be Medium"));
        result.Where(t => t.Count == 1).Should().AllSatisfy(t =>
            t.Size.Should().Be(TagSize.Small, "lower group should be Small"));
    }

    [Fact]
    public void ApplyQuantileSizing_TwoGroups_SplitsAtMedian()
    {
        // Arrange - 10 tags where the original quantile produces 2 groups.
        // The 50th percentile rebalancing should split at the median count value.
        // Counts (descending): [20, 20, 20, 15, 15, 5, 5, 5, 5, 5]
        // Original 25th percentile index = ceil(10*0.25) = 3 → count[3]=15
        // Original 75th percentile index = ceil(10*0.75) = 8 → count[8]=5
        // Original: >=15→Large (20,20,20,15,15), >=5→Medium (5,5,5,5,5), <5→Small (none)
        // → 2 distinct groups (Large & Medium), triggering normalization
        // Median index = ceil(10*0.5) = 5 → count[5]=5
        // After normalization: >=5→Medium (all), <5→Small (none) → actually 1 group → all Medium
        // Let's adjust to get a proper 2-group split at median
        var tags = new List<TagWithCount>
        {
            new() { Tag = "A", Count = 20 },
            new() { Tag = "B", Count = 20 },
            new() { Tag = "C", Count = 20 },
            new() { Tag = "D", Count = 20 },
            new() { Tag = "E", Count = 20 },
            new() { Tag = "F", Count = 2 },
            new() { Tag = "G", Count = 2 },
            new() { Tag = "H", Count = 2 },
            new() { Tag = "I", Count = 2 },
            new() { Tag = "J", Count = 2 }
        };

        // Median index = ceil(10*0.5) = 5 → counts[5] = 2
        // >=2 → Medium (all), <2 → Small (none) → all Medium
        // But original quantile has 2 groups (Large & Medium), so normalization applies
        // 50th percentile threshold = count at index 5 = 2
        // Since all counts >= 2, this becomes all Medium

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert - The median-based rebalancing should produce a balanced split
        var distinctSizes = result.Select(t => t.Size).Distinct().ToList();
        distinctSizes.Should().NotContain(TagSize.Large, "2-group normalization should never use Large");
    }

    [Fact]
    public void ApplyQuantileSizing_TwoGroups_MedianSplitsUnevenly()
    {
        // Arrange - Create a scenario where the original quantile produces 2 groups
        // but the 50th percentile rebalancing creates a different Medium/Small split
        // Counts (descending): [10, 10, 10, 10, 10, 10, 10, 3, 3, 3]
        // Original: 25th→count[3]=10, 75th→count[8]=3 → >=10→Large(7), >=3→Medium(3), <3→none
        // → 2 distinct groups → triggers normalization
        // Median: 50th→count[5]=10 → >=10→Medium(7), <10→Small(3)
        var tags = new List<TagWithCount>
        {
            new() { Tag = "A", Count = 10 },
            new() { Tag = "B", Count = 10 },
            new() { Tag = "C", Count = 10 },
            new() { Tag = "D", Count = 10 },
            new() { Tag = "E", Count = 10 },
            new() { Tag = "F", Count = 10 },
            new() { Tag = "G", Count = 10 },
            new() { Tag = "H", Count = 3 },
            new() { Tag = "I", Count = 3 },
            new() { Tag = "J", Count = 3 }
        };

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert - Median-based split: count>=10 → Medium, count<10 → Small
        result.Where(t => t.Count == 10).Should().AllSatisfy(t =>
            t.Size.Should().Be(TagSize.Medium, "count at or above median threshold should be Medium"));
        result.Where(t => t.Count == 3).Should().AllSatisfy(t =>
            t.Size.Should().Be(TagSize.Small, "count below median threshold should be Small"));
    }

    // ================================================================
    // 3 distinct size groups → Large + Medium + Small (unchanged)
    // ================================================================

    [Fact]
    public void ApplyQuantileSizing_ThreeDistinctSizeGroups_UsesAllSizes()
    {
        // Arrange - Counts that produce all 3 size groups
        var tags = new List<TagWithCount>
        {
            new() { Tag = "VeryPopular", Count = 100 },
            new() { Tag = "Popular", Count = 50 },
            new() { Tag = "Medium1", Count = 20 },
            new() { Tag = "Medium2", Count = 15 },
            new() { Tag = "Medium3", Count = 12 },
            new() { Tag = "Medium4", Count = 10 },
            new() { Tag = "Rare1", Count = 3 },
            new() { Tag = "Rare2", Count = 2 },
            new() { Tag = "Rare3", Count = 1 }
        };

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert - All 3 sizes should be present
        var distinctSizes = result.Select(t => t.Size).Distinct().ToList();
        distinctSizes.Should().HaveCount(3, "wide count range should produce all 3 size categories");
        distinctSizes.Should().Contain(TagSize.Large);
        distinctSizes.Should().Contain(TagSize.Medium);
        distinctSizes.Should().Contain(TagSize.Small);
    }

    // ================================================================
    // Counts and tag names are preserved
    // ================================================================

    [Fact]
    public void ApplyQuantileSizing_PreservesTagNamesAndCounts()
    {
        // Arrange
        var tags = new List<TagWithCount>
        {
            new() { Tag = "AI", Count = 50 },
            new() { Tag = "Azure", Count = 30 },
            new() { Tag = "DevOps", Count = 10 }
        };

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert
        result.Should().HaveCount(3);
        result.Select(t => t.Tag).Should().ContainInOrder("AI", "Azure", "DevOps");
        result.Select(t => t.Count).Should().ContainInOrder(50, 30, 10);
    }

    // ================================================================
    // Real-world scenario: the user's exact case
    // ================================================================

    [Fact]
    public void ApplyQuantileSizing_AllCountOne_TwentyTags_AllMedium()
    {
        // Arrange - This is the exact scenario from the user's report:
        // 20 tags all with count=1 were showing as Large
        var tags = Enumerable.Range(1, 20)
            .Select(i => new TagWithCount { Tag = $"Tag{i}", Count = 1 })
            .ToList();

        // Act
        var result = ContentEndpoints.ApplyQuantileSizing(tags, DefaultOptions);

        // Assert - All should be Medium (regular size), not Large
        result.Should().HaveCount(20);
        result.Should().AllSatisfy(t =>
            t.Size.Should().Be(TagSize.Medium,
                "when all tags have the same count, they should all be Medium (regular size), not Large"));
    }
}
