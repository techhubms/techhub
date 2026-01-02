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
            [
              {
                "Id": "ai",
                "Title": "AI",
                "Description": "Artificial Intelligence",
                "Category": "ai",
                "Url": "/ai",
                "BackgroundImage": "/assets/ai-bg.jpg",
                "Collections": [
                  {
                    "Title": "Latest News",
                    "Collection": "news",
                    "Url": "/ai/news",
                    "Description": "AI news and announcements",
                    "IsCustom": false
                  },
                  {
                    "Title": "Videos",
                    "Collection": "videos",
                    "Url": "/ai/videos",
                    "Description": "AI video content",
                    "IsCustom": false
                  }
                ]
              },
              {
                "Id": "github-copilot",
                "Title": "GitHub Copilot",
                "Description": "AI pair programmer",
                "Category": "github-copilot",
                "Url": "/github-copilot",
                "BackgroundImage": "/assets/ghc-bg.jpg",
                "Collections": [
                  {
                    "Title": "Latest News",
                    "Collection": "news",
                    "Url": "/github-copilot/news",
                    "Description": "GitHub Copilot news",
                    "IsCustom": false
                  }
                ]
              }
            ]
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
            [
              {
                "Id": "azure",
                "Title": "Azure",
                "Description": "Cloud platform",
                "Category": "azure",
                "Url": "/azure",
                "BackgroundImage": "/assets/azure-bg.jpg",
                "Collections": [
                  {
                    "Title": "Latest News",
                    "Collection": "news",
                    "Url": "/azure/news",
                    "Description": "Azure news",
                    "IsCustom": false
                  }
                ]
              }
            ]
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
            [
              {
                "Id": "security",
                "Title": "Security",
                "Description": "Security topics",
                "Category": "security",
                "Url": "/security",
                "BackgroundImage": "/assets/security-bg.jpg",
                "Collections": [
                  {
                    "Title": "Latest News",
                    "Collection": "news",
                    "Url": "/security/news",
                    "Description": "Security news",
                    "IsCustom": false
                  }
                ]
              }
            ]
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
            [
              {
                "Id": "github-copilot",
                "Title": "GitHub Copilot",
                "Description": "AI assistant",
                "Category": "github-copilot",
                "Url": "/github-copilot",
                "BackgroundImage": "/assets/ghc-bg.jpg",
                "Collections": [
                  {
                    "Title": "Latest News",
                    "Collection": "news",
                    "Url": "/github-copilot/news",
                    "Description": "Copilot news",
                    "IsCustom": false
                  }
                ]
              }
            ]
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
        // Arrange: Create empty sections array
        var sectionsJson = "[]";
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
            [
              // AI Section
              {
                "Id": "ai",
                "Title": "AI",
                "Description": "Artificial Intelligence",
                "Category": "ai",
                "Url": "/ai",
                "BackgroundImage": "/assets/ai-bg.jpg",
                "Collections": [
                  {
                    "Title": "Latest News",
                    "Collection": "news",
                    "Url": "/ai/news",
                    "Description": "AI news",
                    "IsCustom": false
                  }
                ], // Trailing comma on collections
              }, // Trailing comma on section
            ]
            """;
        await File.WriteAllTextAsync(_sectionsFilePath, sectionsJson);

        // Act: Load sections
        var sections = await _repository.GetAllAsync();

        // Assert: Parsed successfully despite comments and trailing commas
        Assert.Single(sections);
        Assert.Equal("ai", sections[0].Id);
    }
}
