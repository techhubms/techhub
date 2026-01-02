# Unit Testing Strategy

## Overview

Defines the unit testing approach for Tech Hub .NET migration using xUnit, FluentAssertions, and Moq. This specification ensures comprehensive test coverage for domain models, utilities, and business logic.

## Constitution Alignment

- **Test-Driven Development**: Write tests BEFORE or DURING implementation
- **Clean Architecture**: Test each layer independently
- **Configuration-Driven**: Test configuration validation and loading

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

## Test Project Structure

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

## Naming Conventions

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

## Exception Testing

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

## Async Testing

**Testing Async Methods**:

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
