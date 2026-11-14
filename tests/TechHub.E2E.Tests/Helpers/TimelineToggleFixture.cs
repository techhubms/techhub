using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace TechHub.E2E.Tests.Helpers;

/// <summary>
/// Opens the <c>timeline</c> feature toggle to everyone for the tests of a class and puts the previous mode back afterwards.
/// A visitor without a login only sees the timeline when the toggle is open, and the toggle of a restored database is usually
/// limited to admins, so without this the timeline tests would have to be skipped.
///
/// The toggle is changed through the admin API, which is open in Development. Against a remote deployment (<c>E2E_BASE_URL</c>)
/// the admin API is not reachable and the toggle is left as it is (<see cref="CanChangeToggle"/> is false).
/// The Web app reads the toggles from a cache that refreshes every 30 seconds, so changes are awaited by polling the page.
/// </summary>
public sealed class TimelineToggleFixture : IAsyncLifetime
{
    private const string ToggleKey = "timeline";
    private const string PagePath = "/github-copilot/timeline";

    private static readonly string _apiBaseUrl =
        (Environment.GetEnvironmentVariable("E2E_API_URL") ?? "https://localhost:5001").TrimEnd('/');

    private HttpClient? _http;
    private string? _originalMode;

    /// <summary>True when the API of the environment under test is reachable and open, so the toggle can be changed.</summary>
    public bool CanChangeToggle { get; private set; }

    public async ValueTask InitializeAsync()
    {
        if (!Uri.TryCreate(BlazorHelpers.BaseUrl, UriKind.Absolute, out var baseUri) || !baseUri.IsLoopback)
        {
            return;
        }

        _http = new HttpClient(new HttpClientHandler
        {
            // The local development certificate is self-signed.
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        })
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        _originalMode = await GetModeAsync();
        CanChangeToggle = true;
        await SetModeAsync("Everyone");
    }

    public async ValueTask DisposeAsync()
    {
        if (_http is null)
        {
            return;
        }

        try
        {
            if (CanChangeToggle && _originalMode is not null)
            {
                await SendModeAsync(_originalMode);
            }
        }
        finally
        {
            _http.Dispose();
        }
    }

    /// <summary>Sets the mode of the timeline toggle (<c>Disabled</c>, <c>Admins</c> or <c>Everyone</c>) and waits until the Web app applies it.</summary>
    public async Task SetModeAsync(string mode)
    {
        CanChangeToggle.Should().BeTrue("the toggle can only be changed on a local environment");

        await SendModeAsync(mode);
        var expected = mode == "Everyone" ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        await WaitForPageStatusAsync(expected);
    }

    private async Task<string> GetModeAsync()
    {
        var toggles = await _http!.GetFromJsonAsync<JsonElement>($"{_apiBaseUrl}/api/feature-toggles");
        foreach (var toggle in toggles.EnumerateArray())
        {
            if (string.Equals(toggle.GetProperty("key").GetString(), ToggleKey, StringComparison.Ordinal))
            {
                return toggle.GetProperty("mode").GetString()!;
            }
        }

        throw new InvalidOperationException($"The feature toggle '{ToggleKey}' does not exist.");
    }

    private async Task SendModeAsync(string mode)
    {
        using var response = await _http!.PutAsJsonAsync($"{_apiBaseUrl}/api/admin/feature-toggles/{ToggleKey}", new { mode });
        response.EnsureSuccessStatusCode();
    }

    private async Task WaitForPageStatusAsync(HttpStatusCode expected)
    {
        // The cache of the Web app refreshes every 30 seconds.
        var deadline = DateTime.UtcNow.AddSeconds(60);
        HttpStatusCode last;
        do
        {
            using var response = await _http!.GetAsync($"{BlazorHelpers.BaseUrl}{PagePath}");
            last = response.StatusCode;
            if (last == expected)
            {
                return;
            }

            await Task.Delay(1000);
        }
        while (DateTime.UtcNow < deadline);

        throw new TimeoutException($"{PagePath} still answers {(int)last} after the timeline toggle was changed; expected {(int)expected}.");
    }
}
