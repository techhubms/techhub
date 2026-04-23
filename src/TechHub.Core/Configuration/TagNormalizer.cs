using System.Text.RegularExpressions;

namespace TechHub.Core.Configuration;

/// <summary>
/// Normalizes tag casing and format for consistent display across the site.
/// Ported from the PowerShell Get-FilteredTags.ps1 tag pipeline.
///
/// Processing order:
/// 0. Strip parenthesized suffixes (e.g. "Model Context Protocol (MCP)" → "Model Context Protocol")
/// 1. Full-tag lookup (e.g. "modelcontextprotocol" → "MCP")
/// 2. Per-word lookup within multi-word tags (e.g. "azure openai" → "Azure OpenAI")
/// 3. Fallback: title-case words that start lowercase, preserve already-cased words
/// 4. Remove noise tags (stop-words, numeric-only, hex color codes)
/// 5. Deduplicate case-insensitively (keep first occurrence)
/// </summary>
public static partial class TagNormalizer
{
    /// <summary>
    /// Mapping from section slug to the tag name that should be present.
    /// Used to ensure section-derived tags exist during ingestion and repair.
    /// </summary>
    private static readonly Dictionary<string, string> _sectionSlugToTag = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ai"] = "AI",
        ["azure"] = "Azure",
        ["github-copilot"] = "GitHub Copilot",
        ["dotnet"] = ".NET",
        ["devops"] = "DevOps",
        ["security"] = "Security",
        ["ml"] = "ML"
    };

    /// <summary>
    /// Ensures that section-derived tags are present in the tag list.
    /// For example, if sections contain "ai", ensures the "AI" tag exists.
    /// Also adds "AI" when "GitHub Copilot" is a section.
    /// </summary>
    public static List<string> EnsureSectionTags(IEnumerable<string> tags, IEnumerable<string> sections)
    {
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(sections);

        var result = new List<string>(tags);
        var existing = new HashSet<string>(result, StringComparer.OrdinalIgnoreCase);
        var hasGhc = false;

        foreach (var section in sections)
        {
            if (_sectionSlugToTag.TryGetValue(section, out var sectionTag) && existing.Add(sectionTag))
            {
                result.Add(sectionTag);
            }

            if (section.Equals("github-copilot", StringComparison.OrdinalIgnoreCase))
            {
                hasGhc = true;
            }
        }

        // GitHub Copilot → also ensure AI tag
        if (hasGhc && existing.Add("AI"))
        {
            result.Add("AI");
        }

        return result;
    }

    /// <summary>
    /// Canonical casing for known tags and abbreviations.
    /// Key = lowercased + spaces removed form. Value = display form.
    /// </summary>
    private static readonly Dictionary<string, string> _tagMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        // .NET ecosystem
        [".net"] = ".NET",
        ["dotnet"] = ".NET",
        ["dotnetcli"] = ".NET CLI",
        ["aspnet"] = "ASP.NET",
        ["aspnetcore"] = "ASP.NET Core",
        ["blazor"] = "Blazor",
        ["csharp"] = "C#",
        ["efcore"] = "EF Core",
        ["entityframeworkcore"] = "EF Core",
        ["ef"] = "EF",
        ["entityframework"] = "EF",
        ["fsharp"] = "F#",
        ["maui"] = "MAUI",
        [".netmaui"] = "MAUI",
        ["signalr"] = "SignalR",
        ["nuget"] = "NuGet",
        ["automapper"] = "AutoMapper",
        ["dapper"] = "Dapper",
        ["fluentassertions"] = "FluentAssertions",
        ["mediatr"] = "MediatR",
        ["polly"] = "Polly",
        ["serilog"] = "Serilog",
        ["nlog"] = "NLog",
        ["mstest"] = "MSTest",
        ["nunit"] = "NUnit",
        ["xunit"] = "xUnit",
        ["msbuilds"] = "MSBuild",
        ["mvc"] = "MVC",
        ["webapi"] = "Web API",
        ["winui"] = "WinUI",
        ["xamarin"] = "Xamarin",

        // AI & ML
        ["ai"] = "AI",
        ["artificialintelligence"] = "AI",
        ["aiagent"] = "AI Agent",
        ["aiagents"] = "AI Agents",
        ["ml"] = "ML",
        ["machinelearning"] = "ML",
        ["openai"] = "OpenAI",
        ["azureopenai"] = "Azure OpenAI",
        ["semantickernel"] = "Semantic Kernel",
        ["mcp"] = "MCP",
        ["modelcontextprotocol"] = "MCP",
        ["mcpserver"] = "MCP Server",
        ["a2a"] = "A2A",

        // Azure
        ["azureai"] = "Azure AI",
        ["aifoundry"] = "Azure AI Foundry",
        ["azureappservice"] = "Azure App Service",
        ["cognitiveservices"] = "Azure Cognitive Services",
        ["azurecontainer"] = "Azure Container",
        ["azdo"] = "Azure DevOps",
        ["azuredevops"] = "Azure DevOps",
        ["azurefunctions"] = "Azure Functions",
        ["azurekeyvault"] = "Azure Key Vault",
        ["azureml"] = "Azure Machine Learning",
        ["azuremonitor"] = "Azure Monitor",
        ["azuresql"] = "Azure SQL",
        ["azurestorage"] = "Azure Storage",

        ["llm"] = "LLM",
        ["largelanguagemodel"] = "LLM",
        ["largelanguagemodels"] = "LLM",

        // Cloud & infra
        ["aks"] = "AKS",
        ["azurekubernetesservice"] = "AKS",
        ["azurek8s"] = "AKS",
        ["k8s"] = "Kubernetes",
        ["docker"] = "Docker",
        ["terraform"] = "Terraform",
        ["bicep"] = "Bicep",
        ["iac"] = "IaC",
        ["infrastructureascode"] = "IaC",
        ["arm"] = "ARM",
        ["armtemplates"] = "ARM Templates",
        ["vm"] = "VM",

        // GitHub
        ["github"] = "GitHub",
        ["githubactions"] = "GitHub Actions",
        ["githubcopilot"] = "GitHub Copilot",
        ["copilotchat"] = "Copilot Chat",
        ["copilotcli"] = "Copilot CLI",
        ["copilotvoice"] = "Copilot Voice",
        ["git"] = "Git",

        // Microsoft products
        ["mscopilot"] = "Microsoft Copilot",
        ["powerautomate"] = "Power Automate",
        ["powerapps"] = "Power Apps",
        ["powerbi"] = "Power BI",
        ["powerplatform"] = "Power Platform",
        ["applicationinsights"] = "Application Insights",
        ["appinsights"] = "Application Insights",

        // Web technologies
        ["javascript"] = "JavaScript",
        ["js"] = "JavaScript",
        ["css"] = "CSS",
        ["html"] = "HTML",
        ["json"] = "JSON",
        ["xml"] = "XML",
        ["yaml"] = "YAML",
        ["yml"] = "YAML",
        ["toml"] = "TOML",
        ["csv"] = "CSV",
        ["markdown"] = "Markdown",
        ["md"] = "Markdown",
        ["npm"] = "npm",
        ["pip"] = "pip",

        // Protocols & security
        ["api"] = "API",
        ["rest"] = "REST",
        ["grpc"] = "gRPC",
        ["http"] = "HTTP",
        ["https"] = "HTTPS",
        ["oauth"] = "OAuth",
        ["jwt"] = "JWT",
        ["saml"] = "SAML",
        ["ssl"] = "SSL",
        ["tls"] = "TLS",
        ["tcp"] = "TCP",
        ["udp"] = "UDP",
        ["dns"] = "DNS",
        ["pwa"] = "PWA",
        ["spa"] = "SPA",

        // Tools & roles
        ["vs"] = "VS",
        ["visualstudio"] = "VS",
        ["vscode"] = "VS Code",
        ["visualstudiocode"] = "VS Code",
        ["ide"] = "IDE",
        ["cli"] = "CLI",
        ["sdk"] = "SDK",
        ["orm"] = "ORM",
        ["etl"] = "ETL",
        ["ci"] = "CI",
        ["cd"] = "CD",
        ["db"] = "DB",
        ["sql"] = "SQL",
        ["sqlserver"] = "SQL Server",
        ["nosql"] = "NoSQL",
        ["qa"] = "QA",
        ["ui"] = "UI",
        ["ux"] = "UX",
        ["os"] = "OS",
        ["ad"] = "AD",

        // Cloud providers
        ["aws"] = "AWS",
        ["gcp"] = "GCP",

        // Leave lowercase prepositions as-is when they appear as standalone words
        ["as"] = "as",
        ["in"] = "in",
    };

    /// <summary>
    /// Tags that are too generic or noise-like to be useful.
    /// These are removed during normalization.
    /// </summary>
    private static readonly HashSet<string> _filterWords = new(StringComparer.OrdinalIgnoreCase)
    {
        // Publishing terms
        "Post", "Posts", "Update", "Updates", "Announcement", "Announcements",
        "What's New", "Article", "Articles", "Editorial", "Editorials",
        "Feature", "Features", "Release", "Releases", "Bulletin", "Bulletins",
        "Update Note", "Patch Note", "Patch", "Changelog", "Changelogs",
        "Press Release", "Press Releases", "Statement", "Statements",
        "Notification", "Notifications", "Alert", "Alerts", "Digest", "Digests",
        "Recap", "Recaps", "Review", "Reviews", "Insight", "Insights",
        "Report", "Reports", "Coverage", "Announcement Note",
        "Community Post", "Community Update",
        "Guest Blog", "Guest Post", "Guest Article", "Guest Editorial",
        "Version",

        // Stop words (articles, prepositions, etc.)
        "and", "or", "the", "a", "an", "in", "on", "at", "to", "for", "of", "with", "by", "as",
        "is", "are", "was", "were", "be", "been", "being", "have", "has", "had", "do", "does", "did",
        "will", "would", "could", "should", "may", "might", "can", "must", "shall",
        "this", "that", "these", "those", "it", "its", "they", "them", "their", "we", "our", "you", "your",
        "he", "him", "his", "she", "her", "hers", "i", "me", "my", "mine",
        "from", "into", "onto", "upon", "about", "above", "below", "under", "over", "through",
        "between", "among", "during", "before", "after", "while", "since", "until", "unless",
        "if", "when", "where", "why", "how", "what", "which", "who", "whom", "whose",
        "but", "so", "yet", "nor", "either", "neither", "both", "all", "any", "some", "each", "every",
        "no", "not", "none", "nothing", "something", "anything", "everything",
        "here", "there", "everywhere", "somewhere", "anywhere", "nowhere",
        "now", "then", "always", "never", "sometimes", "often", "usually", "rarely", "seldom",
        "very", "too", "quite", "rather", "pretty", "much", "many", "more", "most", "less", "least",
        "good", "better", "best", "bad", "worse", "worst", "big", "bigger", "biggest", "small", "smaller", "smallest",
        "new", "old", "young", "long", "short", "high", "low", "fast", "slow", "easy", "hard",
    };

    /// <summary>
    /// Deprecated tags that should be removed entirely.
    /// Tags with useful replacements belong in <see cref="_tagMappings"/> instead
    /// (e.g. "Machine Learning" → "ML" via key "machinelearning").
    /// </summary>
    private static readonly HashSet<string> _deprecatedTags = new(StringComparer.OrdinalIgnoreCase)
    {
        "Cloud",                // Too generic, deprecated section
        "Coding",               // Too generic
    };

    /// <summary>
    /// Normalizes a single tag's casing using the mapping dictionary.
    /// Does NOT filter or remove tags — only fixes casing.
    /// </summary>
    public static string NormalizeTagCasing(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            return tag;
        }

        tag = tag.Trim();

        // 1. Full-tag lookup (collapse spaces for matching)
        var key = tag.Replace(" ", "", StringComparison.Ordinal).ToLowerInvariant();
        if (_tagMappings.TryGetValue(key, out var mapped))
        {
            return mapped;
        }

        // 2. Per-word normalization for multi-word tags
        var words = tag.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < words.Length; i++)
        {
            var wordKey = words[i].ToLowerInvariant();
            if (_tagMappings.TryGetValue(wordKey, out var wordMapped))
            {
                words[i] = wordMapped;
            }
            else if (words[i].Length > 0 && char.IsUpper(words[i][0]))
            {
                // Already starts uppercase — preserve (e.g. "Kubernetes")
            }
            else if (words[i].Length > 1)
            {
                // Title-case: uppercase first letter, lowercase rest
                words[i] = char.ToUpperInvariant(words[i][0]) + words[i][1..].ToLowerInvariant();
            }
            else if (words[i].Length == 1)
            {
                words[i] = words[i].ToUpperInvariant();
            }
        }

        return string.Join(' ', words);
    }

    /// <summary>
    /// Normalizes a list of tags: fixes casing, removes noise/deprecated tags,
    /// deduplicates case-insensitively (keeps first occurrence).
    /// Returns a new list; does not modify the input.
    /// </summary>
    public static List<string> NormalizeTags(IEnumerable<string> tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        var result = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var rawTag in tags)
        {
            if (string.IsNullOrWhiteSpace(rawTag))
            {
                continue;
            }

            var tag = rawTag.Trim();

            // Strip parenthesized suffixes like "(MCP)" from "Model Context Protocol (MCP)"
            tag = ParenthesizedSuffixRegex().Replace(tag, "").TrimEnd();

            // Replace dashes/underscores with spaces
            tag = tag.Replace('-', ' ').Replace('_', ' ');

            // Split on semicolons/commas (some feeds embed multiple tags in one)
            var parts = tag.Split([';', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue;
                }

                // Skip deprecated tags
                if (_deprecatedTags.Contains(part))
                {
                    continue;
                }

                // Skip noise/stop-word tags
                if (_filterWords.Contains(part))
                {
                    continue;
                }

                // Skip numeric-only tags
                if (NumericOnlyRegex().IsMatch(part))
                {
                    continue;
                }

                // Skip year tags (2020, 2025, etc.)
                if (YearRegex().IsMatch(part))
                {
                    continue;
                }

                // Skip hex color codes (3 or 6 hex chars) unless in the mapping
                var partKey = part.Replace(" ", "", StringComparison.Ordinal).ToLowerInvariant();
                if ((part.Length is 3 or 6) && HexRegex().IsMatch(part) && !_tagMappings.ContainsKey(partKey))
                {
                    continue;
                }

                // Skip single-char tags
                if (part.Length <= 1)
                {
                    continue;
                }

                // Normalize casing
                var normalized = NormalizeTagCasing(part);

                // Deduplicate (keep first)
                if (seen.Add(normalized))
                {
                    result.Add(normalized);
                }
            }
        }

        return result;
    }

    [GeneratedRegex(@"^\d+$")]
    private static partial Regex NumericOnlyRegex();

    [GeneratedRegex(@"^20\d{2}$")]
    private static partial Regex YearRegex();

    [GeneratedRegex(@"^[0-9A-Fa-f]+$")]
    private static partial Regex HexRegex();

    [GeneratedRegex(@"\s*\([^)]*\)")]
    private static partial Regex ParenthesizedSuffixRegex();
}
