using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;
using TechHub.Infrastructure.Repositories;

namespace TechHub.TestUtilities;

/// <summary>
/// Stub implementation of IContentRepository for integration tests.
/// Uses real markdown files from TestCollections directory instead of hardcoded data.
/// Inherits all filtering/search logic from FileBasedContentRepository.
/// Provides production-like test data from a curated subset of actual content files.
/// </summary>
public class StubContentRepository : FileBasedContentRepository
{
    public StubContentRepository(
        IMarkdownService markdownService,
        ITagMatchingService tagMatchingService,
        IHostEnvironment environment,
        IMemoryCache cache)
        : base(
            Options.Create(new AppSettings
            {
                Content = new ContentSettings
                {
                    // Point to TestCollections directory in the TestUtilities assembly output
                    CollectionsPath = Path.Combine(AppContext.BaseDirectory, "TestCollections"),
                    Sections = [] // Empty sections - FileBasedContentRepository doesn't use this
                }
            }),
            markdownService,
            tagMatchingService,
            environment,
            cache)
    {
        // No cache pre-population needed - base class will load from TestCollections directory
    }

    private static List<ContentItem> CreateTestData_Obsolete()
    {
        var now = DateTimeOffset.UtcNow;

        return
        [
            // AI News
            new ContentItem
            {
                Slug = "2024-01-15-ai-news-1",
                Title = "AI News Article 1",
                Excerpt = "Latest updates in AI technology",
                RenderedHtml = "<p>Full content of AI news article 1</p>",
                Author = "Test Author",
                DateEpoch = now.AddDays(-10).ToUnixTimeSeconds(),
                CollectionName = "news",
                SectionNames = ["ai"],
                Tags = ["ai", "news", "technology"],
                ExternalUrl = "https://example.com/ai-news-1",
                SubcollectionName = null,
                FeedName = null
            },

            // GitHub Copilot Video
            new ContentItem
            {
                Slug = "2024-01-20-copilot-video-1",
                Title = "GitHub Copilot Tutorial",
                Excerpt = "Learn GitHub Copilot basics",
                RenderedHtml = "<p>Full content of Copilot tutorial</p>",
                Author = "Test Author",
                DateEpoch = now.AddDays(-5).ToUnixTimeSeconds(),
                CollectionName = "videos",
                SectionNames = ["github-copilot"],
                Tags = ["github-copilot", "tutorial", "productivity"],
                ExternalUrl = "https://example.com/copilot-video",
                SubcollectionName = null,
                FeedName = null
            },

            // Multi-section content (AI + GitHub Copilot)
            new ContentItem
            {
                Slug = "2024-01-18-ai-copilot-blog",
                Title = "AI-Powered Development",
                Excerpt = "How AI enhances development with Copilot",
                RenderedHtml = "<p>Full content about AI and Copilot</p>",
                Author = "Test Author",
                DateEpoch = now.AddDays(-7).ToUnixTimeSeconds(),
                CollectionName = "blogs",
                SectionNames = ["ai", "github-copilot"],
                Tags = ["ai", "github-copilot", "productivity"],
                ExternalUrl = "https://example.com/ai-copilot-blog",
                SubcollectionName = null,
                FeedName = null
            },

            // Community content
            new ContentItem
            {
                Slug = "2024-01-25-community-post",
                Title = "Community Insights",
                Excerpt = "Community perspectives on AI",
                RenderedHtml = "<p>Full community post content</p>",
                Author = "Community Contributor",
                DateEpoch = now.AddDays(-1).ToUnixTimeSeconds(),
                CollectionName = "community",
                SectionNames = ["ai"],
                Tags = ["ai", "community"],
                ExternalUrl = "https://example.com/community-post",
                SubcollectionName = null,
                FeedName = null
            },

            // Roundup
            new ContentItem
            {
                Slug = "2024-01-22-weekly-roundup",
                Title = "Weekly Tech Roundup",
                Excerpt = "This week's top tech stories",
                RenderedHtml = "<p>Full roundup content</p>",
                Author = "Editorial Team",
                DateEpoch = now.AddDays(-3).ToUnixTimeSeconds(),
                CollectionName = "roundups",
                SectionNames = ["ai", "github-copilot", "azure"],
                Tags = ["roundup", "weekly"],
                ExternalUrl = "https://example.com/weekly-roundup",
                SubcollectionName = null,
                FeedName = null
            }
        ];
    }
}
