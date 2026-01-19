# API Integration Tests - Tech Hub

> **AI CONTEXT**: This is a **LEAF** context file for API integration tests in the `tests/TechHub.Api.Tests/` directory. It complements the [tests/AGENTS.md](../AGENTS.md) testing strategy.
> **RULE**: Global rules (Timezone, Performance) in Root AGENTS.md apply **IN ADDITION** to local rules. Follow **BOTH**.

## Overview

This directory contains **integration tests** for the Tech Hub REST API using **xUnit** and **WebApplicationFactory**. These tests validate API endpoints, HTTP contracts, and request/response behavior against a real (in-memory) ASP.NET Core application.

**Implementation being tested**: See [src/TechHub.Api/AGENTS.md](../../src/TechHub.Api/AGENTS.md) for endpoint patterns.

## What This Directory Contains

**Test Files**: xUnit test classes that validate API endpoint behavior:

- `Endpoints/SectionsEndpointsTests.cs` - Tests for `/api/sections` endpoints
- `Endpoints/ContentEndpointsTests.cs` - Tests for `/api/content` endpoints

**Test Infrastructure**:

- `TechHubApiFactory.cs` - Custom `WebApplicationFactory<Program>` for test setup
- Configures in-memory test server
- Sets up test-specific configuration (file paths, etc.)

## Testing Strategy

**What to Test**:

- ✅ **HTTP status codes** (200, 404, 400, etc.)
- ✅ **Response body structure** (JSON serialization)
- ✅ **Query parameter handling** (filtering, pagination)
- ✅ **Content negotiation** (Accept headers)
- ✅ **Error responses** (validation errors, not found)
- ✅ **Integration with real repositories** (file-based)

**What NOT to Test**:

- ❌ **Business logic** (belongs in unit tests)
- ❌ **Repository internals** (belongs in Infrastructure tests)
- ❌ **UI rendering** (belongs in E2E tests)

## Test Patterns

### Using WebApplicationFactory

The `TechHubApiFactory` class configures `WebApplicationFactory<Program>` for integration testing:

- Creates an in-memory test server
- Allows dependency injection overrides for mocking
- Produces `HttpClient` instances for making requests

Test classes use `IClassFixture<TechHubApiFactory>` to share the factory across tests.

### What to Test

**HTTP Status Codes**:

- 200 OK for successful requests
- 404 Not Found for missing resources
- 400 Bad Request for validation errors

**Query Parameters**:

- Filtering by section, collection, tags
- Pagination parameters
- Invalid parameter handling

**Response Structure**:

- Deserialize responses to DTOs
- Verify expected properties are present
- Validate collection counts and ordering

See actual tests in `Endpoints/` for implementation examples.

## Running Tests

```powershell
# Run all API tests
dotnet test tests/TechHub.Api.Tests

# Run specific test class
dotnet test tests/TechHub.Api.Tests --filter "FullyQualifiedName~SectionsEndpointsTests"

# Run with detailed output
dotnet test tests/TechHub.Api.Tests --logger "console;verbosity=detailed"
```

## Best Practices

1. **Use FluentAssertions** for readable assertions
2. **Test realistic scenarios** - Use actual file-based data
3. **Verify response structure** - Deserialize and validate DTOs
4. **Test error cases** - Invalid inputs, missing resources
5. **Use [Theory]** for testing multiple similar scenarios
6. **Clean test names** - `MethodName_Scenario_ExpectedResult`

## Common Pitfalls

❌ **Don't mock repositories** in integration tests (use real implementations)  
❌ **Don't test business logic** here (belongs in unit tests)  
❌ **Don't share state** between tests (each test is isolated)  
❌ **Don't ignore status codes** (always verify expected HTTP codes)

## Related Documentation

- [tests/AGENTS.md](../AGENTS.md) - Complete testing strategy
- [src/TechHub.Api/AGENTS.md](../../src/TechHub.Api/AGENTS.md) - API development patterns
- [Root AGENTS.md](/AGENTS.md#6-test--validate) - When to write tests
