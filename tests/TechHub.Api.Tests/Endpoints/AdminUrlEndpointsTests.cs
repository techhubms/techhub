using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for GET /api/admin/urls/title and the outcome paths of POST /api/admin/urls/process.
/// Outbound HTTP and AI calls are replaced by stateless stubs (see <see cref="TechHubStubbedExternalServicesApiFactory"/>),
/// so no test touches the network.
/// </summary>
public class AdminUrlEndpointsTests : IClassFixture<TechHubStubbedExternalServicesApiFactory>
{
    private const int MaxTranscriptLength = 50_000;

    private readonly HttpClient _client;

    public AdminUrlEndpointsTests(TechHubStubbedExternalServicesApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    // ── GET /api/admin/urls/title ────────────────────────────────────────────

    [Theory]
    [InlineData("ftp://example.com/file")]
    [InlineData("javascript:alert(1)")]
    [InlineData("file:///etc/passwd")]
    [InlineData("not-a-url")]
    [InlineData("/relative/path")]
    [InlineData("")]
    [InlineData("   ")]
    public async Task FetchUrlTitle_InvalidOrNonHttpUrl_ReturnsBadRequest(string url)
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/admin/urls/title?url={Uri.EscapeDataString(url)}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task FetchUrlTitle_MissingUrlParameter_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/urls/title",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(TechHubStubbedExternalServicesApiFactory.FetchBasicTitleMarker, "Hello & Welcome")]
    [InlineData(TechHubStubbedExternalServicesApiFactory.FetchAttributedTitleMarker, "Attributed <Title>")]
    public async Task FetchUrlTitle_PageWithTitle_ReturnsDecodedAndTrimmedTitle(string marker, string expectedTitle)
    {
        // Act
        var response = await _client.GetAsync(
            TitleUrl($"https://example.com/{marker}-{NewId()}"),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        body.GetProperty("title").GetString().Should().Be(expectedTitle);
    }

    [Theory]
    [InlineData(TechHubStubbedExternalServicesApiFactory.FetchNoTitleMarker)]
    [InlineData(TechHubStubbedExternalServicesApiFactory.FetchNullMarker)]
    [InlineData(TechHubStubbedExternalServicesApiFactory.FetchThrowsMarker)]
    public async Task FetchUrlTitle_NoTitleOrFetchFailure_ReturnsOkWithNullTitle(string marker)
    {
        // Act
        var response = await _client.GetAsync(
            TitleUrl($"https://example.com/{marker}-{NewId()}"),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
        body.GetProperty("title").ValueKind.Should().Be(JsonValueKind.Null);
    }

    // ── POST /api/admin/urls/process ─────────────────────────────────────────

    [Fact]
    public async Task ProcessUrl_TranscriptOverLimit_ReturnsBadRequest()
    {
        // Arrange
        var request = new
        {
            Url = $"https://example.com/articles/{NewId()}",
            CollectionName = "videos",
            FeedName = "Test Feed",
            Transcript = new string('a', MaxTranscriptLength + 1)
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ProcessUrl_WhenAiIncludesContent_ReturnsOkAddedAndPersistsItem()
    {
        // Arrange — mixed-case collection verifies that the name is normalised to lowercase
        var slug = $"stub-ok-{NewId()}";
        var url = $"https://example.com/articles/{slug}";
        var request = new { Url = url, CollectionName = "Blogs", FeedName = "Test Feed" };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AdHocUrlProcessResult>(
            TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Outcome.Should().Be(AdHocUrlProcessOutcome.Added);
        result.Slug.Should().Be(slug);

        var editResponse = await _client.GetAsync(
            $"/api/admin/content-items/edit-data?collection=blogs&slug={Uri.EscapeDataString(slug)}",
            TestContext.Current.CancellationToken);
        editResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var stored = await editResponse.Content.ReadFromJsonAsync<ContentItemEditData>(
            TestContext.Current.CancellationToken);
        stored!.Title.Should().Be(TechHubStubbedExternalServicesApiFactory.AiTitle);
        stored.ExternalUrl.Should().Be(url);
        stored.FeedName.Should().Be("Test Feed");

        var processed = await GetProcessedUrlAsync(url);
        processed.Status.Should().Be("succeeded");
        processed.Slug.Should().Be(slug);
    }

    [Fact]
    public async Task ProcessUrl_AfterSuccessfulProcessing_ReturnsConflictForSameUrl()
    {
        // Arrange
        var url = $"https://example.com/articles/stub-ok-{NewId()}";
        var request = new { Url = url, CollectionName = "blogs", FeedName = "Test Feed" };
        var first = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);
        first.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        var second = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        second.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ProcessUrl_WhenAiExcludesContent_ReturnsOkSkippedAndRecordsSkippedUrl()
    {
        // Arrange
        var url = $"https://example.com/articles/{TechHubStubbedExternalServicesApiFactory.AiExcludeMarker}-{NewId()}";
        var request = new { Url = url, CollectionName = "blogs", FeedName = "Test Feed" };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AdHocUrlProcessResult>(
            TestContext.Current.CancellationToken);
        result!.Outcome.Should().Be(AdHocUrlProcessOutcome.Skipped);
        result.Slug.Should().BeNull();

        var processed = await GetProcessedUrlAsync(url);
        processed.Status.Should().Be("skipped");
    }

    [Theory]
    [InlineData(TechHubStubbedExternalServicesApiFactory.AiFailureMarker)]
    [InlineData(TechHubStubbedExternalServicesApiFactory.AiThrowsMarker)]
    public async Task ProcessUrl_WhenAiFails_ReturnsUnprocessableEntityAndRecordsFailedUrl(string marker)
    {
        // Arrange
        var url = $"https://example.com/articles/{marker}-{NewId()}";
        var request = new { Url = url, CollectionName = "blogs", FeedName = "Test Feed" };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/urls/process",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var result = await response.Content.ReadFromJsonAsync<AdHocUrlProcessResult>(
            TestContext.Current.CancellationToken);
        result!.Outcome.Should().Be(AdHocUrlProcessOutcome.Failed);

        var processed = await GetProcessedUrlAsync(url);
        processed.Status.Should().Be("failed");
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string NewId() => Guid.NewGuid().ToString("N");

    private static string TitleUrl(string url) =>
        $"/api/admin/urls/title?url={Uri.EscapeDataString(url)}";

    private async Task<ProcessedUrlListItem> GetProcessedUrlAsync(string url)
    {
        var page = await _client.GetFromJsonAsync<PagedResult<ProcessedUrlListItem>>(
            $"/api/admin/processed-urls?search={Uri.EscapeDataString(url)}",
            TestContext.Current.CancellationToken);
        return page!.Items.Single(i => i.ExternalUrl == url);
    }
}
