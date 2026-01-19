# Frontmatter Schema for .NET Tech Hub

This document defines the frontmatter fields used in Tech Hub content files for the .NET/Blazor implementation. All content must follow this schema.

## Core Principles

- **Configuration-Driven**: Content structure is defined by frontmatter, not hardcoded in templates
- **Collection-Based**: Each collection type may have specific required fields
- **Section Assignment**: Content can belong to multiple sections via `section_names` array

## Universal Fields (All Content)

These fields are **required** for ALL content files:

| Field | Type | Description | Example |
|-------|------|-------------|---------|
| `layout` | string | Always `"post"` for collection content | `"post"` |
| `title` | string | Content title (plain text) | `"GPT-5 Comes to GitHub Copilot"` |
| `author` | string | Author name or channel name | `"Rhea Patel"` |
| `viewing_mode` | string | Display mode: `"internal"` or `"external"` | `"external"` |
| `date` | datetime | Publication date in ISO format with timezone | `2025-08-12 17:10:52 +00:00` |
| `permalink` | string | URL path in format `/section/collection/slug` | `"/github-copilot/news/GPT-5-Comes-to-GitHub-Copilot-in-Visual-Studio"` |
| `tags` | array | Topic keywords (display format, can include spaces/special chars) | `["AI", "GitHub Copilot", "GPT-5"]` |
| `section_names` | array | Normalized section identifiers (lowercase, hyphenated) | `["ai", "github-copilot"]` |

### Field Details

#### `layout`

- **Always** set to `"post"` for collection content (_blogs,_news, _community,_videos, _roundups)
- Set to `"page"` for custom pages (not in collections)

#### `viewing_mode`

Determines how content is displayed:

- `"internal"`: Full content shown on site (videos, roundups, some custom content)
- `"external"`: Summary shown, links to original source (news, blogs, community)

**Collection defaults:**

- _videos: `"internal"` (embedded players)
- _roundups: `"internal"` (full summaries)
- _news: `"external"` (links to announcements)
- _blogs: `"external"` (links to original posts)
- _community: `"external"` (links to discussions)

#### `section_names`

