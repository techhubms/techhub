#!/usr/bin/env pwsh

# E2E Test Runner Script for Tech Hub
# This script ensures Jekyll is running and executes the Playwright test suite
# Cross-platform compatible for Windows, Linux, and devcontainers

param(
    [switch]$Debug,
    [switch]$UI,
    [string]$Grep = "",
    [string]$TestFile = "",
    [int]$MaxFailures = 999,
    [switch]$Verbose
)

$ErrorActionPreference = "Stop"
$ProgressPreference = 'SilentlyContinue'
Set-StrictMode -Version Latest

# Detect environment and configure accordingly
function Get-Environment {
    if ($env:GITHUB_ACTIONS -eq "true") {
        return "GitHubActions"
    } elseif ($env:CODESPACES -eq "true") {
        return "GitHubCodespaces"
    } elseif ((Test-Path "/.devcontainer.json") -or $env:REMOTE_CONTAINERS -eq "true") {
        return "DevContainer"
    } elseif ($IsWindows -or ($PSVersionTable.Platform -eq "Win32NT")) {
        return "Windows"
    } elseif ($IsLinux) {
        return "Linux"
    } elseif ($IsMacOS) {
        return "MacOS"
    } else {
        return "Unknown"
    }
}

function Get-ProjectRoot {
    # Load Get-SourceRoot function if available
    $sourceRootPath = Join-Path $PSScriptRoot ".github/scripts/functions/Get-SourceRoot.ps1"
    if (Test-Path $sourceRootPath) {
        . $sourceRootPath
        return Get-SourceRoot
    }
    
    # Fallback logic
    if ($env:GITHUB_WORKSPACE) {
        return $env:GITHUB_WORKSPACE
    }
    
    # Search upward for repository indicators
    $currentPath = $PSScriptRoot
    while ($currentPath -and $currentPath -ne [System.IO.Path]::GetPathRoot($currentPath)) {
        if (Test-Path (Join-Path $currentPath ".git")) {
            return $currentPath
        }
        if (Test-Path (Join-Path $currentPath "_config.yml")) {
            return $currentPath
        }
        $currentPath = Split-Path $currentPath -Parent
    }
    
    # Ultimate fallback
    return $PSScriptRoot
}

# Get environment and root directory
$script:environment = Get-Environment
$script:rootDir = Get-ProjectRoot
$script:e2eDir = Join-Path $script:rootDir "spec/e2e"

# Colors for output (compatible with both Windows and Linux terminals)
if ($IsWindows -or ($PSVersionTable.Platform -eq "Win32NT")) {
    # Windows PowerShell or PowerShell Core on Windows
    $Red = "`e[31m"
    $Green = "`e[32m"
    $Yellow = "`e[33m"
    $Blue = "`e[34m"
    $Reset = "`e[0m"
} else {
    # Linux/macOS PowerShell Core
    $Red = "`e[31m"
    $Green = "`e[32m"
    $Yellow = "`e[33m"
    $Blue = "`e[34m"
    $Reset = "`e[0m"
}

function Invoke-WithRetry {
    param(
        [ScriptBlock]$Command,
        [int]$MaxAttempts = 3,
        [int]$DelaySeconds = 5,
        [string]$OperationName = "Operation"
    )
    
    for ($attempt = 1; $attempt -le $MaxAttempts; $attempt++) {
        try {
            Write-ColoredOutput "🔄 $OperationName (attempt ${attempt}/${MaxAttempts})..." $Yellow
            
            $result = & $Command
            
            if ($LASTEXITCODE -eq 0 -or $result) {
                Write-ColoredOutput "✅ $OperationName succeeded on attempt ${attempt}" $Green
                return $true
            }
            
            if ($attempt -lt $MaxAttempts) {
                Write-ColoredOutput "⚠️  $OperationName failed on attempt ${attempt}, retrying in ${DelaySeconds}s..." $Yellow
                Start-Sleep -Seconds $DelaySeconds
            }
        }
        catch {
            if ($attempt -lt $MaxAttempts) {
                Write-ColoredOutput "⚠️  $OperationName error on attempt ${attempt}: $($_.Exception.Message)" $Yellow
                Write-ColoredOutput "🔄 Retrying in ${DelaySeconds}s..." $Yellow
                Start-Sleep -Seconds $DelaySeconds
            } else {
                Write-ColoredOutput "❌ $OperationName failed after $MaxAttempts attempts: $($_.Exception.Message)" $Red
                throw
            }
        }
    }
    
    Write-ColoredOutput "❌ $OperationName failed after $MaxAttempts attempts" $Red
    return $false
}

