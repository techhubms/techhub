#!/usr/bin/env bash
set -e

# Post-create script for Azure & .NET development environment
# This runs automatically when the Azure devcontainer is created
echo "Setting up Azure & .NET development environment..."

# Ensure Node.js tools are available system-wide by creating symlinks
# This fixes issues where npx/npm aren't available in sudo context or other shells
echo "Setting up Node.js tools system-wide..."
# Find the current nvm-managed node/npm/npx paths
NODE_PATH=$(which node)
NPM_PATH=$(which npm)
NPX_PATH=$(which npx)

# Create symlinks in /usr/local/bin so they're available in all contexts
sudo ln -sf "$NODE_PATH" /usr/local/bin/node || echo "Node symlink already exists"
sudo ln -sf "$NPM_PATH" /usr/local/bin/npm || echo "NPM symlink already exists"
sudo ln -sf "$NPX_PATH" /usr/local/bin/npx || echo "NPX symlink already exists"

echo "Node.js tools available system-wide:"
echo "  node: $(ls -la /usr/local/bin/node)"
echo "  npm: $(ls -la /usr/local/bin/npm)"
echo "  npx: $(ls -la /usr/local/bin/npx)"

# Install PowerShell modules 
pwsh -Command 'Install-Module Pester -Force -SkipPublisherCheck -MinimumVersion "5.0.0" -Scope CurrentUser'

# Install .NET development tools and global packages
echo "Installing .NET development tools..."
dotnet --version
dotnet --info

# Install Entity Framework Core tools
dotnet tool install --global dotnet-ef

# Install ASP.NET Core code generator
dotnet tool install --global dotnet-aspnet-codegenerator

# Install useful .NET global tools
dotnet tool install --global dotnet-outdated-tool
dotnet tool install --global dotnet-format
dotnet tool install --global dotnet-serve
dotnet tool install --global microsoft.dotnet.httprepl
dotnet tool install --global dotnet-trace
dotnet tool install --global dotnet-dump
dotnet tool install --global dotnet-counters

# Update PATH to include .NET tools
echo 'export PATH="$PATH:/home/vscode/.dotnet/tools"' >> /home/vscode/.bashrc

# Install Bicep CLI
echo "Installing Bicep CLI..."
# Download and install the latest Bicep CLI
curl -Lo bicep https://github.com/Azure/bicep/releases/latest/download/bicep-linux-x64
chmod +x ./bicep
sudo mv ./bicep /usr/local/bin/bicep

# Verify Bicep installation
echo "Bicep version: $(bicep --version)"

# Install Azure Developer CLI (azd)
echo "Installing Azure Developer CLI (azd)..."
curl -fsSL https://aka.ms/install-azd.sh | bash

# Install npm-check-updates globally for automatic package updates
echo "Installing npm-check-updates globally..."
npm install -g npm-check-updates

# Set up Azure CLI completion
echo "Setting up Azure CLI completion..."
echo 'source /etc/bash_completion.d/azure-cli' >> /home/vscode/.bashrc

# Run the original post-create script from the main tech repository
echo "Running main repository post-create script..."
if [ -f "/workspaces/techhub/.devcontainer/post-create-azure.sh" ]; then
    chmod +x /workspaces/techhub/.devcontainer/post-create-azure.sh
    /workspaces/techhub/.devcontainer/post-create-azure.sh
else
    echo "Main post-create script not found, skipping..."
fi