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
**Description**: A chronological timeline of GitHub Copilot features showing when each capability was introduced, along with subscription tier comparisons and related video content.

**Key fields**:

- `timelineFeatures` — Ordered list of `FeatureTimelineItem` entries, each with:
  - `id` — Unique slug identifier
  - `title` — Feature name
  - `description` — Brief description of the capability
  - `releaseDate` — ISO 8601 date string (e.g. `"2024-06-01"`)
  - `tiers` — Subscription tiers that include this feature
  - `ghesSupport` — Whether the feature is available on GitHub Enterprise Server
  - `category` — High-level category (e.g. `"Code Completion"`, `"Chat"`, `"Agent"`)
  - `videoSlug` — Optional slug of a related video in the `ghc-features` collection
- `subscriptionTiers` — Comparison cards shown in the sidebar
- `featureSections` — Legacy groupings used by the video card grid

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

1. **Database** (`custom_page_data` table) — JSON objects representing the structured data are stored directly in the database. Admin users can edit the JSON directly from the admin UI.
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

The VS Code Updates page (`GitHubCopilotVSCodeUpdates.razor`) is configured as `Custom: true` but does **not** use a custom pages API endpoint. It fetches content through the standard content collection API pulling from the `vscode-updates` subcollection.

See [src/TechHub.Api/Endpoints/CustomPagesEndpoints.cs](../src/TechHub.Api/Endpoints/CustomPagesEndpoints.cs) for endpoint implementation.

## Content Sources for Custom Pages

Some custom pages are populated by specialized content collections based on their `subcollection` field in the database.

### GitHub Copilot Features Content

**Location**: Managed in the database under `videos` collection with subcollection `ghc-features`

This subcollection is treated as a specialized collection for GitHub Copilot feature demonstrations.

**Requirements**:

- Must be categorized in the database with subcollection `ghc-features`
- **Metadata**:
  - Requires `plans` array in AI metadata indicating supported tiers (Required: `["Free", "Pro", "Business", "Pro+", "Enterprise"]`)
  - Requires `ghes_support` boolean in AI metadata

**Features**:

- Automatically identified as "Features" content based on subcollection
- Populates the features page at `/github-copilot/features` (via `/api/custom-pages/features`)
- Fetches content with `lastDays=0` to bypass the default 90-day date filter, since this is a curated collection that should show all items regardless of publication date
- Supports per-section filtering by GHES support and video availability
- Layout: "Free" tier displayed full-width, four paid tiers displayed side-by-side in a grid
- Published items with YouTube URLs display video thumbnails and link to the video page
- Draft items display as "Coming Soon" cards without video thumbnails

### VS Code Updates

**Location**: Managed in the database under `videos` collection with subcollection `vscode-updates`

This subcollection is treated as a specialized collection for VS Code update videos.

**Requirements**:

- Must be categorized in the database with subcollection `vscode-updates`
- **Metadata**:
  - `youtube_id` should be available for embedding

**Features**:

- Automatically identified as "Updates" content based on subcollection
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
