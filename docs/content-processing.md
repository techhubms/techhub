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
| Production | `ContentProcessingBackgroundService` writes RSS content directly every 15 minutes |
| Staging | Database restore from production snapshot (`scripts/Restore-Database.ps1`) |
| Local development | Database restore from production snapshot |

For local development, see [running-and-testing.md](running-and-testing.md) for setup instructions.

### Database Restore (Staging/Local)

To populate a non-production environment with real data:

```powershell
# Restore production data to local Docker Compose database
./scripts/Restore-Database.ps1 -Target local

# Restore production data to staging
./scripts/Restore-Database.ps1 -Target staging
```

Requires VPN access to the production environment and PostgreSQL client tools installed.

## RSS Feed Processing

The Tech Hub automatically processes RSS feeds from Microsoft and technology sources to keep
content current. The pipeline is implemented entirely in C# and runs inside `TechHub.Api`.

### Content Processing Pipeline

`ContentProcessingBackgroundService` (`IHostedService`) runs on a configurable schedule
(default: every 15 minutes) and performs:

1. **Feed ingestion** — Downloads and parses RSS/Atom XML from all enabled feeds in the database
2. **Content fetching** — Fetches the full article HTML for each item, or closed captions
   (transcripts) for YouTube video items. Transcripts are fetched using **YoutubeExplode**
   (with configured HTTP client, browser UA, and persistent cookies) with **yt-dlp** as a
   fallback. Both fetchers can be independently enabled/disabled via `YouTubeExplodeEnabled`
   and `YtDlpEnabled` in `ContentProcessor` settings. Failures are non-fatal.
3. **AI categorization** — Sends content to Azure OpenAI (`AiCategorizationService`) using the
   system prompt embedded in `TechHub.Infrastructure/Data/Resources/system-message.md`
4. **Deduplication** — Checks `processed_urls` table to skip already-attempted URLs
5. **Database write** — Upserts into `content_items` + `content_tags_expanded`; records the run
   in `content_processing_jobs`

Items that the AI determines are off-topic or low quality are skipped and recorded in
`processed_urls` with status `skipped`.

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

**Inclusion Categories**:

- **AI**: Azure OpenAI, Copilot Studio, AI Foundry, Semantic Kernel, MCP
- **GitHub Copilot**: All Copilot editions, features, integrations (always paired with AI)
- **.NET**: C#, F#, ASP.NET Core, Entity Framework, Blazor, MAUI, Visual Studio
- **DevOps**: Azure DevOps, GitHub Actions, CI/CD, Agile, monitoring
- **Azure**: All Azure services, ARM/Bicep, Container Apps, cloud architecture
- **ML**: Azure ML, Azure Databricks, data science pipelines, custom model training
- **Security**: Microsoft Entra ID, Azure Key Vault, Microsoft Sentinel, zero trust

### Roundup Metadata

The AI extracts roundup metadata for each included item (stored in `ai_metadata` JSONB column):

- `roundup_summary` — 1-2 sentence ready-to-use summary
- `key_topics` — e.g. `["MCP", "Semantic Kernel", "RAG"]`
- `roundup_relevance` — `high` / `medium` / `low`
- `topic_type` — `announcement`, `tutorial`, `ga-release`, etc.
- `impact_level` — significance rating
- `time_sensitivity` — `immediate` / `this-week` / `this-month` / `long-term`

Items with `high` or `medium` relevance are used by the weekly roundup generation
(see [Weekly Roundups](#weekly-roundups)).

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
- **Transcript failures**: Non-fatal; YoutubeExplode is tried first (with persistent cookies),
  falling back to yt-dlp if YoutubeExplode fails. Either fetcher can be disabled via
  `ContentProcessor:YouTubeExplodeEnabled` / `ContentProcessor:YtDlpEnabled`.
  If all enabled fetchers fail, the YouTube item is processed without transcript data
- **AI API failures**: Retried up to `MaxRetries` times (configurable in `AiCategorizationOptions`)
- **Rate limiting**: Configurable delay between AI calls (`RateLimitDelaySeconds`)

### Subcollection Rules

Content items can be automatically assigned to subcollections based on the feed name and
video/article title. This is configured in `appsettings.json` under `ContentProcessor.SubcollectionRules`.

Each rule has three fields:

| Field | Description |
|---|---|
| `FeedName` | Feed name to match (exact, case-insensitive) |
| `TitlePattern` | Title pattern with `*` wildcard support (case-insensitive) |
| `Subcollection` | Subcollection name to assign when the rule matches |

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

This rule automatically assigns the `vscode-updates` subcollection to any video from the
"Fokko at Work YouTube" feed whose title starts with "Visual Studio Code and GitHub Copilot".
These videos then appear on the VS Code Updates page at `/github-copilot/vscode-updates`.

Rules are applied after AI categorization succeeds, so only items that pass the AI filter
are assigned subcollections. Manually added items with subcollections already set in the
database are not affected (the pipeline skips items that already exist).

## Weekly Roundups

Weekly roundups are generated entirely by `RoundupGeneratorBackgroundService`, a background
service running inside `TechHub.Api`. It fires every Monday at 08:00 UTC and writes the
completed roundup directly to the `content_items` table (`collection_name = 'roundups'`).

### Roundup Pipeline

1. **Article loading** — Queries `content_items` directly using section boolean columns
   and `ai_metadata`, filtered by `created_at` within the target week
2. **Relevance filtering** — Per section: always includes all `high`-relevance articles; fills up
   to `MinArticlesPerSection` with `medium` then `low` articles if the high count is below the minimum
3. **AI metadata backfill** — Any articles missing `ai_metadata` are categorized on-the-fly before proceeding
4. **Step 1/5** — AI creates news-style narrative stories per section
5. **Step 2/5** — AI adds continuity by comparing with the previous week's roundup
6. **Step 3/5** — AI condenses the content paragraph-by-paragraph
7. **Step 4/5** — AI generates metadata: `title`, `description`, `tags`, `introduction`
8. **Step 5/5** — Table of contents is built from `##`/`###` headers (pure C#)
9. **DB write** — Upserts into `content_items` + `content_tags_expanded`

Writing style guidelines (`docs/writing-style-guidelines.md`) are injected into every AI step
prompt to ensure consistent tone and style throughout the roundup.

### Roundup Configuration

Configured under `"RoundupGenerator"` in `appsettings.json`:

| Setting | Default | Description |
|---|---|---|
| `Enabled` | `false` | Enable/disable weekly roundup generation |
| `RunHourUtc` | `8` | UTC hour to fire on Monday |
| `MinArticlesPerSection` | `10` | Minimum articles per section; fills from medium/low if high count is below |
| `RateLimitDelaySeconds` | `15` | Delay between AI calls |
| `MaxRetries` | `3` | Max retries per AI call |

## Troubleshooting

### Processed URL Was Incorrectly Skipped

If an article was skipped with an unexpected reason:

1. Open the admin panel at `/admin/processed-urls`
2. Find the entry and click **Copy** to copy all details to the clipboard
3. Check the `Reason` field to understand the AI''s decision
4. If the decision was wrong, click **Remove** to delete the entry — it will be retried on the
   next scheduled run

## Content Fixer

`ContentFixerBackgroundService` performs bulk content quality fixes across all items in the
database. It runs on-demand via the admin dashboard (manual trigger only).

### Fix Operations

| Operation | Description |
|---|---|
| **Tag normalization** | Adds missing collection/section tags, removes deprecated tags |
| **Author normalization** | Standardizes author names to canonical forms |
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
