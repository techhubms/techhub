# Collections Management Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `collections/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Follow the 9-step workflow in Root [AGENTS.md](../AGENTS.md). Project principles are in [README.md](../README.md). Follow **BOTH**.

## Critical Content Rules

### ✅ Always Do

- **Always read [docs/writing-style-guidelines.md](../docs/writing-style-guidelines.md) FIRST** before creating or editing ANY content
- **Always use filename format `YYYY-MM-DD-title-slug.md`** for all content files
- **Always match filename date with frontmatter date** for `layout: post` files
- **Always include excerpt section** - Max 200 words, end with `<!--excerpt_end-->`
- **Always mention author in excerpt** - Author name must appear in intro
- **Always place files in correct collection directory** based on content type

### ⚠️ Ask First

- **Ask first before creating new collection types** or changing collection structure
- **Ask first before modifying frontmatter schema** or required fields
- **Ask first before changing content processing** workflows

### 🚫 Never Do

- **Never create content without reading guidelines** - [docs/writing-style-guidelines.md](../docs/writing-style-guidelines.md) is REQUIRED reading
- **Never skip excerpt section** - All content needs introduction
- **Never exceed 200 words in excerpt** - Keep intros concise
- **Never forget `<!--excerpt_end-->` marker** - Required to separate excerpt from main content
- **Never omit author from excerpt** - Must mention who wrote/presented
- **Never hardcode collection names** - Configuration-driven via `appsettings.json`
- **Never skip frontmatter** - All markdown needs YAML front matter
- **Never mismatch filename and frontmatter dates** - Must be identical for posts
- **Never place files in wrong collection** - Use correct directory for content type
- **Never automatically run processing scripts** - Manual fixes preferred

## Overview

**CRITICAL**: Before creating or editing ANY content in this directory, you MUST read:

- **[docs/writing-style-guidelines.md](../docs/writing-style-guidelines.md)** - Tone, voice, language standards, and writing quality requirements

**For markdown formatting issues**: Use `npx markdownlint-cli2 --fix <file-path>` to automatically fix formatting (see [Root AGENTS.md](../AGENTS.md) step 9)

## When to Use This Guide

**Read this file when**:

- Creating new content items (news, videos, posts, etc.)
- Editing existing collection content
- Understanding frontmatter requirements
- Managing content structure and organization

**Related Documentation**:

- Writing style → [docs/writing-style-guidelines.md](../docs/writing-style-guidelines.md)
- Markdown formatting → Use `npx markdownlint-cli2 --fix <file-path>` for automated fixes
- Content workflows → [docs/content-processing.md](../docs/content-processing.md)
- RSS processing → [scripts/content-processing/AGENTS.md (future)](../scripts/content-processing/)

## Collections Structure

This directory contains the content collections for the Tech Hub site. Each collection represents a specific content type with its own processing rules:

- **_community**: Community-sourced content and discussions
- **_news**: Official product updates and announcements
- **_blogs**: Blogs
- **_roundups**: Curated weekly content summaries
- **_videos**: Educational and informational video content
  - **_videos/ghc-features/**: GitHub Copilot features (derived from directory path)
  - **_videos/vscode-updates/**: VS Code updates (derived from directory path)

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

📖 **Full documentation**: See [docs/frontmatter.md](../docs/frontmatter.md) for complete frontmatter schema, field definitions, and examples.

**Quick Reference** (required fields):

- `layout`: Always `"post"`
- `title`: Plain text title
- `author`: Author/presenter name
- `date`: ISO format with Europe/Brussels timezone
- `permalink`: `/section/collection/slug`
- `tags`: Topic keywords array
- `section_names`: Normalized section identifiers array

### Collection-Specific Fields

**GitHub Copilot Features** (`_videos/ghc-features/`):

```yaml
plans: ["Free", "Pro", "Business"]  # Required - supported subscription tiers
ghes_support: true                  # Required - GitHub Enterprise Server support
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

> **See also**: [docs/terminology.md](../docs/terminology.md) for complete content organization concepts and structure definitions.

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

## Related Documentation

### Functional Documentation (docs/)

- **[Frontmatter](../docs/frontmatter.md)** - Complete frontmatter schema with field mappings
- **[Content Processing](../docs/content-processing.md)** - RSS processing and content workflows
- **[Terminology](../docs/terminology.md)** - Section and collection definitions

### Implementation Guides (AGENTS.md)

- **[Root AGENTS.md](../AGENTS.md)** - AI workflow and development process
- **[scripts/AGENTS.md](../scripts/AGENTS.md)** - Content processing scripts
- **[docs/writing-style-guidelines.md](../docs/writing-style-guidelines.md)** - Writing tone and style standards
