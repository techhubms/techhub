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
                Options.Create(new McpOptions { BaseUrl = "https://techhub.test" })))
            .BuildServiceProvider();
    }

    public void Dispose()
    {
        _services.Dispose();
        _cache.Dispose();
    }

    [Fact]
    public void GetTools_ExposesOneToolPerConfiguredSectionExceptAll()
    {
        // Arrange
        var sut = CreateSut(Section("all"), Section("github-copilot"), Section("ai"), Section("dotnet"), Section("brand-new"));

        // Act
        var names = sut.GetTools().Select(t => t.ProtocolTool.Name);

        // Assert
        names.Should().BeEquivalentTo(
            "get_latest_github_copilot_content",
            "get_latest_ai_content",
            "get_latest_dotnet_content",
            "get_latest_brand_new_content");
    }

    [Fact]
    public void GetTools_NamesAreStableReadableSnakeCase()
    {
        // Arrange
        var sut = CreateSut(Section("github-copilot"), Section("ml"), Section("devops"));

        // Act
        var tools = sut.GetTools();

        // Assert
        tools.Should().OnlyContain(t => ToolNamePattern().IsMatch(t.ProtocolTool.Name));
        ContentToolCatalog.GetToolName("github-copilot").Should().Be("get_latest_github_copilot_content");
    }

    [Fact]
    public void GetTools_UsesSectionTitleForTitleAndDescription()
    {
        // Arrange
        var sut = CreateSut(Section("ml", title: "Machine Learning"));

        // Act
        var tool = sut.GetTools().Should().ContainSingle().Subject.ProtocolTool;

        // Assert
        tool.Title.Should().Be("Latest Machine Learning content");
        tool.Description.Should().Contain("Machine Learning").And.Contain("page");
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
    public void GetTools_ExposeOnlyOptionalDescribedPageArgument()
    {
        // Arrange
        var sut = CreateSut(Section("azure"));

        // Act
        var schema = sut.GetTools().Should().ContainSingle().Subject.ProtocolTool.InputSchema;

        // Assert
        var properties = schema.GetProperty("properties");
        properties.EnumerateObject().Select(p => p.Name).Should().Equal("page");
        properties.GetProperty("page").GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
        schema.TryGetProperty("required", out _)
            .Should().BeFalse("page defaults to 1, so callers may omit it");
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
        sut.GetTools().Should().ContainSingle();

        // Act
        cache.Initialize([Section("ai"), Section("quantum")]);
        var names = sut.GetTools().Select(t => t.ProtocolTool.Name);

        // Assert
        names.Should().BeEquivalentTo("get_latest_ai_content", "get_latest_quantum_content");
    }

    [Fact]
    public void GetTools_BeforeSectionsAreLoaded_ReturnsNoTools()
    {
        // Arrange
        var sut = new ContentToolCatalog(SectionCacheBuilder.Empty(), _services);

        // Act
        var tools = sut.GetTools();

        // Assert
        tools.Should().BeEmpty();
    }

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

    [GeneratedRegex("^get_latest_[a-z]+(_[a-z]+)*_content$")]
    private static partial Regex ToolNamePattern();
}
