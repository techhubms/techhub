using System.Text.Json;

using TechHub.Core.Models;

namespace TechHub.TestUtilities;

/// <summary>
/// Builder pattern for creating ContentItem instances in tests.
/// All required properties have sensible defaults to reduce test boilerplate.
/// </summary>
public class ContentItemBuilder
{
    private string _slug = "test-slug";
    private string _title = "Test Title";
    private string _author = "Test Author";
    private long _dateEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    private string _collectionName = "blogs";
    private string? _subcollectionName;
    private string _feedName = "";
    private IReadOnlyList<string> _sectionNames = ["all"];
    private IReadOnlyList<string> _tags = [];
    private string _excerpt = "Test excerpt";
    private string _externalUrl = "";
    private string _url = "/all/blogs/test-slug";
    private string? _content;
    private string? _renderedHtml;
    private IReadOnlyList<string> _plans = [];
    private bool _ghesSupport;
    private bool _draft;
    private bool _ghcFeature;

    public ContentItemBuilder WithSlug(string slug)
    {
        _slug = slug;
        return this;
    }

    public ContentItemBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public ContentItemBuilder WithAuthor(string author)
    {
        _author = author;
        return this;
    }

    public ContentItemBuilder WithDateEpoch(long dateEpoch)
    {
        _dateEpoch = dateEpoch;
        return this;
    }

    public ContentItemBuilder WithDate(DateTime date)
    {
        _dateEpoch = new DateTimeOffset(date, TimeSpan.Zero).ToUnixTimeSeconds();
        return this;
    }

    public ContentItemBuilder WithCollectionName(string collectionName)
    {
        _collectionName = collectionName;
        return this;
    }

    public ContentItemBuilder WithSubcollectionName(string? subcollectionName)
    {
        _subcollectionName = subcollectionName;
        return this;
    }

    public ContentItemBuilder WithFeedName(string feedName)
    {
        _feedName = feedName;
        return this;
    }

    public ContentItemBuilder WithSectionNames(params string[] sectionNames)
    {
        _sectionNames = sectionNames;
        return this;
    }

    public ContentItemBuilder WithTags(params string[] tags)
    {
        _tags = tags;
        return this;
    }

    public ContentItemBuilder WithExcerpt(string excerpt)
    {
        _excerpt = excerpt;
        return this;
    }

    public ContentItemBuilder WithExternalUrl(string externalUrl)
    {
        _externalUrl = externalUrl;
        return this;
    }

    public ContentItemBuilder WithUrl(string url)
    {
        _url = url;
        return this;
    }

    public ContentItemBuilder WithContent(string? content)
    {
        _content = content;
        return this;
    }

    public ContentItemBuilder WithRenderedHtml(string? renderedHtml)
    {
        _renderedHtml = renderedHtml;
        return this;
    }

    public ContentItemBuilder WithPlans(params string[] plans)
    {
        _plans = plans;
        return this;
    }

    public ContentItemBuilder WithGhesSupport(bool ghesSupport)
    {
        _ghesSupport = ghesSupport;
        return this;
    }

    public ContentItemBuilder WithDraft(bool draft)
    {
        _draft = draft;
        return this;
    }

    public ContentItemBuilder WithGhcFeature(bool ghcFeature)
    {
        _ghcFeature = ghcFeature;
        return this;
    }

    public ContentItem Build()
    {
        return new ContentItem
        {
            Slug = _slug,
            Title = _title,
            Author = _author,
            DateEpoch = _dateEpoch,
            CollectionName = _collectionName,
            SubcollectionName = _subcollectionName,
            FeedName = _feedName,
            SectionNames = _sectionNames,
            Tags = _tags,
            Excerpt = _excerpt,
            ExternalUrl = _externalUrl,
            Url = _url,
            Content = _content,
            RenderedHtml = _renderedHtml,
            Plans = _plans,
            GhesSupport = _ghesSupport,
            Draft = _draft,
            GhcFeature = _ghcFeature
        };
    }

    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    public static ContentItemBuilder Create() => new();

    /// <summary>
    /// Creates a builder for a news item with appropriate defaults.
    /// </summary>
    public static ContentItemBuilder CreateNews() => new ContentItemBuilder()
        .WithCollectionName("news")
        .WithExternalUrl("https://example.com/news-article");

    /// <summary>
    /// Creates a builder for a blog item with appropriate defaults.
    /// </summary>
    public static ContentItemBuilder CreateBlog() => new ContentItemBuilder()
        .WithCollectionName("blogs")
        .WithExternalUrl("https://example.com/blog-post");

    /// <summary>
    /// Creates a builder for a video item with appropriate defaults.
    /// </summary>
    public static ContentItemBuilder CreateVideo() => new ContentItemBuilder()
        .WithCollectionName("videos")
        .WithUrl("/all/videos/test-video");

    /// <summary>
    /// Creates a builder for a community item with appropriate defaults.
    /// </summary>
    public static ContentItemBuilder CreateCommunity() => new ContentItemBuilder()
        .WithCollectionName("community")
        .WithExternalUrl("https://example.com/community-post");

    /// <summary>
    /// Creates a builder for a roundup item with appropriate defaults.
    /// </summary>
    public static ContentItemBuilder CreateRoundup() => new ContentItemBuilder()
        .WithCollectionName("roundups")
        .WithUrl("/all/roundups/test-roundup");
}
