using System.Data;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for Admin API endpoints.
/// Validates authentication filter, processing job endpoints, and RSS feed CRUD.
/// </summary>
public class AdminEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;
    private readonly TechHubIntegrationTestApiFactory _factory;

    public AdminEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _factory = factory;
        _client = factory.CreateClient();
    }

    // ── Admin Authorization ────────────────────────────────────────────────

    [Fact]
    public async Task AdminEndpoints_WithoutAuth_WhenAzureAdNotConfigured_ReturnsOk()
    {
        // Arrange — no AzureAd:ClientId is set in test configuration,
        // so the AdminOnly policy allows all requests (local dev bypass)

        // Act
        var response = await _client.GetAsync(
            "/api/admin/processing/jobs",
            TestContext.Current.CancellationToken);

        // Assert — should pass because Azure AD is not configured
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── Processing Job Endpoints ─────────────────────────────────────────────

    [Fact]
    public async Task GetProcessingJobs_ReturnsEmptyListInitially()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/processing/jobs",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var jobs = await response.Content.ReadFromJsonAsync<List<object>>(
            TestContext.Current.CancellationToken);
        jobs.Should().NotBeNull();
    }

    [Fact]
    public async Task GetProcessingJobById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/processing/jobs/999999",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task TriggerRoundupGeneration_ReturnsAccepted()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/admin/roundup/trigger",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }

    // ── RSS Feed CRUD Endpoints ──────────────────────────────────────────────

    [Fact]
    public async Task CreateFeed_WithValidData_ReturnsCreated()
    {
        // Arrange
        var request = new { Name = "API Test Feed", Url = "https://example.com/api-test-feed.xml", OutputDir = "_blogs", Enabled = true };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/feeds",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var feed = await response.Content.ReadFromJsonAsync<FeedConfig>(
            TestContext.Current.CancellationToken);
        feed.Should().NotBeNull();
        feed!.Id.Should().BeGreaterThan(0);
        feed.Name.Should().Be("API Test Feed");
        feed.Url.Should().Be("https://example.com/api-test-feed.xml");
        feed.OutputDir.Should().Be("_blogs");
        feed.Enabled.Should().BeTrue();
    }

    [Fact]
    public async Task CreateFeed_WithMissingName_ReturnsBadRequest()
    {
        // Arrange
        var request = new { Url = "https://example.com/feed.xml", OutputDir = "_blogs" };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/feeds",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateFeed_WithInvalidUrl_ReturnsBadRequest()
    {
        // Arrange
        var request = new { Name = "Bad URL Feed", Url = "not-a-valid-url", OutputDir = "_blogs" };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/feeds",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetFeeds_AfterCreate_ContainsCreatedFeed()
    {
        // Arrange — create a feed first
        var request = new { Name = "List Test Feed", Url = "https://example.com/list-test.xml", OutputDir = "_news", Enabled = true };
        var createResponse = await _client.PostAsJsonAsync(
            "/api/admin/feeds",
            request,
            TestContext.Current.CancellationToken);
        createResponse.EnsureSuccessStatusCode();

        // Act
        var response = await _client.GetAsync(
            "/api/admin/feeds",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var feeds = await response.Content.ReadFromJsonAsync<List<FeedConfig>>(
            TestContext.Current.CancellationToken);
        feeds.Should().NotBeNull();
        feeds!.Should().Contain(f => f.Name == "List Test Feed");
    }

    [Fact]
    public async Task GetFeedById_WithExistingFeed_ReturnsFeed()
    {
        // Arrange
        var request = new { Name = "GetById Feed", Url = "https://example.com/getbyid.xml", OutputDir = "_blogs", Enabled = true };
        var createResponse = await _client.PostAsJsonAsync(
            "/api/admin/feeds",
            request,
            TestContext.Current.CancellationToken);
        var created = await createResponse.Content.ReadFromJsonAsync<FeedConfig>(
            TestContext.Current.CancellationToken);

        // Act
        var response = await _client.GetAsync(
            $"/api/admin/feeds/{created!.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var feed = await response.Content.ReadFromJsonAsync<FeedConfig>(
            TestContext.Current.CancellationToken);
        feed.Should().NotBeNull();
        feed!.Name.Should().Be("GetById Feed");
    }

    [Fact]
    public async Task GetFeedById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/feeds/999999",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateFeed_WithValidData_ReturnsOk()
    {
        // Arrange
        var request = new { Name = "Update Feed", Url = "https://example.com/update.xml", OutputDir = "_blogs", Enabled = true };
        var createResponse = await _client.PostAsJsonAsync(
            "/api/admin/feeds",
            request,
            TestContext.Current.CancellationToken);
        var created = await createResponse.Content.ReadFromJsonAsync<FeedConfig>(
            TestContext.Current.CancellationToken);

        var updateRequest = new { Name = "Updated Feed Name", Url = "https://example.com/updated.xml", OutputDir = "_news", Enabled = false };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/feeds/{created!.Id}",
            updateRequest,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<FeedConfig>(
            TestContext.Current.CancellationToken);
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Updated Feed Name");
        updated.Url.Should().Be("https://example.com/updated.xml");
        updated.OutputDir.Should().Be("_news");
        updated.Enabled.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateFeed_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var request = new { Name = "Ghost Feed", Url = "https://example.com/ghost.xml", OutputDir = "_blogs", Enabled = true };

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/feeds/999999",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteFeed_WithExistingFeed_ReturnsNoContent()
    {
        // Arrange
        var request = new { Name = "Delete Feed", Url = "https://example.com/delete.xml", OutputDir = "_blogs", Enabled = true };
        var createResponse = await _client.PostAsJsonAsync(
            "/api/admin/feeds",
            request,
            TestContext.Current.CancellationToken);
        var created = await createResponse.Content.ReadFromJsonAsync<FeedConfig>(
            TestContext.Current.CancellationToken);

        // Act
        var response = await _client.DeleteAsync(
            $"/api/admin/feeds/{created!.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify it's deleted
        var getResponse = await _client.GetAsync(
            $"/api/admin/feeds/{created.Id}",
            TestContext.Current.CancellationToken);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteFeed_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync(
            "/api/admin/feeds/999999",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task TriggerProcessing_ReturnsAccepted()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/admin/processing/trigger",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }

    // ── Processed URL Endpoints ──────────────────────────────────────────────

    [Fact]
    public async Task GetProcessedUrls_ReturnsPagedResult()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/processed-urls",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<ProcessedUrlListItem>>(
            TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Items.Should().NotBeNull();
        result.TotalCount.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task GetProcessedUrls_WithValidStatusFilter_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/processed-urls?status=succeeded",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetProcessedUrls_WithInvalidStatus_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/processed-urls?status=invalid",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetProcessedUrls_WithPagination_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/processed-urls?page=1&pageSize=10",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<ProcessedUrlListItem>>(
            TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetProcessedUrls_WithSearchFilter_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/processed-urls?search=example.com",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteProcessedUrl_WithMissingUrl_ReturnsBadRequest()
    {
        // Act
        var response = await _client.DeleteAsync(
            "/api/admin/processed-urls",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteProcessedUrl_WithNonExistentUrl_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync(
            "/api/admin/processed-urls?url=https://nonexistent.example.com/page",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProcessedUrl_InvalidatesContentCache()
    {
        // Arrange — seed a content item in a non-external collection (videos link internally)
        // so we can verify it via the slug detail endpoint, which uses the content cache.
        const string testUrl = "/all/videos/cache-invalidation-test";
        const string slug = "cache-invalidation-test";
        const string collectionName = "videos";

        using (var scope = _factory.Services.CreateScope())
        {
            var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            await connection.ExecuteAsync(
                @"INSERT INTO content_items
                    (slug, collection_name, title, content, excerpt, date_epoch,
                     primary_section_name, external_url, author, feed_name,
                     tags_csv, is_ai, sections_bitmask, content_hash)
                  VALUES
                    (@Slug, @Collection, 'Cache Invalidation Test', '# Test', 'Test excerpt',
                     1700000000, 'ai', @Url, 'Test Author', 'Test Feed',
                     ',ai,', TRUE, 1, 'testhash')
                  ON CONFLICT (external_url) DO NOTHING",
                new { Slug = slug, Collection = collectionName, Url = testUrl });

            await connection.ExecuteAsync(
                @"INSERT INTO processed_urls (external_url, status, feed_name, collection_name)
                  VALUES (@Url, 'succeeded', 'Test Feed', @Collection)
                  ON CONFLICT (external_url) DO NOTHING",
                new { Url = testUrl, Collection = collectionName });
        }

        // Act — populate the cache by fetching the item detail, then delete via admin
        var beforeResponse = await _client.GetAsync(
            $"/api/sections/ai/collections/{collectionName}/{slug}",
            TestContext.Current.CancellationToken);
        beforeResponse.StatusCode.Should().Be(HttpStatusCode.OK,
            "the seeded item should be returned before deletion");

        var deleteResponse = await _client.DeleteAsync(
            $"/api/admin/processed-urls?url={Uri.EscapeDataString(testUrl)}",
            TestContext.Current.CancellationToken);
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert — the same request must now return 404 (cache was invalidated)
        var afterResponse = await _client.GetAsync(
            $"/api/sections/ai/collections/{collectionName}/{slug}",
            TestContext.Current.CancellationToken);
        afterResponse.StatusCode.Should().Be(HttpStatusCode.NotFound,
            "the deleted item should not be returned after cache invalidation");
    }

    [Fact]
    public async Task UpdateContentItemEditData_InvalidatesContentCache()
    {
        // Arrange — seed a content item in a non-external collection (videos link internally)
        // so we can verify cache invalidation via the slug detail endpoint.
        const string testUrl = "/all/videos/edit-cache-test";
        const string slug = "edit-cache-test";
        const string collectionName = "videos";

        using (var scope = _factory.Services.CreateScope())
        {
            var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            await connection.ExecuteAsync(
                @"INSERT INTO content_items
                    (slug, collection_name, title, content, excerpt, date_epoch,
                     primary_section_name, external_url, author, feed_name,
                     tags_csv, is_ai, sections_bitmask, content_hash)
                  VALUES
                    (@Slug, @Collection, 'Original Title', '# Original', 'Original excerpt',
                     1700000000, 'ai', @Url, 'Test Author', 'Test Feed',
                     ',ai,', TRUE, 1, 'testhash')
                  ON CONFLICT (external_url) DO NOTHING",
                new { Slug = slug, Collection = collectionName, Url = testUrl });
        }

        // Populate the slug cache by fetching the item
        var beforeResponse = await _client.GetAsync(
            $"/api/sections/ai/collections/{collectionName}/{slug}",
            TestContext.Current.CancellationToken);
        beforeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var beforeItem = await beforeResponse.Content.ReadFromJsonAsync<JsonElement>(
            TestContext.Current.CancellationToken);
        beforeItem.GetProperty("title").GetString().Should().Be("Original Title");

        // Act — update the title via admin endpoint
        var editData = new
        {
            CollectionName = collectionName,
            Slug = slug,
            Title = "Updated Title",
            Author = "Test Author",
            Excerpt = "Updated excerpt",
            Content = "# Updated",
            PrimarySectionName = "ai",
            Tags = new[] { "AI" },
            Sections = new[] { "ai" },
            AiMetadata = (string?)null
        };

        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/admin/content-items/edit-data?collection={Uri.EscapeDataString(collectionName)}&slug={Uri.EscapeDataString(slug)}",
            editData,
            TestContext.Current.CancellationToken);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert — fetching the same slug should return the updated title (cache was invalidated)
        var afterResponse = await _client.GetAsync(
            $"/api/sections/ai/collections/{collectionName}/{slug}",
            TestContext.Current.CancellationToken);
        afterResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var afterItem = await afterResponse.Content.ReadFromJsonAsync<JsonElement>(
            TestContext.Current.CancellationToken);
        afterItem.GetProperty("title").GetString().Should().Be("Updated Title",
            "the updated title should be returned after cache invalidation");
    }

    [Fact]
    public async Task DeleteAllFailedUrls_ReturnsDeletedCount()
    {
        // Act
        var response = await _client.DeleteAsync(
            "/api/admin/processed-urls/failed",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(
            TestContext.Current.CancellationToken);
        body.GetProperty("deleted").GetInt32().Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task GetProcessedUrls_WithFeedMetadata_ReturnsFeedAndCollection()
    {
        // Arrange — seed a processed URL with feed metadata stored directly
        const string testUrl = "https://example.com/feed-meta-test";

        using (var scope = _factory.Services.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<IProcessedUrlRepository>();
            await repo.RecordSuccessAsync(testUrl, feedName: "Test Feed", collectionName: "blogs",
                ct: TestContext.Current.CancellationToken);
        }

        // Act
        var response = await _client.GetAsync(
            $"/api/admin/processed-urls?search={Uri.EscapeDataString(testUrl)}",
            TestContext.Current.CancellationToken);

        // Assert — verify raw JSON contains feedName and collectionName
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var doc = JsonDocument.Parse(json);
        var items = doc.RootElement.GetProperty("items");
        items.GetArrayLength().Should().BeGreaterThan(0);

        var first = items[0];
        first.GetProperty("externalUrl").GetString().Should().Be(testUrl);
        first.GetProperty("feedName").GetString().Should().Be("Test Feed");
        first.GetProperty("collectionName").GetString().Should().Be("blogs");
    }

    [Fact]
    public async Task GetProcessedUrls_WithFeedMetadata_DeserializesToModel()
    {
        // Arrange — seed a processed URL with feed metadata stored directly
        const string testUrl = "https://example.com/feed-meta-deserialize";

        using (var scope = _factory.Services.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<IProcessedUrlRepository>();
            await repo.RecordSuccessAsync(testUrl, feedName: "Test Feed", collectionName: "blogs",
                ct: TestContext.Current.CancellationToken);
        }

        // Act — deserialize using the same type as the Web client
        var result = await _client.GetFromJsonAsync<PagedResult<ProcessedUrlListItem>>(
            $"/api/admin/processed-urls?search={Uri.EscapeDataString(testUrl)}",
            TestContext.Current.CancellationToken);

        // Assert — FeedName and CollectionName must survive deserialization
        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty();

        var first = result.Items[0];
        first.ExternalUrl.Should().Be(testUrl);
        first.FeedName.Should().Be("Test Feed");
        first.CollectionName.Should().Be("blogs");
    }

    // ── Background Job Settings Endpoints ────────────────────────────────────

    [Fact]
    public async Task GetJobSettings_ReturnsSeededSettings()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/job-settings",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var settings = await response.Content.ReadFromJsonAsync<List<BackgroundJobSetting>>(
            TestContext.Current.CancellationToken);
        settings.Should().NotBeNull();
        settings!.Should().Contain(s => s.JobName == "ContentProcessor");
        settings.Should().Contain(s => s.JobName == "RoundupGenerator");
    }

    [Fact]
    public async Task UpdateJobSetting_WithValidJob_ReturnsNoContent()
    {
        // Act — toggle ContentProcessor to enabled
        var response = await _client.PutAsJsonAsync(
            "/api/admin/job-settings/ContentProcessor",
            new { Enabled = true },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the change persisted
        var getResponse = await _client.GetAsync(
            "/api/admin/job-settings",
            TestContext.Current.CancellationToken);
        var settings = await getResponse.Content.ReadFromJsonAsync<List<BackgroundJobSetting>>(
            TestContext.Current.CancellationToken);
        settings!.First(s => s.JobName == "ContentProcessor").Enabled.Should().BeTrue();

        // Clean up — reset to disabled
        await _client.PutAsJsonAsync(
            "/api/admin/job-settings/ContentProcessor",
            new { Enabled = false },
            TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task UpdateJobSetting_WithNonExistentJob_ReturnsNotFound()
    {
        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/job-settings/NonExistentJob",
            new { Enabled = true },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── Cache Invalidation Endpoints ─────────────────────────────────────────

    [Fact]
    public async Task InvalidateCache_ReturnsOk()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/admin/cache/invalidate",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(
            TestContext.Current.CancellationToken);
        body.GetProperty("message").GetString().Should().Be("All caches invalidated");
    }

    // ── Content Review Endpoints ─────────────────────────────────────────────

    [Fact]
    public async Task GetReviews_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/reviews",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var reviews = await response.Content.ReadFromJsonAsync<List<ContentReview>>(
            TestContext.Current.CancellationToken);
        reviews.Should().NotBeNull();
    }

    [Fact]
    public async Task GetReviews_WithValidStatusFilter_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/reviews?status=pending",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetReviews_WithInvalidStatus_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/reviews?status=invalid",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetReviewSummary_ReturnsSummaryCounts()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/reviews/summary",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var summary = await response.Content.ReadFromJsonAsync<ContentReviewSummary>(
            TestContext.Current.CancellationToken);
        summary.Should().NotBeNull();
        summary!.Pending.Should().BeGreaterThanOrEqualTo(0);
        summary.Approved.Should().BeGreaterThanOrEqualTo(0);
        summary.Rejected.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task ApproveReview_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/admin/reviews/999999/approve",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RejectReview_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/admin/reviews/999999/reject",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ApproveReview_WithSeededReview_ApprovesAndAppliesChange()
    {
        // Arrange — seed a content item and a pending review
        const string slug = "review-approve-test";
        const string collection = "blogs";
        const string testUrl = "https://example.com/review-approve-test";

        using (var scope = _factory.Services.CreateScope())
        {
            var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            await connection.ExecuteAsync(
                @"INSERT INTO content_items
                    (slug, collection_name, title, content, excerpt, date_epoch,
                     primary_section_name, external_url, author, feed_name,
                     tags_csv, is_ai, sections_bitmask, content_hash)
                  VALUES
                    (@Slug, @Collection, 'Review Test', '# Test', 'Test excerpt',
                     1700000000, 'ai', @Url, 'Test Author', 'Test Feed',
                     ',ai,', TRUE, 1, 'reviewhash')
                  ON CONFLICT (external_url) DO NOTHING",
                new { Slug = slug, Collection = collection, Url = testUrl });

            var reviewRepo = scope.ServiceProvider.GetRequiredService<IContentReviewRepository>();
            await reviewRepo.CreateAsync(slug, collection, "tags", ",ai,", ",ai,copilot,",
                ct: TestContext.Current.CancellationToken);
        }

        // Get the most recent pending review
        var getResponse = await _client.GetAsync(
            "/api/admin/reviews?status=pending",
            TestContext.Current.CancellationToken);
        var reviews = await getResponse.Content.ReadFromJsonAsync<List<ContentReview>>(
            TestContext.Current.CancellationToken);
        var review = reviews!.FirstOrDefault(r => r.Slug == slug);
        review.Should().NotBeNull();

        // Act
        var response = await _client.PostAsync(
            $"/api/admin/reviews/{review!.Id}/approve",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(
            TestContext.Current.CancellationToken);
        body.GetProperty("message").GetString().Should().Contain("approved");
    }

    [Fact]
    public async Task RejectReview_WithSeededReview_RejectsWithoutChangingContent()
    {
        // Arrange — seed a pending review
        const string slug = "review-reject-test";
        const string collection = "blogs";
        const string testUrl = "https://example.com/review-reject-test";

        using (var scope = _factory.Services.CreateScope())
        {
            var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            await connection.ExecuteAsync(
                @"INSERT INTO content_items
                    (slug, collection_name, title, content, excerpt, date_epoch,
                     primary_section_name, external_url, author, feed_name,
                     tags_csv, is_ai, sections_bitmask, content_hash)
                  VALUES
                    (@Slug, @Collection, 'Reject Test', '# Test', 'Test excerpt',
                     1700000000, 'ai', @Url, 'Test Author', 'Test Feed',
                     ',ai,', TRUE, 1, 'rejecthash')
                  ON CONFLICT (external_url) DO NOTHING",
                new { Slug = slug, Collection = collection, Url = testUrl });

            var reviewRepo = scope.ServiceProvider.GetRequiredService<IContentReviewRepository>();
            await reviewRepo.CreateAsync(slug, collection, "author", "OldAuthor", "NewAuthor",
                ct: TestContext.Current.CancellationToken);
        }

        // Get the pending review
        var getResponse = await _client.GetAsync(
            "/api/admin/reviews?status=pending",
            TestContext.Current.CancellationToken);
        var reviews = await getResponse.Content.ReadFromJsonAsync<List<ContentReview>>(
            TestContext.Current.CancellationToken);
        var review = reviews!.FirstOrDefault(r => r.Slug == slug);
        review.Should().NotBeNull();

        // Act
        var response = await _client.PostAsync(
            $"/api/admin/reviews/{review!.Id}/reject",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ApproveAllReviews_ReturnsApprovedCount()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/admin/reviews/approve-all",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(
            TestContext.Current.CancellationToken);
        body.GetProperty("approved").GetInt32().Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task RejectAllReviews_ReturnsRejectedCount()
    {
        // Act
        var response = await _client.PostAsync(
            "/api/admin/reviews/reject-all",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(
            TestContext.Current.CancellationToken);
        body.GetProperty("rejected").GetInt32().Should().BeGreaterThanOrEqualTo(0);
    }

    // ── Content Preview Endpoints ────────────────────────────────────────────

    [Fact]
    public async Task PreviewMarkdown_WithValidMarkdown_ReturnsHtml()
    {
        // Arrange
        var request = new { Markdown = "# Hello World\n\nThis is a **test**." };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/content-items/preview-markdown",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(
            TestContext.Current.CancellationToken);
        var html = body.GetProperty("html").GetString();
        html.Should().NotBeNullOrEmpty();
        html.Should().Contain("<h1");
        html.Should().Contain("Hello World");
        html.Should().Contain("<strong>test</strong>");
    }

    [Fact]
    public async Task PreviewMarkdown_WithEmptyMarkdown_ReturnsBadRequest()
    {
        // Arrange
        var request = new { Markdown = "" };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/content-items/preview-markdown",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── Update Review Fixed Value Endpoints ──────────────────────────────────

    [Fact]
    public async Task UpdateReviewFixedValue_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var request = new { FixedValue = "updated content" };

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/reviews/999999",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateReviewFixedValue_WithSeededReview_UpdatesFixedValue()
    {
        // Arrange — seed a content item and a pending review
        const string slug = "review-edit-test";
        const string collection = "blogs";
        const string testUrl = "https://example.com/review-edit-test";
        const string newFixedValue = "corrected markdown content";

        using (var scope = _factory.Services.CreateScope())
        {
            var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            await connection.ExecuteAsync(
                @"INSERT INTO content_items
                    (slug, collection_name, title, content, excerpt, date_epoch,
                     primary_section_name, external_url, author, feed_name,
                     tags_csv, is_ai, sections_bitmask, content_hash)
                  VALUES
                    (@Slug, @Collection, 'Edit Test', '# Test', 'Test excerpt',
                     1700000000, 'ai', @Url, 'Test Author', 'Test Feed',
                     ',ai,', TRUE, 1, 'edithash')
                  ON CONFLICT (external_url) DO NOTHING",
                new { Slug = slug, Collection = collection, Url = testUrl });

            var reviewRepo = scope.ServiceProvider.GetRequiredService<IContentReviewRepository>();
            await reviewRepo.CreateAsync(slug, collection, "markdown", "old content", "auto-fixed content",
                ct: TestContext.Current.CancellationToken);
        }

        // Get the pending review
        var getResponse = await _client.GetAsync(
            "/api/admin/reviews?status=pending",
            TestContext.Current.CancellationToken);
        var reviews = await getResponse.Content.ReadFromJsonAsync<List<ContentReview>>(
            TestContext.Current.CancellationToken);
        var review = reviews!.FirstOrDefault(r => r.Slug == slug);
        review.Should().NotBeNull();

        // Act
        var request = new { FixedValue = newFixedValue };
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/reviews/{review!.Id}",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateReviewFixedValue_WithNullFixedValue_ReturnsBadRequest()
    {
        // Arrange — send request without FixedValue
        var request = new { };

        // Act
        var response = await _client.PutAsJsonAsync(
            "/api/admin/reviews/1",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetReviews_IncludesPrimarySectionName()
    {
        // Arrange — seed a content item and review so we can verify PrimarySectionName is returned
        const string slug = "review-section-test";
        const string collection = "blogs";
        const string testUrl = "https://example.com/review-section-test";

        using (var scope = _factory.Services.CreateScope())
        {
            var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            await connection.ExecuteAsync(
                @"INSERT INTO content_items
                    (slug, collection_name, title, content, excerpt, date_epoch,
                     primary_section_name, external_url, author, feed_name,
                     tags_csv, is_ai, sections_bitmask, content_hash)
                  VALUES
                    (@Slug, @Collection, 'Section Test', '# Test', 'Test excerpt',
                     1700000000, 'github-copilot', @Url, 'Test Author', 'Test Feed',
                     ',ai,', TRUE, 1, 'sectionhash')
                  ON CONFLICT (external_url) DO NOTHING",
                new { Slug = slug, Collection = collection, Url = testUrl });

            var reviewRepo = scope.ServiceProvider.GetRequiredService<IContentReviewRepository>();
            await reviewRepo.CreateAsync(slug, collection, "tags", ",ai,", ",ai,copilot,",
                ct: TestContext.Current.CancellationToken);
        }

        // Act
        var response = await _client.GetAsync(
            "/api/admin/reviews?status=pending",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var reviews = await response.Content.ReadFromJsonAsync<List<ContentReview>>(
            TestContext.Current.CancellationToken);
        var review = reviews!.FirstOrDefault(r => r.Slug == slug);
        review.Should().NotBeNull();
        review!.PrimarySectionName.Should().Be("github-copilot");
    }
}
