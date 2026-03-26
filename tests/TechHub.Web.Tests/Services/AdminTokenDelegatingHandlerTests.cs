using System.Net;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Moq;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Services;

/// <summary>
/// Tests for AdminTokenDelegatingHandler verifying that access tokens are correctly
/// acquired via ITokenAcquisition and attached to admin API requests.
/// </summary>
public class AdminTokenDelegatingHandlerTests : IDisposable
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly Mock<ITokenAcquisition> _tokenAcquisition;
    private readonly HttpClient _httpClient;
    private readonly StubHandler _innerHandler;
    private bool _disposed;

    public AdminTokenDelegatingHandlerTests()
    {
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
        _tokenAcquisition = new Mock<ITokenAcquisition>();
        _innerHandler = new StubHandler();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AzureAd:Scopes"] = "api://test-client-id/Admin.Access"
            })
            .Build();

        var handler = new AdminTokenDelegatingHandler(
            _httpContextAccessor.Object,
            config,
            _tokenAcquisition.Object)
        {
            InnerHandler = _innerHandler
        };

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:5001")
        };
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _httpClient.Dispose();
            _innerHandler.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task AdminRequest_WithAuthenticatedUser_AttachesAccessToken()
    {
        // Arrange
        SetupAuthenticatedUser();
        // The handler calls GetAccessTokenForUserAsync(scopes) which resolves to the
        // 5-param overload: (IEnumerable<string>, string?, string?, ClaimsPrincipal?, TokenAcquisitionOptions?)
        _tokenAcquisition
            .Setup(t => t.GetAccessTokenForUserAsync(
                It.Is<IEnumerable<string>>(s => s.Contains("api://test-client-id/Admin.Access")),
                It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<System.Security.Claims.ClaimsPrincipal?>(), It.IsAny<Microsoft.Identity.Web.TokenAcquisitionOptions?>()))
            .ReturnsAsync("valid-access-token");

        // Act
        await _httpClient.GetAsync("/api/admin/dashboard", TestContext.Current.CancellationToken);

        // Assert
        _innerHandler.LastRequest!.Headers.Authorization.Should().NotBeNull();
        _innerHandler.LastRequest.Headers.Authorization!.Scheme.Should().Be("Bearer");
        _innerHandler.LastRequest.Headers.Authorization.Parameter.Should().Be("valid-access-token");
    }

    [Fact]
    public async Task NonAdminRequest_DoesNotAttachToken()
    {
        // Arrange
        SetupAuthenticatedUser();

        // Act
        await _httpClient.GetAsync("/api/content/blogs", TestContext.Current.CancellationToken);

        // Assert
        _innerHandler.LastRequest!.Headers.Authorization.Should().BeNull();
        _tokenAcquisition.Verify(
            t => t.GetAccessTokenForUserAsync(It.IsAny<IEnumerable<string>>(),
                It.IsAny<string?>(), It.IsAny<string?>(),
                It.IsAny<System.Security.Claims.ClaimsPrincipal?>(), It.IsAny<Microsoft.Identity.Web.TokenAcquisitionOptions?>()),
            Times.Never);
    }

    [Fact]
    public async Task AdminRequest_WithUnauthenticatedUser_DoesNotAttachToken()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        await _httpClient.GetAsync("/api/admin/dashboard", TestContext.Current.CancellationToken);

        // Assert
        _innerHandler.LastRequest!.Headers.Authorization.Should().BeNull();
    }

    [Fact]
    public async Task AdminRequest_WithoutTokenAcquisition_DoesNotAttachToken()
    {
        // Arrange — create handler without ITokenAcquisition (local dev scenario)
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AzureAd:Scopes"] = "api://test-client-id/Admin.Access"
            })
            .Build();

        var innerHandler = new StubHandler();
        var handler = new AdminTokenDelegatingHandler(
            _httpContextAccessor.Object,
            config,
            tokenAcquisition: null)
        {
            InnerHandler = innerHandler
        };

        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://localhost:5001") };
        SetupAuthenticatedUser();

        // Act
        await client.GetAsync("/api/admin/dashboard", TestContext.Current.CancellationToken);

        // Assert
        innerHandler.LastRequest!.Headers.Authorization.Should().BeNull();
        innerHandler.Dispose();
    }

    [Fact]
    public async Task AdminRequest_WithEmptyScopes_DoesNotAttachToken()
    {
        // Arrange — no scopes configured
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AzureAd:Scopes"] = ""
            })
            .Build();

        var innerHandler = new StubHandler();
        var handler = new AdminTokenDelegatingHandler(
            _httpContextAccessor.Object,
            config,
            _tokenAcquisition.Object)
        {
            InnerHandler = innerHandler
        };

        using var client = new HttpClient(handler) { BaseAddress = new Uri("https://localhost:5001") };
        SetupAuthenticatedUser();

        // Act
        await client.GetAsync("/api/admin/dashboard", TestContext.Current.CancellationToken);

        // Assert
        innerHandler.LastRequest!.Headers.Authorization.Should().BeNull();
        innerHandler.Dispose();
    }

    private void SetupAuthenticatedUser()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "admin@test.com") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);
    }

    /// <summary>
    /// Stub handler that captures the outgoing request for assertions.
    /// </summary>
    private sealed class StubHandler : DelegatingHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }
}
