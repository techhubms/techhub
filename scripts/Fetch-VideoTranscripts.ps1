#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Fetches YouTube video transcripts using yt-dlp and applies them to existing video content items.

.DESCRIPTION
    Uses yt-dlp to download transcripts for YouTube videos stored in the TechHub database, then
    calls the admin API to regenerate AI content for each video using the fetched transcript.

    This script is useful for backfilling transcript-based content for videos that were originally
    processed without transcripts (e.g., after automatic transcript fetching was disabled).

    Transcripts are fetched using best-practice yt-dlp settings:
    - Prefers manually uploaded subtitles over auto-generated captions
    - Falls back to auto-generated English captions when no manual subtitles exist
    - Downloads VTT format and converts to plain text
    - Cleans up temporary files after processing

    Prerequisites:
    - yt-dlp installed and on PATH (install with: pip install yt-dlp)
    - TechHub API running and accessible at the configured base URL
    - Valid admin credentials (bearer token or local dev mode)

.PARAMETER Slug
    Optional. A single video slug to process. The collection is always 'videos'.
    Mutually exclusive with JobId.

.PARAMETER Slugs
    Optional. A comma-separated list of video slugs to process. The collection is always 'videos'.
    Mutually exclusive with JobId.

.PARAMETER JobId
    Optional. Process all videos in the specified content processing job.
    Mutually exclusive with Slug/Slugs.

.PARAMETER ApiBaseUrl
    Base URL of the TechHub API. Defaults to http://localhost:5244 (local development).

.PARAMETER BearerToken
    Optional. Bearer token for authentication. When omitted, relies on the AdminOnly policy
    allowing unauthenticated access (local development mode).

.PARAMETER DryRun
    When specified, fetches transcripts and logs what would be done but does not call the API.

.EXAMPLE
    # Process a single video by slug
    ./Fetch-VideoTranscripts.ps1 -Slug "github-copilot-features-deep-dive"

.EXAMPLE
    # Process multiple specific videos
    ./Fetch-VideoTranscripts.ps1 -Slugs "slug-one,slug-two,slug-three"

.EXAMPLE
    # Process all videos in a job
    ./Fetch-VideoTranscripts.ps1 -JobId 42

.EXAMPLE
    # Dry run — show what would happen without calling the API
    ./Fetch-VideoTranscripts.ps1 -JobId 42 -DryRun
#>

