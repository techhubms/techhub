# Custom Pages API

This document describes the API endpoints for retrieving structured data for specific, highly customized pages in Tech Hub.

**Related Documentation**:

- [Content API](content-api.md) - Standard content endpoints
- [Terminology](terminology.md) - Collection and section definitions

## Overview

While most content in Tech Hub is standard markdown served via generic item endpoints, certain high-value pages require bespoke layouts and structured data that doesn't fit the standard content model. These are "Custom Pages".

**Characteristics**:

- **Hardcoded Endpoints**: Each custom page has a dedicated API endpoint.
- **Structured Data**: Returns a JSON object specifically shaped for that page's UI requirements (e.g., `LevelsPageData`, `FeaturesPageData`).
- **Composite Content**: Often aggregates data from multiple sources or specific content items.

**Common Use Case**: Landing pages, interactive guides (like "Levels of Enlightenment"), or feature matrices that need specific data structures for frontend rendering.

## Endpoints

Retrieves the structured data required to render the corresponding custom page.

### Developer Experience Space

**Endpoint**: `GET /api/custom-pages/dx-space`
**Data Model**: `DXSpacePageData`
**Description**: Data for the landing page of the Developer Experience Space.

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
**Description**: A matrix or list of specific GitHub Copilot features and their status.

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

### SDLC (Software Development Life Cycle)

**Endpoint**: `GET /api/custom-pages/sdlc`
**Data Model**: `SDLCPageData`
**Description**: Data mapping GenAI tools and practices to SDLC phases.

## Response Format

**Success Response**: `200 OK`
Returns the specific data object (JSON) for the requested page.

**Error Response**: `404 Not Found`
If the data cannot be constructed or the page definition is missing.

## Implementation

Custom pages content is hydrated from:

1. **JSON data files** in `collections/_custom/` directory (e.g., `dx-space.json`, `levels.json`)
2. **Aggregation of items** from standard collections (e.g., Features page pulls from `collections/_videos/ghc-features/`)

See [src/TechHub.Api/Endpoints/CustomPagesEndpoints.cs](../src/TechHub.Api/Endpoints/CustomPagesEndpoints.cs) for endpoint implementation.

## Content Sources for Custom Pages

Some custom pages are populated by specialized content collections based on their directory location.

### GitHub Copilot Features Content

**Location**: `collections/_videos/ghc-features/`

This subfolder is treated as a specialized collection for GitHub Copilot feature demonstrations.

**Requirements**:

- Must be placed in `collections/_videos/ghc-features/`
- **Frontmatter**:
  - `section_names: ["github-copilot", "ai"]` (Required)
  - `plans`: Array of supported tiers (Required: `["Free", "Pro", "Business", "Pro+", "Enterprise"]`)
  - `ghes_support`: Boolean (Required: `true` or `false`)

**Features**:

- Automatically identified as "Features" content based on directory location
- Populates the features page at `/github-copilot/features` (via `/api/custom-pages/features`)
- Supports filtering by subscription plan and GHES support

### Visual Studio Code Updates

**Location**: `collections/_videos/vscode-updates/`

This subfolder is treated as a specialized collection for VS Code update videos.

**Requirements**:

- Must be placed in `collections/_videos/vscode-updates/`
- **Frontmatter**:
  - `section_names: ["github-copilot", "ai"]` (Recommended if relevant)
  - `youtube_id`: ID of the video (Recommended for embedding)

**Features**:

- Automatically identified as "Updates" content based on directory location
- Populates the updates page at `/github-copilot/vscode-updates`
- Latest video is featured prominently

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
