# Tech Hub Development Environments

This repository supports multiple development environments through devcontainer configurations. Choose the environment that best fits your development needs.

## Available Environments

### 1. Jekyll Development Environment (Default)

**File:** `.devcontainer/devcontainer.json` → `jekyll-devcontainer.json`

**Use for:**

- Jekyll site development and content creation
- Ruby plugin development
- Markdown content editing and management
- Testing site functionality and performance

**Includes:**

- Ruby with Jekyll, Bundler, and GitHub Pages support
- Node.js with npm for asset building
- PowerShell for automation scripts
- Git and development utilities

### 2. Azure & Bicep Development Environment

**File:** `.devcontainer/devcontainer.json` → `azure-devcontainer.json`

**Use for:**

- Azure infrastructure development and deployment
- Bicep template creation and testing
- Azure resource management and automation
- Cloud architecture development

**Includes:**

- Azure CLI with full Azure toolchain
- Bicep CLI for Infrastructure as Code
- PowerShell with Azure modules (Az, Pester)
- Azure Developer CLI (azd)
- Azure Functions Core Tools
- Static Web Apps CLI
- Sample templates and helper scripts

## Switching Between Environments

### Method 1: VS Code Dev Containers Extension

1. Open the repository in VS Code
2. Press `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac)
3. Type "Dev Containers: Rebuild Container"
4. Choose your preferred configuration when prompted

### Method 2: GitHub Codespaces

1. When creating a new Codespace, you'll be prompted to choose a configuration
2. Select either "Jekyll Development" or "Azure & Bicep Development"
3. The appropriate environment will be set up automatically

### Method 3: Manual Configuration Selection

Edit `.devcontainer/devcontainer.json` and change the `configFilePath`:

```json
{
  "configFilePath": ".devcontainer/jekyll-devcontainer.json"
}
```

or

```json
{
  "configFilePath": ".devcontainer/azure-devcontainer.json"
}
```

## Environment-Specific Getting Started

### Jekyll Development Environment

```bash
# Start Jekyll development server
pwsh ./jekyll-start.ps1

# Run all tests
pwsh ./run-all-tests.ps1

# Process RSS feeds
pwsh ./.github/scripts/complete-rss-workflow.ps1
```

### Azure & Bicep Development Environment

```bash
# Connect to Azure
azlogin
# or using PowerShell
psaz

# Work with Bicep templates
cd infra/samples
bicep build main.bicep

# Deploy infrastructure
pwsh ../scripts/Deploy-BicepTemplate.ps1 -TemplatePath "main.bicep" -ResourceGroupName "rg-test" -Location "westeurope" -WhatIf
```

## Quick Reference

### Jekyll Environment Commands

- `jekyll serve` - Start Jekyll development server
- `bundle install` - Install Ruby dependencies
- `npm install` - Install Node.js dependencies
- `pwsh ./run-all-tests.ps1` - Run complete test suite

### Azure Environment Commands

- `azlogin` - Login to Azure CLI
- `azlist` - List available subscriptions
- `psaz` - Connect to Azure via PowerShell
- `bicepbuild` - Build Bicep templates
- `bicepvalidate` - Validate Bicep syntax

### Useful Aliases (Azure Environment)

- `azrg` - List resource groups
- `azsub` - Show current subscription
- `psazrg` - List resource groups via PowerShell
- `biceplint` - Lint Bicep templates

## Sample Files and Scripts

### Azure Environment Includes

- **Sample Bicep Templates:** `infra/samples/`
  - `storage-account.bicep` - Storage account template
  - `main.bicep` - Main deployment template
  - `main.bicepparam` - Parameters file example

- **Helper Scripts:** `infra/scripts/`
  - `Connect-AzureAccount.ps1` - Azure authentication helper
  - `Deploy-BicepTemplate.ps1` - Bicep deployment script

## Troubleshooting

### Jekyll Environment Issues

- If Jekyll fails to start, try `bundle install` and `pwsh ./jekyll-start.ps1`
- For test failures, run individual test suites to isolate issues
- Check `docs/` folder for comprehensive development guidelines

### Azure Environment Issues

- If Azure CLI login fails, try `az login --use-device-code`
- For PowerShell Azure issues, ensure modules are installed: `pwsh -Command 'Install-Module Az'`
- Bicep build errors usually indicate template syntax issues

## Need Help?

- **Jekyll Development:** See `docs/jekyll-development.md`
- **Azure Development:** Check `infra/scripts/` for examples
- **General Guidelines:** Review `docs/` for comprehensive documentation
- **Testing:** See `docs/testing-guidelines.md` for test execution
