# AGENTS.md Files Are Mandatory

This repository uses nested `AGENTS.md` files to provide domain-specific instructions for AI agents. These files contain critical rules, conventions, and patterns that MUST be followed.

## Rule

Before modifying any file, you MUST read the `AGENTS.md` in that file's directory **and** every parent directory up to the repository root. Read from root down to the most specific level.

### Example

Editing `src/TechHub.Api/Controllers/FooController.cs` requires reading, in order:

1. `AGENTS.md` (root — mandatory workflow)
2. `src/AGENTS.md` (source code conventions)
3. `src/TechHub.Api/AGENTS.md` (API-specific rules)

### When multiple domains are affected

If your change spans directories (e.g., API + Web + Tests), read the `AGENTS.md` for **every** affected directory tree. For example, an API contract change requires reading the API, Web, and corresponding test project AGENTS.md files.

## Current AGENTS.md locations

- `/` — Root workflow (8-step process, critical rules)
- `src/` — Source code conventions
- `src/TechHub.Api/` — API project rules
- `src/TechHub.Web/` — Web project rules
- `src/TechHub.Core/` — Core domain rules
- `src/TechHub.Infrastructure/` — Infrastructure rules
- `tests/` — Testing strategy and patterns
- `tests/TechHub.Api.Tests/` — API test conventions
- `tests/TechHub.Web.Tests/` — Web test conventions
- `tests/TechHub.Core.Tests/` — Core test conventions
- `tests/TechHub.Infrastructure.Tests/` — Infrastructure test conventions
- `tests/TechHub.E2E.Tests/` — E2E test conventions
- `tests/TechHub.TestUtilities/` — Test utilities conventions
- `tests/javascript/` — JavaScript test conventions
- `tests/powershell/` — PowerShell test conventions
- `scripts/` — Script conventions
- `docs/` — Documentation rules

## Do NOT skip this step

Even for "simple" tasks. The AGENTS.md files contain project-specific constraints (e.g., never use `dotnet` directly, use the `Run` command; never use `Import-Module`; always use Europe/Brussels timezone) that cannot be inferred from code alone.
