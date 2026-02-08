# Documentation Management Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `docs/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Global rules in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Documentation Hierarchy

| Tier | File | Purpose |
|------|------|---------|
| 1 | **[Root AGENTS.md](../AGENTS.md)** | AI workflow - 8-step development process |
| 2 | **[README.md](../README.md)** | Minimal human entry point - what/where to find things |
| 3 | **Domain AGENTS.md files** | Code patterns, framework guidance, domain rules |
| 4 | **docs/** (this directory) | ALL system documentation - architecture, features, specs |

**Precedence**: Docs (all detailed docs live here) -> Domain AGENTS.md -> Root AGENTS.md -> README.md (minimal)

**Finding docs**: See [documentation-index.md](documentation-index.md) for complete file listing with summaries.

## Core Philosophy

**Describe WHAT and WHY, not HOW**:

- ✅ **WHAT**: System behavior, features, capabilities, contracts
- ✅ **WHY**: Architecture decisions, design rationale, tradeoffs
- 🚫 **HOW**: Implementation code, step-by-step coding instructions

**Examples**:

- ✅ "Tag filtering uses word boundary matching to prevent false matches" (WHAT)
- ✅ "Quantile-based sizing ensures consistent distribution across tag counts" (WHY)
- 🚫 Full `TagMatchingService` class implementation with 40 lines of code (HOW)

## Critical Rules

### ✅ Always Do

- **Always keep docs/ to the point** - Only functional documentation
- **Always describe patterns, not implementations** - Reference files, don't duplicate code
- **Always update docs when behavior changes** - Part of "task complete"
- **Always use framework-agnostic language** - Docs must survive tech stack changes
- **Always follow [writing-style-guidelines.md](writing-style-guidelines.md)**

### ⚠️ Ask First

- **Ask first before adding new docs files** - Ensure they belong here
- **Ask first before major restructuring** of documentation hierarchy

### 🚫 Never Do

- **Never copy full implementations** - Describe patterns instead
- **Never document HOW TO CODE** - Belongs in AGENTS.md files
- **Never duplicate content** - Link instead
- **Never hardcode file lists** - Reference [documentation-index.md](documentation-index.md)

## Content Placement Decision

**Where does new content go?**

| Content Type | Location |
|--------------|----------|
| AI development process, workflow steps | Root [AGENTS.md](../AGENTS.md) |
| Human-friendly project intro, quick start | [README.md](../README.md) |
| Code patterns, framework guidance, implementation explanation | Domain AGENTS.md files |
| Any functional documentation, architecture, features, tech stack, terminology, specs | docs/ |
| Writing standards | [writing-style-guidelines.md](writing-style-guidelines.md) |
| Content frontmatter | [collections/AGENTS.md](../collections/AGENTS.md) |

**Rule of Thumb**:

- **README.md** is MINIMAL - only project intro and quick start pointers
- **docs/** contains ALL detailed documentation - architecture, features, tech stack, terminology, specs
- **AGENTS.md** files contain implementation guidance for AI agents and developers

**docs/ File Categories**:

| Category | Files |
|----------|-------|
| **Architecture** | [architecture.md](architecture.md), [database.md](database.md), [technology-stack.md](technology-stack.md) |
| **Content** | [content-api.md](content-api.md), [content-processing.md](content-processing.md), [frontmatter.md](frontmatter.md), [rss-feeds.md](rss-feeds.md) |
| **UI/UX** | [design-system.md](design-system.md), [page-structure.md](page-structure.md), [toc-component.md](toc-component.md), [render-modes.md](render-modes.md) |
| **Features** | [filtering.md](filtering.md), [custom-pages.md](custom-pages.md), [health-checks.md](health-checks.md) |
| **Frontend** | [javascript.md](javascript.md), [caching.md](caching.md), [seo.md](seo.md) |
| **Testing** | [testing-strategy.md](testing-strategy.md), [running-and-testing.md](running-and-testing.md) |
| **Reference** | [terminology.md](terminology.md), [repository-structure.md](repository-structure.md) |

**Cross-Referencing Pattern**: Each AGENTS.md should have a "Related Documentation" section that links to relevant docs/ files. Each docs/ file should have an "Implementation Reference" section linking to source files.

## Documentation Standards

**File Structure**:

```markdown
# Title

Brief description of what this document covers.

## Overview

High-level summary.

## Main Sections

Content organized by topic.
```

**Quality Checklist**:

- [ ] Matches current system behavior
- [ ] Framework-agnostic language
- [ ] Explains WHAT and WHY, not HOW
- [ ] All links work correctly
- [ ] Follows [writing-style-guidelines.md](writing-style-guidelines.md)

**File Naming**: Use lowercase with hyphens: `content-processing.md`

## AGENTS.md Best Practices

> Based on [analysis of 2,500+ repositories](https://github.blog/ai-and-ml/github-copilot/how-to-write-a-great-agents-md-lessons-from-over-2500-repositories/)

**Core Principles**:

1. **Three-Tier Boundaries** - Always/Ask/Never rules at the top
2. **Commands Before Concepts** - Actionable guidance first, theory later
3. **Code Examples Over Explanations** - Show ✅ correct and ❌ incorrect patterns

**Six Core Areas**:

1. Commands & Tools
2. Testing Requirements
3. Project Structure
4. Code Style
5. Git Workflow
6. Boundaries (Always/Ask/Never)
