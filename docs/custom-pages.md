# Custom Pages API

This document describes the API endpoints for retrieving structured data for specific, highly customized pages in Tech Hub.

**Related Documentation**:

- [Content API](content-api.md) - Standard content endpoints
- [Terminology](terminology.md) - Collection and section definitions

## Overview

While most content in Tech Hub is standard markdown served via generic item endpoints, certain high-value pages require bespoke layouts and structured data that doesn't fit the standard content model. These are "Custom Pages".

**Characteristics**:

- **Hardcoded Endpoints**: Each custom page has a dedicated API endpoint returning structured JSON.
- **Structured Data**: Returns a JSON object specifically shaped for that page's UI requirements (e.g., `LevelsPageData`, `FeaturesPageData`).
- **Composite Content**: Often aggregates data from multiple sources or specific content items.
- **Hybrid Pages**: Some custom pages (like VS Code Updates) use the standard content collection pipeline instead of a dedicated custom pages endpoint, but are still configured as `Custom: true` in section settings.

**Common Use Case**: Landing pages, interactive guides (like "Levels of Enlightenment"), or feature matrices that need specific data structures for frontend rendering.

**Sections with custom pages**:

| Section | Custom Pages |
|---|---|
| `github-copilot` | Features, Getting Started, Levels of Enlightenment, The GitHub Copilot Handbook, Tool Tips, VS Code Updates |
| `ai` | GenAI Basics, GenAI Advanced, GenAI Applied, AI SDLC |
| `devops` | DX, SPACE & DORA |

## Endpoints

Retrieves the structured data required to render the corresponding custom page.

### Developer Experience Space

**Endpoint**: `GET /api/custom-pages/dx-space`
**Data Model**: `DXSpacePageData`
**Description**: Data for the landing page of the Developer Experience Space.

### Getting Started

**Endpoint**: `GET /api/custom-pages/getting-started`
**Data Model**: `GettingStartedPageData`
**Description**: A step-by-step onboarding guide for GitHub Copilot covering license acquisition, VS Code setup, custom instructions, MCP servers, model selection, context management, and prompting tips. Markdown content in each section is rendered to HTML.

### GitHub Copilot Handbook

**Endpoint**: `GET /api/custom-pages/handbook`
**Data Model**: `HandbookPageData`
**Description**: Structured data for the GitHub Copilot Handbook.

### Levels of Enlightenment

**Endpoint**: `GET /api/custom-pages/levels`
**Data Model**: `LevelsPageData`
**Description**: Interactive guide data for the "Levels of Enlightenment" capability model.

### GitHub Copilot Features

**Endpoint**: `GET /api/custom-pages/features`
**Data Model**: `FeaturesPageData`
**Description**: An interactive feature timeline showing the chronological evolution of GitHub Copilot features. Includes subscription tier definitions for the sidebar and a `timelineFeatures` array with per-feature release dates, plan availability, GHES support, and optional links to related video content.

### GenAI Basics

**Endpoint**: `GET /api/custom-pages/genai-basics`
**Data Model**: `GenAIPageData`
**Description**: Educational content structural data for the GenAI Basics module.

### GenAI Advanced

**Endpoint**: `GET /api/custom-pages/genai-advanced`
**Data Model**: `GenAIPageData`
**Description**: Educational content structural data for the GenAI Advanced module.

### GenAI Applied

**Endpoint**: `GET /api/custom-pages/genai-applied`
**Data Model**: `GenAIPageData`
**Description**: Real-world application examples and case study data.

### GitHub Copilot Tool Tips

**Endpoint**: `GET /api/custom-pages/tool-tips`
**Data Model**: `ToolTipsPageData`
**Description**: A curated collection of tools, extensions, and resources that enhance the GitHub Copilot experience.

### SDLC (Software Development Life Cycle)

**Endpoint**: `GET /api/custom-pages/sdlc`
**Data Model**: `SDLCPageData`
**Description**: Data mapping GenAI tools and practices to SDLC phases.

### Hero Banner

**Endpoint**: `GET /api/custom-pages/hero-banner`
**Data Model**: `HeroBannerData`
**Description**: Cards for the collapsible announcement banner shown above section content (currently used in the GitHub Copilot section). All cards are returned regardless of date — clients filter by `startDate`/`endDate` using the Europe/Brussels timezone. When no active cards remain, the banner is hidden automatically.

## Response Format

**Success Response**: `200 OK`
Returns the specific data object (JSON) for the requested page.

**Error Response**: `404 Not Found`
If the data cannot be constructed or the page definition is missing.

## Implementation

Custom pages content is hydrated from:

1. **Database** (`custom_page_data` table) — JSON objects representing the structured data are stored directly in the database. Admin users can edit the JSON from the admin UI. This is the **only** source of truth for custom page data (database-first).
2. **Aggregation of items** from standard collections (e.g., Features page aggregates data from collection items)

### Admin Management

Custom page JSON data can be viewed and edited from the admin dashboard at `/admin/custom-pages`. Each entry shows:

- **Key** — Unique identifier matching the API endpoint (e.g., `dx-space`, `levels`)
- **Description** — Human-readable label
- **Last Updated** — Timestamp of the most recent edit
- **Edit** — Opens a JSON editor modal (`JsonEditorModal`) for in-place editing

Changes made via the admin UI are immediately persisted to the `custom_page_data` database table and reflected on the next page load (no seeding or restart required).

See [src/TechHub.Api/Endpoints/AdminEndpoints.cs](../src/TechHub.Api/Endpoints/AdminEndpoints.cs) for the admin API endpoints and [src/TechHub.Infrastructure/Repositories/CustomPageDataRepository.cs](../src/TechHub.Infrastructure/Repositories/CustomPageDataRepository.cs) for the repository.

The three GenAI endpoints use a special handler that processes markdown content within the JSON and replaces `{{mermaid:id}}` placeholders with actual mermaid diagram code blocks. Other endpoints deserialize JSON directly.

All three GenAI pages (`genai-basics`, `genai-advanced`, `genai-applied`) share a single Razor component (`GenAI.razor`) with three `@page` routes.

The VS Code Updates page (`GitHubCopilotVSCodeUpdates.razor`) is configured as `Custom: true` but does **not** use a custom pages API endpoint. It fetches content through the standard content collection API, filtering to items tracked in the `vscode_update_items` lookup table.

See [src/TechHub.Api/Endpoints/CustomPagesEndpoints.cs](../src/TechHub.Api/Endpoints/CustomPagesEndpoints.cs) for endpoint implementation.

## Content Sources for Custom Pages

Some custom pages are populated by specialized video content tracked in dedicated database tables.

### GitHub Copilot Features Content

**Location**: Videos stored under `collection_name = 'videos'` and tracked in the `ghc_features` and `ghc_feature_content` tables.

Each GHC feature markdown file in `_videos/ghc-features/` is synced as both a regular `content_item` and a `ghc_features` entry. The `ghc_feature_content` table links features to their related video content items.

**Requirements**:

- Managed via markdown files in `_videos/ghc-features/` (synced by `ContentSyncService`)
- **Metadata** (frontmatter):
  - Requires `plans` array indicating supported tiers (e.g. `["Free", "Pro", "Business", "Pro+", "Enterprise"]`)
  - Requires `ghes_support` boolean

**Features**:

- Tracked in `ghc_features` and `ghc_feature_content` database tables
- Populates the features timeline page at `/github-copilot/features` (via `/api/ghc-features`)
- Videos are matched to feature entries via the `feature_slug` FK in `ghc_feature_content` — when a match is found, the video thumbnail and link appear in the expanded feature detail
- Published items with YouTube URLs display video thumbnails and link to the video page

### VS Code Updates

**Location**: Videos stored under `collection_name = 'videos'` and tracked in the `vscode_update_items` lookup table.

Items are automatically identified based on feed name and title pattern rules configured in `appsettings.json` under `ContentProcessor.SubcollectionRules`. When a rule matches, the item is written to `vscode_update_items` in addition to `content_items`.

**Requirements**:

- Feed name and title must match a configured rule (e.g. feed `"Fokko at Work YouTube"`, title starting with `"Visual Studio Code and GitHub Copilot"`)
- `youtube_id` should be available for embedding

**Features**:

- Tracked in the `vscode_update_items` lookup table
- Populates the updates page at `/github-copilot/vscode-updates`
- Latest video is featured prominently
- Fetches content with `lastDays=0` to bypass the default 90-day date filter, since this is a curated collection that should show all items regardless of publication date

## Custom Page Ordering

Custom pages (non-collection pages like Features, Levels of Enlightenment, Handbook, etc.) support explicit ordering that controls their display in:

- Homepage section card badges
- Horizontal sub-navigation bar

### Configuration

Add the `Order` property to custom page collections in `appsettings.json`:

```json
{
  "Sections": {
    "github-copilot": {
      "Collections": {
        "features": {
          "Title": "Features",
          "Url": "/github-copilot/features",
          "Description": "GitHub Copilot features and capabilities",
          "Custom": true,
          "Order": 1
        },
        "levels-of-enlightenment": {
          "Title": "Levels of Enlightenment",
          "Url": "/github-copilot/levels-of-enlightenment",
          "Description": "Progressive learning path",
          "Custom": true,
          "Order": 2
        }
      }
    }
  }
}
```

All 10 custom page collections across 3 sections (`github-copilot`, `ai`, `devops`) use this pattern.

### Ordering Rules

1. Custom pages are sorted by `Order` value (lower numbers appear first)
2. If `Order` values are equal, pages are sorted alphabetically by `Title`
3. Regular collections (non-custom) don't use ordering

### Homepage Badge Display

On the homepage section cards:

- All regular collections are shown as badges
- First custom page (lowest `Order`) is always visible
- Additional custom pages are hidden behind a "+X more" button
- Clicking "+X more" permanently reveals remaining custom pages inline
