# Background Services

The API runs several hosted services for startup work, content ingestion, roundup generation, content cleanup and newsletter delivery. The Web app runs two small services that keep its in-memory caches fresh. This document covers what each service does, when it runs, and how scheduled runs are enabled, disabled and triggered manually.

## Overview

| Service | Project | Schedule | Gated by | Records a job row |
|---------|---------|----------|----------|-------------------|
| `StartupBackgroundService` | API | Once at startup | - | No |
| `ContentProcessingBackgroundService` | API | At startup (when enabled), then every `IntervalMinutes` (default 60) | `ContentProcessor:Enabled` and DB setting `ContentProcessor` | `content-processing` |
| `RoundupGeneratorBackgroundService` | API | Mondays at `RunHourUtc` (default 08:00 UTC) | `RoundupGenerator:Enabled` and DB setting `RoundupGenerator` | `roundup-generation` |
| `ContentFixerBackgroundService` | API | Manual trigger only | - | `content-cleanup` |
| `NewsletterBackgroundService` | API | Every `CheckIntervalMinutes` (default 30) | `Newsletter:ScheduledSendEnabled` and DB setting `Newsletter` | No, uses `newsletter_send_log` |
| `SectionCacheRefreshService` | Web | At startup, then every 5 minutes | - | No |
| `HeroBannerCacheRefreshService` | Web | Every 5 minutes | - | No |

Ad-hoc URL processing from the admin UI (`/api/admin/urls/process`) runs inside the request and records an `ad-hoc-processing` job. Sources are in `src/TechHub.Api/Services/` and `src/TechHub.Web/Services/`.

## Startup Sequence

`StartupBackgroundService` runs after Kestrel starts, so health endpoints respond while it works:

1. Run database migrations (see [database.md](database.md)).
2. Mark any job still in `running` state as `aborted`. These are leftovers from a crash or restart of the previous instance.
3. Mark startup complete on `StartupStateService` and log table row counts.

The other API background services wait for `StartupStateService` before doing anything, and the startup health check reports unhealthy until it completes (see [health-checks.md](health-checks.md)). If a step fails, the service logs a critical error and stops the application. A failed migration never leaves the app running against an outdated schema. A cancellation caused by shutdown is not treated as a failure.

## Content Processing

Fetches the enabled RSS feeds, categorizes new items with AI and writes them to `content_items`. It runs once right after startup when scheduled runs are enabled, then every `IntervalMinutes` using a periodic timer. After each run it clears the content cache, trims job history to `PurgeJobKeepCount` (500) and purges failed URL records older than `FailedUrlRetentionDays` (7) so they are retried. The pipeline itself is described in [content-processing.md](content-processing.md).

## Roundup Generator

Generates the weekly roundup for every section for the previous Monday to Sunday week and writes it to `content_items`. The service sleeps until the next Monday at `RunHourUtc` (UTC), and skips weeks whose roundup already exists. An admin can also regenerate one roundup: the API deletes the existing item and queues a run for that item's week, bypassing the "already exists" check. See [content-processing.md](content-processing.md) for the pipeline.

## Content Fixer

Applies bulk cleanup (tags, authors, markdown) to existing content. It has no schedule and no enable gate, and runs only when an admin triggers it. A triggered run records a job and proposes its changes as content reviews, which an admin approves or rejects before they are applied. The content cache is only cleared when a run applied fixes directly. See [content-processing.md](content-processing.md) and [admin.md](admin.md#content-reviews). The `ContentCleanup` row in `background_job_settings` is seeded for completeness but nothing reads it.

## Newsletter

Every `CheckIntervalMinutes` the service checks whether the weekly digest is due and whether the local hour matches `DailyDigestHourLocal`. Deduplication and timing rules are in [newsletter.md](newsletter.md).

## Enabling and Disabling Scheduled Runs

Scheduled runs of the content processor, roundup generator and newsletter need two switches to be on:

1. **Configuration flag** (environment level). `ContentProcessor:Enabled`, `RoundupGenerator:Enabled` and `Newsletter:ScheduledSendEnabled` default to `false` and are set to `true` only in `appsettings.Production.json`. `appsettings.Staging.json`, used by PR previews, sets all three to `false`. The deployment templates can also force `ContentProcessor__Enabled` and `RoundupGenerator__Enabled` to `false`. When the flag is off the database is not consulted. This keeps a preview environment that runs on a restored production database from processing content or emailing subscribers.
2. **Database setting** (runtime level). The `background_job_settings` table has one row per job (`ContentProcessor`, `RoundupGenerator`, `Newsletter`) with an `enabled` column. Migrations seed every row as disabled, and a missing row counts as disabled. Admins toggle the rows at `/admin/settings`, and the services read the value on every scheduled run, so no restart is needed.

Disabled services stay alive and still react to manual triggers.

| Run type | Config flag | DB setting |
|----------|-------------|------------|
| Scheduled | Must be on | Must be on |
| Manual (admin trigger) | Ignored | Ignored |

## Manual Triggers, Cancel and Job Types

Admins start runs from the dashboard or the `/api/admin/*` endpoints (see [admin.md](admin.md#background-jobs)). Triggers return immediately with `202 Accepted` and the work happens on the service's own loop.

- Each service runs one job at a time. A trigger that arrives during a run queues at most one follow-up run.
- Cancel stops the currently running content processing, roundup generation or content fixer run. The job is recorded as `aborted`. The newsletter service has no cancel.
- Jobs are stored in `content_processing_jobs` with a type (`content-processing`, `roundup-generation`, `content-cleanup`, `ad-hoc-processing`), a trigger (`scheduled` or `manual`), a status (`running`, `completed`, `failed`, `aborted`), counters and a log shown on the dashboard.
- Newsletter manual triggers also exist (weekly or daily run, and test sends) and are recorded in the send log instead.

## Error Handling

Each service's loop catches everything except cancellation, logs it and carries on with the next tick, so one bad run never stops the host. Failures inside a run are recorded on the job row. Per-item failures (a single article or subscriber) are logged and skipped. See [src/AGENTS.md](../src/AGENTS.md#error-handling-philosophy) for the coding rules.

## Web Cache Refresh Services

The Web app caches data from the API in memory and refreshes it periodically, so navigation and the hero banner pick up API changes without a restart:

- `SectionCacheRefreshService` loads the sections at startup and refreshes them every 5 minutes. The Web readiness check depends on this first load, so Container Apps does not route traffic to an instance until it succeeds.
- `HeroBannerCacheRefreshService` refreshes the hero banner data every 5 minutes. The first load happens in `Program.cs` at startup, and a failed load is retried on the next tick.

Both log a warning and keep the previous data when the API is unreachable. Admins can force an immediate refresh from `/admin/settings`.

## Related Documentation

- [content-processing.md](content-processing.md) - Ingestion, roundup and fixer pipelines
- [newsletter.md](newsletter.md) - Newsletter schedule and delivery
- [admin.md](admin.md) - Trigger, cancel and settings endpoints and pages
- [health-checks.md](health-checks.md) - Startup and readiness checks
- [caching.md](caching.md) - Browser caching
