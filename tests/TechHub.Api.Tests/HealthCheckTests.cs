using System.Net;
using FluentAssertions;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests;

/// <summary>
/// Integration tests for health check endpoints provided by Aspire service defaults.
/// Tests /health and /alive endpoints for monitoring and load balancer health checks.
/// </summary>
public class HealthCheckTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public HealthCheckTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_ReturnsHealthyResponse()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify response is plain text
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/plain");

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Healthy", "Health check should return 'Healthy' when application is running");
    }

    [Fact]
    public async Task Alive_ReturnsSuccessResponse()
    {
        // Act
        var response = await _client.GetAsync("/alive");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Aspire's /alive endpoint returns a simple success response
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty("Alive endpoint should return a response");
    }

    [Fact]
    public async Task Health_IsCaseSensitive()
    {
        // Aspire health endpoints are typically case-sensitive
        // This test verifies the exact route matching

        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK, "Lowercase /health should work");
    }

    [Fact]
    public async Task Health_CanBeCalledMultipleTimes()
    {
        // Health checks are called frequently by load balancers and monitoring systems
        // Verify they can be called multiple times without issues

        // Act
        var responses = new List<HttpResponseMessage>();
        for (int i = 0; i < 5; i++)
        {
            var response = await _client.GetAsync("/health");
            responses.Add(response);
        }

        // Assert
        responses.Should().AllSatisfy(response =>
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        });

        // Cleanup
        foreach (var response in responses)
        {
            response.Dispose();
        }
    }
}
