# Solution Structure

## Overview

Defines the .NET solution structure, project organization, and dependencies for the Tech Hub migration. This specification establishes the foundation for all development work.

## Constitution Alignment

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

**AGENTS.md**: `/dotnet/src/TechHub.Api/AGENTS.md` - API development patterns

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

**AGENTS.md**: `/dotnet/src/TechHub.Web/AGENTS.md` - Blazor patterns

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

**AGENTS.md**: `/dotnet/src/TechHub.Core/AGENTS.md` - Domain design patterns

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

**AGENTS.md**: `/dotnet/src/TechHub.Infrastructure/AGENTS.md` - Infrastructure patterns

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

**AGENTS.md**: `/dotnet/tests/AGENTS.md` - Testing strategies

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

See `/specs/testing/unit-testing.md` for comprehensive testing approach.

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
2. **API**: https://localhost:5001/swagger
3. **Web**: https://localhost:5173
4. **Aspire Dashboard**: https://localhost:15888

## References

- [.NET Project Structure Best Practices](https://learn.microsoft.com/en-us/dotnet/core/tutorials/libraries)
- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Clean Architecture in .NET](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- `/specs/.speckit/constitution.md` - Project principles
- `/specs/features/domain-models.md` - Domain layer
- `/specs/features/repository-pattern.md` - Infrastructure layer
