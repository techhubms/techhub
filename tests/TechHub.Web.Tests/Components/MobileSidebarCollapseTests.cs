using Bunit;
using FluentAssertions;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for MobileSidebarCollapse.razor component - collapsible sidebar wrapper for mobile.
/// Uses a div with Blazor click handler for accessible collapse/expand behavior.
/// Desktop CSS hides the toggle and makes the wrapper transparent via display: contents.
/// </summary>
public class MobileSidebarCollapseTests : BunitContext
{
    [Fact]
    public void MobileSidebarCollapse_RendersWrapperDiv()
    {
        // Act
        var cut = Render<MobileSidebarCollapse>(parameters => parameters
            .Add(p => p.Label, "Table of Contents")
            .AddChildContent("<div class='test-child'>Content</div>"));

        // Assert
        var wrapper = cut.Find("div.sidebar-collapse");
        wrapper.Should().NotBeNull();
    }

    [Fact]
    public void MobileSidebarCollapse_RendersToggleButtonWithLabel()
    {
        // Act
        var cut = Render<MobileSidebarCollapse>(parameters => parameters
            .Add(p => p.Label, "Filters & Search")
            .AddChildContent("<div>Content</div>"));

        // Assert
        var button = cut.Find("button.sidebar-collapse-toggle");
        button.TextContent.Should().Contain("Filters & Search");
        button.GetAttribute("aria-expanded").Should().Be("false");
    }

    [Fact]
    public void MobileSidebarCollapse_RendersChildContent()
    {
        // Act
        var cut = Render<MobileSidebarCollapse>(parameters => parameters
            .Add(p => p.Label, "Sidebar")
            .AddChildContent("<p class='test-content'>Hello World</p>"));

        // Assert
        var content = cut.Find(".sidebar-collapse-content");
        content.InnerHtml.Should().Contain("Hello World");
        cut.Find(".test-content").Should().NotBeNull();
    }

    [Fact]
    public void MobileSidebarCollapse_DefaultLabel_IsSidebar()
    {
        // Act
        var cut = Render<MobileSidebarCollapse>(parameters => parameters
            .AddChildContent("<div>Content</div>"));

        // Assert
        var button = cut.Find("button.sidebar-collapse-toggle");
        button.TextContent.Should().Contain("Sidebar");
    }

    [Fact]
    public void MobileSidebarCollapse_HasChevronIcon()
    {
        // Act
        var cut = Render<MobileSidebarCollapse>(parameters => parameters
            .Add(p => p.Label, "Sidebar")
            .AddChildContent("<div>Content</div>"));

        // Assert
        var chevron = cut.Find(".sidebar-collapse-chevron");
        chevron.Should().NotBeNull();
    }

    [Fact]
    public void MobileSidebarCollapse_ToggleClick_AddsOpenClass()
    {
        // Arrange
        var cut = Render<MobileSidebarCollapse>(parameters => parameters
            .Add(p => p.Label, "Sidebar")
            .AddChildContent("<div>Content</div>"));

        // Act
        cut.Find("button.sidebar-collapse-toggle").Click();

        // Assert
        var wrapper = cut.Find("div.sidebar-collapse");
        wrapper.ClassList.Should().Contain("sidebar-collapse-open");
        cut.Find("button.sidebar-collapse-toggle").GetAttribute("aria-expanded").Should().Be("true");
    }

    [Fact]
    public void MobileSidebarCollapse_DoubleToggle_RemovesOpenClass()
    {
        // Arrange
        var cut = Render<MobileSidebarCollapse>(parameters => parameters
            .Add(p => p.Label, "Sidebar")
            .AddChildContent("<div>Content</div>"));

        // Act
        cut.Find("button.sidebar-collapse-toggle").Click();
        cut.Find("button.sidebar-collapse-toggle").Click();

        // Assert
        var wrapper = cut.Find("div.sidebar-collapse");
        wrapper.ClassList.Should().NotContain("sidebar-collapse-open");
        cut.Find("button.sidebar-collapse-toggle").GetAttribute("aria-expanded").Should().Be("false");
    }
}
