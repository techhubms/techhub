using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// TEMPORARY end-to-end test for YouTube transcript → Azure OpenAI (gpt-5.2) pipeline.
/// Skips automatically unless AZURE_OPENAI_KEY and YOUTUBE_URL env vars are set.
///
/// Usage:
///   $env:AZURE_OPENAI_KEY = "your-azure-openai-api-key"
///   $env:YOUTUBE_URL = "https://www.youtube.com/watch?v=..."
///   $env:AZURE_OPENAI_ENV = "staging"   # optional: "staging" (default) or "prod"
///   Run -TestProject TechHub.Infrastructure.Tests -Filter "YouTubeTranscriptE2ETest"
///
/// Uses the same Azure OpenAI endpoint as the PowerShell workflow:
///   https://oai-techhub-{env}.cognitiveservices.azure.com/openai/deployments/gpt-5.2/chat/completions
///
/// DELETE THIS FILE when done testing.
/// </summary>
public class YouTubeTranscriptE2ETest : IDisposable
{
    private const string Model = "gpt-5.2";
    private const string ApiVersion = "2024-05-01-preview";
    private readonly string? _apiKey;
    private readonly string? _youtubeUrl;
    private readonly string _environment;
    private bool _disposed;

    public YouTubeTranscriptE2ETest()
    {
        _apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY");
        _youtubeUrl = Environment.GetEnvironmentVariable("YOUTUBE_URL");
        _environment = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENV") ?? "staging";
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    [Fact(Skip = "End-to-end test for YouTube transcript → Azure OpenAI pipeline. Set AZURE_OPENAI_KEY and YOUTUBE_URL env vars to run.")]
    public async Task FetchTranscript_AndCategorize_OutputsFullResult()
    {
        // Skip guard (belt-and-suspenders in case Skip is removed but env vars are missing)
        if (string.IsNullOrWhiteSpace(_apiKey) || string.IsNullOrWhiteSpace(_youtubeUrl))
        {
            Console.WriteLine("⚠ Skipped: set AZURE_OPENAI_KEY and YOUTUBE_URL environment variables");
            return;
        }

        var endpoint = $"https://oai-techhub-{_environment}.cognitiveservices.azure.com/openai/deployments/{Model}/chat/completions?api-version={ApiVersion}";

        // ── Step 1: Fetch transcript ──
        Console.WriteLine($"═══ YouTube Transcript → Azure OpenAI Pipeline Test ═══");
        Console.WriteLine($"URL: {_youtubeUrl}");
        Console.WriteLine($"Endpoint: {endpoint}");
        Console.WriteLine($"Model: {Model}");
        Console.WriteLine();

        var options = Options.Create(new ContentProcessorOptions { RequestTimeoutSeconds = 60 });
        using var transcriptService = new YouTubeTranscriptService(
            options, NullLogger<YouTubeTranscriptService>.Instance);

        var transcript = await transcriptService.GetTranscriptAsync(_youtubeUrl, TestContext.Current.CancellationToken);

        Console.WriteLine($"── Transcript ──");
        if (transcript == null)
        {
            Console.WriteLine("(no transcript available — no captions found)");
            return;
        }

        Console.WriteLine($"Length: {transcript.Length} chars");
        Console.WriteLine($"Preview: {transcript[..Math.Min(500, transcript.Length)]}...");
        Console.WriteLine();

        // ── Step 2: Build the prompt (same logic as AiCategorizationService) ──
        var item = new RawFeedItem
        {
            Title = "Test YouTube Video",
            ExternalUrl = _youtubeUrl,
            PublishedAt = DateTimeOffset.UtcNow,
            Description = "Video from YouTube (temporary test)",
            FeedName = "Manual Test",
            CollectionName = "videos",
            FullContent = transcript
        };

        var userPrompt = BuildUserPrompt(item);
        var systemMessage = LoadSystemMessage();

        Console.WriteLine($"── Prompt Stats ──");
        Console.WriteLine($"System message: {systemMessage.Length} chars");
        Console.WriteLine($"User prompt: {userPrompt.Length} chars");
        Console.WriteLine();

        // ── Step 3: Call Azure OpenAI ──
        using var httpClient = new HttpClient();
        var requestBody = new
        {
            model = Model,
            messages = new[]
            {
                new { role = "system", content = systemMessage },
                new { role = "user", content = userPrompt }
            },
            temperature = 0.1,
            max_completion_tokens = 4000
        };

        var json = JsonSerializer.Serialize(requestBody);
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Add("api-key", _apiKey);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        Console.WriteLine("Calling Azure OpenAI API...");
        var response = await httpClient.SendAsync(request, TestContext.Current.CancellationToken);
        var responseJson = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"✗ API Error: HTTP {(int)response.StatusCode}");
            Console.WriteLine(responseJson);
            response.IsSuccessStatusCode.Should().BeTrue("OpenAI API call should succeed");
            return;
        }

