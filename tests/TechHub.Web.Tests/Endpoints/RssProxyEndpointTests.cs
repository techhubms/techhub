using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TechHub.Core.Models;
using TechHub.Web.Endpoints;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Endpoints;

public class RssProxyEndpointTests
{
    [Theory]
    [InlineData("/unknown-section/feed.xml")]
    [InlineData("/unknown-section/roundups/feed.xml")]
    public async Task UnknownSection_WhenApiReturnsNotFound_ReturnsNotFound(string path)
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();

        var sectionCache = new SectionCache();
        var apiHandler = new NotFoundHandler();
        builder.Services.AddSingleton(sectionCache);
        builder.Services.AddHttpClient<TechHubApiClient>(client =>
            client.BaseAddress = new Uri("https://localhost"))
            .ConfigurePrimaryHttpMessageHandler(() => apiHandler);

        await using var app = builder.Build();
        app.MapSectionRssFeedEndpoints();
        await app.StartAsync();
        using var client = app.GetTestClient();

        // Act
        using var response = await client.GetAsync(path, TestContext.Current.CancellationToken);

        // Assert
        sectionCache.IsReady.Should().BeFalse();
        apiHandler.RequestCount.Should().Be(1);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private sealed class NotFoundHandler : HttpMessageHandler
    {
        public int RequestCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestCount++;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