function Write-ColoredOutput {
    param($Message, $Color)
    Write-Host "$Color$Message$Reset"
}

function Test-JekyllRunning {
    try {
        # Cross-platform port checking
        if ($script:environment -eq "Windows") {
            # Windows: use netstat
            $result = netstat -an | Select-String ":4000.*LISTENING"
            return $null -ne $result
        } else {
            # Linux/macOS: use netstat or ss
            if (Get-Command ss -ErrorAction SilentlyContinue) {
                $result = ss -tlnp 2>/dev/null | grep ":4000"
                return $null -ne $result
            } else {
                $result = netstat -tlnp 2>/dev/null | grep ":4000"
                return $null -ne $result
            }
        }
    }
    catch {
        # Fallback: try to make a simple HTTP request
        try {
            $response = Invoke-WebRequest -Uri "http://localhost:4000" -TimeoutSec 2 -ErrorAction SilentlyContinue
            return $response.StatusCode -eq 200
        }
        catch {
            return $false
        }
    }
}

function Test-JekyllDependencies {
    # Check if bundler is available
    try {
        $bundlerCommand = if ($script:environment -eq "Windows") { "bundle.bat" } else { "bundle" }
        
        # Try to find bundler in PATH
        $bundlerPath = Get-Command $bundlerCommand -ErrorAction SilentlyContinue
        if (-not $bundlerPath) {
            # Try alternative bundler command
            $bundlerPath = Get-Command "bundle" -ErrorAction SilentlyContinue
        }
        
        if (-not $bundlerPath) {
            Write-ColoredOutput "⚠️  Bundler not found in PATH" $Yellow
            return $false
        }
        
        # Check if Gemfile exists in root
        if (-not (Test-Path (Join-Path $script:rootDir "Gemfile"))) {
            Write-ColoredOutput "⚠️  Gemfile not found in project root" $Yellow
            return $false
        }
        
        # Test if bundle can list gems
        $originalLocation = Get-Location
        try {
            Set-Location $script:rootDir
            
            # Use the found bundler command
            $bundlerExe = $bundlerPath.Source
            & $bundlerExe list 2>$null | Out-Null
            return $LASTEXITCODE -eq 0
        }
        finally {
            Set-Location $originalLocation
        }
    }
    catch {
        Write-ColoredOutput "⚠️  Error checking Jekyll dependencies: $($_.Exception.Message)" $Yellow
        return $false
    }
}



function Start-Jekyll {
    Write-ColoredOutput "Starting Jekyll server..." $Blue
    
    # Check Jekyll dependencies first
    if (-not (Test-JekyllDependencies)) {
        Write-ColoredOutput "❌ Jekyll dependencies not available" $Red
        Write-ColoredOutput "💡 Please ensure dependencies are installed via post-create.sh or CI setup" $Yellow
        return $false
    }
    
    # Use script variables for paths
    $jekyllScript = Join-Path $script:rootDir "jekyll-start.ps1"
    
    # Ensure Jekyll script exists
    if (-not (Test-Path $jekyllScript)) {
        Write-ColoredOutput "❌ Jekyll startup script not found: $jekyllScript" $Red
        return $false
    }
    
    # Start Jekyll in background with correct working directory
    try {
        Start-Process -FilePath "pwsh" -ArgumentList $jekyllScript -WorkingDirectory $script:rootDir -NoNewWindow
    }
    catch {
        Write-ColoredOutput "❌ Failed to start Jekyll process: $($_.Exception.Message)" $Red
        return $false
    }
    
    # Wait for Jekyll to start with extended timeout
    $timeout = 300  # Extended timeout for slower systems - Jekyll can take up to 2-3 minutes
    $elapsed = 0
    
    Write-ColoredOutput "Waiting for Jekyll server to start (timeout: ${timeout}s)..." $Yellow
    
    while (-not (Test-JekyllRunning) -and $elapsed -lt $timeout) {
        Start-Sleep -Seconds 2
        $elapsed += 2
        Write-Host "." -NoNewline
        
        # Check every 10 seconds if Jekyll process might have failed
        if ($elapsed % 10 -eq 0) {
            Write-Host ""
            Write-ColoredOutput "Still waiting... ($elapsed/${timeout}s)" $Yellow
        }
    }
    
    Write-Host ""
    
    if (Test-JekyllRunning) {
        Write-ColoredOutput "✅ Jekyll server is running on port 4000" $Green
        return $true
    }
    else {
        Write-ColoredOutput "❌ Failed to start Jekyll server within $timeout seconds" $Red
        Write-ColoredOutput "💡 Check if Jekyll dependencies are properly installed" $Yellow
        Write-ColoredOutput "💡 Try running manually: ./jekyll-start.ps1" $Yellow
        return $false
    }
}

