using TechHub.Core.Models;

namespace TechHub.TestUtilities.Builders;

/// <summary>
/// Builder pattern for creating ContentItem and ContentItemDetail instances in tests.
/// All required properties have sensible defaults to reduce test boilerplate.
/// Use Build() for list-view tests (no content), BuildDetail() for detail-view tests (with content).
/// </summary>
public class ContentItemBuilder
{
    private string _slug = "test-slug";
    private string _title = "Test Title";
    private string _author = "Test Author";
    private long _dateEpoch = 1735689600; // 2025-01-01 00:00:00 UTC
    private string _collectionName = "news";
    private string? _subcollectionName = null;
    private string _primarySectionName = "github-copilot";
    private string _feedName = "Test Feed";
    private IReadOnlyList<string> _tags = ["AI", "GitHub Copilot", "Test Tag"];
    private string _excerpt = "Test excerpt";
    private string _externalUrl = "https://dummy";
    private string _content = "# Test Content\n\nTest markdown content.";
    private string? _renderedHtml = null;
    private IReadOnlyList<string>? _plans = [];
    private bool _ghesSupport = false;
    private bool _draft = false;

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

    public ContentItemBuilder WithContent(string content)
    {
        ArgumentNullException.ThrowIfNull(content);
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

    /// <summary>
    /// Build a ContentItem for list-view tests (no content property).
    /// </summary>
    public ContentItem Build()
    {
        // Convert plans list to CSV string for constructor
        var plansString = _plans != null && _plans.Count > 0
            ? string.Join(",", _plans)
            : null;

        var contentItem = new ContentItem(
            slug: _slug,
            title: _title,
            author: _author,
            dateEpoch: _dateEpoch,
            collectionName: _collectionName,
            feedName: _feedName,
            primarySectionName: _primarySectionName,
            excerpt: _excerpt,
            externalUrl: _externalUrl,
            draft: _draft,
            subcollectionName: _subcollectionName,
            plans: plansString,
            ghesSupport: _ghesSupport
        );

        contentItem.SetTags(_tags);

        return contentItem;
    }

    /// <summary>
    /// Build a ContentItemDetail for detail-view tests (includes content and rendered HTML).
    /// </summary>
    public ContentItemDetail BuildDetail()
    {
        // Convert plans list to CSV string for constructor
        var plansString = _plans != null && _plans.Count > 0
            ? string.Join(",", _plans)
            : null;

        var contentItem = new ContentItemDetail(
            slug: _slug,
            title: _title,
            author: _author,
            dateEpoch: _dateEpoch,
            collectionName: _collectionName,
            feedName: _feedName,
            primarySectionName: _primarySectionName,
            excerpt: _excerpt,
            externalUrl: _externalUrl,
            draft: _draft,
            content: _content,
            subcollectionName: _subcollectionName,
            plans: plansString,
            ghesSupport: _ghesSupport
        );

        contentItem.SetTags(_tags);

        if (_renderedHtml != null)
        {
            contentItem.SetRenderedHtml(_renderedHtml);
        }

        return contentItem;
    }
}
