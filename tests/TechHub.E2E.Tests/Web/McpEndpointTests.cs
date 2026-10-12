using FluentAssertions;
using TechHub.E2E.Tests.Helpers;

namespace TechHub.E2E.Tests.Web;

/// <summary>
/// E2E test verifying the MCP endpoint is reachable through the full Web middleware pipeline.
/// </summary>
public class McpEndpointTests : PlaywrightTestBase
{
    public McpEndpointTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task McpEndpoint_Initialize_ReturnsServerInfo()
    {
        // Arrange - JSON-RPC initialize request per the MCP streamable HTTP transport
        const string initializeRequest = """
            {"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2025-03-26","capabilities":{},"clientInfo":{"name":"e2e-test","version":"1.0.0"}}}
            """;

        // Act
        var response = await Page.APIRequest.PostAsync($"{BlazorHelpers.BaseUrl}/mcp", new()
        {
            Headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json",
                ["Accept"] = "application/json, text/event-stream",
            },
            DataString = initializeRequest,
            FailOnStatusCode = false,
            Timeout = BlazorHelpers.E2ETimeout,
        });

        // Assert - the body is JSON or an SSE stream containing the JSON-RPC result
        response.Status.Should().Be(200, "POST /mcp must pass URL normalization and the HTTP method filter");
        var body = await response.TextAsync();
        body.Should().Contain("\"serverInfo\"", "an initialize response must describe the MCP server");
    }
}
