using TechHub.Core.Models;

namespace TechHub.TestUtilities.Builders;

/// <summary>
/// Builder pattern for creating Collection instances in tests.
/// All required properties have sensible defaults to reduce test boilerplate.
/// </summary>
public class CollectionBuilder
{
    private string _name = "news";
    private string _title = "News";
    private string _url = "/github-copilot/news";
    private string _description = "News articles";
    private string _displayName = "News";
    private bool _isCustom = false;

    public CollectionBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CollectionBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public CollectionBuilder WithUrl(string url)
    {
        _url = url;
        return this;
    }

    public CollectionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public CollectionBuilder WithDisplayName(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public CollectionBuilder WithIsCustom(bool isCustom)
    {
        _isCustom = isCustom;
        return this;
    }

    public Collection Build()
    {
        return new Collection(_name, _title, _url, _description, _displayName, _isCustom);
    }
}
