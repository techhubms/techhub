# Documentation Management Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `docs/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Global rules in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Documentation Hierarchy

The Tech Hub uses a **4-tier documentation hierarchy** with clear purposes:

| File | Purpose | Contains |
|------|---------|----------|
| **[AGENTS.md](../AGENTS.md)** | AI workflow | 10-step development process with checklists |
| **[README.md](../README.md)** | General info | Project overview, tech stack, terminology, running/testing |
| **Domain AGENTS.md files** | Technical details | Code patterns, framework guidance, domain-specific rules |
| **[docs/](.)** (this directory) | Functional docs | WHAT the system does - API specs, features, behavior |

**Precedence**: Domain AGENTS.md (deepest) → Root AGENTS.md → README.md (shallowest)

## Critical Documentation Rules

### 🎯 Core Documentation Philosophy

**Describe WHAT and WHY, not HOW**:

- ✅ **WHAT**: System behavior, features, capabilities, contracts
- ✅ **WHY**: Architecture decisions, design rationale, tradeoffs
- 🚫 **HOW**: Implementation code, step-by-step coding instructions, full class definitions

**Examples**:

- ✅ "Tag filtering uses word boundary matching to prevent false matches" (WHAT)
- ✅ "Quantile-based sizing ensures consistent distribution across tag counts" (WHY)
- 🚫 Full `TagMatchingService` class implementation with 40 lines of code (HOW)

**Key Principle**: Documentation should guide developers to understand the system, not replace reading the source code.

### ✅ Always Do

- **Always keep docs/ MINIMAL** - Only functional documentation (WHAT the system does)
- **Always describe patterns, not implementations** - Name the pattern, explain key decisions, reference actual code
- **Always update docs when behavior changes** - Not optional, part of "task complete"
- **Always search before assuming docs don't exist** - Use `grep_search` to find relevant docs
- **Always use framework-agnostic language** - Docs must survive tech stack changes
- **Always cross-reference related files** - Maintain connections between documents
- **Always show structure, not full code** - If code is needed, show signatures/patterns only
- **Always verify all links** work correctly
- **Always follow writing-style-guidelines.md** for technical writing

### ⚠️ Ask First

- **Ask first before adding new functional documentation files** - Ensure they belong in docs/, not elsewhere
- **Ask first before major restructuring** of documentation hierarchy
- **Ask first before significant terminology changes** - May affect multiple files

### 🚫 Never Do

- **Never copy full implementations** into documentation (describe patterns instead)
- **Never document HOW TO CODE** in docs/ files (belongs in AGENTS.md files)
- **Never include complete class/method implementations** (show structure, reference source)
- **Never provide step-by-step development instructions** in docs/
- **Never duplicate content** from other files (link instead)
- **Never place domain-specific patterns** in root AGENTS.md (use domain-specific AGENTS.md)
- **Never hardcode file lists** or examples (reference dynamic sources)
- **Never mix API specification** with implementation patterns
- **Never describe framework-specific features** without behavioral context
- **Never use vague language** ("might", "probably", "should")
- **Never skip documentation updates** when code behavior changes

## Tech Hub Documentation Structure (4-Tier Hierarchy)

The Tech Hub uses a **4-tier documentation hierarchy** with clear purposes:

**Precedence Order**: Domain AGENTS.md (deepest) → Root AGENTS.md → README.md (shallowest)

### Tier 1: Root [AGENTS.md](../AGENTS.md) - AI Workflow

The required development process for AI coding agents.

**Contains**:

- 10-step AI Assistant Workflow (REQUIRED process)
- Core rules and boundaries (Always/Ask/Never)
- Step-by-step checklists for each development phase

**Purpose**: Process and methodology for ALL development tasks.

### Tier 2: [README.md](../README.md) - General Project Info

Project overview, tech stack, and getting started.

**Contains**:

- Quick start instructions
- Tech stack and architecture
- Site terminology and concepts
- Running and testing workflows
- Documentation navigation

**Purpose**: Understanding the project and getting started.

### Tier 3: Domain-Specific AGENTS.md - Technical Details

