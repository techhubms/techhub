using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Api.Tests.TestSupport;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.Admin;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for the admin content item editing endpoints:
/// edit-data (GET/PUT), apply-transcript and removal of the unused ai-metadata routes.
/// AI categorization is replaced by a stateless stub (see <see cref="TechHubStubbedExternalServicesApiFactory"/>).
/// </summary>
public class AdminContentItemsEndpointsTests : IClassFixture<TechHubStubbedExternalServicesApiFactory>
{
    private const string VideosCollection = "videos";
    private const int MaxTranscriptLength = 50_000;

    private readonly HttpClient _client;
    private readonly TechHubStubbedExternalServicesApiFactory _factory;

    public AdminContentItemsEndpointsTests(TechHubStubbedExternalServicesApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _factory = factory;
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("GET")]
    [InlineData("PUT")]
    public async Task AiMetadata_RemovedEndpoint_ReturnsNotFoundAndDoesNotModifyItem(string method)
    {
        // Arrange
        var slug = await SeedVideoAsync();
        using var request = new HttpRequestMessage(new HttpMethod(method), ItemUrl("ai-metadata", VideosCollection, slug));
        if (method == "PUT")
        {
            request.Content = JsonContent.Create(new { AiMetadata = """{"roundup_summary":"Unused route"}""" });
        }

        // Act
        using var response = await _client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var stored = await GetEditDataAsync(slug);
        stored.AiMetadata.Should().BeNull();
    }

