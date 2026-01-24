# TechHub.TestUtilities

Shared test infrastructure for all test projects in the Tech Hub solution.

## Purpose

This project provides unified test utilities to avoid duplication across integration and E2E test projects.

## TechHubApiFactory

Consolidated `WebApplicationFactory<Program>` for all API tests.

**Usage**:

```csharp
// Integration tests (default - uses appsettings.IntegrationTest.json)
public class MyTests : IClassFixture<TechHubApiFactory>
{
    private readonly HttpClient _client;

    public MyTests(TechHubApiFactory factory)
    {
        _client = factory.CreateClient();
    }
}

// E2E tests (workspace root for real content files)
public class MyE2ETests : IClassFixture<TechHubApiFactory>
{
    private readonly HttpClient _client;

    public MyE2ETests()
    {
        var factory = TechHubApiFactory.ForE2ETests();
        _client = factory.CreateClient();
    }
}
```

**Features**:

- **IntegrationTest environment**: Uses `appsettings.IntegrationTest.json` for test configuration
- **Real data**: Both integration and E2E tests use actual content from `collections/` directory
- **Workspace-aware**: E2E tests automatically find workspace root from test assembly location
- **Minimal logging**: Suppresses verbose logs during tests (LogLevel.Warning minimum)

**Key Differences**:

- **Integration tests** (default): Run from bin directory, use relative paths to collections
- **E2E tests** (`ForE2ETests()`): Set content root to workspace root, explicit path configuration

## Configuration

Tests use the **IntegrationTest** environment:

- API: `src/TechHub.Api/appsettings.IntegrationTest.json`
- Web: `src/TechHub.Web/appsettings.IntegrationTest.json`

These files mimic Production configuration but with test-specific log paths (`/workspaces/techhub/.tmp/logs/*-integrationtest.log`).

## Dependencies

- **Microsoft.AspNetCore.Mvc.Testing**: WebApplicationFactory support
- **TechHub.Api**: API program for factory
- **TechHub.Core**: Domain models and interfaces

## Migration from Old Factories

The following old factories have been consolidated:

- ❌ **TechHub.Api.Tests/TechHubApiFactory.cs** → ✅ **TechHub.TestUtilities/TechHubApiFactory.cs**
- ❌ **TechHub.E2E.Tests/Api/ApiTestFactory.cs** → ✅ **TechHub.TestUtilities/TechHubApiFactory.cs**

**Key changes**:

- Removed mocking infrastructure (both test types now use real data)
- Removed `SetupDefaultSections()` and `SetupDefaultContent()` methods
- Simplified to single boolean parameter: `useWorkspaceRoot`
- All tests now run against actual content files for consistency
