using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;

namespace TechHub.Infrastructure.Tests.Repositories;

public class ViewingModeTests : IDisposable
{
    private readonly FileBasedContentRepository _repository;
    private readonly string _testDir;

    public ViewingModeTests()
    {
        _testDir = Path.Combine(Path.GetTempPath(), $"techhub-test-{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDir);

        var appSettings = new AppSettings
        {
            Content = new ContentSettings
            {
                CollectionsPath = _testDir,
                Timezone = "Europe/Brussels",
                MaxExcerptLength = 1000,
                Sections = [] // Required but not used in these tests
            },
            Caching = new CachingSettings(),
            Seo = new SeoSettings
            {
                BaseUrl = "https://test.example.com",
                SiteTitle = "Test Site",
                SiteDescription = "Test Description"
            },
            Performance = new PerformanceSettings()
        };

        var options = Options.Create(appSettings);
        var markdownService = new MarkdownService();

        // Setup: Create mock IHostEnvironment
        var mockEnvironment = new Mock<IHostEnvironment>();
        mockEnvironment.Setup(e => e.ContentRootPath).Returns(_testDir);

        _repository = new FileBasedContentRepository(options, markdownService, mockEnvironment.Object);
    }

    [Fact]
    public async Task Video_WithViewingModeInternal_ShouldHaveExternalUrlButInternalMode()
    {
        // Arrange: Create video with viewing_mode: internal
        var videosDir = Path.Combine(_testDir, "_videos");
        Directory.CreateDirectory(videosDir);

        var videoContent = """
---
title: "Internal Video"
date: 2024-01-15
categories: ["ML"]
tags: ["test"]
canonical_url: "https://youtube.com/watch?v=abc123"
viewing_mode: "internal"
---
Test video content
""";

        await File.WriteAllTextAsync(Path.Combine(videosDir, "2024-01-15-test-video.md"), videoContent);

        // Act
        var items = await _repository.GetAllAsync();
        var video = items.FirstOrDefault();

        // Assert
        Assert.NotNull(video);
        Assert.Equal("Internal Video", video.Title);
        Assert.Equal("internal", video.ViewingMode);
        Assert.Equal("https://youtube.com/watch?v=abc123", video.ExternalUrl); // URL is always set, ViewingMode controls usage
    }

    [Fact]
    public async Task Video_WithViewingModeInternal_ShouldHaveExternalUrlAndInternalMode()
    {
        // Arrange: Create video with viewing_mode: external
        var videosDir = Path.Combine(_testDir, "_videos");
        Directory.CreateDirectory(videosDir);

        var videoContent = """
---
title: "External Video"
date: 2024-01-15
categories: ["ML"]
tags: ["test"]
canonical_url: "https://youtube.com/watch?v=xyz789"
viewing_mode: "external"
---
Test video content
""";

        await File.WriteAllTextAsync(Path.Combine(videosDir, "2024-01-15-external-video.md"), videoContent);

        // Act
        var items = await _repository.GetAllAsync();
        var video = items.FirstOrDefault();

        // Assert
        Assert.NotNull(video);
        Assert.Equal("External Video", video.Title);
        Assert.Equal("external", video.ViewingMode);
        Assert.Equal("https://youtube.com/watch?v=xyz789", video.ExternalUrl);
    }

    [Fact]
    public async Task Video_WithoutViewingMode_ShouldHaveExternalUrl()
    {
        // Arrange: Create video without viewing_mode (default behavior)
        var videosDir = Path.Combine(_testDir, "_videos");
        Directory.CreateDirectory(videosDir);

        var videoContent = """
---
title: "Default Video"
date: 2024-01-15
categories: ["ML"]
tags: ["test"]
canonical_url: "https://youtube.com/watch?v=def456"
---
Test video content
""";

        await File.WriteAllTextAsync(Path.Combine(videosDir, "2024-01-15-default-video.md"), videoContent);

        // Act
        var items = await _repository.GetAllAsync();
        var video = items.FirstOrDefault();

        // Assert
        Assert.NotNull(video);
        Assert.Equal("Default Video", video.Title);
        Assert.Equal("external", video.ViewingMode); // Defaults to 'external' when not specified
        Assert.Equal("https://youtube.com/watch?v=def456", video.ExternalUrl);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDir))
        {
            Directory.Delete(_testDir, true);
        }

        _repository.Dispose();
        GC.SuppressFinalize(this);
    }
}
