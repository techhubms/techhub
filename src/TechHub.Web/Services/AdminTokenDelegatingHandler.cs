using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace TechHub.Web.Services;

/// <summary>
/// Delegating handler that forwards the authenticated user's Azure AD ID token
/// as a Bearer header for admin API requests. Only attaches the token for requests
/// to <c>/api/admin/*</c> endpoints and when the user is authenticated.
/// When Azure AD is not configured (local dev), no token is attached and the API's
/// AdminOnly policy allows all requests.
/// </summary>
public sealed class AdminTokenDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminTokenDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Only attach bearer token for admin API calls
        var isAdminRequest = request.RequestUri?.PathAndQuery
            .StartsWith("/api/admin", StringComparison.OrdinalIgnoreCase) == true;

        if (isAdminRequest)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var token = await httpContext.GetTokenAsync("id_token");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
