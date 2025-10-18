# Site Overview

This document provides a comprehensive overview of how the Tech Hub site is structured and configured, with a focus on the modular section system, collections, and the plugins that generate pages dynamically.

## Main Index (`index.md`)

The main index page serves as the entry point to the site. It uses the `home` layout and displays:

- A grid of available sections (dynamically generated from `site.data.sections`)
- Roundups collection items
- Latest content from all collections

The sections grid is populated using Liquid templating:

```liquid
{% for section in site.data.sections %}
{% assign section_key = section[0] %}
{% assign section_data = section[1] %}
```

## Section Architecture

The site is organized into multiple sections, each representing a distinct topic area:

### Current Sections (as of 2025-08-22)

1. **Everything Section** (`/all`)
   - Category: "All"
   - Comprehensive hub for all content across every section and collection
2. **AI Section** (`/ai`)
   - Category: "AI"
   - General AI-related content including GitHub Copilot
3. **GitHub Copilot Section** (`/github-copilot`)
   - Category: "GitHub Copilot"
   - Dedicated GitHub Copilot content
4. **ML Section** (`/ml`)
   - Category: "ML"
   - Machine learning and data science with Microsoft's platform ecosystem
5. **Azure Section** (`/azure`)
   - Category: "Azure"
   - Microsoft cloud platform content
6. **.NET Section** (`/coding`)
   - Category: "Coding"
   - .NET ecosystem and development content
7. **DevOps Section** (`/devops`)
   - Category: "DevOps"
   - Development lifecycle and automation content
8. **Security Section** (`/security`)
   - Category: "Security"
   - Microsoft security landscape content

### Section Structure

Each section contains:

- **Index page**: Auto-generated overview of the section with RSS feed link
- **RSS Feed**: Dedicated feed for the section's content (e.g., `/ai.xml`, `/github-copilot.xml`)
- **Collections**: Various content types (news, blogs, videos, etc.)
- **Custom pages**: Manually created pages specific to the section

## Section Navigation System

### `assets/js/sections.js`

This JavaScript file handles section-specific navigation behaviors and is the **only JavaScript file permitted to modify content on page load**.

**Purpose:**

- Manages section-based URL parameters and state
- Handles section collection activation based on navigation
- Provides smooth transitions between sections

**Why This Exception Exists:**

- Section navigation is fundamental site architecture, not user interaction
- Required for proper section-based routing and deep linking
- Ensures consistent section state across page loads and browser navigation

**Key Restriction:** All other JavaScript files must wait for explicit user interaction before modifying page content. Only `sections.js` can run automatically on page load.

## Collections System

Collections are defined in `_config.yml` and represent different content types:

```yaml
collections:
  news:
    output: true
  videos:
    output: true    
  community:
    output: true       
  events:
    output: true
  roundups:
    output: true
```

### Collection Types

- **news**: Official announcements and product updates
- **videos**: YouTube content and video tutorials
  - **ghc-features subfolder**: Special subfolder within `_videos` containing GitHub Copilot feature demonstration videos that automatically populate the GitHub Copilot features page
- **community**: Microsoft Tech community posts and other community-sourced content
- **events**: Official events and community meetups
- **posts**: Blog posts and technical articles
- **roundups**: Curated content summaries (special collection shown on homepage)

## Configuration Files

### `_config.yml`

Primary Jekyll configuration file containing:

- Site metadata (title, description, URLs)
- Collection definitions
- Plugin configurations
- Build settings (Kramdown, Sass, etc.)

### Global Standards

**Timezone Consistency**: All date operations must use `Europe/Brussels` timezone consistently. This applies to:

- Date formatting in content: `date: 2025-07-19 12:00:00 +0200`
- Jekyll configuration timezone setting
- JavaScript date calculations
- Plugin date processing

**Data File Standards**: The single source of truth for sections and collections is `_data/sections.json`. Always use `site.data.sections` to access this data.

### `_data/sections.json` - Source of Truth

This is the **single source of truth** for section configuration. It defines:

