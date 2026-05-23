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
    public async Task GetLatestRoundupPerSectionAsync_ReturnsOneRoundupPerSection_InSectionOrder()
    {
        // Arrange
        var responses = new Dictionary<string, Func<HttpResponseMessage>>(StringComparer.OrdinalIgnoreCase)
        {
            ["/api/sections"] = () => CreateJsonResponse(
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
            ["/api/sections/ai/collections/roundups/items?take=1"] = () => CreateJsonResponse(
                new CollectionItemsResponse(
                    [CreateRoundup("weekly-ai-roundup-2025-05-26", "ai", 1748217600)],
                    1)),
            ["/api/sections/azure/collections/roundups/items?take=1"] = () => CreateJsonResponse(
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
        var roundups = (await sut.GetLatestRoundupPerSectionAsync(TestContext.Current.CancellationToken)).ToList();

        // Assert — one per section, in section order (ai before azure)
        roundups.Should().HaveCount(2);
        roundups[0].PrimarySectionName.Should().Be("ai");
        roundups[1].PrimarySectionName.Should().Be("azure");
    }

    [Fact]
    public async Task GetLatestRoundupPerSectionAsync_SkipsSectionsWithoutRoundupsCollection()
    {
        // Arrange — "dotnet" section has no roundups collection
        var responses = new Dictionary<string, Func<HttpResponseMessage>>(StringComparer.OrdinalIgnoreCase)
        {
            ["/api/sections"] = () => CreateJsonResponse(
                new[]
                {
                    new Section("dotnet", ".NET", ".NET", "/dotnet", ".NET",
                    [
                        new Collection("news", "News", "/dotnet/news", "News", "News")
                    ]),
                    new Section("github-copilot", "GitHub Copilot", "GitHub Copilot", "/github-copilot", "GitHub Copilot",
                    [
                        new Collection("roundups", "Roundups", "/github-copilot/roundups", "Roundups", "Roundups")
                    ])
                }),
            ["/api/sections/github-copilot/collections/roundups/items?take=1"] = () => CreateJsonResponse(
                new CollectionItemsResponse(
                    [CreateRoundup("weekly-github-copilot-roundup-2025-06-09", "github-copilot", 1749427200)],
                    1))
        };

        using var httpClient = new HttpClient(new StubHandler(responses))
        {
            BaseAddress = new Uri("https://localhost:5003")
        };

        var sut = new TechHubApiClient(httpClient, NullLogger<TechHubApiClient>.Instance);

        // Act
        var roundups = (await sut.GetLatestRoundupPerSectionAsync(TestContext.Current.CancellationToken)).ToList();

        // Assert — only github-copilot has a roundup, dotnet is skipped
        roundups.Should().HaveCount(1);
        roundups[0].PrimarySectionName.Should().Be("github-copilot");
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
        private readonly IReadOnlyDictionary<string, Func<HttpResponseMessage>> _responses;

        public StubHandler(IReadOnlyDictionary<string, Func<HttpResponseMessage>> responses)
        {
            ArgumentNullException.ThrowIfNull(responses);
            _responses = responses;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var key = request.RequestUri?.PathAndQuery ?? string.Empty;
            if (_responses.TryGetValue(key, out var factory))
            {
                return Task.FromResult(factory());
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
