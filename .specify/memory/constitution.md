<!--
SYNC IMPACT REPORT - Constitution Amendment (2026-02-08)
=========================================================

VERSION CHANGE: 2.1.0 → 3.0.0 (MAJOR bump)

REASON FOR MAJOR BUMP:
- Complete overhaul to align with current project reality
- Root AGENTS.md completely rewritten with 8-step workflow
- Extensive documentation now exists in docs/ (25+ files)
- Documentation placement strategy fully defined
- Simplified and modernized governance structure
- Breaking changes to how constitution references other files

CHANGES:
- REWRITTEN: All 7 rules simplified and aligned with current reality
- UPDATED: All references to point to actual current documentation
- UPDATED: Examples now reference real docs (page-structure.md, filtering.md, etc.)
- UPDATED: Core principles section streamlined
- REMOVED: Outdated references to non-existent documentation
- REMOVED: Verbose explanations now covered by comprehensive docs/
- VERSION: Bumped to 3.0.0

DOCUMENTATION STATUS:
✅ docs/: 25+ comprehensive documentation files covering all aspects
✅ Root AGENTS.md: Complete 8-step workflow with checklists
✅ Domain AGENTS.md files: Comprehensive implementation guides
✅ docs/AGENTS.md: Complete documentation placement strategy
✅ docs/documentation-index.md: Auto-generated searchable index
✅ docs/running-and-testing.md: Complete testing and running guide
✅ All specs updated to reference correct documentation

FOLLOW-UP ACTIONS:
✅ Constitution simplified - delegates to comprehensive docs
✅ Version bumped to 3.0.0 (MAJOR - breaking reference changes)
✅ Last Amended date updated to 2026-02-08
✅ Verified all file references are accurate
✅ Removed redundant content now in docs/

LAST REVIEW: 2026-02-08
NEXT AMENDMENT: When new principles emerge or fundamental approach changes
-->

# Tech Hub Constitution

> **Purpose**: Modern .NET/Blazor content platform for Microsoft technical content

## Project Identity

- **Name**: Tech Hub
- **Repository**: techhubms/techhub
- **Branch**: dotnet-migration (production deployment in progress)
- **Status**: Implementing production-quality website using the latest modern technologies

## ABSOLUTELY CRITICAL Rules

**These are the NON-NEGOTIABLE foundation rules that ALL development MUST follow:**

### 1. Follow the 8-Step Development Workflow

**🚨 MANDATORY**: EVERY development task MUST follow the exact 8-step workflow defined in the root AGENTS.md.

1. **Review codebase** - Understand existing code and create initial plan
2. **Gather context** - Read docs, validate plan with framework documentation (context7 MCP)
3. **Verify current behavior** - Use Playwright MCP to test (optional)
4. **Write tests first (TDD)** - Tests before implementation, always
5. **Implement changes** - Make tests pass with minimal code
6. **Validate quality** - All tests pass, no errors/warnings
7. **Update documentation** - Keep docs synchronized with code
8. **Report completion** - Summarize changes with file links

**Each step has a checklist** - complete or explicitly skip each item before proceeding.

**Reference**: See [Root AGENTS.md](../../AGENTS.md) for complete step-by-step instructions with checklists.  
**Testing**: See [tests/AGENTS.md](../../tests/AGENTS.md) for testing diamond strategy (integration → unit → E2E).  
**Running**: See [docs/running-and-testing.md](../../docs/running-and-testing.md) for `Run` function and test execution.

### 2. Domain-Specific AGENTS.md Files

**🚨 MANDATORY**: ALWAYS read the relevant AGENTS.md file BEFORE working in that area.

**Key Files**:

