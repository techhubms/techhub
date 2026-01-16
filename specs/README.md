# Tech Hub Feature Specifications

**Purpose**: This directory contains specifications for features currently being built or planned for Tech Hub.

## Organization

Specs are organized by feature, numbered in priority order:

- **001-011**: Active feature specs (building or planning)
- **_archive/**: Completed specs (documentation moved to AGENTS.md files)

## Current Specs

| # | Feature | Status | Priority |
| --- | ------- | ------ | -------- |
| 001 | [Filtering System](001-filtering-system/) | ✅ Spec Complete | HIGH |
| 002 | [Search](002-search/) | ✅ Spec Complete | HIGH |
| 003 | [Infinite Scroll](003-infinite-scroll/) | ✅ Spec Complete | MEDIUM |
| 004 | [Custom Pages](004-custom-pages/) | ✅ Spec Complete | HIGH |
| 005 | [Mobile Navigation](005-mobile-navigation/) | ✅ Spec Complete | HIGH |
| 006 | [SEO Optimization](006-seo/) | ✅ Spec Complete | MEDIUM |
| 007 | [Google Analytics](007-google-analytics/) | ✅ Spec Complete | MEDIUM |
| 008 | [Azure Infrastructure](008-azure-infrastructure/) | ✅ Spec Complete | LOW |
| 009 | [CI/CD Pipeline](009-ci-cd-pipeline/) | ✅ Spec Complete | LOW |
| 010 | [Content Publish Flag](010-content-publish-flag/) | ✅ Spec Complete | LOW |

**Status Legend**:

- 🔄 **In Progress** - Currently implementing
- ⏳ **Planned** - Not started, spec may need updates
- ⚠️ **Partial** - Some functionality exists, needs completion/refinement

## How to Use Specs

### For Developers

1. **Before starting a feature**: Read the spec to understand requirements
2. **While implementing**: Reference spec for acceptance criteria and edge cases
3. **After completing**: Update spec status or move to archive if documentation moved to AGENTS.md

### For Spec Writers

Follow the [SpecKit](../.specify/) process:

1. `/speckit.specify` - Create initial spec from feature description
2. `/speckit.clarify` - Ask clarification questions
3. `/speckit.plan` - Generate implementation plan
4. `/speckit.tasks` - Break down into actionable tasks
5. `/speckit.implement` - Execute implementation

### Spec Lifecycle

1. **Created**: Spec written with requirements and user stories
2. **Active**: Feature being implemented
3. **Completed**: Feature done, spec archived or documentation moved to AGENTS.md
4. **Deprecated**: Spec no longer relevant (moved to `_archive/deprecated/`)

## Finding Documentation

### For Implemented Features

Check domain-specific AGENTS.md files:

- [src/AGENTS.md](../src/AGENTS.md) - .NET patterns across all projects
- [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [src/TechHub.Api/AGENTS.md](../src/TechHub.Api/AGENTS.md) - API endpoint patterns
- [tests/AGENTS.md](../tests/AGENTS.md) - Testing strategies

### For Functional Behavior

Check docs/ directory:

- [docs/api-specification.md](../docs/api-specification.md) - REST API contracts
- [docs/content-management.md](../docs/content-management.md) - Content workflows
- [docs/rss-feeds.md](../docs/rss-feeds.md) - RSS feed specification
- `docs/filtering-system.md` - **TO BE CREATED** (see specs/001-filtering-system for requirements)

### For Migration Status

Check [MIGRATIONSTATUS.md](../MIGRATIONSTATUS.md) for current implementation progress.

## Questions?

See [Root AGENTS.md](../AGENTS.md) for development workflow and architecture.
