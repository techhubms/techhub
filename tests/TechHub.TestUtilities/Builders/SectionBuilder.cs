using TechHub.Core.Models;

namespace TechHub.TestUtilities.Builders;

/// <summary>
/// Builder pattern for creating Section instances in tests.
/// All required properties have sensible defaults to reduce test boilerplate.
/// </summary>
public class SectionBuilder
{
    private string _name = "github-copilot";
    private string _title = "GitHub Copilot";
    private string _description = "GitHub Copilot resources";
    private string _url = "/github-copilot";
    private string _tag = "GitHub Copilot";
    private IReadOnlyList<Collection> _collections = [
        new Collection("blogs", "Blogs", "/github-copilot/blogs", "Blog posts", "Blogs"),
        new Collection("news", "News", "/github-copilot/news", "News articles", "News")
    ];

    public SectionBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public SectionBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public SectionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public SectionBuilder WithUrl(string url)
    {
        _url = url;
        return this;
    }

    public SectionBuilder WithTag(string tag)
    {
        _tag = tag;
        return this;
    }

    public SectionBuilder WithCollections(params Collection[] collections)
    {
        _collections = collections;
        return this;
    }

    public Section Build()
    {
        return new Section(_name, _title, _description, _url, _tag, _collections);
    }
}
