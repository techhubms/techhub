# Integration Testing Strategy

## Overview

Comprehensive integration testing strategy using WebApplicationFactory to test API endpoints, middleware pipeline, CORS configuration, and security headers with real ASP.NET Core infrastructure but mocked dependencies.

## Functional Requirements

### FR-001: API Endpoint Testing

All API endpoints must have integration tests that verify:

- HTTP status codes (200, 404, 400, 500)
- Response content types
- Response body structure (valid JSON, DTOs)
- Request parameter validation
- Error handling and error responses

### FR-002: CORS Configuration Testing

**Priority**: P2

API integration tests must verify CORS headers are properly configured for cross-origin requests from the frontend domain.

**Required Tests**:

```csharp
[Fact]
public async Task GetSections_FromBrowserOrigin_IncludesCorsHeaders()
{
    // Arrange
    var request = new HttpRequestMessage(HttpMethod.Get, "/api/sections");
    request.Headers.Add("Origin", "https://tech.hub.ms");
    
    // Act
    var response = await _client.SendAsync(request);
    
    // Assert
    response.Should().BeSuccessful();
    response.Headers.Should().ContainKey("Access-Control-Allow-Origin");
    response.Headers.GetValues("Access-Control-Allow-Origin")
        .Should().Contain("https://tech.hub.ms");
}

[Fact]
public async Task Api_PreflightRequest_ReturnsAllowedMethods()
{
    // Arrange
    var request = new HttpRequestMessage(HttpMethod.Options, "/api/sections");
    request.Headers.Add("Origin", "https://tech.hub.ms");
    request.Headers.Add("Access-Control-Request-Method", "GET");
    
    // Act
    var response = await _client.SendAsync(request);
    
    // Assert
    response.Should().HaveStatusCode(System.Net.HttpStatusCode.NoContent);
    response.Headers.Should().ContainKey("Access-Control-Allow-Methods");
    response.Headers.GetValues("Access-Control-Allow-Methods")
        .Should().Contain("GET");
}
```

**Acceptance Criteria**:

1. ✅ Tests verify `Access-Control-Allow-Origin` header present
2. ✅ Tests verify allowed origins match frontend domain
3. ✅ Tests verify preflight OPTIONS requests work
4. ✅ Tests verify `Access-Control-Allow-Methods` header
5. ✅ Tests verify `Access-Control-Allow-Headers` if custom headers used

### FR-003: Middleware Pipeline Testing

**Priority**: P2

Integration tests must verify security headers and middleware configuration.

**Required Tests**:

```csharp
[Fact]
public async Task Api_Request_IncludesSecurityHeaders()
{
    // Arrange
    // Act
    var response = await _client.GetAsync("/api/sections");
    
    // Assert
    response.Headers.Should().ContainKey("X-Content-Type-Options");
    response.Headers.GetValues("X-Content-Type-Options")
        .Should().Contain("nosniff");
}

[Fact]
public async Task Api_Request_IncludesFrameOptions()
{
    // Arrange
    // Act
    var response = await _client.GetAsync("/api/sections");
    
    // Assert
    response.Headers.Should().ContainKey("X-Frame-Options");
    response.Headers.GetValues("X-Frame-Options")
        .Should().Contain("DENY");
}

[Fact]
public async Task Api_Request_IncludesStrictTransportSecurity()
{
    // Arrange
    // Act
    var response = await _client.GetAsync("/api/sections");
    
    // Assert
    response.Headers.Should().ContainKey("Strict-Transport-Security");
}
```

**Acceptance Criteria**:

1. ✅ Tests verify `X-Content-Type-Options: nosniff`
2. ✅ Tests verify `X-Frame-Options: DENY`
3. ✅ Tests verify `Strict-Transport-Security` (HSTS)
4. ✅ Tests verify cache headers where applicable
5. ✅ Tests document expected middleware behavior

### FR-004: Response Caching Testing

**Priority**: P3

Verify cache headers are set correctly for cacheable endpoints.

**Required Tests**:

```csharp
[Fact]
public async Task GetSections_CachedEndpoint_IncludesCacheHeaders()
{
    // Arrange
    // Act
    var response = await _client.GetAsync("/api/sections");
    
    // Assert
    response.Should().BeSuccessful();
    response.Headers.CacheControl.Should().NotBeNull();
    response.Headers.CacheControl!.MaxAge.Should().BeGreaterThan(TimeSpan.Zero);
}

[Fact]
public async Task GetSections_MultipleRequests_ReturnsSameETag()
{
    // Arrange
    var response1 = await _client.GetAsync("/api/sections");
    var etag1 = response1.Headers.ETag;
    
    // Act
    var response2 = await _client.GetAsync("/api/sections");
    var etag2 = response2.Headers.ETag;
    
    // Assert
    etag1.Should().NotBeNull();
    etag2.Should().NotBeNull();
    etag1.Should().Be(etag2);
}
```

**Acceptance Criteria**:

1. ✅ Tests verify cache headers on cacheable endpoints
2. ✅ Tests verify ETag generation and consistency
3. ✅ Tests verify `Cache-Control` directives
4. ✅ Tests verify no cache headers on non-cacheable endpoints

### FR-005: WebApplicationFactory Configuration

**Priority**: P1 (Infrastructure)

Test infrastructure must use WebApplicationFactory with proper dependency injection overrides for mocked services.

**Factory Pattern**:

```csharp
public class TechHubApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove real repositories
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IContentRepository));
            if (descriptor != null)
                services.Remove(descriptor);
            
            // Add mock repositories
            services.AddScoped<IContentRepository>(sp => 
            {
                var mock = new Mock<IContentRepository>();
                // Configure mock behavior
                return mock.Object;
            });
            
            // Same for ISectionRepository, IRssService, etc.
        });
        
        builder.UseEnvironment("Test");
    }
}
```

**Acceptance Criteria**:

1. ✅ Factory properly overrides dependencies
2. ✅ Tests use mocked repositories/services
3. ✅ Tests do NOT access real file system
4. ✅ Factory uses Test environment
5. ✅ Factory configuration is reusable across test classes

## Technical Requirements

### Test Organization

```text
tests/TechHub.Api.Tests/
├── TechHubApiFactory.cs           # WebApplicationFactory setup
├── Endpoints/
│   ├── SectionsEndpointsTests.cs  # Section endpoints
│   ├── ContentEndpointsTests.cs   # Content endpoints
│   ├── RssEndpointsTests.cs       # RSS endpoints
│   └── AdvancedEndpointsTests.cs  # Filtering/search
├── Middleware/
│   ├── CorsTests.cs               # CORS configuration
│   ├── SecurityHeadersTests.cs    # Security middleware
│   └── CachingTests.cs            # Response caching
└── Infrastructure/
    └── FactoryTests.cs            # Factory configuration
```

### Test Data

Use test configuration files:

```text
tests/TechHub.Api.Tests/TestData/
├── sections.test.json              # Test section configuration
├── appsettings.test.json           # Test app settings
└── sample-content/                 # Sample markdown files (if needed)
```

### Framework Usage

- **xUnit** - Test framework
- **FluentAssertions** - Assertions
- **Moq** - Mocking repositories/services
- **WebApplicationFactory** - In-memory test server

## Acceptance Criteria

### Overall Integration Test Coverage

1. ✅ All API endpoints have integration tests
2. ✅ CORS configuration properly tested
3. ✅ Security headers verified
4. ✅ Response caching validated
5. ✅ Middleware pipeline tested
6. ✅ Tests use WebApplicationFactory
7. ✅ Tests use mocked dependencies (NOT real file system)
8. ✅ Tests run fast (<30 seconds for full suite)
9. ✅ Tests are isolated (no shared state)
10. ✅ Tests document expected API behavior

## Priority

**P1** - Critical for API quality and stability

## Related Specifications

- [003-resilience-error-handling](../003-resilience-error-handling/) - Error handling patterns
- [013-api-endpoints](../013-api-endpoints/) - API endpoint definitions (documented in src/TechHub.Api/AGENTS.md)
- [tests/TechHub.Api.Tests/AGENTS.md](../../tests/TechHub.Api.Tests/AGENTS.md) - Integration testing patterns

## References

- [ASP.NET Core Integration Testing](https://learn.microsoft.com/aspnet/core/test/integration-tests)
- [WebApplicationFactory](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.testing.webapplicationfactory-1)
