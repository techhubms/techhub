using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
public class FileBasedCustomPageRepository : ICustomPageRepository, IDisposable
{
    private readonly string _basePath;
    private readonly FrontMatterParser _frontMatterParser;
    private readonly ILogger<FileBasedCustomPageRepository> _logger;
    private IReadOnlyList<CustomPage>? _cachedPages;
    private readonly SemaphoreSlim _loadLock = new(1, 1);
    private bool _disposed;

    public FileBasedCustomPageRepository(
        IOptions<AppSettings> settings,
        IHostEnvironment environment,
        ILogger<FileBasedCustomPageRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
        _frontMatterParser = new FrontMatterParser();

        // Get base path from configuration
        var collectionsPath = settings.Value.Content.CollectionsPath;

        // Resolve relative paths to absolute paths based on content root
        // In Development/Test: ContentRootPath = /workspaces/techhub/src/TechHub.Api
        // We need to go up to solution root to find collections/
        if (Path.IsPathRooted(collectionsPath))
        {
            _basePath = collectionsPath;
        }
        else
        {
            // Navigate up from API project to solution root, then to collections
            var solutionRoot = FindSolutionRoot(environment.ContentRootPath);
            _basePath = Path.Combine(solutionRoot, collectionsPath);
        }
    }

    /// <summary>
    /// Find solution root by walking up directory tree looking for TechHub.slnx
    /// </summary>
    private static string FindSolutionRoot(string startPath)
    {
        var directory = new DirectoryInfo(startPath);
        while (directory != null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "TechHub.slnx")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        // Fallback: assume startPath is already at solution root
        return startPath;
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

            _logger.LogInformation("Loading custom pages from: {Path}", customPagesPath);

            if (!Directory.Exists(customPagesPath))
            {
                _logger.LogWarning("Custom pages directory does not exist: {Path}", customPagesPath);
                _cachedPages = [];
                return _cachedPages;
            }

            var pages = new List<CustomPage>();
            var markdownFiles = Directory.GetFiles(customPagesPath, "*.md", SearchOption.TopDirectoryOnly);

            _logger.LogInformation("Found {Count} markdown files in custom pages directory", markdownFiles.Length);

            foreach (var filePath in markdownFiles)
            {
                var page = await LoadCustomPageAsync(filePath, cancellationToken);
                if (page != null)
                {
                    pages.Add(page);
                }
            }

            _logger.LogInformation("Successfully loaded {Count} custom pages", pages.Count);
            _logger.LogInformation("Loaded slugs: {Slugs}", string.Join(", ", pages.Select(p => p.Slug)));
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
#pragma warning disable CA1031 // Do not catch general exception types - intentional to gracefully handle any file loading errors
        catch (Exception ex)
#pragma warning restore CA1031
        {
            _logger.LogError(ex, "Failed to load custom page from file: {FilePath}", filePath);
            return null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _loadLock.Dispose();
            }

            _disposed = true;
        }
    }
}
