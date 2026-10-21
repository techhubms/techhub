using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Api.Tests;

/// <summary>
/// Integration test factory that replaces the outbound HTTP / AI dependencies with stateless stubs.
/// The stub outcome is selected by a marker embedded in the URL, so tests stay independent of each other
/// and never touch the network.
/// </summary>
public class TechHubStubbedExternalServicesApiFactory : TechHubIntegrationTestApiFactory
{
    // IArticleFetchClient markers
    public const string FetchThrowsMarker = "stub-fetch-throws";
    public const string FetchNullMarker = "stub-fetch-null";
    public const string FetchBasicTitleMarker = "stub-title-basic";
    public const string FetchAttributedTitleMarker = "stub-title-attributed";
    public const string FetchNoTitleMarker = "stub-title-none";

    // IAiCategorizationService markers
    public const string AiThrowsMarker = "stub-ai-throws";
    public const string AiFailureMarker = "stub-ai-failure";
    public const string AiExcludeMarker = "stub-ai-exclude";

    public const string AiTitle = "Stub AI Title";
    public const string AiExcerpt = "Stub AI excerpt";

    protected override void ConfigureTestSpecificServices(IWebHostBuilder builder)
    {
        base.ConfigureTestSpecificServices(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<IArticleFetchClient>(new StubArticleFetchClient());
            services.AddSingleton<IAiCategorizationService>(new StubAiCategorizationService());
        });
    }

    private sealed class StubArticleFetchClient : IArticleFetchClient
    {
        public Task<string?> FetchHtmlAsync(string url, CancellationToken ct = default)
        {
            if (url.Contains(FetchThrowsMarker, StringComparison.Ordinal))
            {
                throw new HttpRequestException("Stubbed fetch failure");
            }

            if (url.Contains(FetchBasicTitleMarker, StringComparison.Ordinal))
            {
                return Task.FromResult<string?>(
                    "<html><head><title>  Hello &amp; Welcome  </title></head><body>Body</body></html>");
            }

            if (url.Contains(FetchAttributedTitleMarker, StringComparison.Ordinal))
            {
                return Task.FromResult<string?>(
                    "<HTML><HEAD><TITLE lang=\"en\">Attributed &lt;Title&gt;</TITLE></HEAD></HTML>");
            }

            if (url.Contains(FetchNoTitleMarker, StringComparison.Ordinal))
            {
                return Task.FromResult<string?>("<html><body>No title element here</body></html>");
            }

            // FetchNullMarker and any unmarked URL: simulate an HTTP failure / timeout (null HTML).
            return Task.FromResult<string?>(null);
        }
    }

    private sealed class StubAiCategorizationService : IAiCategorizationService
    {
        public Task<CategorizationResult> ClassifyAsync(RawFeedItem item, CancellationToken ct = default)
            => CategorizeAsync(item, ct);

        public Task<CategorizationResult> CategorizeAsync(RawFeedItem item, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (item.ExternalUrl.Contains(AiThrowsMarker, StringComparison.Ordinal))
            {
                throw new HttpRequestException("Stubbed AI failure");
            }

            if (item.ExternalUrl.Contains(AiFailureMarker, StringComparison.Ordinal))
            {
                return Task.FromResult(new CategorizationResult
                {
                    Explanation = "Stubbed AI failure result",
                    IsFailure = true
                });
            }

            if (item.ExternalUrl.Contains(AiExcludeMarker, StringComparison.Ordinal))
            {
                return Task.FromResult(new CategorizationResult { Explanation = "Stubbed AI exclusion" });
            }

            var slug = new Uri(item.ExternalUrl).Segments[^1].Trim('/').ToLowerInvariant();

            return Task.FromResult(new CategorizationResult
            {
                Explanation = "Stubbed AI inclusion",
                Item = new ProcessedContentItem
                {
                    Slug = slug,
                    Title = AiTitle,
                    Content = $"Summary based on: {item.FullContent}",
                    Excerpt = AiExcerpt,
                    DateEpoch = item.PublishedAt.ToUnixTimeSeconds(),
                    CollectionName = item.CollectionName,
                    ExternalUrl = item.ExternalUrl,
                    FeedName = item.FeedName,
                    Tags = ["AI"],
                    Sections = ["ai"],
                    PrimarySectionName = "ai",
                    ContentHash = $"stub-hash-{slug}",
                    RoundupMetadata = new RoundupMetadata
                    {
                        SchemaVersion = RoundupMetadata.CurrentSchemaVersion,
                        KeyFacts = ["Stubbed fact."],
                        Significance = "medium",
                        SectionRoles = new Dictionary<string, string> { ["ai"] = RoundupMetadata.RoleLead }
                    },
                    TimelineEvents =
                    [
                        new TimelineEvent
                        {
                            Kind = "ga",
                            Product = "Stub Product",
                            Description = "Stubbed event",
                            Date = new DateOnly(2026, 3, 10),
                            DatePrecision = "day",
                            DateSource = TimelineEvent.DateSourceExplicit
                        }
                    ]
                }
            });
        }
    }
}