function Test-DependenciesInstalled {
    # Check if node_modules exists and playwright is installed
    $nodeModules = Join-Path $script:e2eDir "node_modules"
    $playwrightBinary = Join-Path $nodeModules ".bin/playwright"
    
    if (-not (Test-Path $nodeModules)) {
        return $false
    }
    
    if (-not (Test-Path $playwrightBinary)) {
        return $false
    }
    
    # Dependencies are installed if we reach here
    return $true
}

function Test-BrowsersInstalled {
    Set-Location $script:e2eDir
    
    try {
        # Check for actual browser executable files
        # Playwright typically installs browsers in ~/.cache/ms-playwright
        $playwrightCache = "$env:HOME/.cache/ms-playwright"
        
        # Check if the cache directory exists
        if (-not (Test-Path $playwrightCache)) {
            return $false
        }
        
        # Look for chromium browser directory (which should exist if browsers are installed)
        $chromiumDirs = Get-ChildItem -Path $playwrightCache -Directory -Name | Where-Object { $_ -match "chromium" }
        
        if ($chromiumDirs.Count -eq 0) {
            return $false
        }
        
        # Check if at least one chromium directory has the actual browser executable
        foreach ($dir in $chromiumDirs) {
            $chromiumPath = Join-Path $playwrightCache $dir
            $executablePaths = @(
                "chrome-linux/chrome",
                "chrome-linux/headless_shell", 
                "chrome-win/chrome.exe",
                "chrome-mac/Chromium.app/Contents/MacOS/Chromium"
            )
            
            foreach ($execPath in $executablePaths) {
                $fullPath = Join-Path $chromiumPath $execPath
                if (Test-Path $fullPath) {
                    return $true
                }
            }
        }
        
        return $false
    }
    catch {
        return $false
    }
}

function Invoke-Tests {
    param($TestArgs)
    
    Set-Location $script:e2eDir
    
    Write-ColoredOutput "Running Playwright tests..." $Blue
    Write-ColoredOutput "Test directory: $script:e2eDir" $Yellow
    
    # Always show which tests are being discovered
    Write-ColoredOutput "Discovering test files..." $Yellow
    $testFiles = Get-ChildItem -Path "tests/*.spec.js" -Name
    if ($testFiles.Count -gt 0) {
        Write-ColoredOutput "Found test files:" $Green
        foreach ($file in $testFiles) {
            Write-ColoredOutput "  - $file" $Green
        }
    }
    else {
        Write-ColoredOutput "No test files found in tests/ directory" $Red
        return 1
    }
    
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "Starting test execution..." $Blue
    Write-ColoredOutput "Command: npx playwright test --reporter=list --max-failures=$MaxFailures $TestArgs" $Yellow
    Write-ColoredOutput "================================" $Blue
    Write-ColoredOutput "" $Reset
    
    # Execute the tests using Start-Process to ensure output is visible
    # Clear DISPLAY to prevent WSL2 timeout issues (GitHub issue #18255)
    $env:DISPLAY = ""
    $arguments = "playwright test --reporter=list --max-failures=$MaxFailures"
    
    if ($TestArgs) {
        $arguments += " $TestArgs"
    }
    
    $process = Start-Process -FilePath "npx" -ArgumentList $arguments -NoNewWindow -Wait -PassThru
    $exitCode = $process.ExitCode
    
    Write-ColoredOutput "" $Reset
    Write-ColoredOutput "================================" $Blue
    
    if ($exitCode -eq 0) {
        Write-ColoredOutput "✅ All E2E tests passed!" $Green
    }
    else {
        Write-ColoredOutput "❌ Some E2E tests failed. Exit code: $exitCode" $Red
        Write-ColoredOutput "" $Reset
        Write-ColoredOutput "To debug failed tests, you can run:" $Yellow
        Write-ColoredOutput "  run-e2e-tests.ps1 -Debug" $Yellow
        Write-ColoredOutput "  run-e2e-tests.ps1 -UI" $Yellow
        Write-ColoredOutput "  run-e2e-tests.ps1 -TestFile 'tests/tag-filtering.spec.js'" $Yellow
    }
    
    return $exitCode
}

