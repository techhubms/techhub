#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Test script for Azure infrastructure deployment (similar to deploy-infrastructure.yml workflow)

.DESCRIPTION
    This script simulates the GitHub Actions workflow for deploying Azure infrastructure.
    It validates prerequisites, tests Bicep templates, and optionally performs actual deployment.

.PARAMETER Mode
    Deployment mode: 'validate', 'whatif', or 'deploy'
    - validate: Only validates the Bicep template and parameters
    - whatif: Shows what would be deployed without actually deploying
    - deploy: Performs actual deployment (use with caution!)

.PARAMETER EnvironmentFile
    Path to environment parameters file (default: ./infra/parameters.prod.json)

.PARAMETER SubscriptionId
    Azure subscription ID (overrides value in parameters file)

.PARAMETER ResourceGroupName
    Target resource group name (optional - will be generated if not specified)

.PARAMETER Location
    Azure region for deployment (default: westeurope)

.PARAMETER SkipLogin
    Skip Azure login (assumes already logged in)

.EXAMPLE
    # Validate the infrastructure template
    ./test-infrastructure-deployment.ps1 -Mode validate

.EXAMPLE
    # See what would be deployed
    ./test-infrastructure-deployment.ps1 -Mode whatif

.EXAMPLE
    # Deploy to a specific subscription
    ./test-infrastructure-deployment.ps1 -Mode deploy -SubscriptionId "your-sub-id"

.EXAMPLE
    # Validate with custom parameters file
    ./test-infrastructure-deployment.ps1 -Mode validate -EnvironmentFile "./infra/parameters.test.json"
#>

param(
    [Parameter(Mandatory = $false)]
    [ValidateSet('validate', 'whatif', 'deploy')]
    [string]$Mode = 'validate',
    
    [Parameter(Mandatory = $false)]
    [string]$EnvironmentFile = './infra/parameters.prod.json',
    
    [Parameter(Mandatory = $false)]
    [string]$SubscriptionId,
    
    [Parameter(Mandatory = $false)]
    [string]$ResourceGroupName,
    
    [Parameter(Mandatory = $false)]
    [string]$Location = 'westeurope',
    
    [Parameter(Mandatory = $false)]
    [switch]$SkipLogin,
    
    [Parameter(Mandatory = $false)]
    [switch]$VerboseOutput
)

# Set error action preference
$ErrorActionPreference = 'Stop'

# Enable verbose output if requested
if ($VerboseOutput) {
    $VerbosePreference = 'Continue'
}

function Write-Header {
    param([string]$Message)
    Write-Host ""
    Write-Host "===========================================" -ForegroundColor Cyan
    Write-Host $Message -ForegroundColor Cyan
    Write-Host "===========================================" -ForegroundColor Cyan
}

function Write-Step {
    param([string]$Message)
    Write-Host ""
    Write-Host "▶ $Message" -ForegroundColor Green
}

function Write-Error-Custom {
    param([string]$Message)
    Write-Host ""
    Write-Host "❌ ERROR: $Message" -ForegroundColor Red
}

function Write-Warning-Custom {
    param([string]$Message)
    Write-Host "⚠️  WARNING: $Message" -ForegroundColor Yellow
}

function Write-Success {
    param([string]$Message)
    Write-Host "✅ $Message" -ForegroundColor Green
}

