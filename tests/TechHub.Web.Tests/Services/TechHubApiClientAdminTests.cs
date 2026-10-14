using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

/// <summary>
/// Tests for the admin, newsletter and legacy-redirect methods of TechHubApiClient:
/// request shape (verb, path, query escaping, body) and HTTP status mapping (404 → null/false, 409/422/5xx → exception or null).
/// </summary>
public class TechHubApiClientAdminTests
{
    // ── Content reviews ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetContentReviewsAsync_WithStatusAndLimit_BuildsQueryAndReturnsReviews()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """[{"id":7,"slug":"s","collectionName":"blogs","changeType":"tags","originalValue":"a","fixedValue":"b","status":"pending"}]""");

        // Act
        var reviews = await fixture.Client.GetContentReviewsAsync("pending", 50, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("GET");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/reviews?limit=50&status=pending");
        reviews.Should().ContainSingle().Which.Id.Should().Be(7);
    }

    [Fact]
    public async Task GetContentReviewsAsync_WithoutStatus_OmitsStatusFilter()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "[]");

        // Act
        var reviews = await fixture.Client.GetContentReviewsAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/reviews?limit=100");
        reviews.Should().BeEmpty();
    }

    [Fact]
    public async Task GetContentReviewsAsync_NullBody_ReturnsEmptyList()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "null");

        // Act
        var reviews = await fixture.Client.GetContentReviewsAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        reviews.Should().BeEmpty();
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task GetContentReviewsAsync_ErrorStatus_ThrowsHttpRequestExceptionWithStatusCode(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.GetContentReviewsAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public async Task GetContentReviewSummaryAsync_NullBody_ReturnsEmptySummary()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "null");

        // Act
        var summary = await fixture.Client.GetContentReviewSummaryAsync(TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/reviews/summary");
        summary.Total.Should().Be(0);
    }

    [Fact]
    public async Task GetContentReviewSummaryAsync_Success_ReturnsCounts()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"pending":3,"approved":2,"rejected":1}""");

        // Act
        var summary = await fixture.Client.GetContentReviewSummaryAsync(TestContext.Current.CancellationToken);

        // Assert
        summary.Pending.Should().Be(3);
        summary.Total.Should().Be(6);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.NoContent, true)]
    [InlineData(HttpStatusCode.NotFound, false)]
    [InlineData(HttpStatusCode.Conflict, false)]
    [InlineData(HttpStatusCode.InternalServerError, false)]
    public async Task ApproveContentReviewAsync_MapsStatusToBool(HttpStatusCode status, bool expected)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var result = await fixture.Client.ApproveContentReviewAsync(42, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/reviews/42/approve");
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.NotFound, false)]
    [InlineData(HttpStatusCode.InternalServerError, false)]
    public async Task RejectContentReviewAsync_MapsStatusToBool(HttpStatusCode status, bool expected)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var result = await fixture.Client.RejectContentReviewAsync(42, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/reviews/42/reject");
        result.Should().Be(expected);
    }

    [Fact]
    public async Task ApproveAllContentReviewsAsync_Success_ReturnsApprovedCount()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"approved":5}""");

        // Act
        var count = await fixture.Client.ApproveAllContentReviewsAsync(TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/reviews/approve-all");
        count.Should().Be(5);
    }

    [Fact]
    public async Task ApproveAllContentReviewsAsync_ServerError_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        var act = () => fixture.Client.ApproveAllContentReviewsAsync(TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task RejectAllContentReviewsAsync_Success_ReturnsRejectedCount()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"rejected":4}""");

        // Act
        var count = await fixture.Client.RejectAllContentReviewsAsync(TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/reviews/reject-all");
        count.Should().Be(4);
    }

    [Fact]
    public async Task UpdateContentReviewFixedValueAsync_SendsFixedValueAsPut()
    {
        // Arrange
        using var fixture = Fixture.Create();

        // Act
        var result = await fixture.Client.UpdateContentReviewFixedValueAsync(9, "new value", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("PUT");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/reviews/9");
        fixture.Handler.Json.GetProperty("fixedValue").GetString().Should().Be("new value");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateContentReviewFixedValueAsync_NotFound_ReturnsFalse()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var result = await fixture.Client.UpdateContentReviewFixedValueAsync(9, "x", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
    }

    // ── Feed configs ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetFeedConfigsAsync_Success_ReturnsFeeds()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """[{"id":1,"name":"Blog","outputDir":"_blogs","url":"https://example.com/feed","enabled":true}]""");

        // Act
        var feeds = await fixture.Client.GetFeedConfigsAsync(TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/feeds");
        feeds.Should().ContainSingle().Which.CollectionName.Should().Be("blogs");
    }

    [Fact]
    public async Task GetFeedConfigsAsync_ServerError_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        var act = () => fixture.Client.GetFeedConfigsAsync(TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetFeedConfigByIdAsync_NotFound_ReturnsNull()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var feed = await fixture.Client.GetFeedConfigByIdAsync(3, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/feeds/3");
        feed.Should().BeNull();
    }

    [Fact]
    public async Task GetFeedConfigByIdAsync_ServerError_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        var act = () => fixture.Client.GetFeedConfigByIdAsync(3, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task CreateFeedConfigAsync_Success_PostsConfigAndReturnsCreated()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"id":11,"name":"Blog","outputDir":"_blogs","url":"https://example.com/feed","enabled":true}""");
        var config = new FeedConfig { Name = "Blog", OutputDir = "_blogs", Url = "https://example.com/feed" };

        // Act
        var created = await fixture.Client.CreateFeedConfigAsync(config, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/feeds");
        fixture.Handler.Json.GetProperty("name").GetString().Should().Be("Blog");
        fixture.Handler.Json.GetProperty("url").GetString().Should().Be("https://example.com/feed");
        created.Id.Should().Be(11);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Conflict)]
    [InlineData(HttpStatusCode.UnprocessableEntity)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task CreateFeedConfigAsync_ErrorStatus_ThrowsHttpRequestExceptionWithStatusCode(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.CreateFeedConfigAsync(new FeedConfig { Name = "n" }, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public async Task CreateFeedConfigAsync_NullBody_ThrowsInvalidOperation()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "null");

        // Act
        var act = () => fixture.Client.CreateFeedConfigAsync(new FeedConfig { Name = "n" }, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdateFeedConfigAsync_Success_PutsToIdRoute()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"id":5,"name":"Renamed","outputDir":"_news","url":"https://example.com/f","enabled":false}""");

        // Act
        var updated = await fixture.Client.UpdateFeedConfigAsync(
            new FeedConfig { Id = 5, Name = "Renamed", OutputDir = "_news", Url = "https://example.com/f", Enabled = false },
            TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("PUT");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/feeds/5");
        fixture.Handler.Json.GetProperty("enabled").GetBoolean().Should().BeFalse();
        updated.Name.Should().Be("Renamed");
    }

    [Fact]
    public async Task UpdateFeedConfigAsync_NotFound_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var act = () => fixture.Client.UpdateFeedConfigAsync(new FeedConfig { Id = 5 }, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateFeedConfigAsync_NullConfig_ThrowsArgumentNull()
    {
        // Arrange
        using var fixture = Fixture.Create();

        // Act
        var act = () => fixture.Client.UpdateFeedConfigAsync(null!, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteFeedConfigAsync_Success_SendsDelete()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        await fixture.Client.DeleteFeedConfigAsync(8, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("DELETE");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/feeds/8");
    }

    [Fact]
    public async Task DeleteFeedConfigAsync_NotFound_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var act = () => fixture.Client.DeleteFeedConfigAsync(8, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    // ── Content items ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetContentItemsAsync_AllFilters_BuildsEscapedQuery()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"items":[],"totalCount":0}""");

        // Act
        await fixture.Client.GetContentItemsAsync(
            page: 2,
            pageSize: 25,
            search: "a&b c",
            collectionName: "blogs",
            feedName: "The Blog",
            sectionName: "ai",
            primarySectionOnly: true,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be(
            "/api/admin/content-items?page=2&pageSize=25&search=a%26b%20c&collectionName=blogs&feedName=The%20Blog&sectionName=ai&primarySectionOnly=true");
    }

    [Fact]
    public async Task GetContentItemsAsync_PrimarySectionOnlyWithoutSection_OmitsFlag()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"items":[],"totalCount":0}""");

        // Act
        await fixture.Client.GetContentItemsAsync(primarySectionOnly: true, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/content-items?page=1&pageSize=100");
    }

    [Fact]
    public async Task GetContentItemsAsync_NullBody_ReturnsEmptyPage()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "null");

        // Act
        var result = await fixture.Client.GetContentItemsAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetContentItemsAsync_ServerError_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        var act = () => fixture.Client.GetContentItemsAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.NoContent, true)]
    [InlineData(HttpStatusCode.NotFound, false)]
    [InlineData(HttpStatusCode.InternalServerError, false)]
    public async Task DeleteContentItemAsync_MapsStatusToBool(HttpStatusCode status, bool expected)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var result = await fixture.Client.DeleteContentItemAsync("blogs", "my slug/1", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("DELETE");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/content-items?collection=blogs&slug=my%20slug%2F1");
        result.Should().Be(expected);
    }

    [Fact]
    public async Task GetContentItemEditDataAsync_NotFound_ReturnsNull()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var result = await fixture.Client.GetContentItemEditDataAsync("blogs", "missing", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/content-items/edit-data?collection=blogs&slug=missing");
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetContentItemEditDataAsync_ServerError_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        var act = () => fixture.Client.GetContentItemEditDataAsync("blogs", "s", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task UpdateContentItemEditDataAsync_Success_PutsEditData()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        await fixture.Client.UpdateContentItemEditDataAsync("blogs", "slug", CreateEditData(), TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("PUT");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/content-items/edit-data?collection=blogs&slug=slug");
        fixture.Handler.Json.GetProperty("title").GetString().Should().Be("Title");
        fixture.Handler.Json.GetProperty("tags")[0].GetString().Should().Be("ai");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.UnprocessableEntity)]
    public async Task UpdateContentItemEditDataAsync_ErrorStatus_Throws(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.UpdateContentItemEditDataAsync("blogs", "slug", CreateEditData(), TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public async Task GetContentItemAiMetadataAsync_NotFound_ReturnsNull()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var result = await fixture.Client.GetContentItemAiMetadataAsync("blogs", "slug", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/content-items/ai-metadata?collection=blogs&slug=slug");
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateContentItemAiMetadataAsync_Success_PutsAiMetadata()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        await fixture.Client.UpdateContentItemAiMetadataAsync("blogs", "slug", """{"k":1}""", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("PUT");
        fixture.Handler.Json.GetProperty("aiMetadata").GetString().Should().Be("""{"k":1}""");
    }

    [Fact]
    public async Task UpdateContentItemAiMetadataAsync_BadRequest_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.BadRequest);

        // Act
        var act = () => fixture.Client.UpdateContentItemAiMetadataAsync("blogs", "slug", "not json", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Theory]
    [InlineData("", "slug", "text")]
    [InlineData("videos", "", "text")]
    [InlineData("videos", "slug", "  ")]
    public async Task ApplyTranscriptAsync_BlankArgument_ThrowsWithoutCallingApi(string collection, string slug, string transcript)
    {
        // Arrange
        using var fixture = Fixture.Create();

        // Act
        var act = () => fixture.Client.ApplyTranscriptAsync(collection, slug, transcript, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
        fixture.Handler.RequestCount.Should().Be(0);
    }

    [Fact]
    public async Task ApplyTranscriptAsync_Success_PostsTranscriptAndReturnsEditData()
    {
        // Arrange
        using var fixture = Fixture.Create(body: JsonSerializer.Serialize(CreateEditData()));

        // Act
        var result = await fixture.Client.ApplyTranscriptAsync("videos", "slug", "hello", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/content-items/apply-transcript?collection=videos&slug=slug");
        fixture.Handler.Json.GetProperty("transcript").GetString().Should().Be("hello");
        result.Title.Should().Be("Title");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task ApplyTranscriptAsync_ErrorStatus_Throws(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.ApplyTranscriptAsync("videos", "slug", "hello", TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public async Task ApplyTranscriptAsync_NullBody_ThrowsInvalidOperation()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "null");

        // Act
        var act = () => fixture.Client.ApplyTranscriptAsync("videos", "slug", "hello", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    // ── GitHub Copilot features ─────────────────────────────────────────────

    [Fact]
    public async Task UpsertGhcFeatureAsync_Success_PutsFeatureAndSanitizesSlug()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);
        var feature = new GhcFeature { Slug = "agent\r\n-mode", Title = "Agent mode", Plans = ["Pro"] };

        // Act
        var result = await fixture.Client.UpsertGhcFeatureAsync(feature, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("PUT");
        fixture.Handler.PathAndQuery.Should().Be("/api/ghc-features/agent-mode");
        fixture.Handler.Json.GetProperty("title").GetString().Should().Be("Agent mode");
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task UpsertGhcFeatureAsync_ErrorStatus_Throws(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.UpsertGhcFeatureAsync(new GhcFeature { Slug = "s", Title = "t" }, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public async Task UpsertGhcFeatureAsync_NullFeature_ThrowsArgumentNull()
    {
        // Arrange
        using var fixture = Fixture.Create();

        // Act
        var act = () => fixture.Client.UpsertGhcFeatureAsync(null!, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteGhcFeatureAsync_NotFound_ReturnsFalse()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var result = await fixture.Client.DeleteGhcFeatureAsync("missing", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("DELETE");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/ghc-features/missing");
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteGhcFeatureAsync_Success_ReturnsTrueAndSanitizesSlug()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        var result = await fixture.Client.DeleteGhcFeatureAsync("agent\r\n-mode", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/ghc-features/agent-mode");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteGhcFeatureAsync_ServerError_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        var act = () => fixture.Client.DeleteGhcFeatureAsync("s", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task AddGhcFeatureContentLinkAsync_Success_PostsLink()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        await fixture.Client.AddGhcFeatureContentLinkAsync("feat", "videos", "vid-1", true, 3, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/ghc-features/feat/content-links");
        fixture.Handler.Json.GetProperty("collectionName").GetString().Should().Be("videos");
        fixture.Handler.Json.GetProperty("itemSlug").GetString().Should().Be("vid-1");
        fixture.Handler.Json.GetProperty("isThumbnail").GetBoolean().Should().BeTrue();
        fixture.Handler.Json.GetProperty("sortOrder").GetInt32().Should().Be(3);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task AddGhcFeatureContentLinkAsync_ErrorStatus_Throws(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.AddGhcFeatureContentLinkAsync("feat", "videos", "v", false, 0, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public async Task RemoveGhcFeatureContentLinkAsync_Success_DeletesWithEscapedQuery()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        await fixture.Client.RemoveGhcFeatureContentLinkAsync("feat", "videos", "a b", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("DELETE");
        fixture.Handler.PathAndQuery.Should().Be("/api/ghc-features/feat/content-links?collection=videos&itemSlug=a%20b");
    }

    [Fact]
    public async Task RemoveGhcFeatureContentLinkAsync_NotFound_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var act = () => fixture.Client.RemoveGhcFeatureContentLinkAsync("feat", "videos", "v", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task SetGhcFeatureThumbnailAsync_Success_PutsWithQuery()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        await fixture.Client.SetGhcFeatureThumbnailAsync("feat", "videos", "vid-1", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("PUT");
        fixture.Handler.PathAndQuery.Should().Be("/api/ghc-features/feat/thumbnail?collection=videos&itemSlug=vid-1");
    }

    [Fact]
    public async Task SetGhcFeatureThumbnailAsync_NotFound_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var act = () => fixture.Client.SetGhcFeatureThumbnailAsync("feat", "videos", "v", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    // ── Ad-hoc URL processing and title fetch ───────────────────────────────

    [Fact]
    public async Task ProcessAdHocUrlAsync_Success_ReturnsResult()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"outcome":0,"slug":"new-slug","message":"Added"}""");
        var request = new AdHocUrlProcessRequest { Url = "https://example.com/a", CollectionName = "blogs", TitleHint = "Hint" };

        // Act
        var result = await fixture.Client.ProcessAdHocUrlAsync(request, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/urls/process");
        fixture.Handler.Json.GetProperty("url").GetString().Should().Be("https://example.com/a");
        fixture.Handler.Json.GetProperty("collectionName").GetString().Should().Be("blogs");
        result.Should().NotBeNull();
        result!.Outcome.Should().Be(AdHocUrlProcessOutcome.Added);
        result.Slug.Should().Be("new-slug");
    }

    [Fact]
    public async Task ProcessAdHocUrlAsync_Conflict_ReturnsNull()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.Conflict, """{"message":"This URL has already been processed."}""");

        // Act
        var result = await fixture.Client.ProcessAdHocUrlAsync(
            new AdHocUrlProcessRequest { Url = "https://example.com/a", CollectionName = "blogs" },
            TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.UnprocessableEntity)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task ProcessAdHocUrlAsync_OtherErrorStatus_ThrowsHttpRequestException(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status, """{"outcome":2,"message":"Failed"}""");

        // Act
        var act = () => fixture.Client.ProcessAdHocUrlAsync(
            new AdHocUrlProcessRequest { Url = "https://example.com/a", CollectionName = "blogs" },
            TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public async Task ProcessAdHocUrlAsync_NullRequest_ThrowsArgumentNull()
    {
        // Arrange
        using var fixture = Fixture.Create();

        // Act
        var act = () => fixture.Client.ProcessAdHocUrlAsync(null!, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task FetchUrlTitleAsync_Success_ReturnsTitleAndEscapesUrl()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"title":"My Title"}""");

        // Act
        var title = await fixture.Client.FetchUrlTitleAsync("https://example.com/a?x=1&y=2\r\n", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/urls/title?url=https%3A%2F%2Fexample.com%2Fa%3Fx%3D1%26y%3D2%0D%0A");
        title.Should().Be("My Title");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task FetchUrlTitleAsync_ErrorStatus_ReturnsNull(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var title = await fixture.Client.FetchUrlTitleAsync("https://example.com", TestContext.Current.CancellationToken);

        // Assert
        title.Should().BeNull();
    }

    [Fact]
    public async Task FetchUrlTitleAsync_TransportFailure_ReturnsNull()
    {
        // Arrange
        using var fixture = Fixture.Create(throws: new HttpRequestException("connection refused"));

        // Act
        var title = await fixture.Client.FetchUrlTitleAsync("https://example.com", TestContext.Current.CancellationToken);

        // Assert
        title.Should().BeNull();
    }

    [Fact]
    public async Task FetchUrlTitleAsync_Timeout_ReturnsNull()
    {
        // Arrange
        using var fixture = Fixture.Create(throws: new TaskCanceledException("timeout"));

        // Act
        var title = await fixture.Client.FetchUrlTitleAsync("https://example.com", TestContext.Current.CancellationToken);

        // Assert
        title.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task FetchUrlTitleAsync_BlankUrl_ThrowsWithoutCallingApi(string url)
    {
        // Arrange
        using var fixture = Fixture.Create();

        // Act
        var act = () => fixture.Client.FetchUrlTitleAsync(url, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
        fixture.Handler.RequestCount.Should().Be(0);
    }

    // ── Markdown preview ────────────────────────────────────────────────────

    [Fact]
    public async Task PreviewMarkdownAsync_Success_PostsMarkdownAndReturnsHtml()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"html":"<p>hi</p>"}""");

        // Act
        var html = await fixture.Client.PreviewMarkdownAsync("hi", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/content-items/preview-markdown");
        fixture.Handler.Json.GetProperty("markdown").GetString().Should().Be("hi");
        html.Should().Be("<p>hi</p>");
    }

    [Fact]
    public async Task PreviewMarkdownAsync_MissingHtml_ReturnsEmptyString()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "{}");

        // Act
        var html = await fixture.Client.PreviewMarkdownAsync("hi", TestContext.Current.CancellationToken);

        // Assert
        html.Should().BeEmpty();
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task PreviewMarkdownAsync_ErrorStatus_Throws(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.PreviewMarkdownAsync("hi", TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    // ── Newsletter ──────────────────────────────────────────────────────────

    [Fact]
    public async Task SubscribeNewsletterAsync_BadRequest_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.BadRequest);

        // Act
        var act = () => fixture.Client.SubscribeNewsletterAsync("a@b.c", null, ["ai"], [], TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SubscribeNewsletterAsync_BlankDisplayName_SendsNull()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.OK);

        // Act
        await fixture.Client.SubscribeNewsletterAsync("a@b.c", "   ", ["ai"], [], TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Json.GetProperty("displayName").ValueKind.Should().Be(JsonValueKind.Null);
    }

    [Fact]
    public async Task UnsubscribeNewsletterAsync_SanitizesAndPostsPayload()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.OK);

        // Act
        await fixture.Client.UnsubscribeNewsletterAsync("a@b.c\r\n", "to\r\nken", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/newsletter/unsubscribe");
        fixture.Handler.Json.GetProperty("email").GetString().Should().Be("a@b.c");
        fixture.Handler.Json.GetProperty("token").GetString().Should().Be("token");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task UnsubscribeNewsletterAsync_ErrorStatus_Throws(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.UnsubscribeNewsletterAsync("a@b.c", "t", TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public async Task ConfirmNewsletterAsync_Success_ReturnsServerMessageAndEscapesQuery()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"message":"Welcome aboard"}""");

        // Act
        var message = await fixture.Client.ConfirmNewsletterAsync("a+b@example.com", "t/k=", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/newsletter/confirm?email=a%2Bb%40example.com&token=t%2Fk%3D");
        message.Should().Be("Welcome aboard");
    }

    [Fact]
    public async Task ConfirmNewsletterAsync_NoMessage_ReturnsDefaultMessage()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "{}");

        // Act
        var message = await fixture.Client.ConfirmNewsletterAsync("a@b.c", "t", TestContext.Current.CancellationToken);

        // Assert
        message.Should().Be("Your subscription is confirmed.");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task ConfirmNewsletterAsync_ErrorStatus_Throws(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.ConfirmNewsletterAsync("a@b.c", "bad", TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public async Task RequestNewsletterManageLinkAsync_BadRequest_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.BadRequest);

        // Act
        var act = () => fixture.Client.RequestNewsletterManageLinkAsync("not-an-email", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetNewsletterManagePreferencesAsync_NotFound_ReturnsNull()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var result = await fixture.Client.GetNewsletterManagePreferencesAsync("a@b.c", "bad", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetNewsletterManagePreferencesAsync_ServerError_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        var act = () => fixture.Client.GetNewsletterManagePreferencesAsync("a@b.c", "t", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task UpdateNewsletterManagePreferencesAsync_NotFound_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var act = () => fixture.Client.UpdateNewsletterManagePreferencesAsync("a@b.c", "bad", null, [], [], TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetNewsletterSectionsAsync_Success_ReturnsSections()
    {
        // Arrange
        using var fixture = Fixture.Create(body: JsonSerializer.Serialize(new[]
        {
            new Section("ai", "AI", "AI", "/ai", "AI", [new Collection("news", "News", "/ai/news", "News", "News")])
        }));

        // Act
        var sections = await fixture.Client.GetNewsletterSectionsAsync(TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/newsletter/sections");
        sections.Should().ContainSingle().Which.Name.Should().Be("ai");
    }

    [Fact]
    public async Task GetNewsletterSubscribersAsync_AllFilters_BuildsEscapedQuery()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "[]");

        // Act
        await fixture.Client.GetNewsletterSubscribersAsync(2, 50, "j&d\r\n", true, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/newsletter/subscribers?page=2&pageSize=50&search=j%26d&confirmed=true");
    }

    [Fact]
    public async Task GetNewsletterSubscribersAsync_NoFilters_OmitsOptionalParameters()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "null");

        // Act
        var result = await fixture.Client.GetNewsletterSubscribersAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/newsletter/subscribers?page=1&pageSize=200");
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetNewsletterSubscribersAsync_ConfirmedFalse_SendsLowercaseFalse()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "[]");

        // Act
        await fixture.Client.GetNewsletterSubscribersAsync(confirmed: false, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().EndWith("&confirmed=false");
    }

    [Fact]
    public async Task UpdateNewsletterSubscriberAsync_Success_PutsSanitizedPayload()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        await fixture.Client.UpdateNewsletterSubscriberAsync(12, "Jane\r\n", ["ai\r\n"], ["azure"], TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("PUT");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/newsletter/subscribers/12");
        fixture.Handler.Json.GetProperty("displayName").GetString().Should().Be("Jane");
        fixture.Handler.Json.GetProperty("weeklySections")[0].GetString().Should().Be("ai");
        fixture.Handler.Json.GetProperty("dailySections")[0].GetString().Should().Be("azure");
    }

    [Fact]
    public async Task UpdateNewsletterSubscriberAsync_NotFound_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var act = () => fixture.Client.UpdateNewsletterSubscriberAsync(12, null, [], [], TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task DeleteNewsletterSubscriberAsync_Success_SendsDelete()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        await fixture.Client.DeleteNewsletterSubscriberAsync(12, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("DELETE");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/newsletter/subscribers/12");
    }

    [Fact]
    public async Task DeleteNewsletterSubscriberAsync_NotFound_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var act = () => fixture.Client.DeleteNewsletterSubscriberAsync(12, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetNewsletterSendLogAsync_UsesCountAndHandlesNullBody()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "null");

        // Act
        var result = await fixture.Client.GetNewsletterSendLogAsync(10, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/newsletter/send-log?count=10");
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("weekly", "weekly")]
    [InlineData("DAILY", "daily")]
    [InlineData("  daily  ", "daily")]
    [InlineData("anything-else", "weekly")]
    public async Task TriggerNewsletterTestSendAsync_NormalizesKind(string kind, string expectedKind)
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.OK);

        // Act
        await fixture.Client.TriggerNewsletterTestSendAsync("a@b.c", [], kind, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be($"/api/admin/newsletter/test-send?email=a%40b.c&kind={expectedKind}");
    }

    [Fact]
    public async Task TriggerNewsletterTestSendAsync_BadRequest_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.BadRequest);

        // Act
        var act = () => fixture.Client.TriggerNewsletterTestSendAsync("bad", [], "weekly", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    // ── Processed URLs, jobs, settings, custom pages, misc admin ────────────

    [Fact]
    public async Task GetProcessedUrlsAsync_AllFilters_BuildsEscapedQuery()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"items":[],"totalCount":0}""");

        // Act
        await fixture.Client.GetProcessedUrlsAsync(
            page: 3,
            pageSize: 10,
            status: "failed",
            search: "x y",
            feedName: "Feed & Co",
            collectionName: "news",
            jobId: 77,
            sectionName: "azure",
            primarySectionOnly: true,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be(
            "/api/admin/processed-urls?page=3&pageSize=10&status=failed&search=x%20y&feedName=Feed%20%26%20Co&collectionName=news&jobId=77&sectionName=azure&primarySectionOnly=true");
    }

    [Fact]
    public async Task GetProcessedUrlsAsync_NullBody_ReturnsEmptyPage()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "null");

        // Act
        var result = await fixture.Client.GetProcessedUrlsAsync(cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.Items.Should().BeEmpty();
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.NotFound, false)]
    [InlineData(HttpStatusCode.InternalServerError, false)]
    public async Task DeleteProcessedUrlAsync_MapsStatusToBool(HttpStatusCode status, bool expected)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var result = await fixture.Client.DeleteProcessedUrlAsync("https://example.com/a?b=1", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("DELETE");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/processed-urls?url=https%3A%2F%2Fexample.com%2Fa%3Fb%3D1");
        result.Should().Be(expected);
    }

    [Fact]
    public async Task DeleteAllFailedProcessedUrlsAsync_Success_ReturnsDeletedCount()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"deleted":9}""");

        // Act
        var count = await fixture.Client.DeleteAllFailedProcessedUrlsAsync(TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/processed-urls/failed");
        count.Should().Be(9);
    }

    [Fact]
    public async Task DeleteAllFailedProcessedUrlsAsync_ServerError_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        var act = () => fixture.Client.DeleteAllFailedProcessedUrlsAsync(TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetCustomPageEntryAsync_NotFound_ReturnsNullAndEscapesKey()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var result = await fixture.Client.GetCustomPageEntryAsync("a/b c", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/custom-pages/a%2Fb%20c");
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCustomPageAsync_Success_PutsJsonData()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"key":"levels","description":"Levels","jsonData":"{}"}""");

        // Act
        var result = await fixture.Client.UpdateCustomPageAsync("levels", "{}", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("PUT");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/custom-pages/levels");
        fixture.Handler.Json.GetProperty("jsonData").GetString().Should().Be("{}");
        result.Key.Should().Be("levels");
    }

    [Fact]
    public async Task UpdateCustomPageAsync_BadRequest_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.BadRequest);

        // Act
        var act = () => fixture.Client.UpdateCustomPageAsync("levels", "{not json", TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetJobSettingsAsync_Success_ReturnsSettings()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """[{"jobName":"ContentProcessor","enabled":true,"description":"Processes content"}]""");

        // Act
        var settings = await fixture.Client.GetJobSettingsAsync(TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/job-settings");
        settings.Should().ContainSingle().Which.JobName.Should().Be("ContentProcessor");
    }

    [Fact]
    public async Task UpdateJobSettingAsync_Success_PutsEnabledFlag()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        await fixture.Client.UpdateJobSettingAsync("Content Processor", false, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("PUT");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/job-settings/Content%20Processor");
        fixture.Handler.Json.GetProperty("enabled").GetBoolean().Should().BeFalse();
    }

    [Fact]
    public async Task UpdateJobSettingAsync_NotFound_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var act = () => fixture.Client.UpdateJobSettingAsync("Unknown", true, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task InvalidateCachesAsync_Success_PostsToInvalidateRoute()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.OK);

        // Act
        await fixture.Client.InvalidateCachesAsync(TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/cache/invalidate");
    }

    [Fact]
    public async Task GetProcessingJobByIdAsync_NotFound_ReturnsNull()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var job = await fixture.Client.GetProcessingJobByIdAsync(5, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/processing/jobs/5");
        job.Should().BeNull();
    }

    [Fact]
    public async Task GetProcessingJobsAsync_NullBody_ReturnsEmptyListAndUsesCount()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "null");

        // Act
        var jobs = await fixture.Client.GetProcessingJobsAsync(5, TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/processing/jobs?count=5");
        jobs.Should().BeEmpty();
    }

    [Theory]
    [InlineData("/api/admin/processing/trigger")]
    [InlineData("/api/admin/roundup/trigger")]
    [InlineData("/api/admin/content-fixer/trigger")]
    [InlineData("/api/admin/processing/cancel")]
    public async Task TriggerStyleCommands_PostToExpectedRouteAndThrowOnServerError(string expectedPath)
    {
        // Arrange
        using var okFixture = Fixture.Create(HttpStatusCode.Accepted);
        using var failingFixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        await InvokeTriggerAsync(okFixture.Client, expectedPath);
        var act = () => InvokeTriggerAsync(failingFixture.Client, expectedPath);

        // Assert
        okFixture.Handler.Method.Should().Be("POST");
        okFixture.Handler.PathAndQuery.Should().Be(expectedPath);
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task RegenerateRoundupAsync_Success_PostsEscapedQuery()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.Accepted);

        // Act
        await fixture.Client.RegenerateRoundupAsync("roundups", "weekly roundup/1", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/roundup/regenerate?collection=roundups&slug=weekly%20roundup%2F1");
    }

    [Fact]
    public async Task GetDatabaseStatisticsAsync_ServerError_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        var act = () => fixture.Client.GetDatabaseStatisticsAsync(TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    // ── VS Code updates ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetVscodeUpdateItemsAsync_WithSearch_BuildsEscapedQuery()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"items":[],"totalCount":0}""");

        // Act
        await fixture.Client.GetVscodeUpdateItemsAsync(2, 20, "release notes", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/vscode-updates?page=2&pageSize=20&search=release%20notes");
    }

    [Fact]
    public async Task GetPublicVscodeUpdatesAsync_NullBody_ReturnsEmptyList()
    {
        // Arrange
        using var fixture = Fixture.Create(body: "null");

        // Act
        var result = await fixture.Client.GetPublicVscodeUpdatesAsync(TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/vscode-updates");
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddVscodeUpdateItemAsync_Success_PostsCollectionAndSlug()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        await fixture.Client.AddVscodeUpdateItemAsync("videos", "vscode-1-95", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("POST");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/vscode-updates");
        fixture.Handler.Json.GetProperty("collectionName").GetString().Should().Be("videos");
        fixture.Handler.Json.GetProperty("slug").GetString().Should().Be("vscode-1-95");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.Conflict)]
    public async Task AddVscodeUpdateItemAsync_ErrorStatus_Throws(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.AddVscodeUpdateItemAsync("videos", "s", TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    [Fact]
    public async Task RemoveVscodeUpdateItemAsync_NotFound_ReturnsFalse()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NotFound);

        // Act
        var result = await fixture.Client.RemoveVscodeUpdateItemAsync("videos", "missing", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.Method.Should().Be("DELETE");
        fixture.Handler.PathAndQuery.Should().Be("/api/admin/vscode-updates?collection=videos&slug=missing");
        result.Should().BeFalse();
    }

    [Fact]
    public async Task RemoveVscodeUpdateItemAsync_Success_ReturnsTrue()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        var result = await fixture.Client.RemoveVscodeUpdateItemAsync("videos", "s", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task RemoveVscodeUpdateItemAsync_ServerError_Throws()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.InternalServerError);

        // Act
        var act = () => fixture.Client.RemoveVscodeUpdateItemAsync("videos", "s", TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    // ── Legacy redirect ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetLegacyRedirectAsync_Found_ReturnsResultAndSendsSectionHint()
    {
        // Arrange
        using var fixture = Fixture.Create(body: """{"url":"/ai/videos/my-article"}""");

        // Act
        var result = await fixture.Client.GetLegacyRedirectAsync("my-article", "ai", TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/legacy-redirect?slug=my-article&section=ai");
        result.Should().NotBeNull();
        result!.Url.Should().Be("/ai/videos/my-article");
    }

    [Fact]
    public async Task GetLegacyRedirectAsync_WithoutHint_OmitsSection()
    {
        // Arrange
        using var fixture = Fixture.Create(HttpStatusCode.NoContent);

        // Act
        var result = await fixture.Client.GetLegacyRedirectAsync("slug\r\n", cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        fixture.Handler.PathAndQuery.Should().Be("/api/legacy-redirect?slug=slug");
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(HttpStatusCode.NoContent)]
    [InlineData(HttpStatusCode.BadRequest)]
    public async Task GetLegacyRedirectAsync_NoContentOrBadRequest_ReturnsNull(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var result = await fixture.Client.GetLegacyRedirectAsync("slug", "ai", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task GetLegacyRedirectAsync_OtherErrorStatus_ThrowsSoCallerCanDegrade(HttpStatusCode status)
    {
        // Arrange
        using var fixture = Fixture.Create(status);

        // Act
        var act = () => fixture.Client.GetLegacyRedirectAsync("slug", null, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<HttpRequestException>()).Which.StatusCode.Should().Be(status);
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static Task InvokeTriggerAsync(TechHubApiClient client, string path) => path switch
    {
        "/api/admin/processing/trigger" => client.TriggerContentProcessingAsync(TestContext.Current.CancellationToken),
        "/api/admin/roundup/trigger" => client.TriggerRoundupGenerationAsync(TestContext.Current.CancellationToken),
        "/api/admin/content-fixer/trigger" => client.TriggerContentFixerAsync(TestContext.Current.CancellationToken),
        "/api/admin/processing/cancel" => client.CancelRunningJobAsync(TestContext.Current.CancellationToken),
        _ => throw new ArgumentOutOfRangeException(nameof(path), path, "Unknown trigger route")
    };

    private static ContentItemEditData CreateEditData() => new()
    {
        CollectionName = "blogs",
        Slug = "slug",
        Title = "Title",
        Author = "Author",
        Excerpt = "Excerpt",
        Content = "Content",
        PrimarySectionName = "ai",
        Tags = ["ai"],
        Sections = ["ai"]
    };

    private sealed class Fixture : IDisposable
    {
        private readonly HttpClient _httpClient;

        private Fixture(RecordingHandler handler)
        {
            Handler = handler;
            _httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://localhost:5003") };
            Client = new TechHubApiClient(_httpClient, NullLogger<TechHubApiClient>.Instance);
        }

        public RecordingHandler Handler { get; }

        public TechHubApiClient Client { get; }

        public static Fixture Create(HttpStatusCode status = HttpStatusCode.OK, string body = "null", Exception? throws = null) =>
            new(new RecordingHandler(status, body, throws));

        public void Dispose()
        {
            _httpClient.Dispose();
            Handler.Dispose();
        }
    }

    private sealed class RecordingHandler(HttpStatusCode status, string body, Exception? throws) : HttpMessageHandler
    {
        public int RequestCount { get; private set; }

        public string? Method { get; private set; }

        public string? PathAndQuery { get; private set; }

        public string? Body { get; private set; }

        public JsonElement Json
        {
            get
            {
                using var document = JsonDocument.Parse(Body ?? "null");
                return document.RootElement.Clone();
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestCount++;
            Method = request.Method.Method;
            PathAndQuery = request.RequestUri?.PathAndQuery;
            Body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);

            if (throws is not null)
            {
                throw throws;
            }

            return new HttpResponseMessage(status)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
        }
    }
}
