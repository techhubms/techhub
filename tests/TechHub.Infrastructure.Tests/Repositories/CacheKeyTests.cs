using FluentAssertions;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Tests to ensure cache keys include ALL parameters that affect results.
/// This prevents bugs where different parameters return the same cached result.
/// 
/// CRITICAL: Each cached method must return different cache keys when called
/// with different pagination/filter parameters.
/// </summary>
public class CacheKeyTests
{
    [Theory]
    [InlineData(10, 0, 10, 5)]  // Different offset
    [InlineData(10, 0, 20, 0)]  // Different limit
    [InlineData(10, 0, 20, 5)]  // Both different
    public void BuildSearchCacheKey_ShouldDifferForDifferentPagination(
        int take1, int skip1, int take2, int skip2)
    {
        // Arrange
        var request1 = new SearchRequest(
            take: take1,
            sections: new[] { "all" },
            collections: new[] { "all" },
            tags: new[] { "azure" },
            skip: skip1
        );

        var request2 = new SearchRequest(
            take: take2,
            sections: new[] { "all" },
            collections: new[] { "all" },
            tags: new[] { "azure" },
            skip: skip2
        );

        // Act
        var key1 = request1.GetCacheKey();
        var key2 = request2.GetCacheKey();

        // Assert
        key1.Should().NotBe(key2, "different pagination parameters should create different cache keys");
    }

    [Theory]
    [InlineData("azure", "github")]
    [InlineData("azure,ai", "azure")]
    public void BuildSearchCacheKey_ShouldDifferForDifferentTags(
        string tags1, string tags2)
    {
        // Arrange
        var request1 = new SearchRequest(
            take: 10,
            sections: new[] { "all" },
            collections: new[] { "all" },
            tags: tags1.Split(','),
            skip: 0
        );

        var request2 = new SearchRequest(
            take: 10,
            sections: new[] { "all" },
            collections: new[] { "all" },
            tags: tags2.Split(','),
            skip: 0
        );

        // Act
        var key1 = request1.GetCacheKey();
        var key2 = request2.GetCacheKey();

        // Assert
        key1.Should().NotBe(key2, "different tags should create different cache keys");
    }

    [Theory]
    [InlineData("github-copilot", "azure")]
    public void BuildSearchCacheKey_ShouldDifferForDifferentSections(
        string section1, string section2)
    {
        // Arrange
        var request1 = new SearchRequest(
            take: 10,
            sections: new[] { section1 },
            collections: new[] { "all" },
            tags: Array.Empty<string>(),
            skip: 0
        );

        var request2 = new SearchRequest(
            take: 10,
            sections: new[] { section2 },
            collections: new[] { "all" },
            tags: Array.Empty<string>(),
            skip: 0
        );

        // Act
        var key1 = request1.GetCacheKey();
        var key2 = request2.GetCacheKey();

        // Assert
        key1.Should().NotBe(key2, "different sections should create different cache keys");
    }

    [Theory]
    [InlineData("blogs", "videos")]
    public void BuildSearchCacheKey_ShouldDifferForDifferentCollections(
        string collection1, string collection2)
    {
        // Arrange
        var request1 = new SearchRequest(
            take: 10,
            sections: new[] { "all" },
            collections: new[] { collection1 },
            tags: Array.Empty<string>(),
            skip: 0
        );

        var request2 = new SearchRequest(
            take: 10,
            sections: new[] { "all" },
            collections: new[] { collection2 },
            tags: Array.Empty<string>(),
            skip: 0
        );

        // Act
        var key1 = request1.GetCacheKey();
        var key2 = request2.GetCacheKey();

        // Assert
        key1.Should().NotBe(key2, "different collections should create different cache keys");
    }

    [Fact]
    public void SearchRequest_GetCacheKey_ShouldDifferForDifferentIncludeFacets()
    {
        // Arrange - IncludeFacets changes whether Facets is populated in the response,
        // so requests differing only by this flag must not share a cache entry.
        var request1 = new SearchRequest(
            take: 10,
            sections: new[] { "all" },
            collections: new[] { "all" },
            tags: Array.Empty<string>(),
            includeFacets: false
        );

        var request2 = new SearchRequest(
            take: 10,
            sections: new[] { "all" },
            collections: new[] { "all" },
            tags: Array.Empty<string>(),
            includeFacets: true
        );

        // Act
        var key1 = request1.GetCacheKey();
        var key2 = request2.GetCacheKey();

        // Assert
        key1.Should().NotBe(key2, "different IncludeFacets values should create different cache keys");
    }

