#!/bin/bash
set -e

echo "Setting up Tech Hub .NET development environment..."

# ==================== .NET Dev Certificates ====================
sudo dotnet workload update

echo "Setting up development certificates..."
if ! dotnet dev-certs https --check; then
    dotnet dev-certs https
fi

# Export and trust the development certificate for Chrome/Chromium (used by Playwright)
echo "Trusting development certificates for Playwright/Chrome..."

# Install NSS tools for certificate management
sudo apt-get update > /dev/null 2>&1
sudo apt-get install -y libnss3-tools > /dev/null 2>&1

CERT_PATH="$HOME/.aspnet/https/aspnetapp.pfx"
CERT_PEM="$HOME/.aspnet/https/aspnetapp.pem"
mkdir -p "$HOME/.aspnet/https"

# Export the dev cert to a file (without --trust as it doesn't work in Linux containers)
dotnet dev-certs https -ep "$CERT_PATH" -p "password"

# Convert PFX to PEM format for Chrome's certificate store
openssl pkcs12 -in "$CERT_PATH" -out "$CERT_PEM" -nodes -password pass:password

# Initialize NSS database if it doesn't exist
mkdir -p "$HOME/.pki/nssdb"
if [ ! -f "$HOME/.pki/nssdb/cert9.db" ]; then
    certutil -d sql:"$HOME/.pki/nssdb" -N --empty-password
fi

# Add certificate to NSS database (used by Chrome/Chromium)
certutil -d sql:"$HOME/.pki/nssdb" -A -t "C,," -n "ASP.NET Core HTTPS Dev Cert" -i "$CERT_PEM" || true

echo "✓ Development certificates configured"

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

# ==================== Playwright ====================

# Install Chrome browser with dependencies for both .NET E2E tests and Playwright MCP tools
# This installs Chrome system-wide to /usr/bin/google-chrome
echo "Installing Chrome browser for Playwright..."
# Run from E2E test project directory to find the Playwright package reference
cd tests/TechHub.E2E.Tests
playwright install chrome --with-deps
cd ../..

# ==================== npm Dependencies ====================
echo "Installing npm dependencies..."
npm install

# Install markdownlint-cli2 globally for CLI access
echo "Installing markdownlint-cli2 globally..."
npm install -g markdownlint-cli2 || echo "Warning: Failed to install markdownlint-cli2 globally, using local version from package.json"

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

# Add npm global packages
$env:PATH = "/usr/local/share/nvm/versions/node/v24.13.0/bin:$env:PATH"

# Add local npm packages (for project-specific tools like markdownlint-cli2)
if (Test-Path "/workspaces/techhub/node_modules/.bin") {
    $env:PATH = "/workspaces/techhub/node_modules/.bin:$env:PATH"
}

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

# ==================== Tech Hub Runner Module ====================
# Auto-load the TechHubRunner module for 'Run' command
$techHubRoot = "/workspaces/techhub"
if (Test-Path $techHubRoot) {
    $modulePath = Join-Path $techHubRoot "scripts/TechHubRunner.psm1"
    if (Test-Path $modulePath) {
        Import-Module $modulePath -Force -DisableNameChecking
    }
}

# ==================== Welcome Message ====================
Write-Host "Tech Hub .NET Development Environment" -ForegroundColor Cyan
Write-Host "✅ PowerShell profile loaded" -ForegroundColor Green
Write-Host ""
Write-Host "Quick start: Type 'Run' to build, test, and start servers" -ForegroundColor Yellow
Write-Host ""
Write-Host "OPTIONS:" -ForegroundColor Yellow
Write-Host "  -Help          Show this help message" -ForegroundColor Gray
Write-Host "  -Clean         Clean build artifacts before building (use when dependencies change)" -ForegroundColor Gray
Write-Host "  -WithoutTests  Skip all tests, start servers directly (for debugging)" -ForegroundColor Gray
Write-Host "  -StopServers   Stop servers after tests complete (for CI/CD pipelines)" -ForegroundColor Gray
Write-Host "  -TestRerun     Fast test iteration: Only rebuild and run test projects (assumes servers running)" -ForegroundColor Gray
Write-Host "  -Rebuild       Clean rebuild only, then exit" -ForegroundColor Gray
Write-Host "  -TestProject   Scope tests to specific project (e.g., TechHub.Web.Tests, E2E.Tests, powershell)" -ForegroundColor Gray
Write-Host "  -TestName      Scope tests by name pattern (e.g., SectionCard)" -ForegroundColor Gray
Write-Host ""
EOF

chmod +x "$PWSH_PROFILE"

echo "PowerShell profile created at: $PWSH_PROFILE"
echo "Development environment setup complete!"
