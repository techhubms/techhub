# Frontmatter Schema

This document defines the complete frontmatter schema for content files in Tech Hub collections.

## File Structure

All content files use this structure:

1. **Frontmatter**: YAML metadata between `---` delimiters
2. **Excerpt**: Brief introduction (max 200 words) ending with `<!--excerpt_end-->`
3. **Content**: Main detailed content in Markdown format

## Required Fields (All Content)

All content files **must** include these frontmatter fields:

```yaml
---
layout: "post"                          # Always "post" for collection content
title: "Content Title"                  # Plain text title
author: "Author Name"                   # Author or presenter name
date: 2025-01-15 10:00:00 +01:00       # ISO format with timezone
permalink: "/section/collection/slug"   # URL path
tags: ["Tag1", "Tag2", "Tag3"]         # Topic keywords (can include spaces)
section_names: ["ai", "github-copilot"] # Normalized section identifiers
---
```

## Field Definitions

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `layout` | string | Yes | Always `"post"` for collection content |
| `title` | string | Yes | Content title in plain text (no HTML/markdown) |
| `author` | string | Yes | Author or presenter name |
| `date` | datetime | Yes | ISO format with timezone (`YYYY-MM-DD HH:MM:SS +HH:MM`) |
| `permalink` | string | Yes | URL path: `/section/collection/slug` |
| `tags` | array | Yes | Topic keywords (display format, can include spaces) |
| `section_names` | array | Yes | Normalized section identifiers (lowercase, hyphenated) |

### Layout

Always `"post"` for collection content (_news,_videos, _blogs,_community, _roundups).

### Title

Content title in plain text. No HTML or markdown formatting.

### Author

Author name or presenter name. For YouTube videos, use the actual presenter/demonstrator, not the channel owner.

### Date

Publication date in ISO format with timezone:

- Format: `YYYY-MM-DD HH:MM:SS +HH:MM`
- Timezone: Use `+01:00` for Europe/Brussels (or `+02:00` during DST)
- **Important**: Filename date must match frontmatter date (date portion only)

### Permalink

URL path in format `/section/collection/slug`:

- **section**: Primary section (determined automatically by the system)
- **collection**: Collection name without underscore (e.g., `news`, `videos`, `blogs`)
- **slug**: URL-friendly version of title

Example: `/github-copilot/news/GPT-5-Comes-to-GitHub-Copilot`

### Tags

Array of topic keywords in display format (can include spaces and special characters):

```yaml
tags: ["AI", "GitHub Copilot", "Visual Studio"]
```

### Section Names

Array of normalized section identifiers (lowercase, hyphenated):

- Valid values: `["ai", "azure", "github-copilot", "dotnet", "devops", "security", "coding", "ml"]`
- Content can belong to multiple sections
- If content includes `"github-copilot"`, it should also include `"ai"` (unless purely about Copilot setup/admin)

## Content Source Fields

**Required for all content except roundups** (news, blogs, community, videos):

```yaml
external_url: "https://example.com/article"  # Original source URL
feed_name: "Feed Display Name"                # RSS feed name
```

**Not required for roundups** (since they are original Tech Hub content).

## Collection-Specific Fields

### GitHub Copilot Features (`_videos/ghc-features/`)

```yaml
plans: ["Free", "Pro", "Business"]  # Required - supported subscription tiers
ghes_support: true                  # Required - GitHub Enterprise Server support
```

## Frontmatter to Domain Model Mapping

This table shows how frontmatter fields map to C# domain properties:

| Frontmatter Field | Domain Property | Type | Notes |
|-------------------|-----------------|------|-------|
| `title` | `Title` | `string` | Required |
| `author` | `Author` | `string?` | Optional |
| `date` | `DateEpoch` | `long` | Converted to Unix timestamp in Europe/Brussels timezone |
| `section_names` | `Sections` | `IReadOnlyList<string>` | Multi-section support |
| `tags` | `Tags` | `IReadOnlyList<string>` | Normalized to lowercase, hyphen-separated |
| `external_url` | `ExternalUrl` | `string?` | Original source URL |
| Filename | `Slug` | `string` | `2025-01-15-article.md` → `2025-01-15-article` |
| Before `<!--excerpt_end-->` | `Excerpt` | `string` | Plain text, max 200 words |
| Full markdown | `RenderedHtml` | `string` | Processed with Markdig |

## Example Files

### News Article

