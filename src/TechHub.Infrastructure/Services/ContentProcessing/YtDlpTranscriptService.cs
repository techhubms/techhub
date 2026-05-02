using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Logging;
using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Infrastructure.Services.ContentProcessing;

/// <summary>
/// Fetches YouTube video transcripts by shelling out to yt-dlp.
/// Downloads VTT subtitle files and parses them to plain text.
/// This is more resilient than YoutubeExplode in containerized environments
/// because yt-dlp handles YouTube's anti-bot measures better.
/// </summary>
public sealed partial class YtDlpTranscriptService : IDisposable
{
    private readonly ILogger<YtDlpTranscriptService> _logger;
    private readonly string _ytDlpPath;
    private readonly string? _cookieFilePath;
    private bool _disposed;

    /// <summary>Maximum transcript length in characters to avoid overloading the AI prompt.</summary>
    internal const int MaxTranscriptLength = 50_000;

    /// <summary>Regex to detect VTT timing lines like "00:00:05.790 --> 00:00:10.800".</summary>
    private static readonly Regex _timingLineRegex = TimingLineRegex();

    /// <summary>Regex to strip inline timing/formatting tags like &lt;00:00:19.039&gt;&lt;c&gt; and &lt;/c&gt;.</summary>
    private static readonly Regex _inlineTagRegex = InlineTagRegex();

    /// <summary>Regex to strip sound effect markers like [Music], [Applause], etc.</summary>
    private static readonly Regex _soundEffectRegex = SoundEffectRegex();

    public YtDlpTranscriptService(ILogger<YtDlpTranscriptService> logger, IOptions<ContentProcessorOptions>? options = null, string? ytDlpPath = null)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
        _ytDlpPath = ytDlpPath ?? "yt-dlp";

