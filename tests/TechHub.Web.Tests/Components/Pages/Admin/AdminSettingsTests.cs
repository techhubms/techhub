using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.Core.Models.Admin;
using TechHub.TestUtilities.Builders;
using TechHub.Web.Components.Pages.Admin;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages.Admin;

/// <summary>
/// bUnit tests for AdminSettings.razor — background job toggles and cache invalidation.
/// </summary>
public class AdminSettingsTests : AdminPageTestBase
{
    private readonly Mock<ITechHubApiClient> _api = new();
    private readonly SectionCache _sectionCache = A.SectionCache.WithSections("ai").Build();
    private readonly HeroBannerCache _heroBannerCache = new();

    public AdminSettingsTests()
    {
        Services.AddSingleton(_api.Object);
        Services.AddSingleton(_sectionCache);
        Services.AddSingleton(_heroBannerCache);

        SetupSettings([CreateSetting("ContentProcessor", true), CreateSetting("Newsletter", false)]);
    }

    [Fact]
    public void Render_WithSettings_ShowsJobNameDescriptionAndState()
    {
        // Act
        var cut = Render<AdminSettings>();

        // Assert
        var rows = cut.FindAll("tbody tr");
        rows.Should().HaveCount(2);
        rows[0].TextContent.Should().Contain("ContentProcessor").And.Contain("Enabled");
        rows[1].TextContent.Should().Contain("Newsletter").And.Contain("Disabled");
        rows[0].QuerySelector("button")!.ClassList.Should().Contain("btn");
        rows[1].QuerySelector("button")!.ClassList.Should().Contain("btn-secondary");
    }

    [Fact]
    public void Render_NoSettings_ShowsEmptyMessage()
    {
        // Arrange
        SetupSettings([]);

        // Act
        var cut = Render<AdminSettings>();

        // Assert
        cut.Find(".admin-empty").TextContent.Should().Contain("No background job settings found.");
    }

    [Fact]
    public void Render_ApiFails_ShowsGenericErrorWithoutLeakingDetails()
    {
        // Arrange
        _api.Setup(x => x.GetJobSettingsAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("secret internal detail"));

        // Act
        var cut = Render<AdminSettings>();

        // Assert
        var alert = cut.Find(".admin-alert-error");
        alert.TextContent.Should().Contain("Failed to load job settings.");
        alert.TextContent.Should().NotContain("secret internal detail");
    }

    [Fact]
    public void Toggle_EnabledJob_DisablesItAndReloads()
    {
        // Arrange
        var cut = Render<AdminSettings>();

        // Act
        cut.FindAll("tbody tr")[0].QuerySelector("button")!.Click();

        // Assert
        _api.Verify(x => x.UpdateJobSettingAsync("ContentProcessor", false, It.IsAny<CancellationToken>()), Times.Once);
        _api.Verify(x => x.GetJobSettingsAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        cut.Find(".admin-alert-success").TextContent.Should().Contain("ContentProcessor has been disabled.");
    }

    [Fact]
    public void Toggle_DisabledJob_EnablesIt()
    {
        // Arrange
        var cut = Render<AdminSettings>();

        // Act
        cut.FindAll("tbody tr")[1].QuerySelector("button")!.Click();

        // Assert
        _api.Verify(x => x.UpdateJobSettingAsync("Newsletter", true, It.IsAny<CancellationToken>()), Times.Once);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("Newsletter has been enabled.");
    }

    [Fact]
    public void Toggle_ApiFails_ShowsErrorMessage()
    {
        // Arrange
        _api.Setup(x => x.UpdateJobSettingAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("boom"));
        var cut = Render<AdminSettings>();

        // Act
        cut.FindAll("tbody tr")[0].QuerySelector("button")!.Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to update ContentProcessor.");
        _api.Verify(x => x.GetJobSettingsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void InvalidateCaches_Success_RefreshesSectionAndHeroBannerCaches()
    {
        // Arrange
        var newSections = new[] { A.Section.Build() };
        var heroData = new HeroBannerData { Cards = [], Label = "Events" };
        _api.Setup(x => x.GetAllSectionsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(newSections);
        _api.Setup(x => x.GetHeroBannerDataAsync(It.IsAny<CancellationToken>())).ReturnsAsync(heroData);
        var cut = Render<AdminSettings>();

        // Act
        cut.FindAll(".admin-actions .btn-secondary")[1].Click();

        // Assert
        _api.Verify(x => x.InvalidateCachesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _sectionCache.GetSectionByName("github-copilot").Should().NotBeNull();
        _sectionCache.GetSectionByName("ai").Should().BeNull("the cache is replaced, not merged");
        _heroBannerCache.IsInitialized.Should().BeTrue();
        _heroBannerCache.Data.Should().BeSameAs(heroData);
        cut.Find(".admin-alert-success").TextContent.Should().Contain("All caches invalidated successfully.");
    }

    [Fact]
    public void InvalidateCaches_ApiReturnsNoSections_KeepsExistingSectionCache()
    {
        // Arrange
        _api.Setup(x => x.GetAllSectionsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Array.Empty<Section>());
        _api.Setup(x => x.GetHeroBannerDataAsync(It.IsAny<CancellationToken>())).ReturnsAsync((HeroBannerData?)null);
        var cut = Render<AdminSettings>();

        // Act
        cut.FindAll(".admin-actions .btn-secondary")[1].Click();

        // Assert
        _sectionCache.IsReady.Should().BeTrue("an empty API answer must not wipe the cache");
        _sectionCache.GetSectionByName("ai").Should().NotBeNull();
        _heroBannerCache.IsInitialized.Should().BeTrue();
        _heroBannerCache.Data.Should().BeNull();
    }

    [Fact]
    public void InvalidateCaches_ApiFails_ShowsErrorAndLeavesCachesUntouched()
    {
        // Arrange
        _api.Setup(x => x.InvalidateCachesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new HttpRequestException("boom"));
        _heroBannerCache.Initialize(new HeroBannerData { Cards = [] });
        var cut = Render<AdminSettings>();

        // Act
        cut.FindAll(".admin-actions .btn-secondary")[1].Click();

        // Assert
        cut.Find(".admin-alert-error").TextContent.Should().Contain("Failed to invalidate caches.");
        _heroBannerCache.IsInitialized.Should().BeTrue();
        _api.Verify(x => x.GetAllSectionsAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static BackgroundJobSetting CreateSetting(string name, bool enabled) =>
        new() { JobName = name, Enabled = enabled, Description = name + " job" };

    private void SetupSettings(IReadOnlyList<BackgroundJobSetting> settings) =>
        _api.Setup(x => x.GetJobSettingsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(settings);
}
