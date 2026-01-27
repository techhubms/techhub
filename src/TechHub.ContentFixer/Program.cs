using System.Text;
using System.Text.RegularExpressions;
using TechHub.Infrastructure.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TechHub.ContentFixer;

/// <summary>
/// Console application to fix markdown frontmatter and content for the Tech hub
/// 
/// Fixes:
/// - Rename 'categories' to 'section_names' and normalize values
/// - Remove 'tags_normalized' frontmatter field
/// - Remove 'excerpt_separator' frontmatter field
/// - Remove 'youtube_id' frontmatter field (obsolete - extracted from content at runtime)
/// - Remove 'layout' frontmatter field (not used in .NET implementation)
/// - Rename 'canonical_url' to 'external_url'
/// - Remove 'feed_url' frontmatter field (dynamic)
/// - Remove 'permalink' frontmatter field (built dynamically from collection + primary section + slug)
/// - Remove 'alt_collection' and 'alt-collection' frontmatter fields (collection derived from directory path)
/// - Remove 'section' frontmatter field (singular - replaced by section_names)
/// - Remove 'description' field from frontmatter
/// - Remove 'viewing_mode' frontmatter field
/// - Replace template variables ({{ page.variable }}) with actual values
/// - Expand template variables inside tags ({% youtube page.variable %} → {% youtube VALUE %})
/// - Remove {% raw %} and {% endraw %} tags
/// - Process {{ "/path" | relative_url }} filters
/// - Keep {% youtube VIDEO_ID %} tags intact (will be processed at runtime)
/// - Remove section/collection names from tags ONLY for sections/collections this item belongs to
///   (e.g., if section_names contains "ai", remove "AI" tag; if collection is "blogs", remove "Blogs" tag)
/// - Special rule: If "GitHub Copilot" is in tags, also remove "AI" tag
/// </summary>
internal sealed class Program
{
    private static readonly Dictionary<string, string> _sectionMapping = new()
    {
        ["AI"] = "ai",
        ["Azure"] = "azure",
        ["GitHub Copilot"] = "github-copilot",
        [".NET"] = "dotnet",
        ["DevOps"] = "devops",
        ["Security"] = "security",
        ["Coding"] = "coding",
        ["Cloud"] = "cloud",
        ["Machine Learning"] = "ml",
        ["ML"] = "ml"
    };

    /// <summary>
    /// Reverse mapping: URL slug → display name(s)
    /// Used to determine which tag names to remove based on section_names.
    /// </summary>
    private static readonly Dictionary<string, List<string>> _sectionSlugToDisplayNames = new()
    {
        ["ai"] = ["AI", "Artificial Intelligence"],
        ["azure"] = ["Azure"],
        ["github-copilot"] = ["GitHub Copilot"],
        ["dotnet"] = [".NET", "dotnet"],
        ["devops"] = ["DevOps"],
        ["security"] = ["Security"],
        ["coding"] = ["Coding"],
        ["cloud"] = ["Cloud"],
        ["ml"] = ["Machine Learning", "ML"],
        ["machine-learning"] = ["Machine Learning", "ML"]
    };

    /// <summary>
    /// Collection name → display name(s)
    /// Used to determine which tag names to remove based on collection.
    /// </summary>
    private static readonly Dictionary<string, List<string>> _collectionToDisplayNames = new()
    {
        ["news"] = ["News"],
        ["blogs"] = ["Blogs"],
        ["videos"] = ["Videos"],
        ["community"] = ["Community"],
        ["roundups"] = ["Roundups"],
        ["events"] = ["Events"]
    };

    /// <summary>
    /// Section priority order (matches the menubar order).
    /// These are lowercase section NAMES matching Section.Name and ContentItem.SectionNames.
    /// Used to determine which section is "primary" when an item belongs to multiple sections.
    /// </summary>
    private static readonly string[] _sectionPriorityOrder =
    [
        "github-copilot",
        "ai",
        "ml",
        "coding",
        "azure",
        "devops",
        "security"
    ];

