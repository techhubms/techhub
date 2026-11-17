using System.Net;
using AngleSharp.Html.Parser;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.Core.Models.FeatureToggles;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Endpoints;

/// <summary>
/// The page of a feature that is not available to the visitor answers like a page that does not exist: a real HTTP 404 with the
/// "Page Not Found" content, and no hint that signing in would help. Runs the real Web pipeline in memory, so it holds the same
/// on every environment without changing the toggle of a deployed one (which an E2E test cannot do).
/// </summary>
public class FeatureGatedPageEndpointTests(DevelopmentWebFixture fixture) : IClassFixture<DevelopmentWebFixture>
{
    private const string PageUrl = "/github-copilot/timeline";

    [Theory]
    [InlineData(FeatureToggleMode.Disabled)]
    [InlineData(FeatureToggleMode.Admins)]
    public async Task Get_TimelineNotOpenToTheVisitor_AnswersNotFoundAndMentionsNoLogin(FeatureToggleMode mode)
    {
        // Arrange
        SetUp(mode);
        using var client = fixture.CreateClient("10.20.30.1");

        // Act
        using var response = await client.GetAsync(PageUrl, TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        html.Should().Contain("Page Not Found");
        html.Should().NotContainEquivalentOf("log in").And.NotContainEquivalentOf("sign in");
    }

    [Fact]
    public async Task Get_TimelineOpenToEveryone_AnswersOk()
    {
        // Arrange
        SetUp(FeatureToggleMode.Everyone);
        using var client = fixture.CreateClient("10.20.30.2");

        // Act
        using var response = await client.GetAsync(PageUrl, TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        html.Should().NotContain("Page Not Found");
    }

    [Theory]
    [InlineData("", "Next 6 months")]
    [InlineData("&future=hide", "Hide future events")]
    [InlineData("&future=show", "All future events")]
    public async Task Get_TimelineFutureWindow_RendersSelectedFilterAndPreservesItInHistoryLink(string futureQuery, string selectedLabel)
    {
        // Arrange
        SetUp(FeatureToggleMode.Everyone);
        using var client = fixture.CreateClient("10.20.30.3");

        // Act
        using var response = await client.GetAsync($"{PageUrl}?kinds=retirement&since=all{futureQuery}", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        using var document = new HtmlParser().ParseDocument(html);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var active = document.QuerySelector("nav[aria-label='Future events'] a[aria-current='true']");
        active.Should().NotBeNull();
        active!.TextContent.Should().Be(selectedLabel);
        document.QuerySelector(".timeline-page-window a")!.GetAttribute("href")
            .Should().Be($"{PageUrl}?kinds=retirement{futureQuery}");
    }

    // The section must exist, so a 404 can only come from the toggle and not from an unknown section.
    private void SetUp(FeatureToggleMode mode)
    {
        fixture.Services.GetRequiredService<SectionCache>().Initialize(
        [
            new Section("github-copilot", "GitHub Copilot", "GitHub Copilot", "/github-copilot", "GHC",
            [
                new Collection("news", "News", "/github-copilot/news", "News", "News"),
                new Collection("timeline", "Timeline", "/github-copilot/timeline", "Timeline", "Timeline", isCustom: true, feature: FeatureKeys.Timeline)
            ])
        ]);

        fixture.Services.GetRequiredService<FeatureToggleCache>().Initialize(
            [new FeatureToggle { Key = FeatureKeys.Timeline, Mode = mode, Description = string.Empty }]);
    }
}