    // ── GET /api/admin/content-items/edit-data ───────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("?collection=videos")]
    [InlineData("?slug=some-slug")]
    public async Task GetEditData_MissingParams_ReturnsBadRequest(string query)
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/admin/content-items/edit-data{query}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetEditData_UnknownItem_ReturnsNotFound()
    {
        // Arrange
        var slug = $"unknown-{NewId()}";

        // Act
        var response = await _client.GetAsync(
            ItemUrl("edit-data", VideosCollection, slug),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── PUT /api/admin/content-items/edit-data ───────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("?collection=videos")]
    [InlineData("?slug=some-slug")]
    public async Task UpdateEditData_MissingParams_ReturnsBadRequest(string query)
    {
        // Arrange
        var payload = ValidEditData("some-slug");

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/content-items/edit-data{query}",
            payload,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("blank-title")]
    [InlineData("blank-author")]
    [InlineData("blank-primary-section")]
    [InlineData("no-tags")]
    [InlineData("no-sections")]
    [InlineData("zero-date")]
    [InlineData("negative-date")]
    [InlineData("invalid-ai-metadata")]
    public async Task UpdateEditData_InvalidPayload_ReturnsBadRequestAndDoesNotPersist(string scenario)
    {
        // Arrange
        var slug = await SeedVideoAsync(title: "Original Title");
        var payload = Invalidate(ValidEditData(slug), scenario);

        // Act
        var response = await _client.PutAsJsonAsync(
            ItemUrl("edit-data", VideosCollection, slug),
            payload,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var stored = await GetEditDataAsync(slug);
        stored.Title.Should().Be("Original Title");
        stored.Author.Should().Be("Original Author");
        stored.AiMetadata.Should().BeNull();
    }

    [Fact]
    public async Task UpdateEditData_UnknownItem_ReturnsNotFound()
    {
        // Arrange
        var slug = $"unknown-{NewId()}";

        // Act
        var response = await _client.PutAsJsonAsync(
            ItemUrl("edit-data", VideosCollection, slug),
            ValidEditData(slug),
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("""{"roundup_summary":"Persisted summary","key_topics":["a","b"]}""")]
    public async Task UpdateEditData_ValidPayload_PersistsAllEditableFields(string? aiMetadata)
    {
        // Arrange
        var slug = await SeedVideoAsync(title: "Original Title");
        using var initialResponse = await _client.PutAsJsonAsync(
            ItemUrl("edit-data", VideosCollection, slug),
            ValidEditData(slug) with { AiMetadata = """{"roundup_summary":"Original summary"}""" },
            TestContext.Current.CancellationToken);
        initialResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var payload = ValidEditData(slug) with { DateEpoch = 1710000000, AiMetadata = aiMetadata };

        // Act
        var response = await _client.PutAsJsonAsync(
            ItemUrl("edit-data", VideosCollection, slug),
            payload,
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var stored = await GetEditDataAsync(slug);
        stored.Title.Should().Be("Edited Title");
        stored.Author.Should().Be("Edited Author");
        stored.Excerpt.Should().Be("Edited excerpt");
        stored.Content.Should().Be("# Edited");
        stored.DateEpoch.Should().Be(1710000000);
        stored.PrimarySectionName.Should().Be("ai");
        stored.Tags.Should().BeEquivalentTo(["AI", "Azure"]);
        stored.Sections.Should().BeEquivalentTo(["ai", "azure"]);
        if (aiMetadata is null)
        {
            stored.AiMetadata.Should().BeNull();
        }
        else
        {
            stored.AiMetadata.Should().NotBeNull();
            using var doc = JsonDocument.Parse(stored.AiMetadata!);
            doc.RootElement.GetProperty("roundup_summary").GetString().Should().Be("Persisted summary");
            doc.RootElement.GetProperty("key_topics").GetArrayLength().Should().Be(2);
        }
    }

    // ── POST /api/admin/content-items/apply-transcript ───────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("?collection=videos")]
    [InlineData("?slug=some-slug")]
    public async Task ApplyTranscript_MissingParams_ReturnsBadRequest(string query)
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/admin/content-items/apply-transcript{query}",
            new { Transcript = "some transcript" },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("blogs")]
    [InlineData("news")]
    [InlineData("community")]
    public async Task ApplyTranscript_CollectionOtherThanVideos_ReturnsBadRequest(string collection)
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            ItemUrl("apply-transcript", collection, "some-slug"),
            new { Transcript = "some transcript" },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ApplyTranscript_BlankTranscript_ReturnsBadRequest(string? transcript)
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            ItemUrl("apply-transcript", VideosCollection, "some-slug"),
            new { Transcript = transcript },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ApplyTranscript_TranscriptOverLimit_ReturnsBadRequest()
    {
        // Arrange
        var transcript = new string('a', MaxTranscriptLength + 1);

        // Act
        var response = await _client.PostAsJsonAsync(
            ItemUrl("apply-transcript", VideosCollection, "some-slug"),
            new { Transcript = transcript },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ApplyTranscript_TranscriptAtLimit_PassesValidationAndReturnsNotFoundForUnknownItem()
    {
        // Arrange
        var transcript = new string('a', MaxTranscriptLength);
        var slug = $"unknown-{NewId()}";

        // Act
        var response = await _client.PostAsJsonAsync(
            ItemUrl("apply-transcript", VideosCollection, slug),
            new { Transcript = transcript },
            TestContext.Current.CancellationToken);

        // Assert — 404 (not 400) proves the length check accepted exactly the maximum
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ApplyTranscript_UnknownItem_ReturnsNotFound()
    {
        // Arrange
        var slug = $"unknown-{NewId()}";

        // Act
        var response = await _client.PostAsJsonAsync(
            ItemUrl("apply-transcript", VideosCollection, slug),
            new { Transcript = "some transcript" },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ApplyTranscript_NonYouTubeItem_ReturnsBadRequest()
    {
        // Arrange — seeded video links to an internal URL, not YouTube
        var slug = await SeedVideoAsync();

        // Act
        var response = await _client.PostAsJsonAsync(
            ItemUrl("apply-transcript", VideosCollection, slug),
            new { Transcript = "some transcript" },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        body.Should().Contain("YouTube");
    }

    [Fact]
    public async Task ApplyTranscript_WhenAiThrowsHttpError_ReturnsBadGateway()
    {
        // Arrange
        var slug = await SeedVideoAsync(
            externalUrl: YouTubeUrl(TechHubStubbedExternalServicesApiFactory.AiThrowsMarker),
            title: "Original Title");

        // Act
        var response = await _client.PostAsJsonAsync(
            ItemUrl("apply-transcript", VideosCollection, slug),
            new { Transcript = "some transcript" },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadGateway);
        var stored = await GetEditDataAsync(slug);
        stored.Title.Should().Be("Original Title");
    }

    [Theory]
    [InlineData(TechHubStubbedExternalServicesApiFactory.AiFailureMarker)]
    [InlineData(TechHubStubbedExternalServicesApiFactory.AiExcludeMarker)]
    public async Task ApplyTranscript_WhenAiFailsOrExcludesContent_ReturnsUnprocessableEntityAndKeepsItem(string marker)
    {
        // Arrange
        var slug = await SeedVideoAsync(externalUrl: YouTubeUrl(marker), title: "Original Title");

        // Act
        var response = await _client.PostAsJsonAsync(
            ItemUrl("apply-transcript", VideosCollection, slug),
            new { Transcript = "some transcript" },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        var stored = await GetEditDataAsync(slug);
        stored.Title.Should().Be("Original Title");
    }

    [Fact]
    public async Task ApplyTranscript_WhenAiSucceeds_ReturnsUpdatedItemAndPersistsIt()
    {
        // Arrange
        var id = NewId();
        var externalUrl = YouTubeUrl("stub-ai-ok");
        var slug = await SeedVideoAsync(externalUrl: externalUrl, title: "Original Title", dateEpoch: 1700000123);

        // Act
        var response = await _client.PostAsJsonAsync(
            ItemUrl("apply-transcript", VideosCollection, slug),
            new { Transcript = $"  transcript words {id}  " },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returned = await response.Content.ReadFromJsonAsync<ContentItemEditData>(
            TestContext.Current.CancellationToken);
        returned.Should().NotBeNull();
        returned!.Slug.Should().Be(slug);
        returned.CollectionName.Should().Be(VideosCollection);
        returned.Title.Should().Be(TechHubStubbedExternalServicesApiFactory.AiTitle);
        returned.Excerpt.Should().Be(TechHubStubbedExternalServicesApiFactory.AiExcerpt);
        returned.Content.Should().Contain($"transcript words {id}", "the trimmed transcript is passed to the AI service");
        returned.Author.Should().Be("Original Author", "the stub returns no author, so the existing one is kept");
        returned.DateEpoch.Should().Be(1700000123, "the publication date is preserved");
        returned.ExternalUrl.Should().Be(externalUrl);

        var stored = await GetEditDataAsync(slug);
        stored.Title.Should().Be(TechHubStubbedExternalServicesApiFactory.AiTitle);
        stored.Sections.Should().Contain("ai");
        using var metadata = JsonDocument.Parse(stored.AiMetadata!);
        metadata.RootElement.GetProperty("schema_version").GetInt32().Should().Be(2);
        metadata.RootElement.GetProperty("key_facts")[0].GetString().Should().Be("Stubbed fact.");

        using var scope = _factory.Services.CreateScope();
        var timeline = scope.ServiceProvider.GetRequiredService<ITimelineEventRepository>();
        var events = await timeline.GetEventsForItemAsync(VideosCollection, slug, TestContext.Current.CancellationToken);
        events.Should().ContainSingle().Which.Product.Should().Be("Stub Product");
        events[0].ExtractedFrom.Should().Be("source");
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string NewId() => Guid.NewGuid().ToString("N");

    private static string YouTubeUrl(string marker) =>
        $"https://www.youtube.com/watch?v={marker}-{NewId()}";

    private static string ItemUrl(string route, string collection, string slug) =>
        $"/api/admin/content-items/{route}?collection={Uri.EscapeDataString(collection)}&slug={Uri.EscapeDataString(slug)}";

    private static ContentItemEditData ValidEditData(string slug) => new()
    {
        CollectionName = VideosCollection,
        Slug = slug,
        DateEpoch = 1700000000,
        Title = "Edited Title",
        Author = "Edited Author",
        Excerpt = "Edited excerpt",
        Content = "# Edited",
        PrimarySectionName = "ai",
        Tags = ["AI", "Azure"],
        Sections = ["ai", "azure"],
        AiMetadata = null
    };

    private static ContentItemEditData Invalidate(ContentItemEditData valid, string scenario) => scenario switch
    {
        "blank-title" => valid with { Title = " " },
        "blank-author" => valid with { Author = "" },
        "blank-primary-section" => valid with { PrimarySectionName = " " },
        "no-tags" => valid with { Tags = [] },
        "no-sections" => valid with { Sections = [] },
        "zero-date" => valid with { DateEpoch = 0 },
        "negative-date" => valid with { DateEpoch = -5 },
        "invalid-ai-metadata" => valid with { AiMetadata = "{not json" },
        _ => throw new ArgumentOutOfRangeException(nameof(scenario), scenario, "Unknown scenario")
    };

    private async Task<string> SeedVideoAsync(string? externalUrl = null, string title = "Seeded Item", long dateEpoch = 1700000000)
    {
        var slug = $"admin-items-{NewId()}";
        await ContentItemSeeder.InsertAsync(
            _factory.Services,
            VideosCollection,
            slug,
            externalUrl ?? $"/all/videos/{slug}",
            title,
            author: "Original Author",
            dateEpoch);
        return slug;
    }

    private async Task<ContentItemEditData> GetEditDataAsync(string slug)
    {
        var response = await _client.GetAsync(
            ItemUrl("edit-data", VideosCollection, slug),
            TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ContentItemEditData>(
            TestContext.Current.CancellationToken);
        return result!;
    }
}
