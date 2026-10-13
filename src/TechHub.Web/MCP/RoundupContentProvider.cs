using Microsoft.Extensions.Options;
using ModelContextProtocol;
using TechHub.Core.Validation;
using TechHub.Web.Services;

namespace TechHub.Web.MCP;

/// <summary>
/// Looks up a weekly roundup, including its content, for the MCP <c>get_roundup_content</c> tool
/// from the Tech Hub URL returned by the latest-content tools.
/// </summary>
internal sealed class RoundupContentProvider
{
    private const string RoundupsCollection = "roundups";

    private readonly ITechHubApiClient _apiClient;
    private readonly Uri _baseUri;
    private readonly string _baseUrl;

    public RoundupContentProvider(ITechHubApiClient apiClient, IOptions<McpOptions> options)
    {
        _apiClient = apiClient;
        _baseUrl = options.Value.BaseUrl.TrimEnd('/');
        _baseUri = new Uri(_baseUrl, UriKind.Absolute);
    }

    public async Task<RoundupContentResult> GetAsync(string url, CancellationToken cancellationToken)
    {
        var (section, slug) = ParseUrl(url);

        var detail = await _apiClient.GetContentDetailAsync(section, RoundupsCollection, slug, cancellationToken)
            ?? throw new McpException("Roundup not found. Use a url returned by a latest-content tool with contentType=roundups.");

        if (string.IsNullOrWhiteSpace(detail.RenderedHtml))
        {
            throw new McpException("Roundup has no content.");
        }

        return new RoundupContentResult(
            detail.Title,
            DateHelper.FormatTimestampIso(detail.DateEpoch),
            detail.PrimarySectionName,
            $"{_baseUrl}{detail.GetCanonicalHref()}",
            detail.RenderedHtml);
    }

    private (string Section, string Slug) ParseUrl(string url)
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
            && string.Equals(collection, RoundupsCollection, StringComparison.OrdinalIgnoreCase)
            && RouteParameterValidator.IsValidNameSegment(section)
            && RouteParameterValidator.IsValidSlug(slug))
        {
            return (section, slug);
        }

        throw new McpException(
            $"url must be a Tech Hub roundup url as returned by a latest-content tool with contentType=roundups, for example {_baseUrl}/ai/roundups/some-roundup.");
    }
}

internal record RoundupContentResult(
    string Title,
    string Date,
    string Section,
    string Url,
    string Content);
