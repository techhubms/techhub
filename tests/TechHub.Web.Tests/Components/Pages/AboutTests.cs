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

    [Fact]
    public void About_WithDeployImageTag_ShowsVersionInfo()
    {
        // Arrange
        RegisterBranding("tech.hub.ms", deployImageTag: "20260320123232");

        // Act
        var cut = Render<About>();

        // Assert
        var versionInfo = cut.Find(".version-info");
        versionInfo.Should().NotBeNull();
        versionInfo.TextContent.Should().Contain("Deployed");
        versionInfo.TextContent.Should().Contain("20 March 2026");
        versionInfo.TextContent.Should().Contain(".NET");
    }

    [Fact]
    public void About_WithoutDeployImageTag_ShowsVersionInfoWithoutDeployDate()
    {
        // Arrange
        RegisterBranding("tech.hub.ms");

        // Act
        var cut = Render<About>();

        // Assert
        var versionInfo = cut.Find(".version-info");
        versionInfo.Should().NotBeNull();
        versionInfo.TextContent.Should().Contain(".NET");
        versionInfo.TextContent.Should().NotContain("Deployed");
    }

    private void RegisterBranding(string host, string? deployImageTag = null)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Host = new HostString(host);
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var configData = new Dictionary<string, string?>();
        if (deployImageTag != null)
        {
            configData["DEPLOY_IMAGE_TAG"] = deployImageTag;
        }

        var config = new ConfigurationBuilder().AddInMemoryCollection(configData).Build();
        Services.AddScoped<BrandingService>(_ => new BrandingService(accessor, config));
        Services.AddSingleton<IConfiguration>(config);
    }
}
