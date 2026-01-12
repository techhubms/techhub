using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
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

        // Configure logging to file only (no console output)
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            var logPath = Path.Combine(Path.GetTempPath(), "techhub-tests", "api-integration.log");
            logging.AddProvider(new FileLoggerProvider(logPath));
        });

        builder.UseEnvironment("Test");
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
                AltCollection = null,
                Sections = ["AI"],
                Tags = ["ai", "copilot", "azure"],
                RenderedHtml = "<h1>AI News Article 1</h1><p>AI news content...</p>",
                Excerpt = "AI news excerpt...",
                ExternalUrl = "https://example.com/ai-news-1",
                VideoId = null
            },
            new()
            { Slug = "2024-01-16-ai-blog-1", Title = "AI Blog Article 1",
                Description = "AI blog description",
                Author = "Jane Smith",
                DateEpoch = 1705363200, CollectionName = "blogs",
                AltCollection = null,
                Sections = ["AI"],
                Tags = ["ai", "machine-learning"],
                RenderedHtml = "<h1>AI Blog Article 1</h1><p>AI blog content...</p>",
                Excerpt = "AI blog excerpt...",
                ExternalUrl = null,
                VideoId = null
            },
            new()
            { Slug = "2024-01-17-copilot-video-1", Title = "GitHub Copilot Video 1",
                Description = "Copilot video description",
                Author = "Bob Johnson",
                DateEpoch = 1705449600, CollectionName = "videos",
                AltCollection = null,
                Sections = ["GitHub Copilot"],
                Tags = ["copilot", "vscode", "productivity"],
                RenderedHtml = "<h1>GitHub Copilot Video 1</h1><p>Copilot video content...</p>",
                Excerpt = "Copilot video excerpt...",
                ExternalUrl = null,
                VideoId = "abc123"
            },
            new()
            { Slug = "2024-01-18-copilot-news-1", Title = "GitHub Copilot News 1",
                Description = "Copilot news description",
                Author = "Alice Williams",
                DateEpoch = 1705536000, CollectionName = "news",
                AltCollection = null,
                Sections = ["GitHub Copilot"],
                Tags = ["copilot", "github"],
                RenderedHtml = "<h1>GitHub Copilot News 1</h1><p>Copilot news content...</p>",
                Excerpt = "Copilot news excerpt...",
                ExternalUrl = "https://example.com/copilot-news-1",
                VideoId = null
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
        mockRepo.Setup(r => r.GetBySectionAsync("AI", It.IsAny<CancellationToken>()))
            .ReturnsAsync([.. content.Where(c => c.Sections.Contains("AI"))]);

        mockRepo.Setup(r => r.GetBySectionAsync("GitHub Copilot", It.IsAny<CancellationToken>()))
            .ReturnsAsync([.. content.Where(c => c.Sections.Contains("GitHub Copilot"))]);

        // GetAllTagsAsync
        var allTags = content.SelectMany(c => c.Tags).Distinct().ToList();
        mockRepo.Setup(r => r.GetAllTagsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(allTags);
    }
}

/// <summary>
/// Simple file logger provider for test server logs
/// </summary>
file class FileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;
    private readonly StreamWriter _writer;
    private readonly object _lock = new();

    public FileLoggerProvider(string filePath)
    {
        _filePath = filePath;
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        _writer = new StreamWriter(_filePath, append: true) { AutoFlush = true };
    }

    public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, _writer, _lock);

    public void Dispose()
    {
        _writer.Dispose();
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Simple file logger for test server
/// </summary>
file class FileLogger(string categoryName, StreamWriter writer, object lockObj) : ILogger
{
    private readonly string _categoryName = categoryName;
    private readonly StreamWriter _writer = writer;
    private readonly object _lock = lockObj;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        lock (_lock)
        {
            _writer.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] [{logLevel}] {_categoryName}: {formatter(state, exception)}");
            if (exception != null)
            {
                _writer.WriteLine(exception.ToString());
            }
        }
    }
}