    private static async Task<int> Main(string[] args)
    {
        var dryRun = args.Contains("--dry-run");
        var fileArg = Array.FindIndex(args, a => a == "--file");
        var path = fileArg >= 0 && fileArg + 1 < args.Length
            ? args[fileArg + 1]
            : args.FirstOrDefault(a => !a.StartsWith("--", StringComparison.OrdinalIgnoreCase)) ?? "collections";

        Console.WriteLine("==> Tech Hub Content Fixer");
        Console.WriteLine();

        if (dryRun)
        {
            Console.WriteLine("Running in DRY RUN mode - no files will be modified");
            Console.WriteLine();
        }

        var files = GetMarkdownFiles(path);
        Console.WriteLine($"Found {files.Count} markdown files");
        Console.WriteLine();

        // Build slug-to-article mapping for link rewriting
        var slugMap = await BuildSlugMapAsync(files);
        Console.WriteLine($"Built slug map with {slugMap.Count} entries");
        Console.WriteLine();

        int processed = 0;
        int skipped = 0;
        int errors = 0;

        foreach (var file in files)
        {
            try
            {
                var changed = await ProcessFileAsync(file, slugMap, dryRun);
                if (changed)
                {
                    processed++;
                }
                else
                {
                    skipped++;
                }
            }
            catch (Exception ex)
            {
                errors++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR processing {file}:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
                Console.WriteLine();
                return 1; // Exit on first error
            }
        }

        Console.WriteLine();
        Console.WriteLine("==> Summary");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Processed: {processed} files");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Skipped: {skipped} files (already fixed)");
            Console.ResetColor();
        }

        if (errors > 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Errors: {errors} files");
            Console.ResetColor();
        }

        return 0;
    }

    private static List<string> GetMarkdownFiles(string path)
    {
        if (File.Exists(path))
        {
            return [path];
        }

        if (Directory.Exists(path))
        {
            return [.. Directory.GetFiles(path, "*.md", SearchOption.AllDirectories).OrderBy(f => f)];
        }

        throw new ArgumentException($"Path not found: {path}");
    }

    private static async Task<bool> ProcessFileAsync(string filePath, Dictionary<string, ArticleInfo> slugMap, bool dryRun)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Processing: {filePath}");
        Console.ResetColor();

        var content = await File.ReadAllTextAsync(filePath);
        var parser = new FrontMatterParser();
        Dictionary<string, object> frontMatter;
        string markdownContent;

        try
        {
            (frontMatter, markdownContent) = parser.Parse(content);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  ✗ Parser exception: {ex.Message}");
            Console.WriteLine($"    {ex.GetType().Name}: {ex.StackTrace}");
            Console.ResetColor();
            Console.WriteLine();
            return false;
        }

        if (frontMatter.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ⚠ Skipping - no frontmatter found");
            Console.ResetColor();
            Console.WriteLine();
            return false;
        }

        bool changed = false;

        // Determine collection from file path
        var collection = ExtractCollectionFromPath(filePath);

        // 1. Rename 'categories' to 'section_names' and normalize
        if (frontMatter.ContainsKey("categories"))
        {
            var categories = GetListValue(frontMatter, "categories");
            var sectionNames = categories.Select(NormalizeSectionName).Where(s => !string.IsNullOrEmpty(s)).ToList();
            frontMatter["section_names"] = sectionNames;
            frontMatter.Remove("categories");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  ✓ Renamed categories → section_names ({sectionNames.Count} sections)");
            Console.ResetColor();
            changed = true;
        }
        else if (frontMatter.ContainsKey("section_names"))
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("  ℹ Already has section_names");
            Console.ResetColor();
        }
        else
        {
            throw new InvalidOperationException("No categories or section_names found in frontmatter");
        }

        // 1b. Compute and add primary_section based on section priority order
        {
            var sectionNames = GetListValue(frontMatter, "section_names");
            var primarySection = GetPrimarySectionName(sectionNames, collection);

            // Check if already set and matches
            var existingPrimarySection = frontMatter.TryGetValue("primary_section", out var existing) ? existing?.ToString() : null;
            if (existingPrimarySection != primarySection)
            {
                frontMatter["primary_section"] = primarySection;
                if (existingPrimarySection != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"  ✓ Updated primary_section: {existingPrimarySection} → {primarySection}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"  ✓ Added primary_section: {primarySection}");
                }
                Console.ResetColor();
                changed = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"  ℹ primary_section already correct: {primarySection}");
                Console.ResetColor();
            }
        }

        // 2. Remove tags_normalized
        if (frontMatter.Remove("tags_normalized"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed tags_normalized");
            Console.ResetColor();
            changed = true;
        }

        // 3. Remove section/collection names from tags (only for sections/collections this item belongs to)
        if (frontMatter.TryGetValue("tags", out var tagsObj))
        {
            var currentTags = GetListValue(frontMatter, "tags");
            var sectionNames = GetListValue(frontMatter, "section_names");

            // Build list of tags to remove based on actual section_names and collection
            var tagsToRemove = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Add section display names for sections this item belongs to
            foreach (var sectionName in sectionNames)
            {
                if (_sectionSlugToDisplayNames.TryGetValue(sectionName, out var displayNames))
                {
                    foreach (var displayName in displayNames)
                    {
                        tagsToRemove.Add(displayName);
                    }
                }
            }

            // Add collection display names for the collection this item belongs to
            if (_collectionToDisplayNames.TryGetValue(collection, out var collectionDisplayNames))
            {
                foreach (var displayName in collectionDisplayNames)
                {
                    tagsToRemove.Add(displayName);
                }
            }

            // Special rule: If "GitHub Copilot" is in tags, also remove "AI"
            if (currentTags.Any(t => t.Equals("GitHub Copilot", StringComparison.OrdinalIgnoreCase)))
            {
                tagsToRemove.Add("AI");
            }

            var filteredTags = currentTags
                .Where(tag => !tagsToRemove.Contains(tag))
                .ToList();

            var removedCount = currentTags.Count - filteredTags.Count;
            if (removedCount > 0)
            {
                var removedTags = currentTags.Where(tag => tagsToRemove.Contains(tag)).ToList();
                frontMatter["tags"] = filteredTags;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  ✓ Removed {removedCount} section/collection tags: {string.Join(", ", removedTags)}");
                Console.ResetColor();
                changed = true;
            }
        }

        // 4. Remove excerpt_separator
        if (frontMatter.Remove("excerpt_separator"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed excerpt_separator");
            Console.ResetColor();
            changed = true;
        }

        // 4b. Remove youtube_id (obsolete - extracted from content at runtime)
        if (frontMatter.Remove("youtube_id"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed youtube_id");
            Console.ResetColor();
            changed = true;
        }

        // 4c. Remove layout (not used in .NET implementation)
        if (frontMatter.Remove("layout"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed layout");
            Console.ResetColor();
            changed = true;
        }

        // 4d. Rename canonical_url to external_url
        if (frontMatter.TryGetValue("canonical_url", out var canonicalUrlObj))
        {
            frontMatter["external_url"] = canonicalUrlObj;
            frontMatter.Remove("canonical_url");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Renamed canonical_url → external_url");
            Console.ResetColor();
            changed = true;
        }

        // 4e. Remove feed_url (not needed - dynamic)
        if (frontMatter.Remove("feed_url"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed feed_url");
            Console.ResetColor();
            changed = true;
        }

        // 4f. Remove permalink (built dynamically from collection + primary section + slug)
        if (frontMatter.Remove("permalink"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed permalink");
            Console.ResetColor();
            changed = true;
        }

        // 4g. Remove alt_collection (collection derived from directory path)
        // Handle both underscore and hyphen versions
        if (frontMatter.Remove("alt_collection") | frontMatter.Remove("alt-collection"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed alt_collection");
            Console.ResetColor();
            changed = true;
        }

        // 4h. Remove section (singular - replaced by section_names)
        if (frontMatter.Remove("section"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed section");
            Console.ResetColor();
            changed = true;
        }

        // 4i. Remove viewing_mode
        if (frontMatter.Remove("viewing_mode"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed viewing_mode");
            Console.ResetColor();
            changed = true;
        }

        // 5. Save description for variable replacement, then remove from frontmatter
        var description = frontMatter.TryGetValue("description", out var descObj) ? descObj?.ToString() : null;
        if (frontMatter.Remove("description"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed description from frontmatter");
            Console.ResetColor();
            changed = true;
        }

        // 6. Fix .html links to use correct /section/collection/slug URLs
        var originalContentBeforeLinks = markdownContent;
        markdownContent = FixHtmlLinks(markdownContent, slugMap);
        if (markdownContent != originalContentBeforeLinks)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Fixed .html links");
            Console.ResetColor();
            changed = true;
        }

        // 7. Replace template variables in content
        var originalContent = markdownContent;
        markdownContent = ReplaceTemplateVariables(markdownContent, frontMatter, description);
        if (markdownContent != originalContent)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Replaced template variables");
            Console.ResetColor();
            changed = true;
        }

        // Write back to file if changed
        if (changed)
        {
            if (!dryRun)
            {
                var newContent = SerializeFrontMatter(frontMatter) + markdownContent;
                await File.WriteAllTextAsync(filePath, newContent, Encoding.UTF8);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  ✓ Saved changes");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  [DRY RUN] Would save changes");
                Console.ResetColor();
            }
        }

        Console.WriteLine();
        return changed;
    }

    private static string ExtractCollectionFromPath(string filePath)
    {
        var match = Regex.Match(filePath, @"collections/_([^/]+)/");
        return match.Success ? match.Groups[1].Value : "unknown";
    }

    private static string NormalizeSectionName(string displayName)
    {
        if (_sectionMapping.TryGetValue(displayName, out var normalized))
        {
            return normalized;
        }

        return displayName.ToLowerInvariant().Replace(" ", "-", StringComparison.Ordinal);
    }

    private static List<string> GetListValue(Dictionary<string, object> frontMatter, string key)
    {
        if (!frontMatter.TryGetValue(key, out var value))
        {
            return [];
        }

        return value switch
        {
            string str => [str],
            IEnumerable<object> list => [.. list.Select(x => x?.ToString() ?? string.Empty)],
            _ => []
        };
    }

    private static string ReplaceTemplateVariables(string content, Dictionary<string, object> frontMatter, string? description)
    {
        // Step 1: Remove {% raw %} and {% endraw %} tags (used to escape GitHub Actions syntax)
        content = Regex.Replace(content, @"\{%\s*(?:raw|endraw)\s*%\}", string.Empty);

        // Step 2: Process {{ "/path" | relative_url }} filter → /path
        content = Regex.Replace(content, @"\{\{\s*""([^""]+)""\s*\|\s*relative_url\s*\}\}", "$1");

        // Step 3: Expand page.variable inside tags: {% youtube page.youtube_id %} → {% youtube VIDEO_ID %}
        content = Regex.Replace(content, @"(\{%\s*\w+\s+)page\.(\w+)(\s*%\})", match =>
        {
            var variableName = match.Groups[2].Value;
            if (frontMatter.TryGetValue(variableName, out var value))
            {
                var stringValue = ConvertToString(value);
                return $"{match.Groups[1].Value}{stringValue}{match.Groups[3].Value}";
            }

            // Variable not found - log warning but keep original
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ⚠ Warning: Variable 'page.{variableName}' not found in frontmatter (inside tag)");
            Console.ResetColor();
            return match.Value;
        });

        // Step 4: Replace {{ page.description }} with actual description (if available)
        if (!string.IsNullOrEmpty(description))
        {
            content = Regex.Replace(content, @"\{\{\s*page\.description\s*\}\}", description);
        }

        // Step 5: Replace {{ page.variable }} with frontmatter values
        content = Regex.Replace(content, @"\{\{\s*page\.(\w+)\s*\}\}", match =>
        {
            var varName = match.Groups[1].Value;
            if (frontMatter.TryGetValue(varName, out var value))
            {
                return ConvertToString(value);
            }

            // Variable not found - log warning but keep original
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ⚠ Warning: Variable 'page.{varName}' not found in frontmatter");
            Console.ResetColor();
            return match.Value;
        });

        return content;
    }

    private static string ConvertToString(object value)
    {
        return value switch
        {
            null => string.Empty,
            string s => s,
            IEnumerable<object> list => string.Join(", ", list.Select(x => x?.ToString() ?? string.Empty)),
            _ => value.ToString() ?? string.Empty
        };
    }

    private static string SerializeFrontMatter(Dictionary<string, object> frontMatter)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        var yaml = serializer.Serialize(frontMatter);
        return $"---\n{yaml}---\n";
    }

    /// <summary>
    /// Build a mapping of slug → article info (primary section, collection)
    /// This allows us to rewrite .html links to the correct /section/collection/slug URLs
    /// </summary>
    private static async Task<Dictionary<string, ArticleInfo>> BuildSlugMapAsync(List<string> files)
    {
        var map = new Dictionary<string, ArticleInfo>(StringComparer.OrdinalIgnoreCase);
        var parser = new FrontMatterParser();

        foreach (var file in files)
        {
            try
            {
                var content = await File.ReadAllTextAsync(file);
                var (frontMatter, _) = parser.Parse(content);

                // Extract slug from filename
                var filename = Path.GetFileNameWithoutExtension(file);
                var slugMatch = Regex.Match(filename, @"^\d{4}-\d{2}-\d{2}-(.+)$");
                var slug = slugMatch.Success ? slugMatch.Groups[1].Value : filename;

                // Get section names
                var sectionNames = GetListValue(frontMatter, "section_names");
                if (sectionNames.Count == 0)
                {
                    sectionNames = [.. GetListValue(frontMatter, "categories").Select(NormalizeSectionName)];
                }

                // Get collection from file path
                var collection = ExtractCollectionFromPath(file);

                // Determine primary section using priority order
                var primarySection = GetPrimarySectionName(sectionNames, collection);

                map[slug] = new ArticleInfo(primarySection, collection);
            }
            catch
            {
                // Skip files that can't be parsed
            }
        }

        return map;
    }

    /// <summary>
    /// Fix .html links to point to correct /section/collection/slug URLs
    /// Example: [text](2024-06-05-GitHub-Copilot-Power-User-example.html) → [text](/github-copilot/blogs/GitHub-Copilot-Power-User-example)
    /// </summary>
    private static string FixHtmlLinks(string markdown, Dictionary<string, ArticleInfo> slugMap)
    {
        // Pattern to match markdown links: [text](url.html) or [text](url.html "title")
        var linkPattern = @"\[([^\]]+)\]\(([^\s)]+\.html)(?:\s+""([^""]*)"")?\)";

        return Regex.Replace(markdown, linkPattern, match =>
        {
            var text = match.Groups[1].Value;
            var url = match.Groups[2].Value;
            var title = match.Groups[3].Success ? match.Groups[3].Value : null;

            // Only process local .html links (not external URLs)
            if (url.Contains("://", StringComparison.Ordinal))
            {
                return match.Value; // Keep external links unchanged
            }

            // Extract filename from URL (handle relative paths)
            var filename = Path.GetFileNameWithoutExtension(url);

            // Extract slug
            var slugMatch = Regex.Match(filename, @"^\d{4}-\d{2}-\d{2}-(.+)$");
            var slug = slugMatch.Success ? slugMatch.Groups[1].Value : filename;

            // Look up article in map
            if (slugMap.TryGetValue(slug, out var articleInfo))
            {
                // Build correct URL
                var newUrl = $"/{articleInfo.PrimarySectionName}/{articleInfo.CollectionName}/{slug}";

                // Rebuild markdown link
                var result = $"[{text}]({newUrl}";
                if (!string.IsNullOrEmpty(title))
                {
                    result += $" \"{title}\"";
                }

                result += ")";
                return result;
            }

            // If not found in map, keep original link (broken link - will be caught at runtime)
            return match.Value;
        });
    }

    private sealed record ArticleInfo(string PrimarySectionName, string CollectionName);

    /// <summary>
    /// Determines the primary section name for a content item based on its section names and collection.
    /// Uses the section priority order (matching the menubar order) to pick the highest-priority section.
    /// Special case: Roundups always belong to "all" section.
    /// </summary>
    /// <param name="sectionNames">List of lowercase section names (e.g., "ai", "github-copilot")</param>
    /// <param name="collectionName">Optional collection name (e.g., "roundups")</param>
    /// <returns>The lowercase name of the primary section (e.g., "github-copilot", "ai"), or "all" if no match</returns>
    private static string GetPrimarySectionName(IReadOnlyList<string> sectionNames, string? collectionName = null)
    {
        // Special case: Roundups always belong to "all" section
        if (collectionName?.Equals("roundups", StringComparison.OrdinalIgnoreCase) == true)
        {
            return "all";
        }

        if (sectionNames.Count == 0)
        {
            return "all";
        }

        // Find the first section that matches in priority order
        foreach (var sectionName in _sectionPriorityOrder)
        {
            if (sectionNames.Contains(sectionName, StringComparer.OrdinalIgnoreCase))
            {
                return sectionName; // Return lowercase name
            }
        }

        // No match found, default to "all"
        return "all";
    }
}
