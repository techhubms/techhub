# Content Processing

## Overview

Content is automatically ingested from RSS feeds, categorized using AI-powered processing,
and stored exclusively in the PostgreSQL database.

## Content Architecture

### Database as Source of Truth

The PostgreSQL database is the **single source of truth** for all content. Content is written
directly to the database by **`ContentProcessingBackgroundService`** — a background service
running inside `TechHub.Api` that downloads RSS feeds, categorizes content with Azure OpenAI,
and writes directly to the database.

### Environment Strategy

| Environment | Content Source |
|-------------|---------------|
| Production | `ContentProcessingBackgroundService` writes RSS content directly every 60 minutes (`ContentProcessor:IntervalMinutes`) |
| Staging | Scheduled processing is disabled. Data comes from a production snapshot (`scripts/Restore-Database.ps1`) |
| PR previews | Scheduled jobs are disabled through environment variables. Manual triggers still work |
| Local development | Scheduled processing is disabled. Data comes from a production snapshot |

For local development, see [running-and-testing.md](running-and-testing.md) for setup instructions.

### Database Restore (Local/Disk)

To populate a local environment with real data, or to save a snapshot to disk:

```powershell
# Restore production data to local Docker Compose database
./scripts/Restore-Database.ps1 -Target local

# Dump production data to .tmp/ without restoring
./scripts/Restore-Database.ps1 -Target disk
```

The `-Target` parameter accepts only `local` and `disk`. Requires VPN access to the production environment and PostgreSQL client tools installed.

## RSS Feed Processing

The Tech Hub automatically processes RSS feeds from Microsoft and technology sources to keep
content current. The pipeline is implemented entirely in C# and runs inside `TechHub.Api`.

### Content Processing Pipeline