function Test-Prerequisites {
    Write-Step "Checking prerequisites..."
    
    # Check if Azure CLI is installed
    try {
        $azVersion = az version --query '"azure-cli"' -o tsv 2>$null
        if ($azVersion) {
            Write-Success "Azure CLI version $azVersion found"
        } else {
            throw "Azure CLI not found"
        }
    } catch {
        Write-Error-Custom "Azure CLI is not installed or not in PATH. Please install Azure CLI first."
        Write-Host "Install from: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli"
        exit 1
    }
    
    # Check if Bicep is available
    try {
        # First try to use system Bicep directly
        $systemBicepVersion = $null
        try {
            $systemBicepVersion = & bicep --version 2>$null
            if ($systemBicepVersion) {
                Write-Success "System Bicep found: $systemBicepVersion"
            }
        } catch {
            # System Bicep not available
        }
        
        # Try Azure CLI Bicep
        $azureBicepVersion = $null
        try {
            $azureBicepVersion = az bicep version --query 'bicepVersion' -o tsv 2>$null
            if ($azureBicepVersion) {
                Write-Success "Azure CLI Bicep version $azureBicepVersion found"
            }
        } catch {
            # Azure CLI Bicep not available or corrupted
        }
        
        # If neither work, try to install Azure CLI Bicep
        if (-not $systemBicepVersion -and -not $azureBicepVersion) {
            Write-Step "Installing Bicep via Azure CLI..."
            try {
                az bicep install
                $azureBicepVersion = az bicep version --query 'bicepVersion' -o tsv 2>$null
                if ($azureBicepVersion) {
                    Write-Success "Azure CLI Bicep installed successfully: $azureBicepVersion"
                } else {
                    throw "Bicep installation failed"
                }
            } catch {
                Write-Warning-Custom "Azure CLI Bicep installation failed. Please ensure Bicep is available."
                if (-not $systemBicepVersion) {
                    Write-Error-Custom "No working Bicep installation found. Please install Bicep manually."
                    Write-Host "System Bicep: Install from https://github.com/Azure/bicep/releases"
                    Write-Host "Azure CLI Bicep: Run 'az bicep install'"
                    exit 1
                }
            }
        }
        
        # Final verification
        if ($systemBicepVersion -or $azureBicepVersion) {
            Write-Success "Bicep is available for template validation"
        } else {
            throw "No working Bicep installation found"
        }
    } catch {
        Write-Error-Custom "Could not verify Bicep installation: $($_.Exception.Message)"
        exit 1
    }
    
    # Check if required files exist
    $requiredFiles = @(
        './infra/main.bicep',
        $EnvironmentFile
    )
    
    foreach ($file in $requiredFiles) {
        if (Test-Path $file) {
            Write-Success "Found required file: $file"
        } else {
            Write-Error-Custom "Required file not found: $file"
            exit 1
        }
    }
}

function Get-ParametersFromFile {
    param([string]$FilePath)
    
    Write-Step "Reading parameters from $FilePath..."
    
    try {
        $parametersContent = Get-Content $FilePath -Raw | ConvertFrom-Json
        $parameters = @{}
        
        foreach ($param in $parametersContent.parameters.PSObject.Properties) {
            $parameters[$param.Name] = $param.Value.value
        }
        
        # Handle placeholder passwords
        if ($parameters.ContainsKey('sqlAdminPassword') -and 
            ($parameters['sqlAdminPassword'] -eq 'REPLACE_WITH_SECURE_PASSWORD' -or 
             $parameters['sqlAdminPassword'] -eq '' -or 
             $null -eq $parameters['sqlAdminPassword'])) {
            
            Write-Warning-Custom "SQL Admin Password is not set. Generating a secure password for testing..."
            $parameters['sqlAdminPassword'] = New-SecurePassword
            Write-Success "Generated secure password for SQL Admin"
        }
        
        Write-Success "Successfully loaded $($parameters.Count) parameters"
        
        if ($VerboseOutput) {
            Write-Host "Parameters loaded:" -ForegroundColor Gray
            $parameters.GetEnumerator() | ForEach-Object {
                if ($_.Key -like "*password*" -or $_.Key -like "*secret*") {
                    Write-Host "  $($_.Key): [REDACTED]" -ForegroundColor Gray
                } else {
                    Write-Host "  $($_.Key): $($_.Value)" -ForegroundColor Gray
                }
            }
        }
        
        return $parameters
    } catch {
        Write-Error-Custom "Failed to read parameters file: $($_.Exception.Message)"
        exit 1
    }
}

