using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Npgsql;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;
using TechHub.Infrastructure.Services.ContentProcessing;
using TechHub.Infrastructure.Services.RoundupGeneration;

namespace TechHub.Infrastructure.Tests.Evaluation;

/// <summary>
/// LOCAL ONLY, never committed (excluded through .git/info/exclude): evaluates the three-model pipeline of issue #581
/// against a restored production database with the real models. Enabled with TECHHUB_EVAL=1.
/// Results are written to .tmp/eval.
/// </summary>
public class ModelEvaluationTests
{
    private static readonly string[] _sectionNames = ["ai", "azure", "dotnet", "devops", "github-copilot", "ml", "security"];

    private const string Endpoint = "https://oai-techhub-prod.openai.azure.com/";

    private static void RequireEnabled() =>
        Assert.SkipWhen(Environment.GetEnvironmentVariable("TECHHUB_EVAL") != "1", "Set TECHHUB_EVAL=1 to run the model evaluation.");

    private static string RepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, "src", "TechHub.Api")))
        {
            dir = dir.Parent;
        }

        return dir?.FullName ?? throw new InvalidOperationException("Repository root not found");
    }

    private static string OutDir(string sub = "")
    {
        if (Path.IsPathRooted(sub))
        {
            throw new ArgumentException("Sub-directory must be a relative path.", nameof(sub));
        }

        var path = Path.Join(RepoRoot(), ".tmp", "eval", sub);
        Directory.CreateDirectory(path);
        return path;
    }

    private static string OutFile(string fileName)
    {
        if (Path.IsPathRooted(fileName))
        {
            throw new ArgumentException("File name must be relative.", nameof(fileName));
        }

        return Path.Join(OutDir(), fileName);
    }

    private static NpgsqlConnection OpenLocalDb()
    {
        using var doc = JsonDocument.Parse(File.ReadAllText(Path.Join(RepoRoot(), "src", "TechHub.Api", "appsettings.Development.json")));
        var connectionString = doc.RootElement.GetProperty("Database").GetProperty("ConnectionString").GetString()!;
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }

    private static AiProfilesOptions Profiles(string rewriteEffort = "low", AiProfileOptions? classification = null) => new()
    {
        Classification = classification ?? new AiProfileOptions { DeploymentName = "gpt-6.1-sol", ReasoningEffort = "low" },
        Rewrite = new AiProfileOptions { DeploymentName = "gpt-6.1-sol", ReasoningEffort = rewriteEffort },
        RoundupNewsWriter = new AiProfileOptions { DeploymentName = "gpt-6-astra", ReasoningEffort = "medium" },
        RoundupMetadata = new AiProfileOptions { DeploymentName = "gpt-6.1-sol", ReasoningEffort = "low" },
        TimelineExtraction = new AiProfileOptions { DeploymentName = "gpt-6-luna", ReasoningEffort = "low" }
    };

    private static AiCategorizationOptions Options(string rewriteEffort = "low", AiProfileOptions? classification = null) => new()
    {
        Endpoint = Endpoint,
        MaxRetries = 3,
        RateLimitDelaySeconds = 5,
        TimelineBackfillDelaySeconds = 0,
        Profiles = Profiles(rewriteEffort, classification)
    };

    private static IOptions<AppSettings> LoadAppSettings()
    {
        using var settingsDoc = JsonDocument.Parse(File.ReadAllText(Path.Join(RepoRoot(), "src", "TechHub.Api", "appsettings.json")));
        var contentSettings = JsonSerializer.Deserialize<ContentSettings>(
            settingsDoc.RootElement.GetProperty("AppSettings").GetProperty("Content").GetRawText(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        return Microsoft.Extensions.Options.Options.Create(new AppSettings { Content = contentSettings, BaseUrl = "https://localhost:5003" });
    }

    private static (AiCategorizationService Service, UsageRecorder Recorder) CreateAi(string rewriteEffort = "low", UsageRecorder? shared = null, AiProfileOptions? classification = null)
    {
        var options = Options(rewriteEffort, classification);
        var inner = new AiCompletionClient(
            new HttpClient { Timeout = TimeSpan.FromMinutes(15) },
            Microsoft.Extensions.Options.Options.Create(options),
            NullLogger<AiCompletionClient>.Instance);
        var recorder = shared ?? new UsageRecorder();
        recorder.Inner = inner;
        var service = new AiCategorizationService(recorder, Microsoft.Extensions.Options.Options.Create(options), LoadAppSettings(), NullLogger<AiCategorizationService>.Instance);
        return (service, recorder);
    }

    private static ArticleContentService CreateFetcher()
    {
        var handler = new HttpClientHandler { AllowAutoRedirect = false };
        var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(40) };
        http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");
        http.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        return new ArticleContentService(new ArticleFetchClient(http, NullLogger<ArticleFetchClient>.Instance));
    }

    private sealed record StoredItem(
        string CollectionName, string Slug, string Title, string Content, string ExternalUrl, string FeedName,
        long DateEpoch, string PrimarySectionName, int SectionsBitmask, string TagsCsv)
    {
        public IReadOnlySet<string> Sections => _sectionNames.Where((_, i) => (SectionsBitmask & (1 << i)) != 0).ToHashSet(StringComparer.Ordinal);

        public IReadOnlySet<string> Tags => TagsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(t => t.ToLowerInvariant()).ToHashSet(StringComparer.Ordinal);

        public RawFeedItem ToSourceRaw() => new()
        {
            Title = Title,
            ExternalUrl = ExternalUrl,
            PublishedAt = DateTimeOffset.FromUnixTimeSeconds(DateEpoch),
            FeedName = FeedName,
            CollectionName = CollectionName
        };

        public RawFeedItem ToRaw() => new()
        {
            Title = Title,
            ExternalUrl = ExternalUrl,
            PublishedAt = DateTimeOffset.FromUnixTimeSeconds(DateEpoch),
            FeedName = FeedName,
            CollectionName = CollectionName,
            FullContent = Content
        };
    }

    private static async Task<List<StoredItem>> SampleAsync(IDbConnection db, string collection, int count, int sinceDays = 120, string? extraWhere = null)
    {
        var rows = await db.QueryAsync<StoredItem>($"""
            SELECT collection_name AS CollectionName, slug AS Slug, title AS Title, content AS Content,
                   external_url AS ExternalUrl, COALESCE(feed_name, 'Feed') AS FeedName, date_epoch AS DateEpoch,
                   primary_section_name AS PrimarySectionName, sections_bitmask AS SectionsBitmask, COALESCE(tags_csv, '') AS TagsCsv
            FROM content_items
            WHERE collection_name = @Collection
              AND date_epoch > EXTRACT(EPOCH FROM NOW() - make_interval(days => @Days))
              AND length(content) > 400
              {extraWhere}
            ORDER BY random()
            LIMIT @Count
            """, new { Collection = collection, Days = sinceDays, Count = count });
        return rows.ToList();
    }

    private static double Jaccard(IReadOnlySet<string> a, IReadOnlySet<string> b)
    {
        if (a.Count == 0 && b.Count == 0)
        {
            return 1;
        }

        return (double)a.Intersect(b).Count() / a.Union(b).Count();
    }

    private static int Words(string text) => Regex.Matches(text, @"\S+").Count;

    // ── Stage 1: classification ──────────────────────────────────────────────

    [Fact]
    public async Task ModelEvaluation_Classification()
    {
        RequireEnabled();
        var ct = TestContext.Current.CancellationToken;
        using var db = OpenLocalDb();
        var (ai, recorder) = CreateAi();
        var report = new StringBuilder();

        var included = new List<StoredItem>();
        included.AddRange(await SampleAsync(db, "news", 40));
        included.AddRange(await SampleAsync(db, "blogs", 25));
        included.AddRange(await SampleAsync(db, "community", 20));
        included.AddRange(await SampleAsync(db, "videos", 15));

        var rows = new List<(StoredItem Item, CategorizationResult Result)>();
        foreach (var item in included)
        {
            var result = await ai.ClassifyAsync(item.ToRaw(), ct);
            rows.Add((item, result));
        }

        var ok = rows.Where(r => !r.Result.IsFailure).ToList();
        var failures = rows.Count - ok.Count;
        var agreeIncluded = ok.Count(r => r.Result.Item is not null);
        var primaryMatch = ok.Count(r => r.Result.Item is { } i && i.PrimarySectionName == r.Item.PrimarySectionName);
        var exactSections = ok.Count(r => r.Result.Item is { } i && i.Sections.ToHashSet(StringComparer.Ordinal).SetEquals(r.Item.Sections));
        var meanSectionJaccard = ok.Where(r => r.Result.Item is not null).Select(r => Jaccard(r.Result.Item!.Sections.ToHashSet(StringComparer.Ordinal), r.Item.Sections)).DefaultIfEmpty().Average();
        var meanTagJaccard = ok.Where(r => r.Result.Item is not null).Select(r => Jaccard(r.Result.Item!.Tags.Select(t => t.ToLowerInvariant()).ToHashSet(StringComparer.Ordinal), r.Item.Tags)).DefaultIfEmpty().Average();

        report.AppendLine("# Classification evaluation (gpt-6.1-sol, reasoning low)");
        report.AppendLine();
        report.AppendLine("## Included articles (production said: included)");
        report.AppendLine();
        report.AppendLine(CultureInfo.InvariantCulture, $"- Sample: {rows.Count} articles ({failures} failed calls)");
        report.AppendLine(CultureInfo.InvariantCulture, $"- Inclusion agreement: {agreeIncluded}/{ok.Count} = {(ok.Count == 0 ? 0 : 100.0 * agreeIncluded / ok.Count):F1}%");
        report.AppendLine(CultureInfo.InvariantCulture, $"- Primary section match: {primaryMatch}/{agreeIncluded}");
        report.AppendLine(CultureInfo.InvariantCulture, $"- Exact section set match: {exactSections}/{agreeIncluded}");
        report.AppendLine(CultureInfo.InvariantCulture, $"- Mean section Jaccard: {meanSectionJaccard:F2}");
        report.AppendLine(CultureInfo.InvariantCulture, $"- Mean tag Jaccard: {meanTagJaccard:F2}");

        var meta = ok.Select(r => r.Result.Item?.RoundupMetadata).Where(m => m is not null).Cast<RoundupMetadata>().ToList();
        if (meta.Count > 0)
        {
            report.AppendLine(CultureInfo.InvariantCulture, $"- Significance: high {meta.Count(m => m.Significance == "high")}, medium {meta.Count(m => m.Significance == "medium")}, low {meta.Count(m => m.Significance == "low")}");
            var multiSection = meta.Where(m => m.SectionRoles.Count > 1).ToList();
            var briefRoles = multiSection.SelectMany(m => m.SectionRoles.Values).Count(v => v == "brief");
            var totalRoles = multiSection.SelectMany(m => m.SectionRoles.Values).Count();
            report.AppendLine(CultureInfo.InvariantCulture, $"- Multi-section articles: {multiSection.Count}; roles brief {briefRoles}/{totalRoles}");
            report.AppendLine(CultureInfo.InvariantCulture, $"- Mean key facts: {meta.Average(m => m.KeyFacts.Count):F1}; mean products: {meta.Average(m => m.Products.Count):F1}");
            var events = ok.Sum(r => r.Result.Item?.TimelineEvents.Count ?? 0);
            report.AppendLine(CultureInfo.InvariantCulture, $"- Timeline events: {events} from {ok.Count(r => (r.Result.Item?.TimelineEvents.Count ?? 0) > 0)} articles");
        }

        report.AppendLine();
        report.AppendLine("### Disagreements (included articles)");
        foreach (var (item, result) in rows.Where(r => r.Result.IsFailure || r.Result.Item is null || !r.Result.Item.Sections.ToHashSet(StringComparer.Ordinal).SetEquals(r.Item.Sections) || r.Result.Item.PrimarySectionName != r.Item.PrimarySectionName))
        {
            report.AppendLine();
            report.AppendLine(CultureInfo.InvariantCulture, $"- **{item.Title}** ({item.CollectionName}) {item.ExternalUrl}");
            report.AppendLine(CultureInfo.InvariantCulture, $"  - production: primary `{item.PrimarySectionName}`, sections `{string.Join(",", item.Sections.Order())}`");
            report.AppendLine(result.IsFailure
                ? $"  - new: FAILED: {result.Explanation}"
                : result.Item is null
                    ? $"  - new: EXCLUDED: {result.Explanation}"
                    : $"  - new: primary `{result.Item.PrimarySectionName}`, sections `{string.Join(",", result.Item.Sections.Order())}` — {result.Explanation}");
        }

        // Skipped URLs: re-fetch the original article and classify.
        var skipped = (await db.QueryAsync<(string Url, string? Reason, string? Feed, string? Collection)>("""
            SELECT external_url, reason, feed_name, collection_name
            FROM processed_urls
            WHERE status = 'skipped' AND reason IS NOT NULL AND processed_at > NOW() - INTERVAL '90 days'
              AND external_url NOT ILIKE '%youtube.com%' AND external_url NOT ILIKE '%youtu.be%'
            ORDER BY random() LIMIT 50
            """)).ToList();

        // Borderline promotional articles that production skipped; they must stay excluded.
        var pinnedUrls = new[]
        {
            "https://techcommunity.microsoft.com/t5/tools/the-22-identity-settings-i-check-on-every-entra-tenant-and-the/m-p/4562057#M210",
            "https://techcommunity.microsoft.com/t5/microsoft-developer-community/introducing-microsoft-iq-live-a-new-biweekly-series-for/ba-p/4543480"
        };
        skipped.AddRange(pinnedUrls.Where(u => skipped.All(s => s.Url != u)).Select(u => (u, (string?)"pinned", (string?)null, (string?)null)));

        var fetcher = CreateFetcher();
        var fetchFailed = 0;
        var agreeExcluded = 0;
        var disagreeSkipped = new List<string>();
        foreach (var (url, reason, feed, collection) in skipped)
        {
            var raw = await fetcher.EnrichWithContentAsync(new RawFeedItem
            {
                Title = url,
                ExternalUrl = url,
                PublishedAt = DateTimeOffset.UtcNow,
                FeedName = feed ?? "Feed",
                CollectionName = collection ?? "news"
            }, ct);

            if (string.IsNullOrWhiteSpace(raw.FullContent))
            {
                fetchFailed++;
                continue;
            }

            var result = await ai.ClassifyAsync(raw, ct);
            if (result.IsFailure)
            {
                disagreeSkipped.Add($"- {url}\n  - production: excluded — {reason}\n  - new: FAILED — {result.Explanation}");
            }
            else if (result.Item is null)
            {
                agreeExcluded++;
            }
            else
            {
                disagreeSkipped.Add($"- {url}\n  - production: excluded — {reason}\n  - new: INCLUDED in `{string.Join(",", result.Item.Sections)}` — {result.Explanation}");
            }
        }

        var compared = skipped.Count - fetchFailed;
        report.AppendLine();
        report.AppendLine("## Skipped URLs (production said: excluded; article re-fetched)");
        report.AppendLine();
        report.AppendLine(CultureInfo.InvariantCulture, $"- Sample: {skipped.Count} URLs, {fetchFailed} could not be fetched");
        report.AppendLine(CultureInfo.InvariantCulture, $"- Exclusion agreement: {agreeExcluded}/{compared} = {(compared == 0 ? 0 : 100.0 * agreeExcluded / compared):F1}%");
        report.AppendLine();
        report.AppendLine("### Disagreements (skipped URLs)");
        report.AppendLine();
        foreach (var line in disagreeSkipped)
        {
            report.AppendLine(line);
        }

        report.AppendLine();
        report.AppendLine(recorder.Summary());
        File.WriteAllText(OutFile("classification.md"), report.ToString());
        Console.WriteLine(report.ToString());
        rows.Should().NotBeEmpty();
    }

    // ── Stage 1b: control run on a frozen sample ─────────────────────────────

    private sealed record Verdict(bool Failed, bool Included, string Primary, IReadOnlySet<string> Sections);

    private static Verdict ToVerdict(CategorizationResult result) =>
        result.IsFailure || result.Item is null
            ? new Verdict(result.IsFailure, false, "", new HashSet<string>())
            : new Verdict(false, true, result.Item.PrimarySectionName, result.Item.Sections.ToHashSet(StringComparer.Ordinal));

    private static string Compare(string label, IReadOnlyList<StoredItem> items, IReadOnlyList<Verdict> a, IReadOnlyList<Verdict> b)
    {
        var pairs = Enumerable.Range(0, items.Count).Where(i => !a[i].Failed && !b[i].Failed).ToList();
        var sameInclusion = pairs.Count(i => a[i].Included == b[i].Included);
        var both = pairs.Where(i => a[i].Included && b[i].Included).ToList();
        var samePrimary = both.Count(i => a[i].Primary == b[i].Primary);
        var sameSections = both.Count(i => a[i].Sections.SetEquals(b[i].Sections));
        return $"| {label} | {pairs.Count} | {sameInclusion}/{pairs.Count} | {samePrimary}/{both.Count} | {sameSections}/{both.Count} |";
    }

    /// <summary>
    /// Classifies a frozen sample (saved in .tmp/eval/golden-classification.json) with gpt-6-luna twice and with gpt-5.2
    /// using the same new prompt. Differences between the two luna runs measure the noise of the model itself, so only
    /// differences larger than that can be attributed to a model change.
    /// </summary>
    [Fact]
    public async Task ModelEvaluation_ClassificationControl()
    {
        RequireEnabled();
        var ct = TestContext.Current.CancellationToken;
        using var db = OpenLocalDb();

        var goldenPath = OutFile("golden-classification.json");
        List<StoredItem> items;
        if (File.Exists(goldenPath))
        {
            var urls = JsonSerializer.Deserialize<string[]>(File.ReadAllText(goldenPath))!;
            var found = (await db.QueryAsync<StoredItem>("""
                SELECT collection_name AS CollectionName, slug AS Slug, title AS Title, content AS Content,
                       external_url AS ExternalUrl, COALESCE(feed_name, 'Feed') AS FeedName, date_epoch AS DateEpoch,
                       primary_section_name AS PrimarySectionName, sections_bitmask AS SectionsBitmask, COALESCE(tags_csv, '') AS TagsCsv
                FROM content_items WHERE external_url = ANY(@Urls)
                """, new { Urls = urls })).ToDictionary(i => i.ExternalUrl, StringComparer.Ordinal);
            items = urls.Where(found.ContainsKey).Select(u => found[u]).ToList();
        }
        else
        {
            items = [];
            items.AddRange(await SampleAsync(db, "news", 40));
            items.AddRange(await SampleAsync(db, "blogs", 25));
            items.AddRange(await SampleAsync(db, "community", 20));
            items.AddRange(await SampleAsync(db, "videos", 15));
            File.WriteAllText(goldenPath, JsonSerializer.Serialize(items.Select(i => i.ExternalUrl)));
        }

        var runs = new Dictionary<string, List<Verdict>>();
        var recorders = new Dictionary<string, UsageRecorder>();
        var configs = new (string Name, AiProfileOptions? Profile)[]
        {
            ("luna-run-1", new AiProfileOptions { DeploymentName = "gpt-6-luna", ReasoningEffort = "low" }),
            ("luna-run-2", new AiProfileOptions { DeploymentName = "gpt-6-luna", ReasoningEffort = "low" }),
            ("gpt-5.2", new AiProfileOptions { DeploymentName = "gpt-5.2" })
        };

        foreach (var (name, profile) in configs)
        {
            var (ai, recorder) = CreateAi(classification: profile);
            var verdicts = new List<Verdict>();
            foreach (var item in items)
            {
                verdicts.Add(ToVerdict(await ai.ClassifyAsync(item.ToRaw(), ct)));
            }

            runs[name] = verdicts;
            recorders[name] = recorder;
        }

        var production = items.Select(i => new Verdict(false, true, i.PrimarySectionName, i.Sections)).ToList();

        var report = new StringBuilder();
        report.AppendLine("# Classification control run (frozen sample)");
        report.AppendLine();
        report.AppendLine(CultureInfo.InvariantCulture, $"Sample: {items.Count} articles that production included, frozen in `.tmp/eval/golden-classification.json`.");
        report.AppendLine("All runs use the new classify prompt. Production = the stored result made by the old prompt and gpt-5.2.");
        report.AppendLine();
        report.AppendLine("| Compared | Compared articles | Same include/exclude | Same primary section | Same section set |");
        report.AppendLine("|---|---|---|---|---|");
        report.AppendLine(Compare("luna run 1 vs luna run 2 (noise)", items, runs["luna-run-1"], runs["luna-run-2"]));
        report.AppendLine(Compare("luna run 1 vs production", items, runs["luna-run-1"], production));
        report.AppendLine(Compare("luna run 2 vs production", items, runs["luna-run-2"], production));
        report.AppendLine(Compare("gpt-5.2 (new prompt) vs production", items, runs["gpt-5.2"], production));
        report.AppendLine(Compare("luna run 1 vs gpt-5.2 (new prompt)", items, runs["luna-run-1"], runs["gpt-5.2"]));
        report.AppendLine();
        report.AppendLine("How to read this: if `gpt-5.2 (new prompt) vs production` is about as low as `luna vs production`, the difference comes from the new prompt, not from the model. If luna vs luna is far below 100%, part of every difference is plain randomness.");
        foreach (var (name, recorder) in recorders)
        {
            report.AppendLine();
            report.AppendLine(CultureInfo.InvariantCulture, $"## Usage {name}");
            report.AppendLine(recorder.Summary());
        }

        File.WriteAllText(OutFile("classification-control.md"), report.ToString());
        Console.WriteLine(report.ToString());
        items.Should().NotBeEmpty();
    }

    private sealed record SkippedSample(string Url, string? Reason, string? Feed, string? Collection, string Content);

    /// <summary>
    /// Re-classifies a frozen set of articles that production skipped, using several models and reasoning levels with the
    /// same prompt. It answers whether over-inclusion comes from the prompt or from the model. The fetched article text is
    /// cached in .tmp/eval/golden-skipped.json (delete the file to draw a new sample).
    /// </summary>
    [Fact]
    public async Task ModelEvaluation_SkippedControl()
    {
        RequireEnabled();
        var ct = TestContext.Current.CancellationToken;
        using var db = OpenLocalDb();

        var goldenPath = OutFile("golden-skipped.json");
        List<SkippedSample> samples;
        if (File.Exists(goldenPath))
        {
            samples = JsonSerializer.Deserialize<List<SkippedSample>>(File.ReadAllText(goldenPath))!;
        }
        else
        {
            var skipped = (await db.QueryAsync<(string Url, string? Reason, string? Feed, string? Collection)>("""
                SELECT external_url, reason, feed_name, collection_name
                FROM processed_urls
                WHERE status = 'skipped' AND reason IS NOT NULL AND processed_at > NOW() - INTERVAL '90 days'
                  AND external_url NOT ILIKE '%youtube.com%' AND external_url NOT ILIKE '%youtu.be%'
                ORDER BY random() LIMIT 80
                """)).ToList();
            var fetcher = CreateFetcher();
            samples = [];
            foreach (var (url, reason, feed, collection) in skipped)
            {
                var raw = await fetcher.EnrichWithContentAsync(new RawFeedItem
                {
                    Title = url,
                    ExternalUrl = url,
                    PublishedAt = DateTimeOffset.UtcNow,
                    FeedName = feed ?? "Feed",
                    CollectionName = collection ?? "news"
                }, ct);
                if (!string.IsNullOrWhiteSpace(raw.FullContent))
                {
                    samples.Add(new SkippedSample(url, reason, feed, collection, raw.FullContent));
                }
            }

            File.WriteAllText(goldenPath, JsonSerializer.Serialize(samples));
        }

        var configs = new (string Name, AiProfileOptions Profile)[]
        {
            ("luna-low", new AiProfileOptions { DeploymentName = "gpt-6-luna", ReasoningEffort = "low" }),
            ("luna-medium", new AiProfileOptions { DeploymentName = "gpt-6-luna", ReasoningEffort = "medium" }),
            ("sol-low", new AiProfileOptions { DeploymentName = "gpt-6.1-sol", ReasoningEffort = "low" }),
            ("gpt-5.2", new AiProfileOptions { DeploymentName = "gpt-5.2" })
        };

        var included = new Dictionary<string, bool[]>();
        var usage = new StringBuilder();
        foreach (var (name, profile) in configs)
        {
            var (ai, recorder) = CreateAi(classification: profile);
            var flags = new bool[samples.Count];
            for (var i = 0; i < samples.Count; i++)
            {
                var s = samples[i];
                var result = await ai.ClassifyAsync(new RawFeedItem
                {
                    Title = s.Url,
                    ExternalUrl = s.Url,
                    PublishedAt = DateTimeOffset.UtcNow,
                    FeedName = s.Feed ?? "Feed",
                    CollectionName = s.Collection ?? "news",
                    FullContent = s.Content
                }, ct);
                flags[i] = !result.IsFailure && result.Item is not null;
            }

            included[name] = flags;
            usage.AppendLine(CultureInfo.InvariantCulture, $"## Usage {name}");
            usage.AppendLine(recorder.Summary());
        }

        var report = new StringBuilder();
        report.AppendLine("# Skipped-URL control (frozen sample)");
        report.AppendLine();
        report.AppendLine(CultureInfo.InvariantCulture, $"{samples.Count} URLs that production skipped, article text frozen in `.tmp/eval/golden-skipped.json`. Same prompt for every configuration.");
        report.AppendLine();
        report.AppendLine("| Configuration | Stays excluded | Would be included |");
        report.AppendLine("|---|---|---|");
        foreach (var (name, _) in configs)
        {
            var n = included[name].Count(x => x);
            report.AppendLine(CultureInfo.InvariantCulture, $"| {name} | {samples.Count - n}/{samples.Count} = {100.0 * (samples.Count - n) / samples.Count:F1}% | {n} |");
        }

        report.AppendLine();
        report.AppendLine("## URLs included by at least one configuration");
        report.AppendLine();
        report.AppendLine("| URL | Production reason | " + string.Join(" | ", configs.Select(c => c.Name)) + " |");
        report.AppendLine("|---|---|" + string.Join("|", configs.Select(_ => "---")) + "|");
        for (var i = 0; i < samples.Count; i++)
        {
            if (configs.Any(c => included[c.Name][i]))
            {
                var reason = (samples[i].Reason ?? "").Replace("|", "/", StringComparison.Ordinal);
                reason = reason.Length > 140 ? reason[..140] + "…" : reason;
                report.AppendLine($"| {samples[i].Url} | {reason} | " + string.Join(" | ", configs.Select(c => included[c.Name][i] ? "INCLUDED" : "excluded")) + " |");
            }
        }

        report.AppendLine();
        report.Append(usage);
        File.WriteAllText(OutFile("skipped-control.md"), report.ToString());
        Console.WriteLine(report.ToString());
        samples.Should().NotBeEmpty();
    }

    // ── Stage 2: rewrite ─────────────────────────────────────────────────────

    [Fact]
    public async Task ModelEvaluation_Rewrite()
    {
        RequireEnabled();
        var ct = TestContext.Current.CancellationToken;
        using var db = OpenLocalDb();
        var fetcher = CreateFetcher();
        var recorderLow = new UsageRecorder();
        var recorderMedium = new UsageRecorder();
        var (low, _) = CreateAi("low", recorderLow);
        var (medium, _) = CreateAi("medium", recorderMedium);

        var candidates = new List<StoredItem>();
        candidates.AddRange(await SampleAsync(db, "blogs", 10));
        candidates.AddRange(await SampleAsync(db, "news", 10));
        var report = new StringBuilder();
        report.AppendLine("# Rewrite evaluation (gpt-6.1-sol: low vs medium)");
        report.AppendLine();
        report.AppendLine("| Article | stored words | low words | low s | medium words | medium s |");
        report.AppendLine("|---|---|---|---|---|---|");

        var done = 0;
        var dir = OutDir("rewrite");
        foreach (var stored in candidates)
        {
            if (done >= 12)
            {
                break;
            }

            var source = await fetcher.EnrichWithContentAsync(stored.ToSourceRaw(), ct);
            if (string.IsNullOrWhiteSpace(source.FullContent))
            {
                continue;
            }

            var watchLow = Stopwatch.StartNew();
            var lowResult = await low.CategorizeAsync(source, ct);
            watchLow.Stop();
            var watchMedium = Stopwatch.StartNew();
            var mediumResult = await medium.CategorizeAsync(source, ct);
            watchMedium.Stop();
            if (lowResult.Item is null || mediumResult.Item is null)
            {
                continue;
            }

            done++;
            File.WriteAllText(Path.Combine(dir, Path.GetFileName($"{done:00}-{stored.Slug}.current.md")), $"# {stored.Title}\n\n{stored.Content}");
            File.WriteAllText(Path.Combine(dir, Path.GetFileName($"{done:00}-{stored.Slug}.low.md")), $"# {lowResult.Item.Title}\n\n> {lowResult.Item.Excerpt}\n\n{lowResult.Item.Content}");
            File.WriteAllText(Path.Combine(dir, Path.GetFileName($"{done:00}-{stored.Slug}.medium.md")), $"# {mediumResult.Item.Title}\n\n> {mediumResult.Item.Excerpt}\n\n{mediumResult.Item.Content}");
            report.AppendLine(CultureInfo.InvariantCulture,
                $"| {stored.Title} | {Words(stored.Content)} | {Words(lowResult.Item.Content)} | {watchLow.Elapsed.TotalSeconds:F0} | {Words(mediumResult.Item.Content)} | {watchMedium.Elapsed.TotalSeconds:F0} |");
        }

        report.AppendLine();
        report.AppendLine("## Usage, reasoning low");
        report.AppendLine(recorderLow.Summary());
        report.AppendLine();
        report.AppendLine("## Usage, reasoning medium");
        report.AppendLine(recorderMedium.Summary());
        File.WriteAllText(OutFile("rewrite.md"), report.ToString());
        Console.WriteLine(report.ToString());
        done.Should().BeGreaterThan(0);
    }

    // ── Stage 3: timeline ────────────────────────────────────────────────────

    [Fact]
    public async Task ModelEvaluation_Timeline()
    {
        RequireEnabled();
        var ct = TestContext.Current.CancellationToken;
        using var db = OpenLocalDb();
        var (ai, recorder) = CreateAi();

        var items = new List<StoredItem>();
        items.AddRange(await SampleAsync(db, "news", 25, 180, "AND (sections_bitmask & 16) > 0"));
        items.AddRange(await SampleAsync(db, "news", 15, 180, "AND (sections_bitmask & 16) = 0"));
        items.AddRange(await SampleAsync(db, "blogs", 10, 180));

        // Articles that once produced wrong events: bundled release notes, a retrospective and a two-product announcement.
        var pinned = await db.QueryAsync<StoredItem>("""
            SELECT collection_name AS CollectionName, slug AS Slug, title AS Title, content AS Content,
                   external_url AS ExternalUrl, COALESCE(feed_name, 'Feed') AS FeedName, date_epoch AS DateEpoch,
                   primary_section_name AS PrimarySectionName, sections_bitmask AS SectionsBitmask, COALESCE(tags_csv, '') AS TagsCsv
            FROM content_items
            WHERE title ILIKE 'GitHub Copilot in Visual Studio Code, April releases'
               OR title ILIKE 'What%s new across Microsoft SQL in 2026 so far%'
               OR title ILIKE 'OpenAI%s GPT-6 Sol and GPT-6 Luna now available'
            """);
        items.AddRange(pinned.Where(p => items.All(i => i.Slug != p.Slug)));

        var report = new StringBuilder();
        report.AppendLine("# Timeline extraction evaluation (gpt-6-luna)");
        report.AppendLine();
        var withEvents = 0;
        var total = 0;
        foreach (var item in items)
        {
            var result = await ai.ExtractTimelineEventsAsync(item.ToRaw(), ct);
            if (result.IsFailure)
            {
                report.AppendLine(CultureInfo.InvariantCulture, $"- FAILED **{item.Title}**: {result.Explanation}");
                continue;
            }

            if (result.Events.Count > 0)
            {
                withEvents++;
            }

            total += result.Events.Count;
            report.AppendLine();
            report.AppendLine(CultureInfo.InvariantCulture, $"### {item.Title}");
            report.AppendLine(CultureInfo.InvariantCulture, $"{item.ExternalUrl} (published {DateTimeOffset.FromUnixTimeSeconds(item.DateEpoch):yyyy-MM-dd})");
            if (result.Events.Count == 0)
            {
                report.AppendLine("- no events");
            }

            foreach (var e in result.Events)
            {
                report.AppendLine(CultureInfo.InvariantCulture,
                    $"- `{e.Kind}` {e.Product}{(e.Feature is null ? "" : " / " + e.Feature)}: {e.Description} — {e.Date:yyyy-MM-dd} ({e.DatePrecision}, {e.DateSource}) — \"{e.Quote}\"");
            }
        }

        // The restore script copies only the content tables, so ghc_features may not exist locally.
        var features = new List<(string Title, long? ReleaseDate)>();
        try
        {
            features.AddRange(await db.QueryAsync<(string Title, long? ReleaseDate)>("SELECT title, release_date FROM ghc_features WHERE release_date IS NOT NULL ORDER BY release_date DESC LIMIT 25"));
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedTable)
        {
            // Not restored: the comparison with the curated features is then skipped.
        }
        var header = new StringBuilder();
        header.AppendLine(CultureInfo.InvariantCulture, $"Scanned {items.Count} articles: {withEvents} with events, {total} events in total.");
        header.AppendLine();
        header.AppendLine(features.Count == 0 ? "ghc_features was not restored locally, so there is no comparison with the curated features." : "Newest curated GitHub Copilot features (ghc_features), to compare with the events below:");
        foreach (var (title, release) in features)
        {
            header.AppendLine(CultureInfo.InvariantCulture, $"- {DateTimeOffset.FromUnixTimeSeconds(release!.Value):yyyy-MM-dd} {title}");
        }

        header.AppendLine();
        header.AppendLine(recorder.Summary());
        var text = report.ToString().Insert(report.ToString().IndexOf('\n', StringComparison.Ordinal) + 1, "\n" + header);
        File.WriteAllText(OutFile("timeline.md"), text);
        Console.WriteLine(header.ToString());
        items.Should().NotBeEmpty();
    }

    // ── Stage 4: roundups ────────────────────────────────────────────────────

    [Fact]
    public async Task ModelEvaluation_Roundups()
    {
        RequireEnabled();
        var ct = TestContext.Current.CancellationToken;
        using var db = OpenLocalDb();

        var brussels = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");
        var latestEpoch = await db.ExecuteScalarAsync<long>("SELECT max(date_epoch) FROM content_items WHERE collection_name = 'roundups'");
        var publishDate = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(DateTimeOffset.FromUnixTimeSeconds(latestEpoch), brussels).DateTime);
        var weekStart = publishDate.AddDays(-7);
        var weekEnd = publishDate.AddDays(-1);
        var suffix = publishDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        // Keep the production roundups of that week for the comparison, then remove them locally so they are regenerated.
        var production = (await db.QueryAsync<(string Slug, string Section, string Title, string Content)>(
            "SELECT slug, primary_section_name, title, content FROM content_items WHERE collection_name = 'roundups' AND slug LIKE @Pattern",
            new { Pattern = $"%-roundup-{suffix}" })).ToList();
        production.Should().NotBeEmpty("the restored database must contain the roundups of the evaluated week");
        var prodDir = OutDir("roundups/production");
        foreach (var (slug, _, title, content) in production)
        {
            File.WriteAllText(Path.Combine(prodDir, slug + ".md"), $"# {title}\n\n{content}");
        }

        await db.ExecuteAsync("DELETE FROM content_items WHERE collection_name = 'roundups' AND slug LIKE @Pattern", new { Pattern = $"%-roundup-{suffix}" });

        var options = Options();
        var recorder = new UsageRecorder();
        var (ai, _) = CreateAi(shared: recorder);
        var aiOptions = options;
        var roundupOptions = new RoundupGeneratorOptions { RateLimitDelaySeconds = 0, MaxRetries = 3 };
        var logger = NullLogger<RoundupAiHelper>.Instance;
        var aiHelper = new RoundupAiHelper(recorder, roundupOptions, aiOptions, logger);

        var appSettings = LoadAppSettings();

        var generator = new RoundupGeneratorService(
            new SectionRoundupRepository(db, NullLogger<SectionRoundupRepository>.Instance),
            new ContentFixerService(db, null, NullLogger<ContentFixerService>.Instance),
            ai,
            new ContentItemWriteRepository(db, NullLogger<ContentItemWriteRepository>.Instance),
            new RoundupNewsWriter(aiHelper, appSettings, roundupOptions, NullLogger<RoundupNewsWriter>.Instance),
            new RoundupMetadataGenerator(aiHelper, roundupOptions, appSettings, NullLogger<RoundupMetadataGenerator>.Instance),
            appSettings,
            NullLogger<RoundupGeneratorService>.Instance);

        var progress = new List<string>();
        var watch = Stopwatch.StartNew();
        var outcome = await generator.GenerateAsync(weekStart, weekEnd, new SyncProgress(progress.Add), null, ct);
        watch.Stop();

        var generated = (await db.QueryAsync<(string Slug, string Section, string Title, string Content)>(
            "SELECT slug, primary_section_name, title, content FROM content_items WHERE collection_name = 'roundups' AND slug LIKE @Pattern",
            new { Pattern = $"%-roundup-{suffix}" })).ToList();
        var newDir = OutDir("roundups/new");
        foreach (var (slug, _, title, body) in generated)
        {
            File.WriteAllText(Path.Combine(newDir, slug + ".md"), $"# {title}\n\n{body}");
        }

        string Strip(string text) => Regex.Replace(text, @"(?ms)^## (Also relevant this week|Dates to know)\b.*?(?=^## |\z)", string.Empty);
        static HashSet<string> Urls(string text) => Regex.Matches(text, @"\]\((https?://[^)\s]+)\)").Select(m => m.Groups[1].Value).ToHashSet(StringComparer.OrdinalIgnoreCase);
        int Duplicates(IEnumerable<string> texts) => texts.SelectMany(t => Urls(t)).GroupBy(u => u, StringComparer.OrdinalIgnoreCase).Count(g => g.Count() > 1);

        var report = new StringBuilder();
        report.AppendLine(CultureInfo.InvariantCulture, $"# Roundup evaluation, week {weekStart:yyyy-MM-dd} to {weekEnd:yyyy-MM-dd} (published {suffix})");
        report.AppendLine();
        report.AppendLine(CultureInfo.InvariantCulture, $"Outcome: {outcome.Result}, {outcome.GeneratedCount} roundups generated in {watch.Elapsed.TotalMinutes:F1} minutes.");
        report.AppendLine();
        report.AppendLine("| Section | production words | new words | production links | new full-story links | new brief links |");
        report.AppendLine("|---|---|---|---|---|---|");
        foreach (var (slug, section, _, prodContent) in production.OrderBy(p => p.Section))
        {
            var fresh = generated.FirstOrDefault(g => g.Section == section);
            var freshBody = fresh.Content ?? string.Empty;
            var brief = Urls(freshBody).Count - Urls(Strip(freshBody)).Count;
            report.AppendLine(CultureInfo.InvariantCulture, $"| {section} | {Words(prodContent)} | {Words(freshBody)} | {Urls(prodContent).Count} | {Urls(Strip(freshBody)).Count} | {brief} |");
        }

        report.AppendLine();
        report.AppendLine(CultureInfo.InvariantCulture, $"- Links that appear in two or more roundups (full write-ups): production **{Duplicates(production.Select(p => p.Content))}**, new **{Duplicates(generated.Select(g => Strip(g.Content)))}**");
        report.AppendLine(CultureInfo.InvariantCulture, $"- Total words: production {production.Sum(p => Words(p.Content))}, new {generated.Sum(g => Words(g.Content))}");
        report.AppendLine(CultureInfo.InvariantCulture, $"- 'Dates to know' blocks generated: {generated.Count(g => g.Content.Contains("## Dates to know", StringComparison.Ordinal))} of {generated.Count}");
        report.AppendLine();
        report.AppendLine(recorder.Summary());
        report.AppendLine();
        report.AppendLine("## Progress log");
        foreach (var line in progress)
        {
            report.AppendLine("- " + line);
        }

        File.WriteAllText(OutFile("roundups.md"), report.ToString());
        Console.WriteLine(report.ToString());
        outcome.Result.Should().Be(RoundupGenerationResult.Generated);
    }

    private sealed class SyncProgress(Action<string> handler) : IProgress<string>
    {
        public void Report(string value) => handler(value);
    }

    /// <summary>Wraps the real client and records the token usage per model.</summary>
    private sealed class UsageRecorder : IAiCompletionClient
    {
        private readonly Dictionary<string, (int Calls, long Prompt, long Cached, long Completion, long Reasoning, int Flex)> _usage = [];

        public IAiCompletionClient Inner { get; set; } = null!;

        public async Task<AiCompletionResult> SendCompletionAsync(string jsonBody, CancellationToken ct = default)
        {
            var result = await Inner.SendCompletionAsync(jsonBody, ct);
            if (result.ResponseBody is null)
            {
                return result;
            }

            using var request = JsonDocument.Parse(jsonBody);
            var model = request.RootElement.GetProperty("model").GetString()!;
            using var response = JsonDocument.Parse(result.ResponseBody);
            if (response.RootElement.TryGetProperty("usage", out var usage))
            {
                var prompt = usage.GetProperty("prompt_tokens").GetInt64();
                var completion = usage.GetProperty("completion_tokens").GetInt64();
                var cached = usage.TryGetProperty("prompt_tokens_details", out var pd) && pd.TryGetProperty("cached_tokens", out var c) ? c.GetInt64() : 0;
                var reasoning = usage.TryGetProperty("completion_tokens_details", out var cd) && cd.TryGetProperty("reasoning_tokens", out var r) ? r.GetInt64() : 0;
                var flex = response.RootElement.TryGetProperty("service_tier", out var tier) && tier.GetString() == "flex" ? 1 : 0;
                _usage.TryGetValue(model, out var current);
                _usage[model] = (current.Calls + 1, current.Prompt + prompt, current.Cached + cached, current.Completion + completion, current.Reasoning + reasoning, current.Flex + flex);
            }

            return result;
        }

        public string Summary()
        {
            var sb = new StringBuilder();
            sb.AppendLine("### Token usage");
            sb.AppendLine();
            sb.AppendLine("| Model | calls | prompt tokens | cached | completion tokens | of which reasoning | avg prompt | avg completion |");
            sb.AppendLine("|---|---|---|---|---|---|---|---|");
            foreach (var (model, u) in _usage.OrderBy(k => k.Key))
            {
                sb.AppendLine(CultureInfo.InvariantCulture,
                    $"| {model} | {u.Calls} | {u.Prompt} | {u.Cached} | {u.Completion} | {u.Reasoning} | {(u.Calls == 0 ? 0 : u.Prompt / u.Calls)} | {(u.Calls == 0 ? 0 : u.Completion / u.Calls)} |");
            }

            return sb.ToString();
        }
    }
}
