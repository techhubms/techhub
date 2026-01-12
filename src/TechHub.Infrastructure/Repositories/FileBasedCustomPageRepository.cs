using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// File-based repository for custom pages
/// Loads markdown files from the _custom collection directory
/// </summary>
public class FileBasedCustomPageRepository : ICustomPageRepository
{
    private readonly string _basePath;
    private readonly FrontMatterParser _frontMatterParser;
    private IReadOnlyList<CustomPage>? _cachedPages;
    private readonly SemaphoreSlim _loadLock = new(1, 1);

    public FileBasedCustomPageRepository(
        IOptions<AppSettings> settings,
        IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(environment);

        _frontMatterParser = new FrontMatterParser();

        // Get base path from configuration
        var collectionsPath = settings.Value.Content.CollectionsPath;

        // Convert relative path to absolute
        _basePath = Path.IsPathRooted(collectionsPath)
            ? collectionsPath
            : Path.Combine(environment.ContentRootPath, collectionsPath);
    }

    public async Task<IReadOnlyList<CustomPage>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Return cached data if already loaded
        if (_cachedPages != null)
        {
            return _cachedPages;
        }

        // Thread-safe lazy loading
        await _loadLock.WaitAsync(cancellationToken);
        try
        {
            // Double-check after acquiring lock
            if (_cachedPages != null)
            {
                return _cachedPages;
            }

            var customPagesPath = Path.Combine(_basePath, "_custom");

            if (!Directory.Exists(customPagesPath))
            {
                _cachedPages = [];
                return _cachedPages;
            }

            var pages = new List<CustomPage>();
            var markdownFiles = Directory.GetFiles(customPagesPath, "*.md", SearchOption.TopDirectoryOnly);

            foreach (var filePath in markdownFiles)
            {
                var page = await LoadCustomPageAsync(filePath, cancellationToken);
                if (page != null)
                {
                    pages.Add(page);
                }
            }

            _cachedPages = [.. pages.OrderBy(p => p.Title)];
            return _cachedPages;
        }
        finally
        {
            _loadLock.Release();
        }
    }

    public async Task<CustomPage?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var pages = await GetAllAsync(cancellationToken);
        return pages.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<CustomPage?> LoadCustomPageAsync(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            var fileContent = await File.ReadAllTextAsync(filePath, cancellationToken);
            var (frontMatter, content) = _frontMatterParser.Parse(fileContent);

            // Extract required fields
            var title = _frontMatterParser.GetValue<string>(frontMatter, "title");
            var permalink = _frontMatterParser.GetValue<string>(frontMatter, "permalink");

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(permalink))
            {
                // Skip files without required frontmatter
                return null;
            }

            // Extract optional fields
            var description = _frontMatterParser.GetValue<string>(frontMatter, "description") ?? string.Empty;
            var categories = _frontMatterParser.GetListValue(frontMatter, "categories");
            var sidebarInfo = _frontMatterParser.GetValue<JsonElement?>(frontMatter, "sidebar_info");

            // Derive slug from permalink (last segment of the permalink path)
            // Example: "/github-copilot/levels-of-enlightenment" → "levels-of-enlightenment"
            var slug = permalink.TrimEnd('/').Split('/').Last();

            return new CustomPage
            {
                Slug = slug,
                Title = title,
                Description = description,
                Permalink = permalink,
                Categories = categories,
                Content = content,
                SidebarInfo = sidebarInfo
            };
        }
        catch
        {
            // Skip files that can't be parsed
            return null;
        }
    }
}
