using TechHub.Core.Models;

namespace TechHub.TestUtilities.Builders;

/// <summary>
/// Builder pattern for creating ContentItem instances in tests.
/// All required properties have sensible defaults to reduce test boilerplate.
/// </summary>
public class ContentItemBuilder
{
    private string _slug = "test-slug";
    private string _title = "Test Title";
    private string _author = "Test Author";
    private long _dateEpoch = 1735689600; // 2025-01-01 00:00:00 UTC
    private string _collectionName = "news";
    private string? _subcollectionName;
    private string _primarySectionName = "github-copilot";
    private string _feedName = "Test Feed";
    private IReadOnlyList<string> _tags = ["AI", "GitHub Copilot", "Test Tag"];
    private string _excerpt = "Test excerpt";
    private string _externalUrl = "https://dummy";
    private string? _content;
    private string? _renderedHtml;
    private IReadOnlyList<string>? _plans;
    private bool _ghesSupport;
    private bool _draft;

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

    public ContentItemBuilder WithPrimarySectionName(string primarySectionName)
    {
        _primarySectionName = primarySectionName;
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

    public ContentItem Build()
    {
        return new ContentItem(
            slug: _slug,
            title: _title,
            author: _author,
            dateEpoch: _dateEpoch,
            collectionName: _collectionName,
            feedName: _feedName,
            primarySectionName: _primarySectionName,
            tags: _tags,
            excerpt: _excerpt,
            externalUrl: _externalUrl,
            draft: _draft,
            subcollectionName: _subcollectionName,
            plans: _plans,
            ghesSupport: _ghesSupport,
            content: _content,
            renderedHtml: _renderedHtml
        );
    }
}