function New-SecurePassword {
    # Generate a secure password that meets Azure SQL requirements
    # - At least 8 characters
    # - Contains uppercase, lowercase, numbers, and special characters
    $upperCase = "ABCDEFGHKLMNOPRSTUVWXYZ".ToCharArray()
    $lowerCase = "abcdefghiklmnoprstuvwxyz".ToCharArray()
    $numbers = "0123456789".ToCharArray()
    $specialChars = "!@#$%^&*".ToCharArray()
    
    $password = ""
    $random = New-Object System.Random
    
    # Ensure at least one character from each required set
    $password += ($upperCase | Get-Random)
    $password += ($lowerCase | Get-Random)
    $password += ($numbers | Get-Random)
    $password += ($specialChars | Get-Random)
    
    # Fill remaining characters (total length: 16)
    $allChars = $upperCase + $lowerCase + $numbers + $specialChars
    for ($i = 0; $i -lt 12; $i++) {
        $password += ($allChars | Get-Random)
    }
    
    # Shuffle the password
    $passwordArray = $password.ToCharArray()
    for ($i = $passwordArray.Length - 1; $i -gt 0; $i--) {
        $j = $random.Next($i + 1)
        $temp = $passwordArray[$i]
        $passwordArray[$i] = $passwordArray[$j]
        $passwordArray[$j] = $temp
    }
    
    return -join $passwordArray
}

function Test-AzureLogin {
    if ($SkipLogin) {
        Write-Step "Skipping Azure login as requested..."
        return
    }
    
    Write-Step "Checking Azure login status..."
    
    try {
        $account = az account show 2>$null | ConvertFrom-Json
        if ($account) {
            Write-Success "Already logged in as: $($account.user.name)"
            Write-Host "  Subscription: $($account.name) ($($account.id))" -ForegroundColor Gray
        } else {
            throw "Not logged in"
        }
    } catch {
        Write-Step "Not logged in to Azure. Please log in..."
        Write-Host "Opening browser for Azure login..." -ForegroundColor Yellow
        az login
        
        $account = az account show | ConvertFrom-Json
        Write-Success "Successfully logged in as: $($account.user.name)"
    }
}

function Set-AzureSubscription {
    param([string]$SubId)
    
    if ($SubId) {
        Write-Step "Setting Azure subscription to: $SubId"
        try {
            az account set --subscription $SubId
            $currentSub = az account show --query 'id' -o tsv
            if ($currentSub -eq $SubId) {
                Write-Success "Successfully set subscription to: $SubId"
            } else {
                throw "Failed to set subscription"
            }
        } catch {
            Write-Error-Custom "Failed to set subscription: $($_.Exception.Message)"
            exit 1
        }
    } else {
        $currentSub = az account show --query '[name,id]' -o tsv
        Write-Host "Using current subscription: $currentSub" -ForegroundColor Gray
    }
}

function Build-DeploymentParameters {
    param(
        [hashtable]$Parameters,
        [string]$RepoUrl = "https://github.com/techhubms/techhub",
        [string]$RepoBranch = "main"
    )
    
    Write-Step "Building deployment parameters..."
    
    # For secure parameters like passwords, we need to create a temporary parameters file
    # instead of passing them via command line
    $tempParameters = @{
        '$schema' = "https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#"
        'contentVersion' = "1.0.0.0"
        'parameters' = @{}
    }
    
    # Add all parameters from the original file
    foreach ($param in $Parameters.GetEnumerator()) {
        if ($param.Key -eq "subscriptionId") {
            continue # This is handled separately
        }
        
        $tempParameters.parameters[$param.Key] = @{
            'value' = $param.Value
        }
    }
    
    # Add additional parameters that come from GitHub secrets/environment
    $tempParameters.parameters['repositoryUrl'] = @{ 'value' = $RepoUrl }
    $tempParameters.parameters['repositoryBranch'] = @{ 'value' = $RepoBranch }
    $tempParameters.parameters['githubToken'] = @{ 'value' = 'placeholder-token' }
    
    # Create temporary parameters file
    $tempDir = if ($env:TEMP) { $env:TEMP } elseif ($env:TMPDIR) { $env:TMPDIR } else { "/tmp" }
    $tempFile = Join-Path $tempDir "temp-deployment-params-$(Get-Date -Format 'yyyyMMddHHmmss').json"
    $jsonContent = $tempParameters | ConvertTo-Json -Depth 10
    $jsonContent | Set-Content $tempFile -Encoding UTF8
    
    if ($VerboseOutput) {
        Write-Host "Parameter file content:" -ForegroundColor Gray
        Write-Host $jsonContent -ForegroundColor DarkGray
    }
    
    Write-Verbose "Created temporary parameters file: $tempFile"
    return $tempFile
}

