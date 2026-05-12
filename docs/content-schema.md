# Content Entity Schema

This document defines the schema for `ContentItem` records stored in the PostgreSQL `content_items` table.

## Source of Truth

The PostgreSQL database is the **single source of truth** for all content. There are no content files checked into the repository. Content enters the database through two paths:

1. **RSS pipeline** — `ContentProcessingBackgroundService` fetches RSS feeds every 15 minutes, runs AI categorization via Azure OpenAI, and upserts into `content_items`. See [content-processing.md](content-processing.md).
2. **Admin UI / ad-hoc processing** — Admins can submit individual URLs (including YouTube videos and GitHub Copilot Feature items) through the admin panel for immediate AI processing and insertion.

> **Note**: `ContentSyncService` exists only for **integration tests**. It reads markdown fixture files under `tests/TechHub.TestUtilities/TestCollections/` and seeds the test database. It is never used in production or staging.

## Database Table: `content_items`

See the full DDL in [src/TechHub.Infrastructure/Data/Migrations/postgres/001_initial_schema.sql](../src/TechHub.Infrastructure/Data/Migrations/postgres/001_initial_schema.sql).

### Core Columns

| Column | Type | Required | Description |
|--------|------|----------|-------------|
| `slug` | TEXT | Yes | URL-safe identifier. Primary key with `collection_name`. |
| `collection_name` | TEXT | Yes | `news`, `blogs`, `videos`, `community`, `roundups` |
| `title` | TEXT | Yes | Plain text title (no HTML/markdown) |
| `author` | TEXT | Yes | Author or presenter name |
| `date_epoch` | BIGINT | Yes | Unix timestamp (seconds). For RSS items this is the feed publication date. |
| `primary_section_name` | TEXT | Yes | Primary section: `ai`, `azure`, `github-copilot`, `dotnet`, `devops`, `security`, `ml` |
| `external_url` | TEXT | Yes | Source URL. For YouTube videos this is the YouTube URL. |
| `feed_name` | TEXT | Yes | Display name of the originating RSS feed (or `"TechHub"` for admin-added items). |
| `excerpt` | TEXT | Yes | Plain-text summary, max ~200 words. Used in cards, meta descriptions, RSS. |
| `content` | TEXT | Yes | Full body content in markdown format. |
| `tags_csv` | TEXT | Yes | Comma-delimited normalized tags, e.g. `,AI,GitHub Copilot,`. |
| `content_hash` | TEXT | Yes | MD5 of content for change detection. |

### Section Flags (denormalized for fast filtering)

| Column | Type | Description |
|--------|------|-------------|
| `is_ai` | BOOLEAN | Content belongs to the `ai` section |
| `is_azure` | BOOLEAN | Content belongs to the `azure` section |
| `is_dotnet` | BOOLEAN | Content belongs to the `dotnet` section |
| `is_devops` | BOOLEAN | Content belongs to the `devops` section |
| `is_github_copilot` | BOOLEAN | Content belongs to the `github-copilot` section |
| `is_ml` | BOOLEAN | Content belongs to the `ml` section |
| `is_security` | BOOLEAN | Content belongs to the `security` section |
| `sections_bitmask` | INTEGER | Bitwise combination of section flags for fast multi-section queries |

## Domain Model Mapping

How `content_items` columns map to C# properties in `TechHub.Core.Models.ContentItem`:

| DB Column | C# Property | Type | Notes |
|-----------|-------------|------|-------|
| `slug` | `Slug` | `string` | |
| `title` | `Title` | `string` | |
| `author` | `Author` | `string` | |
| `date_epoch` | `DateEpoch` | `long` | Unix timestamp (seconds) |
| `collection_name` | `CollectionName` | `string` | |
| `primary_section_name` | `PrimarySectionName` | `string` | |
| `external_url` | `ExternalUrl` | `string` | |
| `feed_name` | `FeedName` | `string` | |
| `excerpt` | `Excerpt` | `string` | |
| `tags_csv` | `Tags` | `IReadOnlyList<string>` | Parsed from CSV |
| Section booleans | `Sections` | `IReadOnlyList<string>` | Reconstructed from booleans |