    [Theory]
    [InlineData("tags", "sections")]
    [InlineData("tags,sections", "tags")]
    public void FacetRequest_GetCacheKey_ShouldDifferForDifferentFacetFields(
        string fields1, string fields2)
    {
        // Arrange
        var request1 = new FacetRequest(
            facetFields: fields1.Split(','),
            tags: Array.Empty<string>(),
            sections: Array.Empty<string>(),
            collections: Array.Empty<string>(),
            maxFacetValues: 10
        );

        var request2 = new FacetRequest(
            facetFields: fields2.Split(','),
            tags: Array.Empty<string>(),
            sections: Array.Empty<string>(),
            collections: Array.Empty<string>(),
            maxFacetValues: 10
        );

        // Act
        var key1 = request1.GetCacheKey();
        var key2 = request2.GetCacheKey();

        // Assert
        key1.Should().NotBe(key2, "different facet fields should create different cache keys");
    }

    [Theory]
    [InlineData(10, 20)]
    [InlineData(10, 50)]
    public void FacetRequest_GetCacheKey_ShouldDifferForDifferentMaxFacetValues(
        int max1, int max2)
    {
        // Arrange
        var request1 = new FacetRequest(
            facetFields: new[] { "tags" },
            tags: Array.Empty<string>(),
            sections: Array.Empty<string>(),
            collections: Array.Empty<string>(),
            maxFacetValues: max1
        );

        var request2 = new FacetRequest(
            facetFields: new[] { "tags" },
            tags: Array.Empty<string>(),
            sections: Array.Empty<string>(),
            collections: Array.Empty<string>(),
            maxFacetValues: max2
        );

        // Act
        var key1 = request1.GetCacheKey();
        var key2 = request2.GetCacheKey();

        // Assert
        key1.Should().NotBe(key2, "different max values should create different cache keys");
    }

    [Theory]
    [InlineData(null, null, null, 20, 1)]  // Different MaxTags
    [InlineData(null, null, "github-copilot", 10, 1)]  // Different Section
    [InlineData(null, null, null, 10, 2)]  // Different MinUses
    [InlineData(1704067200L, null, null, 10, 1)]  // Different DateFrom (L suffix for long)
    public void TagCountsRequest_GetCacheKey_ShouldDifferForDifferentParameters(
        long? dateFromUnix, long? dateToUnix, string? section, int maxTags, int minUses)
    {
        // Arrange
        DateTimeOffset? dateFrom = dateFromUnix.HasValue
            ? DateTimeOffset.FromUnixTimeSeconds(dateFromUnix.Value)
            : null;
        DateTimeOffset? dateTo = dateToUnix.HasValue
            ? DateTimeOffset.FromUnixTimeSeconds(dateToUnix.Value)
            : null;

        var request1 = new TagCountsRequest(
            sectionName: "all",
            collectionName: "all",
            minUses: 1,
            maxTags: 10,
            dateFrom: null,
            dateTo: null
        );

        var request2 = new TagCountsRequest(
            sectionName: section ?? "all",
            collectionName: "all",
            minUses: minUses,
            maxTags: maxTags,
            dateFrom: dateFrom,
            dateTo: dateTo
        );

        // Act
        var key1 = request1.GetCacheKey();
        var key2 = request2.GetCacheKey();

        // Assert
        key1.Should().NotBe(key2, "different parameters should create different cache keys");
    }

    // Callers compute "lastDays" date ranges from DateTimeOffset.UtcNow on every request, so
    // without bucketing, every call gets a unique DateFrom and never hits the cache in practice.