[CmdletBinding(DefaultParameterSetName = 'Slug')]
param(
    [Parameter(ParameterSetName = 'Slug')]
    [string]$Slug,

    [Parameter(ParameterSetName = 'Slugs')]
    [string]$Slugs,

    [Parameter(ParameterSetName = 'Job')]
    [long]$JobId,

    [string]$ApiBaseUrl = 'http://localhost:5244',

    [string]$BearerToken = '',

    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

# ── Helpers ──────────────────────────────────────────────────────────────────

function Write-Step {
    param([string]$Message)
    Write-Host "`n[$(Get-Date -Format 'HH:mm:ss')] $Message" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "  ✓ $Message" -ForegroundColor Green
}

function Write-Warn {
    param([string]$Message)
    Write-Host "  ⚠ $Message" -ForegroundColor Yellow
}

function Write-Fail {
    param([string]$Message)
    Write-Host "  ✗ $Message" -ForegroundColor Red
}

function Get-ApiHeaders {
    $headers = @{
        'Content-Type' = 'application/json'
    }
    if ($BearerToken -ne '') {
        $headers['Authorization'] = "Bearer $BearerToken"
    }
    return $headers
}

function Get-VideosByJobId {
    param([long]$JobId)

    Write-Step "Fetching videos for job $JobId from API…"
    $url = "$ApiBaseUrl/api/admin/processed-urls?jobId=$JobId&collectionName=videos&status=success&pageSize=500"
    try {
        $response = Invoke-RestMethod -Uri $url -Headers (Get-ApiHeaders) -Method Get
        $slugs = $response.items | Where-Object { $null -ne $_.slug } | Select-Object -ExpandProperty slug
        Write-Success "Found $($slugs.Count) video slug(s) in job $JobId"
        return $slugs
    }
    catch {
        throw "Failed to fetch videos for job $($JobId): $_"
    }
}

function Get-YouTubeUrlForSlug {
    param([string]$Slug)

    $url = "$ApiBaseUrl/api/admin/content-items/edit-data?collection=videos&slug=$([Uri]::EscapeDataString($Slug))"
    try {
        $item = Invoke-RestMethod -Uri $url -Headers (Get-ApiHeaders) -Method Get
        return $item.externalUrl
    }
    catch {
        Write-Warn "Could not fetch external URL for slug '$Slug': $_"
        return $null
    }
}

function Get-TranscriptViaYtDlp {
    param([string]$VideoUrl)

    # Create a temporary directory for yt-dlp output
    $tempDir = Join-Path ([System.IO.Path]::GetTempPath()) "techhub-transcript-$(New-Guid)"
    New-Item -ItemType Directory -Path $tempDir | Out-Null

    try {
        # Best-practice yt-dlp arguments:
        # --write-subs: download manual subtitles if available
        # --write-auto-subs: fall back to auto-generated captions
        # --sub-langs en.*: prefer English subtitles (with any regional variant)
        # --skip-download: only download subtitles, not the video
        # --sub-format vtt: use VTT format (most reliable for parsing)
        # --no-playlist: ensure we only process the single video
        # --quiet: suppress progress output
        # --no-warnings: suppress non-fatal warnings
        $ytDlpArgs = @(
            '--write-subs',
            '--write-auto-subs',
            '--sub-langs', 'en.*',
            '--skip-download',
            '--sub-format', 'vtt',
            '--no-playlist',
            '--quiet',
            '--no-warnings',
            '-o', (Join-Path $tempDir 'transcript'),
            $VideoUrl
        )

        $proc = Start-Process -FilePath 'yt-dlp' -ArgumentList $ytDlpArgs `
            -NoNewWindow -Wait -PassThru -RedirectStandardError (Join-Path $tempDir 'stderr.txt')

        if ($proc.ExitCode -ne 0) {
            $errText = ''
            $stderrFile = Join-Path $tempDir 'stderr.txt'
            if (Test-Path $stderrFile) {
                $errText = Get-Content $stderrFile -Raw
            }
            Write-Warn "yt-dlp exited with code $($proc.ExitCode) for $VideoUrl"
            if ($errText) {
                Write-Warn "  stderr: $errText"
            }
            return $null
        }

        # Find the VTT file (yt-dlp uses the video ID in the filename)
        $vttFiles = Get-ChildItem -Path $tempDir -Filter '*.vtt' -ErrorAction SilentlyContinue
        if (-not $vttFiles -or $vttFiles.Count -eq 0) {
            Write-Warn "No VTT subtitle file found for $VideoUrl"
            return $null
        }

        # Use the first (and typically only) VTT file
        $vttContent = Get-Content -Path $vttFiles[0].FullName -Raw -Encoding UTF8
        return ConvertFrom-Vtt -VttContent $vttContent
    }
    finally {
        # Clean up temporary directory
        if (Test-Path $tempDir) {
            Remove-Item -Path $tempDir -Recurse -Force -ErrorAction SilentlyContinue
        }
    }
}

function ConvertFrom-Vtt {
    param([string]$VttContent)

    # Parse VTT format: remove headers, timestamps, cue settings, and HTML tags
    # VTT format:
    #   WEBVTT
    #   (optional header lines)
    #   (blank line)
    #   (optional cue identifier)
    #   HH:MM:SS.mmm --> HH:MM:SS.mmm (optional positioning)
    #   cue text...

    if ([string]::IsNullOrWhiteSpace($VttContent)) {
        return $null
    }

    $lines = $VttContent -split "`n"
    $textLines = [System.Collections.Generic.List[string]]::new()
    $seenTimestamps = [System.Collections.Generic.HashSet[string]]::new()

    $inCue = $false
    foreach ($line in $lines) {
        $trimmed = $line.Trim()

        # Skip WEBVTT header and NOTE blocks
        if ($trimmed -eq 'WEBVTT' -or $trimmed.StartsWith('NOTE ') -or $trimmed.StartsWith('STYLE')) {
            $inCue = $false
            continue
        }

        # Detect timestamp lines (HH:MM:SS.mmm --> HH:MM:SS.mmm)
        if ($trimmed -match '^\d{2}:\d{2}:\d{2}\.\d{3}\s*-->') {
            $inCue = $true
            continue
        }

        # Skip empty lines (cue separators)
        if ($trimmed -eq '') {
            $inCue = $false
            continue
        }

        if ($inCue) {
            # Remove HTML tags (e.g. <c>, <i>, <b>, <00:00:00.000>)
            $clean = $trimmed -replace '<[^>]+>', ''
            # Remove HTML entities
            $clean = $clean -replace '&amp;', '&' -replace '&lt;', '<' -replace '&gt;', '>' -replace '&quot;', '"' -replace '&#39;', "'"
            $clean = $clean.Trim()

            if ($clean -ne '' -and $seenTimestamps.Add($clean)) {
                $textLines.Add($clean)
            }
        }
    }

    if ($textLines.Count -eq 0) {
        return $null
    }

    return $textLines -join ' '
}

function Invoke-ApplyTranscript {
    param([string]$Slug, [string]$Transcript)

    $url = "$ApiBaseUrl/api/admin/content-items/apply-transcript?collection=videos&slug=$([Uri]::EscapeDataString($Slug))"
    $body = @{ Transcript = $Transcript } | ConvertTo-Json -Depth 2

    try {
        $response = Invoke-RestMethod -Uri $url -Headers (Get-ApiHeaders) -Method Post -Body $body
        return $response
    }
    catch {
        throw "API call failed for slug '$Slug': $_"
    }
}

# ── Main ──────────────────────────────────────────────────────────────────────

Write-Host "`nTechHub Video Transcript Fetcher" -ForegroundColor Magenta
Write-Host "====================================" -ForegroundColor Magenta

if ($DryRun) {
    Write-Host "  [DRY RUN MODE — no API changes will be made]" -ForegroundColor Yellow
}

# Verify yt-dlp is available
try {
    $ytDlpVersion = & yt-dlp --version 2>&1
    Write-Host "  yt-dlp version: $ytDlpVersion" -ForegroundColor Gray
}
catch {
    Write-Host "ERROR: yt-dlp is not installed or not on PATH." -ForegroundColor Red
    Write-Host "       Install it with: pip install yt-dlp" -ForegroundColor Red
    exit 1
}

# Resolve slug list
$slugList = @()

if ($PSCmdlet.ParameterSetName -eq 'Slug' -and $Slug -ne '') {
    $slugList = @($Slug)
}
elseif ($PSCmdlet.ParameterSetName -eq 'Slugs' -and $Slugs -ne '') {
    $slugList = $Slugs -split ',' | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne '' }
}
elseif ($PSCmdlet.ParameterSetName -eq 'Job') {
    $slugList = Get-VideosByJobId -JobId $JobId
}
else {
    Write-Host "ERROR: Specify -Slug, -Slugs, or -JobId." -ForegroundColor Red
    exit 1
}

if ($slugList.Count -eq 0) {
    Write-Warn "No slugs to process."
    exit 0
}

Write-Host "  Processing $($slugList.Count) video(s)…" -ForegroundColor Gray

$succeeded = 0
$skipped = 0
$failed = 0

foreach ($currentSlug in $slugList) {
    Write-Step "Processing: $currentSlug"

    # Get the YouTube URL for this slug
    $youtubeUrl = Get-YouTubeUrlForSlug -Slug $currentSlug
    if ([string]::IsNullOrWhiteSpace($youtubeUrl)) {
        Write-Warn "No YouTube URL found for slug '$currentSlug' — skipping"
        $skipped++
        continue
    }

    $isYouTube = $youtubeUrl -match '(youtube\.com|youtu\.be)'
    if (-not $isYouTube) {
        Write-Warn "URL is not a YouTube URL ($youtubeUrl) — skipping"
        $skipped++
        continue
    }

    Write-Host "  URL: $youtubeUrl" -ForegroundColor Gray

    # Fetch transcript
    Write-Host '  Fetching transcript via yt-dlp…' -ForegroundColor Gray
    try {
        $transcript = Get-TranscriptViaYtDlp -VideoUrl $youtubeUrl
    }
    catch {
        Write-Fail "Failed to run yt-dlp: $_"
        $failed++
        continue
    }

    if ([string]::IsNullOrWhiteSpace($transcript)) {
        Write-Warn "No transcript available for $youtubeUrl"
        $skipped++
        continue
    }

    $wordCount = ($transcript -split '\s+').Count
    Write-Host "  Transcript: $wordCount words" -ForegroundColor Gray

    if ($DryRun) {
        Write-Warn "[DRY RUN] Would call apply-transcript API for slug '$currentSlug'"
        $previewLength = [Math]::Min(200, $transcript.Length)
        $preview = $transcript.Substring(0, $previewLength)
        Write-Warn "[DRY RUN] Transcript preview: $preview…"
        $skipped++
        continue
    }

    # Apply transcript to the content item
    try {
        $result = Invoke-ApplyTranscript -Slug $currentSlug -Transcript $transcript
        Write-Success "Applied transcript for '$currentSlug' → title: $($result.title)"
        $succeeded++
    }
    catch {
        Write-Fail "Failed to apply transcript for '$currentSlug': $_"
        $failed++
    }
}

Write-Host "`n==============================" -ForegroundColor Magenta
Write-Host "  Succeeded : $succeeded" -ForegroundColor Green
Write-Host "  Skipped   : $skipped" -ForegroundColor Yellow
Write-Host "  Failed    : $failed" -ForegroundColor Red
Write-Host "==============================`n" -ForegroundColor Magenta

if ($failed -gt 0) {
    exit 1
}
