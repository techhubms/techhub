using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for MobileSidebarToolbar.razor and MobileSidebarPanel.razor components.
/// Validates toolbar button rendering, panel toggle behavior, and active filter indicators.
/// </summary>
public class MobileSidebarToolbarTests : BunitContext
{
    /// <summary>
    /// Creates a RenderFragment that renders MobileSidebarPanel components inside the toolbar.
    /// </summary>
    private static RenderFragment CreatePanels(params (string Label, string Icon, bool HasActiveFilter)[] panels)
    {
        return builder =>
        {
            foreach (var (label, icon, hasActiveFilter) in panels)
            {
                builder.OpenComponent<MobileSidebarPanel>(0);
                builder.AddAttribute(1, nameof(MobileSidebarPanel.Label), label);
                builder.AddAttribute(2, nameof(MobileSidebarPanel.Icon), icon);
                builder.AddAttribute(3, nameof(MobileSidebarPanel.HasActiveFilter), hasActiveFilter);
                builder.AddAttribute(4, nameof(MobileSidebarPanel.ChildContent),
                    (RenderFragment)(childBuilder => childBuilder.AddContent(0, $"{label} content")));
                builder.CloseComponent();
            }
        };
    }

    [Fact]
    public void MobileSidebarToolbar_RendersToolbarButtons_ForRegisteredPanels()
    {
        // Act
        var cut = Render<MobileSidebarToolbar>(parameters => parameters
            .Add(p => p.ChildContent, CreatePanels(
                ("Search", "search", false),
                ("Tags", "tags", false))));

        // Assert
        var buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons.Should().HaveCount(2);
        buttons[0].TextContent.Should().Contain("Search");
        buttons[1].TextContent.Should().Contain("Tags");
    }

    [Fact]
    public void MobileSidebarToolbar_ButtonClick_ActivatesPanel()
    {
        // Arrange
        var cut = Render<MobileSidebarToolbar>(parameters => parameters
            .Add(p => p.ChildContent, CreatePanels(
                ("Search", "search", false),
                ("Tags", "tags", false))));

        // Act
        var buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons[0].Click();

        // Assert
        var firstButton = cut.FindAll(".sidebar-toolbar-btn")[0];
        firstButton.ClassList.Should().Contain("active");
    }

    [Fact]
    public void MobileSidebarToolbar_HasActiveFilter_ShowsIndicator_WhenPanelHasActiveFilter()
    {
        // Act
        var cut = Render<MobileSidebarToolbar>(parameters => parameters
            .Add(p => p.ChildContent, CreatePanels(
                ("Search", "search", true),
                ("Tags", "tags", false))));

        // Assert - First button should have has-active-filter class, second should not
        var buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons[0].ClassList.Should().Contain("has-active-filter");
        buttons[1].ClassList.Should().NotContain("has-active-filter");
    }

    [Fact]
    public void MobileSidebarToolbar_HasActiveFilter_ShowsDot_WhenPanelHasActiveFilter()
    {
        // Act
        var cut = Render<MobileSidebarToolbar>(parameters => parameters
            .Add(p => p.ChildContent, CreatePanels(
                ("Search", "search", true))));

        // Assert - Button should contain a filter indicator dot
        cut.Find(".sidebar-toolbar-btn").Should().NotBeNull();
        cut.Find(".sidebar-toolbar-filter-dot").Should().NotBeNull();
    }

    [Fact]
    public void MobileSidebarToolbar_HasActiveFilter_NoDot_WhenNoActiveFilter()
    {
        // Act
        var cut = Render<MobileSidebarToolbar>(parameters => parameters
            .Add(p => p.ChildContent, CreatePanels(
                ("Tags", "tags", false))));

        // Assert - No filter indicator dot should be present
        var dots = cut.FindAll(".sidebar-toolbar-filter-dot");
        dots.Should().BeEmpty();
    }

    [Fact]
    public void MobileSidebarToolbar_HasActiveFilter_MultipleActive()
    {
        // Act
        var cut = Render<MobileSidebarToolbar>(parameters => parameters
            .Add(p => p.ChildContent, CreatePanels(
                ("Search", "search", true),
                ("Date", "calendar", true),
                ("Tags", "tags", false))));

        // Assert - First two buttons have indicator, third does not
        var buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons[0].ClassList.Should().Contain("has-active-filter");
        buttons[1].ClassList.Should().Contain("has-active-filter");
        buttons[2].ClassList.Should().NotContain("has-active-filter");

        // Two dots should be present
        var dots = cut.FindAll(".sidebar-toolbar-filter-dot");
        dots.Should().HaveCount(2);
    }

    [Fact]
    public void MobileSidebarToolbar_HasActiveFilter_DefaultIsFalse()
    {
        // Act - Render panel without specifying HasActiveFilter (default)
        var cut = Render<MobileSidebarToolbar>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MobileSidebarPanel>(0);
                builder.AddAttribute(1, nameof(MobileSidebarPanel.Label), "Tags");
                builder.AddAttribute(2, nameof(MobileSidebarPanel.Icon), "tags");
                // Deliberately not setting HasActiveFilter to test default
                builder.AddAttribute(3, nameof(MobileSidebarPanel.ChildContent),
                    (RenderFragment)(childBuilder => childBuilder.AddContent(0, "Tags content")));
                builder.CloseComponent();
            }));

        // Assert - No active filter indicator
        var buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons[0].ClassList.Should().NotContain("has-active-filter");

        var dots = cut.FindAll(".sidebar-toolbar-filter-dot");
        dots.Should().BeEmpty();
    }

    [Fact]
    public void MobileSidebarToolbar_HasActiveFilter_DotHasAriaHidden()
    {
        // Act
        var cut = Render<MobileSidebarToolbar>(parameters => parameters
            .Add(p => p.ChildContent, CreatePanels(
                ("Search", "search", true))));

        // Assert - Dot should have aria-hidden for accessibility
        var dot = cut.Find(".sidebar-toolbar-filter-dot");
        dot.GetAttribute("aria-hidden").Should().Be("true");
    }

    [Fact]
    public void MobileSidebarToolbar_LateRegisteredPanels_ShowsButtonsAfterReRender()
    {
        // Arrange - Render toolbar initially without panels (simulates ContentItem page
        // where panels are inside @if (item != null) and item is null on first render)
        var cut = Render<MobileSidebarToolbar>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => { })));

        // Assert - No buttons initially
        var buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons.Should().BeEmpty();

        // Act - Re-render with panels (simulates item loading and panels becoming visible)
        cut.Render(parameters => parameters
            .Add(p => p.ChildContent, CreatePanels(
                ("Search", "search", false),
                ("Tags", "tags", false))));

        // Assert - Buttons should now appear after panels register
        buttons = cut.FindAll(".sidebar-toolbar-btn");
        buttons.Should().HaveCount(2);
        buttons[0].TextContent.Should().Contain("Search");
        buttons[1].TextContent.Should().Contain("Tags");
    }
}
