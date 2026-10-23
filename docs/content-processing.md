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
3. **AI classification** — Sends content to Azure AI Foundry (`AiCategorizationService`) with the
   classify prompt `TechHub.Infrastructure/Data/Resources/content-classify-system.md`. The answer is
   strict JSON: include or exclude, sections, tags, excerpt, roundup metadata and timeline events
   (see [AI Categorization Service](#ai-categorization-service)). Items that are excluded stop here
   and cost one cheap call
4. **AI rewrite** — Only for included items, a second model writes the cleaned-up markdown body
   (`content-rewrite-system.md`) that continues from the excerpt
5. **Deduplication** — Checks `processed_urls` table to skip already-attempted URLs
6. **Database write** — Upserts into `content_items` + `content_tags_expanded` and replaces the
   article's rows in `content_timeline_events` in one transaction (a failed event write stores nothing, so the
   URL is retried on the next run), and records the run in `content_processing_jobs`

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

The classification step analyzes content using `src/TechHub.Infrastructure/Data/Resources/content-classify-system.md`.

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

`AiCategorizationService` processes each item in two AI steps through `AiCompletionClient`, which
calls the Azure AI Foundry v1 chat completions API (`{Endpoint}/openai/v1/chat/completions`, the
deployment goes in the request `model`). The client authenticates with Microsoft Entra ID (managed
identity in Azure, `az login` locally), so no API key is configured.

| Step | Profile | Output | Model |
|---|---|---|---|
| 1. Classify | `Classification` | Strict JSON (JSON schema): include/exclude, explanation, title, author, sections, primary section, tags, excerpt, `section_roles`, `key_facts`, `significance`, `content_kind`, `lifecycle`, `products`, `timeline_events` | `gpt-6.1-sol` |
| 2. Rewrite | `Rewrite` | Markdown body, only for included items | `gpt-6.1-sol` |
| Roundup writer | `RoundupNewsWriter` | Roundup topics | `gpt-6-astra` |
| Roundup metadata | `RoundupMetadata` | Title, tags and introduction (strict JSON) | `gpt-6.1-sol` |
| Timeline extraction | `TimelineExtraction` | Timeline events only (strict JSON), from already stored articles: the timeline backfill job | `gpt-6-luna` |

`CategorizeAsync` runs step 1 and, when the item is included, step 2. A failing rewrite fails the
whole item; nothing is stored. `ClassifyAsync` runs step 1 only and is used by the roundup
metadata backfill, which never rewrites stored articles.

The sections the classification may return (the `enum` of the JSON schema and the validation of the answer) are the configured
sections in `AppSettings:Content:Sections`, without `all`. Adding or removing a section in configuration therefore changes both together.
Classification runs on `gpt-6.1-sol` rather than the cheaper `gpt-6-luna` because the include/exclude decision needs the strictness of the old model: on 73 articles production had skipped, `gpt-6.1-sol` and `gpt-5.2` kept 95.9% excluded, `gpt-6-luna` only 89.0% (82.2% with `medium` reasoning). The `gpt-6-luna` deployment serves the timeline extraction. See [Evaluating Models](#evaluating-models).

Configured under `"AiCategorization"`:

| Setting | Default | Description |
|---|---|---|
| `Endpoint` | empty | Azure AI Foundry endpoint URL |
| `Profiles:<Step>:DeploymentName` | the models of the table above in `appsettings.json` | Deployment used by the step (`Classification`, `Rewrite`, `RoundupNewsWriter`, `RoundupMetadata`, `TimelineExtraction`) |
| `Profiles:<Step>:ReasoningEffort` | `low` (`medium` for the roundup writer) in `appsettings.json`; empty omits it | `reasoning_effort` sent to the model, e.g. `low` or `medium` |
| `Profiles:<Step>:ServiceTier` | `flex` in `appsettings.json` | `default` (Standard) or `flex` (50% cheaper, may queue) |
| `MaxRetries` | `3` | Attempts per item on rate limits and transient failures |
| `RateLimitDelaySeconds` | `15` | Wait between attempts on a rate limit without a `Retry-After` header (multiplied by the attempt number) |
| `FlexMaxWait` | `00:30:00` | How long a Flex request keeps retrying on Flex before it falls back |
| `FlexMinRetryDelay` / `FlexMaxRetryDelay` | `00:00:30` / `00:02:00` | Backoff bounds between Flex retries. `FlexMaxRetryDelay` caps only the calculated backoff; a server `Retry-After` is honored in full (and the request falls back when that wait would exceed `FlexMaxWait`) |
| `FlexFallbackToDefaultTier` | `true` | After `FlexMaxWait`, send the request once on the Standard tier. When `false`, the request is reported as rate limited |
| `TimelineBackfillDelaySeconds` | `2` | Wait between articles during the timeline backfill |

Behavior worth knowing:

- **Per-step models**: Models, reasoning effort and service tier are configuration only. Switching a step to another deployment, or back to `gpt-5.2` with `ServiceTier: default`, needs no code change. No `temperature` is sent.
- **Flex**: A `429` on a Flex request means "no capacity right now". The client keeps retrying on Flex, then falls back to the Standard tier. Rate-limit responses do not count as circuit-breaker failures, so sustained capacity shortages cannot block retries or fallback; other transient failures still count. Interactive calls (ad-hoc URL add, apply transcript) always use the Standard tier so an admin never waits in a queue.
- **Usage logging**: Every successful call logs the model, requested and served tier, prompt and cached tokens, and completion and reasoning tokens, so the real cost can be checked.
- **Retries**: A `429` on the Standard tier waits for the server's `Retry-After` value when present. Network errors and timeouts back off 5 seconds times the attempt number. The HTTP client itself does not retry; its timeout is 15 minutes because Flex can queue.
- **Non-retryable responses**: Content filter blocks and context-length errors return immediately and are recorded as failures with the Azure message.
- **Response validation**: Responses that are empty, cut off (`finish_reason` other than `stop`) or not valid JSON are failures. A JSON sanitizer repairs invalid escape sequences before parsing. The classification is validated in code: sections must be one of the seven configured sections, the primary section must be one of the item's sections, `section_roles` may only name the item's sections, and the primary section is always `lead`.
- **Content hash**: `content_hash` is the SHA-256 of title, rewritten content and excerpt.

### Roundup Metadata

The classification step stores roundup metadata in the `ai_metadata` JSONB column with `schema_version = 2`:

- `key_facts` — 2 to 5 concrete facts (versions, dates, numbers) the roundup writer uses as its only source of such details
- `significance` — `high` / `medium` / `low`: impact on developers, used to order articles in a roundup
- `content_kind` — `announcement`, `tutorial`, `analysis`, `case-study`, `reference` or `event`
- `lifecycle` — `new`, `preview`, `ga`, `update`, `deprecation`, `retirement` or `none`
- `products` — 1 to 3 official product names, most relevant first. Related articles are sorted next to each other by their first product
- `section_roles` — per section `lead` (full write-up in that section's roundup) or `brief` (one-line mention). The primary section is always `lead`; another section is `lead` only when the article has a distinct angle for its readers; when in doubt it is `lead`

Rows with the older summary-based metadata (no `schema_version`, `roundup_summary`, `key_topics`, ...)
count as missing: the roundup generator re-classifies them on the fly (classification only) the first time
they appear in a roundup.

### Admin Dashboard

The admin area at `/admin` provides:

- **Dashboard** — Recent processing job history with status, duration, log output
- **Run Now** button — Triggers an immediate out-of-schedule run
- **Backfill Timeline** button — Starts the timeline backfill (see [Timeline Events](#timeline-events))
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
   sections is therefore loaded by each of them
2. **Existing check** — Sections whose roundup for that week already exists are skipped
3. **AI metadata backfill** — Articles without `schema_version` 2 metadata are classified on the fly
   (classification only, never a rewrite). Their `ai_metadata` is updated and the timeline events the classification found are stored
   in the same transaction, so the "Dates to know" block does not depend on the timeline backfill having run, and a failed event
   write leaves the article on its old metadata version so it is retried
4. **Lead and brief split (code)** — The article's role for this section decides: `lead` articles get a full
   write-up, `brief` articles only a one-line mention. Lead articles are ordered by `significance`, then by first product
5. **Writer (AI, one call)** — Writes the topics and "Other ... News" from the key facts of the lead articles.
   It also receives only the headings of the previous week's roundup for an occasional "following last week's ..." sentence
6. **Completeness check (code)** — Every lead article must be linked, as the complete destination of a Markdown link (a URL that is only a prefix of another linked URL does not count). Links the writer left out are appended to the "Other ... News" section, and a warning is logged
7. **Metadata (AI, one call)** — Title, tags and introduction as strict JSON
8. **Code-built blocks** — "Also relevant this week" (brief articles, each with its first key fact and, when the roundup of its primary section exists, a link to it for the full story) and "Dates to know" (see below), followed by the table of contents built from the `##`/`###` headers
9. **DB write** — Upserts one roundup per section into `content_items` + `content_tags_expanded`

Steps 1–7 run for every section first and nothing is written yet. Steps 8–9 run afterwards, once it is known which roundups exist
(already stored, or generated in this run), so a one-line mention never links to a roundup that failed to generate; it then keeps the
source-article link without the "Full story" sentence. The consequence is that a run that is cancelled before the writing phase stores no roundups of that run.

**Dates to know** comes from `content_timeline_events` of the section's articles of the week, for dates after the roundup week:
*Deadlines* are `deprecation`, `retirement`, `end-of-support`, `breaking-change` and `pricing-change`;
*Coming up* are `preview`, `ga`, `launch` and `release`. Duplicates are merged and each group shows at most ten dates. The block is
omitted when there is nothing to report.

Writing style guidelines (`docs/writing-style-guidelines.md`) are injected into the writer and metadata prompts
to ensure consistent tone and style throughout the roundup.

### Roundup Configuration

Configured under `"RoundupGenerator"` in `appsettings.json`:

| Setting | Default | Description |
|---|---|---|
| `Enabled` | `false` (`true` in Production) | Enables scheduled roundup generation. Manual triggers ignore it |
| `RunHourUtc` | `8` | UTC hour to fire on Monday |
| `RateLimitDelaySeconds` | `15` | Delay between AI calls |
| `MaxRetries` | `3` | Max retries per AI call |

The models used by the writer and metadata steps are configured in the `RoundupNewsWriter` and `RoundupMetadata` profiles
(see [AI Categorization Service](#ai-categorization-service)).

## Timeline Events

Articles often state dated lifecycle events: a feature goes to preview, becomes generally available, a version
is deprecated or retired. The classification step extracts them and the application stores them with the article they come from.

| Table | Purpose |
|---|---|
| `content_timeline_events` | One row per event: `kind`, `product`, `feature`, `description`, `event_date`, `date_precision` (`day`/`month`/`quarter`), `date_source`, `quote`, `extracted_from`, with a cascading foreign key to the source article |
| `content_timeline_extractions` | One row per scanned article with the `prompt_version` and the number of events found, so the backfill is resumable |

Event kinds are `announced`, `preview`, `ga`, `launch`, `release`, `deprecation`, `retirement`, `end-of-support`, `breaking-change`, `pricing-change` and `event`.

- **Dates come from code when they are "now".** When an article says a feature "is now generally available", the model reports `date_source: publication` and no date; code fills in the article's publication date (Europe/Brussels). Explicit dates are validated, and coarse dates are normalized to the first day of the month or quarter. Events with an unparseable date are skipped and logged.
- **No quote check.** The `quote` is stored for spot checks. If the model proves unreliable, add a check that the quote appears in the article.
- **Reprocessing replaces events.** An article's events are replaced whenever it is processed again, or when a transcript is applied. This happens in the same transaction as the article write (processing, transcript edit, roundup metadata backfill), so a failing event write never leaves a saved article with stale events or a scanned marker that does not match them. Replacements of the same article are serialized with a row lock on the article, so concurrent runs cannot store two event sets.
- **Write-only for now.** The only reader is the roundup "Dates to know" block. The table is the basis for making the GitHub Copilot features page dynamic later.

### Timeline Backfill

Existing articles are scanned by `TimelineBackfillBackgroundService`, a manual-trigger-only job (job type `timeline-backfill`). Use
the **Backfill Timeline** button on the admin dashboard or `POST /api/admin/timeline-backfill/trigger` (see [admin.md](admin.md)).

- Reads the stored excerpt and article text (the text continues after the excerpt, so both are sent) and sends them to the `TimelineExtraction` profile (`gpt-6-luna`: it only extracts dated facts and does not decide whether an article belongs on the site, so it does not need the strictness of the classification model) with a dedicated prompt (`content-timeline-extract-system.md`) that shares its event rules with the classify prompt (`content-timeline-rules.md`)
- Skips articles that were scanned with the current prompt version. `force` scans them again, and raising `ITimelineEventRepository.CurrentPromptVersion` makes every older scan eligible again
- An admin edit that changes the title, excerpt, text or publication date (without extracting new events itself) removes the article's events and scan marker in the same transaction, so an ordinary run scans the edited article again
- Options: `force`, `publishedAfter` (yyyy-MM-dd), `collection` and `limit`. Newest articles come first
- Resumable: an article is marked as scanned only after its events are stored, so a cancelled or failed run can simply be started again. A failing article is logged and left for the next run
- Roundups are never scanned
- The job log lists failures and progress every 25 articles. The job row shows events stored (Added), scanned articles without events (Skipped) and scanned articles (Fixed)

## Evaluating Models

`tests/TechHub.Infrastructure.Tests/Evaluation/ModelEvaluationTests.cs` compares the configured models with production on real data. It is the
tool to use before changing a model or a prompt. The stages call the real models, cost money and are skipped unless `TECHHUB_EVAL=1` is set.

| Stage (test name) | Question | Output in `.tmp/eval/` |
|---|---|---|
| `ModelEvaluation_Classification` | Does the classification agree with production? 100 stored articles (included in production) and 50 re-fetched skipped URLs (excluded in production) | `classification.md` |
| `ModelEvaluation_ClassificationControl` | How much of the disagreement is noise or caused by the prompt, not the model? Classifies a frozen set of 100 stored articles with `gpt-6-luna` twice and with `gpt-5.2` using the new prompt | `classification-control.md`, `golden-classification.json` (the frozen sample) |
| `ModelEvaluation_SkippedControl` | Which models exclude what production excluded? Classifies about 70 frozen, re-fetched skipped URLs with `gpt-6-luna` (low, medium), `gpt-6.1-sol` and `gpt-5.2` | `skipped-control.md`, `golden-skipped.json` (the frozen article text) |
| `ModelEvaluation_Rewrite` | How do the rewrite model at `low` and `medium` reasoning compare with the stored text? About 12 re-fetched articles | `rewrite.md`, `rewrite/*.md` (stored, low and medium side by side) |
| `ModelEvaluation_Timeline` | Are the extracted dates right? About 50 articles | `timeline.md` |
| `ModelEvaluation_Roundups` | Do the roundups of the latest week with a production roundup improve? Regenerates all sections locally | `roundups.md`, `roundups/production`, `roundups/new` |

Every report ends with the token usage per model (prompt, cached, completion and reasoning tokens), which feeds the cost estimate.

How to run:

1. Restore production into the local database: `./scripts/Restore-Database.ps1 -Target local -TablesOnly`. The restore copies only the content tables
2. Start the app once (`Run -WithoutTests`, then `Stop-Servers`) so that pending migrations are applied; the restored `_migrations` table does not contain the newest ones
3. Sign in with `az login` (an account with the `Cognitive Services OpenAI User` role and an allowed IP) and set `TECHHUB_EVAL=1`
4. Run one stage: `Run -TestProject Infrastructure.Tests -TestName ModelEvaluation_Classification`

The roundup stage deletes the local copy of that week's roundups before regenerating them (production copies are saved first), and it changes `ai_metadata` of the
local articles. Never point it at a production database. Run it against a fresh restore if you want to compare again.

How to read the results:

- A disagreement is not an error: the baseline is the production model, not the truth. Review the listed disagreements by hand
- 100 articles means roughly ±5 percentage points of uncertainty on an agreement percentage
- The stored text of included articles is already rewritten and has no feed data, so classifying it is somewhat easier than classifying a raw feed item. The skipped-URL check uses the real original articles
- Each run samples different articles (`ORDER BY random()`). To compare two models, compare their reports on the same run. `ModelEvaluation_ClassificationControl` freezes its sample in `.tmp/eval/golden-classification.json` (delete the file to draw a new sample), so it can be repeated after a prompt change

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
