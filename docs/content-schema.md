# Content Entity Schema

This document defines the complete schema for ContentItems stored in the Tech Hub database.

## Entity Structure

All content items fundamentally contain:

1. **Metadata**: Core structured metadata mapping (Title, Author, Date, Tags, Sections).
2. **Excerpt**: A brief summary or introduction snippet suitable for feed and card previews.
3. **Markdown Content**: Main detailed body content in markdown format.

## Required Properties

All primary content records **must** provide values mapped for these conceptual fields:

```yaml
title: "Content Title"                  # Plain text title
author: "Author Name"                   # Author or publisher
date: '2025-01-15T10:00:00Z'            # Standard DateTime
permalink: "/section/collection/slug"   # Canonical routing path
tags: ["Tag1", "Tag2", "Tag3"]          # Topic keywords 
section_names: ["ai", "github-copilot"] # Normalized section relations
```

## Property Definitions

| Property | Type | Required | Description |
|-------|------|----------|-------------|
| `title` | string | Yes | Content title in plain text (no HTML/markdown) |
| `author` | string | Yes | Author or presenter name |
| `date` | datetime | Yes | Evaluated date and time used for timeline sorting |
| `permalink` | string | Yes | URL path: `/section/collection/slug` |
| `tags` | array | Yes | Topic keywords (display format, can include spaces) |
| `section_names` | array | Yes | Normalized section identifiers (lowercase, hyphenated) |

### Title

Content title in plain text. No HTML or markdown formatting.

### Author

Author name or presenter name. For YouTube videos, use the actual presenter/demonstrator.

### Date

Publication or creation date:

- Normalized into UTC database columns `date_epoch` and structured DateTime internally
- Preserves the logical publish timing

### Permalink

URL routing syntax `/section/collection/slug`:

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

- Valid values: `["ai", "azure", "github-copilot", "dotnet", "devops", "security", "ml"]`
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

## Metadata to Domain Model Mapping

This table shows how properties map to C# domain properties inside `TechHub.Core`:

| Entity Property | Domain Property | Type | Notes |
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

- Acts as a logical preamble before detailed content
- **Maximum 200 words**
- Separated via the `<!--excerpt_end-->` marker (or strictly stored in `excerpt` DB field depending on source pipeline)
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
| `page` | Collection mapped appropriately in storage |
| `video_id` | Use `youtube_id` instead (or embed in content) |

## Implementation Reference

- **Content model**: [src/TechHub.Core/Models/Core/ContentItem.cs](../src/TechHub.Core/Models/Core/ContentItem.cs)
- **Content management**: [collections/AGENTS.md](../collections/AGENTS.md)
