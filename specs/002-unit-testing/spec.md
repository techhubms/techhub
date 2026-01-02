# Unit Testing Strategy

## Overview

Defines the unit testing approach for Tech Hub .NET migration using xUnit, FluentAssertions, and Moq. This specification ensures comprehensive test coverage for domain models, utilities, and business logic.

## Constitution Alignment

- **Test-Driven Development**: Write tests BEFORE or DURING implementation
- **Clean Architecture**: Test each layer independently
- **Configuration-Driven**: Test configuration validation and loading

## Functional Requirements

### FR-001: Testing Framework Setup

The solution must use xUnit 2.9.3 as the primary test framework for all unit tests across Core, Infrastructure, Web, and API projects.

**Rationale**: xUnit provides modern testing capabilities including parallel execution, theory support for parameterized tests, and minimal test class instantiation overhead.

### FR-002: Assertion Library

The solution must use FluentAssertions 7.0.0 as the assertion library for all test projects, providing readable and expressive assertions with detailed error messages.

**Rationale**: FluentAssertions improves test readability with natural language assertions (e.g., `result.Should().Be(expected)`) and provides superior error messages compared to xUnit's built-in `Assert` class.

### FR-003: Mocking Framework

The solution must use Moq 4.20.72 as the mocking library for creating test doubles of interfaces and abstract classes in unit tests.

**Rationale**: Moq is the industry-standard mocking framework for .NET with simple syntax for setting up mock behaviors and verifying interactions.

### FR-004: Code Coverage Collection

The solution must integrate coverlet.collector 6.0.2 for code coverage collection during test execution, enabling coverage reports for all test projects.

**Rationale**: Code coverage metrics identify untested code paths and ensure quality standards (80% minimum for Core/Infrastructure layers) are met.

### FR-005: Test Project Organization

Each source project (TechHub.Core, TechHub.Infrastructure, TechHub.Api, TechHub.Web) must have a corresponding test project in `/dotnet/tests/` directory with `.Tests` suffix.

**Rationale**: Parallel project structure ensures clear test organization, easy navigation, and maintains separation of concerns between production and test code.

### FR-006: Naming Conventions

All test classes must follow `{ClassUnderTest}Tests` naming pattern, and test methods must follow `{MethodName}_{Scenario}_{ExpectedOutcome}` pattern for consistency and clarity.

**Rationale**: Standardized naming ensures tests are self-documenting, easily discoverable, and immediately convey what functionality is being tested and under what conditions.

### FR-007: AAA Pattern Structure

All unit tests must follow the Arrange-Act-Assert (AAA) pattern with clearly separated sections using comments (`// Arrange`, `// Act`, `// Assert`).

**Rationale**: AAA pattern enforces consistent test structure, improves readability, and makes test intent explicit by separating setup, execution, and verification.

### FR-008: Global Using Directives

Each test project must include a `GlobalUsings.cs` file with common using directives (Xunit, FluentAssertions, Moq, domain namespaces) to reduce repetitive imports across test files.

**Rationale**: Global usings eliminate boilerplate code in every test file, improve maintainability, and ensure consistent namespace imports across all tests.

### FR-009: Theory Tests for Parameterization

Unit tests must use xUnit's `[Theory]` attribute with `[InlineData]` or `[MemberData]` for parameterized testing when testing same logic with multiple input combinations.

**Rationale**: Theory tests reduce code duplication, increase test coverage with minimal code, and make edge cases and boundary conditions explicit through data-driven testing.

### FR-010: Exception Testing

Unit tests must validate exception handling using FluentAssertions' `Should().Throw<TException>()` syntax with verification of exception type, message, and parameter names where applicable.

**Rationale**: Proper exception testing ensures error handling works correctly, error messages are helpful, and failures provide appropriate context for debugging.

### FR-011: Async Method Testing

Unit tests for asynchronous methods must use `async Task` return types and FluentAssertions' `Should().ThrowAsync<TException>()` for exception scenarios, ensuring proper async/await patterns.

**Rationale**: Async tests verify that asynchronous operations complete correctly, handle cancellation tokens properly, and throw exceptions on the correct thread context.

### FR-012: Collection Assertions

Unit tests for collections must use FluentAssertions' collection-specific assertions (`.Should().HaveCount()`, `.Should().Contain()`, `.Should().BeEquivalentTo()`) for comprehensive validation.