    [Fact]
    public void SearchRequest_GetCacheKey_Same5MinuteBucket_ProducesSameKey()
    {
        // Arrange - two "now"-relative timestamps 3 minutes apart, within the same 5-minute bucket
        var baseTime = new DateTimeOffset(2026, 1, 1, 10, 30, 0, TimeSpan.Zero);
        var request1 = new SearchRequest(
            take: 10, sections: new[] { "all" }, collections: new[] { "all" }, tags: Array.Empty<string>(),
            dateFrom: baseTime);
        var request2 = new SearchRequest(
            take: 10, sections: new[] { "all" }, collections: new[] { "all" }, tags: Array.Empty<string>(),
            dateFrom: baseTime.AddMinutes(3));

        // Act & Assert
        request1.GetCacheKey().Should().Be(request2.GetCacheKey(),
            "requests within the same 5-minute bucket should share a cache entry");
    }

    [Fact]
    public void SearchRequest_GetCacheKey_Different5MinuteBucket_ProducesDifferentKey()
    {
        // Arrange - timestamps that straddle a 5-minute bucket boundary
        var request1 = new SearchRequest(
            take: 10, sections: new[] { "all" }, collections: new[] { "all" }, tags: Array.Empty<string>(),
            dateFrom: new DateTimeOffset(2026, 1, 1, 10, 34, 59, TimeSpan.Zero));
        var request2 = new SearchRequest(
            take: 10, sections: new[] { "all" }, collections: new[] { "all" }, tags: Array.Empty<string>(),
            dateFrom: new DateTimeOffset(2026, 1, 1, 10, 35, 0, TimeSpan.Zero));

        // Act & Assert
        request1.GetCacheKey().Should().NotBe(request2.GetCacheKey(),
            "requests in different 5-minute buckets should still create different cache keys");
    }

    [Fact]
    public void TagCountsRequest_GetCacheKey_Same5MinuteBucket_ProducesSameKey()
    {
        // Arrange
        var baseTime = new DateTimeOffset(2026, 1, 1, 10, 30, 0, TimeSpan.Zero);
        var request1 = new TagCountsRequest(
            sectionName: "all", collectionName: "all", maxTags: 10, dateFrom: baseTime);
        var request2 = new TagCountsRequest(
            sectionName: "all", collectionName: "all", maxTags: 10, dateFrom: baseTime.AddMinutes(4));

        // Act & Assert
        request1.GetCacheKey().Should().Be(request2.GetCacheKey(),
            "requests within the same 5-minute bucket should share a cache entry");
    }

    [Fact]
    public void FacetRequest_GetCacheKey_Same5MinuteBucket_ProducesSameKey()
    {
        // Arrange - two "now"-relative timestamps 3 minutes apart, within the same 5-minute bucket
        var baseTime = new DateTimeOffset(2026, 1, 1, 10, 30, 0, TimeSpan.Zero);
        var request1 = new FacetRequest(
            facetFields: new[] { "tags" }, tags: Array.Empty<string>(), sections: Array.Empty<string>(),
            collections: Array.Empty<string>(), dateFrom: baseTime);
        var request2 = new FacetRequest(
            facetFields: new[] { "tags" }, tags: Array.Empty<string>(), sections: Array.Empty<string>(),
            collections: Array.Empty<string>(), dateFrom: baseTime.AddMinutes(3));

        // Act & Assert
        request1.GetCacheKey().Should().Be(request2.GetCacheKey(),
            "requests within the same 5-minute bucket should share a cache entry");
    }

    [Fact]
    public void FacetRequest_GetCacheKey_Different5MinuteBucket_ProducesDifferentKey()
    {
        // Arrange - timestamps that straddle a 5-minute bucket boundary
        var request1 = new FacetRequest(
            facetFields: new[] { "tags" }, tags: Array.Empty<string>(), sections: Array.Empty<string>(),
            collections: Array.Empty<string>(), dateFrom: new DateTimeOffset(2026, 1, 1, 10, 34, 59, TimeSpan.Zero));
        var request2 = new FacetRequest(
            facetFields: new[] { "tags" }, tags: Array.Empty<string>(), sections: Array.Empty<string>(),
            collections: Array.Empty<string>(), dateFrom: new DateTimeOffset(2026, 1, 1, 10, 35, 0, TimeSpan.Zero));

        // Act & Assert
        request1.GetCacheKey().Should().NotBe(request2.GetCacheKey(),
            "requests in different 5-minute buckets should still create different cache keys");
    }
}
