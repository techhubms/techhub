using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Endpoints;

public class CustomPagesEndpointsTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly TechHubIntegrationTestApiFactory _factory;
    private readonly HttpClient _client;

    public CustomPagesEndpointsTests(TechHubIntegrationTestApiFactory factory)
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
    public async Task GetSpecificCustomPage_ReturnsOk(string endpoint)
    {
        // Act
        var response = await _client.GetAsync(endpoint, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetDXSpaceData_ReturnsStructuredData()
    {
        // Act
        var response = await _client.GetAsync("/api/custom-pages/dx-space", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await response.Content.ReadFromJsonAsync<DXSpacePageData>(TestContext.Current.CancellationToken);
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
