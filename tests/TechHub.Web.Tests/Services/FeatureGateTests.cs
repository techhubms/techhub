using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models.FeatureToggles;
using TechHub.Web.Services;
using TechHub.Web.Tests.TestSupport;

namespace TechHub.Web.Tests.Services;

public class FeatureGateTests : BunitContext
{
    private readonly FeatureGateSetup _setup;

    public FeatureGateTests()
    {
        _setup = this.AddFeatureGate();
    }

    [Fact]
    public async Task IsEnabledAsync_NoFeature_IsAlwaysEnabled()
    {
        // Act
        var enabled = await Gate().IsEnabledAsync(null);

        // Assert
        enabled.Should().BeTrue();
    }

    [Fact]
    public async Task IsEnabledAsync_FeatureWithoutToggle_IsDisabledEvenForAnAdmin()
    {
        // Arrange
        _setup.Authorization.SetAuthorized("admin@example.com");

        // Act
        var enabled = await Gate().IsEnabledAsync("unknown");

        // Assert
        enabled.Should().BeFalse();
    }

    [Theory]
    [InlineData(FeatureToggleMode.Disabled, false, false)]
    [InlineData(FeatureToggleMode.Disabled, true, false)]
    [InlineData(FeatureToggleMode.Admins, false, false)]
    [InlineData(FeatureToggleMode.Admins, true, true)]
    [InlineData(FeatureToggleMode.Everyone, false, true)]
    [InlineData(FeatureToggleMode.Everyone, true, true)]
    public async Task IsEnabledAsync_Toggle_DecidesPerVisitor(FeatureToggleMode mode, bool isAdmin, bool expected)
    {
        // Arrange
        _setup.SetToggles(("f", mode));
        if (isAdmin)
        {
            _setup.Authorization.SetAuthorized("admin@example.com");
        }

        // Act
        var enabled = await Gate().IsEnabledAsync("f");

        // Assert
        enabled.Should().Be(expected);
    }

    [Fact]
    public async Task IsAdminAsync_SignedInVisitor_IsAnAdmin()
    {
        // Arrange
        _setup.Authorization.SetAuthorized("admin@example.com");

        // Act
        var isAdmin = await Gate().IsAdminAsync();

        // Assert
        isAdmin.Should().BeTrue();
    }

    [Fact]
    public async Task IsAdminAsync_AnonymousVisitor_IsNotAnAdmin()
    {
        // Act
        var isAdmin = await Gate().IsAdminAsync();

        // Assert
        isAdmin.Should().BeFalse();
    }

    [Fact]
    public void Cache_UnknownFeature_IsDisabled()
    {
        // Act
        var mode = new FeatureToggleCache().GetMode("anything");

        // Assert
        mode.Should().Be(FeatureToggleMode.Disabled);
    }

    [Fact]
    public void Cache_Initialize_ReplacesAllToggles()
    {
        // Arrange
        var cache = new FeatureToggleCache();
        cache.Initialize([new FeatureToggle { Key = "a", Mode = FeatureToggleMode.Everyone, Description = "" }]);

        // Act
        cache.Initialize([new FeatureToggle { Key = "b", Mode = FeatureToggleMode.Admins, Description = "" }]);

        // Assert
        cache.GetMode("a").Should().Be(FeatureToggleMode.Disabled);
        cache.GetMode("b").Should().Be(FeatureToggleMode.Admins);
    }

    private FeatureGate Gate() =>
        new(_setup.Cache, Services.GetRequiredService<AuthenticationStateProvider>());
}