- **Replaces** the old `categories` field (which contained display names)
- Contains **normalized identifiers** (lowercase, hyphenated)
- Maps to section configuration in `appsettings.json`
- Valid values: `["ai", "azure", "github-copilot", "dotnet", "devops", "security", "coding", "cloud"]`
- Content can belong to multiple sections
- If content includes `"github-copilot"`, it **should** also include `"ai"` (unless it's purely about Copilot without AI aspects)

#### `permalink`

- Format: `/section/collection/slug`
- **section**: Primary section from `section_names` array (first match from priority order)
- **collection**: Collection name without underscore (e.g., `news`, `videos`, `blogs`)
- **slug**: URL-friendly version of title or filename
- Example: `/github-copilot/news/GPT-5-Comes-to-GitHub-Copilot-in-Visual-Studio`

**Priority order for section selection:**

1. `github-copilot`
2. `ai`
3. `dotnet`
4. `azure`
5. `devops`
6. `security`
7. `coding`
8. `cloud`

## Optional Fields (Common)

These fields are optional but commonly used:

| Field | Type | Description | Example |
|-------|------|-------------|---------|
| `canonical_url` | string | Original source URL (for external content) | `"https://devblogs.microsoft.com/visualstudio/gpt-5-now-available/"` |
| `feed_name` | string | RSS feed display name | `"Microsoft VisualStudio Blog"` |
| `feed_url` | string | RSS feed URL | `"https://devblogs.microsoft.com/visualstudio/feed/"` |

## Collection-Specific Fields

### Videos (_videos/)

**Additional required fields:**

| Field | Type | Description | Example |
|-------|------|-------------|---------|
| *(none)* | | Videos use universal fields only | |

**Optional fields:**

| Field | Type | Description | Example |
|-------|------|-------------|---------|
| `youtube_id` | string | YouTube video ID (optional - can be extracted from content) | `"dQw4w9WgXcQ"` |

**YouTube video ID**: The parser can extract YouTube IDs from `{% youtube VIDEO_ID %}` tags in content, so the frontmatter field is optional.

**Alt-collection usage:**

- Used for videos in subfolders with special categorization needs
- Example: `_videos/ghc-features/` → `collection: "ghc-features"`
- Example: `_videos/vscode-updates/` → `collection: "vscode-updates"`

### Features (_videos/ghc-features/)

GitHub Copilot features require additional fields:

| Field | Type | Description | Example |
|-------|------|-------------|---------|
| `plans` | array | Supported subscription tiers | `["Free", "Business", "Enterprise"]` |
| `ghes_support` | boolean | GitHub Enterprise Server support | `true` or `false` |

**Optional:**

| Field | Type | Description | Example |
|-------|------|-------------|---------|
| `coming_soon` | boolean | Feature not yet released | `true` or `false` |

### VS Code Updates (_videos/vscode-updates/)

VS Code update videos require:

| Field | Type | Description | Example |
|-------|------|-------------|---------|

### News (_news/)

News items use universal fields only. Typically have:

- `viewing_mode: "external"`
- `canonical_url` pointing to original announcement
- `feed_name` and `feed_url` if from RSS feed

### Blogs (_blogs/)

Blog posts use universal fields only. Typically have:

- `viewing_mode: "external"`
- `canonical_url` pointing to original blog post
- `feed_name` and `feed_url` if from RSS feed

### Community (_community/)

Community content uses universal fields only. Typically have:

- `viewing_mode: "external"`
- `canonical_url` pointing to original community post

### Roundups (_roundups/)

Roundup summaries use universal fields only. Typically have:

- `viewing_mode: "internal"` (full content shown on site)
- `author: "Tech Hub Team"`
- `section_names: ["ai", "github-copilot"]` (covers all sections)

## Do Not Use These Fields

**Invalid/Unsupported Fields** - Do not include in frontmatter:

| Field | Reason |
|-------|--------|
| `categories` | Use `section_names` instead (normalized identifiers) |
| `tags_normalized` | Normalization happens in C# code |
| `excerpt_separator` | Use `<!--excerpt_end-->` marker in content |
| `description` | Excerpt serves this purpose |
| `page` | Collection is determined from file path |
| `video_id` | Use `youtube_id` instead |

## Frontmatter Formatting Rules

### Field Order

Follow this order for consistency:

1. `layout`
2. `title`
3. `author`
4. `canonical_url` (if applicable)
5. `viewing_mode`
6. `feed_name` (if applicable)
7. `feed_url` (if applicable)
8. `date`
9. `permalink`
10. `tags`
11. `section_names`
12. Collection-specific fields (plans, `ghes_support`, `coming_soon`)

### Quoting Rules

- **String values**: Always use double quotes (`"value"`)
- **Arrays**: Use square brackets with quoted values (`["value1", "value2"]`)
- **Booleans**: No quotes (`true` or `false`)
- **Dates**: No quotes (`2025-08-12 17:10:52 +00:00`)

### Date Format

- **Format**: `YYYY-MM-DD HH:MM:SS +HH:MM`
- **Timezone**: Always include timezone offset (e.g., `+00:00` for UTC, `+01:00` for Brussels)
- **Example**: `2025-08-12 17:10:52 +00:00`

### Filename Requirements

- **Format**: `YYYY-MM-DD-slug.md`
- **Date match**: Filename date must match frontmatter `date` field (date portion only)
- **Example**: `2025-08-12-GPT-5-Comes-to-GitHub-Copilot-in-Visual-Studio.md`

## Content Structure

All content files must follow this structure:

```markdown
---
layout: "post"
title: "Article Title"
author: "Author Name"
canonical_url: "https://example.com/article"
viewing_mode: "external"
feed_name: "Feed Name"
feed_url: "https://example.com/feed/"
date: 2025-08-12 17:10:52 +00:00
permalink: "/github-copilot/news/Article-Title"
tags: ["AI", "GitHub Copilot", "News"]
section_names: ["ai", "github-copilot"]
---

Excerpt goes here. Maximum 200 words. Mention the author and main points.<!--excerpt_end-->

## Main Content

Detailed article content follows the excerpt.

### Subheadings

More content here.

[Read the entire article here](https://example.com/article)
```

## Examples by Collection Type

### News Article

```yaml
---
layout: "post"
title: "GPT-5 Comes to GitHub Copilot in Visual Studio"
author: "Rhea Patel"
canonical_url: "https://devblogs.microsoft.com/visualstudio/gpt-5-now-available-in-visual-studio/"
viewing_mode: "external"
feed_name: "Microsoft VisualStudio Blog"
feed_url: "https://devblogs.microsoft.com/visualstudio/feed/"
date: 2025-08-12 17:10:52 +00:00
permalink: "/github-copilot/news/GPT-5-Comes-to-GitHub-Copilot-in-Visual-Studio"
tags: ["AI", "GitHub Copilot", "GPT-5", "Visual Studio", "News"]
section_names: ["ai", "github-copilot"]
---
```

### Video (Standard)

```yaml
---
layout: "post"
title: "AI-Powered Email Sorting with n8n and OpenAI"
author: "Alireza Chegini"
canonical_url: "https://www.youtube.com/watch?v=21HSDwtkHNk"
viewing_mode: "internal"
feed_name: "Alireza Chegini's YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCZSAqzABRmDxDHuPS6YuXZA"
date: 2025-06-04 19:00:03 +00:00
permalink: "/ai/videos/AI-Powered-Email-Sorting-with-n8n-and-OpenAI"
tags: ["AI", "Automation", "Email Management", "n8n", "OpenAI", "Videos"]
section_names: ["ai"]
---

Alireza Chegini shares a practical workflow to automate Gmail sorting using n8n and OpenAI.<!--excerpt_end-->

{% youtube 21HSDwtkHNk %}
```

### Video (GitHub Copilot Feature)

```yaml
---
layout: "post"
title: "Multi-File Editing with GitHub Copilot"
author: "GitHub Team"
canonical_url: "https://www.youtube.com/watch?v=example123"
viewing_mode: "internal"
date: 2025-01-15 10:00:00 +00:00
permalink: "/github-copilot/videos/Multi-File-Editing-with-GitHub-Copilot"
tags: ["GitHub Copilot", "Features", "Multi-File Editing", "AI", "Videos"]
section_names: ["ai", "github-copilot"]
collection: "ghc-features"
plans: ["Business", "Enterprise"]
ghes_support: false
---
```

### Blog Post

```yaml
---
layout: "post"
title: "TDD with GitHub Copilot in Python"
author: "Jesse Swart"
canonical_url: "https://sswart.github.io/post/tdd-python-copilot/"
viewing_mode: "external"
feed_name: "Jesse Swart's Blog"
feed_url: "https://blog.jesseswart.nl/index.xml"
date: 2024-06-24 14:06:14 +00:00
permalink: "/github-copilot/blogs/TDD-with-GitHub-Copilot-in-Python"
tags: ["AI", "Coding", "GitHub Copilot", "Python", "TDD", "Testing", "Blogs"]
section_names: ["ai", "coding", "github-copilot"]
---
```

### Roundup

```yaml
---
layout: "post"
title: "AI Agent Frameworks, Copilot Automation, and Security Improvements: Weekly Tech Highlights"
author: "Tech Hub Team"
viewing_mode: "internal"
date: 2025-12-29 09:00:00 +00:00
permalink: "/all/roundups/Weekly-AI-and-Tech-News-Roundup"
tags: ["AI", "Azure", "Automation", "GitHub Copilot", "DevOps", "Security", "Roundups"]
section_names: ["ai", "github-copilot"]
---
```

## Validation Rules

### Content Processing Scripts

When processing content, scripts must:

1. **Validate required fields**: Ensure all universal fields are present
2. **Normalize section names**: Convert display names to lowercase hyphenated identifiers
3. **Generate permalink**: Use primary section + collection + slug format
4. **Remove deprecated fields**: Strip out `categories`, `tags_normalized`, `excerpt_separator`, `description`, `page`
5. **Validate dates**: Ensure filename date matches frontmatter date
6. **Validate viewing_mode**: Must be either `"internal"` or `"external"`
7. **Extract YouTube IDs**: Parse `{% youtube ID %}` tags and optionally add to frontmatter

### Field Mapping from Old to New

| Old Field | New Field | Notes |
|-----------|-----------|-------|
| `categories` | `section_names` | Convert display names to lowercase identifiers |
| `tags_normalized` | *(removed)* | Normalization happens in C# code |
| `excerpt_separator` | *(removed)* | Use `<!--excerpt_end-->` marker in content |
| `description` | *(removed)* | Excerpt serves this purpose |
| `page` | *(removed)* | Derived from file path |
| `video_id` | `youtube_id` | Optional - can extract from content |

## Content Pipeline

**Content Processing**:

- **TechHub.ContentFixer**: One-time tool to clean up template variables and frontmatter from existing content
- **RSS Templates**: Generate correct frontmatter format (`section_names`, all required fields)
- **process-rss-to-markdown.ps1**: Converts AI categories (display names) → section_names (identifiers)
- **fix-markdown-files.ps1**: Fixes AI-generated markdown formatting issues (blank lines, heading spacing, etc.)
