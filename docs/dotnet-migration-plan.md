# Tech Hub .NET Migration Plan

> **AI AGENT CONTEXT**: This document is a comprehensive step-by-step plan for migrating Tech Hub from Jekyll to .NET/Blazor. Follow the checkboxes in order. Mark completed items with `[x]`. This plan uses spec-driven development methodology.

## Index

- [Overview](#overview)
  - [Current State](#current-state)
  - [Target State](#target-state)
  - [Key Architectural Decisions](#key-architectural-decisions)
  - [Development Methodology](#development-methodology)
- [Critical Rules](#critical-rules)
  - [✅ Always Do](#-always-do)
  - [🚫 Never Do](#-never-do)
- [Phase 0: Planning & Research](#phase-0-planning--research)
  - [0.1 Create Project Constitution](#01-create-project-constitution)
  - [0.2 Document Current Site Behavior](#02-document-current-site-behavior)
  - [0.3 Create Feature Specifications](#03-create-feature-specifications)
  - [0.4 Create AGENTS.md Documentation Structure](#04-create-agentsmd-documentation-structure)
- [Phase 1: Environment Setup](#phase-1-environment-setup)
  - [1.1 Create .NET DevContainer](#11-create-net-devcontainer)
  - [1.2 Configure VS Code Debugging](#12-configure-vs-code-debugging)
  - [1.3 Initialize Solution Structure](#13-initialize-solution-structure)
  - [1.4 Configure Aspire](#14-configure-aspire)
- [Phase 2: Core Architecture](#phase-2-core-architecture)
  - [2.1 Define Domain Models](#21-define-domain-models)
  - [2.2 Implement Repository Pattern](#22-implement-repository-pattern)
  - [2.3 Implement Markdown Processing](#23-implement-markdown-processing)
  - [2.4 Configure API Project](#24-configure-api-project)
  - [2.5 Configure Web Project (Blazor Frontend)](#25-configure-web-project-blazor-frontend)
  - [2.6 Implement API Endpoints](#26-implement-api-endpoints)
  - [2.7 MCP Server Preparation](#27-mcp-server-preparation)
  - [2.8 Authentication Preparation](#28-authentication-preparation)
- [Phase 3: Content System](#phase-3-content-system)
  - [3.1 Implement Blazor Components](#31-implement-blazor-components)
  - [3.2 Implement Page Components](#32-implement-page-components)
  - [3.3 Implement Filtering System](#33-implement-filtering-system)
  - [3.4 Implement RSS Feeds](#34-implement-rss-feeds)
- [Phase 4: Features Implementation](#phase-4-features-implementation)
  - [4.1 Implement Styling](#41-implement-styling)
  - [4.2 Implement SEO Features](#42-implement-seo-features)
  - [4.3 Implement Performance Optimizations](#43-implement-performance-optimizations)
  - [4.4 Implement Resilience](#44-implement-resilience)
- [Phase 5: Testing & Validation](#phase-5-testing--validation)
  - [5.1 Unit Testing Strategy](#51-unit-testing-strategy)
  - [5.2 Integration Testing Strategy](#52-integration-testing-strategy)
  - [5.3 E2E Testing Strategy](#53-e2e-testing-strategy)
  - [5.4 Visual Regression Testing](#54-visual-regression-testing)
- [Phase 6: Azure Infrastructure](#phase-6-azure-infrastructure)
  - [6.1 Create Bicep Infrastructure](#61-create-bicep-infrastructure)
  - [6.2 Configure Networking](#62-configure-networking)
  - [6.3 Configure Monitoring](#63-configure-monitoring)
  - [6.4 Configure Security](#64-configure-security)
- [Phase 7: CI/CD Pipeline](#phase-7-cicd-pipeline)
  - [7.1 Create GitHub Actions Workflow](#71-create-github-actions-workflow)
  - [7.2 Create PowerShell Deployment Scripts](#72-create-powershell-deployment-scripts)
- [Phase 8: Migration & Cutover](#phase-8-migration--cutover)
  - [8.1 Pre-Migration Checklist](#81-pre-migration-checklist)
  - [8.2 Migration Steps](#82-migration-steps)
  - [8.3 Post-Migration](#83-post-migration)
- [Technical Specifications](#technical-specifications)
  - [URL Structure](#url-structure)
  - [Date Handling](#date-handling)
  - [Semantic HTML Elements](#semantic-html-elements)
  - [Schema.org Types](#schemaorg-types)
- [Reference Architecture](#reference-architecture)
- [Progress Tracking](#progress-tracking)
- [Appendix](#appendix)
  - [A. External Resources](#a-external-resources)
  - [B. Related Documentation](#b-related-documentation)
  - [C. Glossary](#c-glossary)

---

## Overview

### Current State

Tech Hub is a Jekyll-based static site with:

- **7 sections**: GitHub Copilot, AI, ML, DevOps, Azure, .NET, Security (plus "All")
- **5 collections**: News, Blogs, Videos, Community, Roundups
- **Filtering system**: Date-based, tag-based, and text search
- **RSS feeds**: Per-section and per-collection
- **Configuration-driven**: `_data/sections.json` is single source of truth
- **Timezone**: Europe/Brussels for all date operations

### Target State

Modern .NET application with **separate frontend and backend**:

**Frontend (Blazor WebAssembly + Server)**:

- **Blazor** with Server-Side Rendering for SEO, WebAssembly for interactivity
- **.NET Aspire** for cloud-native orchestration
- **NLWeb compatible** semantic HTML with Schema.org

**Backend (REST API)**:

- **ASP.NET Core Minimal API** - secure, well-documented API
- **MCP Server ready** - designed to expose Model Context Protocol in future
- **Authentication ready** - prepared for IdentityServer/Duende integration
- **OpenAPI/Swagger** documentation

**Analytics & Monitoring**:

- **Google Analytics 4** (GA4) - client tracking (existing: `G-95LLB67KJV`)
- **OpenTelemetry** + **App Insights** - server-side monitoring

**Infrastructure**:

- **.NET 10** (latest LTS)
- **Azure Container Apps** deployment (separate containers for frontend/API)
- **Bicep** Infrastructure as Code
- **GitHub Actions** with PowerShell scripts

### Key Architectural Decisions

#### Separate Frontend and Backend

The application uses a **clean separation** between frontend (Blazor) and backend (API):

- **TechHub.Web** - Blazor frontend, handles all rendering and user interaction
- **TechHub.Api** - REST API, handles all data access and business logic
- Frontend calls API via typed HttpClient with resilience policies
- API is secured and can be deployed independently

#### MCP Server Capability (Future)

The API is designed to support **Model Context Protocol (MCP)** in the future:

- API endpoints follow resource-oriented design
- Content is served in structured, machine-readable formats
- Schema.org metadata embedded in responses
- API can be extended to support MCP transport (stdio, HTTP+SSE)
- See [Phase 2.7: MCP Server Preparation](#27-mcp-server-preparation) for details

#### Authentication (Future)

The architecture supports adding authentication later:

- API designed with authorization in mind (claims-based)
- Ready for **Duende IdentityServer** or **Microsoft Entra ID** integration
- BFF (Backend-for-Frontend) pattern supported
- See [Phase 2.8: Authentication Preparation](#28-authentication-preparation) for details

#### Multi-Location Content URLs

Content can be accessed from multiple section contexts:

- Same article accessible via `/ai/videos/vs-code-107.html` AND `/github-copilot/videos/vs-code-107.html`
- Each URL is fully standalone (no query parameters like `?section=`)
- Canonical URL specified in metadata for SEO
- See [URL Structure](#url-structure) for complete routing rules

### Development Methodology

This migration follows **Spec-Driven Development (SDD)** using the [spec-kit](https://github.com/github/spec-kit) methodology:

1. **Constitution** → Define project principles and constraints
2. **Specification** → Document exact behavior before implementation
3. **Planning** → Break work into small, testable tasks
4. **Implementation** → Write code that matches specifications
5. **Verification** → Validate implementation against specs

---

## Critical Rules

### ✅ Always Do

- [ ] Mark checkboxes `[x]` as items are completed
- [ ] Use spec-kit commands for each feature: `/speckit.specify` → `/speckit.plan` → `/speckit.implement`
- [ ] Write tests BEFORE or DURING implementation (TDD)
- [ ] Use repository pattern for all data access
- [ ] Server-side render ALL visible content (Blazor SSR)
- [ ] Use semantic HTML elements (article, section, nav, main, header, footer, time, figure)
- [ ] Include Schema.org structured data for NLWeb compatibility
- [ ] Use Europe/Brussels timezone for all date operations
- [ ] Store dates as Unix epoch timestamps internally
- [ ] Follow existing URL structure exactly (SEO preservation)
- [ ] Maintain identical visual styling
- [ ] Test on mobile and desktop viewports
- [ ] Document all specifications in `/specs/` directory

### 🚫 Never Do

- [ ] Never modify the Jekyll site (except documentation that stays)
- [ ] Never use JavaScript for initial content rendering (SSR only)
- [ ] Never hardcode section/collection configuration
- [ ] Never skip writing specifications before coding
- [ ] Never deploy without all tests passing
- [ ] Never expose secrets in configuration or code
- [ ] Create clean, optimal URL structure (no legacy compatibility required)

---

## Phase 0: Planning & Research

### 0.1 Create Project Constitution

- [ ] Create `/specs/.speckit/constitution.md` with:

```markdown
# Tech Hub .NET Migration Constitution

## Project Identity
- **Name**: Tech Hub .NET
- **Purpose**: Modern .NET implementation of Tech Hub content platform
- **Repository**: [current repository]/dotnet

## Core Principles
1. **SEO Preservation**: All existing URLs must work identically
2. **Configuration-Driven**: sections.json remains single source of truth
3. **Server-Side First**: All content rendered server-side
4. **Semantic HTML**: NLWeb-compatible markup with Schema.org
5. **Performance**: Sub-second page loads, efficient caching
6. **Accessibility**: WCAG 2.1 Level AA compliance

## Technology Stack
- Runtime: .NET 10 (latest)
- Web Framework: Blazor (Interactive Server)
- Orchestration: .NET Aspire
- Markdown: Markdig
- Testing: xUnit, bUnit, Playwright
- Infrastructure: Azure Container Apps, Bicep
- Monitoring: OpenTelemetry, Application Insights

## Constraints
- Must not modify Jekyll source (except shared docs)
- Must support all existing RSS feed URLs
- Must maintain identical visual appearance
- Must use repository pattern (file-based initially, database-ready)
```

- [ ] Review and finalize constitution

### 0.2 Document Current Site Behavior

- [ ] Create `/specs/current-site-analysis.md` documenting:
  - [ ] All page types (home, section index, collection, item detail)
  - [ ] URL structure patterns
  - [ ] Filtering behavior (date, tags, text search)
  - [ ] RSS feed formats
  - [ ] Mobile responsiveness behavior
  - [ ] Navigation patterns

### 0.3 Create Feature Specifications

For each major feature, create a specification using `/speckit.specify`:

- [ ] `/specs/features/content-rendering.md` - Markdown to HTML pipeline
- [ ] `/specs/features/section-system.md` - Section/collection architecture
- [ ] `/specs/features/filtering-system.md` - Client-side filtering
- [ ] `/specs/features/rss-feeds.md` - RSS generation
- [ ] `/specs/features/search.md` - Text search functionality
- [ ] `/specs/features/seo.md` - SEO and Schema.org markup
- [ ] `/specs/features/google-analytics.md` - Google Analytics 4 integration

### 0.4 Create AGENTS.md Documentation Structure

> **CRITICAL**: Follow the documentation strategy defined in `/docs/AGENTS.md`. The .NET solution requires its own AGENTS.md files for AI agent context.

- [ ] Create `/dotnet/AGENTS.md` (Root .NET AGENTS.md):

```markdown
# Tech Hub .NET Development Guide

> **AI CONTEXT**: This is the **ROOT** context file for the .NET solution. 
> For framework-agnostic principles, see the parent [/AGENTS.md](../AGENTS.md).

## Index

- [AI Assistant Workflow](#ai-assistant-workflow)
- [Solution Structure](#solution-structure)
- [Development Principles](#development-principles)
- [Documentation Map](#documentation-map)

## AI Assistant Workflow

Follow the 8-step workflow defined in the root AGENTS.md:
1. Gather Context - Read AGENTS.md files for the domain you're modifying
2. Create a Plan - Break down tasks into steps
3. Research & Validate - Use context7 MCP for .NET/Blazor docs
4. Verify Behavior - Use Playwright MCP for testing
5. Implement Changes - Follow patterns in domain AGENTS.md
6. Test & Validate - Run appropriate test suites
7. Update Documentation - Keep AGENTS.md files current
8. Report Completion - Summarize changes

## Solution Structure

```text
dotnet/
├── AGENTS.md                     # This file - root .NET context
├── src/
│   ├── TechHub.Api/             # REST API (see src/TechHub.Api/AGENTS.md)
│   ├── TechHub.Web/             # Blazor frontend (see src/TechHub.Web/AGENTS.md)
│   ├── TechHub.Core/            # Domain models (see src/TechHub.Core/AGENTS.md)
│   ├── TechHub.Infrastructure/  # Data access (see src/TechHub.Infrastructure/AGENTS.md)
│   ├── TechHub.ServiceDefaults/ # Shared Aspire config
│   └── TechHub.AppHost/         # Aspire orchestration
├── tests/                       # See tests/AGENTS.md
├── infra/                       # See infra/AGENTS.md
└── scripts/                     # See scripts/AGENTS.md
```

## Development Principles

All principles from root AGENTS.md apply. Additionally:

### .NET-Specific Rules

- **Use .NET 10** latest LTS features and patterns
- **Prefer records** for DTOs and immutable models
- **Use Minimal APIs** for the REST backend
- **Use file-scoped namespaces** in all C# files
- **Use nullable reference types** - enable in all projects
- **Prefer async/await** for all I/O operations

### Testing Requirements

- **Unit tests**: xUnit with Moq for mocking
- **Component tests**: bUnit for Blazor components
- **Integration tests**: WebApplicationFactory for API
- **E2E tests**: Playwright with page object pattern

## Documentation Map

| Area | AGENTS.md Location | Purpose |
| ---- | ------------------ | ------- |
| Root .NET | `/dotnet/AGENTS.md` | High-level .NET guidance |
| API | `/dotnet/src/TechHub.Api/AGENTS.md` | API development patterns |
| Web | `/dotnet/src/TechHub.Web/AGENTS.md` | Blazor component patterns |
| Core | `/dotnet/src/TechHub.Core/AGENTS.md` | Domain model patterns |
| Infrastructure | `/dotnet/src/TechHub.Infrastructure/AGENTS.md` | Data access patterns |
| Tests | `/dotnet/tests/AGENTS.md` | Testing strategy |
| Infra | `/dotnet/infra/AGENTS.md` | Bicep/Azure patterns |
| Scripts | `/dotnet/scripts/AGENTS.md` | Automation scripts |

See each domain AGENTS.md for specific patterns and rules.
```

- [ ] Create domain-specific AGENTS.md files:
  - [ ] `/dotnet/src/TechHub.Api/AGENTS.md` - API endpoint patterns, Minimal API conventions
  - [ ] `/dotnet/src/TechHub.Web/AGENTS.md` - Blazor component patterns, SSR vs WASM guidance
  - [ ] `/dotnet/src/TechHub.Core/AGENTS.md` - Domain model design, repository interfaces
  - [ ] `/dotnet/src/TechHub.Infrastructure/AGENTS.md` - File-based data access, caching
  - [ ] `/dotnet/tests/AGENTS.md` - Testing strategy, test naming conventions
  - [ ] `/dotnet/infra/AGENTS.md` - Bicep modules, Azure resource patterns
  - [ ] `/dotnet/scripts/AGENTS.md` - PowerShell script conventions

- [ ] Update root `/AGENTS.md` to reference .NET documentation:

```markdown
## .NET Migration Documentation

When working on the .NET migration, refer to:
- **[/dotnet/AGENTS.md](dotnet/AGENTS.md)** - Root .NET development guide
- **[/dotnet/src/TechHub.Api/AGENTS.md](dotnet/src/TechHub.Api/AGENTS.md)** - API patterns
- **[/dotnet/src/TechHub.Web/AGENTS.md](dotnet/src/TechHub.Web/AGENTS.md)** - Blazor patterns
```

- [ ] Create `.github/agents/dotnet.md` (Custom Agent):

```markdown
# @dotnet Agent - .NET Development Expert

> **Purpose**: Framework-specific guidance for .NET/Blazor development in Tech Hub.

## Capabilities

This agent specializes in:
- ASP.NET Core Minimal API development
- Blazor Server-Side Rendering (SSR) and WebAssembly
- .NET Aspire orchestration
- C# patterns and best practices
- xUnit, bUnit, and Playwright testing
- Azure deployment with Bicep

## When to Use

Use `@dotnet` agent for:
- Creating new API endpoints
- Building Blazor components
- Writing C# domain models
- Setting up tests
- Infrastructure as Code with Bicep
- Debugging .NET issues

## Key Resources

- Solution root: `/dotnet/`
- Domain models: `/dotnet/src/TechHub.Core/`
- API: `/dotnet/src/TechHub.Api/`
- Frontend: `/dotnet/src/TechHub.Web/`
- Tests: `/dotnet/tests/`

## Commands

```powershell
# Start development server (both API + Web via Aspire)
dotnet run --project src/TechHub.AppHost

# Run all tests
dotnet test

# Run specific test project
dotnet test tests/TechHub.Core.Tests

# Run E2E tests
./scripts/run-e2e-tests.ps1

# Build release
dotnet build -c Release
```

## Patterns

### Minimal API Endpoints

```csharp
// Use static methods for endpoint handlers
public static class SectionEndpoints
{
    public static void MapSectionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/sections").WithTags("Sections");
        group.MapGet("/", GetAllSections);
        group.MapGet("/{url}", GetSectionByUrl);
    }
    
    private static async Task<IResult> GetAllSections(
        ISectionRepository repo, CancellationToken ct) =>
        Results.Ok(await repo.GetAllAsync(ct));
}
```

### Blazor Components

```razor
@* Use code-behind for complex components *@
@inherits SectionIndexBase

<PageTitle>@Section?.Title | Tech Hub</PageTitle>

@if (Section is not null)
{
    <SectionHeader Section="@Section" />
    <ContentList Items="@Items" />
}
```

See `/dotnet/AGENTS.md` for complete documentation structure.
~~~

- [ ] Verify all AGENTS.md files created and linked correctly

---

## Phase 1: Environment Setup

### 1.1 Create .NET DevContainer

- [ ] Create `/dotnet/.devcontainer/devcontainer.json`:

```jsonc
{
  "name": "Tech Hub .NET",
  "image": "mcr.microsoft.com/devcontainers/dotnet:1-10.0",
  "features": {
    "ghcr.io/devcontainers/features/powershell:1": {},
    "ghcr.io/devcontainers/features/azure-cli:1": {
      "installBicep": true
    },
    "ghcr.io/devcontainers/features/github-cli:1": {},
    "ghcr.io/azure/azure-dev/azd:latest": {},
    "ghcr.io/devcontainers/features/node:1": {
      "version": "lts"
    }
  },
  "customizations": {
    "vscode": {
      "extensions": [
        // .NET Development
        "ms-dotnettools.csharp",
        "ms-dotnettools.csdevkit",
        "ms-dotnettools.vscode-dotnet-runtime",
        "ms-dotnettools.dotnet-interactive-vscode",
        
        // Azure & Infrastructure
        "ms-azuretools.vscode-bicep",
        "ms-azuretools.vscode-azurecontainerapps",
        "ms-azuretools.vscode-azureresourcegroups",
        "ms-azuretools.vscode-azure-github-copilot",
        "ms-vscode.vscode-node-azure-pack",
        
        // GitHub & Copilot
        "github.copilot",
        "github.copilot-chat",
        "github.vscode-pull-request-github",
        
        // Testing & Quality
        "ms-playwright.playwright",
        "DavidAnson.vscode-markdownlint",
        "humao.rest-client",
        
        // PowerShell
        "ms-vscode.powershell"
      ],
      "settings": {
        // ==================== .NET Settings ====================
        "dotnet.defaultSolution": "TechHub.sln",
        "dotnet.server.useOmnisharp": false,
        "csharp.semanticHighlighting.enabled": true,
        
        // ==================== GitHub Copilot Settings ====================
        "chat.agent.maxRequests": 100,
        "chat.agent.thinking.collapsedTools": "off",
        "chat.useAgentSkills": true,
        "chat.useNestedAgentsMdFiles": true,
        "github.copilot.chat.agent.thinkingTool": true,
        "github.copilot.chat.codesearch.enabled": true,
        
        // ==================== Extension Settings ====================
        "playwright.reuseBrowser": true,
        "playwright.showTrace": true,
        "powershell.integratedConsole.showOnStartup": false,
        "terminal.integrated.defaultProfile.linux": "pwsh",
        
        "files.associations": {
          "*.ps1": "powershell",
          "*.bicep": "bicep",
          "*.json": "jsonc"
        }
      },
      "mcp": {
        "servers": {
          "github": {
            "type": "http",
            "url": "https://api.githubcopilot.com/mcp/"
          },
          "playwright": {
            "type": "stdio",
            "command": "npx",
            "args": [
              "-y",
              "@playwright/mcp@latest",
              "--vision",
              "--headless",
              "--isolated",
              "--no-sandbox"
            ]
          },
          "context7": {
            "type": "http",
            "url": "https://mcp.context7.com/mcp"
          }
        }
      }
    }
  },
  "postCreateCommand": ".devcontainer/post-create.ps1",
  "forwardPorts": [5000, 5001, 15888, 5173],
  "portsAttributes": {
    "5000": { "label": "API HTTP" },
    "5001": { "label": "API HTTPS" },
    "5173": { "label": "Blazor Dev Server" },
    "15888": { "label": "Aspire Dashboard" }
  }
}
```

- [ ] Create `/dotnet/.devcontainer/post-create.ps1`:

```powershell
#!/usr/bin/env pwsh
# Post-create setup script for Tech Hub .NET

$ErrorActionPreference = "Stop"

Write-Host "Setting up Tech Hub .NET development environment..." -ForegroundColor Cyan

# Ensure Node.js tools are available system-wide
Write-Host "Setting up Node.js tools system-wide..."
if (Test-Path "/usr/local/share/nvm/current/bin") {
    sudo ln -sf "/usr/local/share/nvm/current/bin/node" /usr/local/bin/node
    sudo ln -sf "/usr/local/share/nvm/current/bin/npm" /usr/local/bin/npm
    sudo ln -sf "/usr/local/share/nvm/current/bin/npx" /usr/local/bin/npx
}

# Update npm to latest version
Write-Host "Updating npm..."
npm install -g npm@latest

# Install PowerShell modules
Write-Host "Installing PowerShell modules..."
Install-Module Pester -Force -SkipPublisherCheck -MinimumVersion "5.0.0" -Scope CurrentUser

# Install .NET Aspire workload
Write-Host "Installing .NET Aspire workload..."
dotnet workload install aspire

# Install Entity Framework tools (if needed in future)
Write-Host "Installing .NET global tools..."
dotnet tool install --global dotnet-ef 2>$null
dotnet tool install --global dotnet-aspire 2>$null

# Install Playwright browsers for E2E tests and MCP
Write-Host "Installing Playwright system dependencies..."
sudo npx -y playwright install-deps

Write-Host "Installing Playwright browsers..."
npx -y playwright@latest install chromium chrome --force

# Restore .NET packages
Write-Host "Restoring .NET packages..."
dotnet restore

# Trust development certificates
Write-Host "Trusting development certificates..."
dotnet dev-certs https --trust 2>$null

Write-Host ""
Write-Host "=================================="
Write-Host "Development Environment Setup Complete!" -ForegroundColor Green
Write-Host "=================================="
Write-Host ""
Write-Host "Available commands:"
Write-Host "  dotnet run --project src/TechHub.AppHost  # Start with Aspire"
Write-Host "  dotnet test                               # Run all tests"
Write-Host "  ./scripts/run-e2e-tests.ps1              # Run E2E tests"
Write-Host ""
```

- [ ] Verify devcontainer builds successfully
- [ ] Verify MCP servers are accessible (GitHub, Context7, Playwright)
- [ ] Confirm Playwright browsers installed correctly

### 1.2 Configure VS Code Debugging

- [ ] Create `/dotnet/.vscode/launch.json`:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Launch Aspire AppHost",
      "type": "dotnet",
      "request": "launch",
      "projectPath": "${workspaceFolder}/src/TechHub.AppHost/TechHub.AppHost.csproj"
    },
    {
      "name": "Launch API",
      "type": "dotnet",
      "request": "launch",
      "projectPath": "${workspaceFolder}/src/TechHub.Api/TechHub.Api.csproj",
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ASPNETCORE_URLS": "https://localhost:5001;http://localhost:5000"
      }
    },
    {
      "name": "Launch Web (Blazor)",
      "type": "dotnet",
      "request": "launch",
      "projectPath": "${workspaceFolder}/src/TechHub.Web/TechHub.Web.csproj",
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    {
      "name": "Debug E2E Tests",
      "type": "node",
      "request": "launch",
      "program": "${workspaceFolder}/node_modules/.bin/playwright",
      "args": ["test", "--debug"],
      "cwd": "${workspaceFolder}/tests/TechHub.E2E.Tests",
      "console": "integratedTerminal"
    }
  ],
  "compounds": [
    {
      "name": "Launch API + Web",
      "configurations": ["Launch API", "Launch Web (Blazor)"],
      "stopAll": true
    }
  ]
}
```

- [ ] Create `/dotnet/.vscode/tasks.json`:

```json
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "build",
      "command": "dotnet",
      "type": "process",
      "args": ["build", "${workspaceFolder}/TechHub.sln"],
      "problemMatcher": "$msCompile",
      "group": {
        "kind": "build",
        "isDefault": true
      }
    },
    {
      "label": "test",
      "command": "dotnet",
      "type": "process",
      "args": ["test", "${workspaceFolder}/TechHub.sln"],
      "problemMatcher": "$msCompile",
      "group": "test"
    },
    {
      "label": "watch",
      "command": "dotnet",
      "type": "process",
      "args": ["watch", "run", "--project", "src/TechHub.AppHost"],
      "problemMatcher": "$msCompile",
      "isBackground": true
    },
    {
      "label": "run-e2e-tests",
      "type": "shell",
      "command": "./scripts/run-e2e-tests.ps1",
      "problemMatcher": [],
      "group": "test"
    }
  ]
}
```

- [ ] Create `/dotnet/.vscode/settings.json`:

```json
{
  "dotnet.defaultSolution": "TechHub.sln",
  "editor.formatOnSave": true,
  "editor.codeActionsOnSave": {
    "source.organizeImports": "explicit",
    "source.fixAll": "explicit"
  },
  "[csharp]": {
    "editor.defaultFormatter": "ms-dotnettools.csharp"
  },
  "files.exclude": {
    "**/bin": true,
    "**/obj": true,
    "**/.vs": true
  },
  "search.exclude": {
    "**/bin": true,
    "**/obj": true,
    "**/node_modules": true
  }
}
```

- [ ] Verify F5 debugging works for each configuration
- [ ] Verify breakpoints work in C# code
- [ ] Verify Aspire dashboard opens automatically

### 1.3 Initialize Solution Structure

- [ ] Create solution at `/dotnet/TechHub.sln`:

```text
dotnet/
├── .devcontainer/
├── src/
│   ├── TechHub.Web/              # Blazor frontend application
│   ├── TechHub.Api/              # REST API backend
│   ├── TechHub.Core/             # Domain models and interfaces
│   ├── TechHub.Infrastructure/   # Data access implementations
│   ├── TechHub.ServiceDefaults/  # Shared Aspire service configuration
│   └── TechHub.AppHost/          # .NET Aspire orchestration
├── tests/
│   ├── TechHub.Core.Tests/       # Unit tests
│   ├── TechHub.Api.Tests/        # API integration tests
│   ├── TechHub.Infrastructure.Tests/
│   ├── TechHub.Web.Tests/        # bUnit component tests
│   └── TechHub.E2E.Tests/        # Playwright E2E tests
├── infra/
│   ├── main.bicep                # Main infrastructure
│   ├── modules/                  # Bicep modules
│   └── parameters/               # Environment parameters
├── scripts/
│   ├── build.ps1
│   ├── test.ps1
│   └── deploy.ps1
└── specs/
    ├── .speckit/
    └── features/
```

- [ ] Run `dotnet new sln -n TechHub` in `/dotnet/`
- [ ] Create projects:
  - [ ] `dotnet new classlib -n TechHub.Core -o src/TechHub.Core`
  - [ ] `dotnet new classlib -n TechHub.Infrastructure -o src/TechHub.Infrastructure`
  - [ ] `dotnet new webapi -n TechHub.Api -o src/TechHub.Api` (Minimal API)
  - [ ] `dotnet new blazor -n TechHub.Web -o src/TechHub.Web` (Blazor Web App)
  - [ ] `dotnet new aspire-servicedefaults -n TechHub.ServiceDefaults -o src/TechHub.ServiceDefaults`
  - [ ] `dotnet new aspire-apphost -n TechHub.AppHost -o src/TechHub.AppHost`
- [ ] Add all projects to solution
- [ ] Set up project references:
  - [ ] Api → Core, Infrastructure
  - [ ] Web → Core (for shared DTOs only)
  - [ ] Infrastructure → Core
  - [ ] AppHost → Api, Web, ServiceDefaults
- [ ] Verify solution builds

### 1.4 Configure Aspire

- [ ] Create `TechHub.AppHost` project:

```csharp
// Program.cs
var builder = DistributedApplication.CreateBuilder(args);

// API backend
var api = builder.AddProject<Projects.TechHub_Api>("api")
    .WithExternalHttpEndpoints();

// Blazor frontend - references API
var web = builder.AddProject<Projects.TechHub_Web>("web")
    .WithReference(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
```

- [ ] Configure ServiceDefaults with OpenTelemetry:

```csharp
// TechHub.ServiceDefaults/Extensions.cs
public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.ConfigureOpenTelemetry();
        builder.AddDefaultHealthChecks();
        builder.Services.AddServiceDiscovery();
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
            http.AddServiceDiscovery();
        });
        return builder;
    }
}
```

- [ ] Configure OpenTelemetry in AppHost
- [ ] Verify Aspire dashboard accessible at port 15888
- [ ] Verify both API and Web projects start correctly

---

## Phase 2: Core Architecture

### 2.1 Define Domain Models

- [ ] Create `/specs/features/domain-models.md` specification
- [ ] Implement in `TechHub.Core`:

```csharp
// Models/Section.cs
public record Section
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required string Category { get; init; }
    public required string Image { get; init; }
    public required IReadOnlyList<CollectionReference> Collections { get; init; }
}

// Models/CollectionReference.cs
public record CollectionReference
{
    public required string Title { get; init; }
    public required string Url { get; init; }
    public required string Collection { get; init; }
    public required string Description { get; init; }
    public bool IsCustom { get; init; }
}

// Models/ContentItem.cs
public record ContentItem
{
    public required string Id { get; init; }           // Unique content ID (slug)
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string CanonicalUrl { get; init; } // Primary URL for SEO
    public required string Author { get; init; }
    public required long DateEpoch { get; init; }      // Unix timestamp
    public required string Collection { get; init; }   // Primary collection (news, blogs, etc.)
    public required IReadOnlyList<string> Categories { get; init; }  // All categories this belongs to
    public required IReadOnlyList<string> Tags { get; init; }
    public required string Content { get; init; }      // Rendered HTML
    public required string Excerpt { get; init; }
    public string? ExternalUrl { get; init; }
    public string? VideoId { get; init; }
    public string? AltCollection { get; init; }
    
    /// <summary>
    /// Generate URL for this content in a specific section context.
    /// Example: /ai/videos/vs-code-107.html or /github-copilot/videos/vs-code-107.html
    /// </summary>
    public string GetUrlInSection(string sectionUrl) => 
        $"/{sectionUrl}/{Collection}/{Id}.html";
}

// DTOs/ContentItemDto.cs (for API responses)
public record ContentItemDto
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }          // Context-specific URL
    public required string CanonicalUrl { get; init; } // For SEO <link rel="canonical">
    public required string Author { get; init; }
    public required string Date { get; init; }         // Formatted display date
    public required string DateIso { get; init; }      // ISO 8601 for <time>
    public required long DateEpoch { get; init; }      // For client-side operations
    public required string Collection { get; init; }
    public required IReadOnlyList<string> Categories { get; init; }
    public required IReadOnlyList<string> Tags { get; init; }
    public string? ExternalUrl { get; init; }
    public string? VideoId { get; init; }
}
```

- [ ] Write unit tests for models
- [ ] Verify all tests pass

### 2.2 Implement Repository Pattern

- [ ] Create `/specs/features/repository-pattern.md` specification
- [ ] Define interfaces in `TechHub.Core`:

```csharp
// Interfaces/ISectionRepository.cs
public interface ISectionRepository
{
    Task<IReadOnlyList<Section>> GetAllSectionsAsync(CancellationToken ct = default);
    Task<Section?> GetSectionByIdAsync(string id, CancellationToken ct = default);
    Task<Section?> GetSectionByUrlAsync(string url, CancellationToken ct = default);
    
    /// <summary>
    /// Check if a section contains content with the given category.
    /// Used for validating multi-location content URLs.
    /// </summary>
    Task<bool> SectionContainsCategoryAsync(string sectionUrl, string category, CancellationToken ct = default);
}

// Interfaces/IContentRepository.cs
public interface IContentRepository
{
    Task<IReadOnlyList<ContentItem>> GetItemsByCollectionAsync(
        string collection, 
        CancellationToken ct = default);
    
    Task<IReadOnlyList<ContentItem>> GetItemsByCategoryAsync(
        string category, 
        CancellationToken ct = default);
    
    /// <summary>
    /// Get content by ID, optionally filtered to a specific category.
    /// Supports multi-location access: same content viewable from different sections.
    /// </summary>
    Task<ContentItem?> GetItemByIdAsync(
        string id, 
        string? categoryFilter = null,
        CancellationToken ct = default);
    
    Task<IReadOnlyList<ContentItem>> GetLatestItemsAsync(
        int count, 
        string? category = null,
        CancellationToken ct = default);
    
    /// <summary>
    /// Get items by collection within a specific section context.
    /// </summary>
    Task<IReadOnlyList<ContentItem>> GetItemsByCollectionInSectionAsync(
        string collection,
        string sectionCategory,
        CancellationToken ct = default);
}
```

- [ ] Implement file-based repository in `TechHub.Infrastructure`:

```csharp
// Repositories/FileSectionRepository.cs
public class FileSectionRepository : ISectionRepository
{
    private readonly string _sectionsJsonPath;
    private readonly IMemoryCache _cache;
    private readonly TimeProvider _timeProvider;
    
    // Implementation reads from _data/sections.json
    // Caches parsed sections for performance
}

// Repositories/FileContentRepository.cs  
public class FileContentRepository : IContentRepository
{
    private readonly string _collectionsPath;
    private readonly IMarkdownProcessor _markdownProcessor;
    private readonly IMemoryCache _cache;
    
    // Implementation reads from collections/ markdown files
    // Content items track ALL categories they belong to
    // Supports multi-location access via category filtering
}
```

- [ ] Write integration tests for repositories
- [ ] Verify all tests pass

### 2.3 Implement Markdown Processing

- [ ] Create `/specs/features/markdown-processing.md` specification
- [ ] Install Markdig package: `dotnet add package Markdig`
- [ ] Implement markdown processor:

```csharp
// Services/MarkdownProcessor.cs
public class MarkdownProcessor : IMarkdownProcessor
{
    private readonly MarkdownPipeline _pipeline;
    
    public MarkdownProcessor()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseYamlFrontMatter()
            .UseSyntaxHighlighting()
            .Build();
    }
    
    public ContentItem ParseMarkdownFile(string filePath)
    {
        // Parse YAML front matter
        // Extract excerpt (before <!--excerpt_end-->)
        // Render markdown to HTML
        // Return ContentItem
    }
}
```

- [ ] Write unit tests for markdown processing
- [ ] Test with actual content files from collections/
- [ ] Verify all tests pass

### 2.4 Configure API Project

- [ ] Set up `TechHub.Api/Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (OpenTelemetry, health checks, resilience)
builder.AddServiceDefaults();

// Add services
builder.Services.AddMemoryCache();
builder.Services.AddSingleton(TimeProvider.System);

// Repository registration
builder.Services.AddSingleton<ISectionRepository, FileSectionRepository>();
builder.Services.AddSingleton<IContentRepository, FileContentRepository>();
builder.Services.AddSingleton<IMarkdownProcessor, MarkdownProcessor>();

// Configuration
builder.Services.Configure<ContentOptions>(
    builder.Configuration.GetSection("Content"));

// API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// CORS for Blazor frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [])
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();
app.MapDefaultEndpoints(); // Health checks from ServiceDefaults

// Map API endpoints (see Phase 2.5)
app.MapSectionEndpoints();
app.MapContentEndpoints();
app.MapRssEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "TechHub API"));
}

app.Run();
```

- [ ] Create `ContentOptions` configuration class
- [ ] Add configuration to `appsettings.json`

### 2.5 Configure Web Project (Blazor Frontend)

- [ ] Set up `TechHub.Web/Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add Blazor services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Configure typed HttpClient for API calls
builder.Services.AddHttpClient<ITechHubApiClient, TechHubApiClient>(client =>
{
    // Service discovery via Aspire - "api" is the resource name in AppHost
    client.BaseAddress = new Uri("https+http://api");
})
.AddStandardResilienceHandler();

// Configuration
builder.Services.Configure<WebOptions>(
    builder.Configuration.GetSection("Web"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.MapDefaultEndpoints();

app.Run();
```

- [ ] Create typed API client interface and implementation:

```csharp
// Services/ITechHubApiClient.cs
public interface ITechHubApiClient
{
    Task<IReadOnlyList<SectionDto>> GetSectionsAsync(CancellationToken ct = default);
    Task<SectionDto?> GetSectionAsync(string url, CancellationToken ct = default);
    Task<IReadOnlyList<ContentItemDto>> GetContentAsync(
        string sectionUrl, 
        string? collection = null, 
        CancellationToken ct = default);
    Task<ContentItemDto?> GetContentItemAsync(
        string sectionUrl, 
        string collection, 
        string itemId, 
        CancellationToken ct = default);
    Task<IReadOnlyList<ContentItemDto>> GetLatestRoundupsAsync(
        int count, 
        CancellationToken ct = default);
}
```

- [ ] Verify DI works correctly
- [ ] Verify API client can communicate with API

### 2.6 Implement API Endpoints

- [ ] Create `/specs/features/api-endpoints.md` specification
- [ ] Implement section endpoints:

```csharp
// Endpoints/SectionEndpoints.cs
public static class SectionEndpoints
{
    public static void MapSectionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/sections")
            .WithTags("Sections")
            .WithOpenApi();
        
        group.MapGet("/", async (ISectionRepository repo, CancellationToken ct) =>
            await repo.GetAllSectionsAsync(ct));
        
        group.MapGet("/{url}", async (string url, ISectionRepository repo, CancellationToken ct) =>
            await repo.GetSectionByUrlAsync(url, ct) is { } section
                ? Results.Ok(section)
                : Results.NotFound());
    }
}
```

- [ ] Implement content endpoints:

```csharp
// Endpoints/ContentEndpoints.cs
public static class ContentEndpoints
{
    public static void MapContentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/content")
            .WithTags("Content")
            .WithOpenApi();
        
        // Get content for a section (all collections)
        group.MapGet("/section/{sectionUrl}", async (
            string sectionUrl,
            [FromQuery] string? collection,
            [FromQuery] int? limit,
            IContentRepository repo,
            ISectionRepository sectionRepo,
            CancellationToken ct) =>
        {
            var section = await sectionRepo.GetSectionByUrlAsync(sectionUrl, ct);
            if (section is null) return Results.NotFound();
            
            var items = collection is not null
                ? await repo.GetItemsByCollectionInSectionAsync(collection, section.Category, ct)
                : await repo.GetItemsByCategoryAsync(section.Category, ct);
            
            return Results.Ok(items.Select(i => i.ToDto(sectionUrl)));
        });
        
        // Get specific content item in section context
        // Supports multi-location: /api/content/ai/videos/vs-code-107
        group.MapGet("/{sectionUrl}/{collection}/{itemId}", async (
            string sectionUrl,
            string collection,
            string itemId,
            IContentRepository repo,
            ISectionRepository sectionRepo,
            CancellationToken ct) =>
        {
            var section = await sectionRepo.GetSectionByUrlAsync(sectionUrl, ct);
            if (section is null) return Results.NotFound();
            
            var item = await repo.GetItemByIdAsync(itemId, section.Category, ct);
            if (item is null || item.Collection != collection) return Results.NotFound();
            
            return Results.Ok(item.ToDetailDto(sectionUrl));
        });
        
        // Get latest roundups
        group.MapGet("/roundups/latest", async (
            [FromQuery] int count,
            IContentRepository repo,
            CancellationToken ct) =>
        {
            var items = await repo.GetLatestItemsAsync(count, category: null, ct);
            return items.Where(i => i.Collection == "roundups").Take(count);
        });
    }
}
```

- [ ] Implement RSS endpoints:

```csharp
// Endpoints/RssEndpoints.cs
public static class RssEndpoints
{
    public static void MapRssEndpoints(this WebApplication app)
    {
        app.MapGet("/api/rss/{sectionUrl}", async (
            string sectionUrl,
            IRssGenerator rssGenerator,
            CancellationToken ct) =>
        {
            var rss = await rssGenerator.GenerateSectionFeedAsync(sectionUrl, ct);
            return Results.Content(rss, "application/rss+xml");
        }).WithTags("RSS");
        
        app.MapGet("/api/rss", async (IRssGenerator rssGenerator, CancellationToken ct) =>
        {
            var rss = await rssGenerator.GenerateAllFeedAsync(ct);
            return Results.Content(rss, "application/rss+xml");
        }).WithTags("RSS");
    }
}
```

- [ ] Write API integration tests
- [ ] Verify all endpoints return correct data
- [ ] Verify OpenAPI documentation is accurate

### 2.7 MCP Server Preparation

> **FUTURE CAPABILITY**: This section documents how to extend the API to support Model Context Protocol (MCP).

The API is designed to be MCP-ready. MCP enables AI assistants to interact with web content programmatically.

- [ ] Create `/specs/features/mcp-server.md` specification documenting:
  - MCP transport options (stdio for local, HTTP+SSE for remote)
  - Resource URIs for content (`techhub://sections`, `techhub://content/{id}`)
  - Tool definitions for search and filtering
  - Schema.org alignment for semantic understanding

**MCP Extension Points** (to implement when needed):

```csharp
// Future: MCP/McpServerExtensions.cs
public static class McpServerExtensions
{
    public static void MapMcpEndpoints(this WebApplication app)
    {
        // MCP uses Server-Sent Events for streaming responses
        app.MapGet("/mcp/sse", async (HttpContext context, CancellationToken ct) =>
        {
            context.Response.Headers.Append("Content-Type", "text/event-stream");
            context.Response.Headers.Append("Cache-Control", "no-cache");
            
            // Handle MCP protocol messages
            // - resources/list: Return available content resources
            // - resources/read: Return content by URI
            // - tools/list: Return available search/filter tools
            // - tools/call: Execute search or filter operations
        });
    }
}

// Future: MCP/Resources/ContentResource.cs
public class ContentResource : IMcpResource
{
    public string Uri => "techhub://content";
    public string Name => "Tech Hub Content";
    public string Description => "Microsoft technology content including news, blogs, videos";
    public string MimeType => "application/json";
    
    // Returns Schema.org-aligned JSON-LD for AI consumption
}
```

**Design Decisions for MCP Compatibility**:

1. **Resource-oriented API** - Content accessible via stable URIs
2. **Schema.org metadata** - All responses include structured data
3. **Stateless design** - No session required for content access
4. **Streaming support** - API designed to support SSE for MCP transport

- [ ] Document MCP extension points in specifications
- [ ] Ensure API responses include Schema.org metadata
- [ ] Verify API supports required HTTP features (SSE-ready)

### 2.8 Authentication Preparation

> **FUTURE CAPABILITY**: This section documents how to add authentication with IdentityServer/Duende.

The architecture supports adding authentication without major refactoring:

- [ ] Create `/specs/features/authentication.md` specification documenting:
  - BFF (Backend-for-Frontend) pattern for Blazor
  - Token-based authentication for API
  - Integration points for IdentityServer/Duende
  - User claims and authorization policies

**Authentication Extension Points** (to implement when needed):

```csharp
// Future: TechHub.Api/Program.cs additions
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Identity:Authority"];
        options.Audience = "techhub-api";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => 
        policy.RequireClaim("role", "admin"));
    options.AddPolicy("ContentEditor", policy => 
        policy.RequireClaim("role", "editor", "admin"));
});

// Future: TechHub.Web/Program.cs additions (BFF pattern)
builder.Services.AddBff()
    .AddRemoteApis();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration["Identity:Authority"];
    options.ClientId = "techhub-web";
    // ... additional OIDC configuration
});
```

**Design Decisions for Auth Compatibility**:

1. **Claims-based authorization** - API endpoints use policy-based auth
2. **BFF pattern** - Frontend uses secure cookie auth, API uses JWT
3. **Separation of concerns** - Auth logic isolated for easy integration
4. **No auth required initially** - All content is public by default

- [ ] Document authentication extension points
- [ ] Ensure API endpoints can accept `[Authorize]` attribute
- [ ] Verify middleware pipeline supports auth insertion

---

## Phase 3: Content System

### 3.1 Implement Blazor Components

- [ ] Create `/specs/features/blazor-components.md` specification

#### Layout Component

- [ ] Create `TechHub.Web/Components/Layout/MainLayout.razor`:

```razor
@inherits LayoutComponentBase

<!DOCTYPE html>
<html lang="en">
<head>
    <HeadOutlet />
</head>
<body>
    <Header />
    @Body
    <Footer />
    <script src="_framework/blazor.web.js"></script>
</body>
</html>
```

#### Header Component

- [ ] Create `Components/Shared/Header.razor` with semantic HTML:

```razor
<header role="banner">
    <div class="header-container">
        <a href="/" class="logo" aria-label="Tech Hub Home">
            <span class="logo-text">TH</span>
        </a>
        <nav role="navigation" aria-label="Main navigation">
            <button class="menu-toggle" aria-expanded="false" aria-controls="main-menu">
                <img src="/assets/menu.svg" alt="Menu" />
            </button>
            <!-- Navigation items -->
        </nav>
    </div>
</header>
```

#### Section Navigation Component

- [ ] Create `Components/Shared/SectionNav.razor`:

```razor
@* Section navigation - links use section-specific URLs *@

<nav class="section-nav" aria-label="Section navigation">
    @foreach (var collection in Section.Collections)
    {
        <a href="/@SectionUrl/@(collection.Collection).html" 
           class="@(IsActive(collection.Collection) ? "active" : "")">
            @collection.Title
        </a>
    }
</nav>

@code {
    [Parameter] public required SectionDto Section { get; set; }
    [Parameter] public required string SectionUrl { get; set; }
    [Parameter] public string? ActiveCollection { get; set; }
    
    private bool IsActive(string collection) => 
        string.Equals(ActiveCollection, collection, StringComparison.OrdinalIgnoreCase);
}
```

#### Content Item Card Component

- [ ] Create `Components/Content/ItemCard.razor` with Schema.org:

```razor
@* Item card - URL includes section context from API response *@

<article class="item-card" 
         itemscope 
         itemtype="https://schema.org/Article">
    <a href="@Item.Url" class="item-link">
        <div class="item-content">
            <h3 itemprop="headline">@Item.Title</h3>
            <p itemprop="description">@Item.Description</p>
            <div class="item-meta">
                <span itemprop="author">@Item.Author</span>
                <time datetime="@Item.DateIso" itemprop="datePublished">
                    @Item.Date
                </time>
            </div>
        </div>
    </a>
</article>

@code {
    [Parameter] public required ContentItemDto Item { get; set; }
}
```

- [ ] Write bUnit tests for each component
- [ ] Verify all tests pass

### 3.2 Implement Page Components

#### Home Page

- [ ] Create `Components/Pages/Home.razor`:

```razor
@page "/"
@inject ITechHubApiClient ApiClient

<PageTitle>Tech Hub | All Microsoft tech content in one place.</PageTitle>

<HeadContent>
    <meta name="description" content="Your one-stop hub for Microsoft technology content..." />
    <script type="application/ld+json">@GetStructuredData()</script>
</HeadContent>

<main id="content" role="main">
    <section class="welcome" aria-labelledby="welcome-heading">
        <h1 id="welcome-heading">Welcome to Tech Hub</h1>
        <p>Your one-stop hub for Microsoft technology content...</p>
    </section>
    
    <section class="roundups" aria-labelledby="roundups-heading">
        <h2 id="roundups-heading">Last 4 Roundups</h2>
        @foreach (var roundup in Roundups)
        {
            <a href="/roundups/@roundup.Id.html">@roundup.Title</a>
        }
    </section>
    
    <section class="categories" aria-labelledby="categories-heading">
        <h2 id="categories-heading">Content per category</h2>
        <div class="category-grid">
            @foreach (var section in Sections)
            {
                <SectionCard Section="@section" />
            }
        </div>
    </section>
</main>

@code {
    private IReadOnlyList<SectionDto> Sections { get; set; } = [];
    private IReadOnlyList<ContentItemDto> Roundups { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        Sections = await ApiClient.GetSectionsAsync();
        Roundups = await ApiClient.GetLatestRoundupsAsync(4);
    }
}
```

- [ ] Write bUnit tests for all pages
- [ ] Verify all tests pass

### 3.3 Implement Filtering System

> **Architecture**: Filtering is performed client-side in Blazor WebAssembly after all content is loaded from the API. This enables instant filtering without server round-trips.

- [ ] Create `/specs/features/filtering-implementation.md` specification
- [ ] Reference current filtering behavior from `docs/filtering-system.md`

#### Filter State Component

- [ ] Create `Components/Filters/FilterState.razor`:

```razor
@* Manages client-side filter state synchronized with URL *@
@inject NavigationManager Navigation
@implements IDisposable

@code {
    [Parameter] public IReadOnlyList<ContentItemDto> AllItems { get; set; } = [];
    [Parameter] public EventCallback<IReadOnlyList<ContentItemDto>> FilteredItemsChanged { get; set; }
    
    private string? SearchText;
    private string? DateFilter;
    private HashSet<string> ActiveCollections = new();
    private HashSet<string> ActiveTags = new();
    
    protected override void OnInitialized()
    {
        Navigation.LocationChanged += OnLocationChanged;
        ParseUrlParameters();
    }
    
    private void ParseUrlParameters()
    {
        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        
        SearchText = query["search"];
        DateFilter = query["date"];
        // Parse collections and tags from URL
        
        ApplyFilters();
    }
    
    private void ApplyFilters()
    {
        var filtered = AllItems.AsEnumerable();
        
        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(i => MatchesSearch(i, SearchText));
        
        if (!string.IsNullOrWhiteSpace(DateFilter))
            filtered = ApplyDateFilter(filtered, DateFilter);
        
        if (ActiveCollections.Count > 0)
            filtered = filtered.Where(i => ActiveCollections.Contains(i.Collection));
        
        if (ActiveTags.Count > 0)
            filtered = filtered.Where(i => i.Tags.Any(t => ActiveTags.Contains(t)));
        
        FilteredItemsChanged.InvokeAsync(filtered.ToList());
    }
    
    private void UpdateUrl()
    {
        // Build query string from current filter state
        // Navigate without reloading page
        var queryParams = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(SearchText)) queryParams["search"] = SearchText;
        if (!string.IsNullOrWhiteSpace(DateFilter)) queryParams["date"] = DateFilter;
        // ... add collections and tags
        
        var newUri = Navigation.GetUriWithQueryParameters(queryParams);
        Navigation.NavigateTo(newUri, forceLoad: false);
    }
    
    public void Dispose() => Navigation.LocationChanged -= OnLocationChanged;
}
```

#### Filter Controls Component

- [ ] Create `Components/Filters/FilterControls.razor`:

```razor
<div class="filter-controls" role="search">
    <button class="clear-all" @onclick="ClearFilters">Clear All</button>
    
    <input type="search" 
           placeholder="Search" 
           @bind="SearchText" 
           @bind:event="oninput"
           aria-label="Search content" />
    
    <div class="date-filters" role="group" aria-label="Date filters">
        @foreach (var dateFilter in DateFilters)
        {
            <FilterButton 
                Label="@dateFilter.Label" 
                Count="@dateFilter.Count"
                IsActive="@(ActiveDateFilter == dateFilter.Value)"
                OnClick="@(() => SetDateFilter(dateFilter.Value))" />
        }
    </div>
    
    <div class="collection-filters" role="group" aria-label="Collection filters">
        @foreach (var collection in CollectionFilters)
        {
            <FilterButton 
                Label="@collection.Label" 
                Count="@collection.Count"
                IsActive="@ActiveCollections.Contains(collection.Value)"
                OnClick="@(() => ToggleCollection(collection.Value))" />
        }
    </div>
</div>
```

- [ ] Implement JavaScript interop for URL parameter sync
- [ ] Implement debounced text search
- [ ] Implement the "20 + Same-Day" rule for date filtering
- [ ] Write unit tests for filtering logic
- [ ] Write E2E tests for filtering behavior
- [ ] Verify all tests pass

### 3.4 Implement RSS Feeds

- [ ] Create `/specs/features/rss-implementation.md` specification
- [ ] Create RSS controller:

```csharp
// Controllers/RssController.cs
[Route("{section}/feed.xml")]
[Route("feed.xml")]
public class RssController : Controller
{
    [HttpGet]
    [Produces("application/rss+xml")]
    public async Task<IActionResult> GetFeed(string? section = null)
    {
        // Generate RSS 2.0 feed
        // Include all required elements
        // Use proper date formatting
    }
}
```

- [ ] Implement RSS generation service
- [ ] Preserve all existing feed URLs
- [ ] Write integration tests for RSS feeds
- [ ] Verify all tests pass

---

## Phase 4: Features Implementation

### 4.1 Implement Styling

- [ ] Create `/specs/features/styling.md` specification
- [ ] Port SCSS to CSS (or keep SCSS with build step):

```text
TechHub.Web/wwwroot/css/
├── base.css          # Reset and base styles
├── colors.css        # Color variables (CSS custom properties)
├── layout.css        # Grid and flex layouts
├── navigation.css    # Header, nav, section nav
├── filters.css       # Filter controls
├── cards.css         # Content cards
├── responsive.css    # Media queries
└── main.css          # Import all
```

- [ ] Match all existing visual styles exactly
- [ ] Test on mobile, tablet, and desktop viewports
- [ ] Verify color contrast meets WCAG AA (4.5:1 minimum)
- [ ] Test with keyboard navigation
- [ ] Test with screen reader

### 4.2 Implement SEO Features

- [ ] Create `/specs/features/seo-implementation.md` specification
- [ ] Implement Schema.org structured data:

```csharp
// Services/StructuredDataService.cs
public class StructuredDataService
{
    public string GenerateWebsiteSchema() => JsonSerializer.Serialize(new
    {
        @context = "https://schema.org",
        @type = "WebSite",
        name = "Tech Hub",
        url = "https://tech.hub.ms",
        description = "Your one-stop hub for Microsoft technology content"
    });
    
    public string GenerateArticleSchema(ContentItem item) => JsonSerializer.Serialize(new
    {
        @context = "https://schema.org",
        @type = "Article",
        headline = item.Title,
        description = item.Description,
        author = new { @type = "Person", name = item.Author },
        datePublished = DateTimeOffset.FromUnixTimeSeconds(item.DateEpoch).ToString("O")
    });
}
```

- [ ] Implement sitemap generation
- [ ] Implement robots.txt
- [ ] Implement canonical URLs
- [ ] Implement Open Graph meta tags
- [ ] Write tests for structured data generation
- [ ] Verify all tests pass

### 4.3 Implement Performance Optimizations

- [ ] Create `/specs/features/performance.md` specification
- [ ] Implement response caching:

```csharp
// In Program.cs
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromMinutes(10)));
    options.AddPolicy("ContentPage", builder => 
        builder.Expire(TimeSpan.FromHours(1)));
    options.AddPolicy("RssFeed", builder => 
        builder.Expire(TimeSpan.FromMinutes(30)));
});
```

- [ ] Implement static file caching headers
- [ ] Implement content compression
- [ ] Implement lazy loading for images
- [ ] Measure and optimize Core Web Vitals
- [ ] Write performance benchmarks
- [ ] Verify performance targets met

### 4.4 Implement Resilience

- [ ] Create `/specs/features/resilience.md` specification
- [ ] Add Microsoft.Extensions.Resilience package
- [ ] Implement retry policies for external requests:

```csharp
builder.Services.AddHttpClient("ExternalContent")
    .AddStandardResilienceHandler();
```

- [ ] Implement circuit breaker for dependencies
- [ ] Implement graceful degradation for external links
- [ ] Write tests for resilience behavior
- [ ] Verify all tests pass

---

## Phase 5: Testing & Validation

### 5.1 Unit Testing Strategy

- [ ] Create `/specs/testing/unit-tests.md` specification
- [ ] Test coverage targets:
  - Domain models: 100%
  - Services: 90%
  - Repositories: 90%
  - Utilities: 95%

- [ ] Write unit tests for:
  - [ ] Domain models and validation
  - [ ] Markdown processor
  - [ ] Date/timezone utilities
  - [ ] RSS generation
  - [ ] Structured data generation
  - [ ] Filter logic

### 5.2 Integration Testing Strategy

- [ ] Create `/specs/testing/integration-tests.md` specification
- [ ] Use WebApplicationFactory (TestServer):

```csharp
public class TechHubWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace services with test doubles if needed
        });
    }
}

public class ContentTests : IClassFixture<TechHubWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    [Fact]
    public async Task HomePage_Returns200_AndContainsSections()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("GitHub Copilot", content);
    }
}
```

- [ ] Write integration tests for:
  - [ ] All page routes return 200
  - [ ] All RSS feeds are valid XML
  - [ ] Filtering works correctly
  - [ ] URL parameters are handled correctly
  - [ ] All pages render correctly with new URL structure

### 5.3 E2E Testing Strategy

- [ ] Create `/specs/testing/e2e-tests.md` specification
- [ ] Configure Playwright:

```csharp
// TechHub.E2E.Tests/PlaywrightFixture.cs
public class PlaywrightFixture : IAsyncLifetime
{
    public IBrowser Browser { get; private set; } = null!;
    public IPage Page { get; private set; } = null!;
    
    public async Task InitializeAsync()
    {
        var playwright = await Playwright.CreateAsync();
        Browser = await playwright.Chromium.LaunchAsync();
        Page = await Browser.NewPageAsync();
    }
}
```

- [ ] Write E2E tests for:
  - [ ] Navigation flows
  - [ ] Filtering interactions
  - [ ] Mobile responsive behavior
  - [ ] Keyboard navigation
  - [ ] Screen reader accessibility

### 5.4 Visual Regression Testing

- [ ] Create `/specs/testing/visual-regression.md` specification
- [ ] Capture baseline screenshots of Jekyll site
- [ ] Compare .NET site against baselines
- [ ] Document and approve any intentional differences
- [ ] Verify visual parity

---

## Phase 6: Azure Infrastructure

### 6.1 Create Bicep Infrastructure

- [ ] Create `/specs/infrastructure/azure-resources.md` specification
- [ ] Create `/dotnet/infra/main.bicep`:

```bicep
targetScope = 'subscription'

@description('Environment name')
param environmentName string

@description('Primary location')
param location string = 'westeurope'

// Resource group
resource rg 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: 'rg-techhub-${environmentName}'
  location: location
}

// Virtual Network
module vnet 'modules/vnet.bicep' = {
  name: 'vnet'
  scope: rg
  params: {
    name: 'vnet-techhub-${environmentName}'
    location: location
  }
}

// Container Apps Environment
module containerAppsEnv 'modules/container-apps-env.bicep' = {
  name: 'container-apps-env'
  scope: rg
  params: {
    name: 'cae-techhub-${environmentName}'
    location: location
    vnetSubnetId: vnet.outputs.containerAppsSubnetId
    logAnalyticsWorkspaceId: monitoring.outputs.workspaceId
  }
}

// Container App
module containerApp 'modules/container-app.bicep' = {
  name: 'container-app'
  scope: rg
  params: {
    name: 'ca-techhub-${environmentName}'
    location: location
    containerAppsEnvironmentId: containerAppsEnv.outputs.id
    imageName: 'techhub-web:latest'
  }
}

// Application Insights
module monitoring 'modules/monitoring.bicep' = {
  name: 'monitoring'
  scope: rg
  params: {
    name: 'appi-techhub-${environmentName}'
    location: location
  }
}
```

- [ ] Create VNet module with subnets
- [ ] Create Container Apps Environment module
- [ ] Create Container App module
- [ ] Create Application Insights module
- [ ] Create Key Vault module for secrets
- [ ] Validate Bicep files compile
- [ ] Deploy to test environment

### 6.2 Configure Networking

- [ ] Create `/specs/infrastructure/networking.md` specification
- [ ] Define VNet address space
- [ ] Create subnets:
  - [ ] Container Apps subnet
  - [ ] Private endpoints subnet
- [ ] Configure private endpoints for:
  - [ ] Key Vault
  - [ ] Container Registry (if used)
- [ ] Configure NSG rules
- [ ] Document network topology

### 6.3 Configure Monitoring

- [ ] Create `/specs/infrastructure/monitoring.md` specification
- [ ] Configure Application Insights:

```csharp
// In Program.cs
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource("TechHub.*");
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
    })
    .UseAzureMonitor();
```

- [ ] Set up custom metrics for:
  - [ ] Page views per section
  - [ ] Filter usage
  - [ ] RSS feed requests
- [ ] Configure alerts for:
  - [ ] Error rate > 1%
  - [ ] Response time > 2s
  - [ ] Availability < 99.5%
- [ ] Create monitoring dashboard
- [ ] Verify metrics appear in App Insights

### 6.4 Configure Security

- [ ] Create `/specs/infrastructure/security.md` specification
- [ ] Enable managed identity for Container App
- [ ] Configure Key Vault access policies
- [ ] Enable HTTPS only
- [ ] Configure CORS policies
- [ ] Set up Content Security Policy headers
- [ ] Configure rate limiting
- [ ] Run security scan
- [ ] Document security configuration

---

## Phase 7: CI/CD Pipeline

### 7.1 Create GitHub Actions Workflow

- [ ] Create `/specs/cicd/github-actions.md` specification
- [ ] Create `.github/workflows/dotnet-ci.yml`:

```yaml
name: .NET CI

on:
  push:
    branches: [main]
    paths:
      - 'dotnet/**'
      - '.github/workflows/dotnet-*.yml'
  pull_request:
    branches: [main]
    paths:
      - 'dotnet/**'
      - '.github/workflows/dotnet-*.yml'

jobs:
  build:
    runs-on: ubuntu-latest
    defaults:
      run:
        working-directory: dotnet
    
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'
      
      - name: Restore dependencies
        run: dotnet restore
      
      - name: Build
        run: dotnet build --no-restore --configuration Release
      
      - name: Test
        run: dotnet test --no-build --configuration Release --verbosity normal
      
      - name: Publish
        run: dotnet publish src/TechHub.Web -c Release -o ./publish
      
      - name: Upload artifact
        uses: actions/upload-artifact@v4
        with:
          name: techhub-web
          path: dotnet/publish
```

- [ ] Create `.github/workflows/dotnet-deploy.yml`:

```yaml
name: .NET Deploy

on:
  workflow_dispatch:
    inputs:
      environment:
        description: 'Deployment environment'
        required: true
        default: 'staging'
        type: choice
        options:
          - staging
          - production

jobs:
  deploy:
    runs-on: ubuntu-latest
    environment: ${{ github.event.inputs.environment }}
    
    steps:
      - uses: actions/checkout@v4
      
      - name: Azure Login
        uses: azure/login@v2
        with:
          creds: ${{ secrets.AZURE_CREDENTIALS }}
      
      - name: Deploy Infrastructure
        run: |
          pwsh -File ./dotnet/scripts/deploy-infrastructure.ps1 `
            -Environment ${{ github.event.inputs.environment }}
      
      - name: Deploy Application
        run: |
          pwsh -File ./dotnet/scripts/deploy-app.ps1 `
            -Environment ${{ github.event.inputs.environment }}
```

- [ ] Create deployment scripts in PowerShell
- [ ] Configure GitHub secrets
- [ ] Test CI workflow on PR
- [ ] Test deployment to staging

### 7.2 Create PowerShell Deployment Scripts

- [ ] Create `/dotnet/scripts/deploy-infrastructure.ps1`:

```powershell
#!/usr/bin/env pwsh
param(
    [Parameter(Mandatory)]
    [ValidateSet('staging', 'production')]
    [string]$Environment
)

$ErrorActionPreference = "Stop"

Write-Host "Deploying infrastructure to $Environment..." -ForegroundColor Cyan

# Deploy Bicep
az deployment sub create `
    --location westeurope `
    --template-file ./infra/main.bicep `
    --parameters environmentName=$Environment

Write-Host "✅ Infrastructure deployment complete!" -ForegroundColor Green
```

- [ ] Create `/dotnet/scripts/deploy-app.ps1`:

```powershell
#!/usr/bin/env pwsh
param(
    [Parameter(Mandatory)]
    [ValidateSet('staging', 'production')]
    [string]$Environment
)

$ErrorActionPreference = "Stop"

Write-Host "Deploying application to $Environment..." -ForegroundColor Cyan

# Build container image
docker build -t techhub-web:latest ./src/TechHub.Web

# Push to registry
# Deploy to Container Apps

Write-Host "✅ Application deployment complete!" -ForegroundColor Green
```

- [ ] Create `/dotnet/scripts/test.ps1`:

```powershell
#!/usr/bin/env pwsh
param(
    [switch]$Coverage,
    [switch]$E2E
)

$ErrorActionPreference = "Stop"

Write-Host "Running tests..." -ForegroundColor Cyan

# Run unit and integration tests
dotnet test --verbosity normal

if ($E2E) {
    Write-Host "Running E2E tests..." -ForegroundColor Cyan
    # Run Playwright tests
}

Write-Host "✅ All tests passed!" -ForegroundColor Green
```

- [ ] Verify all scripts run correctly
- [ ] Document script usage

---

## Phase 8: Migration & Cutover

### 8.1 Pre-Migration Checklist

- [ ] All tests passing (unit, integration, E2E)
- [ ] Visual parity verified against Jekyll site
- [ ] All RSS feeds validated
- [ ] All URLs verified (no 404s)
- [ ] Performance benchmarks met
- [ ] Security scan passed
- [ ] Monitoring configured and verified
- [ ] Rollback plan documented
- [ ] DNS changes prepared

### 8.2 Migration Steps

- [ ] Deploy to staging environment
- [ ] Run full E2E test suite against staging
- [ ] Perform manual testing on staging
- [ ] Get stakeholder sign-off
- [ ] Schedule maintenance window
- [ ] Deploy to production
- [ ] Update DNS to point to new site
- [ ] Monitor for errors
- [ ] Verify all functionality
- [ ] Announce migration complete

### 8.3 Post-Migration

- [ ] Monitor error rates for 48 hours
- [ ] Verify RSS subscribers still receiving updates
- [ ] Check search engine indexing
- [ ] Collect user feedback
- [ ] Document lessons learned
- [ ] Archive Jekyll site (read-only)
- [ ] Update documentation to reference .NET site

---

## Technical Specifications

### URL Structure

**New URL Pattern** - Each URL is fully self-contained (no query parameters):

| Page Type | URL Pattern | Example |
| ----------- | ------------- | --------- |
| Home | `/` | `https://tech.hub.ms/` |
| Section Index | `/{section}` | `/github-copilot` |
| Collection | `/{section}/{collection}.html` | `/github-copilot/news.html` |
| Item Detail | `/{section}/{collection}/{item-id}.html` | `/ai/videos/vs-code-107.html` |
| RSS Feed (Section) | `/{section}/feed.xml` | `/ai/feed.xml` |
| RSS Feed (All) | `/feed.xml` | `/feed.xml` |

**Multi-Location Content Access**:

The same content can be accessed from multiple section URLs:

```text
# Same video accessible from both sections:
/ai/videos/vs-code-107.html
/github-copilot/videos/vs-code-107.html

# Both URLs serve the same content, with:
# - Section-specific navigation context
# - Canonical URL in <head> for SEO
# - Section-appropriate styling/background
```

**Routing Rules**:

1. **Section validation** - URL section must be valid (ai, github-copilot, ml, etc.)
2. **Category matching** - Content must have category matching section
3. **Collection matching** - URL collection must match content's collection
4. **Canonical URL** - First category in content's categories array determines canonical
5. **404 handling** - Invalid section/content combination returns 404

**Canonical URL Strategy**:

```csharp
// Each content item has a canonical URL for SEO
// Other URLs redirect via <link rel="canonical">

// Example: VS Code 107 video has categories: ["ai", "github-copilot"]
// Canonical: /ai/videos/vs-code-107.html (first category)
// Also accessible: /github-copilot/videos/vs-code-107.html (alternate)
```

**API URL Structure**:

| Endpoint | URL Pattern | Purpose |
| ----------- | ------------- | --------- |
| List sections | `/api/sections` | Get all sections |
| Get section | `/api/sections/{url}` | Get section details |
| List content | `/api/content/section/{sectionUrl}` | Get content for section |
| Get item | `/api/content/{sectionUrl}/{collection}/{itemId}` | Get specific item |
| RSS | `/api/rss/{sectionUrl}` | Generate RSS feed |

### Date Handling

- **Timezone**: `Europe/Brussels` (CET/CEST)
- **Storage**: Unix epoch timestamps (long/Int64)
- **Display Format**: `YYYY-MM-DD`
- **ISO Format**: `DateTimeOffset.ToString("O")`

```csharp
// DateUtils.cs
public static class DateUtils
{
    private static readonly TimeZoneInfo BrusselsTimeZone = 
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");
    
    public static DateTimeOffset FromEpoch(long epoch) =>
        DateTimeOffset.FromUnixTimeSeconds(epoch);
    
    public static long ToEpoch(DateTimeOffset date) =>
        date.ToUnixTimeSeconds();
    
    public static string FormatDisplay(long epoch) =>
        FromEpoch(epoch)
            .ToOffset(BrusselsTimeZone.GetUtcOffset(FromEpoch(epoch)))
            .ToString("yyyy-MM-dd");
}
```

### Semantic HTML Elements

Use semantic HTML for NLWeb compatibility:

| Element | Purpose | Usage |
| --------- | --------- | ------- |
| `<article>` | Self-contained content | Content items |
| `<section>` | Thematic grouping | Page sections |
| `<nav>` | Navigation | All navigation areas |
| `<main>` | Main content | Primary content area |
| `<header>` | Introductory content | Page and section headers |
| `<footer>` | Footer content | Page footer |
| `<time>` | Dates/times | All date displays |
| `<figure>` | Self-contained media | Images with captions |

### Schema.org Types

| Page Type | Schema.org Type |
| ----------- | ----------------- |
| Website | `WebSite` |
| Section | `CollectionPage` |
| Collection | `CollectionPage` |
| Item Detail | `Article` |
| Video Item | `VideoObject` |

---

## Reference Architecture

```text
┌─────────────────────────────────────────────────────────────────┐
│                         Azure Subscription                       │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────────┐   │
│  │                    Resource Group                        │   │
│  │  ┌─────────────────────────────────────────────────┐    │   │
│  │  │                  Virtual Network                 │    │   │
│  │  │  ┌──────────────────┐  ┌──────────────────────┐ │    │   │
│  │  │  │ Container Apps   │  │ Private Endpoints    │ │    │   │
│  │  │  │ Subnet           │  │ Subnet               │ │    │   │
│  │  │  │                  │  │                      │ │    │   │
│  │  │  │ ┌──────────────┐ │  │ ┌────────────────┐   │ │    │   │
│  │  │  │ │ Container    │ │  │ │ Key Vault PE   │   │ │    │   │
│  │  │  │ │ Apps Env     │ │  │ └────────────────┘   │ │    │   │
│  │  │  │ │              │ │  └──────────────────────┘ │    │   │
│  │  │  │ │ ┌──────────┐ │ │                           │    │   │
│  │  │  │ │ │TechHub   │ │ │                           │    │   │
│  │  │  │ │ │API       │ │ │                           │    │   │
│  │  │  │ │ │Container │ │ │                           │    │   │
│  │  │  │ │ └──────────┘ │ │                           │    │   │
│  │  │  │ │ ┌──────────┐ │ │                           │    │   │
│  │  │  │ │ │TechHub   │ │ │                           │    │   │
│  │  │  │ │ │Web       │ │ │                           │    │   │
│  │  │  │ │ │Container │ │ │                           │    │   │
│  │  │  │ │ └──────────┘ │ │                           │    │   │
│  │  │  │ └──────────────┘ │                           │    │   │
│  │  │  └──────────────────┘                           │    │   │
│  │  └─────────────────────────────────────────────────┘    │   │
│  │                                                          │   │
│  │  ┌────────────────┐  ┌────────────────┐                │   │
│  │  │ Log Analytics  │  │ App Insights   │                │   │
│  │  │ Workspace      │  │                │                │   │
│  │  └────────────────┘  └────────────────┘                │   │
│  │                                                          │   │
│  │  ┌────────────────┐                                    │   │
│  │  │ Key Vault      │                                    │   │
│  │  └────────────────┘                                    │   │
│  └─────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘

Data Flow (Frontend/Backend Separation):
┌──────────────┐    ┌──────────────┐    ┌──────────────────┐
│ User Request │───▶│ TechHub.Web  │───▶│ Blazor SSR       │
│              │    │ Container    │    │ (Server Render)  │
└──────────────┘    └──────────────┘    └──────────────────┘
                            │
                            │ HTTP API Calls
                            ▼
                    ┌──────────────────┐
                    │ TechHub.Api      │
                    │ Container        │
                    │                  │
                    │ /api/sections    │
                    │ /api/content     │
                    │ /api/rss         │
                    └──────────────────┘
                            │
                            ▼
                    ┌──────────────────────────────────────┐
                    │          Repository Layer            │
                    │  ┌─────────────┐  ┌──────────────┐  │
                    │  │ Section     │  │ Content      │  │
                    │  │ Repository  │  │ Repository   │  │
                    │  └─────────────┘  └──────────────┘  │
                    └──────────────────────────────────────┘
                                        │
                                        ▼
                    ┌──────────────────────────────────────┐
                    │           File System                │
                    │  ┌─────────────┐  ┌──────────────┐  │
                    │  │sections.json│  │ collections/ │  │
                    │  │             │  │ _blogs/      │  │
                    │  │             │  │ _news/       │  │
                    │  │             │  │ _videos/     │  │
                    │  │             │  │ etc.         │  │
                    │  └─────────────┘  └──────────────┘  │
                    └──────────────────────────────────────┘
```

---

## Progress Tracking

Mark phases complete as work progresses:

| Phase | Status | Completion Date |
| ------- | -------- | ----------------- |
| Phase 0: Planning | ⬜ Not Started | |
| Phase 1: Environment | ⬜ Not Started | |
| Phase 2: Core Architecture | ⬜ Not Started | |
| Phase 3: Content System | ⬜ Not Started | |
| Phase 4: Features | ⬜ Not Started | |
| Phase 5: Testing | ⬜ Not Started | |
| Phase 6: Azure Infrastructure | ⬜ Not Started | |
| Phase 7: CI/CD | ⬜ Not Started | |
| Phase 8: Migration | ⬜ Not Started | |

---

## Appendix

### A. External Resources

- [spec-kit Documentation](https://github.com/github/spec-kit)
- [NLWeb Documentation](https://github.com/nlweb-ai/NLWeb)
- [Schema.org](https://schema.org/)
- [.NET Aspire Documentation](https://learn.microsoft.com/dotnet/aspire)
- [Azure Container Apps Documentation](https://learn.microsoft.com/azure/container-apps)
- [Bicep Documentation](https://learn.microsoft.com/azure/azure-resource-manager/bicep)

### B. Related Documentation

- [Filtering System](filtering-system.md) - Current filtering behavior
- [Content Management](content-management.md) - Content workflows
- [Root AGENTS.md](../AGENTS.md) - Project principles
- [Full Stack Agent](../.github/agents/fullstack.md) - Jekyll development reference

### C. Glossary

| Term | Definition |
| ------ | ------------ |
| **Section** | Top-level content category (AI, GitHub Copilot, etc.) |
| **Collection** | Content type within a section (News, Blogs, Videos, etc.) |
| **Item** | Individual piece of content |
| **SSR** | Server-Side Rendering |
| **SDD** | Spec-Driven Development |
| **NLWeb** | Natural Language Web - semantic web interface |
| **MCP** | Model Context Protocol |