## Section Names

Valid `primary_section_name` values (also used in `section_names` multi-value context):

- `ai` — Artificial Intelligence
- `azure` — Microsoft Azure
- `github-copilot` — GitHub Copilot
- `dotnet` — .NET
- `devops` — DevOps
- `security` — Security
- `ml` — Machine Learning

If content is tagged `github-copilot` it should also include `ai` (unless it's purely about Copilot administration/configuration).

## Collection Names

| `collection_name` | Description |
|-------------------|-------------|
| `news` | News articles (links externally) |
| `blogs` | Blog posts (links externally) |
| `community` | Community posts (links externally) |
| `videos` | YouTube videos (internal pages at `/section/videos/slug`) |
| `roundups` | TechHub-authored roundups (internal at `/all/roundups/slug`) |

External-linking collections (`news`, `blogs`, `community`) use `external_url` for navigation. Internal collections (`videos`, `roundups`) show content on TechHub pages.

## GitHub Copilot Features

GitHub Copilot feature videos have `collection_name = 'videos'` and appear on the `/github-copilot/features` timeline page. Feature metadata is stored in dedicated tables:

### `ghc_features` table

Stores metadata for each GHC feature release:

| Column | Type | Description |
|---|---|---|
| `slug` | `text` PK | Unique identifier matching the `content_items.slug` |
| `title` | `text` | Feature release title |
| `excerpt` | `text` | Short description |
| `release_date` | `bigint` | Unix epoch timestamp of the release (nullable) |
| `plans` | `text` | Comma-separated plan names: `Free`, `Pro`, `Business`, `Enterprise` |
| `ghes_support` | `boolean` | Whether GHES is supported |
| `created_at` / `updated_at` | `timestamptz` | Audit timestamps |

### `ghc_feature_content` table

Links a GHC feature to one or more content items (videos, blogs):

| Column | Type | Description |
|---|---|---|
| `feature_slug` | `text` FK → `ghc_features.slug` | The feature this link belongs to |
| `collection_name` | `text` | Collection of the linked content item |
| `item_slug` | `text` | Slug of the linked content item |
| `is_thumbnail` | `boolean` | Whether this item is the feature's thumbnail |
| `sort_order` | `int` | Display order |

### `vscode_update_items` table

Tracks which `videos` items are VS Code monthly update videos:

| Column | Type | Description |
|---|---|---|
| `collection_name` | `text` | Always `videos` |
| `slug` | `text` | The content item slug |

This table is populated automatically by:

- `ContentItemWriteRepository` when the item matches a `vscode-updates` rule in `ContentProcessor.SubcollectionRules`
- `ContentSyncService` when seeding items from `_videos/vscode-updates/` fixture directories
- `IGhcFeatureRepository.AddVscodeUpdateItemAsync` for manual admin operations

### API

The `GET /api/ghc-features` endpoint returns all GHC features with their content links. See [content-api.md](content-api.md#ghc-features-api) for details.

## Test Fixture Format

For integration tests only, `ContentSyncService` can parse markdown files from `tests/TechHub.TestUtilities/TestCollections/`. These fixture files use YAML frontmatter with the same field names as the database columns:

```yaml
---
title: Video in Videos Root
date: 2025-01-15
primary_section: github-copilot
section_names:
  - github-copilot
tags:
  - Video
  - GitHub Copilot
external_url: https://example.com/videos/root-video
author: Test Author
feed_name: Test Feed
---
Excerpt text here.
```

This format is **not used in production**. Production content comes exclusively from the RSS pipeline and admin UI.

## Implementation Reference

- **Content model**: [src/TechHub.Core/Models/Core/ContentItem.cs](../src/TechHub.Core/Models/Core/ContentItem.cs)
- **DB schema**: [src/TechHub.Infrastructure/Data/Migrations/postgres/](../src/TechHub.Infrastructure/Data/Migrations/postgres/)
- **Content processing**: [content-processing.md](content-processing.md)
- **Test seeder**: [tests/TechHub.TestUtilities/TestCollectionsSeeder.cs](../tests/TechHub.TestUtilities/TestCollectionsSeeder.cs)
