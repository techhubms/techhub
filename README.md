# Tech Hub .NET Migration

> **Status**: 🚧 In Development

This directory contains the .NET/Blazor implementation of Tech Hub, migrating from the Jekyll-based static site.

## Migration Status

This project is currently migrating from Jekyll to .NET/Blazor using [spec-kit](https://github.com/github/spec-kit) methodology. We're in **Phase 3** of the migration with 52/52 tests passing.

**Quick Status**: API ✅ Complete | Frontend 🔄 In Progress (90%)

For detailed progress, implementation status, and what's working now, see [MIGRATIONSTATUS.md](MIGRATIONSTATUS.md).

**Migration Planning**:

- **[Migration Plan](specs/dotnet-migration/spec.md)** - Complete migration specification and architecture
- **[Task Breakdown](specs/dotnet-migration/tasks.md)** - All tasks with dependencies and status
- **[Data Model](specs/dotnet-migration/data-model.md)** - Domain model design and contracts

## Starting / stopping the website

### For AI Agents

**ALWAYS read [AGENTS.md - Starting & Stopping the Website](AGENTS.md#starting--stopping-the-website)** before starting/stopping the website. This section contains critical safety rules about terminal interactions and proper use of Playwright MCP tools.

### For End Users

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

### DevContainer Setup

1. In VS Code, open Command Palette (`Ctrl+Shift+P`)
2. Select **"Dev Containers: Reopen in Container"**
3. Wait for container to build and initialize
4. Use F5 or run script as described above

## Where to Find More Information

**Start Here**:

- **[AGENTS.md](AGENTS.md)** - Complete development guide (AI workflow, architecture, principles, standards)
- **[Migration Status](#migration-status)** - Current implementation progress

**For Development**:

- **[AI Assistant Workflow](AGENTS.md#ai-assistant-workflow)** - Required 9-step process for all changes
- **[Starting & Stopping the Website](AGENTS.md#starting--stopping-the-website)** - How to run and test locally
- **[.NET Development Guide](.github/agents/dotnet.md)** - .NET/Blazor patterns and examples
- **[Documentation Map](AGENTS.md#complete-documentation-map)** - Navigation to all docs

**For Understanding the System**:

- **[Project Overview](AGENTS.md#project-overview)** - Architecture and project structure
- **[API Specification](docs/api-specification.md)** - REST API endpoints and contracts
- **[Feature Specifications](specs/)** - Detailed feature requirements (spec-kit)
- **[Filtering System](docs/filtering-system.md)** - How filtering works
- **[Content Management](docs/content-management.md)** - Content workflows

## Contributing

All development must follow the guidelines in [AGENTS.md](AGENTS.md):

- **[AI Assistant Workflow](AGENTS.md#ai-assistant-workflow)** - Required 9-step process
- **[Core Rules & Boundaries](AGENTS.md#0-core-rules--boundaries)** - Non-negotiable rules
- **[Feature Specifications](specs/)** - Use spec-kit methodology for all features