`ContentProcessingBackgroundService` (`IHostedService`) runs on a configurable schedule
(default: every 60 minutes, see [Scheduling](#scheduling)) and performs:

1. **Feed ingestion** — Downloads and parses RSS/Atom XML from all enabled feeds in the database
2. **Content fetching** — Fetches the full article HTML for non-YouTube items. **YouTube video items
   are processed using available metadata only** (title, description, feed tags, YouTube API tags) —
   automatic transcript fetching is intentionally disabled to avoid blocking in cloud environments
3. **AI categorization** — Sends content to Azure OpenAI (`AiCategorizationService`) using the
   system prompt embedded in `TechHub.Infrastructure/Data/Resources/system-message.md`
4. **Deduplication** — Checks `processed_urls` table to skip already-attempted URLs
5. **Database write** — Upserts into `content_items` + `content_tags_expanded`; records the run
   in `content_processing_jobs`

Items that the AI determines are off-topic or low quality are skipped and recorded in
`processed_urls` with status `skipped`.

### Manual Transcript Support

For YouTube videos, transcripts can be provided manually in two ways:

#### Via Admin UI (single video)

- **When adding a new video**: In the "Add Content" modal, paste the transcript in the Transcript
  field. The AI will generate a full summary based on the transcript.
- **When editing an existing video**: Open the content item editor, expand the "Apply Transcript"
  section, paste the transcript, and click "Apply Transcript". The AI regenerates excerpt, content,
  tags, and sections and saves those changes immediately. Review the result, and click Save only if
  you make additional manual edits afterward.

The edit flow always updates the existing item in-place (preserving slug and collection) — it never
deletes and re-adds items, so URLs remain stable.

#### Via PowerShell batch script (multiple videos)

Use `scripts/Fetch-VideoTranscripts.ps1` to fetch transcripts using yt-dlp and apply them to
existing videos in batch:

```powershell
# Process a single video by slug
./Fetch-VideoTranscripts.ps1 -Slug "github-copilot-overview"

# Process multiple specific videos
./Fetch-VideoTranscripts.ps1 -Slugs "slug-one,slug-two,slug-three"

# Process all videos in a content processing job
./Fetch-VideoTranscripts.ps1 -JobId 42

# Dry run — preview without applying changes
./Fetch-VideoTranscripts.ps1 -JobId 42 -DryRun
```

The script requires yt-dlp (`pip install yt-dlp`) and a running TechHub API. `-ApiBaseUrl` defaults to
`https://localhost:5001`, and `-BearerToken` supplies credentials for deployed environments.

### Feed Configuration

RSS feeds are stored in the `rss_feed_configs` database table and managed directly via the admin UI at `/admin/feeds`.

**Required Fields**:

- **name**: Human-readable feed identifier
- **url**: RSS or Atom feed URL
- **output_dir**: Target collection directory (`_news`, `_blogs`, etc.)

**Optional Fields**:

- **enabled**: Boolean to enable/disable feed (default: true)

### Adding New Feeds

**Via Admin UI** (recommended):

1. Navigate to `/admin/feeds` (requires Azure AD authentication in deployed environments)
2. Click "Add Feed"
3. Fill in name, URL, output directory
4. Save — the feed is picked up on the next scheduled run

### AI Content Analysis

The system analyzes content using `src/TechHub.Infrastructure/Data/Resources/system-message.md`.

**Exclusion Rules** (applied first — if any apply, item is skipped):

- Biographical content, question-only posts, community posts under 200 words
- Sales pitches without educational value, non-English content, job postings
- Non-development Microsoft business products (Microsoft 365 Copilot, Dynamics, Intune, etc.)

> **Manual operations bypass the sales pitch rule.** Ad-hoc URL adds and transcript applications set `SkipSalesPitchCheck = true` on the `RawFeedItem`, which injects a `PROCESSING_OVERRIDE` line into the AI prompt instructing it to skip that exclusion. Batch RSS processing is unaffected.

**Inclusion Categories**:

- **AI**: Azure OpenAI, Copilot Studio, AI Foundry, Semantic Kernel, MCP
- **GitHub Copilot**: All Copilot editions, features, integrations (always paired with AI)
- **.NET**: C#, F#, ASP.NET Core, Entity Framework, Blazor, MAUI, Visual Studio
- **DevOps**: Azure DevOps, GitHub Actions, CI/CD, Agile, monitoring
- **Azure**: All Azure services, ARM/Bicep, Container Apps, cloud architecture
- **ML**: Azure ML, Azure Databricks, data science pipelines, custom model training
- **Security**: Microsoft Entra ID, Azure Key Vault, Microsoft Sentinel, zero trust

### AI Categorization Service

`AiCategorizationService` sends each item to an Azure OpenAI chat deployment through
`AiCompletionClient`. The client authenticates with Microsoft Entra ID (managed identity in Azure,
`az login` locally), so no API key is configured. Configured under `"AiCategorization"`:

| Setting | Default | Description |
|---|---|---|
| `Endpoint` | empty | Azure OpenAI endpoint URL |
| `DeploymentName` | `gpt-5.2` in `appsettings.json` | Chat deployment to call |
| `ApiVersion` | `2025-04-01-preview` in `appsettings.json` | Azure OpenAI REST API version |
| `MaxRetries` | `3` | Attempts per item on rate limits and transient failures |
| `RateLimitDelaySeconds` | `15` | Wait between attempts on a rate limit without a `Retry-After` header (multiplied by the attempt number) |
| `MaxContentLength` | `200000` | Declared in the options class, but nothing reads it today, so content is not truncated by this setting |

Behavior worth knowing:

- **Retries**: A `429` waits for the server's `Retry-After` value when present. Network errors and timeouts back off 5 seconds times the attempt number. The HTTP client itself does not retry, so all retry logic lives in the service.
- **Non-retryable responses**: Content filter blocks and context-length errors return immediately and are recorded as failures with the Azure message.
- **Response validation**: Responses that are empty, cut off (`finish_reason` other than `stop`), or not valid JSON are recorded as failures. A JSON sanitizer repairs invalid escape sequences before parsing.
- **Section filter**: Sections returned by the AI that are not one of the seven configured sections are discarded.
- **Content hash**: `content_hash` is the SHA-256 of title, content and excerpt.

### Roundup Metadata

The AI extracts roundup metadata for each included item (stored in `ai_metadata` JSONB column):

- `roundup_summary` — 1-2 sentence ready-to-use summary
- `key_topics` — e.g. `["MCP", "Semantic Kernel", "RAG"]`
- `roundup_relevance` — `high` / `medium` / `low`
- `topic_type` — `announcement`, `tutorial`, `ga-release`, etc.
- `impact_level` — significance rating
- `time_sensitivity` — `immediate` / `this-week` / `this-month` / `long-term`

The weekly roundup generation passes this metadata (including the relevance label) to the AI
prompt, but it does not use relevance to select articles (see [Roundup Pipeline](#roundup-pipeline)).

### Admin Dashboard

The admin area at `/admin` provides:

- **Dashboard** — Recent processing job history with status, duration, log output
- **Run Now** button — Triggers an immediate out-of-schedule run
- **RSS Feeds** — Full CRUD management of feed configurations
- **Processed URLs** — Browse all processed URLs with status/reason; remove entries to retry
- **Reviews** — Content review queue for proposed fixer changes (approve/reject before applying)

See [admin-authentication.md](admin-authentication.md) for authentication setup.

### Error Handling

- **Feed unavailability**: Individual feed failures do not stop the pipeline; other feeds continue
- **Content fetch failures**: Non-fatal; pipeline falls back to RSS metadata only
- **YouTube videos**: Processed using only available metadata (no automatic transcript fetch);
  transcripts can be provided manually via the admin UI or batch script
- **AI API failures**: Retried up to `MaxRetries` times (configurable in `AiCategorizationOptions`)
- **Rate limiting**: Configurable delay between AI calls (`RateLimitDelaySeconds`)

### Content Processor Configuration

Configured under `"ContentProcessor"` in `appsettings.json`:

| Setting | Default | Description |
|---|---|---|
| `Enabled` | `false` (`true` in Production) | Enables scheduled runs. Manual triggers ignore it |
| `IntervalMinutes` | `60` | Time between scheduled runs |
| `ItemAgeLimitDays` | `365` | RSS items older than this are skipped at ingestion |
| `RequestDelayMs` | `10000` | Delay between items within a run, to avoid AI and site rate limits |
| `RequestTimeoutSeconds` | `30` | Declared in the options class, but the HTTP clients use fixed timeouts configured in the API startup |
| `MaxItemsPerRun` | `0` | Maximum new items per run (`0` = unlimited) |
| `MaxYouTubeTagCount` | `15` | YouTube videos with more tags are assumed to be tag-spammed and their tags are ignored. `0` disables YouTube tag fetching |
| `FailedUrlRetentionDays` | `7` | Failed URL records older than this are purged so the URLs are retried. `0` keeps them forever |
| `PurgeJobKeepCount` | `500` | Number of recent job records kept. Older ones are deleted at the end of each run |
| `BrowserUserAgent` | none (required) | User-Agent for article and YouTube requests. Startup fails when it is empty |
| `SubcollectionRules` | see below | Rules that route items into lookup tables |

Items whose publication date is in the future are capped to the processing date at ingestion.

### Dedicated Lookup Table Rules

Content items can be automatically written to dedicated lookup tables based on the feed name and
video/article title. This is configured in `appsettings.json` under `ContentProcessor.SubcollectionRules`.

Each rule has three fields:

| Field | Description |
|---|---|
| `FeedName` | Feed name to match (exact, case-insensitive) |
| `TitlePattern` | Title pattern with `*` wildcard support (case-insensitive) |
| `Subcollection` | Target lookup table identifier (`"vscode-updates"` writes to `vscode_update_items`) |

**Example configuration**:

```json
{
  "ContentProcessor": {
    "SubcollectionRules": [
      {
        "FeedName": "Fokko at Work YouTube",
        "TitlePattern": "Visual Studio Code and GitHub Copilot*",
        "Subcollection": "vscode-updates"
      }
    ]
  }
}
```

This rule automatically identifies any video from the
"Fokko at Work YouTube" feed whose title starts with "Visual Studio Code and GitHub Copilot"
as a VS Code update and inserts a row into `vscode_update_items`.
These videos then appear on the VS Code Updates page at `/github-copilot/vscode-updates`.

Rules are applied after AI categorization succeeds, so only items that pass the AI filter
are written to lookup tables. Manually added items that are already in the lookup table
are not affected.

## Scheduling

Three hosted services in `TechHub.Api` run the background jobs. Each one waits for startup
(migrations) to finish before doing anything.

| Service | Schedule | Trigger types |
|---|---|---|
| `ContentProcessingBackgroundService` | On startup (when enabled), then every `IntervalMinutes` | `scheduled`, `manual` |
| `RoundupGeneratorBackgroundService` | Every Monday at `RunHourUtc` | `scheduled`, `manual` (including regenerate) |
| `ContentFixerBackgroundService` | None | `manual` only |

Every run is recorded in `content_processing_jobs` with one of these job types:

| Job type | Written by |
|---|---|
| `content-processing` | Scheduled and manual feed processing |
| `ad-hoc-processing` | Single URL added from the admin UI |
| `roundup-generation` | Weekly roundup generation |
| `content-cleanup` | Content fixer |

Job statuses are `running`, `completed`, `failed` and `aborted`. Jobs still `running` after a crash
or restart are marked aborted on the next startup.

### Enable Gates

A scheduled run happens only when **both** gates are open:

1. The configuration flag (`ContentProcessor:Enabled` or `RoundupGenerator:Enabled`) is `true`. Both
   default to `false` and are switched on only in Production. PR preview environments override them
   with environment variables so a restored production database cannot trigger scheduled runs.
2. The job's row in the `background_job_settings` table (`ContentProcessor`, `RoundupGenerator`) has
   `enabled = true`. Admins can change it at runtime on `/admin/settings`.

**Manual triggers bypass both gates.** An admin can always start a run from the dashboard, even in
environments where scheduling is off. The newsletter job uses the same table, see
[newsletter.md](newsletter.md). Admin endpoints are described in [admin.md](admin.md).

## Weekly Roundups

Weekly roundups are generated entirely by `RoundupGeneratorBackgroundService`, a background
service running inside `TechHub.Api`. It fires every Monday at `RunHourUtc` (default 08:00 UTC)
and writes section-specific roundups directly to the `content_items` table (`collection_name = 'roundups'`).
Each section roundup is generated sequentially to avoid AI throttling.

### Roundup Pipeline

The generator covers the previous Monday-to-Sunday week and loops over every configured section
except `all`. For each section:

1. **Article loading** — Queries `content_items` for items in that section (`sections_bitmask`),
   excluding roundups, with `created_at` inside the target week. An item that belongs to several
   sections is therefore used by each of them
2. **AI metadata backfill** — Any articles missing `ai_metadata` are categorized on-the-fly before proceeding
3. **Existing check** — Sections whose roundup for that week already exists are skipped
4. **Step 1/5** — AI creates news-style narrative stories per section
5. **Step 2/5** — AI adds continuity by comparing with the previous week's roundup
6. **Step 3/5** — AI condenses the content paragraph-by-paragraph. Skipped when `CondensingEnabled` is `false`
7. **Step 4/5** — AI generates metadata: `title`, `tags`, `introduction`
8. **Step 5/5** — Table of contents is built from `##`/`###` headers (pure C#)
9. **DB write** — Upserts one roundup per section into `content_items` + `content_tags_expanded`

**Relevance is not used for selection.** A `RoundupRelevanceFilter` (high relevance always, filled up
to `MinArticlesPerSection` with medium and low) is registered in dependency injection, but the
generator never calls it. All articles of the week go to the AI, each labelled with its
`roundup_relevance`. As a result `MinArticlesPerSection` has no effect today, and the
`NoArticlesAfterFiltering` outcome is never produced.

Writing style guidelines (`docs/writing-style-guidelines.md`) are injected into every AI step
prompt to ensure consistent tone and style throughout the roundup.

### Roundup Configuration

Configured under `"RoundupGenerator"` in `appsettings.json`:

| Setting | Default | Description |
|---|---|---|
| `Enabled` | `false` (`true` in Production) | Enables scheduled roundup generation. Manual triggers ignore it |
| `RunHourUtc` | `8` | UTC hour to fire on Monday |
| `MinArticlesPerSection` | `10` | Used only by the unused relevance filter, so it currently has no effect |
| `CondensingEnabled` | `true` in code, `false` in `appsettings.json` | Runs the condensing step (step 3). The shipped configuration skips it |
| `RateLimitDelaySeconds` | `15` | Delay between AI calls |
| `MaxRetries` | `3` | Max retries per AI call |

## Troubleshooting

### Processed URL Was Incorrectly Skipped

If an article was skipped with an unexpected reason:

1. Open the admin panel at `/admin/processed-urls`
2. Find the entry and click **Copy** to copy all details to the clipboard
3. Check the `Reason` field to understand the AI's decision
4. If the decision was wrong, click **Remove** to delete the entry — it will be retried on the
   next scheduled run

## Content Fixer

`ContentFixerBackgroundService` performs bulk content quality fixes across all items in the
database. It runs on-demand via the admin dashboard (manual trigger only), is recorded as a
`content-cleanup` job, and clears the content cache when changes were applied directly.

### Fix Operations

| Operation | Description |
|---|---|
| **Tag normalization** | Adds missing collection/section tags, removes deprecated tags |
| **Author normalization** | Renames the author `Tech Hub Team` to `TechHub`. This is the only rule |
| **Markdown repair** | Fixes formatting issues (heading spacing, whitespace, HTML entities) |
| **Markdown validation** | Detects structural issues via Markdig AST (empty headings, broken links) |

### Content Review Queue

When the content fixer runs, proposed changes are queued in the `content_reviews` table for
admin approval instead of being applied directly. This provides a review step before any
automated changes are applied to content.

**Review workflow**:

1. Admin triggers a content fixer run from the dashboard
2. The fixer scans all content and creates review records for proposed changes
3. Admin navigates to **Reviews** (`/admin/reviews`) to see pending changes
4. Each review shows the change type (tags/markdown/author/validation), original value, and fixed value
5. Slug column links to the live article (when `primary_section_name` is available)
6. Admin can **Approve** or **Reject** individual changes, or use **Approve All** / **Reject All**
   for bulk actions
7. Admin can click the **Edit** button to open a modal and manually fix the content before approving
8. Approved changes are applied to `content_items`; rejected changes are discarded

**Review statuses**: `pending`, `approved`, `rejected`

**Change types**: `tags` (tag normalization), `markdown` (formatting fixes), `author` (name standardization), `validation` (structural issues — informational, no automatic fix)

### Markdown Preview

The admin area provides a markdown preview endpoint (`POST /api/admin/content-items/preview-markdown`)
that renders raw markdown to HTML. This is used in the review page to preview before/after
markdown changes.

### Admin Endpoints

| Endpoint | Method | Description |
|---|---|---|
| `/api/admin/reviews` | GET | List reviews filtered by status |
| `/api/admin/reviews/summary` | GET | Get pending/approved/rejected counts |
| `/api/admin/reviews/{id}/approve` | POST | Approve and apply a single change |
| `/api/admin/reviews/{id}/reject` | POST | Reject a single change |
| `/api/admin/reviews/approve-all` | POST | Approve all pending reviews |
| `/api/admin/reviews/reject-all` | POST | Reject all pending reviews |
| `/api/admin/reviews/{id}` | PUT | Update fixed value of a pending review |
| `/api/admin/content-items/preview-markdown` | POST | Render markdown to HTML |
