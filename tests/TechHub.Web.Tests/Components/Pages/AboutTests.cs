using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Web.Components.Pages;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components.Pages;

/// <summary>
/// Tests for About.razor component - domain-based branding on the about page.
/// </summary>
public class AboutTests : BunitContext
{
    [Fact]
    public void About_XebiaDomain_ShowsXebiaSectionAtTop()
    {
        // Arrange
        RegisterBranding("tech.xebia.ms");

        // Act
        var cut = Render<About>();

        // Assert - Xebia should be the h1 (first heading)
        var h1 = cut.Find("h1");
        h1.TextContent.Trim().Should().Be("Xebia");
    }

    [Fact]
    public void About_DefaultDomain_ShowsWhoAreWeAsH1()
    {
        // Arrange
        RegisterBranding("tech.hub.ms");

        // Act
        var cut = Render<About>();

        // Assert
        var h1 = cut.Find("h1");
        h1.TextContent.Trim().Should().Be("Who are we");
    }

    [Fact]
    public void About_XebiaDomain_ShowsXebiaSection()
    {
        // Arrange
        RegisterBranding("tech.xebia.ms");

        // Act
        var cut = Render<About>();

        // Assert
        var xebiaSection = cut.Find(".xebia-section");
        xebiaSection.Should().NotBeNull();
    }

    [Fact]
    public void About_XebiaDomain_XebiaSectionContainsLink()
    {
        // Arrange
        RegisterBranding("tech.xebia.ms");

        // Act
        var cut = Render<About>();

        // Assert
        var xebiaLink = cut.Find(".xebia-section a[href='https://xebia.com/']");
        xebiaLink.Should().NotBeNull();
        xebiaLink.TextContent.Should().Contain("Xebia");
    }

    [Fact]
    public void About_XebiaDomain_MentionsCompanyContext()
    {
        // Arrange
        RegisterBranding("tech.xebia.ms");

        // Act
        var cut = Render<About>();

        // Assert
        var xebiaSection = cut.Find(".xebia-section");
        xebiaSection.TextContent.Should().Contain("Xebia");
    }

    private void RegisterBranding(string host)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Host = new HostString(host);
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var config = new ConfigurationBuilder().Build();
        Services.AddScoped<BrandingService>(_ => new BrandingService(accessor, config));
    }
}
