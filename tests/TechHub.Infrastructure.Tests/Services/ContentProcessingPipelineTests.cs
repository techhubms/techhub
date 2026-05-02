using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services.ContentProcessing;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Three-tier golden-master tests for the content processing pipeline.
///
/// Each fixture set in Services/Fixtures/Pipeline/ represents one feed's latest article:
///   {name}.rss             — Real RSS/Atom XML trimmed to 1 item (the stable test seed)
///   {name}.html            — Raw article HTML as fetched by the real ArticleFetchClient
///   {name}.transcript.txt  — Placeholder transcript text for YouTube feeds
///   {name}.md              — Extracted markdown after HTML → markdown conversion
///   {name}.json            — AI user prompt (the golden-master for the pipeline output)
///
/// What the test verifies (three tiers):
///   Tier 1 — Content fetch:
///     ArticleFetchClient fetches the article URL from the .rss feed.
///     In normal mode the stored .html is used as the mock response.
///     In generate mode the real HTTP client fetches live content and saves it as .html.
///
///   Tier 2 — HTML → markdown conversion:
///     ArticleContentService runs HtmlToMarkdownConverter on the raw HTML.
///     Result (enrichedItem.FullContent) is compared to the stored .md file.
///
///   Tier 3 — AI prompt construction:
///     AiCategorizationService.BuildUserPrompt produces the user prompt sent to the AI.
///     Result is compared to the UserPrompt field in the stored .json file.
///
/// Regenerating fixtures:
///   Set GENERATE_PIPELINE_FIXTURES=true and run the pipeline tests.
///   The test fetches live article HTML, saves it as .html, runs the full pipeline,
///   and writes .md and .json golden-master files. Rebuild the project afterward
///   so the new files become embedded resources.
///
/// Adding fixtures for a new feed:
///   1. Drop a trimmed {name}.rss into Services/Fixtures/Pipeline/
///   2. Run with GENERATE_PIPELINE_FIXTURES=true — all other files are written automatically
///   3. Rebuild the project, then run tests normally
///
/// Updating RSS fixtures:
///   Manually download the feed, trim to 1 item, and replace the .rss file.
///   The trimmed XML must contain exactly one item/entry.
/// </summary>
public class ContentProcessingPipelineTests
{
    private const string FixtureNamespace = "TechHub.Infrastructure.Tests.Services.Fixtures.Pipeline.";

    // Canonical source-tree path used when writing generated fixture files to disk.
    private static readonly string _fixtureRepoDir = FindRepoFixtureDir();

    // ── Fixture discovery ─────────────────────────────────────────────────────

