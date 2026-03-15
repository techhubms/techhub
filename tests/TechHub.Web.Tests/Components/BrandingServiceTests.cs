using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Components;

/// <summary>
/// Tests for BrandingService - domain-based branding logic.
/// </summary>
public class BrandingServiceTests
{
    [Fact]
    public void BrandingService_XebiaDomain_IsXebiaBranding()
    {
        // Arrange
        var service = CreateBrandingService("tech.xebia.ms");

        // Assert
        service.IsXebiaBranding.Should().BeTrue();
    }

    [Fact]
    public void BrandingService_TechHubDomain_IsNotXebiaBranding()
    {
        // Arrange
        var service = CreateBrandingService("tech.hub.ms");

        // Assert
        service.IsXebiaBranding.Should().BeFalse();
    }

    [Fact]
    public void BrandingService_LocalhostDomain_IsNotXebiaBranding()
    {
        // Arrange
        var service = CreateBrandingService("localhost");

        // Assert
        service.IsXebiaBranding.Should().BeFalse();
    }

    [Fact]
    public void BrandingService_XebiaDomain_SiteNameIsXebia()
    {
        // Arrange
        var service = CreateBrandingService("tech.xebia.ms");

        // Assert
        service.SiteName.Should().Be("Xebia");
    }

    [Fact]
    public void BrandingService_TechHubDomain_SiteNameIsTechHub()
    {
        // Arrange
        var service = CreateBrandingService("tech.hub.ms");

        // Assert
        service.SiteName.Should().Be("Tech Hub");
    }

    [Fact]
    public void BrandingService_XebiaDomain_BannerTitleIsXebiaTechHub()
    {
        // Arrange
        var service = CreateBrandingService("tech.xebia.ms");

        // Assert
        service.BannerTitle.Should().Be("Xebia Tech Hub");
    }

    [Fact]
    public void BrandingService_TechHubDomain_BannerTitleIsTechHub()
    {
        // Arrange
        var service = CreateBrandingService("tech.hub.ms");

        // Assert
        service.BannerTitle.Should().Be("Tech Hub");
    }

    [Fact]
    public void BrandingService_XebiaDomain_ShowsCopilotLogo()
    {
        // Arrange
        var service = CreateBrandingService("tech.xebia.ms");

        // Assert
        service.ShowCopilotLogo.Should().BeTrue();
    }

    [Fact]
    public void BrandingService_TechHubDomain_DoesNotShowCopilotLogo()
    {
        // Arrange
        var service = CreateBrandingService("tech.hub.ms");

        // Assert
        service.ShowCopilotLogo.Should().BeFalse();
    }

    [Fact]
    public void BrandingService_XebiaDomain_CaseInsensitive()
    {
        // Arrange
        var service = CreateBrandingService("TECH.XEBIA.MS");

        // Assert
        service.IsXebiaBranding.Should().BeTrue();
    }

    [Fact]
    public void BrandingService_ForceXebia_OverridesDomain()
    {
        // Arrange - localhost domain but forced to xebia
        var service = CreateBrandingService("localhost", forceBranding: "xebia");

        // Assert
        service.IsXebiaBranding.Should().BeTrue();
        service.SiteName.Should().Be("Xebia");
        service.BannerTitle.Should().Be("Xebia Tech Hub");
        service.ShowCopilotLogo.Should().BeTrue();
    }

    [Fact]
    public void BrandingService_ForceTechHub_OverridesDomain()
    {
        // Arrange - xebia domain but forced to techhub
        var service = CreateBrandingService("tech.xebia.ms", forceBranding: "techhub");

        // Assert
        service.IsXebiaBranding.Should().BeFalse();
        service.SiteName.Should().Be("Tech Hub");
        service.BannerTitle.Should().Be("Tech Hub");
    }

    [Fact]
    public void BrandingService_ForceEmpty_FallsBackToDomain()
    {
        // Arrange - empty force value, should use domain
        var service = CreateBrandingService("tech.xebia.ms", forceBranding: "");

        // Assert
        service.IsXebiaBranding.Should().BeTrue();
    }

    private static BrandingService CreateBrandingService(string host, string? forceBranding = null)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Host = new HostString(host);
        var accessor = new HttpContextAccessor { HttpContext = httpContext };

        var configData = new Dictionary<string, string?>();
        if (forceBranding != null)
        {
            configData["Branding:Force"] = forceBranding;
        }

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        return new BrandingService(accessor, configuration);
    }
}
