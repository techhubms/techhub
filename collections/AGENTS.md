# Collections Management Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `collections/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Critical Content Rules

### ✅ Always Do

- **Read writing-style-guidelines.md FIRST** before creating or editing ANY content
- **Use filename format `YYYY-MM-DD-title-slug.md`** for all content files
- **Match filename date with frontmatter date** for `layout: post` files
- **Include excerpt section** - Max 200 words, end with `<!--excerpt_end-->`
- **Mention author in excerpt** - Author name must appear in intro
- **Place files in correct collection directory** based on content type
- **Add `alt-collection` frontmatter** for special video subcollections (ghc-features, vscode-updates)

### ⚠️ Ask First

- **Creating new collection types** or changing collection structure
- **Modifying frontmatter schema** or required fields
- **Changing content processing** workflows

### 🚫 Never Do

- **Never create content without reading guidelines** - writing-style-guidelines.md is REQUIRED reading
- **Never skip excerpt section** - All content needs introduction
- **Never exceed 200 words in excerpt** - Keep intros concise
- **Never forget `<!--excerpt_end-->` marker** - Required to separate excerpt from main content
- **Never omit author from excerpt** - Must mention who wrote/presented
- **Never hardcode collection names** - Configuration-driven via `_data/sections.json`
- **Never skip frontmatter** - All markdown needs YAML front matter
- **Never mismatch filename and frontmatter dates** - Must be identical for posts
- **Never place files in wrong collection** - Use correct directory for content type
- **Never automatically run processing scripts** - Manual fixes preferred

## Overview

**CRITICAL**: Before creating or editing ANY content in this directory, you MUST read:

- **[writing-style-guidelines.md](writing-style-guidelines.md)** - Tone, voice, language standards, and writing quality requirements

**For markdown formatting issues**: Use `npx markdownlint-cli2 --fix <file-path>` to automatically fix formatting (see [Root AGENTS.md](../AGENTS.md) step 9)

## When to Use This Guide

**Read this file when**:

- Creating new content items (news, videos, posts, etc.)
- Editing existing collection content
- Understanding frontmatter requirements
- Working with special collection types (alt-collection)
- Managing content structure and organization

**Related Documentation**:

- Writing style → [writing-style-guidelines.md](writing-style-guidelines.md)
- Markdown formatting → Use `npx markdownlint-cli2 --fix <file-path>` for automated fixes
- Content workflows → [docs/content-management.md](../docs/content-management.md)
- RSS processing → [scripts/content-processing/AGENTS.md (future)](../scripts/content-processing/)

## Collections Structure

This directory contains the content collections for the Tech Hub site. Each collection represents a specific content type with its own processing rules:

