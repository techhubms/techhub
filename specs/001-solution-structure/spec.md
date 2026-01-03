# Solution Structure

## Overview

Defines the .NET solution structure, project organization, and dependencies for the Tech Hub migration. This specification establishes the foundation for all development work.

## Functional Requirements

### FR-001: Solution File Organization

The solution must be organized with clear separation between source code, tests, infrastructure, and scripts.

**Required Structure**:

- `src/` - All production code projects
- `tests/` - All test projects
- `infra/` - Bicep infrastructure as code
- `scripts/` - Build and deployment automation
- `specs/` - Feature specifications

### FR-002: Source Projects

The solution must contain exactly six source projects:

1. **TechHub.Api** - ASP.NET Core Minimal API backend
2. **TechHub.Web** - Blazor SSR + WASM frontend
3. **TechHub.Core** - Domain models and interfaces (pure, no dependencies)
4. **TechHub.Infrastructure** - Repository implementations and services
5. **TechHub.ServiceDefaults** - Shared .NET Aspire configuration
6. **TechHub.AppHost** - .NET Aspire orchestration host

### FR-003: Test Projects

The solution must contain exactly five test projects:

1. **TechHub.Core.Tests** - Unit tests for domain models
2. **TechHub.Api.Tests** - Integration tests for API endpoints
3. **TechHub.Infrastructure.Tests** - Tests for repositories and services
4. **TechHub.Web.Tests** - bUnit component tests for Blazor
5. **TechHub.E2E.Tests** - Playwright end-to-end tests

### FR-004: Target Framework

All projects must target .NET 10 (`net10.0`) with C# 13 language features.

### FR-005: Project References

Projects must follow clean architecture dependency rules:

- **TechHub.Core**: No project dependencies (pure domain)
- **TechHub.Infrastructure**: References only TechHub.Core
- **TechHub.Api**: References Core, Infrastructure, ServiceDefaults
- **TechHub.Web**: References Core, ServiceDefaults (NOT Infrastructure)
- **TechHub.AppHost**: References Api, Web
- **Test Projects**: Reference only projects they test

### FR-006: Code Quality Standards

All projects must enforce:

- Nullable reference types enabled (`<Nullable>enable</Nullable>`)
- Implicit usings enabled (`<ImplicitUsings>enable</ImplicitUsings>`)
- File-scoped namespaces (not block-scoped)
- Warnings treated as errors (`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`)

### FR-007: Testing Framework

All test projects must use:

- xUnit as test framework (v2.9.3+)
- FluentAssertions for assertions (v7.0.0+)
- Moq for mocking (v4.20.72+) where applicable
- Coverlet for code coverage (v6.0.2+)

### FR-008: Package Management

All package references must:

- Use specific versions (no wildcards)
- Be compatible with .NET 10
- Follow quarterly update cadence unless security patches required

### FR-009: Build Configuration

The solution must support:

- Debug builds with symbols and no optimization
- Release builds with optimization and no symbols
- Single command build: `dotnet build`
- Single command test: `dotnet test`

### FR-010: Local Development

Developers must be able to:

- Run `dotnet restore` to restore all packages
- Run `dotnet build` to compile all projects
- Run `dotnet test` to execute all tests
- Run `dotnet run --project src/TechHub.AppHost` to start Aspire dashboard
- Access API at <https://localhost:5001>
- Access Web at <https://localhost:5173>
- Access Aspire dashboard at <https://localhost:15888>

## Success Criteria

### Build Performance

- Clean build completes in under 30 seconds on standard development hardware
- Incremental builds complete in under 5 seconds for single file changes
- Package restore completes in under 10 seconds with warm NuGet cache

### Code Quality

- Zero compiler warnings across all projects
- Zero nullable reference warnings
- All projects compile successfully on first checkout

### Developer Experience

- New developer can clone repository and build successfully within 5 minutes
- All test frameworks integrate with VS Code Test Explorer
- Aspire dashboard provides unified view of all services

### Test Coverage

- All test projects discovered by `dotnet test`
- Test execution completes in under 60 seconds for all unit tests
- Code coverage data generated automatically with `--collect:"XPlat Code Coverage"`

## User Scenarios

### Scenario 1: New Developer Onboarding

**Actor**: New developer joining the team

**Goal**: Set up local development environment and verify everything works

**Steps**:

1. Clone repository: `git clone https://github.com/techhubms/techhub.git`
2. Navigate to .NET directory: `cd dotnet`
3. Restore packages: `dotnet restore`
4. Build solution: `dotnet build`
5. Run tests: `dotnet test`
6. Start Aspire: `dotnet run --project src/TechHub.AppHost`

