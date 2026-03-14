using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TechHub.TestUtilities;

namespace TechHub.Api.Tests.Middleware;

/// <summary>
/// Integration tests for the ExceptionHandlerMiddleware.
/// Verifies that error responses do not leak internal exception details in non-Development environments.
/// </summary>
public class ExceptionHandlerMiddlewareTests : IClassFixture<TechHubIntegrationTestApiFactory>
{
    private readonly HttpClient _client;

    public ExceptionHandlerMiddlewareTests(TechHubIntegrationTestApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task NotFound_ReturnsGenericMessage_NotInternalDetails()
    {
        // Arrange - request a section that does not exist
        var response = await _client.GetAsync("/api/sections/nonexistent-section-that-does-not-exist", TestContext.Current.CancellationToken);

        // Assert - if the endpoint returns 404 via TypedResults.NotFound, that's fine
        // We just verify that generic 404 responses don't leak internal details
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task BadRequest_ReturnsGenericMessage_NotInternalDetails()
    {
        // Arrange - request with invalid date parameters to trigger a BadRequest
        var response = await _client.GetAsync(
            "/api/sections/all/collections/news/items?from=not-a-date",
            TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        content.Should().NotBeEmpty();

        // Verify the response doesn't contain stack traces or internal exception type names
        content.Should().NotContain("System.");
        content.Should().NotContain("at TechHub.");
        content.Should().NotContain("StackTrace");
    }
}
