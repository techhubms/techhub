using FluentAssertions;
using Microsoft.Extensions.Options;
using ModelContextProtocol;
using Moq;
using TechHub.TestUtilities.Builders;
using TechHub.Web.MCP;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.MCP;

public sealed class ContentItemProviderTests
{
    private readonly Mock<ITechHubApiClient> _apiClient = new();

    [Theory]
    [InlineData("https://techhub.test/ai/news/my-slug")]
    [InlineData("https://TECHHUB.test/ai/news/my-slug")]
    [InlineData("/ai/news/my-slug")]
    public async Task GetAsync_AcceptsItemUrlsOnConfiguredHostAndRelativePaths(string url)
    {
        // Arrange
        SetupDetail("ai", "news", "my-slug", new ContentItemBuilder().WithSlug("my-slug").WithPrimarySectionName("ai").BuildDetail());
        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync(url, includeTags: false, TestContext.Current.CancellationToken);

        // Assert
        result.Url.Should().Be("https://techhub.test/ai/news/my-slug");
        _apiClient.VerifyAll();
    }

    [Fact]
    public async Task GetAsync_ExternalCollection_ReturnsExcerptAndSourceUrlButNoContent()
    {
        // Arrange - news links externally, so Tech Hub only republishes the excerpt
        var detail = new ContentItemBuilder()
            .WithSlug("my-slug")
            .WithCollectionName("news")
            .WithPrimarySectionName("ai")
            .WithExcerpt("Short summary")
            .WithExternalUrl("https://example.org/original")
            .WithRenderedHtml("<p>Full third-party article</p>")
            .BuildDetail();
        SetupDetail("ai", "news", "my-slug", detail);
        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync("/ai/news/my-slug", includeTags: false, TestContext.Current.CancellationToken);

        // Assert
        result.Excerpt.Should().Be("Short summary");
        result.SourceUrl.Should().Be("https://example.org/original");
        result.Content.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_InternalCollection_ReturnsRenderedContent()
    {
        // Arrange
        var detail = new ContentItemBuilder()
            .WithSlug("weekly")
            .WithCollectionName("roundups")
            .WithPrimarySectionName("ai")
            .WithRenderedHtml("<h2>This week</h2>")
            .BuildDetail();
        SetupDetail("ai", "roundups", "weekly", detail);
        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync("/ai/roundups/weekly", includeTags: false, TestContext.Current.CancellationToken);

        // Assert
        result.Content.Should().Be("<h2>This week</h2>");
        result.Collection.Should().Be("roundups");
    }

    [Fact]
    public async Task GetAsync_MapsMetadataWithBrusselsIsoDate()
    {
        // Arrange
        var detail = new ContentItemBuilder()
            .WithSlug("my-slug")
            .WithTitle("A title")
            .WithAuthor("Jane")
            .WithDateEpoch(new DateTimeOffset(2026, 9, 24, 8, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds())
            .WithPrimarySectionName("ai")
            .BuildDetail();
        SetupDetail("ai", "news", "my-slug", detail);
        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync("/ai/news/my-slug", includeTags: false, TestContext.Current.CancellationToken);

        // Assert
        result.Title.Should().Be("A title");
        result.Author.Should().Be("Jane");
        result.Date.Should().Be("2026-09-24T10:00:00+02:00");
        result.Section.Should().Be("ai");
    }

    [Fact]
    public async Task GetAsync_OnlyReturnsTagsWhenRequested()
    {
        // Arrange
        var detail = new ContentItemBuilder().WithSlug("my-slug").WithPrimarySectionName("ai").WithTags("mcp").BuildDetail();
        SetupDetail("ai", "news", "my-slug", detail);
        var sut = CreateSut();

        // Act
        var without = await sut.GetAsync("/ai/news/my-slug", includeTags: false, TestContext.Current.CancellationToken);
        var with = await sut.GetAsync("/ai/news/my-slug", includeTags: true, TestContext.Current.CancellationToken);

        // Assert
        without.Tags.Should().BeNull();
        with.Tags.Should().Equal("mcp");
    }

    [Theory]
    [InlineData("javascript:alert(1)")]
    [InlineData("https://evil.test/ai/news/my-slug")]
    [InlineData("http://techhub.test/ai/news/my-slug")]
    [InlineData("ai/news/my-slug")]
    [InlineData("/ai/news")]
    [InlineData("/ai/news/my-slug/extra")]
    [InlineData("/ai/news/my.slug")]
    [InlineData("/ai/news/my-slug?x=1")]
    [InlineData("/ai/../news/my-slug")]
    [InlineData("")]
    public async Task GetAsync_UnrecognizedUrl_ThrowsMcpExceptionWithoutCallingApi(string url)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var act = () => sut.GetAsync(url, includeTags: false, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("url");
        _apiClient.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetAsync_UnknownItem_ThrowsMcpExceptionNotFound()
    {
        // Arrange
        _apiClient
            .Setup(c => c.GetContentDetailAsync("ai", "news", "missing", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TechHub.Core.Models.ContentItemDetail?)null);
        var sut = CreateSut();

        // Act
        var act = () => sut.GetAsync("/ai/news/missing", includeTags: false, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("not found");
    }

    private ContentItemProvider CreateSut() =>
        new(_apiClient.Object, Options.Create(new McpOptions { BaseUrl = "https://techhub.test" }));

    private void SetupDetail(string section, string collection, string slug, TechHub.Core.Models.ContentItemDetail detail) =>
        _apiClient
            .Setup(c => c.GetContentDetailAsync(section, collection, slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(detail);
}
