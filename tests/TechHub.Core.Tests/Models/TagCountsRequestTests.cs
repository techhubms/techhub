using FluentAssertions;
using TechHub.Core.Models;

namespace TechHub.Core.Tests.Models;

/// <summary>
/// Unit tests for TagCountsRequest — validates cache key differentiation
/// for multi-collection filtering scenarios.
/// </summary>
public class TagCountsRequestTests
{
    [Fact]
    public void GetCacheKey_WithCollections_IncludesCollectionsInKey()
    {
        // Arrange
        var request = new TagCountsRequest(
            "ai", "all", maxTags: 30, minUses: 1,
            collections: new[] { "articles", "roundups" });

        // Act
        var key = request.GetCacheKey();

        // Assert
        key.Should().Contain("cols:articles,roundups");
    }

    [Fact]
    public void GetCacheKey_WithoutCollections_DoesNotIncludeCollectionsSegment()
    {
        // Arrange
        var request = new TagCountsRequest("ai", "all", maxTags: 30);

        // Act
        var key = request.GetCacheKey();

        // Assert
        key.Should().NotContain("cols:");
    }

    [Fact]
    public void GetCacheKey_CollectionsOrderIndependent_ProducesSameKey()
    {
        // Arrange
        var request1 = new TagCountsRequest(
            "ai", "all", maxTags: 30, minUses: 1,
            collections: new[] { "roundups", "articles" });
        var request2 = new TagCountsRequest(
            "ai", "all", maxTags: 30, minUses: 1,
            collections: new[] { "articles", "roundups" });

        // Act & Assert
        request1.GetCacheKey().Should().Be(request2.GetCacheKey());
    }

    [Fact]
    public void GetCacheKey_DifferentCollections_ProducesDifferentKeys()
    {
        // Arrange
        var requestTwo = new TagCountsRequest(
            "ai", "all", maxTags: 30, minUses: 1,
            collections: new[] { "articles", "roundups" });
        var requestOne = new TagCountsRequest(
            "ai", "all", maxTags: 30, minUses: 1,
            collections: new[] { "articles" });

        // Act & Assert
        requestTwo.GetCacheKey().Should().NotBe(requestOne.GetCacheKey());
    }

    [Fact]
    public void GetCacheKey_CollectionsVsNoCollections_ProducesDifferentKeys()
    {
        // Arrange — this is the key scenario: "all" without types vs "all" with types
        var requestAll = new TagCountsRequest("ai", "all", maxTags: 30);
        var requestFiltered = new TagCountsRequest(
            "ai", "all", maxTags: 30, minUses: 1,
            collections: new[] { "articles", "roundups" });

        // Act & Assert
        requestAll.GetCacheKey().Should().NotBe(requestFiltered.GetCacheKey());
    }

    [Fact]
    public void GetCacheKey_EmptyCollections_DoesNotIncludeCollectionsSegment()
    {
        // Arrange
        var request = new TagCountsRequest(
            "ai", "all", maxTags: 30, minUses: 1,
            collections: Array.Empty<string>());

        // Act
        var key = request.GetCacheKey();

        // Assert
        key.Should().NotContain("cols:");
    }

    [Fact]
    public void GetCacheKey_CollectionsCaseInsensitiveSorting()
    {
        // Arrange
        var request1 = new TagCountsRequest(
            "ai", "all", maxTags: 30, minUses: 1,
            collections: new[] { "Articles", "roundups" });
        var request2 = new TagCountsRequest(
            "ai", "all", maxTags: 30, minUses: 1,
            collections: new[] { "roundups", "articles" });

        // Act & Assert — case-insensitive ordering should produce same key
        request1.GetCacheKey().Should().Be(request2.GetCacheKey());
    }
}
