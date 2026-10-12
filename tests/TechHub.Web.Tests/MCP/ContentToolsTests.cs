using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using ModelContextProtocol.Server;
using Moq;
using TechHub.Core.Models;
using TechHub.Web.MCP;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.MCP;

public sealed partial class ContentToolsTests : IDisposable
{
    private readonly Mock<ITechHubApiClient> _apiClient = new();
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());

    public static TheoryData<string, string> ToolsBySection => new()
    {
        { "get_latest_github_copilot_content", "github-copilot" },
        { "get_latest_ai_content", "ai" },
        { "get_latest_ml_content", "ml" },
        { "get_latest_dotnet_content", "dotnet" },
        { "get_latest_azure_content", "azure" },
        { "get_latest_devops_content", "devops" },
        { "get_latest_security_content", "security" },
    };

    public void Dispose() => _cache.Dispose();

    [Theory]
    [MemberData(nameof(ToolsBySection))]
    public async Task Tool_RequestsLatestContentOfItsOwnSection(string toolName, string sectionName)
    {
        // Arrange
        _apiClient
            .Setup(c => c.GetCollectionItemsAsync(
                sectionName, "all", 10, 10, null, null, null, null, null, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CollectionItemsResponse([], 0));
        var tools = new ContentTools(new LatestContentProvider(_apiClient.Object, _cache, new HttpContextAccessor()));
        var method = FindToolMethod(toolName);

        // Act
        var task = (Task<ContentPage>)method.Invoke(tools, [2, TestContext.Current.CancellationToken])!;
        await task;

        // Assert
        _apiClient.VerifyAll();
    }

    [Fact]
    public void Tools_ExposeOneExplicitToolPerSection()
    {
        var toolNames = GetToolMethods().Select(m => m.GetCustomAttribute<McpServerToolAttribute>()!.Name);

        toolNames.Should().BeEquivalentTo(ToolsBySection.Select(row => row.Data.Item1));
    }

    [Fact]
    public void Tools_HaveDescriptiveSnakeCaseNamesAndTitles()
    {
        foreach (var method in GetToolMethods())
        {
            var attribute = method.GetCustomAttribute<McpServerToolAttribute>()!;

            attribute.Name.Should().MatchRegex(ToolNamePattern().ToString(), $"{method.Name} needs a stable, readable tool name");
            attribute.Title.Should().NotBeNullOrWhiteSpace($"{method.Name} needs a human-readable title");
            attribute.Title.Should().NotContain("Git Hub", "the title must not be derived from the method name");
        }
    }

    [Fact]
    public void Tools_AreDeclaredReadOnlyClosedWorldWithDescriptions()
    {
        foreach (var method in GetToolMethods())
        {
            var attribute = method.GetCustomAttribute<McpServerToolAttribute>()!;

            attribute.ReadOnly.Should().BeTrue($"{method.Name} only reads content");
            attribute.OpenWorld.Should().BeFalse($"{method.Name} only talks to Tech Hub");
            method.GetCustomAttribute<DescriptionAttribute>()?.Description.Should().NotBeNullOrWhiteSpace();
            method.GetParameters().Single(p => p.Name == "page")
                .GetCustomAttribute<DescriptionAttribute>().Should().NotBeNull();
        }
    }

    private static MethodInfo FindToolMethod(string toolName) =>
        GetToolMethods().Single(m => m.GetCustomAttribute<McpServerToolAttribute>()!.Name == toolName);

    private static IEnumerable<MethodInfo> GetToolMethods() =>
        typeof(ContentTools)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => m.GetCustomAttribute<McpServerToolAttribute>() is not null);

    [GeneratedRegex("^get_latest_[a-z]+(_[a-z]+)*_content$")]
    private static partial Regex ToolNamePattern();
}
