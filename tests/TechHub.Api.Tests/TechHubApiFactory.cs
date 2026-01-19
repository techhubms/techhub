using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Api.Tests;

/// <summary>
/// Custom WebApplicationFactory for API integration tests
/// - Logs to .tmp/test-logs/api-integration-tests.log (no console output)
/// - Uses Test environment
/// - Can use mocks (call SetupDefaultSections/SetupDefaultContent) OR real data (don't call setup)
/// </summary>
public class TechHubApiFactory : WebApplicationFactory<Program>
{
    public ISectionRepository? MockSectionRepository { get; private set; }
    public IContentRepository? MockContentRepository { get; private set; }
    private bool _useMocks = false;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set Test environment FIRST
        builder.UseEnvironment("Test");

        // Disable file logging during integration tests
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AppSettings:SkipFileLogging"] = "true"
            });
        });

        // Only swap out repositories if mocks are being used
        if (_useMocks)
        {
            builder.ConfigureServices(services =>
            {
                // Remove real repository registrations
                var sectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISectionRepository));
                if (sectionDescriptor != null)
                {
                    services.Remove(sectionDescriptor);
                }

                var contentDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IContentRepository));
                if (contentDescriptor != null)
                {
                    services.Remove(contentDescriptor);
                }

                // Register mocked repositories
                services.AddSingleton(MockSectionRepository!);
                services.AddSingleton(MockContentRepository!);
            });
        }

        // Override logging configuration to disable all logging during integration tests
        // This prevents log files from being created and keeps test output clean
        builder.ConfigureLogging((context, logging) =>
        {
            logging.ClearProviders();
        });
    }

    /// <summary>
    /// Setup default test data for section repository (enables mocking mode)
    /// </summary>
    public void SetupDefaultSections()
    {
        if (!_useMocks)
        {
            _useMocks = true;
            MockSectionRepository = new Mock<ISectionRepository>().Object;
            MockContentRepository = new Mock<IContentRepository>().Object;
        }

        var sections = new List<Section>
        {
            new()
            {
                Name = "ai", Title = "AI",
                Description = "Artificial Intelligence resources",
                Url = "/ai",
                BackgroundImage = "/assets/section-backgrounds/ai.jpg",
                Collections =
                [
                    new() { Title = "News", Name = "news", Url = "/ai/news", Description = "AI News", IsCustom = false },
                    new() { Title = "Blogs", Name = "blogs", Url = "/ai/blogs", Description = "AI Blogs", IsCustom = false }
                ]
            },
            new()
            {
                Name = "github-copilot",
                Title = "GitHub Copilot",
                Description = "GitHub Copilot resources",
                Url = "/github-copilot",
                BackgroundImage = "/assets/section-backgrounds/github-copilot.jpg",
                Collections =
                [
                    new() { Title = "News", Name = "news", Url = "/github-copilot/news", Description = "GitHub Copilot News", IsCustom = false },
                    new() { Title = "Videos", Name = "videos", Url = "/github-copilot/videos", Description = "GitHub Copilot Videos", IsCustom = false }
                ]
            }
        };

        var mockRepo = Mock.Get(MockSectionRepository!);
        mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(sections);

        mockRepo.Setup(r => r.GetByNameAsync("ai", It.IsAny<CancellationToken>()))
            .ReturnsAsync(sections[0]);

        mockRepo.Setup(r => r.GetByNameAsync("github-copilot", It.IsAny<CancellationToken>()))
            .ReturnsAsync(sections[1]);

        mockRepo.Setup(r => r.GetByNameAsync("invalid", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Section?)null);
    }

    /// <summary>
    /// Setup default test data for content repository (enables mocking mode)
    /// </summary>
    public void SetupDefaultContent()
    {
        if (!_useMocks)
        {
            _useMocks = true;
            MockSectionRepository = new Mock<ISectionRepository>().Object;
            MockContentRepository = new Mock<IContentRepository>().Object;
        }

        var content = new List<ContentItem>
        {
            new()
            { Slug = "2024-01-15-ai-news-1", Title = "AI News Article 1",
                Description = "AI news description",
                Author = "John Doe",
                DateEpoch = 1705276800, CollectionName = "news",
                SectionNames = ["ai"],
                Tags = ["ai", "news", "github copilot", "azure"],
                RenderedHtml = "<h1>AI News Article 1</h1><p>AI news content...</p>",
                Excerpt = "AI news excerpt...",
                ExternalUrl = "https://example.com/ai-news-1",
                ViewingMode = "external"
            },
            new()
            { Slug = "2024-01-16-ai-blog-1", Title = "AI Blog Article 1",
                Description = "AI blog description",
                Author = "Jane Smith",
                DateEpoch = 1705363200, CollectionName = "blogs",
                SectionNames = ["ai"],
                Tags = ["ai", "blogs", "machine learning"],
                RenderedHtml = "<h1>AI Blog Article 1</h1><p>AI blog content...</p>",
                Excerpt = "AI blog excerpt...",
                ExternalUrl = null,
                ViewingMode = "external"
            },
            new()
            { Slug = "2024-01-17-copilot-video-1", Title = "GitHub Copilot Video 1",
                Description = "Copilot video description",
                Author = "Bob Johnson",
                DateEpoch = 1705449600, CollectionName = "videos",
                SectionNames = ["github-copilot"],
                Tags = ["github copilot", "videos", "vs code", "productivity"],
                RenderedHtml = "<h1>GitHub Copilot Video 1</h1><p>Copilot video content...</p>",
                Excerpt = "Copilot video excerpt...",
                ExternalUrl = null,
                ViewingMode = "internal"
            },
            new()
            { Slug = "2024-01-18-copilot-news-1", Title = "GitHub Copilot News 1",
                Description = "Copilot news description",
                Author = "Alice Williams",
                DateEpoch = 1705536000, CollectionName = "news",
                SectionNames = ["github-copilot"],
                Tags = ["github copilot", "news", "github"],
                RenderedHtml = "<h1>GitHub Copilot News 1</h1><p>Copilot news content...</p>",
                Excerpt = "Copilot news excerpt...",
                ExternalUrl = "https://example.com/copilot-news-1",
                ViewingMode = "external"
            }
        };

        var mockRepo = Mock.Get(MockContentRepository!);

        // GetAllAsync
        mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(content);

        // GetByCollectionAsync
        mockRepo.Setup(r => r.GetByCollectionAsync("news", It.IsAny<CancellationToken>()))
            .ReturnsAsync([.. content.Where(c => c.CollectionName == "news")]);

        mockRepo.Setup(r => r.GetByCollectionAsync("blogs", It.IsAny<CancellationToken>()))
            .ReturnsAsync([.. content.Where(c => c.CollectionName == "blogs")]);

        mockRepo.Setup(r => r.GetByCollectionAsync("videos", It.IsAny<CancellationToken>()))
            .ReturnsAsync([.. content.Where(c => c.CollectionName == "videos")]);

        // GetBySectionAsync
        mockRepo.Setup(r => r.GetBySectionAsync("ai", It.IsAny<CancellationToken>()))
            .ReturnsAsync([.. content.Where(c => c.SectionNames.Contains("ai"))]);

        mockRepo.Setup(r => r.GetBySectionAsync("github-copilot", It.IsAny<CancellationToken>()))
            .ReturnsAsync([.. content.Where(c => c.SectionNames.Contains("github-copilot"))]);

        // GetAllTagsAsync - return normalized lowercase unique tags
        var allTags = content.SelectMany(c => c.Tags)
            .Select(t => t.ToLowerInvariant())
            .Distinct()
            .OrderBy(t => t)
            .ToList();
        mockRepo.Setup(r => r.GetAllTagsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(allTags);
    }
}
