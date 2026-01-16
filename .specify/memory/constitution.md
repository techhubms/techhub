<!--
SYNC IMPACT REPORT - Constitution Amendment (2026-01-16)
=========================================================

VERSION CHANGE: 1.2.0 → 2.0.0 (MAJOR bump)

REASON FOR MAJOR BUMP:
- Complete rewrite focused on modern .NET/Blazor development (no Jekyll references)
- Removed all Jekyll-related constraints and directory structure rules
- Restructured around ABSOLUTELY CRITICAL rules that reference AGENTS.md files
- Removed backward compatibility stance (already in 1.2.0, but now formalized)
- Principle-driven organization rather than technology stack listing
- Breaking change: Constitution now expects Jekyll removal, not coexistence

MAJOR CHANGES:
- REMOVED: All Jekyll directory structure requirements
- REMOVED: Technology stack details (moved to AGENTS.md)
- REMOVED: Performance/security requirements (moved to AGENTS.md)
- ADDED: ABSOLUTELY CRITICAL rules section (non-negotiable foundation)
- ADDED: Pending features section (filtering, responsive design, etc.)
- RESTRUCTURED: Focus on principles + references to detailed AGENTS.md files
- CLARIFIED: No backwards compatibility - greenfield .NET approach

TEMPLATE CONSISTENCY STATUS:
✅ plan-template.md: Aligned with .NET project structure expectations
✅ spec-template.md: User stories prioritized, independent testing emphasized
✅ tasks-template.md: TDD workflow matches constitution requirements
✅ All AGENTS.md files reviewed and referenced appropriately

FOLLOW-UP ACTIONS:
✅ Constitution rewritten for .NET-first development
✅ Version bumped to 2.0.0 (MAJOR - breaking governance changes)
✅ Last Amended date updated to 2026-01-16
✅ Reviewed 16 AGENTS.md files for consistency
✅ Verified alignment with MIGRATIONSTATUS.md and README.md
✅ Ensured pending features captured from specs/ directory

LAST REVIEW: 2026-01-16
NEXT AMENDMENT: When new principles emerge or fundamental approach changes
-->

# Tech Hub Constitution

> **Purpose**: Modern .NET/Blazor content platform for Microsoft technical content

## Project Identity

- **Name**: Tech Hub
- **Repository**: techhubms/techhub
- **Branch**: dotnet-migration (production deployment in progress)
- **Status**: Production-quality implementation replacing Jekyll

## ABSOLUTELY CRITICAL Rules

**These are the NON-NEGOTIABLE foundation rules that ALL development MUST follow:**

### 1. Test-Driven Development (TDD)

**🚨 MANDATORY**: Tests MUST be written BEFORE implementation code.

- **For bug fixes**: Write failing test that reproduces bug, THEN fix it
- **For new features**: Write tests defining expected behavior, THEN implement
- **E2E tests are MANDATORY** for ALL UI/frontend changes (URL routing, components, buttons, navigation)
- **Never skip tests**: Run `Run -OnlyTests` after ANY code change to verify
- **Clean slate principle**: Fix ALL broken tests before starting new work
- **No exceptions**: Documentation-only changes are the ONLY case where tests can be skipped

