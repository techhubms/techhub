#!/bin/bash
set -e

echo "Setting up Tech Hub .NET development environment..."

# Remove Yarn repository (not used in this project, has expired GPG key)
echo "Cleaning up unused package repositories..."
sudo rm -f /etc/apt/sources.list.d/yarn.list

# Install NSS tools for certificate management, ImageMagick for image processing, and libjxl for JPEG XL encoding
# Also install exiftool (libimage-exiftool-perl), webp tools, and file utility for Normalize-Images.ps1 script
# sqlite3 is needed for database inspection and queries
# postgresql-client for connecting to PostgreSQL databases (docker-compose or Azure)
echo "Installing system dependencies..."
sudo apt-get update
sudo apt-get install -y libnss3-tools imagemagick libjxl-tools libimage-exiftool-perl webp file sqlite3 postgresql-client

# ==================== .NET Dev Certificates ====================
sudo dotnet workload update

echo "🔐 Setting up .NET HTTPS development certificate..."
# Clean any existing certificate directories with wrong ownership
sudo rm -rf /home/vscode/.dotnet/corefx/cryptography/x509stores
sudo rm -rf /home/vscode/.aspnet/https

# Set up certificate store directory with correct ownership
mkdir -p /home/vscode/.dotnet/corefx/cryptography/x509stores/my
mkdir -p /home/vscode/.aspnet/https

# Clean old certificates (ignore errors if none exist)
dotnet dev-certs https --clean > /dev/null 2>&1 || true

# Generate new certificate (this creates it in the vscode user store)
dotnet dev-certs https

# Export to PEM format for system-level trust
sudo mkdir -p /usr/local/share/ca-certificates/aspnet
sudo -E dotnet dev-certs https -ep /usr/local/share/ca-certificates/aspnet/https.crt --format PEM
sudo update-ca-certificates

# Export to PFX for Docker containers (with password)
echo "Exporting certificate for Docker containers..."
dotnet dev-certs https -ep /home/vscode/.aspnet/https/aspnetapp.pfx -p devpass

# Make certificate readable by Docker containers (non-root user needs read access)
chmod 644 /home/vscode/.aspnet/https/aspnetapp.pfx

echo "✅ HTTPS certificate generated and trusted"

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
# Set TECHHUB_TMP for temporary files (logs, uploads, etc.)
# Points to workspace .tmp directory for local dev (Docker overrides this to /tmp/techhub)
$env:TECHHUB_TMP = "/workspaces/techhub/.tmp"

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
Write-Host "Quick start:" -ForegroundColor Yellow
Write-Host "- Type 'Run' to build, test, and start servers" -ForegroundColor Yellow
Write-Host "- Type 'Run -Docker' to run full stack in Docker containers" -ForegroundColor Yellow
Write-Host "- Type 'Run -Help' for detailed options" -ForegroundColor Yellow
Write-Host ""
Write-Host "Servers run in background - terminal is always free to use" -ForegroundColor Yellow
Write-Host ""
EOF

chmod +x "$PWSH_PROFILE"

echo "PowerShell profile created at: $PWSH_PROFILE"
echo "Development environment setup complete!"
