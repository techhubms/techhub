# Tech Hub .NET - Developer Quickstart

**Last Updated**: 2026-01-02  
**Prerequisites**: .NET 10 SDK, Docker Desktop, VS Code

---

## Quick Start (5 minutes)

### 1. Clone & Open

```bash
# Clone repository

git clone https://github.com/techhubms/techhub.git
cd techhub

# Checkout migration branch

git checkout dotnet-migration

# Open in VS Code

code .
```

### 2. Open in DevContainer

**Option A: VS Code DevContainer (Recommended)**

1. Install "Dev Containers" extension
2. Press `Ctrl+Shift+P` → "Dev Containers: Reopen in Container"
3. Select **"Tech Hub .NET"** configuration
4. Wait for container build (~3-5 minutes first time)

**Option B: Local Development**

```bash
# Install .NET Aspire workload

dotnet workload install aspire

# Restore packages

cd dotnet
dotnet restore
```

### 3. Run the Application

```bash
# Navigate to dotnet directory

cd dotnet

# Run with .NET Aspire (starts API + Web)

dotnet run --project src/TechHub.AppHost

# Or run API only

dotnet run --project src/TechHub.Api

# Or run Web only  

dotnet run --project src/TechHub.Web
```

**Access**:

- Aspire Dashboard: `http://localhost:15000`
- API: `http://localhost:5001`
- Web: `http://localhost:5002`

---

## Project Structure

```
root/
├── src/
│   ├── TechHub.Core/           # Domain models and interfaces
│   ├── TechHub.Infrastructure/ # Data access, caching, services
│   ├── TechHub.Api/            # REST API backend
│   ├── TechHub.Web/            # Blazor frontend
│   └── TechHub.AppHost/        # .NET Aspire orchestration
├── tests/
│   ├── TechHub.Core.Tests/
│   ├── TechHub.Infrastructure.Tests/
│   ├── TechHub.Api.Tests/
│   ├── TechHub.Web.Tests/
│   └── TechHub.E2E.Tests/
├── jekyll/                     # Jekyll reference implementation
└── TechHub.slnx                # .NET solution file
```

---

## Common Tasks

### Run Tests

```bash
# Run all tests

dotnet test

# Run specific project tests

dotnet test tests/TechHub.Core.Tests

# Run with coverage

dotnet test --collect:"XPlat Code Coverage"

# Run E2E tests (requires API + Web running)

cd tests/TechHub.E2E.Tests
dotnet test
```

### Build

```bash
# Build entire solution

dotnet build

# Build specific project

dotnet build src/TechHub.Api

# Build for production

dotnet build -c Release
```

### Add New Content

```bash
# Content files remain in collections/ at repository root

cd /workspaces/techhub/collections/_news

# Create new markdown file

cat > 2026-01-02-example-article.md << 'EOF'
---
title: "Example Article"
author: "Your Name"
date: 2026-01-02
categories: ["AI"]
tags: [azure-openai, gpt-4]
---

Article excerpt appears here.

<!--excerpt_end-->

# Full Article

Content goes here...
EOF

# Restart application to load new content

# (In future: add file watcher for auto-reload)
```

### Debug

**VS Code Launch Configurations** (`.vscode/launch.json`):

- **Debug API** - Starts TechHub.Api with debugger attached
- **Debug Web** - Starts TechHub.Web with debugger attached
- **Debug Aspire** - Starts full application with Aspire dashboard

**Breakpoints**:

- Set breakpoints in C# code
- Press F5 to start debugging
- Use Debug Console for expressions

---

## Development Workflow

### 1. Pick a Feature Spec

Browse `/specs/` directory for feature specifications:

```bash
ls specs/

# 001-solution-structure

# 002-configuration-management

# 003-resilience-error-handling

# ...
```

Each spec has:

- `spec.md` - Feature requirements
- `plan.md` - Implementation plan (if generated)
- `tasks.md` - Task breakdown (if generated)

### 2. Create a Branch

```bash
git checkout -b feature/011-domain-models
```

### 3. Implement with TDD

```bash
# 1. Write failing test

vi tests/TechHub.Core.Tests/Models/SectionTests.cs

# 2. Run test (should fail)

dotnet test tests/TechHub.Core.Tests

# 3. Implement feature

vi src/TechHub.Core/Models/Section.cs

# 4. Run test (should pass)

dotnet test tests/TechHub.Core.Tests

# 5. Refactor if needed
```

### 4. Verify All Tests Pass

```bash
dotnet test
```

### 5. Check Code Quality

```bash
# Format code

dotnet format

# Analyze code

dotnet build /p:TreatWarningsAsErrors=true
```

### 6. Commit and Push

```bash
git add .
git commit -m "Implement 011-domain-models: Core entities"
git push origin feature/011-domain-models
```

---

## Key Technologies

| Technology | Purpose | Documentation |
|------------|---------|---------------|
| .NET 10 | Runtime | <https://learn.microsoft.com/dotnet> |
| Blazor | Frontend | <https://learn.microsoft.com/aspnet/core/blazor> |
| ASP.NET Core | API | <https://learn.microsoft.com/aspnet/core> |
| .NET Aspire | Orchestration | <https://learn.microsoft.com/dotnet/aspire> |
| xUnit | Testing | <https://xunit.net> |
| bUnit | Blazor testing | <https://bunit.dev> |
| Playwright | E2E testing | <https://playwright.dev/dotnet> |
| Markdig | Markdown | <https://github.com/xoofx/markdig> |
| Polly | Resilience | <https://www.pollydocs.org> |

---

## Troubleshooting

### "Workload 'aspire' not found"

```bash
dotnet workload install aspire
```

### "Port 5001 already in use"

```bash
# Kill existing process

lsof -ti:5001 | xargs kill -9

# Or change port in appsettings.json
```

### "Content not loading"

1. Check `collections/` directory exists at repository root
2. Verify `_data/sections.json` is valid JSON
3. Check logs: `dotnet run` output shows content loading status

### "Tests failing in CI/CD"

1. Ensure all dependencies restored: `dotnet restore`
2. Check .NET version matches: `dotnet --version` (should be 10.x)
3. Review GitHub Actions logs for detailed errors

---

## Documentation

- **[Root AGENTS.md](../AGENTS.md)** - Framework-agnostic principles
- **[.NET AGENTS.md](AGENTS.md)** - .NET-specific development guide
- **[Feature Specs](specs/)** - Detailed feature specifications
- **[API Contracts](specs/dotnet-migration/contracts/)** - REST API documentation
- **[Data Model](specs/dotnet-migration/data-model.md)** - Entity definitions

---

## Getting Help

1. **Check AGENTS.md files** - Domain-specific development guidance
2. **Review feature specs** - Authoritative behavior documentation
3. **Ask in GitHub Discussions** - Community support
4. **Open GitHub Issue** - Bug reports and feature requests

---

## Next Steps

1. ✅ Development environment set up
2. 📖 Read [/specs/dotnet-migration/spec.md](specs/dotnet-migration/spec.md)
3. 🎯 Pick a feature from [research.md implementation sequence](specs/dotnet-migration/research.md#implementation-sequence)
4. 💻 Start coding with TDD workflow
5. ✅ Run tests and verify success criteria

**Happy coding! 🚀**