        // ── Step 4: Parse and display result ──
        using var doc = JsonDocument.Parse(responseJson);
        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        var usage = doc.RootElement.GetProperty("usage");
        var promptTokens = usage.GetProperty("prompt_tokens").GetInt32();
        var completionTokens = usage.GetProperty("completion_tokens").GetInt32();

        Console.WriteLine($"✓ OpenAI responded ({promptTokens} prompt + {completionTokens} completion tokens)");
        Console.WriteLine();
        Console.WriteLine($"══ RAW AI RESPONSE ══");
        Console.WriteLine(content);
        Console.WriteLine();

        // Try to extract the JSON and show the markdown content nicely
        if (content != null)
        {
            var jsonContent = ExtractJson(content);
            if (!string.IsNullOrWhiteSpace(jsonContent))
            {
                using var aiDoc = JsonDocument.Parse(jsonContent);
                var root = aiDoc.RootElement;

                Console.WriteLine($"══ PARSED FIELDS ══");
                PrintField(root, "title");
                PrintField(root, "collection");
                PrintField(root, "primary_section");
                PrintArray(root, "sections");
                PrintArray(root, "tags");
                PrintField(root, "author");
                PrintField(root, "excerpt");

                Console.WriteLine();
                Console.WriteLine($"══ GENERATED CONTENT (Markdown) ══");
                if (root.TryGetProperty("content", out var contentProp))
                {
                    Console.WriteLine(contentProp.GetString());
                }

                // ── Step 5: Write markdown file to videos collection ──
                WriteVideoMarkdown(root, _youtubeUrl!, item);
            }
        }

