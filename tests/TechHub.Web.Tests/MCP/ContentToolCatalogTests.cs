using System.Text.Json;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.MCP;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.MCP;

public sealed partial class ContentToolCatalogTests : IDisposable
{
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());
    private readonly ServiceProvider _services;

    public ContentToolCatalogTests()
    {
        _services = new ServiceCollection()
            .AddScoped(_ => new LatestContentProvider(
                new Mock<ITechHubApiClient>().Object,
                _cache,
                new SectionCacheBuilder().Build(),
                Options.Create(new McpOptions { BaseUrl = "https://techhub.test" }),
                TimeProvider.System))
            .AddScoped(_ => new RoundupContentProvider(
                new Mock<ITechHubApiClient>().Object,
                Options.Create(new McpOptions { BaseUrl = "https://techhub.test" })))
            .BuildServiceProvider();
    }

    public void Dispose()
    {
        _services.Dispose();
        _cache.Dispose();
    }

    [Fact]
    public void GetTools_ExposesOneToolPerConfiguredSectionAndAggregateAsGetLatestContent()
    {
        // Arrange
        var sut = CreateSut(Section("all"), Section("github-copilot"), Section("ai"), Section("dotnet"), Section("brand-new"));

        // Act
        var names = SectionToolNames(sut);

        // Assert
        names.Should().BeEquivalentTo(
            "get_latest_content",
            "get_latest_github_copilot_content",
            "get_latest_ai_content",
            "get_latest_dotnet_content",
            "get_latest_brand_new_content");
    }

    [Fact]
    public void GetTools_AlwaysExposesGetRoundupContentTool()
    {
        // Arrange
        var sut = CreateSut(Section("ai"));

        // Act
        var tool = sut.GetTools().Single(t => t.ProtocolTool.Name == ContentToolCatalog.GetRoundupContentToolName).ProtocolTool;

        // Assert
        tool.Description.Should().Contain("roundup");
        tool.InputSchema.GetProperty("properties").EnumerateObject().Select(p => p.Name).Should().Equal("url");
        tool.InputSchema.GetProperty("required").EnumerateArray().Select(e => e.GetString()).Should().Equal("url");
    }

    [Fact]
    public void GetTools_LatestContentDescriptionsDirectAgentsToTechHubUrlAndRoundupTool()
    {
        // Arrange
        var sut = CreateSut(Section("azure"));

        // Act
        var description = SectionTools(sut).Should().ContainSingle().Subject.ProtocolTool.Description;

        // Assert
        description.Should().Contain("link users to the Tech Hub url").And.Contain(ContentToolCatalog.GetRoundupContentToolName)
            .And.Contain("never the full text").And.NotContain("sourceUrl");
    }

    [Fact]
    public void GetTools_NamesAreStableReadableSnakeCase()
    {
        // Arrange
        var sut = CreateSut(Section("all"), Section("github-copilot"), Section("ml"), Section("devops"));

        // Act
        var tools = sut.GetTools();

        // Assert
        tools.Should().OnlyContain(t => ToolNamePattern().IsMatch(t.ProtocolTool.Name));
        ContentToolCatalog.GetToolName("github-copilot").Should().Be("get_latest_github_copilot_content");
        ContentToolCatalog.GetToolName("all").Should().Be("get_latest_content");
    }

    [Fact]
    public void GetTools_UsesSectionTitleForTitleAndDescription()
    {
        // Arrange
        var sut = CreateSut(Section("ml", title: "Machine Learning"));

        // Act
        var tool = SectionTools(sut).Should().ContainSingle().Subject.ProtocolTool;

        // Assert
        tool.Title.Should().Be("Latest Machine Learning content");
        tool.Description.Should().Contain("Machine Learning").And.Contain("page");
    }

    [Fact]
    public void GetTools_AggregateToolDescribesCrossSectionDeduplication()
    {
        // Arrange
        var sut = CreateSut(Section("all", title: "All"), Section("ai"));

        // Act
        var tool = sut.GetTools().Single(t => t.ProtocolTool.Name == "get_latest_content").ProtocolTool;

        // Assert
        tool.Title.Should().Be("Latest content across all sections");
        tool.Description.Should().Contain("once");
    }

    [Fact]
    public void GetTools_DescriptionsExplainSortOrderDefaultWindowAndRoundupShortcut()
    {
        // Arrange
        var sut = CreateSut(Section("azure"));

        // Act
        var description = SectionTools(sut).Should().ContainSingle().Subject.ProtocolTool.Description;

        // Assert
        description.Should().Contain("newest first")
            .And.Contain("90 days")
            .And.Contain("roundups")
            .And.Contain("generatedAt");
    }

    [Fact]
    public void GetTools_AreDeclaredReadOnlyClosedWorld()
    {
        // Arrange
        var sut = CreateSut(Section("azure"), Section("security"));

        // Act
        var tools = sut.GetTools();

        // Assert
        foreach (var tool in tools)
        {
            tool.ProtocolTool.Annotations.Should().NotBeNull();
            tool.ProtocolTool.Annotations!.ReadOnlyHint.Should().BeTrue();
            tool.ProtocolTool.Annotations.OpenWorldHint.Should().BeFalse();
            tool.ProtocolTool.Annotations.DestructiveHint.Should().BeFalse();
            tool.ProtocolTool.Annotations.IdempotentHint.Should().BeTrue();
        }
    }

    [Fact]
    public void GetTools_ExposeOnlyOptionalDescribedFilterArguments()
    {
        // Arrange
        var sut = CreateSut(Section("azure"));

        // Act
        var schema = SectionTools(sut).Should().ContainSingle().Subject.ProtocolTool.InputSchema;

        // Assert
        var properties = schema.GetProperty("properties");
        properties.EnumerateObject().Select(p => p.Name).Should().BeEquivalentTo(
            "page", "days", "since", "contentType", "tags", "query", "includeTags");
        foreach (var property in properties.EnumerateObject())
        {
            property.Value.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace(property.Name);
        }

        schema.TryGetProperty("required", out _)
            .Should().BeFalse("every filter is optional, so callers may omit all of them");
    }

    [Fact]
    public void GetTools_UnchangedSections_ReturnsSameToolInstances()
    {
        // Arrange
        var sut = CreateSut(Section("ai"), Section("azure"));

        // Act
        var first = sut.GetTools();
        var second = sut.GetTools();

        // Assert
        second.Should().BeSameAs(first);
    }

    [Fact]
    public void GetTools_AfterSectionRefresh_ReflectsNewSections()
    {
        // Arrange
        var cache = new SectionCache();
        cache.Initialize([Section("ai")]);
        var sut = new ContentToolCatalog(cache, _services);
        SectionToolNames(sut).Should().ContainSingle();

        // Act
        cache.Initialize([Section("ai"), Section("quantum")]);
        var names = SectionToolNames(sut);

        // Assert
        names.Should().BeEquivalentTo("get_latest_ai_content", "get_latest_quantum_content");
    }

    [Fact]
    public void GetTools_BeforeSectionsAreLoaded_ReturnsOnlyRoundupContentTool()
    {
        // Arrange
        var sut = new ContentToolCatalog(SectionCacheBuilder.Empty(), _services);

        // Act
        var names = sut.GetTools().Select(t => t.ProtocolTool.Name);

        // Assert
        names.Should().Equal(ContentToolCatalog.GetRoundupContentToolName);
    }

    private static IEnumerable<ModelContextProtocol.Server.McpServerTool> SectionTools(ContentToolCatalog sut) =>
        sut.GetTools().Where(t => t.ProtocolTool.Name != ContentToolCatalog.GetRoundupContentToolName);

    private static IEnumerable<string> SectionToolNames(ContentToolCatalog sut) =>
        SectionTools(sut).Select(t => t.ProtocolTool.Name);

    private ContentToolCatalog CreateSut(params Section[] sections)
    {
        var cache = new SectionCache();
        cache.Initialize(sections);
        return new ContentToolCatalog(cache, _services);
    }

    private static Section Section(string name, string? title = null) =>
        new SectionBuilder()
            .WithName(name)
            .WithTitle(title ?? name)
            .WithUrl($"/{name}")
            .Build();

    [GeneratedRegex("^get_(latest_([a-z]+(_[a-z]+)*_)?content|roundup_content)$")]
    private static partial Regex ToolNamePattern();
}
