using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Web.Components;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for SidebarToggle.razor component - desktop sidebar collapse/expand toggle.
/// This component renders a button that toggles the sidebar collapsed state.
/// It is only visible on desktop (≥1293px) and hidden on mobile via CSS.
/// </summary>
public class SidebarToggleTests : BunitContext
{
    public SidebarToggleTests()
    {
        // Setup JS interop mock for the sidebar toggle
        JSInterop.SetupVoid("TechHub.sidebar.toggle");

        // Register IHttpContextAccessor with no cookie (expanded state)
        var httpContext = new DefaultHttpContext();
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        Services.AddSingleton<IHttpContextAccessor>(accessor);
    }

    [Fact]
    public void SidebarToggle_RendersButton()
    {
        // Act
        var cut = Render<SidebarToggle>();

        // Assert
        var button = cut.Find("button.sidebar-toggle");
        button.Should().NotBeNull();
    }

    [Fact]
    public void SidebarToggle_HasCorrectAriaAttributes_WhenExpanded()
    {
        // Act
        var cut = Render<SidebarToggle>();

        // Assert
        var button = cut.Find("button.sidebar-toggle");
        button.GetAttribute("aria-expanded").Should().Be("true");
        button.GetAttribute("aria-label").Should().Be("Collapse sidebar");
    }

    [Fact]
    public void SidebarToggle_HasChevronIcon()
    {
        // Act
        var cut = Render<SidebarToggle>();

        // Assert - Should contain an SVG chevron icon
        var svg = cut.Find("button.sidebar-toggle svg");
        svg.Should().NotBeNull();
    }
}
