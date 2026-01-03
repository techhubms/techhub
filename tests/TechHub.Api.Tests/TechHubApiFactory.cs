using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Api.Tests;

/// <summary>
/// Custom WebApplicationFactory for API integration tests with mocked dependencies
/// </summary>
public class TechHubApiFactory : WebApplicationFactory<Program>
{
    public ISectionRepository MockSectionRepository { get; } = Substitute.For<ISectionRepository>();
    public IContentRepository MockContentRepository { get; } = Substitute.For<IContentRepository>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
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
            services.AddSingleton(MockSectionRepository);
            services.AddSingleton(MockContentRepository);
        });

        // Suppress verbose logging during tests
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Critical);
        });

        builder.UseEnvironment("Test");
    }

    /// <summary>
    /// Setup default test data for section repository
    /// </summary>
    public void SetupDefaultSections()
    {
        var sections = new List<Section>
        {
            new()
            {
                Id = "ai",
                Title = "AI",
                Description = "Artificial Intelligence resources",
                Url = "/ai",
                Category = "AI",
                BackgroundImage = "/assets/section-backgrounds/ai.jpg",
                Collections = new List<CollectionReference>
                {
                    new() { Title = "News", Collection = "news", Url = "/ai/news.html", Description = "AI News", IsCustom = false },
                    new() { Title = "Blogs", Collection = "blogs", Url = "/ai/blogs.html", Description = "AI Blogs", IsCustom = false }
                }
            },
            new()
            {
                Id = "github-copilot",
                Title = "GitHub Copilot",
                Description = "GitHub Copilot resources",
                Url = "/github-copilot",
                Category = "GitHub Copilot",
                BackgroundImage = "/assets/section-backgrounds/github-copilot.jpg",
                Collections = new List<CollectionReference>
                {
                    new() { Title = "News", Collection = "news", Url = "/github-copilot/news.html", Description = "GitHub Copilot News", IsCustom = false },
                    new() { Title = "Videos", Collection = "videos", Url = "/github-copilot/videos.html", Description = "GitHub Copilot Videos", IsCustom = false }
                }
            }
        };

        MockSectionRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<Section>>(sections));

        MockSectionRepository.GetByIdAsync("ai", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Section?>(sections[0]));

        MockSectionRepository.GetByIdAsync("github-copilot", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Section?>(sections[1]));

        MockSectionRepository.GetByIdAsync("invalid", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Section?>(null));
    }

    /// <summary>
    /// Setup default test data for content repository
    /// </summary>
    public void SetupDefaultContent()
    {
        var content = new List<ContentItem>
        {
            new()
            {
                Id = "2024-01-15-ai-news-1",
                Title = "AI News Article 1",
                Description = "AI news description",
                Author = "John Doe",
                DateEpoch = 1705276800,
                Collection = "news",
                AltCollection = null,
                Categories = new List<string> { "AI" },
                Tags = new List<string> { "ai", "copilot", "azure" },
                RenderedHtml = "<h1>AI News Article 1</h1><p>AI news content...</p>",
                Excerpt = "AI news excerpt...",
                ExternalUrl = "https://example.com/ai-news-1",
                VideoId = null
            },
            new()
            {
                Id = "2024-01-16-ai-blog-1",
                Title = "AI Blog Article 1",
                Description = "AI blog description",
                Author = "Jane Smith",
                DateEpoch = 1705363200,
                Collection = "blogs",
                AltCollection = null,
                Categories = new List<string> { "AI" },
                Tags = new List<string> { "ai", "machine-learning" },
                RenderedHtml = "<h1>AI Blog Article 1</h1><p>AI blog content...</p>",
                Excerpt = "AI blog excerpt...",
                ExternalUrl = null,
                VideoId = null
            },
            new()
            {
                Id = "2024-01-17-copilot-video-1",
                Title = "GitHub Copilot Video 1",
                Description = "Copilot video description",
                Author = "Bob Johnson",
                DateEpoch = 1705449600,
                Collection = "videos",
                AltCollection = null,
                Categories = new List<string> { "GitHub Copilot" },
                Tags = new List<string> { "copilot", "vscode", "productivity" },
                RenderedHtml = "<h1>GitHub Copilot Video 1</h1><p>Copilot video content...</p>",
                Excerpt = "Copilot video excerpt...",
                ExternalUrl = null,
                VideoId = "abc123"
            },
            new()
            {
                Id = "2024-01-18-copilot-news-1",
                Title = "GitHub Copilot News 1",
                Description = "Copilot news description",
                Author = "Alice Williams",
                DateEpoch = 1705536000,
                Collection = "news",
                AltCollection = null,
                Categories = new List<string> { "GitHub Copilot" },
                Tags = new List<string> { "copilot", "github" },
                RenderedHtml = "<h1>GitHub Copilot News 1</h1><p>Copilot news content...</p>",
                Excerpt = "Copilot news excerpt...",
                ExternalUrl = "https://example.com/copilot-news-1",
                VideoId = null
            }
        };

        // GetAllAsync
        MockContentRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ContentItem>>(content));

        // GetByCollectionAsync
        MockContentRepository.GetByCollectionAsync("news", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ContentItem>>(
                content.Where(c => c.Collection == "news").ToList()));

        MockContentRepository.GetByCollectionAsync("blogs", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ContentItem>>(
                content.Where(c => c.Collection == "blogs").ToList()));

        MockContentRepository.GetByCollectionAsync("videos", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ContentItem>>(
                content.Where(c => c.Collection == "videos").ToList()));

        // GetByCategoryAsync
        MockContentRepository.GetByCategoryAsync("AI", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ContentItem>>(
                content.Where(c => c.Categories.Contains("AI")).ToList()));

        MockContentRepository.GetByCategoryAsync("GitHub Copilot", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ContentItem>>(
                content.Where(c => c.Categories.Contains("GitHub Copilot")).ToList()));

        // GetAllTagsAsync
        var allTags = content.SelectMany(c => c.Tags).Distinct().ToList();
        MockContentRepository.GetAllTagsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<string>>(allTags));
    }
}
