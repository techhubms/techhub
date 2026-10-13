using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// Tests for McpInfo.razor, the page browsers get when they open the MCP endpoint.
/// </summary>
public class McpInfoTests : BunitContext
{
    [Theory]
    [InlineData("tech.hub.ms", "https://tech.hub.ms/mcp")]
    [InlineData("tech.xebia.ms", "https://tech.xebia.ms/mcp")]
    [InlineData("localhost", "https://tech.hub.ms/mcp")]
    public void McpInfo_ShowsPublicWebsiteUrl_NeverTheRequestHost(string host, string expectedUrl)
    {
        // Arrange
        RegisterServices(host);

        // Act
        var cut = Render<McpInfo>();

        // Assert
        cut.Find("[data-testid='mcp-endpoint-url']").TextContent.Should().Be(expectedUrl);
    }

    [Theory]
    [InlineData("tech.hub.ms", "https://tech.hub.ms/mcp")]
    [InlineData("tech.xebia.ms", "https://tech.xebia.ms/mcp")]
    public void McpInfo_ClientSnippets_ContainPublicWebsiteUrl(string host, string expectedUrl)
    {
        // Arrange
        RegisterServices(host);

        // Act
        var cut = Render<McpInfo>();

        // Assert
        var snippets = cut.FindAll("pre code").Select(c => c.TextContent).ToList();
        snippets.Should().HaveCount(3);
        snippets.Should().OnlyContain(s => s.Contains(expectedUrl, StringComparison.Ordinal));
        snippets.Should().NotContain(s => s.Contains("localhost", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("tech.hub.ms", "Tech Hub MCP server")]
    [InlineData("tech.xebia.ms", "Xebia Tech Hub MCP server")]
    public void McpInfo_Heading_FollowsBranding(string host, string expectedHeading)
    {
        // Arrange
        RegisterServices(host);

        // Act
        var cut = Render<McpInfo>();

        // Assert
        cut.Find("h1").TextContent.Trim().Should().Be(expectedHeading);
    }

    [Fact]
    public void McpInfo_ToolsTable_ListsOneToolPerSectionPlusRoundupTool()
    {
        // Arrange
        RegisterServices("tech.hub.ms", sections: ["all", "github-copilot", "ai"]);

        // Act
        var cut = Render<McpInfo>();

        // Assert
        var tools = cut.FindAll("[data-testid='mcp-tools'] tbody td:first-child code").Select(c => c.TextContent).ToList();
        tools.Should().Equal("get_latest_content", "get_latest_github_copilot_content", "get_latest_ai_content", "get_roundup_content");
    }

    private void RegisterServices(string host, string[]? sections = null)
    {
        JSInterop.SetupVoid("markScriptsReady");

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Host = new HostString(host);
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var config = new ConfigurationBuilder().Build();

        Services.AddScoped<BrandingService>(_ => new BrandingService(accessor, config));
        Services.AddSingleton<IConfiguration>(config);
        Services.AddSingleton(sections is null
            ? new SectionCacheBuilder().Build()
            : new SectionCacheBuilder().WithSections(sections).Build());
    }
}