- [Root AGENTS.md](../../AGENTS.md) - 8-step workflow for ALL development
- [src/AGENTS.md](../../src/AGENTS.md) - .NET patterns across all projects  
- [tests/AGENTS.md](../../tests/AGENTS.md) - Testing diamond (integration → unit → E2E)
- [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [src/TechHub.Api/AGENTS.md](../../src/TechHub.Api/AGENTS.md) - API endpoint patterns
- [docs/AGENTS.md](../../docs/AGENTS.md) - Documentation placement strategy
- [collections/AGENTS.md](../../collections/AGENTS.md) - Content authoring guidelines

**They are nested**: When working on API changes, read src/AGENTS.md → TechHub.Api/AGENTS.md → TechHub.Web/AGENTS.md (if contract changes) → tests/AGENTS.md → tests/TechHub.Api.Tests/AGENTS.md.

**Reference**: See [docs/documentation-index.md](../../docs/documentation-index.md) for complete documentation map.

### 3. Documentation Updates Are Mandatory

**🚨 MANDATORY**: Documentation MUST be updated when code behavior changes.

**Critical Rules**:

- ✅ **Functional docs** (WHAT the system does) → [docs/](../../docs/) directory
- ✅ **Technical docs** (HOW to implement) → Domain AGENTS.md files  
- ✅ **Content guidelines** (writing standards) → [collections/](../../collections/) directory
- 🚫 **Never skip docs** when functionality changes - work is NOT complete until docs sync
- 🚫 **Never duplicate** - link to existing docs instead
- 🚫 **Never mix functional and technical** - keep separate

**Process**:

1. Search for existing docs: `grep_search` in docs/ and AGENTS.md files
2. Update relevant files or create new ones if needed
3. Fix markdown linting: `npx markdownlint-cli2 --fix <file> --config .markdownlint-cli2.jsonc`
4. Regenerate index if headings changed: `scripts/Generate-DocumentationIndex.ps1`

**Reference**: See [docs/AGENTS.md](../../docs/AGENTS.md) for complete documentation placement strategy.

### 4. Configuration-Driven Development

**🚨 MANDATORY**: ALL sections and collections MUST be defined in configuration, NEVER hardcoded.

- **Single source of truth**: `appsettings.json` defines ALL sections, collections, and structure
- **New features work by configuration**: Adding sections/collections requires ONLY config changes, not code
- **Never hardcode values**: Section names, collection types, paths, etc. must come from config

**Reference**: See [docs/terminology.md](../../docs/terminology.md) for section/collection concepts.

### 5. Server-Side Rendering First

**🚨 MANDATORY**: ALL content MUST be fully rendered server-side for initial page load (SEO + performance).

- **SEO**: Search engines must see complete content without JavaScript
- **Performance**: Users must see content immediately (no loading spinners)
- **Progressive enhancement**: JavaScript ONLY enhances server-rendered content

**Reference**: See [docs/render-modes.md](../../docs/render-modes.md) and [docs/seo.md](../../docs/seo.md).

### 6. Accessibility Standards (WCAG 2.1 AA)

**🚨 MANDATORY**: ALL UI components MUST meet WCAG 2.1 Level AA standards.

- **Keyboard navigation**: All interactive elements accessible via keyboard
- **Screen readers**: Proper ARIA labels, semantic HTML
- **Color contrast**: 4.5:1 for normal text, 3:1 for large text
- **Focus indicators**: Visible focus states

**Reference**: See [docs/design-system.md](../../docs/design-system.md).

### 7. Zero Warnings Policy

**🚨 MANDATORY**: Fix ALL compilation warnings and linter errors immediately.

- **Run `get_errors` after every file edit** - catch issues early
- **All tests must pass**: Run `Run` to build + test before considering work complete
- **Markdown linting**: Fix with `npx markdownlint-cli2 --fix <file> --config .markdownlint-cli2.jsonc`
- **No exceptions**: Warnings are not acceptable technical debt

**Reference**: See [docs/running-and-testing.md](../../docs/running-and-testing.md) for `Run` function details.

## Core Development Principles

### Modern .NET/Blazor Stack

- **.NET 10**: Latest LTS with C# 13
- **Blazor Hybrid**: SSR for SEO + WebAssembly for interactivity
- **Clean Architecture**: Core → Infrastructure → API → Web
- **.NET Aspire**: Orchestration, service discovery, telemetry

**Reference**: See [docs/technology-stack.md](../../docs/technology-stack.md) and [docs/architecture.md](../../docs/architecture.md).

### Quality & Testing (Testing Diamond)

- **Integration tests are most important**: All API functionality at the boundary
- **Unit tests for edge cases**: Fast feedback on complex logic
- **E2E tests for critical paths**: User journeys and UI interactions
- **80% minimum coverage**: For unit tests
- **Clean slate principle**: Fix ALL broken tests before new work

**Reference**: See [tests/AGENTS.md](../../tests/AGENTS.md) and [docs/testing-strategy.md](../../docs/testing-strategy.md).

### Performance & UX

- **Server-side rendering**: Complete content on initial load (no spinners)
- **Client-side performance**: Responsive interactions after load
- **Pre-compute at build**: Calculate once, cache intelligently
- **Mobile-first responsive**: Proper rendering on all devices
- **No horizontal scrollbars**: EVER

**Reference**: See [docs/render-modes.md](../../docs/render-modes.md), [docs/seo.md](../../docs/seo.md), and [docs/page-structure.md](../../docs/page-structure.md).

## Development Methodology

This project follows **Spec-Driven Development** using SpecKit:

1. **Constitution** → Project principles (this file)
2. **Specification** → Exact behavior before implementation (`specs/*/spec.md`)
3. **Planning** → Break into testable tasks (`specs/*/plan.md`, `specs/*/tasks.md`)
4. **Implementation** → Follow TDD and 8-step workflow
5. **Verification** → All tests pass, docs updated

**Reference**: See [Root AGENTS.md](../../AGENTS.md) for the mandatory 8-step workflow.

## Governance

- **This constitution supersedes all practices** - Non-negotiable foundation
- **Amendments require justification** - Version follows semantic versioning:
  - MAJOR: Breaking changes to governance or core principles  
  - MINOR: New principles or significant expansions
  - PATCH: Clarifications, wording fixes
- **All code reviews verify compliance** with constitution
- **Specs define authoritative behavior** - `specs/*/spec.md` are the contract

## Version History

**Version**: 3.0.0  
**Ratified**: 2026-01-01  
**Last Amended**: 2026-02-08

**Major Milestones**:

- v1.0.0 (2026-01-01): Initial constitution for .NET migration
- v2.0.0 (2026-01-16): Modern .NET/Blazor focus, added documentation rule
- v3.0.0 (2026-02-08): Complete overhaul - aligned with 8-step workflow, comprehensive docs/, simplified rules