**Rationale**: Collection-specific assertions provide better error messages, validate multiple collection properties in single assertions, and ensure complete collection state verification.

### FR-013: Test Fixtures for Shared Setup

Unit tests requiring expensive or shared setup (e.g., timezone configuration, test data) must use xUnit's `IClassFixture<T>` pattern to share setup across test class instances.

**Rationale**: Test fixtures optimize test execution by sharing expensive setup, ensure consistency across tests, and properly manage resource lifecycle through IDisposable.

### FR-014: Code Coverage Reporting

The solution must support generating HTML code coverage reports using `dotnet test --collect:"XPlat Code Coverage"` and ReportGenerator tool for visual coverage analysis.

**Rationale**: Visual coverage reports identify untested code paths, help prioritize testing efforts, and provide team visibility into test quality metrics.

### FR-015: Test Execution Performance

All unit tests in TechHub.Core.Tests project must execute in under 60 seconds total runtime, with individual test methods completing in under 1 second.

**Rationale**: Fast test execution enables rapid feedback during development, supports test-driven development workflow, and ensures CI/CD pipelines remain efficient.

## Success Criteria

### Test Framework Integration

- xUnit framework discovered and executes all test methods
- Test runner displays real-time test execution progress
- Parallel test execution works correctly across test classes
- Test failures provide clear diagnostic information with stack traces

### Code Quality & Coverage

- All test projects build without warnings
- Code coverage reports generate successfully for all projects
- Core and Infrastructure projects achieve 80% minimum coverage
- Domain models and utilities achieve 90%+ coverage
- Coverage reports identify uncovered code paths accurately

### Developer Experience

- Tests execute in under 60 seconds for all Core tests
- Test discovery completes in under 5 seconds in IDE
- FluentAssertions provide readable error messages on failures
- Test failures clearly indicate what was expected vs actual
- Developers can run single tests or test classes selectively

### Test Maintainability

- Test naming conventions followed consistently (95%+ compliance)
- AAA pattern used in all unit tests (100% compliance)
- No duplicated test logic across test files
- Global usings reduce using directives in test files
- Test helper methods eliminate setup duplication

## User Scenarios

### Scenario 1: Developer Writes First Test for New Model

**Actor**: Backend developer implementing new domain model

**Steps**:

1. Developer creates new domain model class in TechHub.Core (e.g., `Tag.cs`)
2. Developer creates corresponding test class `TagTests.cs` in `tests/TechHub.Core.Tests/Models/`
3. Developer writes first test method following naming convention: `Constructor_WithValidName_CreatesTag()`
4. Developer uses AAA pattern with FluentAssertions: `tag.Name.Should().Be("vscode")`
5. Developer runs test using `dotnet test` or IDE test runner
6. Test executes successfully and appears green in test explorer

**Outcome**: Developer successfully creates and runs first unit test with proper structure, sees immediate feedback, and understands test passed/failed state.

### Scenario 2: Developer Debugs Failing Test

**Actor**: Backend developer investigating test failure

**Steps**:

1. Developer runs test suite and sees `GetUrlInSection_WithAiSection_ReturnsCorrectUrl` failing
2. Test output shows FluentAssertions error: `Expected url to be "/ai/videos/test.html", but found "/videos/test.html"`
3. Developer sets breakpoint in test method's Act section
4. Developer runs test in debug mode using IDE or `dotnet test --debug`
5. Developer steps through code, inspects `item` object state in debugger
6. Developer identifies section parameter not being used in URL construction
7. Developer fixes implementation, re-runs test
8. Test passes with expected URL format

**Outcome**: Developer identifies root cause of test failure using descriptive assertions, debugs test execution, fixes implementation, and validates fix with green test.

### Scenario 3: Developer Adds Parameterized Tests for Edge Cases

**Actor**: Backend developer implementing comprehensive validation

**Steps**:

1. Developer has working test for `DateUtils.ParseIsoDate()` with valid input
2. Developer adds `[Theory]` attribute to test invalid date formats
3. Developer adds multiple `[InlineData("invalid")]`, `[InlineData("2025-13-01")]`, `[InlineData("")]` for edge cases
4. Developer updates test method to accept string parameter
5. Developer uses FluentAssertions exception testing: `act.Should().Throw<FormatException>()`
6. Developer runs theory test
7. Test executes 4 times (1 per InlineData) and all pass

