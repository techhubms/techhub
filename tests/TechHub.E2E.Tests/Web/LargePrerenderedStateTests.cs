using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models;
using TechHub.E2E.Tests.Helpers;
using TechHub.Web.Services;

namespace TechHub.E2E.Tests.Web;

public class LargePrerenderedStateTests : PlaywrightTestBase
{
    public LargePrerenderedStateTests(PlaywrightCollectionFixture fixture) : base(fixture) { }

    [Fact]
    public async Task CircuitStart_WithLargePersistedState_BecomesInteractive()
    {
        // Arrange
        await using var factory = new LargeStateWebFactory();
        factory.UseKestrel(0);
        factory.StartServer();
        var addresses = factory.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();
        var address = addresses!.Addresses.Single();
        var sentFrames = new System.Collections.Concurrent.ConcurrentQueue<string>();
        Page.WebSocket += (_, socket) =>
        {
            socket.FrameSent += (_, frame) =>
            {
                if (frame.Text is not null)
                {
                    sentFrames.Enqueue(frame.Text);
                }
            };
        };

        // Act
        await Page.GotoAndWaitForBlazorAsync(address);

        // Assert
        var startFrame = sentFrames.Single(frame => frame.Contains("StartCircuit", StringComparison.Ordinal));
        var size = Encoding.UTF8.GetByteCount(startFrame);
        size.Should().BeGreaterThan(256 * 1024);
        size.Should().BeLessThan(1024 * 1024);
    }

    private sealed class LargeStateWebFactory : WebApplicationFactory<TechHubApiClient>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.UseSetting("AppSettings:SkipFileLogging", "true");
            builder.UseSetting("AzureAd:ClientId", string.Empty);
            builder.ConfigureTestServices(services =>
            {
                services.AddHttpClient<TechHubApiClient>().ConfigurePrimaryHttpMessageHandler(() => new LargeStateApiHandler());
                services.AddHttpClient<ApiHealthCheck>().ConfigurePrimaryHttpMessageHandler(() => new LargeStateApiHandler());
            });
        }
    }

    private sealed class LargeStateApiHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var path = request.RequestUri?.AbsolutePath;
            if (path == "/api/sections/all/collections/all/items")
            {
                var item = new ContentItem("large-state", new string('x', 180_000), "Test", 1,
                    "roundups", "test", "ai", "", "", "test");
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(new CollectionItemsResponse([item], 1))
                });
            }

            if (path is "/api/sections" or "/api/feature-toggles"
                or "/api/sections/all/collections/all/tags")
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(Array.Empty<object>())
                });
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
