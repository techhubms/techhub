# Documentation Management Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `docs/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Purpose

**GOAL**: Keep this folder MINIMAL - only **2 core functional documentation files**:

1. **[`filtering-system.md`](filtering-system.md)** - How filtering works (tag and date filtering logic)
2. **[`content-management.md`](content-management.md)** - Content workflows and RSS processing

These are the ONLY truly framework-agnostic functional files that explain WHAT the system does, not HOW to build it.

**All other content is located elsewhere**:

- Development patterns → Domain-specific AGENTS.md files
- Framework commands/patterns → `.github/agents/` (fullstack.md for current framework)
- Writing/quality standards → `collections/AGENTS.md`, `collections/markdown-guidelines.md`, `collections/writing-style-guidelines.md`
- Terminology/architecture → Root `AGENTS.md` (Site Terminology and Repository Structure sections)
- Technical how-tos → Appropriate AGENTS.md files

## Migration Context

**Current**: Jekyll static site generator (Ruby, Liquid templates)
**Future**: C# / .NET webapp with Blazor

**docs/ Folder Current State**:

This folder contains ONLY the 2 framework-agnostic functional documentation files:

✅ **`filtering-system.md`** - Tag and date filtering logic and behavior
✅ **`content-management.md`** - Content workflows and RSS processing

**Other documentation has been distributed to appropriate locations**:

- **Terminology** → Root `AGENTS.md` (Site Terminology section)
- **Architecture/Overview** → Root `AGENTS.md` (Repository Structure section)
- **Full-stack development** → `.github/agents/fullstack.md` (framework-specific)
- **Date/timezone handling** → Root `AGENTS.md` (Timezone & Date Handling section) + `.github/agents/fullstack.md` (implementation)
- **Performance guidelines** → Root `AGENTS.md` (Performance Architecture section) + `.github/agents/fullstack.md` (implementation)
- **Markdown formatting** → `collections/markdown-guidelines.md` (content writing)
- **Writing style** → `collections/writing-style-guidelines.md` (content writing)

**Rationale**: Only files that explain system functionality (not development, not writing, not architecture) remain in docs/.

## Tech Hub Documentation Structure

The Tech Hub uses a two-tier documentation system:

### 1. AGENTS.md Files (Domain-Specific Development Guidance)

Located in each major directory, AGENTS.md files provide focused guidance for AI assistants working in specific code domains:

- **`AGENTS.md`** (root): Generic development principles, performance rules, timezone handling, repository navigation
- **`scripts/AGENTS.md`**: PowerShell development and automation scripts
- **`_plugins/AGENTS.md`**: Build plugins and extensions (currently Ruby for Jekyll; will change to C# for .NET)
- **`assets/js/AGENTS.md`**: JavaScript client-side development
- **`_sass/AGENTS.md`**: Styling and CSS architecture (currently SCSS; may change to CSS-in-JS for Blazor)
- **`rss/AGENTS.md`**: RSS feed generation and management
- **`docs/AGENTS.md`**: Documentation guidelines and maintenance (this file)
- **`spec/AGENTS.md`**: Testing strategies and frameworks
- **`collections/AGENTS.md`**: Content creation and management

**Purpose**: Domain-specific development patterns, critical rules, and code standards. These files may need updates during framework migration but maintain domain focus.

**Key Distinction**: Domain-specific (code patterns for a specific area) vs Framework-specific (patterns for Jekyll/Ruby or .NET/C#).

### 2. Framework Agent Files (Framework-Specific Development Guidance)

Located in `.github/agents/`, these custom agents provide framework-specific development guidance:

- **`fullstack.md`** (current): Jekyll static site generation, Liquid templating, Ruby plugins, JavaScript enhancements, PowerShell automation, comprehensive testing
- **`dotnet.md`** (future): .NET/Blazor development, C# patterns, ASP.NET Core, server management

**Purpose**: Complete framework-specific how-to guides for working with the current tech stack. These files are entirely framework-dependent and will be replaced during migration.

**Usage**: Use `@fullstack` custom agent for full-stack development, `@dotnet` for .NET development (future).

### 3. Documentation Files (Framework-Agnostic Functional Documentation)

Located in `docs/`, these files explain how the system works, independent of implementation:

**Purpose**: Explain WHAT the system does and WHY, not HOW TO CODE it.

## Documentation Files (Current State)

### Functional Documentation (in docs/)

**Framework-Agnostic Functional Documentation**:

- **[`filtering-system.md`](filtering-system.md)** - Tag and date filtering logic and behavior
- **[`content-management.md`](content-management.md)** - Content workflows and RSS processing

### Related Documentation (Other Locations)

**Development Guidance** (in AGENTS.md files):

- **[Root `AGENTS.md`](../AGENTS.md)** - Generic principles, architecture, terminology, performance, timezone handling
- **Domain-specific AGENTS.md** - See root `AGENTS.md` for complete list

**Framework-Specific** (in `.github/agents/`):

- **[`.github/agents/fullstack.md`](../.github/agents/fullstack.md)** - Jekyll development, Liquid templating, Ruby plugins, JavaScript, PowerShell, testing

**Content Writing** (in `collections/`):

- **[`collections/AGENTS.md`](../collections/AGENTS.md)** - Content management overview and requirements
- **[`collections/markdown-guidelines.md`](../collections/markdown-guidelines.md)** - Markdown formatting standards
- **[`collections/writing-style-guidelines.md`](../collections/writing-style-guidelines.md)** - Writing tone and style standards

## Content Placement Hierarchy

When adding documentation, place it in the most specific appropriate location:

### 1. Root AGENTS.md

- High-level project overview and navigation
- Generic, framework-agnostic development principles
- Repository structure and organization
- **Not for**: Specific implementation details

### 2. Framework Agent Files (`.github/agents/`)

- **Complete framework-specific guidance** for the current tech stack
- Server management, build processes, templating, framework best practices, tech specific implementations of the generic development principles mentioned in root `AGENTS.md`
- **Current**: `fullstack.md` (Jekyll, Ruby, Liquid, JavaScript, PowerShell, Testing)
- **Future**: `dotnet.md` (C#, Blazor, ASP.NET Core)
- **Migration impact**: Complete replacement - no content carries over

### 3. Domain-Specific AGENTS.md Files

- **Development patterns for specific code domains** (not frameworks)
- Examples: JavaScript patterns, PowerShell patterns, styling patterns
- See root `AGENTS.md` for complete list and when to read them
- **Migration impact**: May need updates for new framework but maintain domain focus
  - `_plugins/AGENTS.md`: Ruby → C# (same purpose: build plugins)
  - `assets/js/AGENTS.md`: JavaScript patterns (likely unchanged)
  - `_sass/AGENTS.md`: SCSS → CSS-in-JS or Blazor styling (same purpose: styling)

### 4. Functional Documentation (`docs/`)

**CRITICAL**: Keep MINIMAL - only 2 files for pure functional documentation.

**ONLY these files belong here**:

- **`filtering-system.md`** - How filtering works (logic, behavior, rules)
- **`content-management.md`** - How content is managed (workflows, RSS processing)

**Everything else goes elsewhere**:

- Terminology/concepts → Root `AGENTS.md` (Site Terminology section)
- Architecture/overview → Root `AGENTS.md` or `docs/AGENTS.md`
- Framework patterns → `.github/agents/fullstack.md` or `.github/agents/dotnet.md`
- Development guidance → Domain-specific AGENTS.md files
- Writing standards → `collections/AGENTS.md`
- Quality standards → `docs/AGENTS.md` (this file)

## Content Placement Strategy

### Before Adding New Content

1. **Check existing files in hierarchy order** - Don't duplicate existing information
2. **Choose the most specific applicable file** - More specific files take precedence
3. **Reference related files** - Use cross-references to maintain connections
4. **Avoid fragmentation** - Keep related concepts together

### Examples of Proper Placement

#### ✅ **Correct Placements**

**Functional Documentation (docs/ - ONLY 2 files)**:

- **How tag filtering works** → `docs/filtering-system.md` (system functionality)
- **How date filtering works** → `docs/filtering-system.md` (system functionality)
- **Content workflow process** → `docs/content-management.md` (system functionality)
- **RSS feed processing** → `docs/content-management.md` (system functionality)

**Development Guidance (AGENTS.md files)**:

- **Site terminology** → Root `AGENTS.md` (already has Site Terminology section)
- **Architecture overview** → Root `AGENTS.md` (Repository Structure section)
- **Generic performance principles** → Root `AGENTS.md` (Performance Architecture section)
- **Timezone handling rules** → Root `AGENTS.md` (Timezone & Date Handling section)
- **Jekyll Liquid patterns** → `.github/agents/fullstack.md` (framework-specific)
- **Blazor component patterns** → `.github/agents/dotnet.md` (future, framework-specific)
- **JavaScript code patterns** → `assets/js/AGENTS.md` (domain-specific)
- **PowerShell script patterns** → `scripts/AGENTS.md` (domain-specific)
- **Markdown formatting** → `collections/AGENTS.md` (content writing)
- **Writing style** → `collections/AGENTS.md` (content writing)

#### ❌ **Incorrect Placements**

- **Terminology/concepts** → `docs/` (belongs in root `AGENTS.md`)
- **Architecture overview** → `docs/` (belongs in root `AGENTS.md`)
- **Jekyll-specific patterns** → `docs/` or root `AGENTS.md` (belongs in `.github/agents/fullstack.md`)
- **Writing standards** → `docs/` (belongs in `collections/markdown-guidelines.md` or `collections/writing-style-guidelines.md`)
- **Code examples** → `docs/` (belongs in AGENTS.md files)
- **Development how-tos** → `docs/` (belongs in AGENTS.md files)
- **Build plugin code** → Root `AGENTS.md` (belongs in `_plugins/AGENTS.md`)
- **Domain-specific patterns** → Framework agents (domain ≠ framework)
- **Content formatting** → `docs/` (belongs in `collections/markdown-guidelines.md`)

## When to Update Documentation Types

**Update Documentation** (`docs/*.md`) when:

- Feature specifications change
- System architecture evolves
- New functional components added
- Conceptual explanations needed

**Update AGENTS.md files** when:

- Development patterns change
- Critical rules added for a domain
- Common errors identified
- Domain-specific guidance needed

**Update Framework Agents** (`.github/agents/`) when:

- Framework-specific commands change
- Tech stack best practices evolve
- Framework versions update

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

## Documentation Quality Standards

### Writing Standards

**For content files** (news, posts, videos, community, roundups in `collections/`):

- Follow [../collections/writing-style-guidelines.md](../collections/writing-style-guidelines.md) for tone, voice, and language standards
- Follow [../collections/markdown-guidelines.md](../collections/markdown-guidelines.md) for markdown formatting and structure

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

### Version Control

- **Commit documentation changes** with descriptive messages
- **Review documentation updates** as part of code review process
- **Track documentation debt** in issues when updates are needed

## Never Do

- Mix framework-specific implementation details with functional documentation
- Document HOW TO CODE in docs/ files (belongs in AGENTS.md files)
- Include framework-specific code examples in core documentation
- Assume a specific tech stack in framework-agnostic docs
- Use vague language ("might", "probably", "should")
- Duplicate content from other files (link instead)
- Place domain-specific patterns in root AGENTS.md (use domain-specific AGENTS.md)
- Hardcode file lists or examples (reference dynamic sources)
