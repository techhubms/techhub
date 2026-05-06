using FluentAssertions;
using TechHub.Core.Security;

namespace TechHub.Core.Tests.Security;

/// <summary>
/// Exhaustive tests for <see cref="ProbeDetector.IsProbeRequest"/>.
/// All probe-detection logic is defined here; higher-level tests in
/// TechHub.Web.Tests and TechHub.Api.Tests only smoke-test the delegation.
/// </summary>
public class ProbeDetectorTests
{
    [Theory]
    // WordPress attack surface
    [InlineData("/wp-admin")]
    [InlineData("/wp-admin/admin.php")]
    [InlineData("/wp-admin/setup-config.php")]
    [InlineData("/wp-content/themes/twenty/style.css")]
    [InlineData("/wp-includes/functions.php")]
    [InlineData("/wp-login.php")]
    // WordPress XML-RPC
    [InlineData("/xmlrpc.php")]
    // PHP admin panels
    [InlineData("/phpmyadmin")]
    [InlineData("/phpmyadmin/index.php")]
    // CGI / Spring Boot actuator
    [InlineData("/cgi-bin/test.cgi")]
    [InlineData("/actuator")]
    [InlineData("/actuator/health")]
    [InlineData("/something/actuator")]
    // Generic application probes
    [InlineData("/app")]
    [InlineData("/app/dashboard")]
    [InlineData("/something/app")]
    [InlineData("/login")]
    [InlineData("/login/callback")]
    [InlineData("/something/login")]
    [InlineData("/ip")]
    [InlineData("/ip/lookup")]
    // Server-side script extensions
    [InlineData("/setup.php")]
    [InlineData("/config.asp")]
    [InlineData("/install.aspx")]
    [InlineData("/some/path.cfm")]
    [InlineData("/run.cgi")]
    [InlineData("/script.pl")]
    [InlineData("/code.py")]
    [InlineData("/app.rb")]
    [InlineData("/page.jsp")]
    // Config / credential files
    [InlineData("/.env")]
    [InlineData("/.env/")]           // trailing slash
    [InlineData("/.htaccess")]
    [InlineData("/server/.htpasswd")]
    // Backup / leftover files
    [InlineData("/backup.sql")]
    [InlineData("/database.bak")]
    [InlineData("/old.backup")]
    [InlineData("/file.orig")]
    [InlineData("/swap.swp")]
    // Executables / binaries
    [InlineData("/malware.exe")]
    [InlineData("/lib.dll")]
    [InlineData("/run.sh")]
    [InlineData("/script.bat")]
    [InlineData("/cmd.cmd")]
    // Certificates and keys
    [InlineData("/private.key")]
    [InlineData("/cert.pem")]
    [InlineData("/client.crt")]
    [InlineData("/keystore.p12")]
    [InlineData("/bundle.pfx")]
    // Archives
    [InlineData("/site.zip")]
    [InlineData("/export.tar.gz")]
    [InlineData("/dump.rar")]
    [InlineData("/archive.7z")]
    // Source maps — never published to production
    [InlineData("/js/bundle.js.map")]
    [InlineData("/css/styles.css.map")]
    // .xml that is NOT a feed or sitemap
    [InlineData("/random.xml")]
    [InlineData("/random.xml/")]     // trailing slash on .xml probe
    [InlineData("/evil-sitemap.xml")]
    [InlineData("/some/evil.xml")]
    [InlineData("/wp-content/plugins/foo.xml")]
    public void IsProbeRequest_ReturnsTrue_ForProbePatterns(string path)
    {
        ProbeDetector.IsProbeRequest(path).Should().BeTrue();
    }

    [Theory]
    // Normal site routes
    [InlineData("/")]
    [InlineData("/ai")]
    [InlineData("/AI")]
    [InlineData("/github-copilot")]
    [InlineData("/github-copilot/features")]
    [InlineData("/github-copilot/features/some-article")]
    [InlineData("/all")]
    [InlineData("/all/roundups/weekly-ai-and-tech-news-roundup-2026-03-16")]
    [InlineData("/not-found")]
    [InlineData("/about")]
    // RSS feeds — always allowed through
    [InlineData("/all/feed.xml")]
    [InlineData("/ai/feed.xml")]
    [InlineData("/github-copilot/feed.xml")]
    [InlineData("/all/roundups/feed.xml")]
    [InlineData("/feed.xml")]
    // Sitemap — always allowed through
    [InlineData("/sitemap.xml")]
    // "actuator" / "login" / "app" as slug prefix or infix, not a whole segment
    [InlineData("/ai/actuator-systems-deep-dive")]
    [InlineData("/ai/login-free-coding-with-copilot")]
    [InlineData("/ai/app-development-trends")]
    // Non-probe extensions
    [InlineData("/config.json")]
    [InlineData("/TechHub.Web.fwv5rmn5un.styles.css")]
    [InlineData("/js/nav-helpers.fmmqw6nflr.js")]
    // Known admin routes that happen to contain probe keywords (with and without trailing slash)
    [InlineData("/admin/login")]
    [InlineData("/admin/login/")]
    // Null / empty
    [InlineData(null)]
    [InlineData("")]
    public void IsProbeRequest_ReturnsFalse_ForLegitimatePathsAndExceptions(string? path)
    {
        ProbeDetector.IsProbeRequest(path).Should().BeFalse();
    }
}
