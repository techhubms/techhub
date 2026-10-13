using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// bUnit tests for NewsletterSubscribe.razor — section loading, fallback and submit outcomes.
/// </summary>
public class NewsletterSubscribeTests : BunitContext
{
    private readonly Mock<ITechHubApiClient> _api = new();

    public NewsletterSubscribeTests()
    {
        _api.Setup(x => x.GetNewsletterSectionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateSections("ai", "azure"));

        Services.AddSingleton(_api.Object);
        Services.AddSingleton(A.SectionCache.WithSections("ai", "all").Build());
        JSInterop.SetupVoid("markScriptsReady");
    }

    [Fact]
    public void Render_ApiReturnsSections_ShowsWeeklyAndDailyCheckboxPerSection()
    {
        // Act
        var cut = Render<NewsletterSubscribe>();

        // Assert
        var groups = cut.FindAll(".subscribe-group");
        groups.Should().HaveCount(2);
        groups[0].QuerySelectorAll("input[type='checkbox']").Should().HaveCount(2);
        groups[1].QuerySelectorAll("input[type='checkbox']").Should().HaveCount(2);
        groups[0].TextContent.Should().Contain("AI").And.Contain("AZURE");
    }

    [Fact]
    public void Render_ApiReturnsNull_FallsBackToCachedSectionsWithoutAll()
    {
        // Arrange
        _api.Setup(x => x.GetNewsletterSectionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyList<Section>?)null);

        // Act
        var cut = Render<NewsletterSubscribe>();

        // Assert
        var weeklyCheckboxes = cut.FindAll(".subscribe-group")[0].QuerySelectorAll("input[type='checkbox']");
        weeklyCheckboxes.Should().ContainSingle("the virtual 'all' section is not subscribable");
    }

    [Fact]
    public void Render_ApiThrows_FallsBackToCachedSections()
    {
        // Arrange
        _api.Setup(x => x.GetNewsletterSectionsAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("down"));

        // Act
        var cut = Render<NewsletterSubscribe>();

        // Assert
        cut.FindAll(".subscribe-group")[0].QuerySelectorAll("input[type='checkbox']").Should().ContainSingle();
    }

    [Fact]
    public void Render_Initially_ShowsNoMessage()
    {
        // Act
        var cut = Render<NewsletterSubscribe>();

        // Assert
        cut.FindAll(".subscribe-alert").Should().BeEmpty();
    }

    [Fact]
    public void Subscribe_WithSelections_SendsEmailNameAndSectionsAndShowsSuccess()
    {
        // Arrange
        var cut = Render<NewsletterSubscribe>();
        cut.Find("#subscribe-email").Change("user@example.com");
        cut.Find("#subscribe-name").Change("Jane");
        cut.FindAll(".subscribe-group")[0].QuerySelectorAll("input[type='checkbox']")[1].Change(true);
        cut.FindAll(".subscribe-group")[1].QuerySelectorAll("input[type='checkbox']")[0].Change(true);

        // Act
        cut.Find("button.btn").Click();

        // Assert
        _api.Verify(
            x => x.SubscribeNewsletterAsync(
                "user@example.com",
                "Jane",
                It.Is<IReadOnlyList<string>>(l => l.SequenceEqual(new[] { "azure" })),
                It.Is<IReadOnlyList<string>>(l => l.SequenceEqual(new[] { "ai" })),
                It.IsAny<CancellationToken>()),
            Times.Once);
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-success");
        alert.TextContent.Should().Contain("Check your inbox");
    }

    [Fact]
    public void Subscribe_ApiFails_ShowsErrorAlert()
    {
        // Arrange
        _api.Setup(x => x.SubscribeNewsletterAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<IReadOnlyList<string>>(),
                It.IsAny<IReadOnlyList<string>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("bad request"));
        var cut = Render<NewsletterSubscribe>();
        cut.Find("#subscribe-email").Change("user@example.com");

        // Act
        cut.Find("button.btn").Click();

        // Assert
        var alert = cut.Find(".subscribe-alert");
        alert.ClassList.Should().Contain("subscribe-alert-error");
        alert.TextContent.Should().Contain("Subscription failed");
    }

    [Fact]
    public void Subscribe_UncheckingSection_RemovesItFromSubmission()
    {
        // Arrange
        var cut = Render<NewsletterSubscribe>();
        cut.Find("#subscribe-email").Change("user@example.com");
        cut.FindAll(".subscribe-group")[0].QuerySelectorAll("input[type='checkbox']")[0].Change(true);
        cut.FindAll(".subscribe-group")[0].QuerySelectorAll("input[type='checkbox']")[0].Change(false);

        // Act
        cut.Find("button.btn").Click();

        // Assert
        _api.Verify(
            x => x.SubscribeNewsletterAsync(
                "user@example.com",
                It.IsAny<string?>(),
                It.Is<IReadOnlyList<string>>(l => l.Count == 0),
                It.Is<IReadOnlyList<string>>(l => l.Count == 0),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    internal static IReadOnlyList<Section> CreateSections(params string[] names) =>
        names.Select(n => new Section(n, n.ToUpperInvariant(), n + " description", "/" + n, n, [new Collection("news", "News", $"/{n}/news", "News", "News")])).ToList();
}