**Outcome**: Developer efficiently tests multiple edge cases with single test method, validates all invalid inputs throw correct exceptions, and achieves comprehensive coverage with minimal code.

### Scenario 4: Developer Generates Code Coverage Report

**Actor**: Team lead reviewing test coverage before sprint review

**Steps**:

1. Team lead runs `dotnet test --collect:"XPlat Code Coverage"` in solution directory
2. Coverage files generate in `TestResults/` directories for each test project
3. Team lead runs ReportGenerator: `reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html`
4. HTML report generates in `coveragereport/` directory
5. Team lead opens `index.html` in browser
6. Report shows 85% coverage for TechHub.Core, 78% for TechHub.Infrastructure
7. Team lead drills into TechHub.Infrastructure to identify untested repository methods
8. Team lead creates tickets to improve coverage in identified areas

**Outcome**: Team lead successfully generates visual coverage report, identifies coverage gaps in Infrastructure layer, and creates actionable work items to improve test coverage.

## Acceptance Criteria

### xUnit Framework Integration

- [ ] xUnit 2.9.3 package referenced in all test projects
- [ ] Test discovery finds all test classes ending in `Tests`
- [ ] Test methods with `[Fact]` attribute execute successfully
- [ ] Theory tests with `[Theory]` and `[InlineData]` execute for each data row
- [ ] Parallel execution runs tests in different classes concurrently
- [ ] Sequential execution runs tests in same class in order
- [ ] `dotnet test` command discovers and executes all tests

### FluentAssertions Integration

- [ ] FluentAssertions 7.0.0 package referenced in all test projects
- [ ] `.Should()` extension methods available on all types
- [ ] Failed assertions provide detailed error messages with expected vs actual
- [ ] Collection assertions (`.Should().HaveCount()`, `.Should().Contain()`) work correctly
- [ ] Exception assertions (`.Should().Throw<T>()`) validate exception type and message
- [ ] Async assertions (`.Should().ThrowAsync<T>()`) work with async methods

### Moq Integration

- [ ] Moq 4.20.72 package referenced in test projects requiring mocking
- [ ] `new Mock<IInterface>()` creates mock objects successfully
- [ ] `.Setup()` configures mock method behaviors
- [ ] `.ReturnsAsync()` works for async method mocks
- [ ] `.Verify()` confirms mock method invocations
- [ ] `.It.IsAny<T>()` matches any parameter value

### Code Coverage Collection

- [ ] coverlet.collector 6.0.2 package referenced in all test projects
- [ ] `dotnet test --collect:"XPlat Code Coverage"` generates coverage files
- [ ] Coverage files created in `TestResults/{guid}/coverage.cobertura.xml`
- [ ] ReportGenerator tool installed globally
- [ ] HTML coverage reports generate from Cobertura XML files
- [ ] Coverage reports show line-by-line coverage highlighting
- [ ] Coverage percentages calculated correctly per project

### Test Project Structure

- [ ] All test projects located in `/dotnet/tests/` directory
- [ ] Test projects named with `.Tests` suffix (e.g., `TechHub.Core.Tests`)
- [ ] Test projects reference corresponding source projects
- [ ] Test files organized in folders matching source structure (e.g., `Models/`, `DTOs/`)
- [ ] `GlobalUsings.cs` file exists in each test project
- [ ] Global usings include Xunit, FluentAssertions, Moq, domain namespaces

### Naming Conventions

- [ ] All test classes follow `{ClassUnderTest}Tests` pattern (e.g., `ContentItemTests`)
- [ ] All test methods follow `{MethodName}_{Scenario}_{ExpectedOutcome}` pattern
- [ ] Test method names clearly describe what is being tested
- [ ] Test method names indicate expected behavior or outcome
- [ ] No test methods named `Test1`, `Test2`, or similarly non-descriptive names

### AAA Pattern Compliance

- [ ] All unit tests have `// Arrange` comment section
- [ ] All unit tests have `// Act` comment section
- [ ] All unit tests have `// Assert` comment section
- [ ] Arrange section contains all test setup and data preparation
- [ ] Act section contains single method call being tested
- [ ] Assert section contains all validations using FluentAssertions

### Theory Tests