```json
{
  "section-key": {
    "title": "Section Display Name",
    "description": "Section description",
    "url": "/section-path",
    "section": "section-key",
    "image": "/assets/section-backgrounds/image.jpg",
    "category": "Category Name",
    "collections": [
      {
        "title": "Collection Title",
        "url": "/section/collection.html",
        "page": "collection-type",
        "description": "Collection description",
        "custom": false
      }
    ]
  }
}
```

**Key Properties:**

- `custom: false`: Pages generated automatically by plugin
- `custom: true`: Manually created pages
- `category`: Used for content filtering and organization
- `collections`: Array of available content types per section

## Plugin System

### `_plugins/section_pages_generator.rb`

The core plugin that creates pages based on `sections.json`:

**Functionality:**

1. Reads `_data/sections.json`
2. Generates section index pages automatically
3. Creates collection pages for entries marked `custom: false`
4. Uses templates to ensure consistent page structure

**Generated Files:**

- `{section}/index.md`: Section overview page
- `{section}/{collection}.html`: Collection listing pages

### Other Plugins

- **`tag_filters.rb`**: Provides filtering functionality and tag processing
- **`date_filters.rb`**: Date formatting utilities and date-based filtering
- **`string_filters.rb`**: String processing and validation utilities
- **`youtube.rb`**: YouTube integration features

## Section Directories

### Current Section Folders (as of 2025-08-22)

- **`ai/`**: Contains AI section-specific files
  - Custom pages like `ai-to-z.md`
  - Section-specific assets and templates
- **`github-copilot/`**: Contains GitHub Copilot section files
  - Custom pages like `features.md` and `levels-of-enlightenment.md`
  - `features.md`: Dynamic GitHub Copilot features page that automatically populates from videos in `_videos/ghc-features/` subfolder
  - Copilot-specific resources

**Note**: Other sections (ML, Azure, Coding, DevOps, Security) are auto-generated by Jekyll plugins and don't have physical directories. Their pages are created dynamically using the section configuration in `_data/sections.json`.

## Testing and Quality Assurance

### `spec/` Directory

Contains automated tests for:

- Site configuration validation
- Content quality checks
- Link validation
- Plugin functionality testing

## Other Important Directories

### Core Jekyll Directories

- **`_includes/`**: Reusable template components
- **`_layouts/`**: Page layout templates
- **`_plugins/`**: Custom Jekyll plugins for site functionality
- **`_sass/`**: Stylesheet organization
- **`assets/`**: Static assets (images, CSS, JS)

### Content Directories

There is one for each collection:

- **`_community/`**: Community-sourced content files
- **`_news/`**: News article files
- **`_videos/`**: Video content metadata
  - **`_videos/ghc-features/`**: GitHub Copilot feature demonstration videos with special frontmatter requirements (`section: "github-copilot"`, `plans`, `ghes_support`) that automatically populate `/github-copilot/features.md`
- **`_events/`**: Events such as meetings or conferences
- **`_posts/`**: Blog posts
- **`_roundups/`**: Roundup content

### Build and Deployment

- **`_site/`**: Generated site output (ignored in git)
- **`test-results/`**: Test execution results
- **`.tmp/`**: Temporary directory for development scripts and intermediate files
- **`.github/scripts/`**: Production automation scripts
- **`.github/workflows/`**: GitHub Actions workflow definitions
- **`.github/prompts/`**: Custom reusable prompt files
- **`docs/`**: All technical documentation
- **`jekyll-*.ps1`**: PowerShell scripts for development workflow

## Key Architectural Principles

1. **Configuration-Driven**: All sections and collections defined in data files
2. **Dynamic Generation**: Pages created automatically from configuration
3. **Modular Design**: New sections can be added by updating `sections.json`
4. **Consistent Structure**: Plugin ensures uniform page generation
5. **Content Separation**: Clear distinction between generated and custom pages

## Data Flow

1. Content creators add files to collection directories (`_news/`, `_videos/`, etc.)
2. `sections.json` defines which collections appear in which sections
3. `section_pages_generator.rb` reads configuration and creates pages
4. Jekyll processes all content and generates the static site
5. Generated pages use data from `site.data.sections` for navigation and display

This architecture enables easy expansion of the site by adding new sections to `sections.json` without requiring code changes, while maintaining consistency through automated page generation.
