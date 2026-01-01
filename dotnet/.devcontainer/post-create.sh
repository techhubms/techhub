#!/bin/bash
set -e

echo "Setting up Tech Hub .NET development environment..."

# Update package lists
echo "Updating package lists..."
sudo apt-get update

# ==================== 1. PowerShell ====================
echo "Installing PowerShell..."
wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O /tmp/packages-microsoft-prod.deb
sudo dpkg -i /tmp/packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y powershell
rm /tmp/packages-microsoft-prod.deb

# ==================== 2. .NET SDK & Dev Certificates ====================
echo "Installing .NET 10 SDK..."
sudo apt-get install -y dotnet-sdk-10.0

echo "Setting up development certificates..."
if ! dotnet dev-certs https --check; then
    dotnet dev-certs https
fi

# ==================== 3. Other .NET Related Tools ====================
echo "Installing .NET global tools..."
if ! dotnet tool list -g | grep -q dotnet-ef; then
    dotnet tool install --global dotnet-ef
fi

echo "Installing Aspire CLI..."
if ! command -v aspire &> /dev/null; then
    curl -fsSL https://aspire.dev/install.sh | bash
    # Add Aspire to PATH for current session
    export PATH="$HOME/.aspire/bin:$PATH"
fi

echo "Installing Aspire project templates..."
dotnet new install Aspire.ProjectTemplates

# ==================== 4. Azure Tools ====================
echo "Installing Azure CLI..."
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

echo "Installing Azure CLI Bicep..."
az bicep install

echo "Installing Azure Developer CLI..."
curl -fsSL https://aka.ms/install-azd.sh | bash

# ==================== 5. Other Tools ====================

echo "Installing GitHub CLI..."
sudo apt-get install -y gh

echo "Installing Node.js..."
curl -fsSL https://deb.nodesource.com/setup_current.x | sudo -E bash -
sudo apt-get install -y nodejs

echo "Updating npm..."
sudo npm install -g npm@latest

echo "Installing Playwright..."
npm install @playwright/test
sudo npx -y playwright install-deps
npx -y playwright@latest install chromium chrome --force

echo "Development environment setup complete!"
