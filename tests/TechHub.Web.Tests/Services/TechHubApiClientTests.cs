using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TechHub.Core.Models;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

public class TechHubApiClientTests
{
    [Fact]
    public async Task GetLatestRoundupAsync_WhenAllRoundupsReturnsNoContent_FallsBackToSectionRoundups()
    {
        // Arrange
        var responses = new Dictionary<string, HttpResponseMessage>(StringComparer.OrdinalIgnoreCase)
        {
            ["/api/sections/all/collections/roundups/items?take=1"] = new HttpResponseMessage(HttpStatusCode.NoContent),
            ["/api/sections"] = CreateJsonResponse(
                new[]
                {
                    new Section("all", "All", "All", "/all", "All",
                    [
                        new Collection("news", "News", "/all/news", "News", "News")
                    ]),
                    new Section("ai", "AI", "AI", "/ai", "AI",
                    [
                        new Collection("roundups", "Roundups", "/ai/roundups", "Roundups", "Roundups")
                    ]),
                    new Section("azure", "Azure", "Azure", "/azure", "Azure",
                    [
                        new Collection("roundups", "Roundups", "/azure/roundups", "Roundups", "Roundups")
                    ])
                }),
            ["/api/sections/ai/collections/roundups/items?take=1"] = CreateJsonResponse(
                new CollectionItemsResponse(
                    [CreateRoundup("weekly-ai-roundup-2025-05-26", "ai", 1748217600)],
                    1)),
            ["/api/sections/azure/collections/roundups/items?take=1"] = CreateJsonResponse(
                new CollectionItemsResponse(
                    [CreateRoundup("weekly-azure-roundup-2025-06-02", "azure", 1748822400)],
                    1))
        };

        using var httpClient = new HttpClient(new StubHandler(responses))
        {
            BaseAddress = new Uri("https://localhost:5003")
        };

        var sut = new TechHubApiClient(httpClient, NullLogger<TechHubApiClient>.Instance);

        // Act
        var latest = await sut.GetLatestRoundupAsync(TestContext.Current.CancellationToken);

        // Assert
        latest.Should().NotBeNull();
        latest!.Slug.Should().Be("weekly-azure-roundup-2025-06-02");
        latest.PrimarySectionName.Should().Be("azure");
    }

    private static ContentItem CreateRoundup(string slug, string sectionName, long dateEpoch) =>
        new(
            slug,
            $"Roundup {sectionName}",
            "TechHub",
            dateEpoch,
            "roundups",
            "Weekly Roundup",
            sectionName,
            ["AI"],
            [sectionName],
            "Roundup excerpt",
            $"/{sectionName}/roundups/{slug}");

    private static HttpResponseMessage CreateJsonResponse<T>(T value) =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent(
                JsonSerializer.Serialize(value),
                Encoding.UTF8,
                "application/json")
        };

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly IReadOnlyDictionary<string, HttpResponseMessage> _responses;

        public StubHandler(IReadOnlyDictionary<string, HttpResponseMessage> responses)
        {
            ArgumentNullException.ThrowIfNull(responses);
            _responses = responses;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var key = request.RequestUri?.PathAndQuery ?? string.Empty;
            if (_responses.TryGetValue(key, out var response))
            {
                return Task.FromResult(response);
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
