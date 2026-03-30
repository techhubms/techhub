# Collections Management Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `collections/` directory. It complements the [Root AGENTS.md](../AGENTS.md).

## Critical Content Rules

### ✅ Always Do

- **Always read [docs/writing-style-guidelines.md](../docs/writing-style-guidelines.md) FIRST** before creating or editing ANY content
- **Always use filename format `YYYY-MM-DD-title-slug.md`**
- **Always match filename date with frontmatter date** for `layout: post` files
- **Always include excerpt section** — Max 200 words, end with `<!--excerpt_end-->`
- **Always mention author in excerpt**
- **Always place files in correct collection directory**

### ⚠️ Ask First

- Creating new collection types, modifying frontmatter schema, changing content processing

### 🚫 Never Do

- **Never create content without reading** [docs/writing-style-guidelines.md](../docs/writing-style-guidelines.md)
- **Never skip excerpt** or exceed 200 words or omit `<!--excerpt_end-->` marker
- **Never omit author from excerpt**
- **Never hardcode collection names** — Configuration-driven via `appsettings.json`
- **Never skip frontmatter** — All markdown files need YAML front matter
- **Never mismatch filename and frontmatter dates**
- **Never place files in wrong collection** — Use correct directory for content type
- **Never automatically run processing scripts** — Manual fixes preferred

## Collections Structure

- **_community**: Community-sourced content
- **_news**: Official product updates
- **_blogs**: Blogs
- **_roundups**: Curated weekly content summaries
- **_videos**: Educational video content
  - **_videos/ghc-features/**: GitHub Copilot features
  - **_videos/vscode-updates/**: VS Code updates

## Frontmatter

📖 **Full schema**: See [docs/frontmatter.md](../docs/frontmatter.md) for complete field definitions, mappings, and examples.

**Required fields**: `layout` (`"post"`), `title`, `author`, `date` (ISO with Europe/Brussels timezone), `permalink`, `tags`, `section_names`

### Collection-Specific Fields

**GitHub Copilot Features** (`_videos/ghc-features/`):

```yaml
plans: ["Free", "Pro", "Business"]  # Required - subscription tiers
ghes_support: true                  # Required - GitHub Enterprise Server
```

### Deprecated Fields (Do Not Use)

`categories`, `tags_normalized`, `excerpt_separator`, `description`, `page`, `video_id` — see [docs/frontmatter.md](../docs/frontmatter.md) for current schema.

## Excerpt Section

Must appear directly after frontmatter. Max 200 words. Must mention author's name. Must end with `<!--excerpt_end-->` marker.

See [docs/terminology.md](../docs/terminology.md) for content organization concepts.
