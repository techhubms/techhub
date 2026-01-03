# Documentation Management Guide

> **AI CONTEXT**: This is a **LEAF** context file for the `docs/` directory. It complements the [Root AGENTS.md](../AGENTS.md).
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Critical Documentation Rules

### ✅ Always Do

- **Keep docs/ MINIMAL** - Only functional documentation (WHAT the system does)
- **Update docs when behavior changes** - Not optional, part of "task complete"
- **Search before assuming docs don't exist** - Use `grep_search` to find relevant docs
- **Use framework-agnostic language** - Docs must survive tech stack changes
- **Cross-reference related files** - Maintain connections between documents
- **Test all code examples** before including them
- **Verify all links** work correctly
- **Follow writing-style-guidelines.md** for technical writing

### ⚠️ Ask First

- **Adding new functional documentation files** - Ensure they belong in docs/, not elsewhere
- **Major restructuring** of documentation hierarchy
- **Significant terminology changes** - May affect multiple files

### 🚫 Never Do

- **Never document HOW TO CODE** in docs/ files (belongs in AGENTS.md files)
- **Never include implementation code examples** in functional documentation
- **Never provide step-by-step development instructions** in docs/
- **Never duplicate content** from other files (link instead)
- **Never place domain-specific patterns** in root AGENTS.md (use domain-specific AGENTS.md)
- **Never hardcode file lists** or examples (reference dynamic sources)
- **Never mix API specification** with implementation patterns
- **Never describe framework-specific features** without behavioral context
- **Never use vague language** ("might", "probably", "should")
- **Never skip documentation updates** when code behavior changes

## Purpose

**GOAL**: Keep this folder MINIMAL - only **functional documentation files** that explain WHAT the system does (not HOW to implement it).

**Current Functional Documentation**:

1. **[`filtering-system.md`](filtering-system.md)** - How filtering works (tag and date filtering logic)
2. **[`content-management.md`](content-management.md)** - Content workflows and RSS processing
3. **[`api-specification.md`](api-specification.md)** - REST API contracts and endpoints

These files are framework-agnostic and describe system behavior, not implementation details.

**All other content is located elsewhere**:

- Development patterns → Domain-specific AGENTS.md files
- Framework commands/patterns → `.github/agents/dotnet.md`
- Writing/quality standards → `collections/markdown-guidelines.md`, `collections/writing-style-guidelines.md`
- Terminology/architecture → Root `AGENTS.md` (Site Terminology and Repository Structure sections)
- Technical implementation guides → Appropriate AGENTS.md files

**Rationale**: Only files that explain system functionality (not development, not writing, not architecture) remain in docs/.

## Tech Hub Documentation Structure

The Tech Hub uses a three-tier documentation system:

### 1. AGENTS.md Files (Domain-Specific Development Guidance)

Located in each major directory, AGENTS.md files provide focused guidance for AI assistants working in specific code domains:

- **`AGENTS.md`** (root): Generic development principles, performance rules, timezone handling, repository navigation
- **`scripts/AGENTS.md`**: PowerShell development and automation scripts
- **`src/AGENTS.md`**: .NET development patterns across all projects
- **`src/TechHub.Api/AGENTS.md`**: API development patterns (Minimal APIs, endpoints, OpenAPI)
- **`src/TechHub.Web/AGENTS.md`**: Blazor component patterns (SSR, interactivity, code-behind)
- **`src/TechHub.Core/AGENTS.md`**: Domain model design (records, DTOs, interfaces)
- **`src/TechHub.Infrastructure/AGENTS.md`**: Data access patterns (repositories, caching)
- **`tests/AGENTS.md`**: Testing strategies and frameworks (xUnit, bUnit, Playwright)
- **`infra/AGENTS.md`**: Infrastructure patterns (Bicep, Azure resources)
- **`docs/AGENTS.md`**: Documentation guidelines and maintenance (this file)
- **`collections/AGENTS.md`**: Content creation and management

**Purpose**: Domain-specific development patterns, critical rules, and code standards. These files maintain domain focus regardless of framework.

**Key Distinction**: Domain-specific (code patterns for a specific area) vs Framework-specific (patterns for the tech stack).

### 2. Framework Agent Files (Framework-Specific Development Guidance)

Located in `.github/agents/`, these custom agents provide framework-specific development guidance:

- **`dotnet.md`**: .NET/Blazor development, C# patterns, ASP.NET Core, Aspire orchestration

**Purpose**: Complete framework-specific how-to guides for working with the tech stack. These files are entirely framework-dependent.

**Usage**: Use `@dotnet` custom agent for .NET development.

### 3. Documentation Files (Framework-Agnostic Functional Documentation)

Located in `docs/`, these files explain how the system works, independent of implementation:

**Purpose**: Explain WHAT the system does and WHY, not HOW TO CODE it.

## Documentation Files (Current State)

### Functional Documentation (in docs/)

**Framework-Agnostic Functional Documentation**:

- **[`filtering-system.md`](filtering-system.md)** - Tag and date filtering logic and behavior
- **[`content-management.md`](content-management.md)** - Content workflows and RSS processing
- **[`api-specification.md`](api-specification.md)** - REST API contracts and endpoints

### Related Documentation (Other Locations)

**Development Guidance** (in AGENTS.md files):

