# Documentation Synchronization Checklist

This checklist ensures documentation accurately reflects the current codebase state.

## Pre-Sync Verification

Before starting documentation sync, verify:

- [ ] Solution builds without errors
- [ ] All tests pass
- [ ] You have latest code from current branch

## Core Documentation Files

### Repository Root

| File | Check | Notes |
| ---- | ----- | ----- |
| `README.md` | [ ] Quick start instructions work | Test the commands locally |
| `README.md` | [ ] Architecture overview is accurate | Compare with actual project structure |
| `README.md` | [ ] Links are not broken | Use `verify-documentation.ps1` |
| `AGENTS.md` | [ ] Workflow steps are current | Verify commands execute correctly |
| `AGENTS.md` | [ ] Tech stack list is accurate | Check package references |
| `MIGRATIONSTATUS.md` | [ ] Progress reflects reality | Count actual completed tasks |

### Functional Documentation (`docs/`)

| File | Check | Notes |
| ---- | ----- | ----- |
| `api-specification.md` | [ ] All endpoints documented | Compare with actual routes in API |
| `api-specification.md` | [ ] Request/response formats accurate | Test with actual API |
| `api-specification.md` | [ ] Error codes complete | Check exception handling |
| `filtering-system.md` | [ ] Filter parameters correct | Test filtering functionality |
| `content-management.md` | [ ] Workflows are current | Verify steps work |
| `rss-feeds.md` | [ ] Feed URLs accurate | Test feed endpoints |

### Domain AGENTS.md Files

| File | Check | Notes |
| ---- | ----- | ----- |
| `AGENTS.md` | [ ] Workflow steps are current | Root development guide |
| `src/AGENTS.md` | [ ] Patterns match code | Check actual implementations |
| `src/TechHub.Api/AGENTS.md` | [ ] Endpoint patterns current | Compare with actual endpoints |
| `src/TechHub.Web/AGENTS.md` | [ ] Component patterns accurate | Check Razor files |
| `src/TechHub.Core/AGENTS.md` | [ ] Model definitions match | Compare with actual models |
| `src/TechHub.Infrastructure/AGENTS.md` | [ ] Repository patterns accurate | Check implementations |
| `docs/AGENTS.md` | [ ] Documentation rules current | Verify placement strategy |
| `collections/AGENTS.md` | [ ] Content patterns valid | Check markdown guidelines |
| `scripts/AGENTS.md` | [ ] Script patterns valid | Execute examples |
| `tests/AGENTS.md` | [ ] Test patterns work | Run example tests |
| `tests/TechHub.Api.Tests/AGENTS.md` | [ ] API test patterns current | Compare with actual tests |
| `tests/TechHub.Core.Tests/AGENTS.md` | [ ] Unit test patterns current | Compare with actual tests |
| `tests/TechHub.Web.Tests/AGENTS.md` | [ ] Component test patterns current | Compare with actual tests |
| `tests/TechHub.Infrastructure.Tests/AGENTS.md` | [ ] Infrastructure test patterns current | Compare with actual tests |
| `tests/TechHub.E2E.Tests/AGENTS.md` | [ ] E2E test patterns current | Compare with actual tests |
| `tests/powershell/AGENTS.md` | [ ] PowerShell test patterns current | Compare with actual tests |

## API Endpoint Verification

For each endpoint in the API:

```markdown
- [ ] Endpoint: `GET /api/sections`
  - [ ] Documented in api-specification.md
  - [ ] Request parameters correct
  - [ ] Response format accurate
  - [ ] Error responses documented
```

### Automated Check

Run the verification script:

```bash
pwsh -File .github/skills/cleanup/scripts/verify-documentation.ps1 -OutputFormat Markdown
```

## Code Comment Verification

### Inline Comments

- [ ] Complex logic has explanatory comments
- [ ] TODO comments have associated issues
- [ ] HACK comments explain the workaround reason
- [ ] No outdated comments that contradict code

## Configuration Documentation

| Configuration | Check | Notes |
| ------------- | ----- | ----- |
| `appsettings.json` | [ ] All sections documented | Check against actual config |
| `Directory.Build.props` | [ ] Build settings explained | Verify in AGENTS.md |
| `.editorconfig` | [ ] Coding style documented | Link from AGENTS.md |

## Cross-Reference Verification

- [ ] All internal links resolve correctly
- [ ] No orphaned documentation files
- [ ] No unresolved circular references (A→B→A with no actual content; mutual cross-references that provide value are fine)
- [ ] Index files list all children

## Post-Sync Actions

After completing sync:

1. [ ] Run `verify-documentation.ps1` - all checks pass
2. [ ] Build solution - no warnings about XML docs
3. [ ] Run tests - documentation tests pass
4. [ ] Review changes in diff before commit

## Common Issues to Fix

### Outdated Content

```markdown
❌ "Uses Jekyll to build the site..."
✅ "Uses .NET Blazor for server-side rendering..."
```

### Broken Links

```markdown
❌ [See details](docs/old-file.md)
✅ [See details](docs/api-specification.md)
```

### Incorrect Commands

```markdown
❌ `bundle exec jekyll serve`
✅ `Run` function (auto-loaded PowerShell function)
```

### Missing Sections

When new features are added:

1. Update relevant AGENTS.md file
2. Add to api-specification.md if API endpoint
3. Update README.md if user-facing

## Automation Scripts

| Script | Purpose |
| ------ | ------- |
| `verify-documentation.ps1` | Check all documentation |
| `find-dead-code.ps1` | Find potentially unused code |
| `dotnet format --verify-no-changes` | Verify code formatting |

## Final Checklist

Before marking documentation sync complete:

- [ ] All automated checks pass
- [ ] Manual review completed
- [ ] No TODO items remaining
- [ ] Changes committed with descriptive message
