using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models.FeatureToggles;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminFeatureToggles.razor — the page where an admin switches a feature off, to admins or to everyone.
/// </summary>
public class AdminFeatureTogglesTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();
    private readonly FeatureToggleCache _cache = new();

    public AdminFeatureTogglesTests()
    {
        Services.AddSingleton(_api.Object);
        Services.AddSingleton(_cache);

        SetupToggles(Toggle(FeatureKeys.Timeline, FeatureToggleMode.Admins));
    }

    [Fact]
    public void Render_WithToggles_ShowsKeyDescriptionAndCurrentMode()
    {
        // Act
        var cut = Render<AdminFeatureToggles>();

        // Assert
        var row = cut.FindAll("tbody tr").Should().ContainSingle().Subject;
        row.TextContent.Should().Contain("timeline").And.Contain("Timeline feature");
        cut.Find("select#toggle-timeline").GetAttribute("value").Should().Be("Admins");
        cut.FindAll("select#toggle-timeline option").Select(o => o.TextContent).Should().Equal("Nobody", "Admins only", "Everyone");
    }

    [Fact]
    public void Render_NoToggles_ShowsEmptyMessage()
    {
        // Arrange
        SetupToggles();

        // Act
        var cut = Render<AdminFeatureToggles>();

        // Assert
        cut.Find(".admin-empty").TextContent.Should().Contain("No feature toggles found.");
    }

    [Fact]
    public void Render_ApiFails_ShowsGenericErrorWithoutLeakingDetails()
    {
        // Arrange
        _api.Setup(x => x.GetFeatureTogglesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("secret internal detail"));

        // Act
        var cut = Render<AdminFeatureToggles>();

        // Assert
        var alert = cut.Find(".admin-alert-error");
        alert.TextContent.Should().Contain("Failed to load the feature toggles.");
        alert.TextContent.Should().NotContain("secret internal detail");
    }

    [Fact]
    public void ChangeMode_SavesItRefreshesTheCacheAndConfirms()
    {
        // Arrange
        var cut = Render<AdminFeatureToggles>();
        _api.Setup(x => x.UpdateFeatureToggleAsync(FeatureKeys.Timeline, FeatureToggleMode.Everyone, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        SetupToggles(Toggle(FeatureKeys.Timeline, FeatureToggleMode.Everyone));

        // Act
        cut.Find("select#toggle-timeline").Change("Everyone");

        // Assert
        _api.Verify(x => x.UpdateFeatureToggleAsync(FeatureKeys.Timeline, FeatureToggleMode.Everyone, It.IsAny<CancellationToken>()), Times.Once);
        _cache.GetMode(FeatureKeys.Timeline).Should().Be(FeatureToggleMode.Everyone);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("timeline").And.Contain("everyone");
    }

    [Fact]
    public void ChangeMode_ToTheSameMode_DoesNotCallTheApi()
    {
        // Arrange
        var cut = Render<AdminFeatureToggles>();

        // Act
        cut.Find("select#toggle-timeline").Change("Admins");

        // Assert
        _api.Verify(x => x.UpdateFeatureToggleAsync(It.IsAny<string>(), It.IsAny<FeatureToggleMode>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void ChangeMode_ToAValueThatIsNotAMode_DoesNotCallTheApi()
    {
        // Arrange
        var cut = Render<AdminFeatureToggles>();

        // Act
        cut.Find("select#toggle-timeline").Change("7");

        // Assert
        _api.Verify(x => x.UpdateFeatureToggleAsync(It.IsAny<string>(), It.IsAny<FeatureToggleMode>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void ChangeMode_ApiFails_ShowsErrorAndLeavesTheCacheAlone()
    {
        // Arrange
        var cut = Render<AdminFeatureToggles>();
        _api.Setup(x => x.UpdateFeatureToggleAsync(It.IsAny<string>(), It.IsAny<FeatureToggleMode>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));

        // Act
        cut.Find("select#toggle-timeline").Change("Everyone");

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to update timeline.");
        _cache.GetMode(FeatureKeys.Timeline).Should().Be(FeatureToggleMode.Disabled);
    }

    private static FeatureToggle Toggle(string key, FeatureToggleMode mode) =>
        new() { Key = key, Mode = mode, Description = "Timeline feature" };

    private void SetupToggles(params FeatureToggle[] toggles) =>
        _api.Setup(x => x.GetFeatureTogglesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(toggles);
}
