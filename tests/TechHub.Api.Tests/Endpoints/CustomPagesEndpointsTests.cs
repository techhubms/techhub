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

    [Fact]
    public async Task GetVSCodeUpdatesData_ReturnsStructuredData()
    {
        // Arrange - Act
        var response = await _client.GetAsync("/api/custom-pages/vscode-updates");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadFromJsonAsync<VSCodeUpdatesPageData>();
        data.Should().NotBeNull();
        data!.Title.Should().NotBeNullOrWhiteSpace();
        data.Description.Should().NotBeNullOrWhiteSpace();
        data.Intro.Should().NotBeNull().And.NotBeEmpty();
        data.VideoCollection.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetVSCodeUpdatesData_HasExpectedContent()
    {
        // Arrange - Act
        var response = await _client.GetAsync("/api/custom-pages/vscode-updates");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadFromJsonAsync<VSCodeUpdatesPageData>();
        data.Should().NotBeNull();
        data!.Title.Should().Contain("Visual Studio Code");
        data.VideoCollection.Should().Be("vscode-updates");
    }

    [Fact]
    public async Task GetVSCodeUpdatesData_RespondsQuickly()
    {
        // Arrange
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var response = await _client.GetAsync("/api/custom-pages/vscode-updates");

        // Assert
        stopwatch.Stop();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000, "API should respond within 1 second");
    }
}
