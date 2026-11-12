using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechHub.Core.Models;
using TechHub.Core.Models.FeatureToggles;
using TechHub.Web.Components;
using TechHub.Web.Services;
using TechHub.Web.Tests.TestSupport;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// The navigation buttons of a collection that has a feature: the navigation, the sub navigation and the section card
/// show them only to the visitors that the feature toggle lets in. A collection without a feature is always shown.
/// </summary>
public class FeatureGatedNavigationTests : BunitContext
{
    private readonly FeatureGateSetup _features;
    private readonly Section _section;

    public FeatureGatedNavigationTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        _features = this.AddFeatureGate();

        _section = new Section(
            "ai",
            "Artificial Intelligence",
            "AI resources",
            "/ai",
            "AI",
            [
                new Collection("news", "News", "/ai/news", "Latest news", "News"),
                new Collection("handbook", "Handbook", "/ai/handbook", "Handbook page", "Handbook", isCustom: true, order: 1),
                new Collection("timeline", "Timeline", "/ai/timeline", "Timeline page", "Timeline", isCustom: true, order: 2, feature: FeatureKeys.Timeline)
            ]);

        var sectionCache = new SectionCache();
        sectionCache.Initialize([_section]);
        Services.AddSingleton(sectionCache);
        Services.AddSingleton(new HomepageStatsState());
        Services.AddSingleton(Mock.Of<ITechHubApiClient>());

        var accessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
        Services.AddScoped<BrandingService>(_ => new BrandingService(accessor, new ConfigurationBuilder().Build()));
    }

    public static TheoryData<FeatureToggleMode, bool, bool> Visibility => new()
    {
        { FeatureToggleMode.Disabled, false, false },
        { FeatureToggleMode.Disabled, true, false },
        { FeatureToggleMode.Admins, false, false },
        { FeatureToggleMode.Admins, true, true },
        { FeatureToggleMode.Everyone, false, true },
        { FeatureToggleMode.Everyone, true, true }
    };

    [Theory]
    [MemberData(nameof(Visibility))]
    public void SubNav_CollectionWithFeature_IsShownOnlyToTheVisitorsThatTheToggleLetsIn(FeatureToggleMode mode, bool isAdmin, bool shown)
    {
        // Arrange
        Configure(mode, isAdmin);

        // Act
        var cut = Render<SubNav>(p => p.Add(x => x.Section, _section));

        // Assert
        cut.FindAll("a[href='/ai/timeline']").Any().Should().Be(shown);
        cut.FindAll("a[href='/ai/handbook']").Should().NotBeEmpty("a collection without a feature is always shown");
    }

    [Theory]
    [MemberData(nameof(Visibility))]
    public void NavHeader_CollectionWithFeature_IsShownOnlyToTheVisitorsThatTheToggleLetsIn(FeatureToggleMode mode, bool isAdmin, bool shown)
    {
        // Arrange
        Configure(mode, isAdmin);
        var cut = Render<NavHeader>();
        cut.Find(".hamburger-btn").Click();

        // Act
        cut.FindAll(".mobile-menu-section-header")[1].Click();

        // Assert
        cut.FindAll(".mobile-menu-sub-items a[href='/ai/timeline']").Any().Should().Be(shown);
        cut.FindAll(".mobile-menu-sub-items a[href='/ai/handbook']").Should().NotBeEmpty();
    }

    [Theory]
    [MemberData(nameof(Visibility))]
    public void SectionCard_CollectionWithFeature_IsShownOnlyToTheVisitorsThatTheToggleLetsIn(FeatureToggleMode mode, bool isAdmin, bool shown)
    {
        // Arrange
        Configure(mode, isAdmin);

        // Act
        var cut = Render<SectionCard>(p => p.Add(x => x.Section, _section));

        // Assert
        cut.FindAll("a[href='/ai/timeline']").Any().Should().Be(shown);
        cut.FindAll("a[href='/ai/handbook']").Should().NotBeEmpty();
    }

    [Fact]
    public void SubNav_ToggleWithoutRow_HidesTheCollection()
    {
        // Arrange: no toggle at all, as when the first load of the toggles failed
        _features.SetToggles();
        _features.Authorization.SetAuthorized("admin@example.com");

        // Act
        var cut = Render<SubNav>(p => p.Add(x => x.Section, _section));

        // Assert
        cut.FindAll("a[href='/ai/timeline']").Should().BeEmpty();
    }

    [Fact]
    public void SubNav_ToggleChangedAfterwards_IsPickedUpOnTheNextRender()
    {
        // Arrange
        Configure(FeatureToggleMode.Disabled, isAdmin: false);
        var cut = Render<SubNav>(p => p.Add(x => x.Section, _section));
        cut.FindAll("a[href='/ai/timeline']").Should().BeEmpty();

        // Act
        _features.SetToggles((FeatureKeys.Timeline, FeatureToggleMode.Everyone));
        cut.Render();

        // Assert
        cut.FindAll("a[href='/ai/timeline']").Should().NotBeEmpty();
    }

    private void Configure(FeatureToggleMode mode, bool isAdmin)
    {
        _features.SetToggles((FeatureKeys.Timeline, mode));
        if (isAdmin)
        {
            _features.Authorization.SetAuthorized("admin@example.com");
        }
    }
}