    /// <summary>
    /// Discovers all pipeline fixture sets by listing embedded .rss resources.
    /// Returns the bare fixture name (e.g. "the-github-blog").
    /// </summary>
    public static IEnumerable<object[]> GetPipelineFixtures()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceNames()
            .Where(n => n.StartsWith(FixtureNamespace, StringComparison.Ordinal)
                     && n.EndsWith(".rss", StringComparison.Ordinal))
            .Select(n => n[FixtureNamespace.Length..][..^".rss".Length])
            .Order(StringComparer.Ordinal)
            .Select(name => new object[] { name });
    }

    // ── Theory ────────────────────────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(GetPipelineFixtures))]
    public async Task Pipeline_WithFixture_PassesAllThreeTiers(string fixtureName)
    {
        var feedXml = LoadEmbeddedResource($"{fixtureName}.rss");
        var expectedFixture = TryLoadExpectedFixture(fixtureName);

        var feedConfig = new FeedConfig
        {
            Name = expectedFixture?.FeedName ?? fixtureName,
            OutputDir = expectedFixture?.CollectionOutputDir ?? "_test",
            Url = $"https://fixture.test/{fixtureName}/feed.rss"
        };

        var cutoff = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var items = RssFeedIngestionService.ParseFeed(feedXml, feedConfig, cutoff);
        items.Should().ContainSingle(
            $"fixture '{fixtureName}.rss' must contain exactly one item");

        var rawItem = items[0];

        if (ShouldGenerate())
        {
            await GenerateAllFixturesAsync(fixtureName, rawItem, feedXml, feedConfig);
            return;
        }

        if (expectedFixture is null)
        {
            Assert.Fail(
                $"Missing golden-master fixture: {fixtureName}.json\n" +
                $"Set GENERATE_PIPELINE_FIXTURES=true and run the tests to generate all fixture files.");
            return;
        }

        // ── Tier 1: Real HTTP fetch vs stored .html ───────────────────────────
        //
        // Makes a live HTTP call and compares the result to the stored .html fixture.
        // Opt-in: set VERIFY_LIVE_HTML=true to enable. Disabled by default because many
        // sites change HTML frequently (timestamps, ads, related posts), which causes
        // unrelated test failures. Use GENERATE_PIPELINE_FIXTURES=true to refresh fixtures.

        if (!rawItem.IsYouTube && ShouldVerifyLiveHtml())
        {
            var storedHtml = LoadEmbeddedResource($"{fixtureName}.html");
            var liveHtml = await FetchLiveHtmlAsync(rawItem.ExternalUrl);

            liveHtml?.Should().Be(storedHtml,
                    $"[Tier 1] Live HTML for '{fixtureName}' has changed since fixtures were generated. " +
                    $"Re-run with GENERATE_PIPELINE_FIXTURES=true to refresh the stored .html fixture.");
        }

        // ── Tiers 2 & 3: Deterministic pipeline on stored .html ──────────────
        //
        // Both tiers use the stored .html via a mock, so results are stable regardless
        // of whether Tier 1 passed. The pipeline runs exactly as in production:
        //   ArticleContentService  → HTML strip + HtmlToMarkdownConverter → FullContent
        //   AiCategorizationService.BuildUserPrompt → the AI user prompt string

        var fetchClient = new Mock<IArticleFetchClient>();
        var ytService = new Mock<IYouTubeTranscriptService>();

        if (rawItem.IsYouTube)
        {
            var transcript = TryLoadEmbeddedText($"{fixtureName}.transcript.txt")
                ?? $"Placeholder transcript for {fixtureName}";
            ytService
                .Setup(s => s.GetTranscriptAsync(rawItem.ExternalUrl, It.IsAny<CancellationToken>()))
                .ReturnsAsync(TranscriptResult.Success(transcript));
        }
        else
        {
            var storedHtml = LoadEmbeddedResource($"{fixtureName}.html");
            fetchClient
                .Setup(c => c.FetchHtmlAsync(rawItem.ExternalUrl, It.IsAny<CancellationToken>()))
                .ReturnsAsync(storedHtml);
        }

        var articleService = new ArticleContentService(fetchClient.Object, ytService.Object);
        var enrichedItem = await articleService.EnrichWithContentAsync(rawItem, CancellationToken.None);
        var actualPrompt = AiCategorizationService.BuildUserPrompt(enrichedItem);

        // ── Tier 2: HTML → markdown ───────────────────────────────────────────
        //
        // ArticleContentService strips page noise and runs HtmlToMarkdownConverter.
        // The result must exactly match the stored .md golden-master.
        // A failure here means the conversion logic changed — update with GENERATE_PIPELINE_FIXTURES.

        if (!rawItem.IsYouTube)
        {
            var expectedMarkdown = TryLoadEmbeddedText($"{fixtureName}.md");
            if (expectedMarkdown is not null)
            {
                enrichedItem.FullContent.Should().Be(expectedMarkdown,
                    $"[Tier 2] HTML→markdown for '{fixtureName}' must match the stored .md fixture. " +
                    $"If the conversion logic changed, re-run with GENERATE_PIPELINE_FIXTURES=true.");
            }
        }

        // ── Tier 3: AI prompt ─────────────────────────────────────────────────
        //
        // AiCategorizationService.BuildUserPrompt assembles the full prompt from the enriched item.
        // The result must exactly match the UserPrompt in the stored .json golden-master.

        actualPrompt.Should().Be(expectedFixture.UserPrompt,
            $"[Tier 3] AI prompt for '{fixtureName}' must match the golden-master {fixtureName}.json");
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string LoadEmbeddedResource(string resourceSuffix)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var name = $"{FixtureNamespace}{resourceSuffix}";

        using var stream = assembly.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException(
                $"Embedded resource '{name}' not found.\n" +
                $"Available: {string.Join(", ", assembly.GetManifestResourceNames())}");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static string? TryLoadEmbeddedText(string resourceSuffix)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var name = $"{FixtureNamespace}{resourceSuffix}";

        using var stream = assembly.GetManifestResourceStream(name);
        if (stream is null)
        {
            return null;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static PipelineFixture? TryLoadExpectedFixture(string fixtureName)
    {
        var json = TryLoadEmbeddedText($"{fixtureName}.json");
        if (json is null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<PipelineFixture>(json, _jsonOptions);
    }

    private static bool ShouldGenerate() =>
        string.Equals(
            Environment.GetEnvironmentVariable("GENERATE_PIPELINE_FIXTURES"),
            "true",
            StringComparison.OrdinalIgnoreCase);

    private static bool ShouldVerifyLiveHtml() =>
        string.Equals(
            Environment.GetEnvironmentVariable("VERIFY_LIVE_HTML"),
            "true",
            StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Runs the full generation pass for a single fixture: fetches live HTML (with
    /// <c>&lt;content:encoded&gt;</c> fallback for bot-blocked sites), saves it as <c>.html</c>,
    /// runs the pipeline, then writes <c>.md</c> and <c>.json</c> golden-master files.
    /// Called only when <c>GENERATE_PIPELINE_FIXTURES=true</c>.
    /// </summary>
    private static async Task GenerateAllFixturesAsync(
        string fixtureName, RawFeedItem rawItem, string _, FeedConfig feedConfig)
    {
        var fetchClient = new Mock<IArticleFetchClient>();
        var ytService = new Mock<IYouTubeTranscriptService>();

        if (rawItem.IsYouTube)
        {
            var transcript =
                $"This is a placeholder transcript for '{feedConfig.Name}' used in pipeline fixture tests. " +
                "The actual transcript would be fetched via YoutubeExplode at runtime.";

            if (Directory.Exists(_fixtureRepoDir))
            {
                File.WriteAllText(
                    Path.Combine(_fixtureRepoDir, $"{fixtureName}.transcript.txt"),
                    transcript, Encoding.UTF8);
            }

            ytService
                .Setup(s => s.GetTranscriptAsync(rawItem.ExternalUrl, It.IsAny<CancellationToken>()))
                .ReturnsAsync(TranscriptResult.Success(transcript));
        }
        else
        {
            // Real HTTP fetch, falling back to content:encoded already parsed into rawItem.EmbeddedHtml.
            var html = await FetchAndSaveHtmlAsync(fixtureName, rawItem);
            if (html is not null)
            {
                fetchClient
                    .Setup(c => c.FetchHtmlAsync(rawItem.ExternalUrl, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(html);
            }
        }

        var articleService = new ArticleContentService(fetchClient.Object, ytService.Object);
        var enrichedItem = await articleService.EnrichWithContentAsync(rawItem, CancellationToken.None);
        var actualPrompt = AiCategorizationService.BuildUserPrompt(enrichedItem);

        WriteGeneratedFixture(fixtureName, feedConfig, actualPrompt, enrichedItem.FullContent);
    }

    /// <summary>
    /// Makes a live HTTP request mirroring production headers (no <c>&lt;content:encoded&gt;</c>
    /// fallback, no disk writes). Used by Tier 1 to compare live content to the stored .html.
    /// Returns <see langword="null"/> when the server rejects the request (e.g. 403 on Medium),
    /// in which case Tier 1 is silently skipped for that feed.
    /// </summary>
    private static async Task<string?> FetchLiveHtmlAsync(string url)
    {
        using var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (compatible; TechHub-ContentProcessor/1.0; +https://tech.hub.ms)");
        httpClient.DefaultRequestHeaders.Accept.ParseAdd(
            "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
        httpClient.Timeout = TimeSpan.FromSeconds(60);

        var client = new ArticleFetchClient(httpClient, NullLogger<ArticleFetchClient>.Instance);
        return await client.FetchHtmlAsync(url, CancellationToken.None);
    }

    /// <summary>
    /// Fetches article HTML via the real <see cref="ArticleFetchClient"/> (mirroring production
    /// headers from Program.cs), falls back to <see cref="RawFeedItem.EmbeddedHtml"/> for
    /// bot-blocked sites (e.g. Medium behind Cloudflare), then saves the raw HTML to the
    /// source-tree fixture directory as <c>{fixtureName}.html</c> for future embedded-resource inclusion.
    /// </summary>
    private static async Task<string?> FetchAndSaveHtmlAsync(string fixtureName, RawFeedItem item)
    {
        // Mirror the ArticleFetchClient HTTP configuration from Program.cs (AllowAutoRedirect=false
        // lets ArticleFetchClient handle HTTPS→HTTP scheme-downgrade redirects explicitly).
        using var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (compatible; TechHub-ContentProcessor/1.0; +https://tech.hub.ms)");
        httpClient.DefaultRequestHeaders.Accept.ParseAdd(
            "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
        httpClient.Timeout = TimeSpan.FromSeconds(60);

        var client = new ArticleFetchClient(httpClient, NullLogger<ArticleFetchClient>.Instance);
        var html = await client.FetchHtmlAsync(item.ExternalUrl, CancellationToken.None);

        // Fall back to content:encoded / Atom <content> already parsed from the feed.
        // Mirrors ArticleContentService production behavior.
        html ??= item.EmbeddedHtml;

        // Persist to the source tree so the file is embedded on the next build.
        if (html is not null && Directory.Exists(_fixtureRepoDir))
        {
            File.WriteAllText(
                Path.Combine(_fixtureRepoDir, $"{fixtureName}.html"),
                html, Encoding.UTF8);
        }

        return html;
    }

    private static void WriteGeneratedFixture(string fixtureName, FeedConfig feedConfig, string actualPrompt, string? markdownContent)
    {
        var fixture = new PipelineFixture
        {
            FeedName = feedConfig.Name,
            CollectionOutputDir = feedConfig.OutputDir,
            UserPrompt = actualPrompt
        };

        var json = JsonSerializer.Serialize(fixture, _jsonOptions);

        // Write to the repo source tree so the file is available to embed on next build.
        if (Directory.Exists(_fixtureRepoDir))
        {
            File.WriteAllText(Path.Combine(_fixtureRepoDir, $"{fixtureName}.json"), json);

            // Also save the extracted markdown so humans can inspect what the pipeline produced.
            if (!string.IsNullOrWhiteSpace(markdownContent))
            {
                File.WriteAllText(Path.Combine(_fixtureRepoDir, $"{fixtureName}.md"), markdownContent);
            }
        }

        // Also write a diagnostic copy to .tmp/ inside the repo.
        if (!string.IsNullOrEmpty(_fixtureRepoDir))
        {
            // _fixtureRepoDir is 5 levels deep inside the repo root.
            var repoRoot = Path.GetFullPath(Path.Combine(_fixtureRepoDir, "..", "..", "..", "..", ".."));
            var tmpDir = Path.Combine(repoRoot, ".tmp", "pipeline-fixtures");
            try
            {
                Directory.CreateDirectory(tmpDir);
                File.WriteAllText(Path.Combine(tmpDir, $"{fixtureName}.json"), json);
            }
            catch (UnauthorizedAccessException) { /* skip tmp copy if no write access */ }
        }
    }

    private static string FindRepoFixtureDir()
    {
        // Walk up from the binary output until we find the repo root (has TechHub.slnx).
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "TechHub.slnx")))
        {
            dir = dir.Parent;
        }

        return dir is not null
            ? Path.Combine(dir.FullName, "tests", "TechHub.Infrastructure.Tests",
                           "Services", "Fixtures", "Pipeline")
            : string.Empty;
    }

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    // ── Fixture data model ────────────────────────────────────────────────────

    private sealed class PipelineFixture
    {
        /// <summary>Feed display name shown as FEED in the AI prompt.</summary>
        public string FeedName { get; set; } = string.Empty;

        /// <summary>Feed output directory (e.g. "_blogs"), determines COLLECTION in the AI prompt.</summary>
        public string CollectionOutputDir { get; set; } = string.Empty;

        /// <summary>Golden-master: the exact user prompt string passed to the AI categorization model.</summary>
        public string UserPrompt { get; set; } = string.Empty;
    }
}
