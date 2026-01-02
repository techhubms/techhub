# Implementation Plan: Tech Hub .NET Migration

**Branch**: `dotnet-migration` | **Date**: 2026-01-02 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/dotnet-migration/spec.md`

**Note**: This plan guides the complete migration of Tech Hub from Jekyll to .NET/Blazor with optimal greenfield architecture.

## Summary

Migrate Tech Hub content platform from Jekyll/Ruby to modern .NET/Blazor architecture with separate API and frontend, optimized for performance, accessibility, and maintainability. The system organizes content into 7 sections with 5 collection types, supports client-side filtering (date/tags/search), generates RSS feeds, and implements infinite scroll pagination. Built using .NET 10, Blazor SSR + WASM, .NET Aspire orchestration, with deployment to Azure Container Apps.

## Technical Context

**Language/Version**: .NET 10 (latest LTS), C# 12+  
**Primary Dependencies**: ASP.NET Core, Blazor, Markdig, Polly, OpenTelemetry, bUnit, Playwright  
**Storage**: File-based markdown initially (repository pattern allows future database migration at ~1000+ items)  
**Testing**: xUnit (unit), WebApplicationFactory (integration), bUnit (component), Playwright (E2E)  
**Target Platform**: Azure Container Apps (Linux containers), Blazor SSR + WASM hybrid rendering  
**Project Type**: Web application with separate API backend + Blazor frontend (monorepo)  
**Performance Goals**: Lighthouse >95, LCP <2.5s, FID <100ms, CLS <0.1, API response <50ms p95 (cached)  
**Constraints**: WCAG 2.1 AA compliance, <200ms TTFB p95, works without JavaScript (progressive enhancement)  
**Scale/Scope**: 7 sections, 5 collections, ~1000 content items initially, auto-scale 1-10 instances, 10k concurrent users max

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

✅ **Modern UX First**: Using .NET 10 + Blazor SSR/WASM hybrid for state-of-the-art experience  
✅ **Configuration-Driven**: sections.json remains single source of truth (spec FR-003)  
✅ **Hybrid Rendering**: SSR for SEO + WASM for interactivity (spec FR-020)  
✅ **Semantic HTML**: WCAG 2.1 AA compliance + Schema.org markup (spec FR-025-FR-030)  
✅ **Performance**: Aggressive caching, infinite scroll, lazy loading (spec FR-019-FR-024)  
✅ **Accessibility**: Full keyboard navigation, screen reader support (spec FR-030)  
✅ **Must not modify Jekyll source**: Used only as reference documentation  
✅ **Repository pattern**: File-based initially, database-ready (spec FR-035)  
✅ **Progressive enhancement**: Server-side rendered, works without JavaScript (spec FR-019)  
✅ **No backwards compatibility**: Greenfield optimal design per constitution v1.2.0  
✅ **Directory structure**: All .NET code in `dotnet/` per constitution requirements  
✅ **Quality standards**: 80% code coverage, all test types defined (spec SC-023)  
✅ **Security**: No secrets in code, validated inputs, HTTPS only  
✅ **Performance requirements**: All metrics defined in spec (SC-001 through SC-007)  

**Status**: ✅ PASS - All constitution principles satisfied by spec. Proceed to Phase 0.

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
dotnet/                           # All .NET implementation code
├── .devcontainer/               # .NET-specific DevContainer config
├── src/
│   ├── TechHub.Core/           # Domain models and interfaces
│   │   ├── Models/             # Domain entities (Section, ContentItem, etc.)
│   │   ├── Interfaces/         # Repository and service interfaces
│   │   └── DTOs/               # Data transfer objects
│   ├── TechHub.Infrastructure/ # Data access implementations
│   │   ├── Repositories/       # File-based repository implementations
│   │   ├── Caching/            # IMemoryCache and output caching
│   │   └── Services/           # Markdown parsing, RSS generation
│   ├── TechHub.Api/            # ASP.NET Core Minimal API backend
│   │   ├── Endpoints/          # API endpoint definitions
│   │   ├── Program.cs          # API configuration and DI setup
│   │   └── appsettings.json    # API configuration
│   ├── TechHub.Web/            # Blazor frontend (SSR + WASM)
│   │   ├── Components/         # Reusable Blazor components
│   │   ├── Pages/              # Page-level components
│   │   ├── Services/           # API client, filter state management
│   │   ├── wwwroot/            # Static assets (CSS, JS, images)
│   │   ├── Program.cs          # Web configuration and DI setup
│   │   └── appsettings.json    # Web configuration
│   └── TechHub.AppHost/        # .NET Aspire orchestration
│       └── Program.cs          # Aspire service definitions
├── tests/
│   ├── TechHub.Core.Tests/     # xUnit unit tests for domain models
│   ├── TechHub.Infrastructure.Tests/ # xUnit tests for repositories
│   ├── TechHub.Api.Tests/      # Integration tests (WebApplicationFactory)
│   ├── TechHub.Web.Tests/      # bUnit component tests
│   └── TechHub.E2E.Tests/      # Playwright end-to-end tests
├── TechHub.sln                 # .NET solution file
├── README.md                   # .NET-specific documentation
└── AGENTS.md                   # .NET development guide

collections/                     # Markdown content files (unchanged)
├── _news/
├── _videos/
├── _community/
├── _blogs/
└── _roundups/

_data/
└── sections.json               # Site structure configuration (unchanged)

.github/                        # CI/CD and GitHub configuration (root level)
.specify/                       # Spec-kit templates and memory (root level)
specs/                          # Feature specifications (root level)
AGENTS.md                       # Framework-agnostic principles (root level)
```