function Test-Prerequisites {
    Write-ColoredOutput "🔍 Checking system prerequisites..." $Yellow
    $issues = @()
    
    # Check PowerShell version
    $psVersion = $PSVersionTable.PSVersion
    if ($psVersion.Major -lt 5) {
        $issues += "PowerShell version $psVersion is too old (requires 5.0+)"
    } else {
        Write-ColoredOutput "✅ PowerShell $psVersion" $Green
    }
    
    # Check if we're in the right directory
    $sourceRoot = Get-ProjectRoot
    if (-not (Test-Path (Join-Path $sourceRoot "package.json"))) {
        $issues += "Not in project root directory or missing package.json"
    } else {
        Write-ColoredOutput "✅ Project root directory located" $Green
    }
    
    # Check Node.js availability
    if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
        $issues += "Node.js not found in PATH"
    } else {
        $nodeVersion = node --version
        Write-ColoredOutput "✅ Node.js $nodeVersion" $Green
    }
    
    # Check npm availability
    if (-not (Get-Command npm -ErrorAction SilentlyContinue)) {
        $issues += "npm not found in PATH"
    } else {
        $npmVersion = npm --version
        Write-ColoredOutput "✅ npm $npmVersion" $Green
    }
    
    if ($issues.Count -gt 0) {
        Write-ColoredOutput "❌ Prerequisites check failed:" $Red
        foreach ($issue in $issues) {
            Write-ColoredOutput "   • $issue" $Red
        }
        return $false
    }
    
    Write-ColoredOutput "✅ All prerequisites satisfied" $Green
    return $true
}

