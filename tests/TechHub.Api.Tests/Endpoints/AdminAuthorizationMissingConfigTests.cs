using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;

namespace TechHub.Api.Tests.Endpoints;

/// <summary>
/// Production without AzureAd:ClientId must deny admin requests with a response, not an unhandled challenge.
/// </summary>
public class AdminAuthorizationMissingConfigTests : IClassFixture<AdminAuthorizationMissingConfigTests.ProductionWithoutAzureAdApiFactory>
{
    private readonly HttpClient _client;

    public AdminAuthorizationMissingConfigTests(ProductionWithoutAzureAdApiFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("/api/admin/feeds")]
    [InlineData("/api/admin/statistics")]
    public async Task AdminRoute_InProductionWithoutAzureAd_ReturnsUnauthorized(string route)
    {
        // Act
        var response = await _client.GetAsync(route, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    public sealed class ProductionWithoutAzureAdApiFactory : TechHubIntegrationTestApiFactory
    {
        protected override void ConfigureTestSpecificServices(IWebHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            base.ConfigureTestSpecificServices(builder);

            builder.UseEnvironment("Production");
            builder.UseSetting("AzureAd:ClientId", string.Empty);
        }
    }
}
