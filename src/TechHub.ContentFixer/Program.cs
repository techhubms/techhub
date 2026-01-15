using System.Text;
using System.Text.RegularExpressions;
using TechHub.Core.Helpers;
using TechHub.Infrastructure.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TechHub.ContentFixer;

/// <summary>
/// Console application to fix markdown frontmatter and content for .NET migration
/// 
/// Fixes:
/// - Rename 'categories' to 'section_names' and normalize values
/// - Remove 'tags_normalized' frontmatter field
/// - Remove 'excerpt_separator' frontmatter field
/// - Fix permalinks to include /primarySection/collection/ prefix
/// - Remove 'description' field from frontmatter
/// - Replace Jekyll variables with actual values
/// - Keep {% youtube VIDEO_ID %} tags intact
/// </summary>
internal sealed class Program
{
    private static readonly Dictionary<string, string> SectionMapping = new()
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

    private static async Task<int> Main(string[] args)
    {
        var dryRun = args.Contains("--dry-run");
        var fileArg = Array.FindIndex(args, a => a == "--file");
        var path = fileArg >= 0 && fileArg + 1 < args.Length
            ? args[fileArg + 1]
            : args.FirstOrDefault(a => !a.StartsWith("--", StringComparison.Ordinal)) ?? "collections";

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

        int processed = 0;
        int skipped = 0;
        int errors = 0;

        foreach (var file in files)
        {
            try
            {
                var changed = await ProcessFileAsync(file, dryRun);
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

    private static async Task<bool> ProcessFileAsync(string filePath, bool dryRun)
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

        // 2. Remove tags_normalized
        if (frontMatter.Remove("tags_normalized"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed tags_normalized");
            Console.ResetColor();
            changed = true;
        }

        // 3. Remove excerpt_separator
        if (frontMatter.Remove("excerpt_separator"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Removed excerpt_separator");
            Console.ResetColor();
            changed = true;
        }

        // 4. Fix permalink to include /section/collection/ prefix
        if (frontMatter.TryGetValue("permalink", out var permalinkObj))
        {
            var permalink = permalinkObj?.ToString() ?? string.Empty;

            // Check if already has section/collection prefix (idempotent)
            if (!Regex.IsMatch(permalink, @"^/[^/]+/[^/]+/"))
            {
                var filename = permalink.TrimStart('/').Replace(".html", "", StringComparison.Ordinal);
                var slug = Regex.Replace(filename, @"^\d{4}-\d{2}-\d{2}-", "");

                var sectionNames = GetListValue(frontMatter, "section_names");
                var primarySection = SectionPriorityHelper.GetPrimarySectionUrl(sectionNames, collection);

                var newPermalink = $"/{primarySection}/{collection}/{slug}";
                frontMatter["permalink"] = newPermalink;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  ✓ Fixed permalink: {permalink} → {newPermalink}");
                Console.ResetColor();
                changed = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  ℹ Permalink already has section/collection prefix");
                Console.ResetColor();
            }
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

        // 6. Replace Jekyll variables in content
        var originalContent = markdownContent;
        markdownContent = ReplaceJekyllVariables(markdownContent, frontMatter, description);
        if (markdownContent != originalContent)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Replaced Jekyll variables");
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
        if (SectionMapping.TryGetValue(displayName, out var normalized))
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

    private static string ReplaceJekyllVariables(string content, Dictionary<string, object> frontMatter, string? description)
    {
        // Replace {{ page.description }} with actual description
        if (!string.IsNullOrEmpty(description))
        {
            content = Regex.Replace(content, @"\{\{\s*page\.description\s*\}\}", description);
        }

        // Replace {{ "/path" | relative_url }} with /path
        content = Regex.Replace(content, @"\{\{\s*""([^""]+)""\s*\|\s*relative_url\s*\}\}", "$1");

        // Replace any other {{ page.variable }} with frontmatter values
        content = Regex.Replace(content, @"\{\{\s*page\.(\w+)\s*\}\}", match =>
        {
            var varName = match.Groups[1].Value;
            if (frontMatter.TryGetValue(varName, out var value))
            {
                return value switch
                {
                    IEnumerable<object> list => string.Join(", ", list.Select(x => x?.ToString() ?? "")),
                    _ => value?.ToString() ?? match.Value
                };
            }

            return match.Value;
        });

        // Remove {% raw %} and {% endraw %} tags
        content = Regex.Replace(content, @"\{%\s*(?:raw|endraw)\s*%\}", "");

        return content;
    }

    private static string SerializeFrontMatter(Dictionary<string, object> frontMatter)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        var yaml = serializer.Serialize(frontMatter);
        return $"---\n{yaml}---\n";
    }
}