function Invoke-EndToEndTestsRunner {
    # Main execution
    try {
        Write-ColoredOutput "Tech Hub E2E Test Runner" $Blue
        Write-ColoredOutput "================================" $Blue
        Write-ColoredOutput "Environment: $script:environment" $Yellow
        Write-ColoredOutput "Project Root: $script:rootDir" $Yellow
        Write-ColoredOutput "E2E Directory: $script:e2eDir" $Yellow
        Write-ColoredOutput "PowerShell Version: $($PSVersionTable.PSVersion)" $Yellow
        Write-ColoredOutput "" $Reset
        
        # Validate directories exist
        if (-not (Test-Path $script:rootDir)) {
            Write-ColoredOutput "❌ Project root directory not found: $script:rootDir" $Red
            exit 1
        }
        
        if (-not (Test-Path $script:e2eDir)) {
            Write-ColoredOutput "❌ E2E test directory not found: $script:e2eDir" $Red
            Write-ColoredOutput "💡 Expected directory: spec/e2e" $Yellow
            exit 1
        }
        
        # Check prerequisites first
        if (-not (Test-Prerequisites)) {
            Write-ColoredOutput "❌ Prerequisites not met. Cannot continue." $Red
            exit 1
        }
        
        Write-ColoredOutput "" $Reset
        
        # Clean up any leftover Playwright processes for fresh environment
        Write-ColoredOutput "🧹 Cleaning up leftover Playwright processes..." $Yellow
        try {
            # Clean up chromium profile processes (addresses GitHub issue #18255)
            bash -c 'pkill -f "playwright_chromiumdev_profile" 2>/dev/null || true'
            Write-ColoredOutput "✅ Process cleanup completed" $Green
        }
        catch {
            # If cleanup fails, it's not critical - continue with tests
            Write-ColoredOutput "⚠️  Process cleanup failed (non-critical): $($_.Exception.Message)" $Yellow
        }
        
        # Clear DISPLAY variable to prevent WSL2 timeout issues (GitHub issue #18255)
        Write-ColoredOutput "🔧 Configuring environment for headless testing..." $Yellow
        $env:DISPLAY = ""
        Write-ColoredOutput "✅ Environment configured (DISPLAY cleared)" $Green

        Write-ColoredOutput "" $Reset
        
        # Check and install dependencies if needed
        Write-ColoredOutput "🔍 Checking dependencies..." $Yellow
        
        # Ensure we're in the right directory and package.json exists
        if (-not (Test-Path (Join-Path $script:e2eDir "package.json"))) {
            Write-ColoredOutput "❌ package.json not found in spec/e2e directory. Exiting." $Red
            exit 1
        }
        
        if (-not (Test-DependenciesInstalled)) {
            Write-ColoredOutput "❌ Dependencies not found." $Red
            exit 1
        }
        else {
            Write-ColoredOutput "✅ npm dependencies already installed" $Green
            
            # Check if browsers are available
            if (-not (Test-BrowsersInstalled)) {
                Write-ColoredOutput "❌ Playwright browsers not available" $Red
                Write-ColoredOutput "💡 Please ensure browsers are installed via post-create.sh or CI setup" $Yellow
                exit 1
            }
            else {
                Write-ColoredOutput "✅ Playwright browsers already installed" $Green
            }
        }
        
        # Final check - warn if no browser is available but don't fail completely
        if (-not (Test-BrowsersInstalled) -and -not $env:PLAYWRIGHT_CHROMIUM_EXECUTABLE_PATH) {
            Write-ColoredOutput "⚠️  No Playwright browsers detected, tests may fail" $Yellow
            Write-ColoredOutput "💡 Consider installing system browser: sudo apt-get install chromium-browser" $Yellow
        } else {
            Write-ColoredOutput "✅ Browser environment ready for testing" $Green
        }
        
        # Check if Jekyll is running
        Write-ColoredOutput "🔍 Checking Jekyll server status..." $Yellow
        if (-not (Test-JekyllRunning)) {
            Write-ColoredOutput "🚀 Jekyll server is not running. Starting it now..." $Yellow
            
            if (-not (Start-Jekyll)) {
                Write-ColoredOutput "❌ Cannot proceed without Jekyll server. Exiting." $Red
                Write-ColoredOutput "💡 Try starting Jekyll manually: ./jekyll-start.ps1" $Yellow
                exit 1
            }
        }
        else {
            Write-ColoredOutput "✅ Jekyll server is already running on port 4000" $Green
        }
        
        # Build test arguments
        $testArgs = @()
        
        if ($TestFile) {
            $testArgs += $TestFile
            Write-ColoredOutput "📄 Running specific test file: $TestFile" $Yellow
        }
        
        if ($Debug) {
            $testArgs += "--debug"
            Write-ColoredOutput "🐛 Running tests in debug mode (--debug)" $Yellow
        }
        
        if ($UI) {
            $testArgs += "--ui"
            Write-ColoredOutput "🎨 Running tests with UI mode (--ui)" $Yellow
        }
        
        if ($Grep) {
            $testArgs += "--grep=`"$Grep`""
            Write-ColoredOutput "🔍 Filtering tests with pattern: $Grep" $Yellow
        }
        
        if ($MaxFailures -gt 0) {
            Write-ColoredOutput "⚠️  Max failures set to: $MaxFailures" $Yellow
        }
        else {
            Write-ColoredOutput "🔄 Running all tests (no failure limit)" $Yellow
        }
        
        Write-ColoredOutput "" $Reset
        
        # Run the tests
        $result = Invoke-Tests ($testArgs -join " ")
        
        Write-ColoredOutput "" $Reset
        if ($result -eq 0) {
            Write-ColoredOutput "🎉 E2E test execution completed successfully!" $Green
        }
        else {
            Write-ColoredOutput "💥 E2E test execution completed with failures!" $Red
        }
        
        exit $result
        
    }
    catch {
        Write-ColoredOutput "💥 An error occurred: $($_.Exception.Message)" $Red
        Write-ColoredOutput "Stack trace: $($_.Exception.StackTrace)" $Red
        exit 1
    }
}

$location = Get-Location
try {
    Invoke-EndToEndTestsRunner
}
finally
{
    Set-Location $location
}