using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.DTOs;

namespace TechHub.E2E.Tests.Api;

/// <summary>
/// End-to-end tests for API endpoints using real file system data
/// These tests validate the entire stack from HTTP request to file reading
/// </summary>
public class ApiEndToEndTests : IClassFixture<ApiTestFactory>
{
    private readonly HttpClient _client;

    public ApiEndToEndTests(ApiTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    #region Section Endpoints

    [Fact]
    public async Task GetAllSections_ReturnsRealSections()
    {
        // Act
        var response = await _client.GetAsync("/api/sections");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var sections = await response.Content.ReadFromJsonAsync<List<SectionDto>>();
        sections.Should().NotBeNull();
        sections!.Should().NotBeEmpty();
        
        // Verify expected sections from sections.json
        sections.Should().Contain(s => s.Id == "ai");
        sections.Should().Contain(s => s.Id == "github-copilot");
        sections.Should().Contain(s => s.Id == "azure");
        sections.Should().Contain(s => s.Id == "ml");
        sections.Should().Contain(s => s.Id == "coding");
        sections.Should().Contain(s => s.Id == "devops");
        sections.Should().Contain(s => s.Id == "security");
        
        // All sections should have collections
        sections.Should().AllSatisfy(s => s.Collections.Should().NotBeEmpty());
    }

    [Fact]
    public async Task GetSectionById_ReturnsRealSection()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var section = await response.Content.ReadFromJsonAsync<SectionDto>();
        section.Should().NotBeNull();
        section!.Id.Should().Be("ai");
        section.Title.Should().Be("Artificial Intelligence");
        section.Category.Should().Be("AI");
        section.Collections.Should().NotBeEmpty();
    }

    #endregion

    #region Content Endpoints - Collections

    [Fact]
    public async Task GetAllContent_ReturnsRealContent()
    {
        // Act
        var response = await _client.GetAsync("/api/content");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        content.Should().NotBeNull();
        content.Should().NotBeEmpty();
        
        // Verify content has expected structure
        content!.Should().AllSatisfy(item =>
        {
            item.Id.Should().NotBeNullOrEmpty();
            item.Title.Should().NotBeNullOrEmpty();
            item.Collection.Should().NotBeNullOrEmpty();
            item.DateEpoch.Should().BeGreaterThan(0);
            item.Url.Should().NotBeNullOrEmpty();
        });
        
        // Content should be sorted by date descending (newest first)
        for (int i = 0; i < content!.Count - 1; i++)
        {
            var currentItem = content[i];
            var nextItem = content[i + 1];
            if (currentItem is not null && nextItem is not null)
            {
                currentItem.DateEpoch.Should().BeGreaterThanOrEqualTo(nextItem.DateEpoch);
            }
        }
    }