**Expected Outcome**:

- All steps complete without errors
- Build shows "Build succeeded. 0 Warning(s)"
- All tests pass
- Aspire dashboard opens showing API and Web running

### Scenario 2: Adding New Dependency

**Actor**: Developer adding new NuGet package

**Goal**: Add package to correct project with proper version

**Steps**:

1. Identify target project (e.g., TechHub.Api)
2. Add package: `dotnet add src/TechHub.Api package PackageName --version X.Y.Z`
3. Verify build: `dotnet build`
4. Verify tests: `dotnet test`

**Expected Outcome**:

- Package added to correct `.csproj` file
- No version conflicts with existing packages
- Build succeeds with zero warnings
- All tests still pass

### Scenario 3: Creating New Test Project

**Actor**: Developer needing to test new component

**Goal**: Add test project with proper configuration and references

**Steps**:

1. Create test project: `dotnet new xunit -n TechHub.NewFeature.Tests -o tests/TechHub.NewFeature.Tests`
2. Add to solution: `dotnet sln add tests/TechHub.NewFeature.Tests`
3. Add project reference: `dotnet add tests/TechHub.NewFeature.Tests reference src/TechHub.NewFeature`
4. Add test dependencies: FluentAssertions, Moq, Coverlet
5. Run tests: `dotnet test`

**Expected Outcome**:

- New test project appears in solution
- Test project discovered by `dotnet test`
- Can run tests from VS Code Test Explorer
- Coverage collection works

## Acceptance Criteria

### Build System

- [ ] `dotnet restore` exits with code 0
- [ ] `dotnet build` exits with code 0
- [ ] `dotnet build -c Release` exits with code 0
- [ ] Build produces zero warnings
- [ ] Build log shows all 11 projects compiled successfully

### Test Discovery

- [ ] `dotnet test` discovers all 5 test projects
- [ ] All test projects show in VS Code Test Explorer
- [ ] Running tests from command line works
- [ ] Running tests from VS Code works
- [ ] Code coverage collector runs successfully

### Project Structure

- [ ] Solution file lists all 11 projects
- [ ] `src/` contains exactly 6 projects
- [ ] `tests/` contains exactly 5 projects
- [ ] All projects target `net10.0`
- [ ] All projects have nullable reference types enabled
- [ ] All projects use file-scoped namespaces

### Dependencies

- [ ] TechHub.Core has zero project references
- [ ] TechHub.Infrastructure references only TechHub.Core
- [ ] TechHub.Web does NOT reference TechHub.Infrastructure
- [ ] All dependency rules from FR-005 satisfied
- [ ] No circular dependencies exist

### Aspire Integration

- [ ] `dotnet run --project src/TechHub.AppHost` starts successfully
- [ ] Aspire dashboard accessible at <https://localhost:15888>
- [ ] API service registered and running
- [ ] Web service registered and running
- [ ] Environment variables propagate to services

### Build Standards

- [ ] No compiler warnings in any project
- [ ] No nullable reference warnings
- [ ] StyleCop or similar analyzer passes (if configured)
- [ ] EditorConfig rules enforced

### Documentation

- [ ] All projects have AGENTS.md files (where applicable)
- [ ] README exists explaining how to build and run
- [ ] Package versions documented in this spec

---

## Implementation Guide

### Constitution Alignment

- **Modern UX First**: Separate frontend (Blazor) and backend (API) projects
- **Clean Architecture**: Domain models, infrastructure, and application layers
- **.NET 10 Stack**: Latest LTS runtime for all projects
- **Configuration-Driven**: Shared configuration via TechHub.Core

## Solution File

**Location**: `/dotnet/TechHub.sln`

**Structure**:

```text
TechHub.sln
├── src/
│   ├── TechHub.Api/                   # REST API Backend
│   ├── TechHub.Web/                   # Blazor Frontend
│   ├── TechHub.Core/                  # Domain Models & Interfaces
│   ├── TechHub.Infrastructure/        # Data Access Implementations
│   ├── TechHub.ServiceDefaults/       # Aspire Shared Configuration
│   └── TechHub.AppHost/               # Aspire Orchestration
├── tests/
│   ├── TechHub.Core.Tests/            # Unit Tests
│   ├── TechHub.Api.Tests/             # API Integration Tests
│   ├── TechHub.Infrastructure.Tests/  # Infrastructure Tests
│   ├── TechHub.Web.Tests/             # bUnit Component Tests
│   └── TechHub.E2E.Tests/             # Playwright E2E Tests
├── infra/
│   ├── main.bicep
│   ├── modules/
│   └── parameters/
├── scripts/
│   ├── build.ps1
│   ├── test.ps1
│   └── deploy.ps1
└── specs/
    ├── .speckit/
    └── features/
```

