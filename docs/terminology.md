# Site Terminology

## Core Concepts

### Naming Convention

When working with sections and collections in code, use consistent naming:

| Term | Meaning | Example |
|------|---------|--------|
| `section` | Refers to the entire Section object/concept | `var section = new Section()` |
| `sectionName` | Refers to the Name property (the string identifier/slug) | `"ai"`, `"github-copilot"` |
| `collection` | Refers to the entire Collection object/concept | `var collection = new Collection()` |
| `collectionName` | Refers to the Name property (the string identifier) | `"news"`, `"videos"` |

**Consistency Rule**: Use suffixed names (`sectionName`, `collectionName`) for string identifiers in API parameters, route parameters, and method parameters.

**Sections**: Top-level organizational units that group related content by topic or domain.

- **Purpose**: Provide thematic organization (e.g., AI, GitHub Copilot, Azure)
- **Configuration**: Defined in `appsettings.json` as single source of truth
- **Properties**: Each section includes display title, description, URL path, background image, and collections
- **Key Features**: Dynamic and configuration-driven - new sections added without code changes, each has own index page and navigation

**Collections**: Content types that represent different formats within sections.

- **Purpose**: Organize content by format and purpose (news, videos, community, blogs, roundups)
- **Configuration**: Defined in `appsettings.json`, associated with sections via section configuration
- **Technical**: Each collection has its own directory, can be marked as custom (manually created) or auto-generated
- **Properties**: Collections generate individual pages for each item via Blazor routing

**Items**: Individual pieces of content within collections. Also referred to as content or content items.

- **Definition**: Actual content users consume (articles, videos, announcements, blogs)
- **Terminology Note**: "Item" is the preferred term, but "Article" and "Post" are also used in code/documentation to refer to content (note: "Post" in variables does NOT specifically mean blogs from `_blogs/`)
- **Structure**: Markdown files with YAML front matter containing metadata (title, date, author, sections, tags) and content body
- **Section Names Frontmatter Field**: The `section_names` field in frontmatter contains section names (e.g., "ai", "github-copilot") that determine which sections this content appears in.
- **Processing**: Items are processed by the build system and can be listed on collection pages, filtered by date/tags/sections, displayed on section index pages, and included in RSS feeds

## Collection Types

- **News**: Official product updates and announcements
- **Videos**: Educational and informational video content
- **Community**: Community-sourced content and discussions
- **Blogs**: Blogs
- **Roundups**: Curated weekly content summaries

## Standard Values

### Section Names

Current valid section values (slugs) used in the system:

- `all` - Everything (Global scope)
- `github-copilot` - GitHub Copilot
- `ai` - AI
- `ml` - ML (Machine Learning)
- `devops` - DevOps
- `azure` - Azure
- `coding` - .NET
- `security` - Security

### Collection Names

Current valid collection values (slugs) used in the system:

- `news` - Official announcements and product updates
- `videos` - Video content and tutorials
- `community` - Community posts and discussions
- `blogs` - Blogs and technical articles
- `roundups` - Weekly content summaries

# Specialized Collections & Classification

## Specialized Collections

Specialized collections are predefined collection types that have specific rendering behavior:

| Slug | Display Name | Rendering Behavior |
|------|--------------|-------------------|
| `news` | News | Standard list view, reverse chronological |
| `blogs` | Blogs | Standard list view, reverse chronological |
| `videos` | Videos | Grid view with thumbnails |
| `roundups` | Roundups | Standard list view |
| `community` | Community | Author-centric view |
| `custom` | Custom | Special rendering (e.g., Features table) |

Any other collection slug is treated as a generic collection and rendered with the standard list view.

## Alt-Collection

The "Alt-Collection" property allows an item to appear in a secondary collection while physically residing in its primary collection folder.

- **Primary Collection**: Determined by the file system folder name (e.g., `_news/`).
- **Secondary Collection**: Defined via frontmatter `altCollection: <slug>`.

### Use Case

An item primarily belongs to `news` (Folder: `_news/`) but should also appear in a `videos` list (Alt: `videos`).

### Behavior

- The item appears in API responses for both collections.
- The item's `Collection` property reflects the query context:
  - When querying `news`: Returns `news`.
  - When querying `videos`: Returns `videos` (even though file is in `_news`).
