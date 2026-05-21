using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.Web.Components;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// Tests for SectionCollection.razor — the page used for both collection and Browse (/section/all) views.
/// Validates sidebar panel ordering and content-type filter visibility behavior.
/// </summary>
public class SectionCollectionTests : BunitContext
{
    public SectionCollectionTests()
    {
        // Stub complex child components that have their own DI requirements so
        // these tests can focus exclusively on toolbar/panel behavior.
        ComponentFactories.AddStub<SidebarToggle>();
        ComponentFactories.AddStub<SeoMetaTags>();
        ComponentFactories.AddStub<SidebarTagCloud>();
        ComponentFactories.AddStub<ContentItemsGrid>();
        ComponentFactories.AddStub<DateRangeSlider>();
        ComponentFactories.AddStub<HeroBanner>();

        // SectionCache is injected directly by SectionCollection.
        // Provide an "all" section with two regular (non-custom) collections so
        // ShowContentTypeFilter becomes true on the Browse page.
        var sectionCache = new SectionCache();
        sectionCache.Initialize(
        [
            new Section(
                "all",
                "All",
                "All content",
                "/all",
                "All",
                [
                    new Collection("articles", "Articles", "/all/articles", "Articles", "Articles"),
                    new Collection("videos", "Videos", "/all/videos", "Videos", "Videos"),
                ])
        ]);
        Services.AddSingleton(sectionCache);

        SetRendererInfo(new RendererInfo("Server", true));

        JSInterop.SetupVoid("markScriptsReady");
    }

    /// <summary>
    /// On a Browse page (/section/all) the "Content" filter toolbar button must be the
    /// first (leftmost) button. This test covers a fresh render directly in Browse mode.
    /// </summary>
    [Fact]
    public void SectionCollection_BrowsePage_ContentFilterButtonIsFirst()
    {
        // Act — render directly in Browse mode (/all/all)
        var cut = Render<SectionCollection>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        // Assert — Content should be first, then Search, Date, Tags
        var buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons.Should().HaveCount(4);
        buttons[0].TextContent.Should().Contain("Content");
        buttons[1].TextContent.Should().Contain("Search");
        buttons[2].TextContent.Should().Contain("Date");
        buttons[3].TextContent.Should().Contain("Tags");
    }

    /// <summary>
    /// When navigating from a regular collection page to the Browse page within the same
    /// component instance, the "Content" filter button must appear as the first (leftmost)
    /// button — not drift to the rightmost position due to late panel registration.
    /// Regression test for: "Order of sidebar buttons inconsistent on mobile".
    /// </summary>
    [Fact]
    public void SectionCollection_NavigateFromCollectionToBrowse_ContentFilterButtonIsFirst()
    {
        // Arrange — first render as a collection page (ShowContentTypeFilter = false)
        var cut = Render<SectionCollection>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "articles"));

        // Verify Content filter is absent on a collection page
        var buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons.Should().HaveCount(3); // Search, Date, Tags — no Content
        buttons[0].TextContent.Should().Contain("Search");

        // Act — navigate to Browse mode (same component instance, CollectionName → "all")
        cut.Render(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        // Assert — Content filter must now be the first button
        buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons.Should().HaveCount(4);
        buttons[0].TextContent.Should().Contain("Content");
        buttons[1].TextContent.Should().Contain("Search");
        buttons[2].TextContent.Should().Contain("Date");
        buttons[3].TextContent.Should().Contain("Tags");
    }

    /// <summary>
    /// When types URL parameter is set and then cleared (e.g., tag click drops types),
    /// the Content filter indicator should be removed.
    /// Regression test for: ParseTypesFromUrl not clearing activeContentTypes.
    /// </summary>
    [Fact]
    public void SectionCollection_TypesCleared_ContentFilterNoLongerActive()
    {
        // Arrange — navigate to URL with types=articles (Content filter active)
        var navMan = Services.GetRequiredService<NavigationManager>();
        navMan.NavigateTo("/all/all?types=articles");

        var cut = Render<SectionCollection>(parameters => parameters
            .Add(p => p.SectionName, "all")
            .Add(p => p.CollectionName, "all"));

        // Verify Content button has "has-active-filter" class
        var buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons[0].TextContent.Should().Contain("Content");
        buttons[0].ClassList.Should().Contain("has-active-filter",
            "Content filter button should show active indicator when types is set");

        // Act — navigate to URL without types (simulates URL losing types param)
        navMan.NavigateTo("/all/all");
        cut.Render();

        // Assert — Content filter should no longer be active
        buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons[0].TextContent.Should().Contain("Content");
        buttons[0].ClassList.Should().NotContain("has-active-filter",
            "Content filter should not show active indicator after types param is cleared");
    }
}