- [ ] Parameterized tests use `[Theory]` attribute instead of `[Fact]`
- [ ] Theory tests have at least one `[InlineData]` or `[MemberData]` attribute
- [ ] InlineData values cover happy path, edge cases, and boundary conditions
- [ ] Theory test method parameters match InlineData types and count
- [ ] Complex test data uses `[MemberData]` with `IEnumerable<object[]>` method

### Exception Testing

- [ ] Exception tests use `Action act = () => methodCall;` pattern for sync methods
- [ ] Exception tests use `Func<Task> act = async () => await methodCall;` pattern for async methods
- [ ] Exception assertions use `.Should().Throw<TException>()` for sync methods
- [ ] Exception assertions use `.Should().ThrowAsync<TException>()` for async methods
- [ ] Exception tests verify exception type, message, and parameter name where applicable

### Async Testing

- [ ] Async test methods have `async Task` return type (not `async void`)
- [ ] Async test methods await all async operations
- [ ] Async exception tests use `.Should().ThrowAsync<TException>()`
- [ ] Async tests verify cancellation token handling with `OperationCanceledException`
- [ ] Async mocks use `.ReturnsAsync()` or `.ThrowsAsync()`

### Collection Testing

- [ ] Collection tests use `.Should().HaveCount()` to verify collection size
- [ ] Collection tests use `.Should().Contain()` to verify item presence
- [ ] Collection tests use `.Should().BeEquivalentTo()` to compare entire collections
- [ ] Collection tests use `.Should().OnlyHaveUniqueItems()` for uniqueness validation
- [ ] Collection tests use `.Should().AllSatisfy()` for item-level validation

### Test Fixtures

- [ ] Test classes requiring shared setup implement `IClassFixture<TFixture>`
- [ ] Fixture classes have constructor for expensive setup
- [ ] Fixture classes implement `IDisposable` for cleanup
- [ ] Fixture instances injected into test class constructors
- [ ] Test methods access fixture properties for shared state

### Test Execution

- [ ] `dotnet test` executes all tests successfully
- [ ] `dotnet test --filter "FullyQualifiedName~ClassName"` runs specific test class
- [ ] `dotnet test --filter "FullyQualifiedName~MethodName"` runs specific test method
- [ ] Test execution completes in under 60 seconds for all Core tests
- [ ] Individual test methods complete in under 1 second
- [ ] Test runner displays pass/fail count and execution time

### Code Coverage Standards

- [ ] TechHub.Core.Tests achieves 90%+ coverage for domain models
- [ ] TechHub.Core.Tests achieves 90%+ coverage for utilities
- [ ] TechHub.Core.Tests achieves 80%+ overall coverage
- [ ] TechHub.Infrastructure.Tests achieves 70%+ coverage (repositories tested in integration tests)
- [ ] Coverage reports identify uncovered lines and branches
- [ ] Coverage trends tracked over time (no decrease in coverage)

## Implementation Guide

## Testing Framework

**Test Framework**: xUnit 2.9.3  
**Assertion Library**: FluentAssertions 7.0.0  
**Mocking Library**: Moq 4.20.72  
**Coverage Tool**: coverlet.collector 6.0.2

**Why xUnit?**

- Modern, extensible, community standard for .NET
- Theory support for parameterized tests
- Parallel execution by default
- No test class instantiation overhead

**Why FluentAssertions?**

- Readable, expressive assertions
- Better error messages than xUnit.Assert
- Extensive API for collections, exceptions, async

**Why Moq?**

- Industry standard for .NET mocking
- Simple, clean syntax
- Supports all interface mocking needs

---

## Test Project Organization

**Location**: `/dotnet/tests/TechHub.Core.Tests/`

**Organization**:

```text
TechHub.Core.Tests/
├── Models/
│   ├── ContentItemTests.cs
│   ├── SectionTests.cs
│   └── DateUtilsTests.cs
├── DTOs/
│   ├── ContentItemDtoTests.cs
│   └── SectionDtoTests.cs
└── GlobalUsings.cs
```

**GlobalUsings.cs**:

```csharp
global using Xunit;
global using FluentAssertions;
global using Moq;
global using TechHub.Core.Models;
global using TechHub.Core.DTOs;
global using TechHub.Core.Interfaces;
```

---

## Test Naming Standards

**Test Class**: `{ClassUnderTest}Tests`

**Test Method**: `{MethodName}_{Scenario}_{ExpectedOutcome}`

**Examples**:

- `GetUrlInSection_WithAiSection_ReturnsCorrectUrl()`
- `ToEpoch_WithValidDate_ReturnsUnixTimestamp()`
- `Constructor_WithNullTitle_ThrowsArgumentNullException()`

---

## Test Structure (AAA Pattern)

**Arrange-Act-Assert**:

```csharp
[Fact]
public void GetUrlInSection_WithAiSection_ReturnsCorrectUrl()
{
    // Arrange
    var item = new ContentItem
    {
        Id = "vs-code-107",
        Collection = "videos",
        Title = "VS Code Update",
        Description = "Latest features",
        Author = "Microsoft",
        DateEpoch = 1735689600,
        Categories = ["ai"],
        Tags = ["vscode", "ai"],
        Content = "Full content",
        Excerpt = "Excerpt",
        CanonicalUrl = "/ai/videos/vs-code-107.html"
    };
    
    // Act
    var url = item.GetUrlInSection("ai");
    
    // Assert
    url.Should().Be("/ai/videos/vs-code-107.html");
}
```

---

## Test Categories

### 1. Model Tests

**Purpose**: Validate domain model behavior, calculated properties, business rules

**Example - ContentItem.GetUrlInSection()**:

```csharp
namespace TechHub.Core.Tests.Models;

public class ContentItemTests
{
    [Fact]
    public void GetUrlInSection_WithValidSection_ReturnsCorrectFormat()
    {
        // Arrange
        var item = CreateContentItem(id: "test", collection: "videos");
        
        // Act
        var url = item.GetUrlInSection("ai");
        
        // Assert
        url.Should().Be("/ai/videos/test.html");
    }
    
    [Theory]
    [InlineData("ai", "/ai/videos/test.html")]
    [InlineData("github-copilot", "/github-copilot/videos/test.html")]
    [InlineData("azure", "/azure/videos/test.html")]
    public void GetUrlInSection_WithDifferentSections_ReturnsCorrectUrls(
        string sectionUrl, 
        string expectedUrl)
    {
        // Arrange
        var item = CreateContentItem(id: "test", collection: "videos");
        
        // Act
        var url = item.GetUrlInSection(sectionUrl);
        
        // Assert
        url.Should().Be(expectedUrl);
    }
    
    [Theory]
    [InlineData("news")]
    [InlineData("blogs")]
    [InlineData("community")]
    public void GetUrlInSection_WithDifferentCollections_ReturnsCorrectUrls(
        string collection)
    {
        // Arrange
        var item = CreateContentItem(id: "test", collection: collection);
        
        // Act
        var url = item.GetUrlInSection("ai");
        
        // Assert
        url.Should().Be($"/ai/{collection}/test.html");
    }
    
    private static ContentItem CreateContentItem(
        string id = "test",
        string collection = "videos") => new()
    {
        Id = id,
        Collection = collection,
        Title = "Test",
        Description = "Test description",
        Author = "Test Author",
        DateEpoch = 1735689600,
        Categories = ["ai"],
        Tags = ["test"],
        Content = "Content",
        Excerpt = "Excerpt",
        CanonicalUrl = $"/ai/{collection}/{id}.html"
    };
}
```

---

### 2. Utility Tests

**Purpose**: Test pure functions and helper methods

**Example - DateUtils**:

```csharp
namespace TechHub.Core.Tests.Models;

public class DateUtilsTests
{
    private readonly TimeZoneInfo _timezone;
    
    public DateUtilsTests()
    {
        _timezone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");
    }
    
    [Fact]
    public void ToEpoch_WithUtcDate_ReturnsCorrectTimestamp()
    {
        // Arrange
        var date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        // Act
        var epoch = DateUtils.ToEpoch(date);
        
        // Assert
        epoch.Should().Be(1735689600);
    }
    
    [Fact]
    public void FromEpoch_WithTimestamp_ReturnsCorrectDate()
    {
        // Arrange
        var epoch = 1735689600L;
        
        // Act
        var date = DateUtils.FromEpoch(epoch, _timezone);
        
        // Assert
        date.Year.Should().Be(2025);
        date.Month.Should().Be(1);
        date.Day.Should().Be(1);
        date.Hour.Should().Be(1); // Brussels is UTC+1 in winter
        date.Minute.Should().Be(0);
        date.Second.Should().Be(0);
    }
    
    [Theory]
    [InlineData("2025-01-01", 1735689600)]
    [InlineData("2025-06-15", 1749081600)]
    [InlineData("2024-12-31", 1735603200)]
    public void ParseIsoDate_WithValidFormats_ReturnsCorrectEpoch(
        string isoDate, 
        long expectedEpoch)
    {
        // Arrange & Act
        var epoch = DateUtils.ParseIsoDate(isoDate, _timezone);
        
        // Assert
        epoch.Should().Be(expectedEpoch);
    }
    
    [Theory]
    [InlineData("invalid")]
    [InlineData("2025-13-01")]
    [InlineData("2025-01-32")]
    [InlineData("")]
    public void ParseIsoDate_WithInvalidFormats_ThrowsFormatException(
        string invalidDate)
    {
        // Arrange & Act
        Action act = () => DateUtils.ParseIsoDate(invalidDate, _timezone);
        
        // Assert
        act.Should().Throw<FormatException>();
    }
    
    [Fact]
    public void NormalizeToMidnight_WithDateTime_ReturnsStartOfDay()
    {
        // Arrange
        var dateTime = new DateTime(2025, 1, 15, 14, 30, 45, DateTimeKind.Utc);
        
        // Act
        var normalized = DateUtils.NormalizeToMidnight(dateTime, _timezone);
        
        // Assert
        normalized.Hour.Should().Be(0);
        normalized.Minute.Should().Be(0);
        normalized.Second.Should().Be(0);
        normalized.Day.Should().Be(15);
    }
}
```

---

### 3. DTO Tests

**Purpose**: Validate data transfer object mappings and conversions

**Example - ContentItemDto**:

```csharp
namespace TechHub.Core.Tests.DTOs;

public class ContentItemDtoTests
{
    [Fact]
    public void FromModel_WithValidContentItem_MapsAllProperties()
    {
        // Arrange
        var model = new ContentItem
        {
            Id = "test-id",
            Title = "Test Title",
            Description = "Test Description",
            Author = "Test Author",
            DateEpoch = 1735689600,
            Collection = "videos",
            Categories = ["ai", "ml"],
            Tags = ["vscode", "copilot"],
            Content = "Full content here",
            Excerpt = "Excerpt here",
            CanonicalUrl = "/ai/videos/test-id.html",
            ExternalUrl = "https://example.com",
            VideoId = "abc123"
        };
        
        // Act
        var dto = ContentItemDto.FromModel(model);
        
        // Assert
        dto.Id.Should().Be(model.Id);
        dto.Title.Should().Be(model.Title);
        dto.Description.Should().Be(model.Description);
        dto.Author.Should().Be(model.Author);
        dto.DateEpoch.Should().Be(model.DateEpoch);
        dto.Collection.Should().Be(model.Collection);
        dto.Categories.Should().BeEquivalentTo(model.Categories);
        dto.Tags.Should().BeEquivalentTo(model.Tags);
        dto.Content.Should().Be(model.Content);
        dto.Excerpt.Should().Be(model.Excerpt);
        dto.CanonicalUrl.Should().Be(model.CanonicalUrl);
        dto.ExternalUrl.Should().Be(model.ExternalUrl);
        dto.VideoId.Should().Be(model.VideoId);
    }
    
    [Fact]
    public void FromModel_WithNullOptionalFields_HandlesGracefully()
    {
        // Arrange
        var model = new ContentItem
        {
            Id = "test",
            Title = "Test",
            Description = "Test",
            Author = "Test",
            DateEpoch = 1735689600,
            Collection = "news",
            Categories = ["ai"],
            Tags = [],
            Content = "Content",
            Excerpt = "Excerpt",
            CanonicalUrl = "/ai/news/test.html",
            ExternalUrl = null,
            VideoId = null
        };
        
        // Act
        var dto = ContentItemDto.FromModel(model);
        
        // Assert
        dto.ExternalUrl.Should().BeNull();
        dto.VideoId.Should().BeNull();
    }
}
```

---

### 4. Validation Tests

**Purpose**: Test input validation and error handling

**Example - Section Validation**:

```csharp
namespace TechHub.Core.Tests.Models;

public class SectionValidationTests
{
    [Fact]
    public void Constructor_WithNullUrl_ThrowsArgumentNullException()
    {
        // Arrange
        Func<Section> act = () => new Section
        {
            Url = null!, // Required property
            Title = "Test",
            Description = "Test",
            Category = "ai",
            Collections = []
        };
        
        // Act & Assert
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void Constructor_WithEmptyCollections_CreatesValidSection()
    {
        // Arrange & Act
        var section = new Section
        {
            Url = "ai",
            Title = "AI",
            Description = "AI Section",
            Category = "ai",
            Collections = []
        };
        
        // Assert
        section.Collections.Should().BeEmpty();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithWhitespaceUrl_ThrowsArgumentException(
        string invalidUrl)
    {
        // Arrange
        Func<Section> act = () => new Section
        {
            Url = invalidUrl,
            Title = "Test",
            Description = "Test",
            Category = "ai",
            Collections = []
        };
        
        // Act & Assert
        act.Should().Throw<ArgumentException>();
    }
}
```

---

## Theory Tests for Parameterized Testing

**Use When**: Testing same logic with multiple inputs

**InlineData Example**:

```csharp
[Theory]
[InlineData("ai", "videos", "test", "/ai/videos/test.html")]
[InlineData("github-copilot", "news", "announcement", "/github-copilot/news/announcement.html")]
[InlineData("azure", "blogs", "article", "/azure/blogs/article.html")]
public void GetUrlInSection_WithVariousInputs_ReturnsCorrectUrls(
    string section,
    string collection,
    string id,
    string expectedUrl)
{
    // Arrange
    var item = CreateContentItem(id: id, collection: collection);
    
    // Act
    var url = item.GetUrlInSection(section);
    
    // Assert
    url.Should().Be(expectedUrl);
}
```

**MemberData Example** (for complex test data):

```csharp
public static IEnumerable<object[]> GetSectionTestData()
{
    yield return new object[] { "ai", "AI", "Artificial Intelligence" };
    yield return new object[] { "github-copilot", "GitHub Copilot", "AI pair programmer" };
    yield return new object[] { "ml", "ML", "Machine Learning" };
}

[Theory]
[MemberData(nameof(GetSectionTestData))]
public void Section_WithVariousData_CreatesCorrectly(
    string url,
    string title,
    string description)
{
    // Arrange & Act
    var section = new Section
    {
        Url = url,
        Title = title,
        Description = description,
        Category = url,
        Collections = []
    };
    
    // Assert
    section.Url.Should().Be(url);
    section.Title.Should().Be(title);
    section.Description.Should().Be(description);
}
```

---

## Testing Exceptions

**Testing Expected Exceptions**:

```csharp
[Fact]
public void ParseIsoDate_WithNullInput_ThrowsArgumentNullException()
{
    // Arrange
    Action act = () => DateUtils.ParseIsoDate(null!, TimeZoneInfo.Utc);
    
    // Act & Assert
    act.Should().Throw<ArgumentNullException>()
        .WithParameterName("isoDate");
}

[Fact]
public void ParseIsoDate_WithInvalidFormat_ThrowsFormatException()
{
    // Arrange
    Action act = () => DateUtils.ParseIsoDate("invalid", TimeZoneInfo.Utc);
    
    // Act & Assert
    act.Should().Throw<FormatException>()
        .WithMessage("*ISO 8601*");
}
```

---

## Testing Async Methods

**Async Method Patterns**:

```csharp
[Fact]
public async Task GetAllSectionsAsync_ReturnsAllSections()
{
    // Arrange
    var mockRepo = new Mock<ISectionRepository>();
    mockRepo.Setup(r => r.GetAllSectionsAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(new List<Section>
        {
            new() { Url = "ai", Title = "AI", /* ... */ },
            new() { Url = "github-copilot", Title = "GitHub Copilot", /* ... */ }
        });
    
    // Act
    var sections = await mockRepo.Object.GetAllSectionsAsync();
    
    // Assert
    sections.Should().HaveCount(2);
    sections.Should().Contain(s => s.Url == "ai");
}

[Fact]
public async Task GetSectionAsync_WithCancellation_ThrowsOperationCanceledException()
{
    // Arrange
    var mockRepo = new Mock<ISectionRepository>();
    var cts = new CancellationTokenSource();
    cts.Cancel();
    
    mockRepo.Setup(r => r.GetSectionByUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(new OperationCanceledException());
    
    // Act
    Func<Task> act = async () => await mockRepo.Object.GetSectionByUrlAsync("ai", cts.Token);
    
    // Assert
    await act.Should().ThrowAsync<OperationCanceledException>();
}
```