Development patterns for specific code domains. **Read FIRST** when working in a domain.

**Examples**:

- [src/AGENTS.md](../src/AGENTS.md) - .NET development patterns
- [src/TechHub.Api/AGENTS.md](../src/TechHub.Api/AGENTS.md) - API development patterns
- [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [tests/AGENTS.md](../tests/AGENTS.md) - Testing strategies
- [scripts/AGENTS.md](../scripts/AGENTS.md) - PowerShell automation

**Purpose**: Domain-specific rules, patterns, and code examples. These provide implementation details for specific areas.

### Tier 4: Functional Documentation (docs/) - System Behavior

**This directory**. Framework-agnostic descriptions of WHAT the system does.

**Files**:

- [content-management.md](content-management.md) - Content workflows and RSS processing
- [api-specification.md](api-specification.md) - REST API contracts
- [rss-feeds.md](rss-feeds.md) - RSS feed system and available feeds
- [toc-component.md](toc-component.md) - Table of contents component architecture

**Purpose**: Describes system behavior, not implementation details.

### Supporting: Content Guidelines (collections/)

- [AGENTS.md](../collections/AGENTS.md#frontmatter-schema) - Content management with frontmatter schema
- [writing-style-guidelines.md](../collections/writing-style-guidelines.md) - Writing standards

**Purpose**: Framework-agnostic descriptions of system behavior and content standards.

## Purpose

**GOAL**: Keep this folder MINIMAL - only **functional documentation files** that explain WHAT the system does (not HOW to implement it).

**Current Functional Documentation**:

1. **[`content-management.md`](content-management.md)** - Content workflows and RSS processing
2. **[`api-specification.md`](api-specification.md)** - REST API contracts and endpoints
3. **[`rss-feeds.md`](rss-feeds.md)** - RSS feed system
4. **[`toc-component.md`](toc-component.md)** - Table of contents architecture

These files are framework-agnostic and describe system behavior, not implementation details.

**All other content is located elsewhere**:

- AI development workflow → Root [AGENTS.md](../AGENTS.md)
- Project overview, tech stack, terminology → [README.md](../README.md)
- Development patterns → Domain-specific AGENTS.md files
- Writing/quality standards → `collections/writing-style-guidelines.md`

**Rationale**: Only files that explain system functionality (not development workflow, not project overview, not implementation) remain in docs/.

## Documentation Files (Current State)

### Functional Documentation (in docs/)

**Framework-Agnostic Functional Documentation**:

- **[`content-management.md`](content-management.md)** - Content workflows and RSS processing
- **[`api-specification.md`](api-specification.md)** - REST API contracts and endpoints
- **[`rss-feeds.md`](rss-feeds.md)** - RSS feed system
- **[`toc-component.md`](toc-component.md)** - Table of contents architecture

### Related Documentation (Other Locations)

**AI Workflow** (in root AGENTS.md):

- **[Root `AGENTS.md`](../AGENTS.md)** - 10-step AI development workflow with checklists

**General Project Info** (in README.md):

- **[`README.md`](../README.md)** - Project overview, tech stack, terminology, running/testing

**Development Guidance** (in domain AGENTS.md files):

- **[`src/AGENTS.md`](../src/AGENTS.md)** - .NET development patterns
- **Domain-specific AGENTS.md** - See root `AGENTS.md` for complete list

**Content Writing** (in `collections/`):

- **[`collections/AGENTS.md`](../collections/AGENTS.md)** - Content management overview and frontmatter schema
- **[`collections/writing-style-guidelines.md`](../collections/writing-style-guidelines.md)** - Writing tone and style standards

## Content Placement Hierarchy

When adding documentation, place it in the most specific appropriate location:

### 1. Root AGENTS.md

- AI development workflow (10 steps)
- Core rules and boundaries (Always/Ask/Never)
- Process guidance for all tasks
- **Not for**: Project overview, tech stack, implementation patterns

### 2. README.md

- Project overview and quick start
- Tech stack and architecture
- Site terminology and concepts
- Running and testing workflows
- Documentation navigation
- **Not for**: AI workflow, implementation patterns

### 3. Domain-Specific AGENTS.md Files

- **Development patterns for specific code domains** (API, Web, Core, Infrastructure, scripts, tests)
- Examples: API endpoint patterns, Blazor component patterns, PowerShell script patterns
- See root `AGENTS.md` for complete list and when to read them

### 4. Functional Documentation (`docs/`)

**CRITICAL**: Keep MINIMAL - only files that describe system behavior, not implementation.

**Files that belong here**:

- **`content-management.md`** - How content is managed (workflows, RSS processing)
- **`api-specification.md`** - REST API contracts and endpoint specifications
- **`rss-feeds.md`** - RSS feed system and available feeds
- **`toc-component.md`** - Table of contents architecture

**Everything else goes elsewhere**:

- AI workflow → Root `AGENTS.md`
- Project overview, terminology → `README.md`
- .NET/Blazor patterns → Domain-specific AGENTS.md files (src/, tests/, scripts/)
- Writing standards → `collections/AGENTS.md`

## Content Placement Strategy

### Before Adding New Content

1. **Check existing files in hierarchy order** - Don't duplicate existing information
2. **Choose the most specific applicable file** - More specific files take precedence
3. **Reference related files** - Use cross-references to maintain connections
4. **Avoid fragmentation** - Keep related concepts together

### Examples of Proper Placement

#### ✅ **Correct Placements**

**Functional Documentation (docs/)**:

- **Content workflow process** → `docs/content-management.md` (system functionality)
- **RSS feed processing** → `docs/content-management.md` (system functionality)
- **API endpoint contracts** → `docs/api-specification.md` (API specification)
- **REST API behavior** → `docs/api-specification.md` (API specification)

**General Info (README.md)**:

- **Site terminology** → `README.md` (Site Terminology section)
- **Tech stack overview** → `README.md` (Tech Stack section)
- **Architecture overview** → `README.md` (Repository Organization section)
- **Running/testing workflows** → `README.md` (Starting, Stopping and Testing section)
- **Project quick start** → `README.md` (Quick Start section)

**AI Workflow (Root AGENTS.md)**:

- **10-step development process** → Root `AGENTS.md`
- **Core rules (Always/Ask/Never)** → Root `AGENTS.md`
- **Step checklists** → Root `AGENTS.md`

**Development Guidance (Domain AGENTS.md files)**:

- **Blazor component patterns** → `src/TechHub.Web/AGENTS.md` (domain-specific)
- **API endpoint patterns** → `src/TechHub.Api/AGENTS.md` (domain-specific)
- **PowerShell script patterns** → `scripts/AGENTS.md` (domain-specific)
- **Testing patterns** → `tests/AGENTS.md` and test subdirectory AGENTS.md files
- **Writing style** → `collections/writing-style-guidelines.md` (content writing)

#### ❌ **Incorrect Placements**

- **Terminology/concepts** → `docs/` (belongs in `README.md`)
- **Architecture overview** → `docs/` (belongs in `README.md`)
- **Tech stack details** → `docs/` (belongs in `README.md`)
- **AI workflow steps** → `docs/` or `README.md` (belongs in root `AGENTS.md`)
- **Writing standards** → `docs/` (belongs in `collections/writing-style-guidelines.md`)
- **Code examples** → `docs/` (belongs in domain AGENTS.md files)
- **Development how-tos** → `docs/` (belongs in domain AGENTS.md files)
- **API implementation code** → `docs/` (belongs in `src/TechHub.Api/AGENTS.md`)
- **Domain-specific patterns** → Root AGENTS.md (belongs in domain AGENTS.md files)

## When to Update Documentation Types

**Update Root AGENTS.md** when:

- AI workflow process changes
- Core rules (Always/Ask/Never) change
- New development steps needed

**Update README.md** when:

- Project overview changes
- Tech stack changes
- Site terminology changes
- Running/testing workflows change
- Documentation hierarchy changes

**Update Documentation** (`docs/*.md`) when:

- Feature specifications change
- System architecture evolves
- New functional components added
- API contracts change

**Update Domain AGENTS.md files** when:

- Development patterns change
- Critical rules added for a domain
- Common errors identified
- Domain-specific guidance needed

## Documentation Maintenance

**CRITICAL**: Keep documentation synchronized with system behavior.

### When to Update

1. **Functional changes** - How the system works
2. **Architecture changes** - System structure
3. **New features** - Additional capabilities
4. **Concept changes** - Terminology or definitions

### Update Checklist

- [ ] Accuracy: Matches current system behavior
- [ ] Framework-agnostic: Applies regardless of tech stack (for core docs)
- [ ] Clear concepts: Explains WHAT and WHY, not just HOW TO CODE
- [ ] Cross-references: Links still valid
- [ ] Terminology: Consistent with terminology.md

## Documentation Standards

Each functional documentation file should explain:

- **What**: What the system does
- **Why**: Why it works this way  
- **How**: High-level workflow (framework-agnostic)

## Implementation Mentions in Functional Documentation

**When Implementation References Are Appropriate**:

Functional documentation files in `docs/` may reference specific implementations (API endpoints, service names, etc.) when the mention is **essential to understanding the system behavior**.

**Examples of Appropriate Implementation References**:

- ✅ "The GET /api/content/filter endpoint supports multi-criteria filtering" - Describes API behavior
- ✅ "Content items are sorted by dateEpoch in descending order" - Explains data ordering
- ✅ "Filtering applies AND logic for multiple tags" - Shows filtering behavior

**Examples of Inappropriate Implementation References**:

- ❌ ".NET uses dependency injection" - Generic framework information (belongs in Root AGENTS.md)
- ❌ "To run the API, use dotnet run" - Generic command (belongs in Root AGENTS.md)
- ❌ "Blazor component lifecycle is..." - Generic tutorial (belongs in TechHub.Web/AGENTS.md)

**Why This Approach**:

Functional documentation describes WHAT the system does (behavior, contracts, rules), while implementation documentation describes HOW to build it (code patterns, commands, frameworks).

**Rule of Thumb**: If the information helps users understand system behavior (filtering, APIs, data flows), it belongs in docs/. If it helps developers implement features (code patterns, commands, frameworks), it belongs in AGENTS.md or framework agents.

## Documentation Quality Standards

### Writing Standards

**For content files** (news, blogs, videos, community, roundups in `collections/`):

- Follow [../collections/writing-style-guidelines.md](../collections/writing-style-guidelines.md) for tone, voice, and language standards
- Follow [../collections/AGENTS.md](../collections/AGENTS.md#frontmatter-schema) for frontmatter structure
- Use `npx markdownlint-cli2 --fix <file-path> --config /workspaces/techhub/.markdownlint-cli2.jsonc` to fix markdown formatting issues

**For documentation files** (in `docs/`):

- Follow [../collections/writing-style-guidelines.md](../collections/writing-style-guidelines.md) for technical documentation writing
- Use consistent terminology from root [AGENTS.md](../AGENTS.md) (Site Terminology section)
- Prefer active voice and concise explanations
- Test all code examples before including them

### Technical Accuracy

- **Test all code examples** before including them
- **Verify all links** work correctly
- **Update screenshots** when UI changes
- **Maintain consistency** with actual implementation

### Accessibility

- **Descriptive link text** rather than "click here"
- **Alt text for images** when they convey important information
- **Logical heading structure** for screen readers
- **Clear table headers** when using tables

### Content Organization

**Standard File Structure**:

```markdown
# Title

Brief description of what this document covers.

## Overview

High-level summary of the topic.

## Main Sections

### Subsection 1

Content organized logically.
```

**Heading Guidelines**:

- Use **descriptive headings** that clearly indicate content
- Follow **logical hierarchy**: H1 → H2 → H3 → H4
- Avoid **duplicate headings** within the same file
- Use **consistent formatting**: Title case for main headings

### Cross-Referencing

**Linking Standards**:

- **Always reference more detailed files** from higher-level files
- **Use standard markdown link syntax**: `[link text](path/to/file.md)`
- **Provide context** when referencing: "See [file](path.md) for implementation details"
- **Avoid circular references** - maintain clear hierarchy flow

**Good Reference Patterns**:

```markdown
# ✅ Good: Contextual reference

For specific implementation details, see [Development Guide](dev-guide.md).

# ❌ Bad: Vague reference

See other docs for more info.
```

### File Naming

- Use **descriptive names**: `content-management.md` not `content.md`
- Use **specific suffixes** when appropriate: `-guidelines.md`, `-overview.md`
- Use **lowercase with hyphens**: `my-document.md` not `My_Document.md`

## Documentation Index Maintenance

### Adding New Documentation Files

1. **Add new files** to the appropriate section in this file with brief descriptions
2. **Update file purposes** to reflect current content
3. **Maintain recommended reading order** for new users
4. **Cross-reference** related files appropriately

### Regular Reviews

- **Quarterly reviews** of all documentation for accuracy
- **Update cross-references** when files are added or restructured
- **Remove outdated content** that no longer applies
- **Consolidate** fragmented information when appropriate

## AGENTS.md Best Practices

> Based on analysis of over 2,500 repositories using AGENTS.md files ([source](https://github.blog/ai-and-ml/github-copilot/how-to-write-a-great-agents-md-lessons-from-over-2500-repositories/))

### Core Principles

**1. Three-Tier Boundaries (Always/Ask/Never)**:

- ✅ **Always Do**: Actions AI should take automatically
- ⚠️ **Ask First**: Decisions requiring human approval
- 🚫 **Never Do**: Forbidden actions that create problems

**Rationale**: Clear boundaries help AI make correct decisions autonomously while avoiding common pitfalls.

**2. Commands Before Concepts**:

- Put actionable commands EARLY in the file (top third)
- Critical rules at the top, immediately after intro
- Detailed explanations and theory come later

**Rationale**: AI assistants need actionable guidance first, context second.

**3. Code Examples Over Explanations**:

- Show correct patterns with real code snippets
- Include both ✅ correct and ❌ incorrect examples
- Demonstrate full workflows, not isolated fragments

**Rationale**: Examples are more actionable than prose descriptions.

### Six Core Areas for AGENTS.md Files

Effective AGENTS.md files typically cover these areas:

1. **Commands & Tools**: How to build, test, run, deploy
2. **Testing Requirements**: When and how to test, coverage expectations
3. **Project Structure**: Directory layout, file organization
4. **Code Style**: Formatting, naming conventions, patterns to follow
5. **Git Workflow**: Branching, commits, PR requirements
6. **Boundaries**: Always/Ask/Never rules for decision-making

### Implementation in Tech Hub

Tech Hub implements these best practices through:

- **Three-tier boundaries**: All AGENTS.md files have ✅ Always/⚠️ Ask/🚫 Never sections at top
- **Commands early**: Development commands appear in first third of domain-specific AGENTS.md files
- **Code examples**: Pattern sections show both correct (✅) and incorrect (❌) approaches
- **Six core areas**: Root AGENTS.md covers all six areas; domain files focus on relevant subset

**Example Pattern**:

````markdown
# Domain Agent Guide

## Critical Rules

### ✅ Always Do

- **Always run tests after changes**
- **Always fix linting errors**
- **Always update documentation**

### ⚠️ Ask First

- **Ask first before breaking API changes**
- **Ask first before adding dependencies**

### 🚫 Never Do

- **Never commit with errors**
- **Never skip tests**

## Commands
```bash

# Build

npm run build

# Test

npm test

```
## Patterns

✅ **Correct**:
```code

// Good example

```
❌ **Wrong**:
```code

// Bad example

```
````

### Maintaining Quality

- **Review quarterly**: Check if rules still apply
- **Update with errors**: When common mistakes occur, add to "Never Do"
- **Evolve boundaries**: Refine Always/Ask/Never based on experience
- **Keep examples current**: Update code snippets when patterns change

### Version Control

- **Commit documentation changes** with descriptive messages
- **Review documentation updates** as part of code review process
- **Track documentation debt** in issues when updates are needed
