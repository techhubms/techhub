using FluentAssertions;
using TechHub.Core.Models.FeatureToggles;

namespace TechHub.Core.Tests.Models;

public class FeatureToggleModeTests
{
    [Theory]
    [InlineData(FeatureToggleMode.Disabled, false, false)]
    [InlineData(FeatureToggleMode.Disabled, true, false)]
    [InlineData(FeatureToggleMode.Admins, false, false)]
    [InlineData(FeatureToggleMode.Admins, true, true)]
    [InlineData(FeatureToggleMode.Everyone, false, true)]
    [InlineData(FeatureToggleMode.Everyone, true, true)]
    public void IsEnabledFor_Mode_DependsOnTheVisitorOnlyForAdmins(FeatureToggleMode mode, bool isAdmin, bool expected)
    {
        // Act
        var result = mode.IsEnabledFor(isAdmin);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void IsEnabledFor_ModeThatIsNotDefined_IsDisabled()
    {
        // Act
        var result = ((FeatureToggleMode)99).IsEnabledFor(isAdmin: true);

        // Assert
        result.Should().BeFalse();
    }
}
