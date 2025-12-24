#!/usr/bin/env bash
set -e

# Post-create script for setting up development environment
# This runs automatically when the devcontainer is created
echo "Setting up development environment..."

# Ensure Node.js tools are available system-wide by creating symlinks
# This fixes issues where npx/npm aren't available in sudo context or other shells
echo "Setting up Node.js tools system-wide..."

# Find the current nvm-managed node/npm/npx paths
# Use /usr/local/share/nvm/current/bin as fallback if which doesn't work
if [ -d "/usr/local/share/nvm/current/bin" ]; then
    NODE_PATH="/usr/local/share/nvm/current/bin/node"
    NPM_PATH="/usr/local/share/nvm/current/bin/npm"
    NPX_PATH="/usr/local/share/nvm/current/bin/npx"
else
    NODE_PATH=$(which node 2>/dev/null || echo "")
    NPM_PATH=$(which npm 2>/dev/null || echo "")
    NPX_PATH=$(which npx 2>/dev/null || echo "")
fi

# Only create symlinks if we found the paths
if [ -n "$NODE_PATH" ] && [ -f "$NODE_PATH" ]; then
    sudo ln -sf "$NODE_PATH" /usr/local/bin/node || echo "Node symlink already exists"
    sudo ln -sf "$NPM_PATH" /usr/local/bin/npm || echo "NPM symlink already exists"
    sudo ln -sf "$NPX_PATH" /usr/local/bin/npx || echo "NPX symlink already exists"
else
    echo "Warning: Node.js not found, skipping symlink creation"
fi

echo "Node.js tools available system-wide:"
echo "  node: $(ls -la /usr/local/bin/node)"
echo "  npm: $(ls -la /usr/local/bin/npm)"
echo "  npx: $(ls -la /usr/local/bin/npx)"

# Update npm to the latest version
echo "Updating npm to latest version..."
npm install -g npm@latest
echo "npm version: $(npm --version)"

# Install PowerShell modules required for the project
echo "Installing PowerShell modules..."
pwsh -Command 'Install-Module HtmlToMarkdown -AcceptLicense -Force'
pwsh -Command 'Install-Module Pester -Force -SkipPublisherCheck -MinimumVersion "5.0.0" -Scope CurrentUser'

# Install npm-check-updates globally for automatic package updates
echo "Installing npm-check-updates globally..."
sudo npm install -g npm-check-updates

# Install ESLint globally for JavaScript/TypeScript linting
echo "Installing ESLint globally..."
sudo npm install -g eslint
echo "ESLint version: $(eslint --version)"

# Update and install Node.js dependencies for JavaScript unit tests (Jest)
echo "Updating JavaScript test dependencies to latest versions..."
cd /workspaces/techhub/spec/javascript || cd $(pwd)/spec/javascript
# Clean node_modules to avoid permission issues
rm -rf node_modules
# Update all packages to latest versions
npx npm-check-updates -u
npm install

# Update and install Node.js dependencies for end-to-end tests (Playwright)
echo "Updating E2E test dependencies to latest versions..."
cd /workspaces/techhub/spec/e2e || cd $(pwd)/spec/e2e
# Clean node_modules to avoid permission issues
rm -rf node_modules
# Update all packages to latest versions
npx npm-check-updates -u
npm install

# Install latest Playwright globally to ensure we have the newest version
echo "Installing latest Playwright globally..."
sudo npm install -g playwright@latest

# Install system libraries required by Playwright browsers first
# This ensures all dependencies are in place before browser installation
echo "Installing Playwright system dependencies first..."
sudo npx -y playwright install-deps

# Install only chromium browser for Playwright (as configured in playwright.config.js)
# This ensures browsers are installed in ~/.cache/ms-playwright where tests can find them
echo "Installing Playwright chromium browser for current user..."
npx -y playwright@latest install chromium --force

echo "Latest Playwright browsers and dependencies installed successfully"
echo "Playwright version: $(npx playwright@latest --version)"

# Ruby is provided by the Jekyll base image - install gems from Gemfile
echo "Verifying Ruby installation..."
echo "Ruby version: $(ruby --version)"
echo "Bundler version: $(bundle --version)"
echo "Jekyll version: $(jekyll --version)"

# Change back to workspace root and install Ruby gems
echo "Installing Ruby gems from Gemfile..."
cd /workspaces/techhub || cd $(pwd)
bundle install

# Verify Bicep and Azure Developer CLI installations (installed via devcontainer features)
echo "Verifying Azure CLI tools..."
echo "Bicep version: $(az bicep version 2>/dev/null || bicep --version 2>/dev/null || echo 'Not installed')"
echo "Azure Developer CLI version: $(azd version 2>/dev/null || echo 'Not installed')"

# Set up Azure CLI completion
echo "Setting up Azure CLI completion..."
if [ -f /etc/bash_completion.d/azure-cli ]; then
    echo 'source /etc/bash_completion.d/azure-cli' >> /home/vscode/.bashrc
fi

echo ""
echo "=================================="
echo "Development Environment Setup Complete!"
echo "=================================="

exit 0