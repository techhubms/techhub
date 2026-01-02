# API Integration Testing

This document describes the integration testing strategy for the Tech Hub API.

## Overview

The API integration tests use `WebApplicationFactory<Program>` to spin up an in-memory test server with mocked file system dependencies. This approach provides fast, reliable tests that verify endpoint behavior without requiring actual file I/O operations.

## Test Infrastructure

### TechHubApiFactory

Custom `WebApplicationFactory<Program>` that:

- Creates an in-memory test server
- Replaces real repositories with NSubstitute mocks
- Provides test data setup methods
- Exposes `HttpClient` for making test requests

**Location**: `tests/TechHub.Api.Tests/TechHubApiFactory.cs`

### Test Data

The factory sets up:

- **2 sections**: AI and GitHub Copilot (with collections)
- **4 content items**: Mix of news, blogs, and videos with various tags

This minimal dataset covers all filtering and querying scenarios.

## Test Suites

### Section Endpoints Tests

**Location**: `tests/TechHub.Api.Tests/Endpoints/SectionsEndpointsTests.cs`

**Coverage**: 18 tests across 6 endpoints

1. **GET /api/sections/**
   - Returns all sections
   - Validates DTO structure

2. **GET /api/sections/{sectionId}**
   - Returns section by ID
   - Returns 404 for invalid section
   - Returns correct category

3. **GET /api/sections/{sectionId}/items**
   - Returns all items in section
   - Returns 404 for invalid section

4. **GET /api/sections/{sectionId}/collections/**
   - Returns collections for section
   - Returns 404 for invalid section

5. **GET /api/sections/{sectionId}/collections/{collectionName}**
   - Returns specific collection
   - Returns 404 for invalid section
   - Returns 404 for invalid collection

6. **GET /api/sections/{sectionId}/collections/{collectionName}/items**
   - Returns items in collection
   - Filters correctly by section and collection
   - Generates correct URLs
   - Returns 404 for invalid section
   - Returns 404 for invalid collection

### Content Filtering Tests

**Location**: `tests/TechHub.Api.Tests/Endpoints/ContentEndpointsTests.cs`

**Coverage**: 22 tests across 2 endpoints

1. **GET /api/content/filter**
   - No parameters returns all content
   - Single section filter
   - Multiple sections filter (OR logic)
   - Single collection filter
   - Multiple collections filter (OR logic)
   - Section AND collection filter
   - Single tag filter
   - Multiple tags filter (AND logic)
   - Complex multi-criteria filter
   - Text search across title/description/tags
   - Text search with section filter
   - Case-insensitive filtering
   - No matches returns empty list
   - Generates correct URLs
   - Preserves all ContentItem properties

2. **GET /api/content/tags**
   - Returns all unique tags

## Running Tests

### Run All Integration Tests

```bash
dotnet test tests/TechHub.Api.Tests/TechHub.Api.Tests.csproj
```

### Run Specific Test Class

```bash
# Section endpoints only
dotnet test tests/TechHub.Api.Tests/TechHub.Api.Tests.csproj --filter "FullyQualifiedName~SectionsEndpointsTests"

# Content filtering only
dotnet test tests/TechHub.Api.Tests/TechHub.Api.Tests.csproj --filter "FullyQualifiedName~ContentEndpointsTests"
```

### Run Specific Test

```bash
dotnet test tests/TechHub.Api.Tests/TechHub.Api.Tests.csproj --filter "FullyQualifiedName~FilterContent_ComplexFilter"
```

## Test Patterns

### Using FluentAssertions

All tests use FluentAssertions for readable, expressive assertions:

```csharp
response.StatusCode.Should().Be(HttpStatusCode.OK);
sections.Should().HaveCount(2);
section.Id.Should().Be("ai");
items.Should().NotBeNull();
items.Should().Contain(item => item.Tags.Contains("copilot"));
```

### Testing 404 Scenarios

```csharp
var response = await _client.GetAsync("/api/sections/invalid");
response.StatusCode.Should().Be(HttpStatusCode.NotFound);
```

### Testing Complex Filters

```csharp
var response = await _client.GetAsync(
    "/api/content/filter?sections=AI&collections=news&tags=copilot,azure"
);
var items = await response.Content.ReadFromJsonAsync<List<ContentItem>>();
items.Should().HaveCount(1);
items![0].Id.Should().Be("2024-01-15-ai-news-1");
```

## Benefits of This Approach

1. **No File System Dependencies**: Tests run in-memory with mocked data
2. **Fast Execution**: All 40 tests complete in ~1.4 seconds
3. **Isolated**: Each test gets a fresh `HttpClient` from the factory
4. **Realistic**: Tests actual HTTP request/response cycle
5. **Maintainable**: Test data defined once in factory setup
6. **Comprehensive**: Covers all endpoints, filters, and error scenarios

## Test Coverage Summary

| Category           | Tests | Coverage                                 |
| ------------------ | ----- | ---------------------------------------- |
| Section Endpoints  | 18    | All 6 endpoints with error handling      |
| Content Filtering  | 22    | All filter combinations and edge cases   |
| **Total**          | **40**| **Complete API surface**                 |

## Integration with CI/CD

These tests are designed to run in CI/CD pipelines:

```yaml
- name: Run API Integration Tests
  run: dotnet test tests/TechHub.Api.Tests/TechHub.Api.Tests.csproj --logger "trx;LogFileName=api-test-results.trx"
```

No external dependencies (databases, file systems) are required.
