using Bunit;
using FluentAssertions;
using TechHub.Core.Models;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SectionBanner.razor component — banner/hero displayed at the top of each page.
/// </summary>
public class SectionBannerTests : BunitContext
{
    [Fact]
    public void SectionBanner_IsHomepage_True_AddsHomeBannerClass()
    {
        // Act
        var cut = Render<SectionBanner>(parameters => parameters
            .Add(p => p.Title, "Tech Hub")
            .Add(p => p.IsHomepage, true));

        // Assert
        var banner = cut.Find(".section-banner");
        banner.ClassList.Should().Contain("home-banner",
            "IsHomepage=true should apply the home-banner CSS class");
    }

    [Fact]
    public void SectionBanner_IsHomepage_True_ShowsHomeMobileTitle()
    {
        // Act
        var cut = Render<SectionBanner>(parameters => parameters
            .Add(p => p.Title, "Tech Hub")
            .Add(p => p.IsHomepage, true));

        // Assert
        var mobileTitle = cut.Find(".banner-title-mobile");
        mobileTitle.TextContent.Trim().Should().Be("Home",
            "IsHomepage=true should show 'Home' as the mobile title");
    }

    [Fact]
    public void SectionBanner_IsHomepage_False_DoesNotAddHomeBannerClass()
    {
        // Act - simulates newsletter page
        var cut = Render<SectionBanner>(parameters => parameters
            .Add(p => p.Title, "Newsletter")
            .Add(p => p.IsHomepage, false));

        // Assert
        var banner = cut.Find(".section-banner");
        banner.ClassList.Should().NotContain("home-banner",
            "Non-homepage custom-banner pages must not receive the home-banner CSS class");
    }

    [Fact]
    public void SectionBanner_NewsletterPage_ShowsNewsletterMobileTitle()
    {
        // Act - simulates newsletter page
        var cut = Render<SectionBanner>(parameters => parameters
            .Add(p => p.Title, "Newsletter")
            .Add(p => p.IsHomepage, false));

        // Assert
        var mobileTitle = cut.Find(".banner-title-mobile");
        mobileTitle.TextContent.Trim().Should().Be("Newsletter",
            "Newsletter page should show 'Newsletter' as the mobile title, not 'Home'");
    }

    [Fact]
    public void SectionBanner_WithSection_IgnoresIsHomepage()
    {
        // Arrange - section pages always use their own title and background
        var section = new Section("ai", "Artificial Intelligence", "AI", "/ai", "AI",
            [new Collection("news", "News", "/ai/news", "Latest news", "News")]);

        // Act
        var cut = Render<SectionBanner>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert - no home-banner class regardless of IsHomepage default (false)
        var banner = cut.Find(".section-banner");
        banner.ClassList.Should().NotContain("home-banner",
            "Section pages should never use the home-banner CSS class");
        cut.Find(".banner-title-mobile").TextContent.Trim().Should().Be("Artificial Intelligence",
            "Section page mobile title should be the section title");
    }
}
