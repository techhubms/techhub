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
fi

# ==================== Aspire ====================
echo "Installing Aspire CLI..."
if ! command -v aspire &> /dev/null; then
    curl -fsSL https://aspire.dev/install.sh | bash
    # Add Aspire to PATH for current session
    export PATH="$HOME/.aspire/bin:$PATH"
fi

echo "Installing Aspire project templates..."
dotnet new install Aspire.ProjectTemplates

# ==================== Playwright ====================
echo "Installing Playwright..."
npm install @playwright/test
npx -y playwright install-deps chromium chrome
npx -y playwright install chromium chrome --force

echo "Development environment setup complete!"
