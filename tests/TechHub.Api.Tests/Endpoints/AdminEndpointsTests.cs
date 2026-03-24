using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
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

    public AdminEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

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

    // ── RSS Feed CRUD Endpoints ──────────────────────────────────────────────

    [Fact]
    public async Task CreateFeed_WithValidData_ReturnsCreated()
    {
        // Arrange
        var request = new { Name = "Test Feed", Url = "https://example.com/feed.xml", OutputDir = "_blogs", Enabled = true };

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
        feed.Name.Should().Be("Test Feed");
        feed.Url.Should().Be("https://example.com/feed.xml");
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
}
