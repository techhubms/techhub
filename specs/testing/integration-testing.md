# Integration Testing Strategy

## Overview

Defines integration testing approach for Tech Hub .NET migration using WebApplicationFactory for API testing and RealTimeProvider for realistic scenarios.

## Constitution Alignment

- **Clean Architecture**: Test API layer with real infrastructure
- **Configuration-Driven**: Test configuration loading and validation
- **.NET 10 Stack**: Leverage ASP.NET Core test host

## Testing Framework

**Test Framework**: xUnit 2.9.3  
**Test Host**: WebApplicationFactory (ASP.NET Core)  
**Assertion Library**: FluentAssertions 7.0.0  
**Mocking Library**: Moq 4.20.72 (for external dependencies only)  
**Coverage Tool**: coverlet.collector 6.0.2

---

## Test Project Structure

**Location**: `/dotnet/tests/TechHub.Api.Tests/`

**Organization**:

```text
TechHub.Api.Tests/
├── Endpoints/
│   ├── SectionEndpointsTests.cs
│   ├── ContentEndpointsTests.cs
│   └── RssEndpointsTests.cs
├── Fixtures/
│   └── ApiWebApplicationFactory.cs
├── TestData/
│   ├── sections.test.json
│   └── sample-content/
└── GlobalUsings.cs
```

**GlobalUsings.cs**:

```csharp
global using Xunit;
global using FluentAssertions;
global using Moq;
global using Microsoft.AspNetCore.Mvc.Testing;
global using System.Net.Http.Json;
global using TechHub.Core.DTOs;
global using TechHub.Api;
```

---

## WebApplicationFactory Setup

**Purpose**: Create in-memory test server with real API pipeline

**ApiWebApplicationFactory.cs**:

```csharp
namespace TechHub.Api.Tests.Fixtures;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Override ContentOptions to use test data
            services.Configure<ContentOptions>(options =>
            {
                options.SectionsJsonPath = "TestData/sections.test.json";
                options.CollectionsRootPath = "TestData/sample-content";
                options.Timezone = "Europe/Brussels";
                options.EnableCaching = false; // Disable caching for tests
            });
            
            // Use real repositories with test data
            // No mocking needed - test actual file reading
        });
        
        builder.UseEnvironment("Test");
    }
}
```

**Usage**:

```csharp
public class SectionEndpointsTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    public SectionEndpointsTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    // Tests...
}
```

---

## Endpoint Testing Patterns

### GET Endpoints

**Test Structure**:

```csharp
namespace TechHub.Api.Tests.Endpoints;

public class SectionEndpointsTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    public SectionEndpointsTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAllSections_Returns200_WithSections()
    {
        // Act
        var response = await _client.GetAsync("/api/sections");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var sections = await response.Content
            .ReadFromJsonAsync<List<SectionDto>>();
        
        sections.Should().NotBeNull();
        sections.Should().NotBeEmpty();
        sections.Should().Contain(s => s.Url == "ai");
    }
    
    [Theory]
    [InlineData("ai")]
    [InlineData("github-copilot")]
    [InlineData("azure")]
    public async Task GetSectionByUrl_WithValidUrl_Returns200(string url)
    {
        // Act
        var response = await _client.GetAsync($"/api/sections/{url}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var section = await response.Content
            .ReadFromJsonAsync<SectionDto>();
        
        section.Should().NotBeNull();
        section!.Url.Should().Be(url);
        section.Title.Should().NotBeNullOrEmpty();
        section.Collections.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task GetSectionByUrl_WithInvalidUrl_Returns404()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/nonexistent");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetAllSections_ReturnsCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/sections");
        
        // Assert
        response.Content.Headers.ContentType?.MediaType
            .Should().Be("application/json");
    }
    
    [Fact]
    public async Task GetAllSections_IncludesCorsHeaders()
    {
        // Arrange
        _client.DefaultRequestHeaders.Add("Origin", "https://localhost:5173");
        
        // Act
        var response = await _client.GetAsync("/api/sections");
        
        // Assert
        response.Headers.Should().ContainKey("Access-Control-Allow-Origin");
    }
}
```

---

### Content Endpoints

**Test Multi-Location Content Access**:

```csharp
namespace TechHub.Api.Tests.Endpoints;

public class ContentEndpointsTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    public ContentEndpointsTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetContent_ForSection_ReturnsAllContentInCategory()
    {
        // Act
        var response = await _client.GetAsync("/api/content/ai");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var items = await response.Content
            .ReadFromJsonAsync<List<ContentItemDto>>();
        
        items.Should().NotBeNull();
        items.Should().AllSatisfy(item => 
            item.Categories.Should().Contain("ai"));
    }
    
    [Fact]
    public async Task GetContent_ForSectionAndCollection_FiltersCorrectly()
    {
        // Act
        var response = await _client.GetAsync("/api/content/ai/videos");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var items = await response.Content
            .ReadFromJsonAsync<List<ContentItemDto>>();
        
        items.Should().NotBeNull();
        items.Should().AllSatisfy(item =>
        {
            item.Collection.Should().Be("videos");
            item.Categories.Should().Contain("ai");
        });
    }
    
    [Fact]
    public async Task GetContentItem_WithMultipleCategories_AccessibleFromBoth()
    {
        // Arrange - content with categories: ["ai", "ml"]
        const string itemId = "multi-category-article";
        
        // Act - Access from AI section
        var aiResponse = await _client.GetAsync($"/api/content/ai/blogs/{itemId}");
        var aiItem = await aiResponse.Content.ReadFromJsonAsync<ContentItemDto>();
        
        // Act - Access from ML section
        var mlResponse = await _client.GetAsync($"/api/content/ml/blogs/{itemId}");
        var mlItem = await mlResponse.Content.ReadFromJsonAsync<ContentItemDto>();
        
        // Assert - Same content accessible from both sections
        aiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        mlResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        aiItem.Should().NotBeNull();
        mlItem.Should().NotBeNull();
        aiItem!.Id.Should().Be(mlItem!.Id);
        aiItem.Title.Should().Be(mlItem.Title);
    }
    
    [Fact]
    public async Task GetContentItem_WithWrongCategory_Returns404()
    {
        // Arrange - content only in "ai" category
        const string itemId = "ai-only-article";
        
        // Act - Try to access from wrong category
        var response = await _client.GetAsync($"/api/content/azure/blogs/{itemId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
```

---

### RSS Endpoints

**Test Output Caching**:

```csharp
namespace TechHub.Api.Tests.Endpoints;

public class RssEndpointsTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    public RssEndpointsTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetRssFeed_ReturnsXmlContentType()
    {
        // Act
        var response = await _client.GetAsync("/feed.xml");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType
            .Should().Be("application/xml");
    }
    
    [Fact]
    public async Task GetRssFeed_IncludesCacheHeaders()
    {
        // Act
        var response = await _client.GetAsync("/feed.xml");
        
        // Assert
        response.Headers.CacheControl.Should().NotBeNull();
        response.Headers.CacheControl!.MaxAge
            .Should().Be(TimeSpan.FromMinutes(15));
    }
    
    [Theory]
    [InlineData("/feed.xml")]
    [InlineData("/ai/feed.xml")]
    [InlineData("/github-copilot/feed.xml")]
    public async Task GetRssFeed_ForVariousSections_Returns200(string feedUrl)
    {
        // Act
        var response = await _client.GetAsync(feedUrl);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("<rss version=\"2.0\"");
        content.Should().Contain("<channel>");
    }
    
    [Fact]
    public async Task GetRssFeed_ContainsValidRssStructure()
    {
        // Act
        var response = await _client.GetAsync("/feed.xml");
        var xml = await response.Content.ReadAsStringAsync();
        
        // Assert
        xml.Should().Contain("<rss");
        xml.Should().Contain("<channel>");
        xml.Should().Contain("<title>");
        xml.Should().Contain("<link>");
        xml.Should().Contain("<description>");
        xml.Should().Contain("<item>");
    }
}
```

---

## Testing with Test Data

**TestData/sections.test.json**:

```json
{
  "sections": [
    {
      "url": "ai",
      "title": "AI",
      "description": "Artificial Intelligence",
      "category": "ai",
      "collections": ["news", "videos", "blogs"]
    },
    {
      "url": "github-copilot",
      "title": "GitHub Copilot",
      "description": "AI pair programmer",
      "category": "github-copilot",
      "collections": ["news", "videos"]
    }
  ]
}
```

**TestData/sample-content/_videos/test-video.md**:

```markdown
---
title: Test Video
description: Sample video for testing
author: Test Author
date: 2025-01-01
categories:
  - ai
tags:
  - vscode
  - testing
video_id: abc123
---

This is test video content.

<!--excerpt_end-->

Full content here.
```

---

## Configuration Testing

**Test Configuration Loading**:

```csharp
namespace TechHub.Api.Tests;

public class ConfigurationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;
    
    public ConfigurationTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public void Configuration_LoadsContentOptions()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var options = scope.ServiceProvider
            .GetRequiredService<IOptions<ContentOptions>>();
        
        // Assert
        options.Value.Should().NotBeNull();
        options.Value.SectionsJsonPath.Should().NotBeNullOrEmpty();
        options.Value.Timezone.Should().Be("Europe/Brussels");
    }
    
    [Fact]
    public void Configuration_RegistersAllRepositories()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        
        // Act & Assert
        var sectionRepo = scope.ServiceProvider
            .GetService<ISectionRepository>();
        sectionRepo.Should().NotBeNull();
        
        var contentRepo = scope.ServiceProvider
            .GetService<IContentRepository>();
        contentRepo.Should().NotBeNull();
    }
}
```

