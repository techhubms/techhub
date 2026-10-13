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
        const string InitializeRequest = """
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
            DataString = InitializeRequest,
            FailOnStatusCode = false,
            Timeout = BlazorHelpers.E2ETimeout,
        });

        // Assert - the body is JSON or an SSE stream containing the JSON-RPC result
        response.Status.Should().Be(200, "POST /mcp must pass URL normalization and the HTTP method filter");
        var body = await response.TextAsync();
        body.Should().Contain("\"serverInfo\"", "an initialize response must describe the MCP server");
        body.Should().Contain("\"techhub\"", "the server must identify itself with a readable name");
    }

    [Fact]
    public async Task McpEndpoint_ToolsList_ExposesOneDescriptiveToolPerSection()
    {
        // Act
        var body = await PostJsonRpcAsync("""{"jsonrpc":"2.0","id":2,"method":"tools/list","params":{}}""");

        // Assert
        foreach (var section in new[] { "github_copilot", "ai", "ml", "dotnet", "azure", "devops", "security" })
        {
            body.Should().Contain($"get_latest_{section}_content");
        }

        body.Should().Contain("get_latest_content", "the cross-section tool must be exposed");
        body.Should().Contain("get_roundup_content", "the roundup content tool must be exposed");
    }

    [Theory]
    [InlineData("get_latest_content")]
    [InlineData("get_latest_github_copilot_content")]
    [InlineData("get_latest_ai_content")]
    [InlineData("get_latest_ml_content")]
    [InlineData("get_latest_dotnet_content")]
    [InlineData("get_latest_azure_content")]
    [InlineData("get_latest_devops_content")]
    [InlineData("get_latest_security_content")]
    public async Task McpEndpoint_ToolCall_ReturnsLatestContentPage(string toolName)
    {
        // Act
        var body = await PostJsonRpcAsync(
            """{"jsonrpc":"2.0","id":3,"method":"tools/call","params":{"name":"TOOL_NAME","arguments":{"page":1}}}"""
                .Replace("TOOL_NAME", toolName, StringComparison.Ordinal));

        // Assert
        body.Should().Contain("totalCount", "the tool must return a page of content");
        body.Should().NotContain("\"isError\":true");
    }

    [Fact]
    public async Task McpEndpoint_ToolCallWithFilters_ReturnsFreshnessMetadata()
    {
        // Act
        var body = await PostJsonRpcAsync(
            """{"jsonrpc":"2.0","id":4,"method":"tools/call","params":{"name":"get_latest_content","arguments":{"days":0,"contentType":"news,blogs","includeTags":true}}}""");

        // Assert
        body.Should().Contain("generatedAt", "callers need the server time to judge data freshness");
        body.Should().NotContain("\"isError\":true");
    }

    [Fact]
    public async Task McpEndpoint_ToolCallWithInvalidSince_ReturnsReadableToolError()
    {
        // Act
        var body = await PostJsonRpcAsync(
            """{"jsonrpc":"2.0","id":5,"method":"tools/call","params":{"name":"get_latest_content","arguments":{"since":"yesterday"}}}""");

        // Assert
        body.Should().Contain("\"isError\":true");
        body.Should().Contain("yyyy-MM-dd", "the error must tell the caller what format is expected");
    }

    [Fact]
    public async Task McpEndpoint_LatestContentForExternalCollections_LinksToTechHubWithoutSourceUrl()
    {
        // Act - days=0 removes the default date window so seeded test content is always found
        var body = await PostJsonRpcAsync(
            """{"jsonrpc":"2.0","id":6,"method":"tools/call","params":{"name":"get_latest_content","arguments":{"days":0,"contentType":"news,blogs"}}}""");

        // Assert
        body.Should().Contain("totalCount");
        body.Should().NotContain("sourceUrl", "agents must send readers to the Tech Hub page, not bypass it");
        body.Should().NotContain("\"isError\":true");
    }

    [Fact]
    public async Task McpEndpoint_GetRoundupContent_NonRoundupUrl_ReturnsReadableToolError()
    {
        // Act
        var body = await PostJsonRpcAsync(
            """{"jsonrpc":"2.0","id":9,"method":"tools/call","params":{"name":"get_roundup_content","arguments":{"url":"/ai/news/x"}}}""");

        // Assert
        body.Should().Contain("\"isError\":true");
        body.Should().Contain("roundup url");
    }

    private async Task<string> PostJsonRpcAsync(string jsonRpcRequest)
    {
        var response = await Page.APIRequest.PostAsync($"{BlazorHelpers.BaseUrl}/mcp", new()
        {
            Headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json",
                ["Accept"] = "application/json, text/event-stream",
            },
            DataString = jsonRpcRequest,
            FailOnStatusCode = false,
            Timeout = BlazorHelpers.E2ETimeout,
        });

        response.Status.Should().Be(200);
        return await response.TextAsync();
    }
}
