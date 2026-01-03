# E2E Tests - Tech Hub

End-to-end tests using Playwright to verify complete user workflows and functionality.

## Prerequisites

1. **Playwright browsers must be installed**:

   ```powershell
   pwsh tests/TechHub.E2E.Tests/bin/Debug/net10.0/playwright.ps1 install
   ```

2. **Application must be running**:

   ```powershell
   # Start the entire application using the run script
   ./run.ps1
   ```

## Running Tests

### Run All E2E Tests

```powershell
# From repository root
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj
```

### Run Specific Test File

```powershell
# URL routing and navigation tests
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj --filter "FullyQualifiedName~UrlRoutingAndNavigationTests"

# Navigation improvements tests
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj --filter "FullyQualifiedName~NavigationImprovementsTests"
```

### Run Single Test

```powershell
# Example: Run only the URL routing test
dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj --filter "FullyQualifiedName~UrlRoutingAndNavigationTests.NavigateToSection_DefaultsToAllCollection"
```

## Test Coverage

### URL Routing and Navigation (UrlRoutingAndNavigationTests.cs)

Comprehensive tests for URL-based navigation and "all" collection functionality:

**URL Routing** (7 tests):

- ✅ Navigate to section defaults to /section/all
- ✅ Navigate to section/collection maintains URL
- ✅ Click collection button updates URL to /section/collection
- ✅ Click "All" button updates URL to /section/all
- ✅ Browser back button navigates to previous collection
- ✅ Browser forward button navigates to next collection

**"All" Collection Functionality** (6 tests):

- ✅ /section/all shows all content from that section
- ✅ /all/all shows everything from all sections and collections
- ✅ /all/collection shows that collection across all sections
- ✅ "All" button exists in collection sidebar
- ✅ "All" collection shows collection badges (proper capitalization)
- ✅ Specific collection hides redundant collection badge

**Interactive Buttons** (3 tests):

- ✅ All collection buttons are clickable and update URL
- ✅ Retry button reloads content after errors
- ✅ Active collection button has "active" CSS class

**URL Sharing and Bookmarking** (2 tests):

- ✅ Direct URL loads correct collection state
- ✅ Copied URL restores exact collection state in new tab

#### Total: 20 comprehensive test cases

### Navigation Improvements (NavigationImprovementsTests.cs)

Tests for section ordering, styling, and navigation flow:

- ✅ Homepage sections ordered correctly (starts with "Everything")
- ✅ Section card click navigates to section homepage
- ✅ Collection navigation updates URL (no hash fragments)
- ✅ Collection page hides redundant collection badge
- ✅ "All" page shows collection badge with proper capitalization
- ✅ Collection sidebar buttons are clickable
- ✅ Header area has consistent height
- ✅ Section background images display correctly (no grey bars)
- ✅ Direct URL to section/collection loads correct content

#### Total: 10 test cases

## Test Architecture

### Structure

```text
tests/TechHub.E2E.Tests/
├── Tests/
│   ├── UrlRoutingAndNavigationTests.cs  ← URL routing, "all" collection, buttons
│   └── NavigationImprovementsTests.cs   ← Section ordering, styling, navigation
├── TechHub.E2E.Tests.csproj
└── README.md (this file)
```

### Base Configuration

- **Browser**: Chromium (headless mode)
- **Web URL**: <http://localhost:5184>
- **API URL**: <http://localhost:5029>
- **Test Framework**: xUnit + Playwright
- **Assertions**: FluentAssertions for readable assertions

### Test Patterns

Each test class implements `IAsyncLifetime` for proper setup/teardown:

```csharp
public async Task InitializeAsync()
{
    _playwright = await Playwright.CreateAsync();
    _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
}

public async Task DisposeAsync()
{
    if (_browser != null)
        await _browser.DisposeAsync();
    
    _playwright?.Dispose();
}
```

## Writing New Tests

### Test Naming Convention

Use the format: `ComponentOrFeature_Action_ExpectedResult`

Examples:

- `NavigateToSection_DefaultsToAllCollection`
- `ClickCollectionButton_UpdatesURL`
- `AllCollection_ShowsAllContentFromSection`

### Assertion Style

Use FluentAssertions for readable, descriptive assertions:

```csharp
// ✅ Good - descriptive with reason
page.Url.Should().EndWith("/github-copilot/all",
    "navigating to a section without collection should default to /section/all");

// ❌ Bad - no context
Assert.Equal("/github-copilot/all", page.Url);
```

### Waiting for Elements

Always wait for elements before interacting:

```csharp
// Wait for element to be ready
await page.WaitForSelectorAsync(".collection-nav");

// Then interact
var button = page.Locator(".collection-nav button", new() { HasTextString = "News" });
await button.ClickAsync();

// Wait for navigation to complete
await page.WaitForURLAsync("**/github-copilot/news");
```

## Debugging Tests

### Run Tests in Headed Mode

Modify `LaunchAsync` in test setup:

```csharp
_browser = await _playwright.Chromium.LaunchAsync(new() { Headless = false });
```

### Slow Down Execution

```csharp
_browser = await _playwright.Chromium.LaunchAsync(new() 
{ 
    Headless = false,
    SlowMo = 1000 // 1 second delay between actions
});
```

### Screenshot on Failure

```csharp
try
{
    // Test code
}
catch
{
    await page.ScreenshotAsync(new() { Path = "test-failure.png" });
    throw;
}
```

## Continuous Integration

These tests should run in CI/CD pipeline:

```yaml
# Example GitHub Actions workflow
- name: Install Playwright
  run: pwsh tests/TechHub.E2E.Tests/bin/Debug/net10.0/playwright.ps1 install

- name: Start Application
  run: ./run.ps1 &
  
- name: Wait for Application
  run: Start-Sleep -Seconds 30

- name: Run E2E Tests
  run: dotnet test tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj
```

## Maintenance

### When to Update Tests

- ✅ **ALWAYS** when changing URL routes or navigation behavior
- ✅ **ALWAYS** when modifying collection filtering logic
- ✅ **ALWAYS** when changing button behavior or interactivity
- ✅ **REQUIRED** when adding new collections or sections
- ⚠️ **CONSIDER** when changing CSS selectors (tests may break)

### Test Stability

All tests should be:

- **Deterministic**: Same input → same result
- **Isolated**: Tests don't depend on each other
- **Fast**: Complete in <30 seconds total
- **Clear**: Failures point to exact issue

## Related Documentation

- [Root AGENTS.md](/AGENTS.md) - Overall development workflow and TDD approach
- [tests/AGENTS.md](/tests/AGENTS.md) - Testing strategies across all layers
- [src/TechHub.Web/AGENTS.md](/src/TechHub.Web/AGENTS.md) - Blazor component patterns
- [specs/dotnet-migration/tasks.md](/specs/dotnet-migration/tasks.md) - Task tracking and completion status
