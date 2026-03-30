# Content Processing

## Overview

The Tech Hub supports both manual and automated content creation. Content is organized into
collections and automatically categorized using AI-powered processing. All content is stored
exclusively in the PostgreSQL database.

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

## Manual Content Creation

The easiest way to add a single content item is using the built-in GitHub Copilot command:

```text
/new-article
```

For detailed information about site structure and terminology, see:

- [Terminology](terminology.md) — Site terminology, sections, and standard values
- [Repository Structure](repository-structure.md) — Code and content organization
- [Collections Guide](../collections/AGENTS.md) — Content management overview
- [Custom Pages](custom-pages.md) — Custom pages and specialized collections

## Publishing Content

Use the GitHub Copilot command for publishing:

```text
/pushall
```

## RSS Feed Processing

The Tech Hub automatically processes RSS feeds from Microsoft and technology sources to keep
content current. The pipeline is implemented entirely in C# and runs inside `TechHub.Api`.

### Content Processing Pipeline

`ContentProcessingBackgroundService` (`IHostedService`) runs on a configurable schedule
(default: every 15 minutes) and performs:

1. **Feed ingestion** — Downloads and parses RSS/Atom XML from all enabled feeds in the database
2. **Content fetching** — Fetches the full article HTML for each item, or closed captions
   (transcripts) for YouTube video items via **YoutubeExplode**. Failures are non-fatal.
3. **AI categorization** — Sends content to Azure OpenAI (`AiCategorizationService`) using the
   system prompt embedded in `TechHub.Infrastructure/Data/Resources/system-message.md`
4. **Deduplication** — Checks `processed_urls` table to skip already-attempted URLs
5. **Database write** — Upserts into `content_items` + `content_tags_expanded`; records the run
   in `content_processing_jobs`; registers items in per-section roundup accumulators
   (`section_roundup_items`)

Items that the AI determines are off-topic or low quality are skipped and recorded in
`processed_urls` with status `skipped`.

### Feed Configuration

RSS feeds are stored in the `rss_feed_configs` database table and managed via the admin UI at
`/admin/feeds`. On first startup, the API seeds the database from `scripts/data/rss-feeds.json`
if no feeds exist yet.

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

**Via JSON seed file** (initial setup only):

1. Edit `scripts/data/rss-feeds.json`
2. On next startup (if database has no feeds), the API seeds from this file

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

Items with `high` or `medium` relevance are automatically registered in `section_roundup_items`
for use in the weekly roundup generation (see [Weekly Roundups](#weekly-roundups)).

### Admin Dashboard

The admin area at `/admin` provides:

- **Dashboard** — Recent processing job history with status, duration, log output
- **Run Now** button — Triggers an immediate out-of-schedule run
- **RSS Feeds** — Full CRUD management of feed configurations
- **Processed URLs** — Browse all processed URLs with status/reason; remove entries to retry

See [admin-authentication.md](admin-authentication.md) for authentication setup.

### Error Handling

- **Feed unavailability**: Individual feed failures do not stop the pipeline; other feeds continue
- **Content fetch failures**: Non-fatal; pipeline falls back to RSS metadata only
- **Transcript failures**: Non-fatal; YouTube items are processed without transcript
- **AI API failures**: Retried up to `MaxRetries` times (configurable in `AiCategorizationOptions`)
- **Rate limiting**: Configurable delay between AI calls (`RateLimitDelaySeconds`)

## Weekly Roundups

Weekly roundups are generated entirely by `RoundupGeneratorBackgroundService`, a background
service running inside `TechHub.Api`. It fires every Monday at 08:00 UTC and writes the
completed roundup directly to the `content_items` table (`collection_name = 'roundups'`).

### Roundup Pipeline

1. **Article loading** — Reads accumulated candidates from `section_roundup_items` (joined with
   `content_items` for AI metadata)
2. **Relevance filtering** — Per section: includes all `high`-relevance articles; adds `medium`
   articles when fewer than `MinHighArticlesPerSection` high articles exist; adds `low` articles
   when the combined count is still below `MinTotalArticlesPerSection`
3. **Step 3** — AI creates news-style narrative stories per section
4. **Step 4** — AI adds continuity by comparing with the previous week's roundup
5. **Step 6** — AI condenses the content paragraph-by-paragraph
6. **Step 7** — AI generates metadata: `title`, `description`, `tags`, `introduction`
7. **Step 8** — Table of contents is built from `##`/`###` headers (pure C#)
8. **Step 9** — AI rewrites for writing style compliance and returns the final
   `title` and `description`; returns `{"title": "...", "description": "..."}` followed by `---`
   and the markdown body
9. **DB write** — Upserts into `content_items` + `content_tags_expanded`

### Roundup Configuration

Configured under `"RoundupGenerator"` in `appsettings.json`:

| Setting | Default | Description |
|---|---|---|
| `Enabled` | `false` | Enable/disable weekly roundup generation |
| `RunHourUtc` | `8` | UTC hour to fire on Monday |
| `MinHighArticlesPerSection` | `3` | Threshold below which medium articles are included |
| `MinTotalArticlesPerSection` | `5` | Threshold below which low articles are included |
| `RateLimitDelaySeconds` | `15` | Delay between AI calls |
| `MaxRetries` | `3` | Max retries per AI call |

The `section_roundup_items` table is populated automatically during RSS processing — one row per
`(section, week, content item)` for all items with `high` or `medium` roundup relevance.

## Troubleshooting

### Processed URL Was Incorrectly Skipped

If an article was skipped with an unexpected reason:

1. Open the admin panel at `/admin/processed-urls`
2. Find the entry and click **Copy** to copy all details to the clipboard
3. Check the `Reason` field to understand the AI''s decision
4. If the decision was wrong, click **Remove** to delete the entry — it will be retried on the
   next scheduled run

### Common Frontmatter Issues (Manual Content)

- **Missing Frontmatter**: Check requirements in [collections/AGENTS.md](../collections/AGENTS.md#frontmatter)
- **File Naming**: Use `YYYY-MM-DD-title.md` pattern
- **Sections/Tags**: Verify against site configuration in [terminology.md](terminology.md)
- **Date Formats**: Use ISO 8601 format: `YYYY-MM-DD HH:MM:SS +00:00`