function Test-BicepTemplate {
    param(
        [string]$TemplatePath,
        [string]$ParameterFile,
        [string]$DeploymentLocation
    )
    
    Write-Step "Validating Bicep template..."
    
    try {
        $validationResult = az deployment sub validate `
            --location $DeploymentLocation `
            --template-file $TemplatePath `
            --parameters $ParameterFile `
            --output table
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Bicep template validation passed"
            if ($VerboseOutput -and $validationResult) {
                Write-Host "Validation details:" -ForegroundColor Gray
                Write-Host $validationResult -ForegroundColor Gray
            }
        } else {
            throw "Validation failed with exit code $LASTEXITCODE"
        }
    } catch {
        Write-Error-Custom "Bicep template validation failed: $($_.Exception.Message)"
        exit 1
    } finally {
        # Clean up temporary parameters file
        if ($ParameterFile -and (Test-Path $ParameterFile) -and $ParameterFile.Contains("temp-deployment-params")) {
            Remove-Item $ParameterFile -Force -ErrorAction SilentlyContinue
            Write-Verbose "Cleaned up temporary parameters file"
        }
    }
}

function Show-WhatIf {
    param(
        [string]$TemplatePath,
        [string]$ParameterFile,
        [string]$DeploymentLocation
    )
    
    Write-Step "Running what-if analysis..."
    
    try {
        Write-Host "This will show what resources would be created/modified/deleted:" -ForegroundColor Yellow
        Write-Host ""
        
        az deployment sub what-if `
            --location $DeploymentLocation `
            --template-file $TemplatePath `
            --parameters $ParameterFile
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "What-if analysis completed"
        } else {
            Write-Warning-Custom "What-if analysis completed with warnings"
        }
    } catch {
        Write-Error-Custom "What-if analysis failed: $($_.Exception.Message)"
        exit 1
    } finally {
        # Clean up temporary parameters file
        if ($ParameterFile -and (Test-Path $ParameterFile) -and $ParameterFile.Contains("temp-deployment-params")) {
            Remove-Item $ParameterFile -Force -ErrorAction SilentlyContinue
            Write-Verbose "Cleaned up temporary parameters file"
        }
    }
}

function Deploy-Infrastructure {
    param(
        [string]$TemplatePath,
        [string]$ParameterFile,
        [string]$DeploymentLocation
    )
    
    Write-Step "Deploying infrastructure..."
    Write-Host ""
    Write-Host "⚠️  WARNING: This will deploy actual Azure resources!" -ForegroundColor Red
    Write-Host "⚠️  This may incur costs in your Azure subscription!" -ForegroundColor Red
    Write-Host ""
    
    $confirmation = Read-Host "Are you sure you want to proceed? (yes/no)"
    if ($confirmation -ne "yes") {
        Write-Host "Deployment cancelled by user." -ForegroundColor Yellow
        exit 0
    }
    
    try {
        $deploymentName = "techhub-infrastructure-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
        Write-Host "Deployment name: $deploymentName" -ForegroundColor Gray
        
        Write-Host "Starting deployment..." -ForegroundColor Yellow
        $deploymentResult = az deployment sub create `
            --name $deploymentName `
            --location $DeploymentLocation `
            --template-file $TemplatePath `
            --parameters $ParameterFile `
            --output table
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Infrastructure deployment completed successfully!"
            Write-Host ""
            Write-Host "Deployment results:" -ForegroundColor Green
            if ($deploymentResult) {
                Write-Host $deploymentResult -ForegroundColor Gray
            }
            
            # Get deployment outputs
            Write-Step "Getting deployment outputs..."
            $outputs = az deployment sub show --name $deploymentName --query 'properties.outputs' --output json
            if ($outputs -and $outputs -ne "null") {
                Write-Host "Deployment outputs:" -ForegroundColor Green
                $outputs | ConvertFrom-Json | ConvertTo-Json -Depth 3 | Write-Host -ForegroundColor Gray
            }
            
        } else {
            throw "Deployment failed with exit code $LASTEXITCODE"
        }
    } catch {
        Write-Error-Custom "Infrastructure deployment failed: $($_.Exception.Message)"
        exit 1
    } finally {
        # Clean up temporary parameters file
        if ($ParameterFile -and (Test-Path $ParameterFile) -and $ParameterFile.Contains("temp-deployment-params")) {
            Remove-Item $ParameterFile -Force -ErrorAction SilentlyContinue
            Write-Verbose "Cleaned up temporary parameters file"
        }
    }
}