    [Fact]
    public async Task GetContentByCollection_News_ReturnsNewsItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collection=news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items!.Should().AllSatisfy(item => item.Collection.Should().Be("news"));
    }

    [Fact]
    public async Task GetContentByCollection_Videos_ReturnsVideoItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collection=videos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.Collection.Should().Be("videos"));
    }

    [Fact]
    public async Task GetContentByCollection_Blogs_ReturnsBlogItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collection=blogs");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.Collection.Should().Be("blogs"));
    }

    [Fact]
    public async Task GetContentByCollection_Community_ReturnsCommunityItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collection=community");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.Collection.Should().Be("community"));
    }

    [Fact]
    public async Task GetContentByCollection_Roundups_ReturnsRoundupItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?collection=roundups");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.Collection.Should().Be("roundups"));
    }

    #endregion

    #region Content Endpoints - Categories

    [Fact]
    public async Task GetContentByCategory_AI_ReturnsAIItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?category=AI");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.Categories.Should().Contain("AI"));
    }

    [Fact]
    public async Task GetContentByCategory_GitHubCopilot_ReturnsCopilotItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?category=GitHub Copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.Categories.Should().Contain("GitHub Copilot"));
    }

    [Fact]
    public async Task GetContentByCategory_Azure_ReturnsAzureItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?category=Azure");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.Categories.Should().Contain("Azure"));
    }

    #endregion

    #region Content Endpoints - Filtering

    [Fact]
    public async Task FilterContent_BySection_ReturnsFilteredItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item => item.Categories.Should().Contain("AI"));
    }

    [Fact]
    public async Task FilterContent_BySectionAndCollection_ReturnsFilteredItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=github-copilot&collections=news");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items.Should().AllSatisfy(item =>
        {
            item.Collection.Should().Be("news");
            item.Categories.Should().Contain("GitHub Copilot");
        });
    }

    [Fact]
    public async Task FilterContent_ByMultipleSections_ReturnsItemsFromAnySections()
    {
        // Act
        var response = await _client.GetAsync("/api/content/filter?sections=ai,azure");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        items!.Should().AllSatisfy(item =>
        {
            var hasAI = item.Categories.Contains("AI");
            var hasAzure = item.Categories.Contains("Azure");
            (hasAI || hasAzure).Should().BeTrue("each item should have at least one of the filtered categories");
        });
    }

    [Fact]
    public async Task SearchContent_ByQuery_ReturnsMatchingItems()
    {
        // Act
        var response = await _client.GetAsync("/api/content?q=copilot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();
        items.Should().NotBeNull();
        items!.Should().NotBeEmpty();
        
        // Items should contain "copilot" in title, description, or tags
        items.Should().AllSatisfy(item =>
        {
            var containsInTitle = item.Title.Contains("copilot", StringComparison.OrdinalIgnoreCase);
            var containsInDescription = item.Description.Contains("copilot", StringComparison.OrdinalIgnoreCase);
            var containsInTags = item.Tags.Any(tag => tag.Contains("copilot", StringComparison.OrdinalIgnoreCase));
            
            (containsInTitle || containsInDescription || containsInTags).Should().BeTrue();
        });
    }

    #endregion

    #region Content Endpoints - Tags

    [Fact]
    public async Task GetAllTags_ReturnsRealTags()
    {
        // Act
        var response = await _client.GetAsync("/api/content/tags");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tags = await response.Content.ReadFromJsonAsync<List<string>>();
        tags.Should().NotBeNull();
        tags!.Should().NotBeEmpty();
        tags.Should().OnlyHaveUniqueItems();
        
        // Tags should be lowercase (normalized)
        tags.Should().AllSatisfy(tag => tag.Should().Be(tag.ToLowerInvariant()));
    }

    #endregion

    #region Performance Tests

    [Fact]
    public async Task GetAllContent_RespondsQuickly_DueToInMemoryCache()
    {
        // Warm up cache
        await _client.GetAsync("/api/content");

        // Act - Second request should be very fast (from cache)
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _client.GetAsync("/api/content");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Should respond in less than 100ms from cache
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(100);
    }

    [Fact]
    public async Task FilterContent_RespondsQuickly_DueToInMemoryCache()
    {
        // Warm up cache
        await _client.GetAsync("/api/content/filter?sections=ai");

        // Act - Second request should be very fast (from cache)
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _client.GetAsync("/api/content/filter?sections=ai");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Should respond in less than 100ms from cache
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(100);
    }

    #endregion

    #region Data Validation Tests

    [Fact]
    public async Task AllContent_HasRequiredFields()
    {
        // Act
        var response = await _client.GetAsync("/api/content");
        var content = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();

        // Assert
        content!.Should().AllSatisfy(item =>
        {
            item.Id.Should().NotBeNullOrEmpty();
            item.Title.Should().NotBeNullOrEmpty();
            item.Description.Should().NotBeNullOrEmpty();
            item.Author.Should().NotBeNullOrEmpty();
            item.DateEpoch.Should().BeGreaterThan(0);
            item.DateIso.Should().NotBeNullOrEmpty();
            item.Collection.Should().NotBeNullOrEmpty();
            item.Categories.Should().NotBeEmpty();
            item.Tags.Should().NotBeEmpty();
            item.Url.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task AllContent_HasValidDateFormats()
    {
        // Act
        var response = await _client.GetAsync("/api/content");
        var content = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();

        // Assert
        content!.Should().AllSatisfy(item =>
        {
            // DateEpoch should be a reasonable Unix timestamp
            item.DateEpoch.Should().BeGreaterThan(1000000000); // After 2001
            item.DateEpoch.Should().BeLessThan(2000000000); // Before 2033
            
            // DateIso should be parseable
            DateTimeOffset.TryParse(item.DateIso, out _).Should().BeTrue();
        });
    }

    [Fact]
    public async Task AllContent_HasValidUrls()
    {
        // Act
        var response = await _client.GetAsync("/api/content");
        var content = await response.Content.ReadFromJsonAsync<List<ContentItemDto>>();

        // Assert
        content!.Should().AllSatisfy(item =>
        {
            // URL should start with / and contain the collection and id
            item.Url.Should().StartWith("/");
            item.Url.Should().Contain(item.Collection);
            item.Url.Should().Contain(item.Id);
        });
    }

    #endregion
}

/// <summary>
/// Custom WebApplicationFactory for E2E tests with real file system
/// </summary>
public class ApiTestFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set content root to workspace directory
        var workspaceRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."));
        
        // CRITICAL: Change working directory for File.* APIs to match content root
        Directory.SetCurrentDirectory(workspaceRoot);
        
        builder.UseContentRoot(workspaceRoot);
        
        // Configure AppSettings directly in DI with object initializer
        builder.ConfigureServices((context, services) =>
        {
            // Remove any existing AppSettings configuration
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IOptions<AppSettings>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            // Configure with test-specific settings using object initializer
            var testSettings = new AppSettings
            {
                Content = new ContentSettings
                {
                    CollectionsPath = "collections",
                    SectionsConfigPath = "_data/sections.json",
                    Timezone = "Europe/Brussels",
                    MaxExcerptLength = 1000
                },
                Caching = new CachingSettings
                {
                    ContentAbsoluteExpirationMinutes = 60,
                    ContentSlidingExpirationMinutes = 30,
                    ApiResponseAbsoluteExpirationMinutes = 60,
                    EnableOutputCaching = true
                },
                Seo = new SeoSettings
                {
                    BaseUrl = "https://tech.hub.ms",
                    SiteTitle = "Microsoft Tech Hub",
                    SiteDescription = "Your central hub for Microsoft technology updates, tutorials, and community content"
                },
                Performance = new PerformanceSettings
                {
                    EnableCompression = true,
                    EnableHttp2 = true,
                    EnableHttp3 = false,
                    MaxConcurrentRequests = 10000
                }
            };
            
            services.AddSingleton(Options.Create(testSettings));
        });
        
        // Suppress verbose logging during tests
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Warning);
        });

        builder.UseEnvironment("Test");
    }
}