---

## Collection Testing with FluentAssertions

**Testing Collections**:

```csharp
[Fact]
public void Section_WithMultipleCollections_ReturnsAllCollections()
{
    // Arrange
    var section = new Section
    {
        Url = "ai",
        Title = "AI",
        Description = "AI Section",
        Category = "ai",
        Collections = ["news", "videos", "blogs"]
    };
    
    // Act & Assert
    section.Collections.Should().HaveCount(3);
    section.Collections.Should().Contain("news");
    section.Collections.Should().Contain("videos");
    section.Collections.Should().Contain("blogs");
    section.Collections.Should().BeInAscendingOrder();
}

[Fact]
public void ContentItem_WithCategories_ContainsExpectedCategories()
{
    // Arrange
    var item = CreateContentItem();
    
    // Act & Assert
    item.Categories.Should()
        .NotBeEmpty()
        .And.HaveCountGreaterThan(0)
        .And.OnlyHaveUniqueItems()
        .And.AllSatisfy(cat => cat.Should().NotBeNullOrWhiteSpace());
}
```

---

## Test Fixtures for Shared Setup

**Use When**: Multiple test classes need same setup

**Fixture Class**:

```csharp
namespace TechHub.Core.Tests.Fixtures;

public class TimeZoneFixture : IDisposable
{
    public TimeZoneInfo BrusselsTimeZone { get; }
    
    public TimeZoneFixture()
    {
        BrusselsTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Brussels");
    }
    
    public void Dispose()
    {
        // Cleanup if needed
    }
}
```

**Usage**:

```csharp
public class DateUtilsTests : IClassFixture<TimeZoneFixture>
{
    private readonly TimeZoneInfo _timezone;
    
    public DateUtilsTests(TimeZoneFixture fixture)
    {
        _timezone = fixture.BrusselsTimeZone;
    }
    
    [Fact]
    public void FromEpoch_UsesConfiguredTimezone()
    {
        // Timezone already available from fixture
        var date = DateUtils.FromEpoch(1735689600, _timezone);
        // ...
    }
}
```

---

## Code Coverage Requirements

**Target**: 80% minimum for Core and Infrastructure layers

**Run Coverage**:

```powershell
dotnet test --collect:"XPlat Code Coverage"
```

**View Report**:

```powershell
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

**Coverage Goals**:

- **Domain Models**: 100% (critical business logic)
- **Utilities**: 100% (pure functions, high reuse)
- **DTOs**: 90% (mostly mapping, some edge cases)
- **Repositories**: 70% (covered by integration tests)
- **Services**: 70% (covered by integration tests)

---

## Test Organization Best Practices

### ✅ DO

- **One test class per production class**
- **Use descriptive test method names**
- **Follow AAA pattern consistently**
- **Test one scenario per test method**
- **Use Theory for parameterized tests**
- **Use FluentAssertions for readability**
- **Test edge cases and null inputs**
- **Use helper methods to reduce duplication**

### ❌ DON'T

- **Don't test framework code** (e.g., ASP.NET Core internals)
- **Don't test external libraries** (e.g., Markdig)
- **Don't duplicate logic in tests** (test REAL implementation)
- **Don't add wrapper methods just for testing**
- **Don't make code backwards compatible unless requested**
- **Don't use `Assert.True(condition)` when specific assertions exist**
- **Don't ignore warnings or failing tests**

---

## Running Tests

**All Tests**:

```powershell
dotnet test
```

**Specific Project**:

```powershell
dotnet test tests/TechHub.Core.Tests
```

**Specific Test Class**:

```powershell
dotnet test --filter "FullyQualifiedName~ContentItemTests"
```

**Specific Test Method**:

```powershell
dotnet test --filter "FullyQualifiedName~GetUrlInSection_WithAiSection_ReturnsCorrectUrl"
```

**With Detailed Output**:

```powershell
dotnet test --logger "console;verbosity=detailed"
```

**Parallel Execution** (xUnit default):

- Tests in different classes run in parallel
- Tests in same class run sequentially
- Configure in `xunit.runner.json` if needed

---

## References

- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Moq Documentation](https://github.com/moq/moq4)
- [.NET Testing Best Practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- `/specs/infrastructure/solution-structure.md` - Test project setup
- `/specs/features/domain-models.md` - Models to test
- `/specs/features/repository-pattern.md` - Repositories to test
