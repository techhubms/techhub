# Blazor Component Tests - Tech Hub

> **AI CONTEXT**: This is a **LEAF** context file for Blazor component tests in the `tests/TechHub.Web.Tests/` directory. It complements the [tests/AGENTS.md](../AGENTS.md) testing strategy.
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Overview

This directory contains **component tests** for Tech Hub Blazor components using **bUnit**. These tests validate component rendering, interactivity, and state management without requiring a browser.

**Implementation being tested**: See [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) for Blazor component patterns.

## What This Directory Contains

**Test Files**: bUnit test classes that validate Blazor component behavior:

- `Components/Pages/` - Tests for Blazor page components
- `Components/Shared/` - Tests for shared UI components (future)
- `Components/Layout/` - Tests for layout components (future)

## Testing Strategy

**What to Test**:

- ✅ **Component rendering** (output HTML structure)
- ✅ **Parameter binding** (prop changes update UI)
- ✅ **Event handlers** (button clicks, form submissions)
- ✅ **Conditional rendering** (if/else, null checks)
- ✅ **Component lifecycle** (OnInitialized, OnParametersSet)
- ✅ **Dependency injection** (mocked services)

**What NOT to Test**:

- ❌ **Complete user workflows** (belongs in E2E tests)
- ❌ **Browser APIs** (localStorage, sessionStorage)
- ❌ **HTTP requests** (mock API clients)
- ❌ **Razor syntax** (framework responsibility)

## Test Patterns

### Basic Component Rendering

```csharp
public class SectionCardTests : TestContext
{
    [Fact]
    public void SectionCard_WithSection_RendersTitle()
    {
        // Arrange
        var section = new SectionDto
        {
            Title = "Test Section",
            Description = "Test description",
            Url = "test-section"
        };
        
        // Act
        var cut = RenderComponent<SectionCard>(parameters => parameters
            .Add(p => p.Section, section));
        
        // Assert
        cut.Find("h2").TextContent.Should().Be("Test Section");
    }
}
```

### Testing with Mocked Services

```csharp
public class SectionIndexTests : TestContext
{
    [Fact]
    public async Task SectionIndex_OnInitialized_LoadsContent()
    {
        // Arrange
        var mockApiClient = Substitute.For<ITechHubApiClient>();
        var section = new SectionDto { Url = "ai", Title = "AI" };
        var content = new List<ContentItemDto> { /* test data */ };
        
        mockApiClient.GetSectionAsync("ai").Returns(section);
        mockApiClient.GetContentAsync("ai").Returns(content);
        
        Services.AddSingleton(mockApiClient);
        
        // Act
        var cut = RenderComponent<SectionIndex>(parameters => parameters
            .Add(p => p.SectionUrl, "ai"));
        
        // Assert
        cut.WaitForState(() => cut.Instance.Section != null);
        cut.Instance.Section.Should().NotBeNull();
        cut.Instance.AllItems.Should().HaveCount(content.Count);
    }
}
```

### Testing Event Handlers

```csharp
[Fact]
public void FilterButton_WhenClicked_UpdatesFilter()
{
    // Arrange
    var cut = RenderComponent<FilterControls>();
    var button = cut.Find("button.filter-tag");
    
    // Act
    button.Click();
    
    // Assert
    cut.Instance.SelectedTags.Should().Contain("ai");
}
```

### Testing Conditional Rendering

```csharp
[Fact]
public void SectionIndex_WhenSectionIsNull_ShowsNotFound()
{
    // Arrange
    var mockApiClient = Substitute.For<ITechHubApiClient>();
    mockApiClient.GetSectionAsync(Arg.Any<string>()).Returns((SectionDto?)null);
    
    Services.AddSingleton(mockApiClient);
    
    // Act
    var cut = RenderComponent<SectionIndex>(parameters => parameters
        .Add(p => p.SectionUrl, "nonexistent"));
    
    // Assert
    cut.WaitForState(() => cut.Instance.Section == null);
    cut.Find(".not-found").Should().NotBeNull();
}
```

## Running Tests

```powershell
# Run all Blazor component tests
dotnet test tests/TechHub.Web.Tests

# Run specific component tests
dotnet test tests/TechHub.Web.Tests --filter "FullyQualifiedName~SectionCardTests"

# Run with detailed output
dotnet test tests/TechHub.Web.Tests --logger "console;verbosity=detailed"
```

## Best Practices

1. **Use bUnit's TestContext** - Inherit from TestContext base class
2. **Mock injected services** - Use Moq for API clients
3. **Test user perspective** - Find elements by CSS selectors, not implementation
4. **Use WaitForState** - For async component initialization
5. **Test accessibility** - Verify ARIA labels, semantic HTML
6. **Isolate component tests** - Each test creates its own instance
7. **Clean test names** - `ComponentName_Scenario_ExpectedResult`

## Common Pitfalls

❌ **Don't test Blazor internals** (component lifecycle is framework's job)  
❌ **Don't test CSS styling** (component tests verify structure, not appearance)  
❌ **Don't use real HTTP clients** (always mock ITechHubApiClient)  
❌ **Don't test browser APIs** (use E2E tests for localStorage, etc.)  
❌ **Don't forget async initialization** (use WaitForState for OnInitializedAsync)

## bUnit Setup

```csharp
public class ComponentTestBase : TestContext
{
    public ComponentTestBase()
    {
        // Register common services
        Services.AddSingleton(Substitute.For<NavigationManager>());
        Services.AddSingleton(Substitute.For<ITechHubApiClient>());
        
        // Add logging (optional)
        Services.AddLogging();
    }
}
```

## Related Documentation

- [tests/AGENTS.md](../AGENTS.md) - Complete testing strategy
- [src/TechHub.Web/AGENTS.md](../../src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [Root AGENTS.md](/AGENTS.md#6-test--validate) - When to write tests
- [bUnit Documentation](https://bunit.dev/) - Official bUnit docs

