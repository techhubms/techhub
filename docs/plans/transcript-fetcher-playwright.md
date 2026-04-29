# Plan: TechHub.TranscriptFetcher (Playwright-based YouTube Transcript Fetcher)

## Problem

The current content processing pipeline uses **YoutubeExplode** (NuGet package) to fetch YouTube
closed captions/transcripts. This works locally but fails when running from Azure Container Apps
due to network/environment restrictions that YoutubeExplode cannot handle.

## Goal

Create a new standalone project `TechHub.TranscriptFetcher` that uses **Playwright with headless
Chromium** to fetch YouTube transcripts by navigating to the video page and extracting the caption
text from the DOM. The project provides:

1. A reusable **library class** (`TranscriptFetcher`) that can be instantiated and called
   from any .NET code
2. A **CLI entrypoint** for local/manual use

Later, if stable, this replaces the `YouTubeTranscriptService` (YoutubeExplode-based) in
`TechHub.Infrastructure` as the `IYouTubeTranscriptService` implementation.

## Current Architecture

- **Interface**: `IYouTubeTranscriptService` in `TechHub.Core/Interfaces/`
- **Implementation**: `YouTubeTranscriptService` in `TechHub.Infrastructure/Services/ContentProcessing/`
  - Uses `YoutubeExplode` NuGet package (v6.6.0)
  - Fetches closed caption manifest, selects best English track, builds plain text
  - Returns `TranscriptResult` (success with text or failure with reason)
- **Consumer**: `ArticleContentService` calls `IYouTubeTranscriptService.GetTranscriptAsync()`
- **Registration**: `Program.cs` registers as `Transient`
- **Behavior**: Prefers English manual captions, falls back to auto-generated, retries transient
  failures, truncates to 50,000 chars

## Design

### Project Structure

```text
src/TechHub.TranscriptFetcher/
  TechHub.TranscriptFetcher.csproj
  PlaywrightTranscriptFetcher.cs    # Main library class
  Program.cs                         # CLI entrypoint
```

### Project Configuration

- **Type**: Console application (`Exe`) with `net10.0` target framework
- **Key dependency**: `Microsoft.Playwright` (already in `Directory.Packages.props` at v1.59.0)
- **References**: `TechHub.Core` (for `IYouTubeTranscriptService` and `TranscriptResult`)
- **Add to**: `TechHub.slnx` under `/src/` folder

### PlaywrightTranscriptFetcher Class

```text
public class PlaywrightTranscriptFetcher : IYouTubeTranscriptService, IAsyncDisposable
```

**Responsibilities**:

- On construction/initialization: launch a headless Chromium browser via Playwright
- Implement `GetTranscriptAsync(string videoUrl, CancellationToken ct)` returning `TranscriptResult`
- Browser lifecycle: create once, reuse across multiple calls, dispose on teardown
- Lazy initialization pattern: browser launched on first call, not in constructor

**Transcript extraction approach**:

1. Navigate to the YouTube video page
2. Wait for the page to load
3. Click the "More" / "...more" button to expand the description area
4. Click "Show transcript" button to open the transcript panel
5. Wait for transcript entries to appear in the DOM
6. Extract all transcript text segments, join into plain text
7. Truncate to 50,000 chars (matching current behavior)
8. Return `TranscriptResult.Success(text)` or `TranscriptResult.Failure(reason)`

**Error handling**:

- Timeout waiting for elements: return failure (non-fatal, matching current behavior)
- Navigation errors: return failure
- Browser crash: reinitialize browser on next call
- Consent/cookie dialogs: handle YouTube's cookie consent banner if it appears

**Configuration**:

- Configurable timeout (default: 30 seconds per video)
- Headless mode (default: true, overridable for debugging)
- Optional slow-mo for debugging

### CLI Entrypoint (Program.cs)

Simple command-line interface:

```text
Usage: TechHub.TranscriptFetcher <youtube-id-or-url> [options]

Options:
  --headed          Run with visible browser (for debugging)
  --output <file>   Write transcript to file instead of stdout
  --timeout <secs>  Timeout per video (default: 30)
```

**Behavior**:

- Accept a YouTube video ID (e.g., `dQw4w9WgXcQ`) or full URL
- Print transcript text to stdout (or to file with `--output`)
- Exit code 0 on success, 1 on failure
- Print failure reason to stderr on failure

### Integration Path (Future)

When ready to replace YoutubeExplode in the pipeline:

1. Add `TechHub.TranscriptFetcher` as a project reference from `TechHub.Infrastructure`
   (or register directly in `TechHub.Api`)
2. Change DI registration in `Program.cs`:
   `builder.Services.AddSingleton<IYouTubeTranscriptService, PlaywrightTranscriptFetcher>()`
   (singleton because it manages its own browser instance)
3. Remove `YoutubeExplode` package reference from `TechHub.Infrastructure`
4. Ensure Playwright browsers are installed in the Container Apps Docker image
5. Update `content-processing.md` docs

## Implementation Steps

1. Create `src/TechHub.TranscriptFetcher/TechHub.TranscriptFetcher.csproj`
   - Output type `Exe`, target `net10.0`
   - Reference `Microsoft.Playwright` and `TechHub.Core`
2. Add the project to `TechHub.slnx` under `/src/`
3. Implement `PlaywrightTranscriptFetcher` class
   - Lazy browser initialization
   - DOM-based transcript extraction
   - Cookie consent handling
   - Proper `IAsyncDisposable` for browser cleanup
4. Implement `Program.cs` CLI with argument parsing
5. Install Playwright browsers (`pwsh bin/.../playwright.ps1 install chromium`)
6. Test manually against a few known YouTube videos
7. Write unit/integration tests in `tests/TechHub.TranscriptFetcher.Tests/`

## Risks and Considerations

- **DOM selectors are fragile**: YouTube's HTML structure can change without notice. The
  transcript panel selectors will need maintenance. Consider using aria labels and
  data-attribute selectors where available for resilience.
- **Performance**: Headless browser is heavier than HTTP-based YoutubeExplode. Each transcript
  fetch involves a full page load. Mitigate by reusing the browser instance and creating
  new pages (tabs) per request.
- **Container Apps deployment**: Playwright requires Chromium binaries and system dependencies.
  The Docker image must include these (Playwright provides a Docker base image for this).
- **Rate limiting**: YouTube may throttle or challenge automated browsers. Consider adding
  delays between requests and handling bot detection pages.
- **Consent dialogs**: YouTube shows cookie consent banners in some regions. Must be handled
  before interacting with the transcript panel.
- **No transcript available**: Some videos have no captions at all. Must handle gracefully,
  same as the current YoutubeExplode implementation.

## Files Changed

| File | Change |
|------|--------|
| `src/TechHub.TranscriptFetcher/TechHub.TranscriptFetcher.csproj` | New project file |
| `src/TechHub.TranscriptFetcher/PlaywrightTranscriptFetcher.cs` | New - main library class |
| `src/TechHub.TranscriptFetcher/Program.cs` | New - CLI entrypoint |
| `TechHub.slnx` | Add new project under `/src/` |
| `docs/content-processing.md` | Document the new project and future integration path |
| `docs/repository-structure.md` | Add TranscriptFetcher to project listing |
