# Infrastructure Tests - Tech Hub

> **AI CONTEXT**: This is a **LEAF** context file for Infrastructure tests in the `tests/TechHub.Infrastructure.Tests/` directory. It complements the [tests/AGENTS.md](../AGENTS.md) testing strategy.
> **RULE**: Follow the 10-step workflow in Root [AGENTS.md](../../AGENTS.md). Project principles are in [README.md](../../README.md). Follow **BOTH**.

## Overview

This directory contains **unit and integration tests** for the Tech Hub Infrastructure layer using **xUnit**. These tests validate repositories, services, parsers, and data access logic with real file operations.

**Implementation being tested**: See [src/TechHub.Infrastructure/AGENTS.md](../../src/TechHub.Infrastructure/AGENTS.md) for repository and service patterns.

## What This Directory Contains

**Test Files**: xUnit test classes that validate infrastructure behavior:

- `FrontMatterParserTests.cs` - Tests for YAML front matter parsing
- `MarkdownServiceTests.cs` - Tests for markdown rendering
- `Repositories/SectionRepositoryTests.cs` - Tests for section repository
- `Repositories/ContentRepositoryTests.cs` - Tests for content repository

## Testing Strategy

**What to Test**:

- ✅ **File parsing** (YAML front matter, markdown)
- ✅ **Repository operations** (GetAll, GetById, filtering)
- ✅ **Data transformations** (DTO mapping, normalization)
- ✅ **Caching behavior** (when applicable)
- ✅ **Error handling** (invalid files, missing data)
- ✅ **Real file I/O** (read actual test files)

**What NOT to Test**:

- ❌ **Domain logic** (belongs in Core tests)
- ❌ **API endpoints** (belongs in API integration tests)
- ❌ **Markdown library internals** (Markdig is external)

## Test Patterns

### Testing Singleton Services

**🚨 CRITICAL**: Services registered as **Singleton** in production MUST be tested with shared instances. See [tests/AGENTS.md](../AGENTS.md#testing-singleton-services) for the full pattern and rationale.

**Key Points**:

- Create shared instance in test constructor (mimics production)
- Add parallel execution test to catch mutable state bugs
- Services: `MarkdownService`, `SectionRepository`, `ContentRepository`, `RssService`

### What to Test

**File Parsing**:

- Valid YAML front matter extracts correctly
- Invalid YAML throws appropriate exceptions
- Content body separated from front matter

**Repository Operations**:

- `GetAllAsync` returns content sorted by `DateEpoch` descending
- Content is read from real `collections/` directory
- Caching behavior works correctly

**Error Handling**:

- Missing end marker in YAML
- Malformed front matter
- Missing required fields

See actual tests for implementation examples.

## Running Tests

**Use the Run function for all test execution** (see [README.md - Starting, Stopping and Testing](../../README.md#starting-stopping-and-testing-the-website)):

```powershell
# Run all tests (recommended)
Run

# Run only Infrastructure integration tests
Run -TestProject Infrastructure.Tests

# Run specific test class
Run -TestName FrontMatterParser

# Or run repository tests only
Run -TestName Repository
```

## Best Practices

1. **Use real files** from `collections/` directory (not mocked)
2. **Test timezone handling** - Verify Europe/Brussels timezone
3. **Test sorting** - Content MUST be sorted by DateEpoch descending
4. **Verify caching** - Ensure repeated calls use cached data
5. **Test error cases** - Invalid YAML, missing files
6. **Use FluentAssertions** for collection assertions
7. **Dispose resources** - Clean up file handles and caches

## Common Pitfalls

❌ **Don't mock file system** in infrastructure tests (use real files)  
❌ **Don't forget timezone** - Always test with Europe/Brussels  
❌ **Don't skip sorting tests** - Descending DateEpoch is CRITICAL  
❌ **Don't test Markdig internals** (it's a third-party library)  
❌ **Don't leave test files** in collections/ (use existing content)

## Test Data Location

```text
collections/
├── _news/           # Real news content for testing
├── _videos/         # Real video content for testing
│   ├── ghc-features/
│   └── vscode-updates/
├── _community/      # Real community content for testing
├── _blogs/          # Real blog content for testing
└── _roundups/       # Real roundup content for testing
```

## Related Documentation

- [tests/AGENTS.md](../AGENTS.md) - Complete testing strategy
- [src/TechHub.Infrastructure/AGENTS.md](../../src/TechHub.Infrastructure/AGENTS.md) - Infrastructure patterns
- [Root AGENTS.md](../../AGENTS.md#step-6-write-tests-first-tdd) - When to write tests
- [README.md](../../README.md#timezone--date-handling) - Timezone requirements