## Project Details

### TechHub.Api

**Purpose**: ASP.NET Core Minimal API backend

**Framework**: `net10.0`

**Package References**:

```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0" />
<PackageReference Include="Azure.Monitor.OpenTelemetry.AspNetCore" Version="1.3.0" />
<PackageReference Include="Markdig" Version="0.40.0" />
<PackageReference Include="YamlDotNet" Version="16.2.0" />
<PackageReference Include="Microsoft.Extensions.Http.Resilience" Version="10.0.0" />
```

**Project References**:

- `TechHub.Core`
- `TechHub.Infrastructure`
- `TechHub.ServiceDefaults`

**Key Files**:

- `Program.cs` - Entry point, DI setup, middleware pipeline
- `Endpoints/*.cs` - Minimal API endpoint groups
- `appsettings.json` - Configuration

**AGENTS.md**: `/src/TechHub.Api/AGENTS.md` - API development patterns

---

### TechHub.Web

**Purpose**: Blazor Server-Side Rendering + WebAssembly frontend

**Framework**: `net10.0`

**Package References**:

```xml
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="10.0.0" />
<PackageReference Include="Microsoft.Extensions.Http.Resilience" Version="10.0.0" />
<PackageReference Include="Azure.Monitor.OpenTelemetry.AspNetCore" Version="1.3.0" />
```

**Project References**:

- `TechHub.Core`
- `TechHub.ServiceDefaults`

**Key Files**:

- `Program.cs` - Entry point, DI, HttpClient configuration
- `Components/Layout/*.razor` - MainLayout, Header, Footer
- `Components/Pages/*.razor` - Routable page components
- `Components/Shared/*.razor` - Reusable components
- `wwwroot/` - Static assets (CSS, JS, images)

**AGENTS.md**: `/src/TechHub.Web/AGENTS.md` - Blazor patterns

---

### TechHub.Core

**Purpose**: Domain models, DTOs, interfaces, shared logic

**Framework**: `net10.0`

**Package References**: None (pure domain layer)

**Key Files**:

- `Models/Section.cs` - Section record
- `Models/ContentItem.cs` - ContentItem record
- `Models/DateUtils.cs` - Date conversion utilities
- `DTOs/*.cs` - Data transfer objects
- `Interfaces/ISectionRepository.cs`
- `Interfaces/IContentRepository.cs`
- `Interfaces/IMarkdownProcessor.cs`
- `Interfaces/IRssGenerator.cs`

**AGENTS.md**: `/src/TechHub.Core/AGENTS.md` - Domain design patterns

---

### TechHub.Infrastructure

**Purpose**: Repository implementations, file I/O, markdown processing

**Framework**: `net10.0`

**Package References**:

```xml
<PackageReference Include="Markdig" Version="0.40.0" />
<PackageReference Include="YamlDotNet" Version="16.2.0" />
<PackageReference Include="Microsoft.Extensions.Caching.Memory" Version="10.0.0" />
```

**Project References**:

- `TechHub.Core`

**Key Files**:

- `Repositories/FileSectionRepository.cs`
- `Repositories/FileContentRepository.cs`
- `Services/MarkdownProcessor.cs`
- `Services/RssGenerator.cs`
- `Configuration/ContentOptions.cs`

**AGENTS.md**: `/src/TechHub.Infrastructure/AGENTS.md` - Infrastructure patterns

---

### TechHub.ServiceDefaults

**Purpose**: Shared .NET Aspire configuration (OpenTelemetry, health checks, resilience)

**Framework**: `net10.0`

**Package References**:

```xml
<PackageReference Include="Microsoft.Extensions.ServiceDiscovery" Version="10.0.0" />
<PackageReference Include="Microsoft.Extensions.Http.Resilience" Version="10.0.0" />
<PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" Version="1.10.0" />
<PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.10.0" />
<PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" Version="1.10.0" />
<PackageReference Include="OpenTelemetry.Instrumentation.Http" Version="1.10.0" />
<PackageReference Include="OpenTelemetry.Instrumentation.Runtime" Version="1.10.0" />
```

**Key Files**:

- `Extensions.cs` - `AddServiceDefaults()` extension method

