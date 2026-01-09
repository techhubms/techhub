# Playwright Configuration for Tech Hub E2E Tests

## ✅ RESOLVED: Browser Page Creation Hang in DevContainer

### Root Cause

Playwright's default `chromium_headless_shell` binary **hangs indefinitely** when calling `browser.NewPageAsync()` in DevContainer environments. The browser process launches successfully, but page creation never completes.

### Solution

Use `Channel = "chrome"` to launch the full Chrome browser in headless mode instead of the lightweight headless_shell binary.

### Evidence

**Before Fix** (using default chromium_headless_shell):

```text
[1] Creating Playwright instance... ✓
[2] Launching Chromium browser... ✓  (pid=38698)
[3] Creating a new page... [HANGS FOREVER]
```

**After Fix** (using `Channel = "chrome"`):

```text
[1] Creating Playwright instance... ✓
[2] Launching Chromium browser... ✓
[3] Creating a new page... ✓
[4] Navigating to about:blank... ✓
✅ SUCCESS: All Playwright operations work!
```

### Implementation

**File**: `PlaywrightCollectionFixture.cs`

```csharp
Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    Headless = true,
    Channel = "chrome",  // CRITICAL: Use regular Chrome instead of headless_shell
    Timeout = 5000,
    Args = new[]
    {
        "--no-sandbox",
        "--disable-setuid-sandbox",
        "--disable-dev-shm-usage",
        "--disable-gpu"
    }
});
```

### Test Results

- **Before fix**: 0/116 Web E2E tests running (hung during fixture initialization)
- **After fix**: 82/116 Web E2E tests passing (71% success rate)
- **Remaining failures**: Actual test failures (timeouts, focus assertions), NOT infrastructure issues

## Supporting Optimizations

### 1. Limited Parallel Execution

**File**: `xunit.runner.json`

```json
{
  "maxParallelThreads": 2  // Prevents resource exhaustion in DevContainer
}
```

**Why 2 threads?**

- Balances parallelism with resource constraints
- Allows tests to complete without exhausting memory
- Still faster than sequential execution
- Can be increased to 4 on more powerful machines

### 2. Removed `--single-process` Browser Flag

**File**: `PlaywrightCollectionFixture.cs`

**Problem**: `--single-process` flag caused .NET test host crashes  
**Solution**: Removed from browser launch args  
**Impact**: Browser runs with normal multi-process architecture (more stable)

### 3. Smart Polling Instead of Hard-Coded Delays

**File**: `Helpers/BlazorHelpers.cs`

Existing smart polling functions **already avoid hard-coded delays**:

- `WaitForBlazorReady()` - Polls every 50ms until Blazor is ready
- `WaitForJavaScriptToSettle()` - Polls every 50ms until JS state stabilizes
- `WaitForUrlStateSync()` - Polls every 50ms until URL matches expected state

**No SlowMo needed** - tests already wait intelligently for conditions to be met.

## Browser Launch Configuration

```csharp
Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
{
    Headless = true,
    Timeout = 5000, // Works fine in DevContainer
    Args = new[]
    {
        "--no-sandbox",                // Required for Docker/DevContainer
        "--disable-setuid-sandbox",    // Required for Docker/DevContainer
        "--disable-web-security",      // Faster loading (test only!)
        "--disable-features=IsolateOrigins,site-per-process",
        "--disable-blink-features=AutomationControlled",
        "--disable-dev-shm-usage",     // Overcome limited resource problems
        "--disable-gpu",               // Disable GPU hardware acceleration
        "--no-zygote",                 // Disable zygote process (helps cleanup)
        // NOTE: --single-process REMOVED - causes test host crashes
    }
});
```

## Performance Characteristics

### Before Fix

- 10+ tests run in parallel
- Tests hang after 60 seconds
- Test host crashes
- 0 Web E2E tests pass

### After Fix

- 2 tests run in parallel
- Tests complete successfully
- No crashes
- 82/116 tests pass (71%)

## Monitoring & Tuning

**If tests still hang:**

1. Reduce `maxParallelThreads` to 1 (sequential)
2. Check server logs: `/tmp/techhub-tests/web-e2e.log`
3. Enable Playwright debug: `$env:DEBUG='pw:api'`

**If tests are slow but stable:**

1. Increase `maxParallelThreads` to 4 (faster)
2. Monitor resource usage with `htop` or `docker stats`
3. Adjust based on available CPU/memory

## References

- [xUnit Parallel Execution](https://xunit.net/docs/running-tests-in-parallel)
- [Playwright Browser Launch Options](https://playwright.dev/dotnet/docs/api/class-browsertype#browser-type-launch)
- [DevContainer Resource Limits](https://code.visualstudio.com/remote/advancedcontainers/resource-limits)