**Reference**: See [Root AGENTS.md - Step 6: Write Tests First](../../AGENTS.md#6-write-tests-first-tdd) for complete TDD workflow.  
**Implementation**: See [tests/AGENTS.md](../../tests/AGENTS.md) for testing strategies across all layers.

### 2. 10-Step Development Workflow

**🚨 MANDATORY**: EVERY development task MUST follow this workflow in order.

1. **Core Rules & Boundaries** - Review always/ask/never rules
2. **Gather Context** - Read documentation and code
3. **Create a Plan** - Break down task into steps
4. **Research & Validate** - Use context7 MCP for latest docs
5. **Verify Current Behavior** - Use Playwright MCP for testing (optional)
6. **Write Tests First** - TDD (see rule #1)
7. **Implement Changes** - Make tests pass
8. **Validate & Fix** - Run all tests, fix errors
9. **Update Documentation** - Keep docs in sync
10. **Report Completion** - Summarize changes

**Reference**: See [Root AGENTS.md - AI Assistant Workflow](../../AGENTS.md#ai-assistant-workflow) for detailed instructions on each step.

### 3. Domain-Specific AGENTS.md Files

**🚨 MANDATORY**: ALWAYS read the domain-specific AGENTS.md file BEFORE editing code in that domain.

**Why**: Each domain (src/, tests/, scripts/, collections/, etc.) has specialized patterns, rules, and examples that complement the root AGENTS.md.

**Key Files**:

- [src/AGENTS.md](../../src/AGENTS.md) - .NET development patterns across all projects
- [tests/AGENTS.md](../../tests/AGENTS.md) - Testing strategies (unit, integration, component, E2E)
- [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [src/TechHub.Api/AGENTS.md](../../src/TechHub.Api/AGENTS.md) - API endpoint patterns
- [collections/AGENTS.md](../../collections/AGENTS.md) - Content creation and markdown guidelines

**Reference**: See [Root AGENTS.md - Documentation Architecture](../../AGENTS.md#documentation-architecture) for complete documentation map.

### 4. Configuration-Driven Development

**🚨 MANDATORY**: ALL sections and collections MUST be defined in configuration, NEVER hardcoded.

- **Single source of truth**: `appsettings.json` defines ALL sections, collections, and structure
- **New features work by configuration**: Adding sections/collections requires ONLY config changes, not code
- **Code stays synchronized**: All components derive structure from configuration automatically
- **Never hardcode values**: Section names, collection types, paths, etc. must come from config

**Reference**: See [Root AGENTS.md - Configuration-Driven Development](../../AGENTS.md#configuration-driven-development) for principles.

### 5. Server-Side Rendering First

**🚨 MANDATORY**: ALL content MUST be fully rendered server-side for initial page load.

- **SEO requirement**: Search engines must see complete content without JavaScript
- **Performance requirement**: Users must see complete content immediately
- **Progressive enhancement**: JavaScript ONLY enhances server-rendered content, never creates it
- **The ONLY exception**: `assets/js/sections.js` may modify collection state on page load based on URL parameters

**Reference**: See [Root AGENTS.md - Performance Architecture](../../AGENTS.md#performance-architecture) for complete performance principles.

### 6. Accessibility Standards (WCAG 2.1 AA)

**🚨 MANDATORY**: ALL UI components MUST meet WCAG 2.1 Level AA standards.

- **Keyboard navigation**: All interactive elements accessible via keyboard
- **Screen readers**: Proper ARIA labels, semantic HTML
- **Color contrast**: Minimum 4.5:1 for normal text, 3:1 for large text
- **Focus indicators**: Visible focus states on all interactive elements
- **Never rely on color alone**: Provide text alternatives

**Reference**: See [Root AGENTS.md - Accessibility Standards](../../AGENTS.md#accessibility-standards) for complete accessibility requirements.

## Core Development Principles

### Modern .NET/Blazor Architecture

- **.NET 10**: Latest LTS runtime with C# 13
- **Blazor Hybrid Rendering**: SSR for SEO + WebAssembly for interactivity
- **Clean Architecture**: Separation of concerns (Core, Infrastructure, API, Web)
- **.NET Aspire**: Orchestration, service discovery, OpenTelemetry
- **Repository Pattern**: File-based initially, database-ready architecture

**Reference**: See [Root AGENTS.md - Tech Stack](../../AGENTS.md#tech-stack) for complete technology details.  
**Implementation**: See [src/AGENTS.md](../../src/AGENTS.md) for .NET development patterns.

### Quality & Testing

- **Test Pyramid**: Unit → Integration → Component → E2E
- **80% minimum code coverage** for unit tests
- **Zero warnings policy**: Fix all build warnings immediately
- **Continuous validation**: Run tests after EVERY code change
- **No flaky tests**: Fix or remove, never ignore

**Reference**: See [tests/AGENTS.md](../../tests/AGENTS.md) for comprehensive testing strategies.

### Performance & User Experience

- **Client-side performance is paramount**: After initial load, all interactions must be responsive
- **Pre-compute during build**: Calculate once at build time for optimal runtime performance
- **Never sacrifice functionality**: Use intelligent caching and pre-computation instead
- **Mobile-first responsive design**: Proper rendering regardless of device and screen size
- **No horizontal scrollbars**: EVER

**Reference**: See [Root AGENTS.md - Performance Architecture](../../AGENTS.md#performance-architecture) for complete performance principles.

## Pending Features (Not Yet Implemented)

These features are planned but not yet implemented. See `specs/` directory for detailed specifications:

### High Priority

- **Tagging/Filtering System** - Client-side content filtering by date, tags, text search ([specs/019-filtering-system/](../../specs/019-filtering-system/))
- **Infinite Scrolling** - Progressive content loading with Intersection Observer ([specs/020-infinite-scroll/](../../specs/020-infinite-scroll/))
- **Fully Responsive Design** - Hamburger menu, collapsible navigation, proper mobile rendering ([specs/029-mobile-navigation/](../../specs/029-mobile-navigation/))

### Medium Priority

- **Custom Pages** - Levels of Enlightenment, feature showcases ([specs/028-advanced-ui-features/](../../specs/028-advanced-ui-features/))
- **Search Functionality** - Full-text search across all content ([specs/022-search/](../../specs/022-search/))
- **SEO Optimization** - Meta tags, structured data, sitemaps ([specs/023-seo/](../../specs/023-seo/))

### Lower Priority

- **Distributed Caching** - Redis for performance optimization (may not be needed)
- **Google Analytics** - Usage tracking ([specs/024-google-analytics/](../../specs/024-google-analytics/))
- **Content Publish Flag** - Draft/published workflow ([specs/027-content-publish-flag/](../../specs/027-content-publish-flag/))

## Development Methodology

This project follows **Spec-Driven Development (SDD)** using the spec-kit methodology:

1. **Constitution** → Define project principles and constraints (this file)
2. **Specification** → Document exact behavior before implementation (`specs/*/spec.md`)
3. **Planning** → Break work into small, testable tasks (`specs/*/plan.md`)
4. **Implementation** → Write code that matches specifications (following TDD)
5. **Verification** → Validate implementation against specs (all tests passing)

**Template Files**:

- [spec-template.md](../templates/spec-template.md) - Feature specifications with user stories
- [plan-template.md](../templates/plan-template.md) - Implementation planning
- [tasks-template.md](../templates/tasks-template.md) - Task breakdown and tracking

## Governance

- **This constitution supersedes all other practices** - Non-negotiable foundation
- **Amendments require justification** - Version bumps follow semantic versioning:
  - MAJOR: Breaking changes to governance or core principles
  - MINOR: New principles or significant expansions
  - PATCH: Clarifications, wording fixes, non-semantic refinements
- **All code reviews must verify constitution compliance**
- **Specs define authoritative behavior** - `/specs/NNN-feature-name/spec.md` files are the contract
- **AGENTS.md files provide implementation guidance** - Domain-specific patterns and rules

## Version History

**Version**: 2.0.0  
**Ratified**: 2026-01-01  
**Last Amended**: 2026-01-16  

**Major Milestones**:

- v1.0.0 (2026-01-01): Initial constitution for Jekyll migration
- v1.2.0 (2026-01-02): Removed backwards compatibility constraints
- v2.0.0 (2026-01-16): Complete rewrite for modern .NET/Blazor focus (this version)