**Structure Decision**: Web application architecture with separate API and Blazor frontend per constitution requirements. All .NET code isolated in `dotnet/` directory, while content, configuration, and infrastructure remain at repository root for consistency with Jekyll reference implementation.

## Complexity Tracking

> **No violations detected** - All architecture decisions align with constitution principles. No justification required.

---

## Phase 0: Research ✅ COMPLETE

**Artifacts Generated**:
- [research.md](research.md) - All technology choices validated

**Key Decisions**:
- .NET 10 + Blazor SSR/WASM hybrid rendering
- Separate API + Frontend architecture
- File-based storage with repository pattern
- IMemoryCache caching strategy
- Client-side filtering implementation
- xUnit/WebApplicationFactory/bUnit/Playwright testing
- Azure Container Apps deployment
- OpenTelemetry + Application Insights monitoring

**Status**: ✅ All unknowns resolved, ready for Phase 1

---

## Phase 1: Design & Contracts ✅ COMPLETE

**Artifacts Generated**:
- [data-model.md](data-model.md) - Complete entity and DTO definitions
- [contracts/README.md](contracts/README.md) - REST API endpoint specifications
- [quickstart.md](quickstart.md) - Developer onboarding guide
- `.github/agents/copilot-instructions.md` - Updated with .NET context

**Key Deliverables**:
- Domain entities: Section, CollectionReference, ContentItem, FilterState
- DTOs: SectionDto, ContentItemDto, ContentItemDetailDto, PagedResultDto, RssChannelDto
- 8 REST API endpoints fully specified
- Entity relationships and validation rules documented
- Extension methods for entity/DTO conversion
- Developer quickstart with common tasks
- Agent context updated with technology stack

**Status**: ✅ Design complete, ready for Phase 2 (Tasks)

---

## Post-Design Constitution Check

Re-validating constitution compliance after detailed design:

✅ **Modern UX First**: Blazor SSR+WASM provides optimal user experience  
✅ **Configuration-Driven**: sections.json single source of truth, no hardcoded values  
✅ **Hybrid Rendering**: SSR for SEO, WASM for client-side interactivity  
✅ **Semantic HTML**: All DTOs support Schema.org markup generation  
✅ **Performance**: Caching strategy defined (IMemoryCache with expiration policies)  
✅ **Accessibility**: Data model supports WCAG 2.1 AA compliance metadata  
✅ **Repository pattern**: File-based initially, designed for future database migration  
✅ **Progressive enhancement**: Server-side rendering works without JavaScript  
✅ **Directory structure**: All .NET code in `dotnet/` per constitution  
✅ **Quality standards**: Testing strategy covers all test types (unit/integration/component/E2E)  
✅ **Security**: No secrets in DTOs, API contracts follow security best practices  
✅ **Performance SLAs**: API contracts define <50ms p95 response time targets  

**Final Status**: ✅ PASS - All constitution principles satisfied by detailed design

---

## Next Steps

1. ✅ Phase 0: Research COMPLETE
2. ✅ Phase 1: Design & Contracts COMPLETE
3. **Phase 2: Tasks** - Generate actionable task breakdown:
   - Run `/speckit.tasks` command
   - Generates `tasks.md` with dependency-ordered implementation checklist
4. **Phase 3: Implementation** - Execute tasks following spec-driven development:
   - Run `/speckit.implement` command (or manual implementation)
   - Follow 6-phase implementation sequence from research.md

**Status**: ✅ Planning workflow complete - Ready for task generation and implementation
