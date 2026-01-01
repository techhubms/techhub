#!/usr/bin/env pwsh
# Post-create setup script for Tech Hub .NET

$ErrorActionPreference = "Stop"

Write-Host "Setting up Tech Hub .NET development environment..." -ForegroundColor Cyan

# Disable PowerShell GUI behaviors in container environment
Write-Host "Configuring PowerShell for container environment..."
$profileDir = Split-Path $PROFILE -Parent
if (-not (Test-Path $profileDir)) {
    New-Item -ItemType Directory -Path $profileDir -Force | Out-Null
}

@'
# Container environment configuration - disable GUI features
$PSDefaultParameterValues['Out-Default:OutVariable'] = $null
if ($env:TERM_PROGRAM -eq 'vscode' -or $env:REMOTE_CONTAINERS -eq 'true') {
    # Disable features that try to open browsers/GUIs
    $env:POWERSHELL_UPDATECHECK = 'Off'
    $env:POWERSHELL_TELEMETRY_OPTOUT = '1'
    $env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
}
'@ | Out-File -FilePath $PROFILE -Encoding utf8 -Force

# Ensure Node.js tools are available system-wide
Write-Host "Setting up Node.js tools system-wide..."
if (Test-Path "/usr/local/share/nvm/current/bin") {
    sudo ln -sf "/usr/local/share/nvm/current/bin/node" /usr/local/bin/node
    sudo ln -sf "/usr/local/share/nvm/current/bin/npm" /usr/local/bin/npm
    sudo ln -sf "/usr/local/share/nvm/current/bin/npx" /usr/local/bin/npx
}

# Update npm to latest version
Write-Host "Updating npm..."
npm install -g npm@latest

# Install PowerShell modules
Write-Host "Installing PowerShell modules..."
Install-Module Pester -Force -SkipPublisherCheck -MinimumVersion "5.0.0" -Scope CurrentUser

# Install .NET global tools
Write-Host "Installing .NET global tools..."
dotnet tool install --global dotnet-ef 2>$null
dotnet tool install --global dotnet-aspire 2>$null

# Install Playwright browsers for E2E tests and MCP
Write-Host "Installing Playwright system dependencies..."
sudo npx -y playwright install-deps

Write-Host "Installing Playwright browsers..."
npx -y playwright@latest install chromium chrome --force

# Restore .NET packages (only if solution exists)
if (Test-Path "TechHub.sln") {
    Write-Host "Restoring .NET packages..."
    dotnet restore
}

# Trust development certificates (skip on Linux as it requires GUI)
Write-Host "Setting up development certificates..."
if ($IsWindows) {
    dotnet dev-certs https --trust 2>$null
} else {
    # On Linux, just ensure cert exists but don't try to trust (no GUI)
    dotnet dev-certs https --check 2>$null
    if ($LASTEXITCODE -ne 0) {
        dotnet dev-certs https 2>$null
    }
}

Write-Host ""
Write-Host "=================================="
Write-Host "Development Environment Setup Complete!" -ForegroundColor Green
Write-Host "=================================="
Write-Host ""
