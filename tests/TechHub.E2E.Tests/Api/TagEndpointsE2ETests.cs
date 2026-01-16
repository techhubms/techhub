using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.DTOs;

namespace TechHub.E2E.Tests.Api;

/// <summary>
/// End-to-end tests for Tag endpoints
/// </summary>
public class TagEndpointsE2ETests(ApiTestFactory factory) : IClassFixture<ApiTestFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    #region GET /api/tags/all

    [Fact]
    public async Task GetAllTags_WithNoParameters_ReturnsAllTagsWithCounts()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/all");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<AllTagsResponse>();
        result.Should().NotBeNull();
        result!.Tags.Should().NotBeEmpty();
        result.Tags.All(t => t.Count > 0).Should().BeTrue("all tags should have positive counts");
        result.Tags.Select(t => t.Tag).Should().OnlyHaveUniqueItems("tags should be unique");
    }

    [Fact]
    public async Task GetAllTags_WithSectionFilter_ReturnsOnlyTagsFromSection()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/all?section=ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<AllTagsResponse>();
        result.Should().NotBeNull();
        result!.Tags.Should().NotBeEmpty();
        result.Tags.All(t => t.Count > 0).Should().BeTrue();
    }

    [Fact]
    public async Task GetAllTags_WithSectionAndCollectionFilter_ReturnsOnlyTagsFromCollection()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/all?section=ai&collection=news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<AllTagsResponse>();
        result.Should().NotBeNull();
        // May be empty if no news in AI section, but should succeed
    }

    #endregion

    #region GET /api/tags/cloud

    [Fact]
    public async Task GetTagCloud_HomepageScope_ReturnsTopTagsWithSizes()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/cloud?scope=Homepage");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        tagCloud!.Should().NotBeEmpty();
        tagCloud.Should().HaveCountLessThanOrEqualTo(20, "default max tags is 20");
        tagCloud.All(t => t.Count > 0).Should().BeTrue();
        tagCloud.All(t => Enum.IsDefined(typeof(TagSize), t.Size)).Should().BeTrue("all tags should have a valid size (Small=0, Medium=1, Large=2)");
        tagCloud.Should().BeInDescendingOrder(t => t.Count, "tags should be sorted by count descending");
    }

    [Fact]
    public async Task GetTagCloud_SectionScope_WithValidSection_ReturnsTagsForSection()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/cloud?scope=Section&section=github-copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        tagCloud!.Should().NotBeEmpty();
        tagCloud.Should().BeInDescendingOrder(t => t.Count);
    }

    [Fact]
    public async Task GetTagCloud_SectionScope_WithoutSectionParameter_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/cloud?scope=Section");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetTagCloud_CollectionScope_WithValidParameters_ReturnsTagsForCollection()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/cloud?scope=Collection&section=ai&collection=blogs");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        // May be empty if no blogs in AI section
    }

    [Fact]
    public async Task GetTagCloud_CollectionScope_WithoutCollectionParameter_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/cloud?scope=Collection&section=ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetTagCloud_WithMaxTagsParameter_LimitsResults()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/cloud?scope=Homepage&maxTags=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        tagCloud!.Should().HaveCountLessThanOrEqualTo(5);
    }

    [Fact]
    public async Task GetTagCloud_WithMinUsesParameter_FiltersLowUsageTags()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/cloud?scope=Homepage&minUses=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        tagCloud!.All(t => t.Count >= 10).Should().BeTrue("all tags should have at least minUses count");
    }

    [Fact]
    public async Task GetTagCloud_WithLastDaysParameter_FiltersOldContent()
    {
        // Act - only content from last 30 days
        var response = await _client.GetAsync("/api/tags/cloud?scope=Homepage&lastDays=30");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();
        // Results depend on content age, but should succeed
    }

    [Fact]
    public async Task GetTagCloud_InvalidScope_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/cloud?scope=InvalidScope");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetTagCloud_VerifyQuantileSizing_ReturnsCorrectSizeDistribution()
    {
        // Act
        var response = await _client.GetAsync("/api/tags/cloud?scope=Homepage&maxTags=20&minUses=1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tagCloud = await response.Content.ReadFromJsonAsync<List<TagCloudItem>>();
        tagCloud.Should().NotBeNull();

        if (tagCloud!.Count >= 4) // Need at least 4 tags to test quantiles
        {
            // Top 25% should be Large
            var largeTags = tagCloud.Where(t => t.Size == TagSize.Large).ToList();
            // 25-75% should be Medium
            var mediumTags = tagCloud.Where(t => t.Size == TagSize.Medium).ToList();
            // 75-100% should be Small
            var smallTags = tagCloud.Where(t => t.Size == TagSize.Small).ToList();

            // All tags should have a size
            (largeTags.Count + mediumTags.Count + smallTags.Count).Should().Be(tagCloud.Count);

            // Large tags should have highest counts
            if (largeTags.Any() && smallTags.Any())
            {
                largeTags.Min(t => t.Count).Should().BeGreaterThanOrEqualTo(smallTags.Max(t => t.Count));
            }
        }
    }

    #endregion
}
