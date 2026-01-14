#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Formats all code in the solution using dotnet format.

.DESCRIPTION
    Applies consistent code formatting to all C# files in the TechHub solution.
    Uses the formatting rules defined in .editorconfig and Directory.Build.props.

.PARAMETER Verify
    If specified, verifies formatting without making changes (dry-run mode).

.EXAMPLE
    .\format-code.ps1
    Formats all code files in the solution.

.EXAMPLE
    .\format-code.ps1 -Verify
    Verifies formatting without making changes.
#>

param(
    [switch]$Verify
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Get solution root (script is in .github/skills/cleanup/scripts/, solution root is 4 levels up)
$SolutionRoot = Split-Path -Parent (Split-Path -Parent (Split-Path -Parent (Split-Path -Parent $PSScriptRoot)))
$SolutionFile = Join-Path $SolutionRoot "TechHub.slnx"

Write-Host "Formatting code in solution: $SolutionFile" -ForegroundColor Cyan

try {
    if ($Verify) {
        Write-Host "Running format verification (no changes will be made)..." -ForegroundColor Yellow
        dotnet format $SolutionFile --verify-no-changes
    }
    else {
        Write-Host "Applying code formatting..." -ForegroundColor Yellow
        dotnet format $SolutionFile
    }

    if ($LASTEXITCODE -eq 0) {
        Write-Host "`n✅ Code formatting completed successfully" -ForegroundColor Green
    }
    else {
        Write-Host "`n❌ Code formatting failed with exit code: $LASTEXITCODE" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}
catch {
    Write-Host "`n❌ Error during code formatting: $_" -ForegroundColor Red
    exit 1
}
