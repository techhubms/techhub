---
external_url: https://devblogs.microsoft.com/devops/azure-developer-cli-from-dev-to-prod-with-one-click/
title: 'Azure Developer CLI: Build Once, Deploy Everywhere from Dev to Prod with One Click'
author: PuiChee (PC) Chan, Kristen Womack
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2025-07-21 16:17:37 +00:00
tags:
- Automation
- Azd
- Azure & Cloud
- Azure Developer CLI
- Azure Developer CLI (azd)
- Bicep
- Build Artifacts
- CI/CD
- Cloud Deployment
- Conditional Deployment
- Environment Variables
- GitHub Actions
- IaC
section_names:
- azure
- coding
- devops
---
Authored by PuiChee (PC) Chan and Kristen Womack, this post guides readers through implementing a 'build once, deploy everywhere' CI/CD pipeline using Azure Developer CLI, conditional Bicep templates, and GitHub Actions for efficient dev-to-prod promotions.<!--excerpt_end-->

# Azure Developer CLI: Build Once, Deploy Everywhere from Dev to Prod with One Click

*By PuiChee (PC) Chan, Kristen Womack*

This article details how to implement a **build once, deploy everywhere** pattern using [Azure Developer CLI (azd)](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/overview). The approach centers on provisioning environment-specific infrastructure and promoting applications from development to production using the same build artifacts. Readers will learn how to:

- Use conditional Bicep deployments
- Inject environment variables
- Preserve packages across environments for consistent artifact promotion
- Automate CI/CD processes using GitHub Actions

## Environment-Specific Infrastructure

When deploying across environments (development, production, etc.), each stage may require different infrastructure settings. The authors provide a comparison table:

| Component           | Development            | Production                        |
|---------------------|------------------------|-------------------------------------|
| **Networking**      | Public access          | VNet integration + Private endpoints |
| **Storage**         | Public (with restrictions) | Private endpoints only             |
| **App Service Plan**| B2 Basic               | S1 Standard                        |
| **Security**        | Managed Identity       | Enhanced network isolation          |

**Managing One Template, Multiple Environments:**
Instead of maintaining multiple infrastructure templates, use a single Bicep file that adapts based on an environment variable. This method prevents infrastructure drift and keeps your deployments consistent.

## Conditional Provisioning with Environment Variables

Use the environment variable `AZURE_ENV_TYPE` to drive decisions in your Bicep files. `azd` passes this as the `envType` parameter. Example:

```bicep
@description('Environment type - determines networking configuration (dev/test/prod)')
@allowed(['dev', 'test', 'prod'])
param envType string = 'dev'
```

### Conditional Deployment Examples

**Network** (Production only):

```bicep
module network './network.bicep' = if (envType == 'prod') {
  name: 'networkDeployment'
  params: { /* ... */ }
}
```

**Storage** (Public or Private based on environment):

```bicep
module storageAccount 'br/public:avm/res/storage/storage-account:0.17.2' = {
  name: 'storageAccount'
  params: {
    name: storageAccountName
    allowSharedKeyAccess: false
    publicNetworkAccess: envType == 'prod' ? 'Disabled' : 'Enabled'
    networkAcls: envType == 'prod' ? {
      defaultAction: 'Deny'
      virtualNetworkRules: []
      bypass: 'AzureServices'
    } : {
      defaultAction: 'Allow'
      virtualNetworkRules: []
      bypass: 'AzureServices'
    }
    // ... other config
  }
}
```

**App Service Plan Sizing:**

```bicep
sku: {
  name: envType == 'prod' ? 'S1' : 'B2'
  tier: envType == 'prod' ? 'Standard' : 'Basic'
}
```

## Enhancing the CI/CD Workflow

The Azure Developer CLI includes a command to bootstrap your CI/CD pipeline:

```sh
azd pipeline config
```

This generates a basic workflow. For the build-once, deploy-everywhere pattern, the authors recommend these enhancements (noting an important update: using GitHub Actions Artifacts instead of local file copies).

### Example Workflow Steps

1. **Package Once:**

   ```yaml
   - name: Package Application
     run: |
       mkdir -p ./dist
       azd package app --output-path ./dist/app-package.zip
       echo "✅ Application packaged successfully"
   ```

2. **Upload Package:**

   ```yaml
   - name: Upload Application Package
     uses: actions/upload-artifact@v4
     with:
       name: app-package
       path: ./dist/app-package.zip
       retention-days: 30
   ```

3. **Deploy to Dev:**

   ```yaml
   - name: Deploy to Development
     run: azd deploy app --from-package ./dist/app-package.zip --no-prompt
   ```

4. **Validation Gate:**

   ```yaml
   - name: Validate Application
     run: |
       echo "🔍 Validating application in development environment..."
       # Add your validation logic here (health checks, tests, etc.)
       sleep 3
       echo "✅ Application validation passed"
   ```

5. **Download Application Package (for Prod):**

   ```yaml
   - name: Download Application Package
     uses: actions/download-artifact@v4
     with:
       name: app-package
       path: ./prod-deploy
   ```

6. **Set Environment Variables for Prod:**

   ```yaml
   - name: Promote to Production
     run: |
       PROD_ENV_NAME="${AZURE_ENV_NAME%-dev}-prod"
       echo "Production environment name: $PROD_ENV_NAME"
       export AZURE_ENV_NAME="$PROD_ENV_NAME"
       export AZURE_ENV_TYPE="prod"
   ```

7. **Deploy to Prod:**

   ```yaml
   - name: Deploy to Production
     run: |
       PACKAGE_PATH="./prod-deploy/app-package.zip"
       if [ -f "$PACKAGE_PATH" ]; then
         echo "🚀 Deploying to production using artifact package: $PACKAGE_PATH"
         azd deploy app --from-package "$PACKAGE_PATH" --no-prompt
         echo "✅ Production deployment completed successfully"
       else
         echo "❌ Package artifact not found - falling back to regular deployment"
         azd deploy --no-prompt
       fi
   ```

## Step-by-Step Setup Guide

1. **Initialize the Project:**

   ```sh
   azd init -t https://github.com/puicchan/azd-dev-prod-appservice-storage
   ```

   (Downloads templates and enhanced GitHub Actions workflows)

2. **Set Up Development Environment:**

   ```sh
   azd up
   ```

   When prompted, use a name like `myproj-dev`. The default `envType` is `dev`, so public access and cost-optimized settings are used.

3. **Set Up Production Environment:**

   ```sh
   azd env new myproj-prod
   azd env set AZURE_ENV_TYPE prod
   azd up
   ```

   Provisions with VNet integration, private endpoints, and enhanced security.

4. **Switch Environments:**

   ```sh
   azd env select myproj-dev
   ```

   Allows further development and testing in the dev environment.

5. **Make Code Changes:**
   - Modify your application code (e.g., `index.html`, `app.py`) to test the promotion workflow.

6. **Configure the CI/CD Pipeline:**

   ```sh
   azd pipeline config
   ```

   This configures the GitHub Actions workflow for dev-to-prod promotion with environment variable handling and artifact reuse.

**For a complete walkthrough and example implementation, see [this repository](https://github.com/puicchan/azd-dev-prod-appservice-storage).**

## Conclusion

Using Azure Developer CLI in this way allows teams to reliably promote the same build from development to production via a streamlined, automated CI/CD process. Conditional Bicep deployment and artifact management ensure consistency, security, and efficiency throughout the pipeline.

Questions or experiences to share? Join the discussion [here](https://github.com/Azure/azure-dev/discussions/5447).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/azure-developer-cli-from-dev-to-prod-with-one-click/)
