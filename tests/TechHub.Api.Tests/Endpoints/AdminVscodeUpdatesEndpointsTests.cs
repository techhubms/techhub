using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Api.Tests.TestSupport;
using TechHub.Core.Models;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for GET/POST/DELETE /api/admin/vscode-updates and the public GET /api/vscode-updates.
/// Every test seeds its own uniquely named content items, so tests are independent of each other.
/// </summary>
public class AdminVscodeUpdatesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private const string VideosCollection = "videos";

    // Far-future epoch so seeded items sort first in the date-descending list (the public endpoint caps at 100 items)
    private const long NewestEpoch = 4_000_000_000;

    private readonly HttpClient _client;
    private readonly TechHubIntegrationTestApiFactory _factory;

    public AdminVscodeUpdatesEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _factory = factory;
        _client = factory.CreateClient();
    }

    // ── GET /api/admin/vscode-updates ────────────────────────────────────────

    [Fact]
    public async Task GetVscodeUpdates_ReturnsPagedShape()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/admin/vscode-updates",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var page = await response.Content.ReadFromJsonAsync<VscodeUpdatesPage>(
            TestContext.Current.CancellationToken);
        page.Should().NotBeNull();
        page!.Items.Should().NotBeNull();
        page.TotalCount.Should().BeGreaterThanOrEqualTo(page.Items.Count);
    }

    [Fact]
    public async Task GetVscodeUpdates_WithSearch_FiltersBySlug()
    {
        // Arrange
        var matching = await SeedItemAsync();
        var other = await SeedItemAsync();
        await AddAsync(matching);
        await AddAsync(other);

        // Act
        var page = await SearchAsync(matching);

        // Assert
        page.TotalCount.Should().Be(1);
        page.Items.Should().ContainSingle(i => i.Slug == matching);
        page.Items.Should().NotContain(i => i.Slug == other);
    }

    [Fact]
    public async Task GetVscodeUpdates_WithPageSize_ReturnsRequestedPageAndFullTotal()
    {
        // Arrange — two items sharing a unique search prefix
        var prefix = $"vscode-page-{NewId()}";
        var first = await SeedItemAsync(slug: $"{prefix}-a");
        var second = await SeedItemAsync(slug: $"{prefix}-b");
        await AddAsync(first);
        await AddAsync(second);

        // Act
        var firstPage = await GetPageAsync($"search={prefix}&page=1&pageSize=1");
        var secondPage = await GetPageAsync($"search={prefix}&page=2&pageSize=1");

        // Assert
        firstPage.TotalCount.Should().Be(2);
        secondPage.TotalCount.Should().Be(2);
        firstPage.Items.Should().HaveCount(1);
        secondPage.Items.Should().HaveCount(1);
        firstPage.Items[0].Slug.Should().NotBe(secondPage.Items[0].Slug);
    }

    // ── POST /api/admin/vscode-updates ───────────────────────────────────────

    [Theory]
    [InlineData("", "some-slug")]
    [InlineData("videos", "")]
    [InlineData("  ", "  ")]
    public async Task AddVscodeUpdate_BlankFields_ReturnsBadRequest(string collectionName, string slug)
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/vscode-updates",
            new { CollectionName = collectionName, Slug = slug },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddVscodeUpdate_MissingSlug_ReturnsBadRequest()
    {
        // Act — Slug is a required member of the request DTO
        var response = await _client.PostAsJsonAsync(
            "/api/admin/vscode-updates",
            new { CollectionName = VideosCollection },
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddVscodeUpdate_ExistingItem_ReturnsNoContentAndAppearsInAdminAndPublicLists()
    {
        // Arrange
        var slug = await SeedItemAsync(dateEpoch: NewestEpoch);

        // Act
        var response = await AddAsync(slug);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var adminPage = await SearchAsync(slug);
        adminPage.TotalCount.Should().Be(1);
        adminPage.Items.Should().ContainSingle(i => i.Slug == slug && i.CollectionName == VideosCollection);

        var publicItems = await _client.GetFromJsonAsync<List<VscodeUpdateListItem>>(
            "/api/vscode-updates",
            TestContext.Current.CancellationToken);
        publicItems.Should().NotBeNull();
        publicItems!.Should().Contain(i => i.Slug == slug);
    }

    [Fact]
    public async Task AddVscodeUpdate_NonExistentContentItem_ReturnsNotFound()
    {
        // Act
        var response = await AddAsync($"does-not-exist-{NewId()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddVscodeUpdate_CalledTwice_IsIdempotent()
    {
        // Arrange
        var slug = await SeedItemAsync();

        // Act
        var first = await AddAsync(slug);
        var second = await AddAsync(slug);

        // Assert
        first.StatusCode.Should().Be(HttpStatusCode.NoContent);
        second.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var page = await SearchAsync(slug);
        page.TotalCount.Should().Be(1);
    }

    // ── DELETE /api/admin/vscode-updates ─────────────────────────────────────

    [Theory]
    [InlineData("")]
    [InlineData("?collection=videos")]
    [InlineData("?slug=some-slug")]
    [InlineData("?collection=%20&slug=%20")]
    public async Task RemoveVscodeUpdate_MissingOrBlankParams_ReturnsBadRequest(string query)
    {
        // Act
        var response = await _client.DeleteAsync(
            $"/api/admin/vscode-updates{query}",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RemoveVscodeUpdate_NotRegisteredItem_ReturnsNotFound()
    {
        // Arrange — the content item exists but was never registered as a VS Code update
        var slug = await SeedItemAsync();

        // Act
        var response = await RemoveAsync(slug);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveVscodeUpdate_RegisteredItem_RemovesFromListButKeepsContentItem()
    {
        // Arrange
        var slug = await SeedItemAsync();
        await AddAsync(slug);

        // Act
        var response = await RemoveAsync(slug);
        var secondRemove = await RemoveAsync(slug);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        secondRemove.StatusCode.Should().Be(HttpStatusCode.NotFound, "the item is no longer registered");

        var page = await SearchAsync(slug);
        page.TotalCount.Should().Be(0);

        var editData = await _client.GetAsync(
            $"/api/admin/content-items/edit-data?collection={VideosCollection}&slug={Uri.EscapeDataString(slug)}",
            TestContext.Current.CancellationToken);
        editData.StatusCode.Should().Be(HttpStatusCode.OK, "removing a VS Code update must not delete the content item");
    }

    // ── GET /api/vscode-updates (public) ─────────────────────────────────────

    [Fact]
    public async Task GetPublicVscodeUpdates_ReturnsJsonArray()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/vscode-updates",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.Content.ReadFromJsonAsync<List<VscodeUpdateListItem>>(
            TestContext.Current.CancellationToken);
        items.Should().NotBeNull();
        items.Should().NotBeEmpty("the test fixtures contain seeded vscode-updates items");
    }

    [Fact]
    public async Task GetPublicVscodeUpdates_AfterRemoval_NoLongerContainsItem()
    {
        // Arrange
        var slug = await SeedItemAsync(dateEpoch: NewestEpoch);
        await AddAsync(slug);
        var before = await _client.GetFromJsonAsync<List<VscodeUpdateListItem>>(
            "/api/vscode-updates",
            TestContext.Current.CancellationToken);
        before!.Should().Contain(i => i.Slug == slug);

        // Act
        await RemoveAsync(slug);

        // Assert
        var after = await _client.GetFromJsonAsync<List<VscodeUpdateListItem>>(
            "/api/vscode-updates",
            TestContext.Current.CancellationToken);
        after!.Should().NotContain(i => i.Slug == slug);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string NewId() => Guid.NewGuid().ToString("N");

    private async Task<string> SeedItemAsync(string? slug = null, long dateEpoch = 1700000000)
    {
        slug ??= $"vscode-update-{NewId()}";
        await ContentItemSeeder.InsertAsync(
            _factory.Services,
            VideosCollection,
            slug,
            $"/all/videos/{slug}",
            title: $"VS Code Update {slug}",
            dateEpoch: dateEpoch);
        return slug;
    }

    private Task<HttpResponseMessage> AddAsync(string slug) =>
        _client.PostAsJsonAsync(
            "/api/admin/vscode-updates",
            new { CollectionName = VideosCollection, Slug = slug },
            TestContext.Current.CancellationToken);

    private Task<HttpResponseMessage> RemoveAsync(string slug) =>
        _client.DeleteAsync(
            $"/api/admin/vscode-updates?collection={VideosCollection}&slug={Uri.EscapeDataString(slug)}",
            TestContext.Current.CancellationToken);

    private Task<VscodeUpdatesPage> SearchAsync(string slug) =>
        GetPageAsync($"search={Uri.EscapeDataString(slug)}");

    private async Task<VscodeUpdatesPage> GetPageAsync(string query)
    {
        var response = await _client.GetAsync(
            $"/api/admin/vscode-updates?{query}",
            TestContext.Current.CancellationToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var page = await response.Content.ReadFromJsonAsync<VscodeUpdatesPage>(
            TestContext.Current.CancellationToken);
        return page!;
    }

    private sealed record VscodeUpdatesPage(IReadOnlyList<VscodeUpdateListItem> Items, int TotalCount);
}
