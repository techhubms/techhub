# Jekyll Development Server Script

[CmdletBinding()]
param(
    [switch]$SkipStop,
    [switch]$SkipClean,
    [switch]$BuildInsteadOfServe,
    [switch]$VerboseOutput
)

$ErrorActionPreference = "Stop"
$ProgressPreference = 'SilentlyContinue'
Set-StrictMode -Version Latest

# Validate that no unknown parameters were passed
$boundParameters = $PSBoundParameters.Keys
$validParameters = @('SkipStop', 'SkipClean', 'BuildInsteadOfServe', 'VerboseOutput')
$unknownParameters = $boundParameters | Where-Object { $_ -notin $validParameters }

if ($unknownParameters) {
    Write-Host "Error: Unknown parameter(s) specified: $($unknownParameters -join ', ')" -ForegroundColor Red
    Write-Host "Valid parameters are: $($validParameters -join ', ')" -ForegroundColor Yellow
    exit 1
}

try {
    if (-not $SkipStop) {
        # Stop existing Jekyll processes using separate script
        & "./jekyll-stop.ps1"
    }
    
    if (-not $SkipClean) {
        # Clean Jekyll
        Write-Host "Cleaning Jekyll cache..." -ForegroundColor Yellow
        & bundle exec jekyll clean
        if ($LASTEXITCODE -ne 0) {
            throw "Jekyll clean failed with exit code $LASTEXITCODE"
        }
        Write-Host "Jekyll cache cleaned successfully" -ForegroundColor Green
    }

    $jekyllArgs = @()
    if ($BuildInsteadOfServe) {
        # Build site only
        Write-Host "Only building Jekyll site..." -ForegroundColor Cyan
        $jekyllArgs = @('build')
    }
    else {
        # Start Jekyll server
        Write-Host "Building and serving Jekyll site..." -ForegroundColor Cyan
        $jekyllArgs = @('serve', "--host", "0.0.0.0", "--watch", "--livereload", "--force_polling", "--incremental")
    }

    # Build Jekyll command with optional verbose flag
    if ($VerboseOutput) {
        $jekyllArgs += "--verbose"
    }
    
    # Run Jekyll serve directly in current terminal
    & bundle exec jekyll @jekyllArgs
}
catch {
    Write-Host "Jekyll script error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}