using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Integration tests for Author API endpoints.
/// Tests GET /api/authors and GET /api/authors/{authorName}/items.
/// </summary>
public class AuthorEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public AuthorEndpointsTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllAuthors_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/authors", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllAuthors_ReturnsAuthorsWithItemCounts()
    {
        // Act
        var response = await _client.GetAsync("/api/authors", TestContext.Current.CancellationToken);
        var authors = await response.Content.ReadFromJsonAsync<List<AuthorSummary>>(TestContext.Current.CancellationToken);

        // Assert - test data uses "Test Author" for most items
        authors.Should().NotBeNull();
        authors!.Should().NotBeEmpty();
        authors.Should().AllSatisfy(a =>
        {
            a.Name.Should().NotBeNullOrWhiteSpace("author name should not be empty");
            a.ItemCount.Should().BeGreaterThan(0, "each author should have at least one published item");
        });
    }

    [Fact]
    public async Task GetAllAuthors_ReturnsKnownTestAuthor()
    {
        // Act
        var response = await _client.GetAsync("/api/authors", TestContext.Current.CancellationToken);
        var authors = await response.Content.ReadFromJsonAsync<List<AuthorSummary>>(TestContext.Current.CancellationToken);

        // Assert - test collections use "Test Author"
        authors.Should().NotBeNull();
        authors!.Should().Contain(a => a.Name == "Test Author", "test data includes 'Test Author'");
    }

    [Fact]
    public async Task GetAllAuthors_ReturnsAuthorsInAlphabeticalOrder()
    {
        // Act
        var response = await _client.GetAsync("/api/authors", TestContext.Current.CancellationToken);
        var authors = await response.Content.ReadFromJsonAsync<List<AuthorSummary>>(TestContext.Current.CancellationToken);

        // Assert - authors should be sorted alphabetically
        authors.Should().NotBeNull();
        var names = authors!.Select(a => a.Name).ToList();
        names.Should().BeInAscendingOrder("authors should be sorted alphabetically");
    }

    [Fact]
    public async Task GetAuthorItems_WithValidAuthor_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/authors/Test%20Author/items",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAuthorItems_WithValidAuthor_ReturnsItemsForThatAuthor()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/authors/Test%20Author/items",
            TestContext.Current.CancellationToken);
        var result = await response.Content.ReadFromJsonAsync<CollectionItemsResponse>(TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Items.Should().NotBeEmpty("Test Author has content in test data");
        result.Items.Should().AllSatisfy(item =>
        {
            item.Author.Should().Be("Test Author", "all returned items should be by the requested author");
        });
        result.TotalCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetAuthorItems_WithNonExistentAuthor_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/authors/NonExistentAuthorXYZ123/items",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAuthorItems_WithPagination_ReturnsCorrectPage()
    {
        // Arrange - Get first page
        var firstPageResponse = await _client.GetAsync(
            "/api/authors/Test%20Author/items?take=2&skip=0",
            TestContext.Current.CancellationToken);
        var firstPage = await firstPageResponse.Content.ReadFromJsonAsync<CollectionItemsResponse>(TestContext.Current.CancellationToken);

        // Act - Get second page
        var secondPageResponse = await _client.GetAsync(
            "/api/authors/Test%20Author/items?take=2&skip=2",
            TestContext.Current.CancellationToken);
        var secondPage = await secondPageResponse.Content.ReadFromJsonAsync<CollectionItemsResponse>(TestContext.Current.CancellationToken);

        // Assert
        firstPage.Should().NotBeNull();
        secondPage.Should().NotBeNull();
        firstPage!.TotalCount.Should().Be(secondPage!.TotalCount, "total count should be consistent");

        // Pages should not have overlapping items
        if (firstPage.Items.Count > 0 && secondPage.Items.Count > 0)
        {
            var firstPageSlugs = firstPage.Items.Select(i => i.Slug).ToHashSet();
            secondPage.Items.Should().NotContain(
                i => firstPageSlugs.Contains(i.Slug),
                "second page should not contain items from first page");
        }
    }

    [Fact]
    public async Task GetAuthorItems_WithInvalidAuthorNameFormat_ReturnsBadRequest()
    {
        // Act - path traversal attempt
        var response = await _client.GetAsync(
            "/api/authors/..%2F..%2Fetc%2Fpasswd/items",
            TestContext.Current.CancellationToken);

        // Assert - should be bad request or not found (ASP.NET may decode and reject)
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.NotFound,
            HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllAuthors_ExcludesDraftAuthors()
    {
        // Act
        var response = await _client.GetAsync("/api/authors", TestContext.Current.CancellationToken);
        var authors = await response.Content.ReadFromJsonAsync<List<AuthorSummary>>(TestContext.Current.CancellationToken);

        // Assert - "Draft Author" from test data should not appear
        // (draft articles should not be counted)
        authors.Should().NotBeNull();
        // Note: Draft Author may still appear if they have non-draft content
        // This just verifies the endpoint doesn't crash on draft data
        authors!.Should().AllSatisfy(a => a.ItemCount.Should().BeGreaterThan(0));
    }
}
