using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.Web.Components;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SubNav.razor component - horizontal navigation below page header.
/// </summary>
public class SubNavTests : BunitContext
{
    public SubNavTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void SubNav_WithNoParameters_RendersNothing()
    {
        // Act - SubNav with no Section and no Sections renders the empty state
        var cut = Render<SubNav>();

        // Assert - Should not render any markup (collapsed)
        cut.Markup.Trim().Should().BeEmpty(
            "empty SubNav should collapse and render no HTML when there is no content");
    }

    [Fact]
    public void SubNav_WithSection_RendersNavElement()
    {
        // Arrange
        var section = new Section(
            "ai",
            "Artificial Intelligence",
            "AI resources",
            "/ai",
            "AI",
            [
                new Collection("news", "News", "/ai/news", "Latest news", "News"),
                new Collection("blogs", "Blogs", "/ai/blogs", "Blog posts", "Blogs")
            ]
        );

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Section, section));

        // Assert - Should render a nav element with sub-nav class
        var nav = cut.Find("nav.sub-nav");
        nav.Should().NotBeNull();
        nav.GetAttribute("aria-label").Should().Be("Artificial Intelligence navigation");
    }

    [Fact]
    public void SubNav_WithSections_RendersHomepageNav()
    {
        // Arrange
        var sections = new List<Section>
        {
            new("ai", "Artificial Intelligence", "AI", "/ai", "AI",
                [new Collection("news", "News", "/ai/news", "Latest news", "News")]),
            new("github-copilot", "GitHub Copilot", "Copilot", "/github-copilot", "GitHub Copilot",
                [new Collection("news", "News", "/github-copilot/news", "Latest news", "News")])
        };

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections));

        // Assert - Should render homepage subnav
        var nav = cut.Find("nav.sub-nav.sub-nav-homepage");
        nav.Should().NotBeNull();
    }

    [Fact]
    public void SubNav_WithEmptySections_RendersNothing()
    {
        // Arrange - empty sections list
        var sections = new List<Section>();

        // Act
        var cut = Render<SubNav>(parameters => parameters
            .Add(p => p.Sections, sections));

        // Assert - Should not render any markup when sections list is empty
        cut.Markup.Trim().Should().BeEmpty(
            "SubNav with empty Sections list should collapse and render no HTML");
    }
}
