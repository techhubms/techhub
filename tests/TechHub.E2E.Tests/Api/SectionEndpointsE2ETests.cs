using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;

namespace TechHub.E2E.Tests.Api;

/// <summary>
/// End-to-end tests for Section API endpoints
/// Tests: GET /api/sections, GET /api/sections/{name}
/// </summary>
[Collection("API E2E Tests")]
public class SectionEndpointsE2ETests(ApiCollectionFixture fixture)
{
    private readonly HttpClient _client = fixture.Factory.CreateClient();

    [Fact]
    public async Task GetAllSections_ReturnsRealSections()
    {
        // Act
        var response = await _client.GetAsync("/api/sections");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var sections = await response.Content.ReadFromJsonAsync<List<Section>>();
        sections.Should().NotBeNull();
        sections!.Should().NotBeEmpty();

        // Verify expected sections from appsettings.json
        sections.Should().Contain(s => s.Name == "ai");
        sections.Should().Contain(s => s.Name == "github-copilot");
        sections.Should().Contain(s => s.Name == "azure");
        sections.Should().Contain(s => s.Name == "ml");
        sections.Should().Contain(s => s.Name == "coding");
        sections.Should().Contain(s => s.Name == "devops");
        sections.Should().Contain(s => s.Name == "security");

        // All sections should have collections
        sections.Should().AllSatisfy(s => s.Collections.Should().NotBeEmpty());
    }

    [Fact]
    public async Task GetSectionByName_ReturnsRealSection()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var section = await response.Content.ReadFromJsonAsync<Section>();
        section.Should().NotBeNull();
        section!.Name.Should().Be("ai");
        section.Title.Should().Be("Artificial Intelligence");
        section.Collections.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetSectionByName_InvalidSection_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/nonexistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllSections_CaseSensitive_ExactMatchRequired()
    {
        // Act - Try with different casing
        var upperResponse = await _client.GetAsync("/api/sections/AI");
        var mixedResponse = await _client.GetAsync("/api/sections/GitHub-Copilot");

        // Assert - Section names are case-sensitive in URLs
        upperResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        mixedResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
