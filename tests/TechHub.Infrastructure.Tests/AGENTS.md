# Infrastructure Tests - Tech Hub

> **AI CONTEXT**: This is a **LEAF** context file for Infrastructure tests in the `tests/TechHub.Infrastructure.Tests/` directory. It complements the [tests/AGENTS.md](../AGENTS.md) testing strategy.
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

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

### Testing File Parsing

```csharp
public class FrontMatterParserTests
{
    [Fact]
    public void ParseFrontMatter_WithValidYaml_ReturnsMetadata()
    {
        // Arrange
        var markdown = @"---
title: Test Article
date: 2026-01-07
categories: [ai, ml]
---

# Content here
";
        var parser = new FrontMatterParser();
        
        // Act
        var result = parser.Parse(markdown);
        
        // Assert
        result.FrontMatter.Should().ContainKey("title");
        result.FrontMatter["title"].Should().Be("Test Article");
        result.Content.Should().Contain("# Content here");
    }
}
```

### Testing Repository with Real Files

```csharp
public class ContentRepositoryTests
{
    private readonly IContentRepository _repository;
    private readonly IOptions<ContentOptions> _options;
    
    public ContentRepositoryTests()
    {
        // Use test data directory
        _options = Options.Create(new ContentOptions
        {
            CollectionsRootPath = "../../../../collections",
            Timezone = "Europe/Brussels"
        });
        
        _repository = new FileContentRepository(
            _options,
            new MemoryCache(Options.Create(new MemoryCacheOptions())),
            new MarkdownService(),
            NullLogger<FileContentRepository>.Instance
        );
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsContentSortedByDateDescending()
    {
        // Act
        var items = await _repository.GetAllAsync();
        
        // Assert
        items.Should().NotBeEmpty();
        items.Should().BeInDescending Order(x => x.DateEpoch);
    }
}
```

### Testing Error Handling

```csharp
[Fact]
public void ParseFrontMatter_WithInvalidYaml_ThrowsException()
{
    // Arrange
    var invalidMarkdown = @"---
title: Missing end marker
content here";
    var parser = new FrontMatterParser();
    
    // Act
    Action act = () => parser.Parse(invalidMarkdown);
    
    // Assert
    act.Should().Throw<InvalidOperationException>();
}
```

## Running Tests

```powershell
# Run all Infrastructure tests
dotnet test tests/TechHub.Infrastructure.Tests

# Run specific test class
dotnet test tests/TechHub.Infrastructure.Tests --filter "FullyQualifiedName~FrontMatterParserTests"

# Run repository tests only
dotnet test tests/TechHub.Infrastructure.Tests --filter "FullyQualifiedName~Repository"
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
- [Root AGENTS.md](/AGENTS.md#6-test--validate) - When to write tests
- [Root AGENTS.md](/AGENTS.md#timezone--date-handling) - Timezone requirements
