#!/bin/bash
set -e

echo "Setting up Tech Hub .NET development environment..."

# ==================== .NET Dev Certificates ====================
sudo dotnet workload update

echo "Setting up development certificates..."
if ! dotnet dev-certs https --check; then
    dotnet dev-certs https
fi

# ==================== .NET Global Tools ====================
echo "Installing .NET global tools..."
if ! dotnet tool list -g | grep -q dotnet-ef; then
    dotnet tool install --global dotnet-ef
else
    echo "dotnet-ef already installed"
fi

if ! dotnet tool list -g | grep -q microsoft.playwright.cli; then
    dotnet tool install --global Microsoft.Playwright.CLI
else
    dotnet tool update --global Microsoft.Playwright.CLI
fi

# ==================== Aspire ====================

#DO NOT INSTALL THE ASPIRE WORKLOAD VIA DOTNET NEW ANYMORE: The Aspire workload is deprecated and no longer necessary. Aspire is now available as NuGet packages that you can add directly to your projects. For more information, see https://aka.ms/aspire/support-policy

if ! command -v aspire &> /dev/null; then
    echo "Installing Aspire CLI..."
    curl -fsSL https://aspire.dev/install.sh | bash
    export PATH="$HOME/.aspire/bin:$PATH"
else
    echo "Aspire CLI already installed"
fi

echo "Installing/updating Aspire project templates..."
dotnet new install Aspire.ProjectTemplates

# ==================== Playwright ====================
echo "Installing Playwright browsers for .NET..."
# Build the test project to ensure Playwright.CLI is restored
cd /workspaces/techhub
dotnet build tests/TechHub.E2E.Tests/TechHub.E2E.Tests.csproj

# Install browsers using the .NET Playwright CLI
if [ -f "tests/TechHub.E2E.Tests/bin/Debug/net10.0/playwright.ps1" ]; then
    pwsh tests/TechHub.E2E.Tests/bin/Debug/net10.0/playwright.ps1 install --with-deps chromium
else
    echo "Warning: playwright.ps1 not found, browsers may not be installed"
fi

npx playwright install chrome

# ==================== PowerShell Modules ====================
echo "Installing PowerShell modules..."
pwsh -Command 'Install-Module HtmlToMarkdown -AcceptLicense -Force'
pwsh -Command 'Install-Module Pester -Force -SkipPublisherCheck -MinimumVersion "5.0.0" -Scope CurrentUser'

# ==================== Spec-Kit for Spec-Driven Development ====================
if ! command -v uv &> /dev/null; then
    echo "Installing uv package manager..."
    curl -LsSf https://astral.sh/uv/install.sh | sh
else
    echo "uv package manager already installed"
fi

# Ensure uv is in PATH
export PATH="$HOME/.local/bin:$PATH"

if ! uv tool list | grep -q specify-cli; then
    echo "Installing spec-kit CLI..."
    uv tool install specify-cli --from git+https://github.com/github/spec-kit.git
else
    echo "spec-kit CLI already installed"
fi

# ==================== PowerShell Profile ====================
echo "Setting up PowerShell profile..."
PWSH_PROFILE_DIR="$HOME/.config/powershell"
PWSH_PROFILE="$PWSH_PROFILE_DIR/Microsoft.PowerShell_profile.ps1"

mkdir -p "$PWSH_PROFILE_DIR"
rm -f "$PWSH_PROFILE"

cat > "$PWSH_PROFILE" << 'EOF'
# Tech Hub .NET Development Environment Profile

# ==================== PATH Configuration ====================
# Add uv and spec-kit tools
$env:PATH = "$HOME/.local/bin:$env:PATH"

# Add Aspire CLI
$env:PATH = "$HOME/.aspire/bin:$env:PATH"

# Add .NET global tools
$env:PATH = "$HOME/.dotnet/tools:$env:PATH"

# ==================== Opt-Out Settings ====================
# Disable .NET CLI telemetry
$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"

# Skip .NET first-time experience
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"

# Disable .NET CLI update checks
$env:DOTNET_CLI_UI_DISABLE_UPDATE_CHECK = "1"

# Disable PowerShell update notifications
$env:POWERSHELL_UPDATECHECK = "Off"

# Disable PowerShell telemetry
$env:POWERSHELL_TELEMETRY_OPTOUT = "1"

# ==================== Development Settings ====================
# Set default encoding to UTF-8
$PSDefaultParameterValues['Out-File:Encoding'] = 'utf8'
$PSDefaultParameterValues['*:Encoding'] = 'utf8'

# ==================== Welcome Message ====================
Write-Host "Tech Hub .NET Development Environment" -ForegroundColor Cyan
Write-Host "✅ PowerShell profile loaded" -ForegroundColor Green
EOF

chmod +x "$PWSH_PROFILE"

echo "PowerShell profile created at: $PWSH_PROFILE"
echo "Development environment setup complete!"
