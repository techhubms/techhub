# Collections Management Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `collections/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

**CRITICAL**: Before creating or editing ANY content in this directory, you MUST read these files:

1. **[writing-style-guidelines.md](writing-style-guidelines.md)** - Tone, voice, language standards, and writing quality requirements
2. **[markdown-guidelines.md](markdown-guidelines.md)** - Front matter structure, markdown formatting rules, and content organization

These documents contain essential guidance that applies to ALL content creation and editing work.

## When to Use This Guide

**Read this file when**:

- Creating new content items (news, videos, posts, etc.)
- Editing existing collection content
- Understanding frontmatter requirements
- Working with special collection types (alt-collection)
- Managing content structure and organization

**Related Documentation**:

- Writing style → [writing-style-guidelines.md](writing-style-guidelines.md)
- Markdown formatting → [markdown-guidelines.md](markdown-guidelines.md)
- Content workflows → [docs/content-management.md](../docs/content-management.md)
- RSS processing → [rss/AGENTS.md](../rss/AGENTS.md)

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

- Collections are defined in configuration files (`_config.yml` in Jekyll, similar in other frameworks)
- Each collection has its own processing rules and display logic
- Never hardcode collection names - they are configuration-driven

**Content Structure (All Files):**

1. **Front Matter**: YAML metadata (see [markdown-guidelines.md](markdown-guidelines.md) for required fields and ordering)
2. **Excerpt**: Brief introduction (max 200 words) ending with `<!--excerpt_end-->`
3. **Content**: Main detailed content in Markdown format

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
- Should be well-structured using proper markdown formatting (see [markdown-guidelines.md](markdown-guidelines.md))

## Important Implementation Notes

**Viewing Modes:**

- `"internal"`: Self-contained content displayed fully on site (videos, roundups)
- `"external"`: Content linking to original sources (news, posts, community)

**Layout Types:**

- `layout: "post"`: For collection files (`_news`, `_videos`, `_blogs`, etc.)
- `layout: "page"`: For section directory files (in `/ai`, `/github-copilot`, etc.)

**Page Field:**

- Automatically set based on directory: `_blogs` → `"blogs"`, `_news` → `"news"`, `_videos` → `"videos"`, etc.

**Automated Processing:**

- Automated scripts can fix common formatting issues (implementation varies by framework)
- Focus on following guidelines correctly rather than relying on automation
- Do NOT automatically run processing scripts