**References**: See [.NET Aspire docs](https://learn.microsoft.com/en-us/dotnet/aspire/)

---

### TechHub.AppHost

**Purpose**: .NET Aspire orchestration for local development

**Framework**: `net10.0`

**Package References**:

```xml
<PackageReference Include="Aspire.Hosting.AppHost" Version="10.0.0" />
```

**Project References**:

- `TechHub.Api`
- `TechHub.Web`

**Key Files**:

- `Program.cs` - Service discovery, environment variables

**Example**:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.TechHub_Api>("api")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

var web = builder.AddProject<Projects.TechHub_Web>("web")
    .WithReference(api)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

builder.Build().Run();
```

---

### Test Projects

All test projects use:

- **Framework**: `net10.0`
- **Test SDK**: `Microsoft.NET.Test.Sdk` v18.2.0
- **Test Framework**: `xUnit` v2.9.3 with `xunit.runner.visualstudio` v3.0.0
- **Assertion Library**: `FluentAssertions` v7.0.0
- **Coverage**: `coverlet.collector` v6.0.2

#### TechHub.Core.Tests

**Purpose**: Unit tests for domain models and utilities

**Additional Packages**: None

**Project References**: `TechHub.Core`

**AGENTS.md**: `/tests/AGENTS.md` - Testing strategies

---

#### TechHub.Api.Tests

**Purpose**: API integration tests

**Additional Packages**:

```xml
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="10.0.0" />
<PackageReference Include="Moq" Version="4.20.72" />
```

**Project References**: `TechHub.Api`, `TechHub.Core`

---

#### TechHub.Infrastructure.Tests

**Purpose**: Infrastructure layer tests

**Additional Packages**:

```xml
<PackageReference Include="Moq" Version="4.20.72" />
```

**Project References**: `TechHub.Infrastructure`, `TechHub.Core`

---

#### TechHub.Web.Tests

**Purpose**: Blazor component tests

**Additional Packages**:

```xml
<PackageReference Include="bunit" Version="1.35.3" />
<PackageReference Include="Moq" Version="4.20.72" />
```

**Project References**: `TechHub.Web`, `TechHub.Core`

---

#### TechHub.E2E.Tests

**Purpose**: End-to-end browser tests

**Additional Packages**:

```xml
<PackageReference Include="Microsoft.Playwright" Version="1.50.0" />
```

**Project References**: None (tests running application)

---

## File-Scoped Namespaces

All C# files MUST use file-scoped namespaces:

```csharp
namespace TechHub.Api.Endpoints;

public static class SectionEndpoints
{
    // ...
}
```

**NOT**:

```csharp
namespace TechHub.Api.Endpoints
{
    public static class SectionEndpoints
    {
        // ...
    }
}
```

## Nullable Reference Types

All projects MUST enable nullable reference types:

```xml
<PropertyGroup>
  <Nullable>enable</Nullable>
</PropertyGroup>
```

## Common Build Properties

All projects share:

```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <Nullable>enable</Nullable>
  <ImplicitUsings>enable</ImplicitUsings>
  <LangVersion>13</LangVersion>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
</PropertyGroup>
```

## Global Usings

**TechHub.Core/GlobalUsings.cs**:

```csharp
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
```

**TechHub.Api/GlobalUsings.cs**:

```csharp
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using TechHub.Core.Interfaces;
global using TechHub.Core.Models;
global using TechHub.Core.DTOs;
```

**TechHub.Web/GlobalUsings.cs**:

```csharp
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Web;
global using TechHub.Core.DTOs;
```

## Directory Structure Commands

**Create Projects**:

```powershell
# Navigate to /workspaces/techhub/dotnet
cd /workspaces/techhub/dotnet

# Create solution
dotnet new sln -n TechHub

# Create source projects
dotnet new web -n TechHub.Api -o src/TechHub.Api
dotnet new blazor -n TechHub.Web -o src/TechHub.Web -int Server
dotnet new classlib -n TechHub.Core -o src/TechHub.Core
dotnet new classlib -n TechHub.Infrastructure -o src/TechHub.Infrastructure
dotnet new classlib -n TechHub.ServiceDefaults -o src/TechHub.ServiceDefaults
dotnet new aspire-apphost -n TechHub.AppHost -o src/TechHub.AppHost

# Create test projects
dotnet new xunit -n TechHub.Core.Tests -o tests/TechHub.Core.Tests
dotnet new xunit -n TechHub.Api.Tests -o tests/TechHub.Api.Tests
dotnet new xunit -n TechHub.Infrastructure.Tests -o tests/TechHub.Infrastructure.Tests
dotnet new xunit -n TechHub.Web.Tests -o tests/TechHub.Web.Tests
dotnet new xunit -n TechHub.E2E.Tests -o tests/TechHub.E2E.Tests

# Add projects to solution
dotnet sln add src/TechHub.Api/TechHub.Api.csproj
dotnet sln add src/TechHub.Web/TechHub.Web.csproj
dotnet sln add src/TechHub.Core/TechHub.Core.csproj
dotnet sln add src/TechHub.Infrastructure/TechHub.Infrastructure.csproj
dotnet sln add src/TechHub.ServiceDefaults/TechHub.ServiceDefaults.csproj
dotnet sln add src/TechHub.AppHost/TechHub.AppHost.csproj
dotnet sln add tests/TechHub.Core.Tests/TechHub.Core.Tests.csproj
dotnet sln add tests/TechHub.Api.Tests/TechHub.Api.Tests.csproj
dotnet sln add tests/TechHub.Infrastructure.Tests/TechHub.Infrastructure.Tests.csproj
dotnet sln add tests/TechHub.Web.Tests/TechHub.Web.Tests.csproj
dotnet sln add tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj

# Add project references
dotnet add src/TechHub.Api/TechHub.Api.csproj reference src/TechHub.Core/TechHub.Core.csproj
dotnet add src/TechHub.Api/TechHub.Api.csproj reference src/TechHub.Infrastructure/TechHub.Infrastructure.csproj
dotnet add src/TechHub.Api/TechHub.Api.csproj reference src/TechHub.ServiceDefaults/TechHub.ServiceDefaults.csproj

dotnet add src/TechHub.Web/TechHub.Web.csproj reference src/TechHub.Core/TechHub.Core.csproj
dotnet add src/TechHub.Web/TechHub.Web.csproj reference src/TechHub.ServiceDefaults/TechHub.ServiceDefaults.csproj

dotnet add src/TechHub.Infrastructure/TechHub.Infrastructure.csproj reference src/TechHub.Core/TechHub.Core.csproj

dotnet add src/TechHub.AppHost/TechHub.AppHost.csproj reference src/TechHub.Api/TechHub.Api.csproj
dotnet add src/TechHub.AppHost/TechHub.AppHost.csproj reference src/TechHub.Web/TechHub.Web.csproj

dotnet add tests/TechHub.Core.Tests/TechHub.Core.Tests.csproj reference src/TechHub.Core/TechHub.Core.csproj
dotnet add tests/TechHub.Api.Tests/TechHub.Api.Tests.csproj reference src/TechHub.Api/TechHub.Api.csproj
dotnet add tests/TechHub.Infrastructure.Tests/TechHub.Infrastructure.Tests.csproj reference src/TechHub.Infrastructure/TechHub.Infrastructure.csproj
dotnet add tests/TechHub.Web.Tests/TechHub.Web.Tests.csproj reference src/TechHub.Web/TechHub.Web.csproj
```

## NuGet Package Management

**Package Source**: NuGet.org (default)

**Restore Command**: `dotnet restore`

**Update Strategy**: Update packages quarterly or for security patches

**Version Policy**: Use specific versions in `.csproj`, not wildcards

## Build Configuration

**Debug Build** (default):

- Symbol generation enabled
- Optimizations disabled
- DEBUG constant defined

**Release Build**:

- Symbol generation disabled
- Optimizations enabled
- No DEBUG constant

**Build Command**: `dotnet build -c Release`

## Testing Strategy

See `/specs/004-unit-testing/spec.md` for comprehensive testing approach.

**Test Execution**:

```powershell
# Run all tests
dotnet test

# Run specific project
dotnet test tests/TechHub.Core.Tests

# With coverage
dotnet test --collect:"XPlat Code Coverage"

# With logger
dotnet test --logger "console;verbosity=detailed"
```

## Development Workflow

1. **Start Aspire**: `dotnet run --project src/TechHub.AppHost`
2. **API**: <https://localhost:5001/swagger>
3. **Web**: <https://localhost:5173>
4. **Aspire Dashboard**: <https://localhost:15888>

## References

- [.NET Project Structure Best Practices](https://learn.microsoft.com/en-us/dotnet/core/tutorials/libraries)
- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Clean Architecture in .NET](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- `/.specify/memory/constitution.md` - Project principles
- `/specs/011-domain-models/spec.md` - Domain layer
- `/specs/012-repository-pattern/spec.md` - Infrastructure layer
