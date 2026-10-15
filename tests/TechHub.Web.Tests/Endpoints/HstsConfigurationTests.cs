using FluentAssertions;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TechHub.Web.Tests.Endpoints;

public class HstsConfigurationTests(ProductionWebFixture production) : IClassFixture<ProductionWebFixture>
{
    [Fact]
    public void HstsOptions_ProductionConfiguration_UsesOneYearWithSubdomainsAndPreload()
    {
        // Arrange
        var options = production.Services.GetRequiredService<IOptions<HstsOptions>>().Value;

        // Assert
        options.MaxAge.Should().Be(TimeSpan.FromDays(365));
        options.IncludeSubDomains.Should().BeTrue();
        options.Preload.Should().BeTrue();
    }
}
