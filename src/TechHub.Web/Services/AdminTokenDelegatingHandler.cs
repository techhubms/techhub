using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

namespace TechHub.Web.Services;

/// <summary>
/// Delegating handler that acquires an Azure AD access token via
/// <see cref="ITokenAcquisition"/> and attaches it as a Bearer header for admin
/// API requests. Only attaches the token for requests to <c>/api/admin/*</c>
/// endpoints and when the user is authenticated.
/// When Azure AD is not configured (local dev), no token is attached and the API's
/// AdminOnly policy allows all requests.
/// </summary>
public sealed class AdminTokenDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenAcquisition? _tokenAcquisition;
    private readonly string[] _scopes;

    public AdminTokenDelegatingHandler(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        ITokenAcquisition? tokenAcquisition = null)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(configuration);

        _httpContextAccessor = httpContextAccessor;
        _tokenAcquisition = tokenAcquisition;
        _scopes = configuration.GetValue<string>("AzureAd:Scopes")?.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            ?? [];
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Only attach bearer token for admin API calls
        var isAdminRequest = request.RequestUri?.PathAndQuery
            .StartsWith("/api/admin", StringComparison.OrdinalIgnoreCase) == true;

        if (isAdminRequest && _tokenAcquisition != null && _scopes.Length > 0)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                try
                {
                    var token = await _tokenAcquisition.GetAccessTokenForUserAsync(_scopes);
                    if (!string.IsNullOrEmpty(token))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                }
                catch (MicrosoftIdentityWebChallengeUserException)
                {
                    // Token cache is empty (e.g. after logout or server restart).
                    // Let the request proceed without a token — the API will return 401.
                }
                catch (MsalUiRequiredException)
                {
                    // User context unavailable in SignalR circuit (no HttpContext with auth cookies).
                    // Let the request proceed without a token — the API will return 401.
                }
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
