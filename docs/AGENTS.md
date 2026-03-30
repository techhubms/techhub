# Documentation Management Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `docs/` directory. It complements the [Root AGENTS.md](../AGENTS.md).

## Documentation Hierarchy

| Tier | File | Purpose |
|------|------|---------|
| 1 | **[Root AGENTS.md](../AGENTS.md)** | AI workflow - 8-step development process |
| 2 | **[README.md](../README.md)** | Minimal human entry point - what/where to find things |
| 3 | **Domain AGENTS.md files** | Code patterns, framework guidance, domain rules |
| 4 | **docs/** (this directory) | ALL system documentation - architecture, features, specs |

**Precedence**: Docs -> Domain AGENTS.md -> Root AGENTS.md -> README.md

## Core Philosophy

**Describe WHAT and WHY, not HOW**: System behavior, features, capabilities, architecture decisions — not implementation code.

## Critical Rules

### ✅ Always Do

- **Always keep docs/ to the point** — Only functional documentation
- **Always describe patterns, not implementations** — Reference files, don't duplicate code
- **Always update docs when behavior changes**
- **Always use framework-agnostic language**
- **Always follow [writing-style-guidelines.md](writing-style-guidelines.md)**

### ⚠️ Ask First

- Adding new docs files, major restructuring of documentation hierarchy

### 🚫 Never Do

- **Never copy full implementations** — Describe patterns instead
- **Never document HOW TO CODE** — Belongs in AGENTS.md files
- **Never duplicate content** — Link instead
- **Never hardcode file lists** — Use `grep_search` or `list_dir`

## Content Placement Decision

| Content Type | Location |
|--------------|----------|
| AI development process, workflow steps | Root [AGENTS.md](../AGENTS.md) |
| Human-friendly project intro, quick start | [README.md](../README.md) |
| Code patterns, framework guidance | Domain AGENTS.md files |
| Functional docs, architecture, features, specs | docs/ |
| Writing standards | [writing-style-guidelines.md](writing-style-guidelines.md) |
| Content frontmatter | [collections/AGENTS.md](../collections/AGENTS.md) |

**docs/ File Categories**:

| Category | Files |
|----------|-------|
| **Architecture** | architecture.md, database.md, technology-stack.md |
| **Observability** | telemetry.md, google-analytics.md, query-logging.md |
| **Content** | content-api.md, content-processing.md, frontmatter.md, rss-feeds.md |
| **UI/UX** | design-system.md, page-structure.md, toc-component.md, render-modes.md |
| **Features** | filtering.md, custom-pages.md, health-checks.md |
| **Frontend** | javascript.md, caching.md, seo.md |
| **Testing** | testing-strategy.md, running-and-testing.md |
| **Reference** | terminology.md, repository-structure.md |

## Documentation Standards

**File Naming**: Lowercase with hyphens: `content-processing.md`

**File Structure**: Title → Brief description → Overview → Main sections by topic

**Quality**: Matches current behavior, framework-agnostic, WHAT/WHY not HOW, working links, follows writing-style-guidelines.md