function Show-Summary {
    param(
        [string]$Mode,
        [hashtable]$Parameters
    )
    
    Write-Header "Summary"
    Write-Host "Mode: $Mode" -ForegroundColor Cyan
    Write-Host "Location: $Location" -ForegroundColor Cyan
    Write-Host "Parameters file: $EnvironmentFile" -ForegroundColor Cyan
    
    if ($Parameters.ContainsKey('baseName')) {
        Write-Host "Base name: $($Parameters['baseName'])" -ForegroundColor Cyan
    }
    
    if ($Mode -eq 'deploy') {
        Write-Host ""
        Write-Host "Next steps after deployment:" -ForegroundColor Yellow
        Write-Host "1. Check the Azure portal for deployed resources" -ForegroundColor Gray
        Write-Host "2. Verify the resource group was created successfully" -ForegroundColor Gray
        Write-Host "3. Test the deployed application" -ForegroundColor Gray
        Write-Host "4. Configure any additional settings required" -ForegroundColor Gray
    }
}

# Main execution
try {
    Write-Header "Tech Hub Infrastructure Deployment Test Script"
    Write-Host "Mode: $Mode | Location: $Location | Parameters: $EnvironmentFile" -ForegroundColor Gray
    
    # Step 1: Check prerequisites
    Test-Prerequisites
    
    # Step 2: Load parameters
    $parameters = Get-ParametersFromFile -FilePath $EnvironmentFile
    
    # Step 3: Handle subscription ID
    $targetSubscriptionId = if ($SubscriptionId) { $SubscriptionId } else { $parameters['subscriptionId'] }
    
    # Step 4: Azure login and subscription setup
    Test-AzureLogin
    Set-AzureSubscription -SubId $targetSubscriptionId
    
    # Step 5: Build deployment parameters
    $parameterFile = Build-DeploymentParameters -Parameters $parameters
    
    # Step 6: Execute based on mode
    switch ($Mode) {
        'validate' {
            Test-BicepTemplate -TemplatePath './infra/main.bicep' -ParameterFile $parameterFile -DeploymentLocation $Location
            Write-Success "Validation completed successfully!"
        }
        'whatif' {
            Test-BicepTemplate -TemplatePath './infra/main.bicep' -ParameterFile $parameterFile -DeploymentLocation $Location
            Show-WhatIf -TemplatePath './infra/main.bicep' -ParameterFile $parameterFile -DeploymentLocation $Location
        }
        'deploy' {
            Test-BicepTemplate -TemplatePath './infra/main.bicep' -ParameterFile $parameterFile -DeploymentLocation $Location
            Show-WhatIf -TemplatePath './infra/main.bicep' -ParameterFile $parameterFile -DeploymentLocation $Location
            Deploy-Infrastructure -TemplatePath './infra/main.bicep' -ParameterFile $parameterFile -DeploymentLocation $Location
        }
    }
    
    # Step 7: Show summary
    Show-Summary -Mode $Mode -Parameters $parameters
    
} catch {
    Write-Error-Custom "Script execution failed: $($_.Exception.Message)"
    if ($VerboseOutput) {
        Write-Host "Stack trace:" -ForegroundColor Red
        Write-Host $_.ScriptStackTrace -ForegroundColor Red
    }
    exit 1
}
