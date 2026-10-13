using FluentAssertions;
using Microsoft.Extensions.Options;
using ModelContextProtocol;
using Moq;
using TechHub.TestUtilities.Builders;
using TechHub.Web.MCP;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.MCP;

public sealed class RoundupContentProviderTests
{
    private readonly Mock<ITechHubApiClient> _apiClient = new();

    [Theory]
    [InlineData("https://techhub.test/ai/roundups/weekly")]
    [InlineData("https://TECHHUB.test/ai/roundups/weekly")]
    [InlineData("/ai/roundups/weekly")]
    public async Task GetAsync_AcceptsUrlsOnConfiguredHostAndRelativePaths(string url)
    {
        // Arrange
        SetupDetail("weekly", "<h2>This week</h2>");
        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        result.Url.Should().Be("https://techhub.test/ai/roundups/weekly");
        _apiClient.VerifyAll();
    }

    [Fact]
    public async Task GetAsync_ReturnsMetadataWithBrusselsIsoDateAndRenderedContent()
    {
        // Arrange
        var detail = new ContentItemBuilder()
            .WithSlug("weekly")
            .WithTitle("Weekly AI roundup")
            .WithDateEpoch(new DateTimeOffset(2026, 9, 24, 8, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds())
            .WithCollectionName("roundups")
            .WithPrimarySectionName("ai")
            .WithRenderedHtml("<h2>This week</h2>")
            .BuildDetail();
        SetupDetail(detail);
        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync("/ai/roundups/weekly", TestContext.Current.CancellationToken);

        // Assert
        result.Title.Should().Be("Weekly AI roundup");
        result.Date.Should().Be("2026-09-24T10:00:00+02:00");
        result.Section.Should().Be("ai");
        result.Url.Should().Be("https://techhub.test/ai/roundups/weekly");
        result.Content.Should().Be("<h2>This week</h2>");
    }

    [Theory]
    [InlineData("news")]
    [InlineData("videos")]
    [InlineData("blogs")]
    [InlineData("community")]
    public async Task GetAsync_NonRoundupUrl_ThrowsMcpExceptionWithoutCallingApi(string collection)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var act = () => sut.GetAsync($"/ai/{collection}/my-slug", TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("roundup url");
        _apiClient.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("javascript:alert(1)")]
    [InlineData("https://evil.test/ai/roundups/weekly")]
    [InlineData("http://techhub.test/ai/roundups/weekly")]
    [InlineData("ai/roundups/weekly")]
    [InlineData("/ai/roundups")]
    [InlineData("/ai/roundups/weekly/extra")]
    [InlineData("/ai/roundups/my.slug")]
    [InlineData("/ai/roundups/weekly?x=1")]
    [InlineData("/ai/../roundups/weekly")]
    [InlineData("")]
    public async Task GetAsync_UnrecognizedUrl_ThrowsMcpExceptionWithoutCallingApi(string url)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var act = () => sut.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("url");
        _apiClient.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetAsync_UnknownRoundup_ThrowsMcpExceptionNotFound()
    {
        // Arrange
        _apiClient
            .Setup(c => c.GetContentDetailAsync("ai", "roundups", "missing", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TechHub.Core.Models.ContentItemDetail?)null);
        var sut = CreateSut();

        // Act
        var act = () => sut.GetAsync("/ai/roundups/missing", TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("not found");
    }

    [Fact]
    public async Task GetAsync_RoundupWithoutContent_ThrowsMcpException()
    {
        // Arrange
        var detail = new ContentItemBuilder()
            .WithSlug("weekly")
            .WithCollectionName("roundups")
            .WithPrimarySectionName("ai")
            .BuildDetail();
        SetupDetail(detail);
        var sut = CreateSut();

        // Act
        var act = () => sut.GetAsync("/ai/roundups/weekly", TestContext.Current.CancellationToken);

        // Assert
        (await act.Should().ThrowAsync<McpException>()).Which.Message.Should().Contain("no content");
    }

    private RoundupContentProvider CreateSut() =>
        new(_apiClient.Object, Options.Create(new McpOptions { BaseUrl = "https://techhub.test" }));

    private void SetupDetail(string slug, string renderedHtml) =>
        SetupDetail(new ContentItemBuilder()
            .WithSlug(slug)
            .WithCollectionName("roundups")
            .WithPrimarySectionName("ai")
            .WithRenderedHtml(renderedHtml)
            .BuildDetail());

    private void SetupDetail(TechHub.Core.Models.ContentItemDetail detail) =>
        _apiClient
            .Setup(c => c.GetContentDetailAsync("ai", "roundups", detail.Slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(detail);
}
