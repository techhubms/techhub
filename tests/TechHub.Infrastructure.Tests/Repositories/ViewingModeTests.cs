using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
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
                Sections = [], // Required but not used in these tests
                Timezone = "Europe/Brussels"
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

        // Setup: Create mock ITagMatchingService
        var mockTagMatchingService = new Mock<ITagMatchingService>();
        mockTagMatchingService.Setup(s => s.Normalize(It.IsAny<string>()))
            .Returns((string tag) => tag.ToLowerInvariant());
        mockTagMatchingService.Setup(s => s.Matches(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);
        mockTagMatchingService.Setup(s => s.MatchesAny(It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
            .Returns(true);

        _repository = new FileBasedContentRepository(options, markdownService, mockTagMatchingService.Object, mockEnvironment.Object);
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
section_names: ["ml"]
tags: ["test"]
external_url: "https://youtube.com/watch?v=abc123"
viewing_mode: "internal"
---
Test video content
""";

        await File.WriteAllTextAsync(Path.Combine(videosDir, "2024-01-15-test-video.md"), videoContent);

        // Act
        var items = await _repository.GetAllAsync();
        var video = items.FirstOrDefault();

        // Assert
        video.Should().NotBeNull();
        video!.Title.Should().Be("Internal Video");
        video!.ViewingMode.Should().Be("internal");
        video!.ExternalUrl.Should().Be("https://youtube.com/watch?v=abc123"); // URL is always set, ViewingMode controls usage
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
section_names: ["ml"]
tags: ["test"]
external_url: "https://youtube.com/watch?v=xyz789"
viewing_mode: "external"
---
Test video content
""";

        await File.WriteAllTextAsync(Path.Combine(videosDir, "2024-01-15-external-video.md"), videoContent);

        // Act
        var items = await _repository.GetAllAsync();
        var video = items.FirstOrDefault();

        // Assert
        video.Should().NotBeNull();
        video!.Title.Should().Be("External Video");
        video!.ViewingMode.Should().Be("external");
        video!.ExternalUrl.Should().Be("https://youtube.com/watch?v=xyz789");
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
section_names: ["ml"]
tags: ["test"]
external_url: "https://youtube.com/watch?v=def456"
---
Test video content
""";

        await File.WriteAllTextAsync(Path.Combine(videosDir, "2024-01-15-default-video.md"), videoContent);

        // Act
        var items = await _repository.GetAllAsync();
        var video = items.FirstOrDefault();

        // Assert
        video.Should().NotBeNull();
        video.Title.Should().Be("Default Video");
        video.ViewingMode.Should().Be("external"); // Defaults to 'external' when not specified
        video.ExternalUrl.Should().Be("https://youtube.com/watch?v=def456");
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