- **[Root `AGENTS.md`](../AGENTS.md)** - Generic principles, architecture, terminology, performance, timezone handling
- **Domain-specific AGENTS.md** - See root `AGENTS.md` for complete list

**Framework-Specific** (in `.github/agents/`):

- **[`.github/agents/dotnet.md`](../.github/agents/dotnet.md)** - .NET/Blazor development, C# patterns, Aspire orchestration

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

- **Complete framework-specific guidance** for the tech stack
- Server management, build processes, templating, framework best practices, tech specific implementations of the generic development principles mentioned in root `AGENTS.md`
- **Current**: `dotnet.md` (C#, Blazor, ASP.NET Core, Aspire)

### 3. Domain-Specific AGENTS.md Files

- **Development patterns for specific code domains** (not frameworks)
- Examples: API patterns, Blazor patterns, PowerShell patterns
- See root `AGENTS.md` for complete list and when to read them

### 4. Functional Documentation (`docs/`)

**CRITICAL**: Keep MINIMAL - only files that describe system behavior, not implementation.

**Files that belong here**:

- **`filtering-system.md`** - How filtering works (logic, behavior, rules)
- **`content-management.md`** - How content is managed (workflows, RSS processing)
- **`api-specification.md`** - REST API contracts and endpoint specifications

**Everything else goes elsewhere**:

- Terminology/concepts → Root `AGENTS.md` (Site Terminology section)
- Architecture/overview → Root `AGENTS.md` or `docs/AGENTS.md`
- Framework patterns → `.github/agents/dotnet.md`
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

**Functional Documentation (docs/)**:

- **How tag filtering works** → `docs/filtering-system.md` (system functionality)
- **How date filtering works** → `docs/filtering-system.md` (system functionality)
- **Content workflow process** → `docs/content-management.md` (system functionality)
- **RSS feed processing** → `docs/content-management.md` (system functionality)
- **API endpoint contracts** → `docs/api-specification.md` (API specification)
- **REST API behavior** → `docs/api-specification.md` (API specification)

**Development Guidance (AGENTS.md files)**:

- **Site terminology** → Root `AGENTS.md` (already has Site Terminology section)
- **Architecture overview** → Root `AGENTS.md` (Repository Structure section)
- **Generic performance principles** → Root `AGENTS.md` (Performance Architecture section)
- **Timezone handling rules** → Root `AGENTS.md` (Timezone & Date Handling section)
- **Blazor component patterns** → `src/TechHub.Web/AGENTS.md` (domain-specific)
- **API endpoint patterns** → `src/TechHub.Api/AGENTS.md` (domain-specific)
- **.NET development patterns** → `.github/agents/dotnet.md` (framework-specific)
- **PowerShell script patterns** → `scripts/AGENTS.md` (domain-specific)
- **Markdown formatting** → `collections/markdown-guidelines.md` (content writing)
- **Writing style** → `collections/writing-style-guidelines.md` (content writing)

#### ❌ **Incorrect Placements**

- **Terminology/concepts** → `docs/` (belongs in root `AGENTS.md`)
- **Architecture overview** → `docs/` (belongs in root `AGENTS.md`)
- **.NET-specific patterns** → `docs/` or root `AGENTS.md` (belongs in `.github/agents/dotnet.md`)
- **Writing standards** → `docs/` (belongs in `collections/markdown-guidelines.md` or `collections/writing-style-guidelines.md`)
- **Code examples** → `docs/` (belongs in AGENTS.md files)
- **Development how-tos** → `docs/` (belongs in AGENTS.md files)
- **API implementation code** → `docs/` (belongs in `src/TechHub.Api/AGENTS.md`)
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

## Implementation Mentions in Functional Documentation

**When Implementation References Are Appropriate**:

Functional documentation files in `docs/` may reference specific implementations (API endpoints, service names, etc.) when the mention is **essential to understanding the system behavior**.

**Examples of Appropriate Implementation References**:

- ✅ "The GET /api/content/filter endpoint supports multi-criteria filtering" - Describes API behavior
- ✅ "Content items are sorted by dateEpoch in descending order" - Explains data ordering
- ✅ "Filtering applies AND logic for multiple tags" - Shows filtering behavior

**Examples of Inappropriate Implementation References**:

- ❌ ".NET uses dependency injection" - Generic framework information (belongs in dotnet.md)
- ❌ "To run the API, use dotnet run" - Generic command (belongs in dotnet.md)
- ❌ "Blazor component lifecycle is..." - Generic tutorial (belongs in TechHub.Web/AGENTS.md)

**Why This Approach**:

Functional documentation describes WHAT the system does (behavior, contracts, rules), while implementation documentation describes HOW to build it (code patterns, commands, frameworks).

**Rule of Thumb**: If the information helps users understand system behavior (filtering, APIs, data flows), it belongs in docs/. If it helps developers implement features (code patterns, commands, frameworks), it belongs in AGENTS.md or framework agents.

## Documentation Quality Standards

### Writing Standards

**For content files** (news, blogs, videos, community, roundups in `collections/`):

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
- Run tests after changes
- Fix linting errors
- Update documentation

### ⚠️ Ask First
- Breaking API changes
- Adding dependencies

### 🚫 Never Do
- Never commit with errors
- Never skip tests

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
