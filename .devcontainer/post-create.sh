#!/usr/bin/env bash
set -e

# Post-create script for setting up development environment
# This runs automatically when the devcontainer is created
echo "Setting up development environment..."

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

# Install PowerShell modules required for the project
echo "Installing PowerShell modules..."
pwsh -Command 'Install-Module HtmlToMarkdown -AcceptLicense -Force'
pwsh -Command 'Install-Module Pester -Force -SkipPublisherCheck -MinimumVersion "5.0.0" -Scope CurrentUser'

# Install npm-check-updates globally for automatic package updates
echo "Installing npm-check-updates globally..."
npm install -g npm-check-updates

# Update and install Node.js dependencies for JavaScript unit tests (Jest)
echo "Updating JavaScript test dependencies to latest versions..."
cd /workspaces/techhub/spec/javascript || cd $(pwd)/spec/javascript
# Update all packages to latest versions
ncu -u
npm install

# Update and install Node.js dependencies for end-to-end tests (Playwright)
echo "Updating E2E test dependencies to latest versions..."
cd /workspaces/techhub/spec/e2e || cd $(pwd)/spec/e2e
# Update all packages to latest versions
ncu -u
npm install

# Install latest Playwright globally to ensure we have the newest version
echo "Installing latest Playwright globally..."
npm install -g playwright@latest

# Install system libraries required by Playwright browsers first
# This ensures all dependencies are in place before browser installation
echo "Installing Playwright system dependencies first..."
sudo npx playwright install-deps

# Install latest Playwright browsers using the global installation
# This ensures we always get the latest browser versions
echo "Installing latest Playwright browsers..."
npx playwright@latest install --force

# Install Chrome browser for Playwright MCP compatibility
echo "Installing Chrome browser for Playwright MCP..."
npx playwright@latest install --force chrome

echo "Latest Playwright browsers and dependencies installed successfully"
echo "Playwright version: $(npx playwright@latest --version)"

# Add Ruby user gem bin directory to PATH for bundler availability
echo "Setting up Ruby bundler PATH..."
if command -v ruby >/dev/null 2>&1; then
    RUBY_VERSION=$(ruby -e "puts RbConfig::CONFIG['ruby_version']" 2>/dev/null)
    if [ -n "$RUBY_VERSION" ]; then
        USER_GEM_BIN="$HOME/.local/share/gem/ruby/$RUBY_VERSION/bin"
        
        # Check if bundler exists in user gem directory
        if [ -f "$USER_GEM_BIN/bundle" ]; then
            # Add to bashrc for permanent PATH modification
            if ! grep -q "$USER_GEM_BIN" "$HOME/.bashrc" 2>/dev/null; then
                echo "export PATH=\"$USER_GEM_BIN:\$PATH\"" >> "$HOME/.bashrc"
                echo "✅ Added Ruby user gem bin directory to PATH: $USER_GEM_BIN"
            else
                echo "✅ Ruby user gem bin directory already in PATH"
            fi
            
            # Add to current session PATH
            export PATH="$USER_GEM_BIN:$PATH"
        else
            echo "ℹ️  Bundler not found in user gem directory, will be available after gem installation"
        fi
    else
        echo "⚠️  Could not detect Ruby version"
    fi
else
    echo "⚠️  Ruby not found, skipping bundler PATH setup"
fi

echo "Development environment setup complete!"
echo "Checking available tools and browsers:"
echo "Ruby bundle: $(which bundle 2>/dev/null || echo 'Not found in PATH')"
echo "Playwright browsers:"
ls -la /home/vscode/.cache/ms-playwright/ 2>/dev/null || echo "  No Playwright browsers found"
echo "System browsers:"
which chromium-browser chromium google-chrome google-chrome-stable firefox 2>/dev/null || echo "  Limited system browsers available"

# Install Entity Framework Core tools
dotnet tool install --global dotnet-ef

# Update PATH to include .NET tools
echo 'export PATH="$PATH:/home/vscode/.dotnet/tools"' >> /home/vscode/.bashrc

# Install Bicep CLI
echo "Installing Bicep CLI..."
# Download and install the latest Bicep CLI
curl -Lo bicep https://github.com/Azure/bicep/releases/latest/download/bicep-linux-x64
chmod +x ./bicep
sudo mv ./bicep /usr/local/bin/bicep

# Try to install Bicep via Azure CLI, but handle failures gracefully
echo "Attempting to install Bicep via Azure CLI..."
if az bicep install 2>/dev/null; then
    echo "Azure CLI Bicep installation succeeded"
    
    # Check if the Azure CLI Bicep installation is valid
    if [ -f "/home/vscode/.azure/bin/bicep" ]; then
        # Check if the file is not empty and has the right format
        if [ -s "/home/vscode/.azure/bin/bicep" ] && file "/home/vscode/.azure/bin/bicep" | grep -q "ELF.*executable"; then
            echo "Azure CLI Bicep installation is valid"
            chmod +x /home/vscode/.azure/bin/bicep
        else
            echo "Azure CLI Bicep installation is corrupted, removing it..."
            rm -f /home/vscode/.azure/bin/bicep
            echo "Will use system Bicep instead"
        fi
    fi
else
    echo "Azure CLI Bicep installation failed (this is normal if not logged in)"
    echo "Will use system Bicep installation"
fi

# Verify Bicep installation
echo "System Bicep version: $(bicep --version)"
echo "Azure CLI Bicep check: $(az bicep version 2>/dev/null || echo 'Using system Bicep (recommended)')"

# Install Azure Developer CLI (azd)
echo "Installing Azure Developer CLI (azd)..."
curl -fsSL https://aka.ms/install-azd.sh | bash

# Set up Azure CLI completion
echo "Setting up Azure CLI completion..."
echo 'source /etc/bash_completion.d/azure-cli' >> /home/vscode/.bashrc