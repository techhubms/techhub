# Tech Hub .NET Migration

> **Status**: 🚧 In Development

This directory contains the .NET/Blazor implementation of Tech Hub, migrating from the Jekyll-based static site.

## Quick Start

### Option 1: Open in DevContainer (Recommended)

1. In VS Code, open the Command Palette (`Ctrl+Shift+P` or `Cmd+Shift+P`)
2. Select **"Dev Containers: Reopen in Container"**
3. Choose the **"Tech Hub .NET"** container
4. Wait for the container to build and initialize

### Option 2: Local Development

Requirements:

- .NET 10 SDK
- Node.js LTS
- PowerShell 7+
- Azure CLI (optional)

```powershell
# Install .NET Aspire workload
dotnet workload install aspire

# Restore packages (when solution exists)
dotnet restore

# Run with Aspire (when solution exists)
dotnet run --project src/TechHub.AppHost
```

## Architecture

This is a modern .NET application with **separate frontend and backend**:

- **TechHub.Api** - REST API backend (ASP.NET Core Minimal API)
- **TechHub.Web** - Blazor frontend (SSR + WebAssembly)
- **TechHub.Core** - Domain models and interfaces
- **TechHub.Infrastructure** - Data access implementations
- **TechHub.AppHost** - .NET Aspire orchestration

See [/specs/](../specs/) for detailed feature specifications.

## Development Workflow

Follow the [8-step workflow](../AGENTS.md#ai-assistant-workflow) defined in the root AGENTS.md:

1. **Gather Context** - Read AGENTS.md files for the domain you're modifying
2. **Create a Plan** - Break down tasks into steps
3. **Research & Validate** - Use context7 MCP for .NET/Blazor docs
4. **Verify Behavior** - Use Playwright MCP for testing
5. **Implement Changes** - Follow patterns in domain AGENTS.md
6. **Test & Validate** - Run appropriate test suites
7. **Update Documentation** - Keep AGENTS.md files current
8. **Report Completion** - Summarize changes

## Documentation

- **[Feature Specifications](../specs/)** - Complete feature requirements and specifications
- **[Root AGENTS.md](../AGENTS.md)** - Framework-agnostic principles
- **[.NET AGENTS.md](AGENTS.md)** - .NET-specific development guide
- **[@dotnet Agent](../.github/agents/dotnet.md)** - Custom agent for .NET development

## Current Status

Following the migration plan phases:

- [ ] Phase 0: Planning & Research
- [ ] Phase 1: Environment Setup ← **You are here**
- [ ] Phase 2: Core Architecture
- [ ] Phase 3: Content System
- [ ] Phase 4: Features Implementation
- [ ] Phase 5: Testing & Validation
- [ ] Phase 6: Azure Infrastructure
- [ ] Phase 7: CI/CD Pipeline
- [ ] Phase 8: Migration & Cutover

See [/specs/](../specs/) for detailed feature specifications and implementation status.

## Contributing

This is a migration project. All changes should:

1. Follow the [feature specifications](../specs/)
2. Use spec-driven development methodology
3. Maintain feature parity with Jekyll site
4. Include tests for all code changes

## Related Resources

- [Current Jekyll Site](../) - Source implementation
- [Filtering System Docs](../docs/filtering-system.md) - Current filtering behavior
- [Content Management Docs](../docs/content-management.md) - Content workflows
- [spec-kit](https://github.com/github/spec-kit) - Development methodology