---

## Middleware Testing

**Test Middleware Pipeline**:

```csharp
namespace TechHub.Api.Tests;

public class MiddlewareTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    public MiddlewareTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Api_RedirectsHttpToHttps()
    {
        // This would require additional setup to test HTTP → HTTPS redirect
        // Typically tested in E2E or staging environment
    }
    
    [Fact]
    public async Task Api_EnablesResponseCompression()
    {
        // Arrange
        _client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
        
        // Act
        var response = await _client.GetAsync("/api/sections");
        
        // Assert
        response.Content.Headers.ContentEncoding
            .Should().Contain("gzip");
    }
    
    [Fact]
    public async Task Api_EnablesOutputCaching()
    {
        // Act - First request
        var response1 = await _client.GetAsync("/api/sections");
        
        // Act - Second request
        var response2 = await _client.GetAsync("/api/sections");
        
        // Assert - Should have cache headers
        response1.Headers.Should().ContainKey("Age")
            .Or.ContainKey("Cache-Control");
    }
}
```

---

## Error Handling Tests

**Test 404 and Error Responses**:

```csharp
namespace TechHub.Api.Tests.Endpoints;

public class ErrorHandlingTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    public ErrorHandlingTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetSection_WithNonexistentUrl_Returns404()
    {
        // Act
        var response = await _client.GetAsync("/api/sections/nonexistent");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetContent_WithInvalidSectionAndCollection_Returns404()
    {
        // Act
        var response = await _client.GetAsync("/api/content/invalid/invalid");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetContentItem_WithNonexistentItem_Returns404()
    {
        // Act
        var response = await _client.GetAsync("/api/content/ai/videos/nonexistent");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("/api/sections/../../etc/passwd")] // Path traversal attempt
    [InlineData("/api/sections/<script>alert(1)</script>")] // XSS attempt
    public async Task GetSection_WithMaliciousInput_Returns400Or404(string maliciousUrl)
    {
        // Act
        var response = await _client.GetAsync(maliciousUrl);
        
        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.NotFound);
    }
}
```

---

## Performance Testing

**Test Response Times**:

```csharp
namespace TechHub.Api.Tests;

public class PerformanceTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    public PerformanceTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAllSections_CompletesWithinAcceptableTime()
    {
        // Arrange
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        // Act
        var response = await _client.GetAsync("/api/sections");
        stopwatch.Stop();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(100); // 100ms max
    }
    
    [Fact]
    public async Task GetContent_WithCaching_IsFaster()
    {
        // Arrange - First request (cache miss)
        var stopwatch1 = System.Diagnostics.Stopwatch.StartNew();
        await _client.GetAsync("/api/content/ai");
        stopwatch1.Stop();
        
        // Act - Second request (cache hit)
        var stopwatch2 = System.Diagnostics.Stopwatch.StartNew();
        var response = await _client.GetAsync("/api/content/ai");
        stopwatch2.Stop();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch2.Elapsed.Should().BeLessThan(stopwatch1.Elapsed);
    }
}
```

---

## OpenAPI/Swagger Testing

**Test API Documentation**:

```csharp
namespace TechHub.Api.Tests;

public class OpenApiTests
{
    [Fact]
    public async Task Swagger_IsAvailableInDevelopment()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Development");
            });
        
        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/openapi/v1.json");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var json = await response.Content.ReadAsStringAsync();
        json.Should().Contain("openapi");
        json.Should().Contain("paths");
    }
}
```

---

## Running Integration Tests

**All Integration Tests**:

```powershell
dotnet test tests/TechHub.Api.Tests
```

**Specific Test Class**:

```powershell
dotnet test --filter "FullyQualifiedName~SectionEndpointsTests"
```

**With Coverage**:

```powershell
dotnet test tests/TechHub.Api.Tests --collect:"XPlat Code Coverage"
```

---

## Best Practices

### ✅ DO

- **Use WebApplicationFactory** for real API testing
- **Test actual file reading** with test data
- **Test multi-location content access** thoroughly
- **Verify HTTP status codes** and content types
- **Test caching headers** and middleware
- **Test error scenarios** (404, 400, 500)
- **Use realistic test data** that mirrors production

### ❌ DON'T

- **Don't mock repositories** in integration tests (use real implementations with test data)
- **Don't test framework code** (ASP.NET Core internals)
- **Don't skip error handling tests**
- **Don't ignore performance benchmarks**
- **Don't test external dependencies** (mock those)

---

## References

- [Integration Tests in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
- [WebApplicationFactory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.testing.webapplicationfactory-1)
- `/specs/testing/unit-testing.md` - Unit testing strategy
- `/specs/features/api-endpoints.md` - Endpoints to test
- `/specs/features/repository-pattern.md` - Repository integration