        content!.Should().NotBeNullOrWhiteSpace("AI should return content");
    }

    private static string BuildUserPrompt(RawFeedItem item)
    {
        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Please categorize the following content:");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"FEED: {item.FeedName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"COLLECTION: {item.CollectionName}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"TITLE: {item.Title}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"URL: {item.ExternalUrl}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"DATE: {item.PublishedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");

        if (!string.IsNullOrWhiteSpace(item.Author))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"AUTHOR: {item.Author}");
        }

        if (!string.IsNullOrWhiteSpace(item.Description))
        {
            sb.AppendLine();
            sb.AppendLine("DESCRIPTION:");
            sb.AppendLine(item.Description);
        }

        if (!string.IsNullOrWhiteSpace(item.FullContent))
        {
            sb.AppendLine();
            sb.AppendLine("TRANSCRIPT (auto-generated from video closed captions):");
            sb.AppendLine(item.FullContent);
        }

        if (item.FeedTags.Count > 0)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"FEED TAGS: {string.Join(", ", item.FeedTags)}");
        }

        return sb.ToString();
    }

    private static string LoadSystemMessage()
    {
        var assembly = typeof(AiCategorizationService).Assembly;
        const string ResourceName = "TechHub.Infrastructure.Data.Resources.system-message.md";
        using var stream = assembly.GetManifestResourceStream(ResourceName);
        if (stream == null)
        {
            return "You are a content categorization assistant for a tech hub site.";
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static void WriteVideoMarkdown(JsonElement root, string youtubeUrl, RawFeedItem item)
    {
        var title = root.TryGetProperty("title", out var titleProp) ? titleProp.GetString() ?? "Untitled" : "Untitled";
        var author = root.TryGetProperty("author", out var authorProp) ? authorProp.GetString() : item.Author;
        var excerpt = root.TryGetProperty("excerpt", out var excerptProp) ? excerptProp.GetString() ?? "" : "";
        var contentBody = root.TryGetProperty("content", out var contentBodyProp) ? contentBodyProp.GetString() ?? "" : "";

        // Map categories to section_names
        var sectionNameMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["AI"] = "ai",
            ["Azure"] = "azure",
            ["GitHub Copilot"] = "github-copilot",
            [".NET"] = "dotnet",
            ["DevOps"] = "devops",
            ["Security"] = "security",
            ["Coding"] = "coding",
            ["Cloud"] = "cloud"
        };

        var sectionNames = new List<string>();
        if (root.TryGetProperty("categories", out var catProp) && catProp.ValueKind == JsonValueKind.Array)
        {
            sectionNames = catProp.EnumerateArray()
                .Select(cat => cat.GetString())
                .Where(catStr => catStr != null)
                .Select(catStr => sectionNameMapping.TryGetValue(catStr!, out var mapped)
                    ? mapped
                    : Regex.Replace(catStr!.ToLowerInvariant(), @"\s+", "-"))
                .ToList();
        }

        sectionNames = sectionNames.Distinct().OrderBy(s => s).ToList();

        // Compute primary_section
        var priorityOrder = new[] { "github-copilot", "ai", "ml", "dotnet", "azure", "devops", "security" };
        var primarySection = priorityOrder.FirstOrDefault(sectionNames.Contains) ?? "all";

        if (primarySection == "all" && sectionNames.Count > 0)
        {
            primarySection = sectionNames[0];
        }

        // Collect tags
        var tags = new List<string>();
        if (root.TryGetProperty("tags", out var tagsProp) && tagsProp.ValueKind == JsonValueKind.Array)
        {
            tags.AddRange(tagsProp.EnumerateArray()
                .Where(e => e.ValueKind == JsonValueKind.String)
                .Select(e => e.GetString()!)
                .Where(t => !string.IsNullOrWhiteSpace(t)));
        }

        if (!tags.Any(t => t.Equals("Videos", StringComparison.OrdinalIgnoreCase)))
        {
            tags.Add("Videos");
        }

        tags = tags.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(t => t).ToList();

        // Build frontmatter
        var sb = new StringBuilder();
        sb.AppendLine("---");
        sb.AppendLine("section_names:");
        foreach (var s in sectionNames)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"- {s}");
        }

        sb.AppendLine("feed_name: Manual Test");
        sb.AppendLine("tags:");
        foreach (var t in tags)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"- {t}");
        }

        sb.AppendLine(CultureInfo.InvariantCulture, $"external_url: {youtubeUrl}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"title: \"{title.Replace("\"", "\\\"")}\"");
        sb.AppendLine(CultureInfo.InvariantCulture, $"date: {DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture)}");
        sb.AppendLine(CultureInfo.InvariantCulture, $"primary_section: {primarySection}");
        if (!string.IsNullOrWhiteSpace(author))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"author: {author}");
        }

        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"{excerpt}<!--excerpt_end-->");
        sb.AppendLine();
        sb.AppendLine(contentBody);

        // Generate filename
        var safeTitle = Regex.Replace(title, @"[^\w\s-]", "");
        safeTitle = Regex.Replace(safeTitle, @"\s+", "-");
        if (safeTitle.Length > 200)
        {
            safeTitle = safeTitle[..200];
        }

        var datePrefix = DateTimeOffset.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var filename = $"{datePrefix}-{safeTitle}.md";

        // Write to .tmp/tests/ instead of collections/ to avoid polluting the repository
        var repoRoot = FindRepoRoot();
        var outputDir = Path.Join(repoRoot, ".tmp", "tests", "videos");
        Directory.CreateDirectory(outputDir);
        var filePath = Path.Join(outputDir, filename);

        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        Console.WriteLine();
        Console.WriteLine($"══ MARKDOWN FILE WRITTEN ══");
        Console.WriteLine($"  Path: {filePath}");
        Console.WriteLine($"  Filename: {filename}");
    }

    private static string FindRepoRoot()
    {
        var dir = AppContext.BaseDirectory;
        while (dir != null)
        {
            if (File.Exists(Path.Join(dir, "TechHub.slnx")))
            {
                return dir;
            }

            dir = Path.GetDirectoryName(dir);
        }

        // Fallback to known path in dev container
        return "/workspaces/techhub";
    }

    private static string ExtractJson(string response)
    {
        var cleaned = Regex.Replace(
            response, @"```(?:json)?\s*", string.Empty,
            RegexOptions.IgnoreCase).Trim();
        var start = cleaned.IndexOf('{', StringComparison.Ordinal);
        var end = cleaned.LastIndexOf('}');
        if (start < 0 || end < 0 || end <= start)
        {
            return string.Empty;
        }

        return cleaned[start..(end + 1)];
    }

    private static void PrintField(JsonElement root, string name)
    {
        if (root.TryGetProperty(name, out var prop) && prop.ValueKind == JsonValueKind.String)
        {
            Console.WriteLine($"  {name}: {prop.GetString()}");
        }
    }

    private static void PrintArray(JsonElement root, string name)
    {
        if (root.TryGetProperty(name, out var prop) && prop.ValueKind == JsonValueKind.Array)
        {
            var items = prop.EnumerateArray()
                .Where(e => e.ValueKind == JsonValueKind.String)
                .Select(e => e.GetString());
            Console.WriteLine($"  {name}: [{string.Join(", ", items)}]");
        }
    }
}