- **_community**: Community-sourced content and discussions
- **_news**: Official product updates and announcements
- **_blogs**: Blogs
- **_roundups**: Curated weekly content summaries
- **_videos**: Educational and informational video content
  - **_videos/ghc-features/**: GitHub Copilot features (requires `alt-collection: "features"`, `plans: []`, `ghes_support` in frontmatter)
  - **_videos/vscode-updates/**: VS Code updates (requires `alt-collection: "vscode-updates"` in frontmatter)

## Content Organization

**File Placement:**

- Place files in the appropriate collection directory based on content type
- Use filename format: `YYYY-MM-DD-title-slug.md`
- Date in filename must match the `date` field in front matter for `layout: post` files

**Configuration:**

- Collections are defined in `appsettings.json` in the .NET application
- Each collection has its own processing rules and display logic
- Never hardcode collection names - they are configuration-driven

**Content Structure (All Files):**

1. **Front Matter**: YAML metadata (see [Frontmatter Schema](#frontmatter-schema) below for required fields)
2. **Excerpt**: Brief introduction (max 200 words) ending with `<!--excerpt_end-->`
3. **Content**: Main detailed content in Markdown format

## Frontmatter Schema

### Required Fields (All Content)

All content files **must** include these frontmatter fields:

```yaml
---
layout: "post"                          # Always "post" for collection content
title: "Content Title"                  # Plain text title
author: "Author Name"                   # Author or presenter name
date: 2025-01-15 10:00:00 +01:00       # ISO format with timezone (Europe/Brussels)
permalink: "/section/collection/slug"   # URL path
tags: ["Tag1", "Tag2", "Tag3"]         # Topic keywords (can include spaces)
section_names: ["ai", "github-copilot"] # Normalized section identifiers
---
```

### Field Definitions

**`layout`**: Always `"post"` for collection content (_news,_videos, _blogs,_community, _roundups)

**`title`**: Content title in plain text (no HTML or markdown)

**`author`**: Author name or presenter name. For YouTube videos, use the actual presenter/demonstrator, not the channel owner.

**`date`**: Publication date in ISO format with timezone

- Format: `YYYY-MM-DD HH:MM:SS +HH:MM`
- Timezone: Use `+01:00` for Europe/Brussels (or `+02:00` during DST)
- Filename date must match frontmatter date (date portion only)

**`permalink`**: URL path in format `/section/collection/slug`

- **section**: Primary section (determined automatically by the system)
- **collection**: Collection name without underscore (e.g., `news`, `videos`, `blogs`)
- **slug**: URL-friendly version of title
- Example: `/github-copilot/news/GPT-5-Comes-to-GitHub-Copilot`

**`tags`**: Array of topic keywords in display format (can include spaces and special characters)

- Example: `["AI", "GitHub Copilot", "Visual Studio"]`

**`section_names`**: Array of normalized section identifiers (lowercase, hyphenated)

- Valid values: `["ai", "azure", "github-copilot", "dotnet", "devops", "security", "coding", "ml"]`
- Content can belong to multiple sections
- If content includes `"github-copilot"`, it should also include `"ai"` (unless purely about Copilot setup/admin)
- Primary section for permalink is determined automatically by the system

### Content Source Fields

**Required for all content except roundups** (news, blogs, community, videos):

```yaml
canonical_url: "https://example.com/article"  # Original source URL (required)
feed_name: "Feed Display Name"                # RSS feed name (required)
```

**Not required for roundups** (since they are original Tech Hub content).

### Collection-Specific Fields

**GitHub Copilot Features** (`_videos/ghc-features/`):

```yaml
plans: ["Free", "Pro", "Business"]  # Required - supported subscription tiers
ghes_support: true                  # Required - GitHub Enterprise Server support
```

### Frontmatter Examples

**News Article:**

```yaml
---
layout: "post"
title: "GPT-5 Comes to GitHub Copilot in Visual Studio"
author: "Rhea Patel"
canonical_url: "https://devblogs.microsoft.com/visualstudio/gpt-5-now-available/"
feed_name: "Microsoft VisualStudio Blog"
feed_url: "https://devblogs.microsoft.com/visualstudio/feed/"
date: 2025-08-12 17:10:52 +01:00
permalink: "/github-copilot/news/GPT-5-Comes-to-GitHub-Copilot-in-Visual-Studio"
tags: ["AI", "GitHub Copilot", "GPT-5", "Visual Studio"]
section_names: ["ai", "github-copilot"]
---
```

**Standard Video:**

```yaml
---
layout: "post"
title: "AI-Powered Email Sorting with n8n and OpenAI"
author: "Alireza Chegini"
canonical_url: "https://www.youtube.com/watch?v=21HSDwtkHNk"
date: 2025-06-04 19:00:03 +01:00
permalink: "/ai/videos/AI-Powered-Email-Sorting"
tags: ["AI", "Automation", "n8n", "OpenAI"]
section_names: ["ai"]
---
```

**GitHub Copilot Feature:**

```yaml
---
layout: "post"
title: "Multi-File Editing with GitHub Copilot"
author: "GitHub Team"
date: 2025-01-15 10:00:00 +01:00
permalink: "/github-copilot/videos/Multi-File-Editing"
tags: ["GitHub Copilot", "Features", "Multi-File Editing"]
section_names: ["ai", "github-copilot"]
alt-collection: "features"
plans: ["Business", "Enterprise"]
ghes_support: false
---
```

**Roundup:**

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
```

### Deprecated Fields (Do Not Use)

These fields are no longer supported and should **not** be included:

- `categories` - Use `section_names` instead
- `tags_normalized` - Normalization happens automatically in C# code
- `excerpt_separator` - Use `<!--excerpt_end-->` marker in content
- `description` - Excerpt serves this purpose
- `page` - Collection is determined from file path
- `video_id` - Use `youtube_id` instead (or embed in content)

### Excerpt Section

**Definition**: An introduction that summarizes the main points and mentions the author.

**Purpose**: Serves as a logical introduction to the main content and provides users with a quick overview of what to expect.

> **See also**: [Root AGENTS.md - Site Terminology](../AGENTS.md#site-terminology) for complete content organization concepts and structure definitions.

**Requirements**:

- Must appear directly after the frontmatter
- Maximum 200 words
- Must be immediately followed by the `<!--excerpt_end-->` code
- Should be informative and engaging
- Must mention the author's name

**Example**:

```markdown
---
title: Example Post
date: 2025-01-15
---

John Smith introduces the latest updates to GitHub Copilot, including new features that improve code completion accuracy and provide better suggestions for complex algorithms. This article covers the key improvements and how they benefit developers.

<!--excerpt_end-->

## Detailed Content Begins Here

...
```

### Content Section

**Definition**: The main content section that provides detailed, well-structured information.

**Purpose**: Delivers the full information following logically from the excerpt.

**Requirements**:

- Always comes after the excerpt and the `<!--excerpt_end-->` marker
- Can be as long as needed
- Must follow the excerpt logically
