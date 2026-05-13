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
    // WordPress attack surface — caught by path-segment probe
    [InlineData("/wp-admin")]
    [InlineData("/wp-admin/admin.php")]
    [InlineData("/wp-admin/setup-config.php")]
    [InlineData("/wp-content/themes/twenty/style.css")]
    [InlineData("/wp-includes/functions.php")]
    // Note: /wp-login.php and /xmlrpc.php are caught by the extension whitelist, not path-segment — see IsKnownStaticAssetPath tests
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
    // Common static-asset / build-output directories
    [InlineData("/assets")]
    [InlineData("/assets/images/logo.png")]
    [InlineData("/static")]
    [InlineData("/static/js/app.js")]
    [InlineData("/media")]
    [InlineData("/media/uploads/photo.jpg")]
    [InlineData("/dist")]
    [InlineData("/dist/bundle.js")]
    [InlineData("/vendor")]
    [InlineData("/vendor/jquery/jquery.min.js")]
    // Common backend / config directories
    [InlineData("/backend")]
    [InlineData("/backend/api/users")]
    [InlineData("/config")]
    [InlineData("/config/database.yml")]
    // robots.txt at any sub-path (real crawlers only use the root)
    [InlineData("/all/robots.txt")]
    [InlineData("/some/path/robots.txt")]
    [InlineData("/all/ROBOTS.TXT")]
    // .xml probe under a known probe path segment (caught by path-segment check)
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
    // robots.txt at the site root — always allowed through
    [InlineData("/robots.txt")]
    [InlineData("/ROBOTS.TXT")]
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
    // New probe keywords must not match when they are a prefix/infix of a slug
    [InlineData("/ai/videos/config-as-code-is-the-best")]
    [InlineData("/ai/videos/static-analysis-tools")]
    [InlineData("/ai/videos/backend-for-frontend-pattern")]
    [InlineData("/ai/videos/media-streaming-with-dotnet")]
    [InlineData("/github-copilot/features/dist-systems-explained")]
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

    [Theory]
    // Server-side script extensions — not served by this site, unknown path
    [InlineData("/setup.php")]
    [InlineData("/config.asp")]
    [InlineData("/install.aspx")]
    [InlineData("/some/path.cfm")]
    [InlineData("/run.cgi")]
    [InlineData("/script.pl")]
    [InlineData("/code.py")]
    [InlineData("/app.rb")]
    [InlineData("/page.jsp")]
    [InlineData("/wp-login.php")]
    [InlineData("/xmlrpc.php")]
    // Config / credential files
    [InlineData("/.env")]
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
    // Wrong extension under a known directory (prefix matches but extension does not)
    [InlineData("/js/bundle.js.map")]
    [InlineData("/css/styles.css.map")]
    [InlineData("/images/photo.tmb")]
    // Compound probe extensions
    [InlineData("/.env.live")]
    [InlineData("/.env.prod")]
    [InlineData("/.env.old")]
    [InlineData("/.env.bak")]
    [InlineData("/secrets.env.backup")]
    [InlineData("/config.php.bak")]
    [InlineData("/dump.sql.gz")]
    // .xml at unknown paths (not a feed or sitemap)
    [InlineData("/random.xml")]
    [InlineData("/evil-sitemap.xml")]
    [InlineData("/some/evil.xml")]
    // File under an unserved path — the original motivating case
    [InlineData("/devops/js/mobile-nav.4uezpgfs2f.js")]
    // Credential files at the root
    [InlineData("/credentials.json")]
    [InlineData("/secrets.json")]
    public void IsKnownStaticAssetPath_ReturnsFalse_ForUnknownExtensionPaths(string path)
    {
        ProbeDetector.IsKnownStaticAssetPath(path).Should().BeFalse();
    }

    [Theory]
    // wwwroot/js — plain and fingerprinted
    [InlineData("/js/mobile-nav.js")]
    [InlineData("/js/mobile-nav.4uezpgfs2f.js")]
    [InlineData("/js/scroll-manager.js")]
    // wwwroot/css — plain and fingerprinted
    [InlineData("/css/base.css")]
    [InlineData("/css/base.abc123.css")]
    // Blazor framework
    [InlineData("/_framework/blazor.web.js")]
    [InlineData("/_framework/dotnet.native.wasm")]
    // Root-level app bundle assets
    [InlineData("/TechHub.Web.styles.css")]
    [InlineData("/TechHub.Web.fwv5rmn5un.styles.css")]
    [InlineData("/TechHub.Web.lib.module.js")]
    // Blazor collocated component JS files
    [InlineData("/Components/Layout/ReconnectModal.p0sww062j0.razor.js")]
    [InlineData("/Components/Pages/SectionCollection.abc123.razor.js")]
    // RCL static assets
    [InlineData("/_content/SomeLib/script.js")]
    [InlineData("/_content/SomeLib/styles.css")]
    // wwwroot/images
    [InlineData("/images/ghc-handbook.jpg")]
    [InlineData("/images/section-backgrounds/ai.webp")]
    [InlineData("/images/svg/logo.svg")]
    // Root-level well-known files
    [InlineData("/favicon.ico")]
    [InlineData("/robots.txt")]
    [InlineData("/sitemap.xml")]
    // RSS feeds
    [InlineData("/all/feed.xml")]
    [InlineData("/ai/feed.xml")]
    [InlineData("/github-copilot/feed.xml")]
    public void IsKnownStaticAssetPath_ReturnsTrue_ForKnownAssets(string path)
    {
        ProbeDetector.IsKnownStaticAssetPath(path).Should().BeTrue();
    }

    [Theory]
    // Extension-less paths are always allowed through (Blazor routes)
    [InlineData("/")]
    [InlineData("/github-copilot")]
    [InlineData("/github-copilot/features")]
    [InlineData("/all")]
    // Known static asset paths are allowed through
    [InlineData("/js/mobile-nav.4uezpgfs2f.js")]
    [InlineData("/css/base.css")]
    [InlineData("/favicon.ico")]
    [InlineData("/all/feed.xml")]
    // Null / empty treated as extension-less
    [InlineData(null)]
    [InlineData("")]
    public void IsKnownStaticAssetOrExtensionless_ReturnsTrue_ForExtensionlessAndKnownAssets(string? path)
    {
        ProbeDetector.IsKnownStaticAssetOrExtensionless(path).Should().BeTrue();
    }

    [Theory]
    // Any file extension at an unknown path → false
    [InlineData("/devops/js/mobile-nav.4uezpgfs2f.js")]
    [InlineData("/cert.pem")]
    [InlineData("/secrets.json")]
    [InlineData("/random.xml")]
    [InlineData("/js/bundle.js.map")]
    public void IsKnownStaticAssetOrExtensionless_ReturnsFalse_ForUnknownExtensionPaths(string path)
    {
        ProbeDetector.IsKnownStaticAssetOrExtensionless(path).Should().BeFalse();
    }
}
