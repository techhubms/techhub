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
            sections: ["all"],
            collections: ["all"],
            tags: ["azure"],
            skip: skip1
        );

        var request2 = new SearchRequest(
            take: take2,
            sections: ["all"],
            collections: ["all"],
            tags: ["azure"],
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
            sections: ["all"],
            collections: ["all"],
            tags: [.. tags1.Split(',')],
            skip: 0
        );

        var request2 = new SearchRequest(
            take: 10,
            sections: ["all"],
            collections: ["all"],
            tags: [.. tags2.Split(',')],
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
            sections: [section1],
            collections: ["all"],
            tags: [],
            skip: 0
        );

        var request2 = new SearchRequest(
            take: 10,
            sections: [section2],
            collections: ["all"],
            tags: [],
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
            sections: ["all"],
            collections: [collection1],
            tags: [],
            skip: 0
        );

        var request2 = new SearchRequest(
            take: 10,
            sections: ["all"],
            collections: [collection2],
            tags: [],
            skip: 0
        );

        // Act
        var key1 = request1.GetCacheKey();
        var key2 = request2.GetCacheKey();

        // Assert
        key1.Should().NotBe(key2, "different collections should create different cache keys");
    }

    [Theory]
    [InlineData("tags", "sections")]
    [InlineData("tags,sections", "tags")]
    public void FacetRequest_GetCacheKey_ShouldDifferForDifferentFacetFields(
        string fields1, string fields2)
    {
        // Arrange
        var request1 = new FacetRequest(
            facetFields: [.. fields1.Split(',')],
            tags: [],
            sections: [],
            collections: [],
            maxFacetValues: 10
        );

        var request2 = new FacetRequest(
            facetFields: [.. fields2.Split(',')],
            tags: [],
            sections: [],
            collections: [],
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
            facetFields: ["tags"],
            tags: [],
            sections: [],
            collections: [],
            maxFacetValues: max1
        );

        var request2 = new FacetRequest(
            facetFields: ["tags"],
            tags: [],
            sections: [],
            collections: [],
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
}