        var cookieString = options?.Value.YouTubeCookies;
        if (!string.IsNullOrWhiteSpace(cookieString))
        {
            try
            {
                _cookieFilePath = WriteCookiesFile(cookieString);
                _logger.LogDebug("yt-dlp cookies file written for transcript downloads");
            }
            catch (IOException ex)
            {
                _logger.LogWarning(ex, "Failed to write yt-dlp cookies file — proceeding without cookies");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Failed to write yt-dlp cookies file — proceeding without cookies");
            }
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            if (_cookieFilePath is not null)
            {
                try
                {
                    File.Delete(_cookieFilePath);
                }
                catch (IOException ex)
                {
                    _logger.LogDebug(ex, "Failed to delete yt-dlp cookies temp file");
                }
            }
        }
    }

    /// <summary>
    /// Fetches the transcript for a YouTube video using yt-dlp.
    /// Downloads English subtitles (manual preferred, auto-generated as fallback)
    /// and parses the VTT output to plain text.
    /// </summary>
    public async Task<TranscriptResult> GetTranscriptAsync(string videoUrl, int timeoutSeconds, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(videoUrl);
        ObjectDisposedException.ThrowIf(_disposed, this);

        string? tempDir = null;
        try
        {
            tempDir = Path.Combine(Path.GetTempPath(), $"yt-dlp-{Guid.NewGuid():N}");
            Directory.CreateDirectory(tempDir);

            var args = BuildYtDlpArguments(videoUrl, tempDir, timeoutSeconds, _cookieFilePath);

            _logger.LogDebug("Running yt-dlp for transcript: {Url}", videoUrl);

            var (exitCode, _, stderr) = await RunProcessAsync(
                _ytDlpPath, args, timeoutSeconds, ct);

            if (ct.IsCancellationRequested)
            {
                return TranscriptResult.Failure("Cancelled");
            }

            // Find the downloaded VTT file (yt-dlp names it <id>.en.vtt or similar)
            var vttFiles = Directory.GetFiles(tempDir, "*.vtt");
            if (vttFiles.Length == 0)
            {
                var reason = exitCode != 0
                    ? $"yt-dlp exited with code {exitCode}: {TruncateForLog(stderr)}"
                    : "No subtitle file was downloaded";
                _logger.LogInformation("yt-dlp transcript failed for {Url}: {Reason}", videoUrl, reason.Sanitize());
                return TranscriptResult.Failure(reason);
            }

            // Prefer manual English subtitle over auto-generated
            // yt-dlp names: <id>.en.vtt (manual) vs <id>.en-orig.vtt or similar
            var bestVtt = SelectBestVttFile(vttFiles);
            var vttContent = await File.ReadAllTextAsync(bestVtt, ct);
            var transcript = ParseVttToPlainText(vttContent);

            if (string.IsNullOrWhiteSpace(transcript))
            {
                _logger.LogInformation("yt-dlp downloaded empty transcript for {Url}", videoUrl);
                return TranscriptResult.Failure("Downloaded subtitle was empty");
            }

            _logger.LogInformation(
                "yt-dlp fetched transcript for {Url}: {Length} chars",
                videoUrl, transcript.Length);

            return TranscriptResult.Success(transcript);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (IOException ex)
        {
            _logger.LogWarning(ex, "yt-dlp transcript fetch failed for {Url}", videoUrl);
            return TranscriptResult.Failure($"yt-dlp error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "yt-dlp transcript fetch failed for {Url}", videoUrl);
            return TranscriptResult.Failure($"yt-dlp error: {ex.Message}");
        }
        catch (System.ComponentModel.Win32Exception ex)
        {
            // yt-dlp binary not found on PATH
            _logger.LogWarning(ex, "yt-dlp not found on PATH");
            return TranscriptResult.Failure($"yt-dlp not found: {ex.Message}");
        }
        finally
        {
            // Clean up temp directory
            if (tempDir != null && Directory.Exists(tempDir))
            {
                try
                {
                    Directory.Delete(tempDir, recursive: true);
                }
                catch (IOException ex)
                {
                    _logger.LogDebug(ex, "Failed to clean up temp directory: {Dir}", tempDir);
                }
            }
        }
    }

    /// <summary>
    /// Builds the command-line arguments for yt-dlp to download subtitles only.
    /// </summary>
    internal static string BuildYtDlpArguments(string videoUrl, string outputDir, int timeoutSeconds, string? cookieFilePath = null)
    {
        ArgumentNullException.ThrowIfNull(videoUrl);
        ArgumentNullException.ThrowIfNull(outputDir);

        // Use Path.Join to avoid Path.Combine silently dropping earlier arguments
        var outputTemplate = Path.Join(outputDir, "%(id)s");

        // --write-sub: download manual subtitles
        // --write-auto-sub: download auto-generated subtitles as fallback
        // --sub-lang en: prefer English subtitles
        // --sub-format vtt: request WebVTT format
        // --skip-download: don't download the video itself
        // --no-warnings: reduce noise in stderr
        // --socket-timeout: network timeout
        var parts = new List<string>
        {
            "--write-sub",
            "--write-auto-sub",
            "--sub-lang", "en",
            "--sub-format", "vtt",
            "--skip-download",
            "--no-warnings",
            "--socket-timeout", timeoutSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture),
        };

        if (!string.IsNullOrEmpty(cookieFilePath))
        {
            parts.Add("--cookies");
            parts.Add($"\"{cookieFilePath}\"");
        }

        parts.Add("-o");
        parts.Add($"\"{outputTemplate}\"");
        parts.Add($"\"{videoUrl}\"");

        return string.Join(' ', parts);
    }

    /// <summary>
    /// Writes the cookie string to a Netscape-format cookies file that yt-dlp can read via --cookies.
    /// The caller is responsible for deleting the file when done.
    /// </summary>
    /// <param name="cookieString">
    /// Semicolon-delimited "name=value" pairs as stored in <see cref="ContentProcessorOptions.YouTubeCookies"/>.
    /// </param>
    /// <returns>The path to the written temp file.</returns>
    internal static string WriteCookiesFile(string cookieString)
    {
        ArgumentNullException.ThrowIfNull(cookieString);

        var path = Path.Combine(Path.GetTempPath(), $"yt-dlp-cookies-{Guid.NewGuid():N}.txt");

        var sb = new StringBuilder();
        sb.AppendLine("# Netscape HTTP Cookie File");

        foreach (var entry in cookieString.Split(';'))
        {
            if (string.IsNullOrWhiteSpace(entry))
            {
                continue;
            }

            var sepIdx = entry.IndexOf('=', StringComparison.Ordinal);
            if (sepIdx <= 0)
            {
                continue;
            }

            var name = entry[..sepIdx].Trim();
            var value = entry[(sepIdx + 1)..].Trim();

            // Determine domain/security based on cookie name prefix conventions.
            // __Host- cookies are host-only (no leading dot, subdomains=FALSE, always secure).
            //   __Host-GAPS is a Google Accounts cookie, so its domain is accounts.google.com.
            //   Other __Host- cookies are assumed to be youtube.com.
            // __Secure- cookies require HTTPS (secure=TRUE).
            // All others default to .youtube.com with secure=FALSE.
            string domain;
            string includeSubdomains;
            string secure;

            if (name.StartsWith("__Host-", StringComparison.Ordinal))
            {
                domain = name == "__Host-GAPS" ? "accounts.google.com" : "youtube.com";
                includeSubdomains = "FALSE";
                secure = "TRUE";
            }
            else if (name.StartsWith("__Secure-", StringComparison.Ordinal))
            {
                domain = ".youtube.com";
                includeSubdomains = "TRUE";
                secure = "TRUE";
            }
            else
            {
                domain = ".youtube.com";
                includeSubdomains = "TRUE";
                secure = "FALSE";
            }

            // Netscape format: domain<TAB>subdomain-flag<TAB>path<TAB>secure<TAB>expiry<TAB>name<TAB>value
            sb.Append(domain).Append('\t')
              .Append(includeSubdomains).Append('\t')
              .Append('/').Append('\t')
              .Append(secure).Append('\t')
              .Append('0').Append('\t')
              .Append(name).Append('\t')
              .AppendLine(value);
        }

        File.WriteAllText(path, sb.ToString());
        return path;
    }

    /// <summary>
    /// Parses a WebVTT subtitle file into plain text, stripping timing codes,
    /// formatting tags, duplicate lines, and sound effect markers.
    /// </summary>
    internal static string ParseVttToPlainText(string vttContent, int maxLength = MaxTranscriptLength)
    {
        ArgumentNullException.ThrowIfNull(vttContent);

        var sb = new StringBuilder();
        var seenLines = new HashSet<string>(StringComparer.Ordinal);
        var lines = vttContent.Split('\n');

        foreach (var line in lines.Select(static l => l.Trim()))
        {

            // Skip empty lines, WEBVTT header, Kind/Language metadata, NOTE blocks
            if (string.IsNullOrWhiteSpace(line)
                || line.StartsWith("WEBVTT", StringComparison.Ordinal)
                || line.StartsWith("Kind:", StringComparison.Ordinal)
                || line.StartsWith("Language:", StringComparison.Ordinal)
                || line.StartsWith("NOTE", StringComparison.Ordinal))
            {
                continue;
            }

            // Skip timing lines (e.g., "00:00:05.790 --> 00:00:10.800 align:start position:0%")
            if (_timingLineRegex.IsMatch(line))
            {
                continue;
            }

            // Strip inline timing/formatting tags
            var cleaned = _inlineTagRegex.Replace(line, "");

            // Strip sound effect markers like [Music], [Applause]
            cleaned = _soundEffectRegex.Replace(cleaned, "");

            cleaned = cleaned.Trim();

            if (string.IsNullOrWhiteSpace(cleaned))
            {
                continue;
            }

            // Deduplicate lines (auto-generated VTT repeats lines across cue boundaries)
            if (!seenLines.Add(cleaned))
            {
                continue;
            }

            if (sb.Length > 0)
            {
                sb.Append(' ');
            }

            sb.Append(cleaned);

            if (sb.Length > maxLength)
            {
                break;
            }
        }

        var result = sb.ToString().Trim();
        return result.Length > maxLength ? result[..maxLength] : result;
    }

    /// <summary>
    /// Selects the best VTT file from those downloaded by yt-dlp.
    /// Prefers files without "orig" in the name (indicating manual subs).
    /// </summary>
    internal static string SelectBestVttFile(string[] vttFiles)
    {
        // Prefer manual subtitle (typically just .en.vtt) over auto-generated (.en-orig.vtt)
        var manual = Array.Find(vttFiles, f =>
            !f.Contains("-orig", StringComparison.OrdinalIgnoreCase)
            && !f.Contains("auto", StringComparison.OrdinalIgnoreCase));
        return manual ?? vttFiles[0];
    }

    private async Task<(int ExitCode, string Stdout, string Stderr)> RunProcessAsync(
        string fileName, string arguments, int timeoutSeconds, CancellationToken ct)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };

        var stdoutBuilder = new StringBuilder();
        var stderrBuilder = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                stdoutBuilder.AppendLine(e.Data);
            }
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                stderrBuilder.AppendLine(e.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        cts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

        try
        {
            await process.WaitForExitAsync(cts.Token);
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            // Timeout — kill the process
            try
            {
                process.Kill(entireProcessTree: true);
            }
            catch (InvalidOperationException)
            {
                // Process already exited
            }

            _logger.LogWarning("yt-dlp process timed out after {Timeout}s", timeoutSeconds);
            return (-1, stdoutBuilder.ToString(), "Process timed out");
        }

        return (process.ExitCode, stdoutBuilder.ToString(), stderrBuilder.ToString());
    }

    private static string TruncateForLog(string value, int maxLength = 500)
    {
        return value.Length <= maxLength ? value : value[..maxLength] + "...";
    }

    [GeneratedRegex(@"^\d{2}:\d{2}:\d{2}\.\d{3}\s*-->")]
    private static partial Regex TimingLineRegex();

    [GeneratedRegex(@"<[^>]+>")]
    private static partial Regex InlineTagRegex();

    [GeneratedRegex(@"\[[^\]]*\]")]
    private static partial Regex SoundEffectRegex();
}
