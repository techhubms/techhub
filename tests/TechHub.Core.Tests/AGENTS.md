# Core Unit Tests - Tech Hub

> **AI CONTEXT**: This is a **LEAF** context file for Core unit tests in the `tests/TechHub.Core.Tests/` directory. It complements the [tests/AGENTS.md](../AGENTS.md) testing strategy.
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Overview

This directory contains **unit tests** for the Tech Hub Core domain layer using **xUnit**. These tests validate domain models, business logic, and pure functions in complete isolation with no external dependencies.

## What This Directory Contains

**Test Files**: xUnit test classes that validate domain model behavior:

- `Models/ContentItemTests.cs` - Tests for ContentItem domain model
- `Models/SectionTests.cs` - Tests for Section domain model (future)
- `Extensions/` - Tests for extension methods (future)

## Testing Strategy

**What to Test**:

- ✅ **Domain model behavior** (GetUrlInSection, property initialization)
- ✅ **Business rules** (validation, constraints)
- ✅ **Value calculations** (derived properties)
- ✅ **Extension methods** (pure functions)
- ✅ **Edge cases** (null handling, boundary conditions)

**What NOT to Test**:

- ❌ **Framework features** (C# record equality, property getters)
- ❌ **Simple property assignments** (no logic to test)
- ❌ **External dependencies** (file I/O, HTTP calls - use mocks)

## Test Patterns

### Testing Domain Model Methods

```csharp
public class ContentItemTests
{
    [Theory]
    [InlineData("ai", "/ai/videos/example.html")]
    [InlineData("github-copilot", "/github-copilot/videos/example.html")]
    public void GetUrlInSection_ReturnsCorrectUrl(string sectionUrl, string expectedUrl)
    {
        // Arrange
        var item = new ContentItem
        {
            Slug = "example",
            CollectionName = "videos",
            // ... other required properties
        };
        
        // Act
        var url = item.GetUrlInSection(sectionUrl);
        
        // Assert
        url.Should().Be(expectedUrl);
    }
}
```

### Testing Record Equality

```csharp
[Fact]
public void ContentItem_WithSameValues_AreEqual()
{
    // Arrange
    var item1 = CreateContentItem("slug-1");
    var item2 = CreateContentItem("slug-1");
    
    // Act & Assert
    item1.Should().Be(item2);
}
```

### Testing Edge Cases

```csharp
[Theory]
[InlineData(null)]
[InlineData("")]
[InlineData("   ")]
public void GetUrlInSection_WithInvalidSection_ThrowsException(string invalidSection)
{
    // Arrange
    var item = CreateContentItem("test");
    
    // Act
    Action act = () => item.GetUrlInSection(invalidSection);
    
    // Assert
    act.Should().Throw<ArgumentException>();
}
```

## Running Tests

```powershell
# Run all Core tests
dotnet test tests/TechHub.Core.Tests

# Run specific test class
dotnet test tests/TechHub.Core.Tests --filter "FullyQualifiedName~ContentItemTests"

# Run with code coverage
dotnet test tests/TechHub.Core.Tests --collect:"XPlat Code Coverage"
```

## Best Practices

1. **Test behavior, not implementation** - Focus on what the method does, not how
2. **Use [Theory] for multiple cases** - DRY principle for similar scenarios
3. **Use FluentAssertions** - More readable than Assert.Equal
4. **Test edge cases** - Null, empty, boundary values
5. **Fast tests** - No I/O, no external dependencies
6. **Clear test names** - `MethodName_Scenario_ExpectedResult`
7. **Use helper methods** - Create test data factories for complex objects

## Common Pitfalls

❌ **Don't test C# language features** (record equality already works)  
❌ **Don't test simple getters/setters** (no logic = no test needed)  
❌ **Don't use real file paths** (use test data factories)  
❌ **Don't mock what you're testing** (only mock external dependencies)  
❌ **Don't share state** between tests (each test should be isolated)

## Test Data Factories

```csharp
private static ContentItem CreateContentItem(string slug) => new()
{
    Slug = slug,
    Title = $"Test Title for {slug}",
    Description = "Test description",
    DateEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
    CollectionName = "videos",
    Categories = new[] { "ai" },
    Tags = new[] { "test" },
    RenderedHtml = "<p>Test content</p>",
    Excerpt = "Test excerpt"
};
```

## Related Documentation

- [tests/AGENTS.md](../AGENTS.md) - Complete testing strategy
- [src/TechHub.Core/AGENTS.md](../../src/TechHub.Core/AGENTS.md) - Domain model patterns
- [Root AGENTS.md](/AGENTS.md#6-test--validate) - When to write tests
