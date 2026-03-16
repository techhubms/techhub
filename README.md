# Tech Hub

Tech Hub is a **modern .NET 10 web application** built with Blazor that serves as a technical content hub — a fast, responsive, accessible platform for showcasing technical content across **AI**, **GitHub Copilot**, **Azure**, **.NET**, **DevOps**, **Security**, and **ML** topics.

## Technology

| Layer | Technology |
|-------|-----------|
| **Backend** | ASP.NET Core Minimal API, .NET 10, C# 13 |
| **Frontend** | Blazor InteractiveServer with prerendering |
| **Orchestration** | .NET Aspire (service discovery, telemetry, health checks) |
| **Database** | PostgreSQL (Dapper) or FileSystem (no database) |
| **Infrastructure** | Azure Container Apps, Bicep IaC, GitHub Actions CI/CD |
| **Testing** | xUnit v3, bUnit, Playwright, Pester |

## Quick Start

Open in a **Dev Container** (VS Code or GitHub Codespaces), then:

```powershell
# Build, run ALL tests (unit + integration + E2E), and start servers
Run

# Start servers without tests (faster iteration)
Run -WithoutTests
```

| URL | Service |
|-----|---------|
| `https://localhost:5003` | Web (Blazor) |
| `https://localhost:5001` | API (Swagger UI at `/swagger`) |
| `http://localhost:18888` | Aspire Dashboard |

See **[docs/running-and-testing.md](docs/running-and-testing.md)** for the full `Run` command reference, Docker mode, and targeted test execution.

## Architecture

```text
┌──────────────────────────────────────────────────────┐
│                  Aspire AppHost                       │
└──────────────────────────────────────────────────────┘
             │                │                │
    ┌────────▼──────┐ ┌──────▼────────┐ ┌─────▼──────┐
    │ TechHub.Api   │ │ TechHub.Web   │ │  Dashboard  │
    │ (port 5001)   │◄┤ (port 5003)   │ │ (port 18888)│
    │ REST API      │ │ Blazor SSR    │ │ Traces/Logs │
    └───────────────┘ └───────────────┘ └─────────────┘
```

- **TechHub.Core** — Domain models and interfaces (framework-agnostic)
- **TechHub.Infrastructure** — Dapper repositories, markdown processing, caching
- **collections/** — Markdown content (news, videos, blogs, community, roundups) synced to database at startup

See **[docs/architecture.md](docs/architecture.md)** for full details.

## Project Structure

| Directory | Purpose |
|-----------|---------|
| `src/` | Application source code (API, Web, Core, Infrastructure, AppHost) |
| `tests/` | Unit, integration, component, E2E, and PowerShell tests |
| `collections/` | Markdown content organized by collection type |
| `docs/` | Functional and technical documentation |
| `scripts/` | PowerShell automation (build, deploy, content processing) |
| `infra/` | Bicep IaC templates for Azure Container Apps |
| `specs/` | Feature specifications (SpecKit format) |

See **[docs/repository-structure.md](docs/repository-structure.md)** for the complete breakdown.

## Documentation

All detailed documentation lives in **[docs/](docs/)**. Start with:

- **[docs/documentation-index.md](docs/documentation-index.md)** — Full index of all documentation
- **[docs/running-and-testing.md](docs/running-and-testing.md)** — Running, testing, and debugging
- **[docs/architecture.md](docs/architecture.md)** — System architecture and .NET Aspire setup
- **[docs/technology-stack.md](docs/technology-stack.md)** — Complete technology stack
- **[docs/testing-strategy.md](docs/testing-strategy.md)** — Testing Diamond approach

For AI agents: see **[AGENTS.md](AGENTS.md)** and the nested domain AGENTS.md files in each directory.
