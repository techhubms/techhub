using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Infrastructure.Repositories;

namespace TechHub.Infrastructure.Tests.Repositories;

/// <summary>
/// Tests for section repository
/// Validates: sections.json loading, section lookup by ID, error handling
/// </summary>
public class FileBasedSectionRepositoryTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly string _sectionsFilePath;
    private readonly FileBasedSectionRepository _repository;

    public FileBasedSectionRepositoryTests()
    {
        // Create temp directory for test files
        _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDirectory);
        
        var dataDir = Path.Combine(_tempDirectory, "_data");
        Directory.CreateDirectory(dataDir);
        
        _sectionsFilePath = Path.Combine(dataDir, "sections.json");

        // Configure repository with temp path
        var settings = Options.Create(new AppSettings
        {
            Content = new ContentSettings 
            { 
                CollectionsPath = Path.Combine(_tempDirectory, "collections"),
                SectionsConfigPath = _sectionsFilePath,
                Timezone = "Europe/Brussels"
            },
            Caching = new CachingSettings(),
            Seo = new SeoSettings 
            { 
                BaseUrl = "https://test.local", 
                SiteTitle = "Test", 
                SiteDescription = "Test" 
            },
            Performance = new PerformanceSettings()
        });
        
        _repository = new FileBasedSectionRepository(settings);
    }

    public void Dispose()
    {
        // Cleanup temp directory
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, true);
        }
    }

    /// <summary>
    /// Test: Valid sections.json should load all sections
    /// Why: Core functionality - reading site structure from configuration
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ValidSectionsFile_LoadsAllSections()
    {
        // Arrange: Create sections.json with 2 sections
        var sectionsJson = """
            {
              "ai": {
                "title": "AI",
                "description": "Artificial Intelligence",
                "category": "ai",
                "url": "/ai",
                "section": "ai",
                "image": "/assets/ai-bg.jpg",
                "collections": [
                  {
                    "title": "Latest News",
                    "collection": "news",
                    "url": "/ai/news",
                    "description": "AI news and announcements",
                    "custom": false
                  },
                  {
                    "title": "Videos",
                    "collection": "videos",
                    "url": "/ai/videos",
                    "description": "AI video content",
                    "custom": false
                  }
                ]
              },
              "github-copilot": {
                "title": "GitHub Copilot",
                "description": "AI pair programmer",
                "category": "github-copilot",
                "url": "/github-copilot",
                "section": "github-copilot",
                "image": "/assets/ghc-bg.jpg",
                "collections": [
                  {
                    "title": "Latest News",
                    "collection": "news",
                    "url": "/github-copilot/news",
                    "description": "GitHub Copilot news",
                    "custom": false
                  }
                ]
              }
            }
            """;
        await File.WriteAllTextAsync(_sectionsFilePath, sectionsJson);

        // Act: Load all sections
        var sections = await _repository.GetAllAsync();

        // Assert: Both sections loaded with correct properties
        Assert.Equal(2, sections.Count);
        
        var aiSection = sections.First(s => s.Id == "ai");
        Assert.Equal("AI", aiSection.Title);
        Assert.Equal("Artificial Intelligence", aiSection.Description);
        Assert.Equal(2, aiSection.Collections.Count);
        
        var ghcSection = sections.First(s => s.Id == "github-copilot");
        Assert.Equal("GitHub Copilot", ghcSection.Title);
        Assert.Single(ghcSection.Collections);
    }

    /// <summary>
    /// Test: GetByIdAsync should return matching section
    /// Why: Individual section lookup by URL segment
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ExistingSection_ReturnsSection()
    {
        // Arrange: Create sections.json
        var sectionsJson = """
            {
              "azure": {
                "title": "Azure",
                "description": "Cloud platform",
                "category": "azure",
                "url": "/azure",
                "section": "azure",
                "image": "/assets/azure-bg.jpg",
                "collections": [
                  {
                    "title": "Latest News",
                    "collection": "news",
                    "url": "/azure/news",
                    "description": "Azure news",
                    "custom": false
                  }
                ]
              }
            }
            """;
        await File.WriteAllTextAsync(_sectionsFilePath, sectionsJson);

        // Act: Get section by ID
        var section = await _repository.GetByIdAsync("azure");

        // Assert: Correct section returned
        Assert.NotNull(section);
        Assert.Equal("azure", section.Id);
        Assert.Equal("Azure", section.Title);
    }

    /// <summary>
    /// Test: GetByIdAsync with non-existent ID should return null
    /// Why: Graceful handling of missing sections
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_NonExistentSection_ReturnsNull()
    {
        // Arrange: Create sections.json without "devops" section
        var sectionsJson = """
            {
              "security": {
                "title": "Security",
                "description": "Security topics",
                "category": "security",
                "url": "/security",
                "section": "security",
                "image": "/assets/security-bg.jpg",
                "collections": [
                  {
                    "title": "Latest News",
                    "collection": "news",
                    "url": "/security/news",
                    "description": "Security news",
                    "custom": false
                  }
                ]
              }
            }
            """;
        await File.WriteAllTextAsync(_sectionsFilePath, sectionsJson);

        // Act: Get non-existent section
        var section = await _repository.GetByIdAsync("devops");

        // Assert: Null returned (section not found)
        Assert.Null(section);
    }

    /// <summary>
    /// Test: Case-insensitive section ID lookup
    /// Why: URLs are case-insensitive, ID lookup should match
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_CaseInsensitive_ReturnsSection()
    {
        // Arrange: Create section with lowercase ID
        var sectionsJson = """
            {
              "github-copilot": {
                "title": "GitHub Copilot",
                "description": "AI assistant",
                "category": "github-copilot",
                "url": "/github-copilot",
                "section": "github-copilot",
                "image": "/assets/ghc-bg.jpg",
                "collections": [
                  {
                    "title": "Latest News",
                    "collection": "news",
                    "url": "/github-copilot/news",
                    "description": "Copilot news",
                    "custom": false
                  }
                ]
              }
            }
            """;
        await File.WriteAllTextAsync(_sectionsFilePath, sectionsJson);

        // Act: Search with different casing
        var section1 = await _repository.GetByIdAsync("GitHub-Copilot");
        var section2 = await _repository.GetByIdAsync("GITHUB-COPILOT");
        var section3 = await _repository.GetByIdAsync("github-copilot");

        // Assert: All variations return same section
        Assert.NotNull(section1);
        Assert.NotNull(section2);
        Assert.NotNull(section3);
        Assert.Equal("github-copilot", section1.Id);
        Assert.Equal("github-copilot", section2.Id);
        Assert.Equal("github-copilot", section3.Id);
    }

    /// <summary>
    /// Test: Missing sections.json file should throw FileNotFoundException
    /// Why: Fail fast when configuration file is missing
    /// </summary>
    [Fact]
    public async Task GetAllAsync_MissingFile_ThrowsFileNotFoundException()
    {
        // Arrange: Delete sections.json if it exists
        if (File.Exists(_sectionsFilePath))
        {
            File.Delete(_sectionsFilePath);
        }

        // Act & Assert: Exception thrown when file missing
        await Assert.ThrowsAsync<FileNotFoundException>(
            async () => await _repository.GetAllAsync());
    }

    /// <summary>
    /// Test: Empty sections.json should return empty list
    /// Why: Graceful handling of empty configuration
    /// </summary>
    [Fact]
    public async Task GetAllAsync_EmptyArray_ReturnsEmptyList()
    {
        // Arrange: Create empty sections dictionary
        var sectionsJson = "{}";
        await File.WriteAllTextAsync(_sectionsFilePath, sectionsJson);

        // Act: Load sections
        var sections = await _repository.GetAllAsync();

        // Assert: Empty list returned (no exception)
        Assert.Empty(sections);
    }

    /// <summary>
    /// Test: JSON with comments and trailing commas should parse correctly
    /// Why: sections.json may contain comments for documentation
    /// </summary>
    [Fact]
    public async Task GetAllAsync_JsonWithCommentsAndCommas_ParsesCorrectly()
    {
        // Arrange: JSON with comments and trailing comma
        var sectionsJson = """
            {
              // AI Section
              "ai": {
                "title": "AI",
                "description": "Artificial Intelligence",
                "category": "ai",
                "url": "/ai",
                "section": "ai",
                "image": "/assets/ai-bg.jpg",
                "collections": [
                  {
                    "title": "Latest News",
                    "collection": "news",
                    "url": "/ai/news",
                    "description": "AI news",
                    "custom": false
                  }
                ], // Trailing comma on collections
              }, // Trailing comma on section
            }
            """;
        await File.WriteAllTextAsync(_sectionsFilePath, sectionsJson);

        // Act: Load sections
        var sections = await _repository.GetAllAsync();

        // Assert: Parsed successfully despite comments and trailing commas
        Assert.Single(sections);
        Assert.Equal("ai", sections[0].Id);
    }

    /// <summary>
    /// INTEGRATION TEST: Load the actual sections.json file from the repository
    /// Why: Ensures our code works with the real data, not just test fixtures
    /// This catches mismatches between test data and production data
    /// </summary>
    [Fact]
    public async Task GetAllAsync_RealSectionsFile_LoadsSuccessfully()
    {
        // Arrange: Find the real sections.json file in the repository
        var repoRoot = FindRepositoryRoot();
        var realSectionsPath = Path.Combine(repoRoot, "_data", "sections.json");
        
        // Skip test if sections.json not found (e.g., running in isolated environment)
        if (!File.Exists(realSectionsPath))
        {
            return; // Skip test gracefully
        }

        // Copy real file to temp location
        File.Copy(realSectionsPath, _sectionsFilePath, overwrite: true);

        // Act: Load the real sections.json file
        var sections = await _repository.GetAllAsync();

        // Assert: Should load successfully and return multiple sections
        Assert.NotEmpty(sections);
        Assert.True(sections.Count >= 7, $"Expected at least 7 sections, got {sections.Count}");
        
        // Verify known sections exist
        Assert.Contains(sections, s => s.Id == "all");
        Assert.Contains(sections, s => s.Id == "ai");
        Assert.Contains(sections, s => s.Id == "github-copilot");
        
        // Verify section structure
        var aiSection = sections.First(s => s.Id == "ai");
        Assert.NotNull(aiSection.Title);
        Assert.NotNull(aiSection.Description);
        Assert.NotNull(aiSection.Url);
        Assert.NotNull(aiSection.Category);
        Assert.NotNull(aiSection.BackgroundImage);
        Assert.NotEmpty(aiSection.Collections);
    }

    /// <summary>
    /// Helper method to find repository root directory
    /// </summary>
    private static string FindRepositoryRoot()
    {
        var currentDir = Directory.GetCurrentDirectory();
        while (currentDir != null && !Directory.Exists(Path.Combine(currentDir, ".git")))
        {
            currentDir = Directory.GetParent(currentDir)?.FullName;
        }
        return currentDir ?? Directory.GetCurrentDirectory();
    }
}
