# Integration Testing Strategy

## Overview

Defines integration testing approach for Tech Hub .NET migration using WebApplicationFactory for API testing and RealTimeProvider for realistic scenarios.

## Constitution Alignment

- **Clean Architecture**: Test API layer with real infrastructure
- **Configuration-Driven**: Test configuration loading and validation
- **.NET 10 Stack**: Leverage ASP.NET Core test host

## Functional Requirements

### FR-001: WebApplicationFactory Integration

The solution must use ASP.NET Core's WebApplicationFactory to create in-memory test server instances for integration testing all API endpoints without external HTTP dependencies.

**Rationale**: WebApplicationFactory provides realistic API testing with actual HTTP pipeline, middleware, routing, and dependency injection while remaining fast and isolated for automated testing.

### FR-002: Test Data Isolation

Integration tests must use dedicated test data files (sections.test.json, sample-content/) that are separate from production data, ensuring tests are repeatable and don't modify production content.

**Rationale**: Isolated test data prevents test pollution, ensures deterministic test outcomes, and allows testing edge cases without affecting production data.

### FR-003: Real Repository Testing

Integration tests must use real repository implementations (SectionRepository, ContentRepository) with test data files instead of mocks, validating actual file reading and parsing logic.

**Rationale**: Integration tests verify real system behavior including file I/O, YAML frontmatter parsing, and configuration loading—mocking defeats the purpose of integration testing.

### FR-004: HTTP Status Code Validation

All endpoint integration tests must verify correct HTTP status codes (200 OK, 404 Not Found, 400 Bad Request) for both successful and error scenarios.

**Rationale**: Status codes are the primary API contract for client applications and must be tested to ensure proper error handling and response semantics.

### FR-005: Response Content Validation

Integration tests must deserialize and validate JSON response content using DTOs (SectionDto, ContentItemDto), verifying data structure, required fields, and business logic correctness.

**Rationale**: Testing response shape and content ensures API contract compliance, prevents breaking changes, and validates that business logic produces expected results.

### FR-006: Multi-Location Content Access Testing

Integration tests must verify that content items with multiple categories are accessible from all corresponding section endpoints (e.g., item with ["ai", "ml"] accessible via /api/content/ai and /api/content/ml).

**Rationale**: Multi-location content access is a critical business requirement that must be validated end-to-end through the API layer.

### FR-007: CORS Header Validation

Integration tests must verify that CORS headers (Access-Control-Allow-Origin) are present in API responses when Origin header is provided in requests.

**Rationale**: CORS configuration is essential for web client functionality and must be tested to prevent production CORS failures.

### FR-008: Response Caching Testing

Integration tests must validate cache headers (Cache-Control, Age) on cacheable endpoints (RSS feeds, static content) to ensure proper output caching behavior.

**Rationale**: Caching is critical for performance and must be tested to ensure headers are correctly configured for CDN and browser caching.

### FR-009: Configuration Loading Validation

Integration tests must verify that dependency injection correctly loads and provides configuration options (ContentOptions) with expected values from test configuration.

**Rationale**: Configuration errors are common deployment failures and must be caught through integration testing before production.

### FR-010: Middleware Pipeline Testing

Integration tests must validate middleware functionality including response compression (gzip), output caching, and error handling through actual HTTP requests.

**Rationale**: Middleware configuration affects all endpoints and must be tested as integrated system to catch configuration errors.

### FR-011: Error Handling Coverage

Integration tests must validate API error handling for invalid inputs (nonexistent resources, malicious inputs like path traversal, XSS attempts) returning appropriate 404 or 400 status codes.

**Rationale**: Security and error handling must be tested at integration level to ensure malicious inputs are handled safely before reaching business logic.

### FR-012: RSS Feed Structure Validation

Integration tests must verify RSS feed endpoints return valid XML structure with required RSS 2.0 elements (rss, channel, title, link, description, item) and correct content-type headers.

**Rationale**: RSS feeds must comply with RSS 2.0 specification for feed reader compatibility and must be validated end-to-end including XML serialization.

### FR-013: API Performance Benchmarks

Integration tests must measure and assert endpoint response times are within acceptable thresholds (e.g., <100ms for section list, cache hits faster than cache misses).

**Rationale**: Performance regression detection through automated benchmarks prevents production performance degradation.

### FR-014: OpenAPI Documentation Testing

Integration tests must validate that OpenAPI/Swagger documentation is available in development environment at /openapi/v1.json with valid OpenAPI schema structure.

**Rationale**: API documentation accuracy must be verified to ensure client developers have correct API contract information.

## Success Criteria