```yaml
---
layout: "post"
title: "GPT-5 Comes to GitHub Copilot in Visual Studio"
author: "Rhea Patel"
external_url: "https://devblogs.microsoft.com/visualstudio/gpt-5-now-available/"
feed_name: "Microsoft VisualStudio Blog"
date: 2025-08-12 17:10:52 +01:00
permalink: "/github-copilot/news/GPT-5-Comes-to-GitHub-Copilot-in-Visual-Studio"
tags: ["AI", "GitHub Copilot", "GPT-5", "Visual Studio"]
section_names: ["ai", "github-copilot"]
---

Rhea Patel announces the availability of GPT-5 in GitHub Copilot
for Visual Studio users, bringing enhanced code suggestions...

<!--excerpt_end-->

## Full Article Content

...
```

### Standard Video

```yaml
---
layout: "post"
title: "AI-Powered Email Sorting with n8n and OpenAI"
author: "Alireza Chegini"
external_url: "https://www.youtube.com/watch?v=21HSDwtkHNk"
date: 2025-06-04 19:00:03 +01:00
permalink: "/ai/videos/AI-Powered-Email-Sorting"
tags: ["AI", "Automation", "n8n", "OpenAI"]
section_names: ["ai"]
---

Alireza Chegini demonstrates how to build an automated email
sorting pipeline using n8n workflows and OpenAI...

<!--excerpt_end-->

## Video Content

...
```

### GitHub Copilot Feature

```yaml
---
layout: "post"
title: "Multi-File Editing with GitHub Copilot"
author: "GitHub Team"
date: 2025-01-15 10:00:00 +01:00
permalink: "/github-copilot/videos/Multi-File-Editing"
tags: ["GitHub Copilot", "Features", "Multi-File Editing"]
section_names: ["ai", "github-copilot"]
plans: ["Business", "Enterprise"]
ghes_support: false
---

The GitHub Team demonstrates how to use multi-file editing
capabilities in GitHub Copilot...

<!--excerpt_end-->

## Feature Details

...
```

## Excerpt Section

### Definition

An introduction that summarizes the main points and mentions the author.

### Purpose

Serves as a logical introduction to the main content and provides users with a quick overview of what to expect.

### Requirements

- Must appear directly after the frontmatter
- **Maximum 200 words**
- Must be immediately followed by the `<!--excerpt_end-->` marker
- Should be informative and engaging
- **Must mention the author's name**

### Example

```markdown
---
title: Example Post
author: John Smith
date: 2025-01-15
---

John Smith introduces the latest updates to GitHub Copilot, including new features that improve code completion accuracy and provide better suggestions for complex algorithms. This article covers the key improvements and how they benefit developers.

<!--excerpt_end-->

## Detailed Content Begins Here

...
```

### Processing

- Content before `<!--excerpt_end-->` is extracted as the `Excerpt` property
- Excerpt is converted to plain text (no HTML/markdown)
- Used for content cards, meta descriptions, and RSS feed descriptions

> **See also**: [docs/terminology.md](terminology.md) for content organization concepts.

## Example: Roundup

```yaml
---
layout: "post"
title: "AI Agent Frameworks and Security: Weekly Tech Highlights"
author: "Tech Hub Team"
date: 2025-12-29 09:00:00 +01:00
permalink: "/all/roundups/Weekly-Tech-Highlights"
tags: ["AI", "Azure", "GitHub Copilot", "DevOps", "Security"]
section_names: ["ai", "github-copilot"]
---

The Tech Hub Team presents this week's highlights covering AI agent
frameworks and security developments...

<!--excerpt_end-->

## Weekly Highlights

...
```

## Deprecated Fields (Do Not Use)

These fields are no longer supported:

| Field | Replacement |
|-------|-------------|
| `categories` | Use `section_names` instead |
| `tags_normalized` | Normalization happens automatically |
| `excerpt_separator` | Use `<!--excerpt_end-->` marker in content |
| `description` | Excerpt serves this purpose |
| `page` | Collection is determined from file path |
| `video_id` | Use `youtube_id` instead (or embed in content) |

## File Naming Convention

All content files use the format: `YYYY-MM-DD-title-slug.md`

- Date in filename must match the `date` field in frontmatter
- Title slug should be URL-friendly (lowercase, hyphens)

## Implementation Reference

- **Frontmatter parsing**: [src/TechHub.Infrastructure/Services/FrontMatterParser.cs](../src/TechHub.Infrastructure/Services/FrontMatterParser.cs)
- **Content model**: [src/TechHub.Core/Models/Core/ContentItem.cs](../src/TechHub.Core/Models/Core/ContentItem.cs)
- **Content management**: [collections/AGENTS.md](../collections/AGENTS.md)
