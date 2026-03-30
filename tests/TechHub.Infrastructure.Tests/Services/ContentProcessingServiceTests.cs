using System.Data;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Integration tests for <see cref="ContentProcessingService"/>.
/// Uses a real PostgreSQL database for DB-hitting methods (ExistsAsync, WriteItemAsync, PurgeOldJobsAsync)
/// while mocking external services (RSS, AI, article fetching, YouTube tags).
/// </summary>
public class ContentProcessingServiceTests
    : IClassFixture<DatabaseFixture<ContentProcessingServiceTests>>
{
    private readonly DatabaseFixture<ContentProcessingServiceTests> _fixture;
    private readonly Mock<IRssFeedIngestionService> _rssService = new();
    private readonly Mock<IArticleContentService> _articleService = new();
    private readonly Mock<IAiCategorizationService> _aiService = new();
    private readonly Mock<IYouTubeTagService> _youtubeTagService = new();
    private readonly ContentProcessingJobRepository _jobRepo;
    private readonly ProcessedUrlRepository _processedUrlRepo;
    private readonly Mock<IRssFeedConfigRepository> _feedRepo = new();

    public ContentProcessingServiceTests(DatabaseFixture<ContentProcessingServiceTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
        _jobRepo = new ContentProcessingJobRepository(fixture.Connection, NullLogger<ContentProcessingJobRepository>.Instance);
        _processedUrlRepo = new ProcessedUrlRepository(fixture.Connection, NullLogger<ProcessedUrlRepository>.Instance);
    }

    private ContentProcessingService CreateService(ContentProcessorOptions? options = null)
    {
        var opts = options ?? new ContentProcessorOptions { Enabled = true };

        // EnrichWithContentAsync returns the item unchanged by default
        _articleService
            .Setup(s => s.EnrichWithContentAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((RawFeedItem item, CancellationToken _) => item);

        return new ContentProcessingService(
            _rssService.Object,
            _articleService.Object,
            _aiService.Object,
            _youtubeTagService.Object,
            _fixture.Connection,
            _jobRepo,
            _processedUrlRepo,
            _feedRepo.Object,
            TimeProvider.System,
            Options.Create(opts),
            NullLogger<ContentProcessingService>.Instance);
    }

    private static RawFeedItem CreateRawItem(string url = "https://example.com/article-1", string title = "Test Article") => new()
    {
        Title = title,
        ExternalUrl = url,
        PublishedAt = DateTimeOffset.UtcNow,
        FeedName = "Test Feed",
        CollectionName = "blogs"
    };

    private static ProcessedContentItem CreateProcessedItem(string url = "https://example.com/article-1", string slug = "test-article") => new()
    {
        Slug = slug,
        Title = "Test Article",
        Excerpt = "A test excerpt",
        DateEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        CollectionName = "blogs",
        ExternalUrl = url,
        FeedName = "Test Feed",
        ContentHash = "abc123",
        Sections = ["ai"],
        PrimarySectionName = "ai",
        Tags = ["csharp", "dotnet"]
    };

    // ── Job Lifecycle ──────────────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_CreatesAndCompletesJob()
    {
        // Arrange
        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — check the DB for a completed job
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs.Should().NotBeEmpty();
        var job = jobs[0];
        job.Status.Should().Be("completed");
        job.TriggerType.Should().Be("scheduled");
    }

    [Fact]
    public async Task RunAsync_WhenDisabledAndScheduled_SkipsProcessing()
    {
        // Arrange
        var sut = CreateService(new ContentProcessorOptions { Enabled = false });

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — should not fetch feeds
        _feedRepo.Verify(r => r.GetEnabledAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RunAsync_WhenDisabledAndManual_ProcessesFeeds()
    {
        // Arrange
        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var sut = CreateService(new ContentProcessorOptions { Enabled = false });

        // Act
        await sut.RunAsync("manual", CancellationToken.None);

        // Assert — should fetch feeds even when disabled
        _feedRepo.Verify(r => r.GetEnabledAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── Dedup Logic ────────────────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_SkipsItemsAlreadyProcessed()
    {
        // Arrange — pre-record a URL as processed
        const string url = "https://example.com/already-processed-dedup";
        await _processedUrlRepo.RecordSuccessAsync(url, ct: CancellationToken.None);

        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var rawItem = CreateRawItem(url);

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync([rawItem]);

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — AI should never be called for already-processed items
        _aiService.Verify(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // ── AI Categorization Flow ─────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_NewItem_GoesThroughFullPipeline()
    {
        // Arrange
        const string url = "https://example.com/new-pipeline-item";
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var rawItem = CreateRawItem(url);
        var processed = CreateProcessedItem(url, "new-pipeline-item");

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync([rawItem]);
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included: relevant content" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — full pipeline: enrich → categorize → DB write
        _articleService.Verify(s => s.EnrichWithContentAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Once);
        _aiService.Verify(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Once);
        // URL should now be recorded in processed_urls
        var exists = await _processedUrlRepo.ExistsAsync(url, CancellationToken.None);
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task RunAsync_WhenAiReturnsNull_SkipsItemAndRecordsSkipped()
    {
        // Arrange
        const string url = "https://example.com/ai-skip-item";
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var rawItem = CreateRawItem(url);

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync([rawItem]);
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = null, Explanation = "Content excluded: not relevant" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — URL recorded as skipped (AI skip is not an error, but distinct from success)
        var exists = await _processedUrlRepo.ExistsAsync(url, CancellationToken.None);
        exists.Should().BeTrue();

        var result = await _processedUrlRepo.GetPagedAsync(0, 10, status: "skipped", search: url, ct: CancellationToken.None);
        result.Items.Should().ContainSingle();
        result.Items[0].Status.Should().Be("skipped");
    }

    // ── Error Handling ─────────────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_WhenAiThrowsHttpException_RecordsFailureAndContinues()
    {
        // Arrange
        var testId = Guid.NewGuid().ToString("N")[..8];
        var failUrl = $"https://example.com/ai-fail-{testId}";
        var okUrl = $"https://example.com/ai-ok-{testId}";
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var item1 = CreateRawItem(failUrl);
        var item2 = CreateRawItem(okUrl, "Good Article");
        var processed2 = CreateProcessedItem(okUrl, $"ai-ok-{testId}");

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync([item1, item2]);

        _aiService.Setup(s => s.CategorizeAsync(
            It.Is<RawFeedItem>(i => i.ExternalUrl == failUrl), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("API is down"));
        _aiService.Setup(s => s.CategorizeAsync(
            It.Is<RawFeedItem>(i => i.ExternalUrl == okUrl), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed2, Explanation = "Included" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — failed URL recorded as failure, good URL as success
        var failRecord = await _processedUrlRepo.GetAsync(failUrl, CancellationToken.None);
        failRecord.Should().NotBeNull();
        failRecord!.Status.Should().Be("failed");

        var okRecord = await _processedUrlRepo.GetAsync(okUrl, CancellationToken.None);
        okRecord.Should().NotBeNull();
        okRecord!.Status.Should().Be("succeeded");
    }

    [Fact]
    public async Task RunAsync_WhenFeedLoadFails_FailsJob()
    {
        // Arrange
        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB connection lost"));

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — job should be marked as failed
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs.Should().NotBeEmpty();
        jobs[0].Status.Should().Be("failed");
    }

    // ── MaxItemsPerRun ────────────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_RespectsMaxItemsPerRun()
    {
        // Arrange
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var items = Enumerable.Range(1, 5)
            .Select(i => CreateRawItem($"https://example.com/max-limit-{i}", $"Article {i}"))
            .ToList();

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(items);
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((RawFeedItem r, CancellationToken _) => new CategorizationResult { Item = CreateProcessedItem(r.ExternalUrl, $"max-limit-{r.Title.Split(' ').Last()}"), Explanation = "Included" });

        var sut = CreateService(new ContentProcessorOptions { Enabled = true, MaxItemsPerRun = 2 });

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — only 2 items should be categorized
        _aiService.Verify(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    // ── Multiple Feeds ────────────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_ProcessesMultipleFeeds()
    {
        // Arrange
        var feed1 = new FeedConfig { Id = 1, Name = "Feed A", Url = "https://a.com/feed", OutputDir = "_blogs", Enabled = true };
        var feed2 = new FeedConfig { Id = 2, Name = "Feed B", Url = "https://b.com/feed", OutputDir = "_news", Enabled = true };
        var item1 = CreateRawItem("https://example.com/multi-feed-a");
        var item2 = CreateRawItem("https://example.com/multi-feed-b");

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed1, feed2]);
        _rssService.Setup(r => r.IngestAsync(feed1, It.IsAny<CancellationToken>())).ReturnsAsync([item1]);
        _rssService.Setup(r => r.IngestAsync(feed2, It.IsAny<CancellationToken>())).ReturnsAsync([item2]);
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((RawFeedItem r, CancellationToken _) => new CategorizationResult { Item = CreateProcessedItem(r.ExternalUrl, $"multi-feed-{Guid.NewGuid():N}"), Explanation = "Included" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — both items should be categorized
        _aiService.Verify(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    // ── Log Flushing ──────────────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_CompletedJob_HasLogOutput()
    {
        // Arrange
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — job should have log content
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs.Should().NotBeEmpty();
        jobs[0].LogOutput.Should().NotBeNullOrWhiteSpace();
        jobs[0].LogOutput.Should().Contain("Starting content processing run");
    }

    // ── YouTube Tag Merging ────────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_YouTubeItem_MergesTagsFromService()
    {
        // Arrange
        var feed = new FeedConfig { Id = 1, Name = "YT", Url = "https://youtube.com/feed", OutputDir = "_videos", Enabled = true };
        var ytItem = new RawFeedItem
        {
            Title = "Video",
            ExternalUrl = "https://youtube.com/watch?v=yt-merge-test",
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "YT",
            CollectionName = "videos",
            FeedTags = ["existing-tag"]
        };

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync([ytItem]);
        _youtubeTagService.Setup(s => s.GetTagsAsync(ytItem.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string> { "yt-tag-1", "yt-tag-2" });
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = CreateProcessedItem(ytItem.ExternalUrl, "yt-merge-test"), Explanation = "Included" });

        var sut = CreateService(new ContentProcessorOptions { Enabled = true, MaxYouTubeTagCount = 10 });

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — AI should receive merged tags (existing + YouTube)
        _aiService.Verify(s => s.CategorizeAsync(
            It.Is<RawFeedItem>(r => r.FeedTags.Count == 3), // existing-tag + yt-tag-1 + yt-tag-2
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
