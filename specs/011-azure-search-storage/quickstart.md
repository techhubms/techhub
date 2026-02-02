# Quick Start: PostgreSQL Storage & Search

**Feature**: PostgreSQL Storage & Search Architecture  
**Branch**: `011-azure-search-storage`  
**Date**: 2026-01-27

## Overview

This guide helps developers get started with the PostgreSQL-based content storage and search system. It covers local development setup, running tests, and understanding the architecture.

## Prerequisites

- ✅ .NET 10 SDK installed (via DevContainer)
- ✅ Docker running (for PostgreSQL container)
- ✅ PowerShell 7+ (via DevContainer)

## Local Development Setup

### Option 1: SQLite (Default - No Docker Required)

**Best for**: Fast iteration, lightweight development, integration tests

```bash
# No setup needed - SQLite runs in-process
# Database file created automatically at techhub.db

# Run with SQLite
Run

# Configuration in appsettings.Development.json:
{
  "Database": {
    "Provider": "SQLite",
    "ConnectionString": "Data Source=techhub.db"
  }
}
```

### Option 2: Docker PostgreSQL (Production-Like)

**Best for**: E2E tests, testing PostgreSQL-specific features, validating production behavior

```bash
# Start PostgreSQL container
docker-compose up -d postgres

# Run with PostgreSQL
Run

# Configuration in appsettings.Development.json:
{
  "Database": {
    "Provider": "PostgreSQL",
    "ConnectionString": "Host=localhost;Port=5432;Database=techhub;Username=techhub;Password=localdev"
  }
}
```

## First Run

The first run performs a full sync of all markdown files (4000+ files, ~60 seconds):

```bash
Run
```

**Output**:

```text
[10:30:00] Starting content sync...
[10:30:45] Sync complete: 4013 added, 0 updated, 0 deleted, 0 unchanged (45230ms)
[10:30:46] Repository initialized with 4013 items
[10:30:46] Servers starting...
```

## Subsequent Runs

Subsequent runs use hash-based incremental sync (<1 second if no changes):

```bash
Run
```

**Output**:

```text
[10:35:00] Starting content sync...
[10:35:01] Sync complete: 0 added, 0 updated, 0 deleted, 4013 unchanged (850ms)
[10:35:01] Repository initialized with 4013 items
[10:35:01] Servers starting...
```

## Skip Sync for Fast Startup

When iterating on code changes (not content changes), skip sync entirely:

```bash
# In appsettings.Development.json:
{
  "ContentSync": {
    "Enabled": false
  }
}

Run
```

**Output**:

```text
[10:40:00] Content sync disabled (ContentSync:Enabled = false)
[10:40:00] Repository initialized from existing database
[10:40:00] Servers starting...
```

## Running Tests

### All Tests (Default)

```bash
Run
# Runs PowerShell, unit, integration, component, and E2E tests
```

### Specific Test Projects

```bash
# PowerShell tests only (fastest - no .NET build)
Run -TestProject powershell

# Integration tests only
Run -TestProject Infrastructure.Tests

# E2E tests only (requires servers)
Run -TestProject E2E.Tests
```

### No Tests (Debugging)

```bash
Run -WithoutTests
# Skips all tests, starts servers directly
```

## Architecture Overview

```text
Markdown Files (Git)
  ↓
ContentSyncService (SHA256 hash-based diff)
  ↓
PostgreSQL / SQLite Database
  ↓
IContentRepository (Dapper)
  ↓
API / Blazor Components
```

## Key Concepts

### Content Sync

- **Hash-based change detection**: SHA256 of file content determines INSERT/UPDATE/DELETE
- **Transactional**: Entire sync runs in transaction, rollback on any error
- **Fail-fast**: App won't start if sync fails (immediate visibility)
- **Skippable**: Set `ContentSync:Enabled = false` for fast iteration

### Database Providers

| Provider | When Used | Connection |
|----------|-----------|------------|
| SQLite | Local dev, integration tests | In-process, techhub.db file |
| PostgreSQL | E2E tests, production | Docker localhost:5432 or Azure PostgreSQL |

### Repository Pattern

```csharp
// Existing methods (database-backed)
var item = await _repository.GetBySlugAsync("videos", "my-video");
var items = await _repository.GetBySectionAsync("ai");

// New search methods
var results = await _repository.SearchAsync(new SearchRequest
{
    Query = "agent framework",
    Tags = ["AI", "Azure"],
    Sections = ["ai"],
    Take = 20
});

var facets = await _repository.GetFacetsAsync(new FacetRequest
{
    Sections = ["ai"],
    FacetFields = ["tags", "collections"]
});
```

## Testing Workflows

### Unit Tests

Test individual components in isolation:

```bash
# Core domain logic
dotnet test tests/TechHub.Core.Tests

# Infrastructure services
dotnet test tests/TechHub.Infrastructure.Tests
```

### Integration Tests

Test database operations with in-memory SQLite:

```bash
dotnet test tests/TechHub.Infrastructure.Tests
```

### E2E Tests

Test complete user workflows with Docker PostgreSQL:

```bash
# CRITICAL: Use Run function, not direct dotnet test
Run -TestProject E2E.Tests

# Direct dotnet test WILL FAIL (servers not running)
```

## Common Tasks

### Force Full Re-import

```bash
# In appsettings.Development.json:
{
  "ContentSync": {
    "ForceFullSync": true
  }
}

Run
# Deletes all database content and re-imports everything
```

### Clean Build

```bash
Run -Clean
# Cleans bin/obj directories before building
```

### View Logs

```text
Console: .tmp/logs/console.log (Development) or api/web-console.log (Production)
API logs: .tmp/logs/api-dev.log
Web logs: .tmp/logs/web-dev.log
```

### Database Inspection

**SQLite**:

```bash
# Install SQLite CLI
sqlite3 techhub.db

# Run queries
SELECT COUNT(*) FROM content_items;
SELECT * FROM content_tags LIMIT 10;
```

**PostgreSQL**:

```bash
# Connect to Docker container
docker exec -it techhub-postgres-1 psql -U techhub -d techhub

# Run queries
SELECT COUNT(*) FROM content_items;
SELECT * FROM content_tags LIMIT 10;
```

## Performance Targets

| Metric | Target | Actual |
|--------|--------|--------|
| First sync (4000+ files) | < 60s | ~45s |
| Subsequent sync (no changes) | < 1s | ~850ms |
| Tag filtering | < 200ms | TBD |
| Full-text search | < 300ms | TBD |

## Troubleshooting

### Sync Fails Mid-Way

**Error**: "Sync failed: Unable to parse markdown file X"

**Solution**: Fix the markdown file, restart app. Transaction rollback ensures no partial state.

### E2E Tests Fail

**Error**: "Connection refused to localhost:5001"

**Solution**: Use `Run -TestProject E2E.Tests` instead of direct `dotnet test`. Servers must be running.

### SQLite Database Locked

**Error**: "Database is locked"

**Solution**: Stop all running processes, delete techhub.db, restart app.

### PostgreSQL Container Won't Start

**Error**: "Port 5432 already in use"

**Solution**:

```bash
# Stop any existing PostgreSQL
docker-compose down
sudo systemctl stop postgresql  # If PostgreSQL installed on host

# Restart
docker-compose up -d postgres
```

## Next Steps

1. Read [data-model.md](data-model.md) - Complete database schema
2. Read [research.md](research.md) - Technical decisions and patterns
3. Review [contracts/](contracts/) - Interface definitions
4. See [src/AGENTS.md](../../src/AGENTS.md) - .NET implementation patterns
5. See [tests/AGENTS.md](../../tests/AGENTS.md) - Testing strategies
