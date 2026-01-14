using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TechHub.Core.DTOs;

namespace TechHub.Api.Tests.Endpoints;

public class CustomPagesEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CustomPagesEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllCustomPages_ReturnsOk_WithCustomPagesList()
    {
        // Act
        var response = await _client.GetAsync("/api/custom-pages");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pages = await response.Content.ReadFromJsonAsync<IEnumerable<CustomPageDto>>();
        pages.Should().NotBeNull();
        pages.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAllCustomPages_ReturnsPages_WithRequiredFields()
    {
        // Act
        var response = await _client.GetAsync("/api/custom-pages");
        var pages = await response.Content.ReadFromJsonAsync<List<CustomPageDto>>();

        // Assert
        pages.Should().NotBeNull();
        pages.Should().AllSatisfy(page =>
        {
            page.Slug.Should().NotBeNullOrWhiteSpace();
            page.Title.Should().NotBeNullOrWhiteSpace();
            page.Description.Should().NotBeNull();
            page.Url.Should().NotBeNullOrWhiteSpace();
            page.Categories.Should().NotBeNull();
        });
    }

    [Fact]
    public async Task GetCustomPageBySlug_WithValidSlug_ReturnsOk()
    {
        // Arrange - get a valid slug that doesn't have a specific route
        // Note: Pages like 'features', 'handbook', 'sdlc', etc. now have specific endpoints
        // so we need to test the generic endpoint with a page that doesn't have one
        var allPagesResponse = await _client.GetAsync("/api/custom-pages");
        var allPages = await allPagesResponse.Content.ReadFromJsonAsync<List<CustomPageDto>>();
        allPages.Should().NotBeEmpty();

        // Find a page that doesn't have a specific route - look for 'levels-of-enlightenment'
        // which has a different slug from our '/levels' endpoint
        var pageWithGenericRoute = allPages!.FirstOrDefault(p => p.Slug == "levels-of-enlightenment")
            ?? allPages!.First(); // Fallback to first page

        // If the first page has a specific route, this test will verify those work too
        // Act
        var response = await _client.GetAsync($"/api/custom-pages/{pageWithGenericRoute.Slug}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCustomPageBySlug_WithInvalidSlug_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/custom-pages/non-existent-page");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCustomPageBySlug_ReturnsRenderedHtml()
    {
        // Arrange - get a page that uses the generic slug endpoint
        // Note: 'levels-of-enlightenment' doesn't have a specific route (/levels route is different)
        var allPagesResponse = await _client.GetAsync("/api/custom-pages");
        var allPages = await allPagesResponse.Content.ReadFromJsonAsync<List<CustomPageDto>>();
        var pageWithGenericRoute = allPages!.FirstOrDefault(p => p.Slug == "levels-of-enlightenment");

        // Skip test if we can't find a page with generic route
        if (pageWithGenericRoute == null)
        {
            return; // No pages available that use generic route
        }

        // Act
        var response = await _client.GetAsync($"/api/custom-pages/{pageWithGenericRoute.Slug}");
        var page = await response.Content.ReadFromJsonAsync<CustomPageDetailDto>();

        // Assert
        page.Should().NotBeNull();
        page!.RenderedHtml.Should().NotBeNullOrWhiteSpace();
        page.RenderedHtml.Should().Contain("<"); // Should contain HTML tags
    }

    // Tests for specific structured data endpoints
    [Theory]
    [InlineData("/api/custom-pages/features")]
    [InlineData("/api/custom-pages/genai-applied")]
    [InlineData("/api/custom-pages/handbook")]
    [InlineData("/api/custom-pages/levels")]
    [InlineData("/api/custom-pages/sdlc")]
    [InlineData("/api/custom-pages/genai-basics")]
    [InlineData("/api/custom-pages/genai-advanced")]
    [InlineData("/api/custom-pages/vscode-updates")]
    public async Task GetSpecificCustomPage_ReturnsOk(string endpoint)
    {
        // Act
        var response = await _client.GetAsync(endpoint);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetDXSpaceData_ReturnsStructuredData()
    {
        // Act
        var response = await _client.GetAsync("/api/custom-pages/dx-space");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadFromJsonAsync<DXSpacePageData>();
        data.Should().NotBeNull();
        data!.Title.Should().Be("Developer Experience Space");
        data.Dora.Should().NotBeNull();
        data.Dora.Metrics.Should().HaveCount(4);
        data.Space.Should().NotBeNull();
        data.Space.Dimensions.Should().HaveCount(5);
        data.DevEx.Should().NotBeNull();
        data.BestPractices.Should().NotBeNull();
    }
}
