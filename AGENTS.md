# AI Assistant Workflow

**This file is for AI coding agents.** It defines the mandatory workflow for all development tasks.

Read the [critical rules](#critical-rules) first, then follow the [8-step workflow](#the-8-step-workflow).

---

## Critical Rules

### ✅ Always Do

- **Always give intermediate updates when analyzing and thinking**, especially if you have a theory and then come to a conclusion but need to investigate further. First share the conclusion.
- **Always use `Run` function** for all build/test/run operations — see [docs/running-and-testing.md](docs/running-and-testing.md)
- **Always check for errors after editing files** (`get_errors` tool)
- **Always fix all linter issues**
- **Always store temp files, scripts, output, etc in `.tmp/`**
- **Always use PowerShell for scripts** (save as `.ps1`, then execute)
- **Always follow timezone standard**: `Europe/Brussels`
- **Always be direct and concise**
- **Always go back to Step 2** when struggling or working on something you haven't read docs for

### ⚠️ Ask First

- Breaking changes to public APIs
- Adding new dependencies
- Cross-domain changes (API + Web + Infrastructure)
- Significant refactoring

### 🚫 Never Do

- **Never use `| head`, `| tail`, `Select-Object -Last`** (blocks output)
- **Never paste scripts into terminal** (save as file first)
- **Never commit secrets or API keys**
- **Never hardcode section/collection data**
- **Never assume UTC** (use Europe/Brussels)
- **Never hide errors or swallow exceptions** — see [src/AGENTS.md](src/AGENTS.md#error-handling-philosophy)
- **Never use `Import-Module` to load `TechHubRunner.psm1`** — it auto-imports. Force reload only if you changed the psm1 file.
- **Never use lowlevel `dotnet` or `Stop-Process` or `kill`** — the `Run` command handles all that. Use `Stop-Servers` if you really must kill processes.
- **Never write Python scripts** — always use PowerShell (`.ps1`). This applies to one-off scripts, data processing, terminal commands, and any ad-hoc scripting.

---

## The 8-Step Workflow

### 1. Review the codebase and create an initial plan

1. Read [docs/repository-structure.md](docs/repository-structure.md) to understand the repository
2. Explore relevant code with `read_file`, `grep_search`, `semantic_search`
3. Check existing tests to understand expected behavior
4. Create an initial plan (files to modify, tests needed, docs to update)

### 2. Gather context from documentation and validate the plan

1. **Read functional docs** — Explore the `docs/` directory to find relevant documentation. This is crucial so never skip reading the actual functional docs!
2. **Read domain AGENTS.md** files for the area you're working in. They are nested — read parent AGENTS.md files too. Example for an API change: [src/AGENTS.md](src/AGENTS.md) → [src/TechHub.Api/AGENTS.md](src/TechHub.Api/AGENTS.md) → [src/TechHub.Web/AGENTS.md](src/TechHub.Web/AGENTS.md) (if contract changes) → [tests/AGENTS.md](tests/AGENTS.md) → [tests/TechHub.Api.Tests/AGENTS.md](tests/TechHub.Api.Tests/AGENTS.md)
3. **Use context7 MCP tool** for latest framework/library docs (MANDATORY for new features/big changes)
4. Validate plan against documentation; update if better approaches exist

### 3. Verify current behavior before making changes (Optional)

For reproducing bugs or understanding complex interactions:

1. Start website: `Run -WithoutTests`
2. Use Playwright MCP tools for browser testing

### 4. Write tests BEFORE implementing changes (TDD)

1. Read [docs/running-and-testing.md](docs/running-and-testing.md) for test execution
2. Read [tests/AGENTS.md](tests/AGENTS.md) for testing patterns
3. Run ALL tests first: `Run` — fix any broken tests before proceeding
4. Write failing tests that define the expected behavior
5. Verify they fail for the right reason

**Test Types Required**:

| Change Type | Required Tests |
|---|---|
| Bug fix | Failing test reproducing bug |
| New feature | **Integration (MANDATORY)** + Unit (edge cases) + E2E (critical paths) |
| API changes | **Integration tests (MANDATORY)** |
| UI/frontend | Component + **E2E (MANDATORY)** + Integration for API endpoints |
| Backend-only | **Integration (MANDATORY)** + Unit (edge cases) |
| Documentation-only | None |

### 5. Implement changes to make tests pass

1. Write ONLY enough code to make tests pass
2. Follow existing patterns and conventions
3. Run `get_errors` after each file edit; fix all issues
4. Run tests frequently
5. **If struggling** → go back to Step 2 and gather more context

### 6. Validate all tests pass and code quality is high

1. Run full test suite: `Run`
2. Use `get_errors` for linting/compilation
3. Verify no regressions

### 7. Update documentation if you made changes

1. Read [docs/AGENTS.md](docs/AGENTS.md) to understand documentation placement
2. Use `grep_search` to find docs mentioning modified features
3. Update relevant docs or introduce new files
4. Fix markdown linting: `npx markdownlint-cli2 --fix <file> --config /workspaces/techhub/.markdownlint-cli2.jsonc`

### 8. Report completion with a clear summary

Provide concise summary of changes, confirm tests pass, note any caveats.
