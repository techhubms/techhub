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
        // Arrange - get a valid slug first
        var allPagesResponse = await _client.GetAsync("/api/custom-pages");
        var allPages = await allPagesResponse.Content.ReadFromJsonAsync<List<CustomPageDto>>();
        allPages.Should().NotBeEmpty();
        var firstPage = allPages!.First();

        // Act
        var response = await _client.GetAsync($"/api/custom-pages/{firstPage.Slug}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await response.Content.ReadFromJsonAsync<CustomPageDetailDto>();
        page.Should().NotBeNull();
        page!.Slug.Should().Be(firstPage.Slug);
        page.Title.Should().Be(firstPage.Title);
        page.RenderedHtml.Should().NotBeNullOrWhiteSpace();
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
        // Arrange
        var allPagesResponse = await _client.GetAsync("/api/custom-pages");
        var allPages = await allPagesResponse.Content.ReadFromJsonAsync<List<CustomPageDto>>();
        var firstPage = allPages!.First();

        // Act
        var response = await _client.GetAsync($"/api/custom-pages/{firstPage.Slug}");
        var page = await response.Content.ReadFromJsonAsync<CustomPageDetailDto>();

        // Assert
        page.Should().NotBeNull();
        page!.RenderedHtml.Should().NotBeNullOrWhiteSpace();
        page.RenderedHtml.Should().Contain("<"); // Should contain HTML tags
    }

    [Theory]
    [InlineData("features")]
    [InlineData("genai-basics")]
    [InlineData("dx-space")]
    public async Task GetCustomPageBySlug_WithKnownPages_ReturnsCorrectPage(string slug)
    {
        // Act
        var response = await _client.GetAsync($"/api/custom-pages/{slug}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var page = await response.Content.ReadFromJsonAsync<CustomPageDetailDto>();
        page.Should().NotBeNull();
        page!.Slug.Should().Be(slug);
    }
}
