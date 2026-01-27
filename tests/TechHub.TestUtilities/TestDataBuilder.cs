using System.Data;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using TechHub.Core.Models;

namespace TechHub.TestUtilities;

/// <summary>
/// Fluent builder for creating test content items and inserting them into the database.
/// Makes integration tests more readable and maintainable.
/// </summary>
public class TestDataBuilder
{
    private string _id = Guid.NewGuid().ToString();
    private string _title = "Test Article";
    private string _content = "This is test content.";
    private string? _excerpt;
    private long _dateEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    private string _collectionName = "blogs";
    private string? _subcollectionName;
    private string _primarySectionName = "all";
    private string? _externalUrl;
    private string? _author;
    private string? _feedName;
    private bool _ghesSupport;
    private bool _draft;
    private readonly List<string> _tags = [];
    private readonly List<string> _sections = ["all"];
    private readonly List<string> _plans = [];

    /// <summary>
    /// Creates a builder for a new test content item.
    /// </summary>
    public static TestDataBuilder AContentItem() => new();

    /// <summary>
    /// Creates a builder from an existing content item.
    /// NOTE: ContentItem.Slug maps to database id field.
    /// </summary>
    public static TestDataBuilder From(ContentItem item) => new TestDataBuilder
    {
        _id = item.Slug,
        _title = item.Title,
        _content = "# Markdown content", // ContentItem has RenderedHtml, database has markdown
        _excerpt = item.Excerpt,
        _dateEpoch = item.DateEpoch,
        _collectionName = item.CollectionName,
        _subcollectionName = item.SubcollectionName,
        _primarySectionName = item.SectionNames.FirstOrDefault() ?? "all",
        _externalUrl = item.ExternalUrl,
        _author = item.Author,
        _feedName = item.FeedName,
        _ghesSupport = item.GhesSupport,
        _draft = item.Draft
    };

    public TestDataBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public TestDataBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public TestDataBuilder WithContent(string content)
    {
        _content = content;
        return this;
    }

    public TestDataBuilder WithExcerpt(string? excerpt)
    {
        _excerpt = excerpt;
        return this;
    }

    public TestDataBuilder WithDateEpoch(long dateEpoch)
    {
        _dateEpoch = dateEpoch;
        return this;
    }

    public TestDataBuilder WithDate(DateTime date)
    {
        _dateEpoch = ((DateTimeOffset)date).ToUnixTimeSeconds();
        return this;
    }

    public TestDataBuilder WithCollectionName(string collectionName)
    {
        _collectionName = collectionName;
        return this;
    }

    public TestDataBuilder WithSubcollectionName(string? subcollectionName)
    {
        _subcollectionName = subcollectionName;
        return this;
    }

    public TestDataBuilder WithPrimarySectionName(string primarySectionName)
    {
        _primarySectionName = primarySectionName;
        return this;
    }

    public TestDataBuilder WithExternalUrl(string? externalUrl)
    {
        _externalUrl = externalUrl;
        return this;
    }

    public TestDataBuilder WithAuthor(string? author)
    {
        _author = author;
        return this;
    }

    public TestDataBuilder WithFeedName(string? feedName)
    {
        _feedName = feedName;
        return this;
    }

    public TestDataBuilder WithGhesSupport(bool ghesSupport = true)
    {
        _ghesSupport = ghesSupport;
        return this;
    }

    public TestDataBuilder WithDraft(bool draft = true)
    {
        _draft = draft;
        return this;
    }

    public TestDataBuilder WithTags(params string[] tags)
    {
        _tags.Clear();
        _tags.AddRange(tags);
        return this;
    }

    public TestDataBuilder WithSections(params string[] sections)
    {
        _sections.Clear();
        _sections.AddRange(sections);
        return this;
    }

    public TestDataBuilder WithPlans(params string[] plans)
    {
        _plans.Clear();
        _plans.AddRange(plans);
        return this;
    }

    /// <summary>
    /// Inserts the content item into the database with all junction table entries.
    /// Returns the inserted ID for verification.
    /// </summary>
    public async Task<string> InsertAsync(IDbConnection connection)
    {
        var excerpt = _excerpt ?? _content[..Math.Min(100, _content.Length)];
        var contentHash = ComputeHash(_content);

        // Insert main content item
        await connection.ExecuteAsync(@"
            INSERT INTO content_items (
                id, title, content, excerpt, date_epoch, collection_name, subcollection_name,
                primary_section_name, external_url, author, feed_name, ghes_support, draft, content_hash,
                created_at, updated_at
            ) VALUES (
                @Id, @Title, @Content, @Excerpt, @DateEpoch, @CollectionName, @SubcollectionName,
                @PrimarySectionName, @ExternalUrl, @Author, @FeedName, @GhesSupport, @Draft, @ContentHash,
                CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
            )",
            new
            {
                Id = _id,
                Title = _title,
                Content = _content,
                Excerpt = excerpt,
                DateEpoch = _dateEpoch,
                CollectionName = _collectionName,
                SubcollectionName = _subcollectionName,
                PrimarySectionName = _primarySectionName,
                ExternalUrl = _externalUrl,
                Author = _author,
                FeedName = _feedName,
                GhesSupport = _ghesSupport,
                Draft = _draft,
                ContentHash = contentHash
            });

        // Insert tags
        foreach (var tag in _tags)
        {
            var tagNormalized = tag.ToLowerInvariant().Trim();
            await connection.ExecuteAsync(
                "INSERT INTO content_tags (content_id, tag, tag_normalized) VALUES (@Id, @Tag, @TagNormalized)",
                new { Id = _id, Tag = tag, TagNormalized = tagNormalized });

            // Expand tags into words
            var words = tag.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                var wordNormalized = word.ToLowerInvariant().Trim();
                await connection.ExecuteAsync(
                    "INSERT OR IGNORE INTO content_tags_expanded (content_id, tag_word) VALUES (@Id, @Word)",
                    new { Id = _id, Word = wordNormalized });
            }
        }

        // Insert sections
        foreach (var section in _sections)
        {
            await connection.ExecuteAsync(
                "INSERT INTO content_sections (content_id, section_name) VALUES (@Id, @Section)",
                new { Id = _id, Section = section });
        }

        // Insert plans
        foreach (var plan in _plans)
        {
            await connection.ExecuteAsync(
                "INSERT INTO content_plans (content_id, plan_name) VALUES (@Id, @Plan)",
                new { Id = _id, Plan = plan });
        }

        return _id;
    }

    /// <summary>
    /// Inserts multiple content items into the database.
    /// </summary>
    public static async Task<string[]> InsertManyAsync(IDbConnection connection, int count, Action<TestDataBuilder, int>? configure = null)
    {
        var ids = new string[count];
        for (int i = 0; i < count; i++)
        {
            var builder = AContentItem()
                .WithId($"test-item-{Guid.NewGuid()}")
                .WithTitle($"Test Article {i + 1}");

            configure?.Invoke(builder, i);

            ids[i] = await builder.InsertAsync(connection);
        }

        return ids;
    }

    private static string ComputeHash(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
