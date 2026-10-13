using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using ModelContextProtocol;
using TechHub.Core.Validation;
using TechHub.Web.Services;

namespace TechHub.Web.MCP;

/// <summary>
/// Looks up a single content item for the MCP <c>get_content_item</c> tool from the Tech Hub URL
/// returned by the latest-content tools.
/// </summary>
internal sealed class ContentItemProvider
{
    private readonly ITechHubApiClient _apiClient;
    private readonly Uri _baseUri;
    private readonly string _baseUrl;

    public ContentItemProvider(ITechHubApiClient apiClient, IOptions<McpOptions> options)
    {
        _apiClient = apiClient;
        _baseUrl = options.Value.BaseUrl.TrimEnd('/');
        _baseUri = new Uri(_baseUrl, UriKind.Absolute);
    }

    public async Task<ContentItemResult> GetAsync(string url, bool includeTags, CancellationToken cancellationToken)
    {
        var (section, collection, slug) = ParseUrl(url);

        var detail = await _apiClient.GetContentDetailAsync(section, collection, slug, cancellationToken)
            ?? throw new McpException("Content item not found. Use a url returned by one of the latest-content tools.");

        return new ContentItemResult(
            detail.Title,
            detail.Author,
            DateHelper.FormatTimestampIso(detail.DateEpoch),
            detail.PrimarySectionName,
            detail.CollectionName,
            detail.Excerpt,
            $"{_baseUrl}{detail.GetCanonicalHref()}")
        {
            SourceUrl = SafeSourceUrl(detail.ExternalUrl),
            // Externally linked collections only show an excerpt on Tech Hub, so the full text is not republished here.
            Content = detail.LinksExternally() ? null : detail.RenderedHtml,
            Tags = includeTags ? detail.Tags : null,
        };
    }

    private (string Section, string Collection, string Slug) ParseUrl(string url)
    {
        string? path = null;
        if (url.StartsWith('/'))
        {
            path = url;
        }
        else if (Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && Uri.Compare(uri, _baseUri, UriComponents.SchemeAndServer, UriFormat.UriEscaped, StringComparison.OrdinalIgnoreCase) == 0)
        {
            path = uri.AbsolutePath;
        }

        var segments = path?.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments is [var section, var collection, var slug]
            && RouteParameterValidator.IsValidNameSegment(section)
            && RouteParameterValidator.IsValidNameSegment(collection)
            && RouteParameterValidator.IsValidSlug(slug))
        {
            return (section, collection, slug);
        }

        throw new McpException(
            $"url must be a Tech Hub item url as returned by the latest-content tools, for example {_baseUrl}/ai/news/some-item.");
    }

    private static string? SafeSourceUrl(string? url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
            ? url
            : null;
}

internal record ContentItemResult(
    string Title,
    string Author,
    string Date,
    string Section,
    string Collection,
    string Excerpt,
    string Url)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceUrl { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Content { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? Tags { get; init; }
}