### API Contract Compliance

- All endpoints return documented HTTP status codes (200, 404, 400)
- Response JSON structure matches DTO contracts
- Required fields present in all responses
- Content-Type headers correct (application/json, application/xml)
- Multi-location content accessible from all valid section URLs

### System Integration

- Real repositories load and parse test data files successfully
- Configuration dependency injection works correctly
- Middleware pipeline executes in correct order
- CORS headers present when Origin header provided
- Output caching headers present on cacheable endpoints

### Test Quality

- All integration tests execute in under 5 seconds total
- Individual endpoint tests complete in under 500ms
- Tests are deterministic (no flaky tests)
- Test data isolated from production data
- Zero false positives or negatives in test results

### Error Handling

- 404 returned for nonexistent resources
- 400 or 404 returned for malicious inputs (path traversal, XSS)
- Error responses include appropriate problem details
- Validation errors return correct status codes
- Exception handling prevents 500 errors for expected failures

## User Scenarios

### Scenario 1: Backend Developer Tests New API Endpoint

**Actor**: Backend developer implementing new /api/tags endpoint

**Steps**:

1. Developer creates `TagEndpointsTests.cs` in `tests/TechHub.Api.Tests/Endpoints/`
2. Developer adds `IClassFixture<ApiWebApplicationFactory>` to test class
3. Developer injects `HttpClient` via factory in constructor
4. Developer writes test: `GetAllTags_Returns200_WithTags()`
5. Developer uses `_client.GetAsync("/api/tags")` to call endpoint
6. Developer deserializes response: `ReadFromJsonAsync<List<TagDto>>()`
7. Developer asserts status code: `response.StatusCode.Should().Be(HttpStatusCode.OK)`
8. Developer validates content: `tags.Should().NotBeEmpty()`
9. Developer runs `dotnet test --filter "TagEndpointsTests"`
10. Test executes against in-memory API server, passes in under 200ms

**Outcome**: Developer successfully creates and runs integration test for new endpoint using WebApplicationFactory, validates API contract, and confirms functionality without deploying to staging environment.

### Scenario 2: Developer Debugs Multi-Location Content Access Bug

**Actor**: Backend developer investigating reported bug where content not appearing in ML section

**Steps**:

1. Developer writes test: `GetContentItem_WithMultipleCategories_AccessibleFromBoth()`
2. Test creates content with `categories: ["ai", "ml"]`
3. Test calls `/api/content/ai/blogs/test-article` - expects 200 OK
4. Test calls `/api/content/ml/blogs/test-article` - expects 200 OK
5. Test runs and fails - ML endpoint returns 404
6. Developer sets breakpoint in test, runs debug mode
7. Developer steps through ContentRepository.GetContentForSection()
8. Developer discovers category filter logic bug
9. Developer fixes repository implementation
10. Developer re-runs test - now passes for both AI and ML sections

**Outcome**: Developer uses integration test to reproduce multi-location access bug, debugs through actual repository code, fixes implementation, and validates fix with passing test.

### Scenario 3: QA Engineer Validates API Performance

**Actor**: QA engineer ensuring API meets performance requirements before release

**Steps**:

1. QA engineer reviews `PerformanceTests.cs` in test suite
2. QA engineer runs `dotnet test --filter "PerformanceTests"`
3. Test measures section list endpoint: must complete in <100ms
4. Test measures first content request (cache miss) vs second request (cache hit)
5. All performance tests pass with margins (75ms average for section list)
6. QA engineer generates coverage report to confirm all endpoints tested
7. QA engineer reviews test results in CI/CD pipeline logs
8. QA engineer approves performance benchmarks for release

**Outcome**: QA engineer uses integration test performance benchmarks to validate API meets response time requirements, confirms caching improves performance, and approves deployment.

### Scenario 4: Security Tester Validates Input Sanitization

**Actor**: Security tester verifying API handles malicious inputs safely

**Steps**:

1. Security tester reviews `ErrorHandlingTests.cs` for security test coverage
2. Tester runs `dotnet test --filter "ErrorHandlingTests"`
3. Tests validate path traversal attempts: `/api/sections/../../etc/passwd` returns 400/404
4. Tests validate XSS attempts: `/api/sections/<script>alert(1)</script>` returns 400/404
5. All security tests pass - malicious inputs handled safely
6. Tester adds new test for SQL injection attempt in query parameters
7. New test fails - reveals vulnerability in search filtering
8. Developer fixes vulnerability, test now passes
9. Tester re-runs full security test suite - all pass

**Outcome**: Security tester uses integration tests to validate malicious input handling, discovers vulnerability through test-driven approach, and confirms fix prevents security issue.

