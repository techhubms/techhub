# Tech Hub .NET Migration

> **Status**: 🚧 In Development

This directory contains the .NET/Blazor implementation of Tech Hub, migrating from the Jekyll-based static site.

## Quick Start

**Easiest Way - F5 in VS Code**:

1. Open the project in VS Code
2. Press **F5** (or click **Run > Start Debugging**)
3. Select **"Tech Hub (API + Web)"** from the dropdown
4. Web UI opens automatically at <http://localhost:5184>
5. API available at <http://localhost:5029> (Swagger: <http://localhost:5029/swagger>)

**Alternative - PowerShell Script**:

```powershell
# Build and run both API and Web
./run.ps1

# Clean build and test first, then start both API and Web if tests pass
./run.ps1 -Clean -Test
```

**Stop the Application**: Press `Ctrl+C` in the terminal where the script is running.

## Migration Status

This project is currently migrating from Jekyll to .NET/Blazor using [spec-kit](https://github.com/github/spec-kit) methodology. We're in **Phase 3** of the migration with 245/245 unit/integration tests and 60/69 E2E tests passing.

**Quick Status**: API ✅ Complete | Frontend 🔄 In Progress (95%)

For detailed progress, implementation status, and what's working now, see [MIGRATIONSTATUS.md](MIGRATIONSTATUS.md).

## Documentation Guide

This project uses a **3-level documentation hierarchy**:

**Level 1: Domain-Specific AGENTS.md** (Read FIRST when working in a domain):

- [src/TechHub.Api/AGENTS.md](src/TechHub.Api/AGENTS.md) - API patterns
- [src/TechHub.Web/AGENTS.md](src/TechHub.Web/AGENTS.md) - Blazor patterns
- [tests/AGENTS.md](tests/AGENTS.md) - Testing strategies
- [See complete list in root AGENTS.md](AGENTS.md#complete-3-level-documentation-structure)

**Level 2: Root [AGENTS.md](AGENTS.md)** (Read SECOND for repository-wide standards):

- 10-step AI Assistant Workflow (REQUIRED for all development)
- Core rules and boundaries
- .NET tech stack, commands, and patterns
- Architecture principles and site terminology

**Level 3: [README.md](README.md)** (You're here - minimal getting started)

**Precedence**: Domain AGENTS.md → Root AGENTS.md → README.md

**Supporting Documentation**:

- **Feature Specifications**: [specs/](specs/) - Detailed requirements using spec-kit
- **Functional Docs**: [docs/filtering-system.md](docs/filtering-system.md), [docs/content-management.md](docs/content-management.md), [docs/api-specification.md](docs/api-specification.md)
- **Migration Planning**: [specs/dotnet-migration/](specs/dotnet-migration/) - Complete migration spec and task breakdown

## Contributing

All development must follow the guidelines in [AGENTS.md](AGENTS.md):

- **[AI Assistant Workflow](AGENTS.md#ai-assistant-workflow)** - Required 10-step process
- **[Core Rules & Boundaries](AGENTS.md#1-core-rules--boundaries)** - Non-negotiable rules
- **[Feature Specifications](specs/)** - Use spec-kit methodology for all features

## DevContainer Setup

1. In VS Code, open Command Palette (`Ctrl+Shift+P`)
2. Select **"Dev Containers: Reopen in Container"**
3. Wait for container to build and initialize
4. Use F5 or run script as described above
