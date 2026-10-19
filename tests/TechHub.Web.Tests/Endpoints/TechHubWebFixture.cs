using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.Endpoints;

/// <summary>
/// Hosts the real <c>TechHub.Web</c> pipeline (<c>Program.cs</c>) in-memory. The API is replaced by
/// <see cref="StubApiHandler"/> and the OIDC scheme by <see cref="RecordingOidcHandler"/>, so tests never
/// touch the network, Entra ID or the file system.
/// </summary>
internal sealed class TechHubWebFactory(string environment) : WebApplicationFactory<Program>
{
    private readonly MemoryConfigurationProvider _config = new(new MemoryConfigurationSource());

    public SignOutRecorder SignOuts { get; } = new();

    public void SetConfig(string key, string? value) => _config.Set(key, value);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.UseEnvironment(environment);

        // Read by Program.cs while building, so it must be a setting rather than a ConfigureAppConfiguration callback.
        builder.UseSetting("AppSettings:SkipFileLogging", "true");

        // Added last, so values set through SetConfig win over appsettings and are read live by endpoints.
        builder.ConfigureAppConfiguration((_, configuration) => configuration.Add(new ProviderSource(_config)));

        builder.ConfigureLogging(logging =>
            logging.AddFilter<ConsoleLoggerProvider>(level => level >= LogLevel.Error));

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(SignOuts);
            services.AddHttpClient<TechHubApiClient>().ConfigurePrimaryHttpMessageHandler(() => new StubApiHandler());
            services.AddHttpClient<ApiHealthCheck>().ConfigurePrimaryHttpMessageHandler(() => new StubApiHandler());

            // Register a recording OIDC scheme so tests can verify federated sign-out without Azure AD.
            services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, RecordingOidcHandler>("OpenIdConnect", _ => { });
        });
    }

    private sealed class ProviderSource(IConfigurationProvider provider) : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder) => provider;
    }
}

/// <summary>Shared in-memory Web host in the Development environment (loopback clients are rate-limit exempt).</summary>
public sealed class DevelopmentWebFixture : TechHubWebFixtureBase
{
    public DevelopmentWebFixture() : base("Development")
    {
    }
}

/// <summary>Shared in-memory Web host in the Production environment (no loopback exemption).</summary>
public sealed class ProductionWebFixture : TechHubWebFixtureBase
{
    public ProductionWebFixture() : base("Production")
    {
    }
}

public abstract class TechHubWebFixtureBase : IAsyncDisposable
{
    private readonly TechHubWebFactory _factory;

    protected TechHubWebFixtureBase(string environment)
    {
        _factory = new TechHubWebFactory(environment);
    }

    public IServiceProvider Services => _factory.Services;

    public SignOutRecorder SignOuts => _factory.SignOuts;

    /// <summary>Sets a configuration value for the running host; <c>null</c> removes it.</summary>
    public void SetConfig(string key, string? value) => _factory.SetConfig(key, value);

    /// <summary>
    /// Creates a non-redirecting HTTPS client. <paramref name="forwardedFor"/> is sent as <c>X-Forwarded-For</c>,
    /// which Program.cs trusts, so each test can use its own client IP (rate-limit partition).
    /// </summary>
    public HttpClient CreateClient(string? forwardedFor = null)
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost")
        });

        if (forwardedFor is not null)
        {
            client.DefaultRequestHeaders.Add("X-Forwarded-For", forwardedFor);
        }

        return client;
    }

    public async ValueTask DisposeAsync()
    {
        await _factory.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}

/// <summary>Answers the few API calls made during Web startup and by the RSS proxy; everything else is 404.</summary>
public sealed class StubApiHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Task.FromResult(request.RequestUri?.AbsolutePath switch
        {
            "/api/sections" => Ok("[]", "application/json"),
            "/api/rss/all" => Ok("<rss version=\"2.0\"><channel><title>Tech Hub</title></channel></rss>", "application/rss+xml"),
            _ => NotFound()
        });
    }

    private static HttpResponseMessage Ok(string content, string mediaType) => new(HttpStatusCode.OK)
    {
        Content = new StringContent(content, Encoding.UTF8, mediaType)
    };

    private static HttpResponseMessage NotFound() => new(HttpStatusCode.NotFound);
}

/// <summary>Collects the redirect URIs of sign-out calls made against the fake OIDC scheme.</summary>
public sealed class SignOutRecorder
{
    private readonly List<string?> _redirectUris = [];

    public IReadOnlyList<string?> RedirectUris
    {
        get
        {
            lock (_redirectUris)
            {
                return [.. _redirectUris];
            }
        }
    }

    public void Record(string? redirectUri)
    {
        lock (_redirectUris)
        {
            _redirectUris.Add(redirectUri);
        }
    }
}

/// <summary>Fake <c>OpenIdConnect</c> scheme that only supports sign-out and records it.</summary>
public sealed class RecordingOidcHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    SignOutRecorder recorder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder), IAuthenticationSignOutHandler
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync() =>
        Task.FromResult(AuthenticateResult.NoResult());

    public Task SignOutAsync(AuthenticationProperties? properties)
    {
        recorder.Record(properties?.RedirectUri);
        return Task.CompletedTask;
    }
}