## Acceptance Criteria

### WebApplicationFactory Setup

- [ ] WebApplicationFactory creates in-memory test server successfully
- [ ] Test server uses Test environment configuration
- [ ] ConfigureWebHost overrides ContentOptions with test data paths
- [ ] Test data paths point to TestData/ directory
- [ ] Caching disabled in test configuration (EnableCaching = false)
- [ ] Test server creates HttpClient instances via CreateClient()
- [ ] Real repositories registered in DI container (no mocks)

### Test Data Structure

- [ ] TestData/sections.test.json exists with valid section definitions
- [ ] TestData/sample-content/ directory contains test markdown files
- [ ] Test markdown files have valid YAML frontmatter
- [ ] Test content includes multi-category items for testing
- [ ] Test data separate from production data
- [ ] Tests don't modify or depend on production files

### Endpoint Testing - GET Requests

- [ ] GET /api/sections returns 200 OK with SectionDto list
- [ ] GET /api/sections/{url} returns 200 OK for valid sections
- [ ] GET /api/sections/{url} returns 404 for nonexistent sections
- [ ] GET /api/content/{section} returns 200 OK with ContentItemDto list
- [ ] GET /api/content/{section}/{collection} filters by collection correctly
- [ ] GET /api/content/{section}/{collection}/{id} returns single content item
- [ ] Response Content-Type headers set to application/json

### Multi-Location Content Access

- [ ] Content with multiple categories accessible from all section endpoints
- [ ] Same content returned from different section URLs (verified by Id match)
- [ ] Content with single category returns 404 from wrong section
- [ ] Multi-category filtering works correctly in repository layer
- [ ] GetContentForSection() includes items matching section category

### CORS and Security Headers

- [ ] CORS headers present when Origin header in request
- [ ] Access-Control-Allow-Origin header matches allowed origins
- [ ] Security headers prevent XSS (Content-Security-Policy)
- [ ] Response compression enabled (gzip) when Accept-Encoding header present

### RSS Feed Testing

- [ ] GET /feed.xml returns 200 OK with application/xml content-type
- [ ] RSS XML structure valid (rss, channel, title, link, description, item elements)
- [ ] Section-specific feeds return correct filtered content
- [ ] Cache-Control headers set on RSS feeds (15 minute max-age)
- [ ] RSS feeds validate against RSS 2.0 specification

### Configuration and DI

- [ ] ContentOptions loaded from configuration successfully
- [ ] SectionsJsonPath points to test data file
- [ ] Timezone set to Europe/Brussels in test configuration
- [ ] ISectionRepository resolved from DI container
- [ ] IContentRepository resolved from DI container
- [ ] All required services registered in DI

### Middleware Validation

- [ ] Response compression middleware applies gzip encoding
- [ ] Output caching middleware adds cache headers
- [ ] Cache-Control headers present on cacheable endpoints
- [ ] Age header indicates cache hits on repeated requests
- [ ] HTTPS redirect middleware configured (testable in E2E)

### Error Response Validation

- [ ] Nonexistent resources return 404 Not Found
- [ ] Invalid section URLs return 404 Not Found
- [ ] Path traversal attempts return 400 or 404
- [ ] XSS attempts in URL parameters return 400 or 404
- [ ] Malformed requests return appropriate error status codes
- [ ] Error responses don't leak sensitive information

### Performance Benchmarks

- [ ] GET /api/sections completes in under 100ms
- [ ] GET /api/content/{section} completes in under 200ms
- [ ] Cache hits faster than cache misses (measurable improvement)
- [ ] Individual tests complete in under 500ms
- [ ] Full integration test suite completes in under 5 seconds

### OpenAPI Documentation

- [ ] GET /openapi/v1.json returns 200 OK in Development environment
- [ ] OpenAPI JSON contains "openapi" version field
- [ ] OpenAPI JSON contains "paths" with endpoint definitions
- [ ] Swagger UI accessible at /swagger in Development
- [ ] API documentation matches actual endpoint contracts

### Test Quality Standards

- [ ] All tests follow AAA pattern (Arrange-Act-Assert)
- [ ] Test names describe scenario and expected outcome
- [ ] Theory tests used for parameterized scenarios
- [ ] GlobalUsings.cs reduces repetitive imports
- [ ] Test classes use IClassFixture&lt;ApiWebApplicationFactory&gt;
- [ ] Tests are deterministic (no random failures)
- [ ] Tests clean up resources (HttpClient disposal)

## Implementation Guide

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

## WebApplicationFactory Configuration

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

## Testing Error Handling

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

