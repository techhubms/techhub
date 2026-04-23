using System.Data;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models.ContentProcessing;
using TechHub.Infrastructure.Data;
using TechHub.Infrastructure.Repositories;
using TechHub.Infrastructure.Services.ContentProcessing;
using TechHub.TestUtilities;

namespace TechHub.Infrastructure.Tests.Services;

/// <summary>
/// Integration tests for <see cref="ContentProcessingService"/>.
/// Uses a real PostgreSQL database for DB-hitting methods via repositories
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
    private readonly Mock<IContentFixerService> _contentFixer = new();
    private readonly ContentProcessingJobRepository _jobRepo;
    private readonly ProcessedUrlRepository _processedUrlRepo;
    private readonly ContentItemWriteRepository _writeRepo;
    private readonly Mock<IRssFeedConfigRepository> _feedRepo = new();

    public ContentProcessingServiceTests(DatabaseFixture<ContentProcessingServiceTests> fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
        _jobRepo = new ContentProcessingJobRepository(fixture.Connection, new PostgresConnectionFactory(fixture.ConnectionString), NullLogger<ContentProcessingJobRepository>.Instance);
        _processedUrlRepo = new ProcessedUrlRepository(fixture.Connection, NullLogger<ProcessedUrlRepository>.Instance);
        _writeRepo = new ContentItemWriteRepository(fixture.Connection, NullLogger<ContentItemWriteRepository>.Instance);
    }

    private ContentProcessingService CreateService(ContentProcessorOptions? options = null)
    {
        var opts = options ?? new ContentProcessorOptions();

        // EnrichWithContentAsync returns the item unchanged by default
        _articleService
            .Setup(s => s.EnrichWithContentAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((RawFeedItem item, CancellationToken _) => item);

        // RepairMarkdown returns content unchanged
        _contentFixer
            .Setup(s => s.RepairMarkdown(It.IsAny<string>()))
            .Returns((string content) => content);

        return new ContentProcessingService(
            _rssService.Object,
            _articleService.Object,
            _aiService.Object,
            _youtubeTagService.Object,
            _writeRepo,
            _jobRepo,
            _processedUrlRepo,
            _feedRepo.Object,
            _contentFixer.Object,
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
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));

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
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
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
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
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

    [Fact]
    public async Task RunAsync_WhenCategorizationFails_RecordsFailureNotSkipped()
    {
        // Arrange — AI returns a failure (e.g., empty response) rather than a legitimate skip
        const string url = "https://example.com/ai-failure-item";
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var rawItem = CreateRawItem(url);

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = null, Explanation = "AI returned empty response", IsFailure = true });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — URL recorded as failed, not skipped
        var exists = await _processedUrlRepo.ExistsAsync(url, CancellationToken.None);
        exists.Should().BeTrue();

        var result = await _processedUrlRepo.GetPagedAsync(0, 10, status: "failed", search: url, ct: CancellationToken.None);
        result.Items.Should().ContainSingle();
        result.Items[0].Status.Should().Be("failed");
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
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([item1, item2]));

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

    [Fact]
    public async Task RunAsync_WhenFeedDownloadFails_CountsAsError()
    {
        // Arrange — feed ingestion returns a failure (e.g., HTTP 404 or timeout)
        var feed = new FeedConfig { Id = 1, Name = "Broken Feed", Url = "https://example.com/broken", OutputDir = "_blogs", Enabled = true };

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>()))
            .ReturnsAsync(FeedIngestionResult.Failure("Failed to download feed from https://example.com/broken"));

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — job should complete with errorCount >= 1
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs.Should().NotBeEmpty();
        jobs[0].Status.Should().Be("completed");
        jobs[0].ErrorCount.Should().BeGreaterThanOrEqualTo(1);
        var jobDetail = await _jobRepo.GetByIdAsync(jobs[0].Id, CancellationToken.None);
        jobDetail!.LogOutput.Should().Contain("Feed error");
        jobDetail.LogOutput.Should().Contain("Broken Feed");
    }

    [Fact]
    public async Task RunAsync_WhenFeedParseFails_CountsAsErrorAndContinuesNextFeed()
    {
        // Arrange — first feed fails, second succeeds
        var brokenFeed = new FeedConfig { Id = 1, Name = "Broken", Url = "https://example.com/broken", OutputDir = "_blogs", Enabled = true };
        var goodFeed = new FeedConfig { Id = 2, Name = "Good", Url = "https://example.com/good", OutputDir = "_news", Enabled = true };
        var testId = Guid.NewGuid().ToString("N")[..8];
        var goodUrl = $"https://example.com/good-item-{testId}";
        var goodItem = CreateRawItem(goodUrl);
        var processed = CreateProcessedItem(goodUrl, $"good-item-{testId}");

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([brokenFeed, goodFeed]);
        _rssService.Setup(r => r.IngestAsync(brokenFeed, It.IsAny<CancellationToken>()))
            .ReturnsAsync(FeedIngestionResult.Failure("Failed to parse feed: invalid XML"));
        _rssService.Setup(r => r.IngestAsync(goodFeed, It.IsAny<CancellationToken>()))
            .ReturnsAsync(FeedIngestionResult.Success([goodItem]));
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — job completed, error counted for broken feed, good item still processed
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs.Should().NotBeEmpty();
        jobs[0].Status.Should().Be("completed");
        jobs[0].ErrorCount.Should().BeGreaterThanOrEqualTo(1);
        jobs[0].ItemsAdded.Should().BeGreaterThanOrEqualTo(1);
        jobs[0].FeedsProcessed.Should().Be(2);
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
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success(items));
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((RawFeedItem r, CancellationToken _) => new CategorizationResult { Item = CreateProcessedItem(r.ExternalUrl, $"max-limit-{r.Title.Split(' ').Last()}"), Explanation = "Included" });

        var sut = CreateService(new ContentProcessorOptions { MaxItemsPerRun = 2 });

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
        _rssService.Setup(r => r.IngestAsync(feed1, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([item1]));
        _rssService.Setup(r => r.IngestAsync(feed2, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([item2]));
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
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([]));

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — job should have log content
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs.Should().NotBeEmpty();
        var jobDetail = await _jobRepo.GetByIdAsync(jobs[0].Id, CancellationToken.None);
        jobDetail!.LogOutput.Should().NotBeNullOrWhiteSpace();
        jobDetail.LogOutput.Should().Contain("Starting content processing run");
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
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([ytItem]));
        _youtubeTagService.Setup(s => s.GetTagsAsync(ytItem.ExternalUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string> { "yt-tag-1", "yt-tag-2" });
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = CreateProcessedItem(ytItem.ExternalUrl, "yt-merge-test"), Explanation = "Included" });

        var sut = CreateService(new ContentProcessorOptions { MaxYouTubeTagCount = 10 });

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — AI should receive merged tags (existing + YouTube)
        _aiService.Verify(s => s.CategorizeAsync(
            It.Is<RawFeedItem>(r => r.FeedTags.Count == 3), // existing-tag + yt-tag-1 + yt-tag-2
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── DB Write Verification ─────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_NewItem_WritesContentItemWithCorrectSectionsAndBitmask()
    {
        // Arrange
        const string url = "https://example.com/verify-db-write";
        const string slug = "verify-db-write";
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var rawItem = CreateRawItem(url);
        var processed = new ProcessedContentItem
        {
            Slug = slug,
            Title = "Verify DB Write",
            Content = "Full content here",
            Excerpt = "Short excerpt",
            DateEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            CollectionName = "blogs",
            ExternalUrl = url,
            FeedName = "Test Feed",
            ContentHash = "abc123",
            Sections = ["ai", "azure", "dotnet"],
            PrimarySectionName = "ai",
            Tags = ["csharp", "azure-openai"]
        };

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — verify the row in content_items
        var title = await _fixture.Connection.QueryFirstOrDefaultAsync<string>(
            "SELECT title FROM content_items WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = slug, Collection = "blogs" });
        title.Should().NotBeNull("WriteItemAsync should have inserted the content item");
        title.Should().Be("Verify DB Write");

        var isAi = await _fixture.Connection.QueryFirstAsync<bool>(
            "SELECT is_ai FROM content_items WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = slug, Collection = "blogs" });
        isAi.Should().BeTrue();

        var isAzure = await _fixture.Connection.QueryFirstAsync<bool>(
            "SELECT is_azure FROM content_items WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = slug, Collection = "blogs" });
        isAzure.Should().BeTrue();

        var isDotnet = await _fixture.Connection.QueryFirstAsync<bool>(
            "SELECT is_dotnet FROM content_items WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = slug, Collection = "blogs" });
        isDotnet.Should().BeTrue();

        var isDevops = await _fixture.Connection.QueryFirstAsync<bool>(
            "SELECT is_devops FROM content_items WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = slug, Collection = "blogs" });
        isDevops.Should().BeFalse();

        var bitmask = await _fixture.Connection.QueryFirstAsync<int>(
            "SELECT sections_bitmask FROM content_items WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = slug, Collection = "blogs" });
        bitmask.Should().Be(1 | 2 | 4); // ai=1, azure=2, dotnet=4

        var tagsCsv = await _fixture.Connection.QueryFirstAsync<string>(
            "SELECT tags_csv FROM content_items WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = slug, Collection = "blogs" });
        tagsCsv.Should().Be(",C#,Azure OpenAI,AI,Azure,.NET,Blogs,");

        var primarySection = await _fixture.Connection.QueryFirstAsync<string>(
            "SELECT primary_section_name FROM content_items WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = slug, Collection = "blogs" });
        primarySection.Should().Be("ai");
    }

    [Fact]
    public async Task RunAsync_NewItem_WritesAiMetadataToDatabase()
    {
        // Arrange
        const string url = "https://example.com/verify-ai-metadata";
        const string slug = "verify-ai-metadata";
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var rawItem = CreateRawItem(url);
        var processed = new ProcessedContentItem
        {
            Slug = slug,
            Title = "AI Metadata Write",
            Excerpt = "Testing",
            DateEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            CollectionName = "news",
            ExternalUrl = url,
            FeedName = "Test Feed",
            ContentHash = "meta123",
            Sections = ["ai"],
            PrimarySectionName = "ai",
            Tags = [],
            RoundupMetadata = new RoundupMetadata
            {
                Summary = "AI metadata test summary",
                KeyTopics = ["testing", "metadata"],
                Relevance = "high",
                TopicType = "announcement",
                ImpactLevel = "high",
                TimeSensitivity = "immediate"
            }
        };

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — verify ai_metadata JSONB column
        var metaJson = await _fixture.Connection.QueryFirstOrDefaultAsync<string>(
            "SELECT ai_metadata::text FROM content_items WHERE slug = @Slug AND collection_name = @Collection",
            new { Slug = slug, Collection = "news" });

        metaJson.Should().NotBeNullOrWhiteSpace();
        using var doc = System.Text.Json.JsonDocument.Parse(metaJson!);
        doc.RootElement.GetProperty("roundup_summary").GetString().Should().Be("AI metadata test summary");
        doc.RootElement.GetProperty("roundup_relevance").GetString().Should().Be("high");
        doc.RootElement.GetProperty("topic_type").GetString().Should().Be("announcement");
        doc.RootElement.GetProperty("key_topics").GetArrayLength().Should().Be(2);
    }

    // ── Tag Expansion Verification ────────────────────────────────────────

    [Fact]
    public async Task RunAsync_NewItem_PopulatesContentTagsExpanded()
    {
        // Arrange
        const string url = "https://example.com/verify-tags-expanded";
        const string slug = "verify-tags-expanded";
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var rawItem = CreateRawItem(url);
        var processed = new ProcessedContentItem
        {
            Slug = slug,
            Title = "Tag Expansion Test",
            Excerpt = "Testing",
            DateEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            CollectionName = "blogs",
            ExternalUrl = url,
            FeedName = "Test Feed",
            ContentHash = "tags123",
            Sections = ["ai"],
            PrimarySectionName = "ai",
            Tags = ["azure openai", "csharp"]
        };

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — verify content_tags_expanded has full tags + word expansions
        var tags = (await _fixture.Connection.QueryAsync<string>(
            "SELECT tag_word FROM content_tags_expanded WHERE slug = @Slug AND collection_name = @Collection ORDER BY tag_word",
            new { Slug = slug, Collection = "blogs" })).ToList();

        // "azure-openai" → normalized to "Azure OpenAI" → tag_word="azure openai" + "azure" + "openai" word expansions
        // "csharp" → normalized to "C#" → tag_word="c#" (single word, no expansion)
        tags.Should().Contain("azure openai");
        tags.Should().Contain("azure");
        tags.Should().Contain("openai");
        tags.Should().Contain("c#");
    }

    [Fact]
    public async Task RunAsync_NewItem_TagExpansionHasDenormalizedSectionFlags()
    {
        // Arrange
        const string url = "https://example.com/verify-tag-sections";
        const string slug = "verify-tag-sections";
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var rawItem = CreateRawItem(url);
        var processed = new ProcessedContentItem
        {
            Slug = slug,
            Title = "Tag Section Flags",
            Excerpt = "Testing",
            DateEpoch = 1700000000,
            CollectionName = "blogs",
            ExternalUrl = url,
            FeedName = "Test Feed",
            ContentHash = "tagsec123",
            Sections = ["ai", "security"],
            PrimarySectionName = "ai",
            Tags = ["zero-trust"]
        };

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — tag row should have the same section flags as the content item
        // Note: TagNormalizer converts "zero-trust" → "Zero Trust" (hyphens to spaces), so tag_word = 'zero trust'
        var isAi = await _fixture.Connection.QueryFirstOrDefaultAsync<bool?>(
            "SELECT is_ai FROM content_tags_expanded WHERE slug = @Slug AND collection_name = @Collection AND tag_word = 'zero trust'",
            new { Slug = slug, Collection = "blogs" });

        isAi.Should().NotBeNull("tag row should exist for 'Zero Trust'");
        isAi.Should().BeTrue();

        var isSecurity = await _fixture.Connection.QueryFirstAsync<bool>(
            "SELECT is_security FROM content_tags_expanded WHERE slug = @Slug AND collection_name = @Collection AND tag_word = 'zero trust'",
            new { Slug = slug, Collection = "blogs" });
        isSecurity.Should().BeTrue();

        var isAzure = await _fixture.Connection.QueryFirstAsync<bool>(
            "SELECT is_azure FROM content_tags_expanded WHERE slug = @Slug AND collection_name = @Collection AND tag_word = 'zero trust'",
            new { Slug = slug, Collection = "blogs" });
        isAzure.Should().BeFalse();

        var dateEpoch = await _fixture.Connection.QueryFirstAsync<long>(
            "SELECT date_epoch FROM content_tags_expanded WHERE slug = @Slug AND collection_name = @Collection AND tag_word = 'zero trust'",
            new { Slug = slug, Collection = "blogs" });
        dateEpoch.Should().Be(1700000000);

        var sectionsBitmask = await _fixture.Connection.QueryFirstAsync<int>(
            "SELECT sections_bitmask FROM content_tags_expanded WHERE slug = @Slug AND collection_name = @Collection AND tag_word = 'zero trust'",
            new { Slug = slug, Collection = "blogs" });
        sectionsBitmask.Should().Be(1 | 64); // ai=1, security=64
    }

    // ── Improved Log Messages ─────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_LogShowsPreviouslySkippedSeparately()
    {
        // Arrange — pre-record a URL as skipped
        var testId = Guid.NewGuid().ToString("N")[..8];
        var skippedUrl = $"https://example.com/previously-skipped-{testId}";
        await _processedUrlRepo.RecordSkippedAsync(skippedUrl, feedName: "Test", reason: "Not relevant", ct: CancellationToken.None);

        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var rawItem = CreateRawItem(skippedUrl);

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — log should mention "previously skipped", not "previously processed"
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs.Should().NotBeEmpty();
        var jobDetail = await _jobRepo.GetByIdAsync(jobs[0].Id, CancellationToken.None);
        jobDetail!.LogOutput.Should().Contain("previously skipped");
        jobDetail.LogOutput.Should().NotContain("previously processed");
    }

    // ── Real-time Progress ────────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_CompletedJob_HasCountersSet()
    {
        // Arrange
        var testId = Guid.NewGuid().ToString("N")[..8];
        var url = $"https://example.com/progress-counters-{testId}";
        var feed = new FeedConfig { Id = 1, Name = "Test", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var rawItem = CreateRawItem(url);
        var processed = CreateProcessedItem(url, $"progress-counters-{testId}");

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — completed job should have non-zero counters
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs.Should().NotBeEmpty();
        jobs[0].FeedsProcessed.Should().BeGreaterThanOrEqualTo(1);
        jobs[0].ItemsAdded.Should().BeGreaterThanOrEqualTo(1);
    }

    // ── Transcript Tracking ───────────────────────────────────────────────

    [Fact]
    public async Task RunAsync_YouTubeItem_WithTranscript_TracksSucceeded()
    {
        // Arrange
        var testId = Guid.NewGuid().ToString("N")[..8];
        var feed = new FeedConfig { Id = 1, Name = "YT", Url = "https://youtube.com/feed", OutputDir = "_videos", Enabled = true };
        var ytUrl = $"https://youtube.com/watch?v=transcript-ok-{testId}";
        var ytItem = new RawFeedItem
        {
            Title = "Video",
            ExternalUrl = ytUrl,
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "YT",
            CollectionName = "videos",
            FeedTags = []
        };
        var processed = CreateProcessedItem(ytUrl, $"transcript-ok-{testId}");

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([ytItem]));

        var sut = CreateService(new ContentProcessorOptions { MaxYouTubeTagCount = 0 });

        // Override default article service mock to simulate transcript fetch succeeding
        _articleService
            .Setup(s => s.EnrichWithContentAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((RawFeedItem item, CancellationToken _) => new RawFeedItem
            {
                Title = item.Title,
                ExternalUrl = item.ExternalUrl,
                PublishedAt = item.PublishedAt,
                FeedName = item.FeedName,
                CollectionName = item.CollectionName,
                FeedTags = item.FeedTags,
                FullContent = "Transcript text here"
            });
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — has_transcript should be true in processed_urls
        var result = await _processedUrlRepo.GetPagedAsync(0, 10, search: ytUrl, ct: CancellationToken.None);
        result.Items.Should().ContainSingle();
        result.Items[0].HasTranscript.Should().BeTrue();

        // Assert — job should track transcript counts
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs[0].TranscriptsSucceeded.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task RunAsync_YouTubeItem_WithoutTranscript_TracksFailed()
    {
        // Arrange
        var testId = Guid.NewGuid().ToString("N")[..8];
        var feed = new FeedConfig { Id = 1, Name = "YT", Url = "https://youtube.com/feed", OutputDir = "_videos", Enabled = true };
        var ytUrl = $"https://youtube.com/watch?v=transcript-fail-{testId}";
        var ytItem = new RawFeedItem
        {
            Title = "Video",
            ExternalUrl = ytUrl,
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "YT",
            CollectionName = "videos",
            FeedTags = []
        };
        var processed = CreateProcessedItem(ytUrl, $"transcript-fail-{testId}");

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([ytItem]));

        var sut = CreateService(new ContentProcessorOptions { MaxYouTubeTagCount = 0 });

        // CreateService already sets up EnrichWithContentAsync to return item unchanged (no transcript)
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — has_transcript should be false in processed_urls
        var result = await _processedUrlRepo.GetPagedAsync(0, 10, search: ytUrl, ct: CancellationToken.None);
        result.Items.Should().ContainSingle();
        result.Items[0].HasTranscript.Should().BeFalse();

        // Assert — job should track transcript failures
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs[0].TranscriptsFailed.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task RunAsync_NonYouTubeItem_HasTranscriptIsNull()
    {
        // Arrange
        var testId = Guid.NewGuid().ToString("N")[..8];
        var feed = new FeedConfig { Id = 1, Name = "Blog", Url = "https://example.com/feed", OutputDir = "_blogs", Enabled = true };
        var blogUrl = $"https://example.com/article-{testId}";
        var rawItem = CreateRawItem(blogUrl);
        var processed = CreateProcessedItem(blogUrl, $"article-{testId}");

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        var sut = CreateService();

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — has_transcript should be null for non-YouTube items
        var result = await _processedUrlRepo.GetPagedAsync(0, 10, search: blogUrl, ct: CancellationToken.None);
        result.Items.Should().ContainSingle();
        result.Items[0].HasTranscript.Should().BeNull();
    }

    [Fact]
    public async Task RunAsync_TranscriptMandatory_WithoutTranscript_FailsItem()
    {
        // Arrange
        var testId = Guid.NewGuid().ToString("N")[..8];
        var feed = new FeedConfig { Id = 1, Name = "YT Mandatory", Url = "https://youtube.com/feed", OutputDir = "_videos", Enabled = true, TranscriptMandatory = true };
        var ytUrl = $"https://youtube.com/watch?v=mandatory-fail-{testId}";
        var ytItem = new RawFeedItem
        {
            Title = "Video",
            ExternalUrl = ytUrl,
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "YT Mandatory",
            CollectionName = "videos",
            FeedTags = []
        };

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([ytItem]));

        var sut = CreateService(new ContentProcessorOptions { MaxYouTubeTagCount = 0 });

        // CreateService sets up EnrichWithContentAsync to return item unchanged (no transcript)

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — AI should NOT be called (item fails before AI)
        _aiService.Verify(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Never);

        // Assert — item should be recorded as failed
        var result = await _processedUrlRepo.GetPagedAsync(0, 10, status: "failed", search: ytUrl, ct: CancellationToken.None);
        result.Items.Should().ContainSingle();
        result.Items[0].HasTranscript.Should().BeFalse();
        result.Items[0].ErrorMessage.Should().Contain("Transcript mandatory");

        // Assert — job should count it as an error
        var jobs = await _jobRepo.GetRecentAsync(1, CancellationToken.None);
        jobs[0].ErrorCount.Should().BeGreaterThanOrEqualTo(1);
        jobs[0].TranscriptsFailed.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task RunAsync_TranscriptMandatory_WithTranscript_Succeeds()
    {
        // Arrange
        var testId = Guid.NewGuid().ToString("N")[..8];
        var feed = new FeedConfig { Id = 1, Name = "YT Mandatory", Url = "https://youtube.com/feed", OutputDir = "_videos", Enabled = true, TranscriptMandatory = true };
        var ytUrl = $"https://youtube.com/watch?v=mandatory-ok-{testId}";
        var ytItem = new RawFeedItem
        {
            Title = "Video",
            ExternalUrl = ytUrl,
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "YT Mandatory",
            CollectionName = "videos",
            FeedTags = []
        };
        var processed = CreateProcessedItem(ytUrl, $"mandatory-ok-{testId}");

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([ytItem]));

        var sut = CreateService(new ContentProcessorOptions { MaxYouTubeTagCount = 0 });

        // Override default article service mock to simulate transcript fetch succeeding
        _articleService
            .Setup(s => s.EnrichWithContentAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((RawFeedItem item, CancellationToken _) => new RawFeedItem
            {
                Title = item.Title,
                ExternalUrl = item.ExternalUrl,
                PublishedAt = item.PublishedAt,
                FeedName = item.FeedName,
                CollectionName = item.CollectionName,
                FeedTags = item.FeedTags,
                FullContent = "Transcript text here"
            });
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — AI should be called (transcript available)
        _aiService.Verify(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()), Times.Once);

        // Assert — item should succeed
        var result = await _processedUrlRepo.GetPagedAsync(0, 10, search: ytUrl, ct: CancellationToken.None);
        result.Items.Should().ContainSingle();
        result.Items[0].Status.Should().Be("succeeded");
        result.Items[0].HasTranscript.Should().BeTrue();
    }

    // ── Subcollection Rules ────────────────────────────────────────────────

    [Theory]
    [InlineData("Visual Studio Code and GitHub Copilot - What's new in March 2026", true)]
    [InlineData("Visual Studio Code and GitHub Copilot - What's new in April 2026", true)]
    [InlineData("Visual Studio Code and GitHub Copilot", true)]
    [InlineData("Some other video about Azure", false)]
    [InlineData("visual studio code and github copilot - lowercase test", true)]
    public void MatchesWildcardPattern_WithVSCodePattern_MatchesCorrectly(string title, bool expected)
    {
        // Arrange
        const string pattern = "Visual Studio Code and GitHub Copilot*";

        // Act
        var result = ContentProcessingService.MatchesWildcardPattern(title, pattern);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void MatchSubcollectionRule_WhenFeedAndTitleMatch_ReturnsSubcollection()
    {
        // Arrange
        var options = new ContentProcessorOptions
        {
            SubcollectionRules =
            [
                new SubcollectionRule
                {
                    FeedName = "Fokko at Work YouTube",
                    TitlePattern = "Visual Studio Code and GitHub Copilot*",
                    Subcollection = "vscode-updates"
                }
            ]
        };
        var sut = CreateService(options);

        // Act
        var result = sut.MatchSubcollectionRule("Fokko at Work YouTube", "Visual Studio Code and GitHub Copilot - What's new in March 2026");

        // Assert
        result.Should().Be("vscode-updates");
    }

    [Fact]
    public void MatchSubcollectionRule_WhenFeedMatchesButTitleDoesNot_ReturnsNull()
    {
        // Arrange
        var options = new ContentProcessorOptions
        {
            SubcollectionRules =
            [
                new SubcollectionRule
                {
                    FeedName = "Fokko at Work YouTube",
                    TitlePattern = "Visual Studio Code and GitHub Copilot*",
                    Subcollection = "vscode-updates"
                }
            ]
        };
        var sut = CreateService(options);

        // Act
        var result = sut.MatchSubcollectionRule("Fokko at Work YouTube", "Building a Custom MCP Server");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void MatchSubcollectionRule_WhenFeedDoesNotMatch_ReturnsNull()
    {
        // Arrange
        var options = new ContentProcessorOptions
        {
            SubcollectionRules =
            [
                new SubcollectionRule
                {
                    FeedName = "Fokko at Work YouTube",
                    TitlePattern = "Visual Studio Code and GitHub Copilot*",
                    Subcollection = "vscode-updates"
                }
            ]
        };
        var sut = CreateService(options);

        // Act
        var result = sut.MatchSubcollectionRule("Some Other Channel", "Visual Studio Code and GitHub Copilot - What's new");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void MatchSubcollectionRule_WhenNoRulesConfigured_ReturnsNull()
    {
        // Arrange
        var sut = CreateService();

        // Act
        var result = sut.MatchSubcollectionRule("Any Feed", "Any Title");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RunAsync_WhenSubcollectionRuleMatches_SetsSubcollectionOnItem()
    {
        // Arrange
        var testId = Guid.NewGuid().ToString("N")[..8];
        var url = $"https://www.youtube.com/watch?v=sub-rule-{testId}";
        var feed = new FeedConfig { Id = 1, Name = "Fokko at Work YouTube", Url = "https://example.com/feed", OutputDir = "_videos", Enabled = true };
        var rawItem = new RawFeedItem
        {
            Title = "Visual Studio Code and GitHub Copilot - What's new in March 2026",
            ExternalUrl = url,
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "Fokko at Work YouTube",
            CollectionName = "videos"
        };
        var processed = new ProcessedContentItem
        {
            Slug = $"vscode-copilot-march-{testId}",
            Title = "Visual Studio Code and GitHub Copilot - What's new in March 2026",
            Excerpt = "Monthly VS Code updates",
            DateEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            CollectionName = "videos",
            ExternalUrl = url,
            FeedName = "Fokko at Work YouTube",
            ContentHash = $"hash-{testId}",
            Sections = ["github-copilot", "ai"],
            PrimarySectionName = "github-copilot",
            Tags = ["vscode", "copilot"]
        };

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
        _youtubeTagService.Setup(s => s.GetTagsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        var options = new ContentProcessorOptions
        {
            SubcollectionRules =
            [
                new SubcollectionRule
                {
                    FeedName = "Fokko at Work YouTube",
                    TitlePattern = "Visual Studio Code and GitHub Copilot*",
                    Subcollection = "vscode-updates"
                }
            ]
        };
        var sut = CreateService(options);

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — verify the item was written to DB with subcollection_name set
        var dbItem = await _fixture.Connection.QuerySingleOrDefaultAsync<dynamic>(
            "SELECT subcollection_name FROM content_items WHERE external_url = @Url",
            new { Url = url });
        ((string)dbItem!.subcollection_name).Should().Be("vscode-updates");
    }

    [Fact]
    public async Task RunAsync_WhenNoSubcollectionRuleMatches_SubcollectionRemainsNull()
    {
        // Arrange
        var testId = Guid.NewGuid().ToString("N")[..8];
        var url = $"https://www.youtube.com/watch?v=no-sub-{testId}";
        var feed = new FeedConfig { Id = 1, Name = "Fokko at Work YouTube", Url = "https://example.com/feed", OutputDir = "_videos", Enabled = true };
        var rawItem = new RawFeedItem
        {
            Title = "Building a Custom MCP Server",
            ExternalUrl = url,
            PublishedAt = DateTimeOffset.UtcNow,
            FeedName = "Fokko at Work YouTube",
            CollectionName = "videos"
        };
        var processed = new ProcessedContentItem
        {
            Slug = $"mcp-server-{testId}",
            Title = "Building a Custom MCP Server",
            Excerpt = "How to build MCP servers",
            DateEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            CollectionName = "videos",
            ExternalUrl = url,
            FeedName = "Fokko at Work YouTube",
            ContentHash = $"hash-{testId}",
            Sections = ["ai"],
            PrimarySectionName = "ai",
            Tags = ["mcp"]
        };

        _feedRepo.Setup(r => r.GetEnabledAsync(It.IsAny<CancellationToken>())).ReturnsAsync([feed]);
        _rssService.Setup(r => r.IngestAsync(feed, It.IsAny<CancellationToken>())).ReturnsAsync(FeedIngestionResult.Success([rawItem]));
        _youtubeTagService.Setup(s => s.GetTagsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);
        _aiService.Setup(s => s.CategorizeAsync(It.IsAny<RawFeedItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategorizationResult { Item = processed, Explanation = "Included" });

        var options = new ContentProcessorOptions
        {
            SubcollectionRules =
            [
                new SubcollectionRule
                {
                    FeedName = "Fokko at Work YouTube",
                    TitlePattern = "Visual Studio Code and GitHub Copilot*",
                    Subcollection = "vscode-updates"
                }
            ]
        };
        var sut = CreateService(options);

        // Act
        await sut.RunAsync("scheduled", CancellationToken.None);

        // Assert — verify the item was written without a subcollection
        var dbItem = await _fixture.Connection.QuerySingleOrDefaultAsync<dynamic>(
            "SELECT subcollection_name FROM content_items WHERE external_url = @Url",
            new { Url = url });
        ((string?)dbItem!.subcollection_name).Should().BeNull();
    }
}
