---
layout: post
title: 'Say Goodbye to Personal Access Tokens (PATs) in Azure DevOps: Practical Migration Strategies'
author: Michael Thomsen
canonical_url: https://jessehouwing.net/azure-devops-say-goodbye-to-personal-access-tokens-pats/
viewing_mode: external
feed_name: Jesse Houwing's Blog
feed_url: https://jessehouwing.net/rss/
date: 2025-03-04 12:30:04 +00:00
permalink: /azure/blogs/Say-Goodbye-to-Personal-Access-Tokens-PATs-in-Azure-DevOps-Practical-Migration-Strategies
tags:
- App Registration
- Authentication
- Azure DevOps
- Azure Functions
- Azure Resource Manager
- Integration Testing
- Jest
- Managed Identity
- NodeJS
- PATs
- Personal Access Tokens
- Pipeline Automation
- Renovate
- Security Best Practices
- Service Principal
- Workload Identity Federation
section_names:
- azure
- devops
- security
---
In this post, Michael Thomsen discusses how his team eliminated all Azure DevOps Personal Access Tokens (PATs). He details practical migration steps, leveraging service principals and workload identity federation, making it a must-read for DevOps professionals focused on secure automation.<!--excerpt_end-->

# Say Goodbye to Your Personal Access Tokens

**Author: Michael Thomsen**

> Michael Thomsen, an established author of Azure DevOps Extensions, shares his team's migration journey away from Personal Access Tokens (PATs) in Azure DevOps automation pipelines, inspired by Jesse Houwing's guidance and resources.

## Introduction

As a team extensively using Azure DevOps REST APIs, we at Agile Extensions relied on PATs for various automation scenarios—until recently. Thanks to community help (especially Jesse Houwing's blog post), we've now eradicated PATs, improving automation security and reducing maintenance overhead.

---

### Credit

This transformation was made possible thanks to direct help and resources shared by Jesse Houwing. If you want to deepen your knowledge, check Jesse's [guide on publishing marketplace extensions without PATs](https://jessehouwing.net/publish-azure-devops-extensions-using-workload-identity-oidc/).

---

## The 5 Scenarios Where We Used PATs

We previously used PATs (Personal Access Tokens) for these scenarios:

1. **Publishing Bravo Notes extension** for development, staging, and production (via Azure Pipelines/Marketplace APIs)
2. **Retrieving marketplace event data** (via Azure Functions on a schedule)
3. **Running integration tests** for Bravo Notes components (in Azure Pipelines)
4. **Running end-to-end (e2e) tests** outside Azure DevOps (in Azure Pipelines)
5. **Running integration/e2e tests locally**

Manually managing PATs was tedious and error-prone, requiring regular manual renewal and secret management.

## PAT REST APIs—A Dead End

We first considered automating PAT renewal using the new PAT REST APIs. However, thanks to Jesse's advice, we realized there was a better path—using an Azure Service Principal with workload identity federation.

### The Modern Approach: Workload Identity Federation & Service Principals

This modern authentication scheme uses Azure AD service principals and ARM service connections, providing:

- Automated token acquisition
- Fine-grained permissions
- Reduced secret management overhead

## Migrating Each Scenario

### Scenario 1: Publishing Marketplace Extensions

1. **Create an ARM Service Connection** (Choose "App registration (automatic)" and "Workload identity federation")
2. **Add the app registration/service principal as a user in Azure DevOps**
3. **Extract the Azure DevOps Identity ID** from the Profile API via pipeline task
4. **Add the App registration as a marketplace publisher member**
5. **Update pipeline to use the ARM service connection**
6. **Use the latest marketplace extension tasks (v5)**, e.g.:

   ```yaml
   - task: PublishAzureDevOpsExtension@5
     displayName: 'Publish Extension'
     inputs:
       connectTo: 'AzureRM'
       connectedServiceNameAzureRM: 'marketplace-service-connection'
       fileType: vsix
       vsixFile: '$(Pipeline.Workspace)/vsix/production.vsix'
       updateTasksVersion: false
   ```

Now, no more PATs needed for extension publishing.

---

### Scenario 2: Azure Function App API Calls

- Add the Azure Function app as a user in Azure DevOps
- Use Azure Managed Identity to obtain an access token (Node.js sample):

  ```javascript
  const { ManagedIdentityCredential } = require("@azure/identity");
  async function getMarketplaceAccessToken() {
    const credential = new ManagedIdentityCredential({
      clientId: process.env.AZURE_CLIENT_ID,
    });
    const tokenResponse = await credential.getToken(
      "499b84ac-1321-427f-aa17-267ca6975798/.default",
      { tenantId: process.env.AZURE_TENANT_ID }
    );
    return tokenResponse.token;
  }
  ```

---

### Scenario 3: Integration Tests in Azure Pipelines

- Create a dedicated ARM service connection/application registration for integration testing
- Retrieve the access token via Azure CLI in the pipeline:

  ```yaml
  - task: AzureCLI@2
    displayName: 'Acquire token for integration testing'
    inputs:
      azureSubscription: 'azure-devops-integration-testing-connection'
      scriptType: 'pscore'
      scriptLocation: 'inlineScript'
      useGlobalConfig: true
      inlineScript: |
        $accessToken = az account get-access-token --resource 499b84ac-1321-427f-aa17-267ca6975798 --query "accessToken" --output tsv
        write-host "##vso[task.setsecret]$accessToken"
        write-host "##vso[task.setvariable variable=SECRET_INGETRATION_TESTING_TOKEN;issecret=true]$accessToken"
  ```

- Use the token in subsequent tasks—no PATs needed.

---

### Scenario 4: E2E Tests in Azure Pipelines (Across Orgs)

- Add the application registration/service principal as a user in the target test Azure DevOps organization
- Grant it the required permissions
- Use the same Azure CLI approach to acquire tokens for cross-org API calls

---

### Scenario 5: Local Integration and E2E Tests

- Use Azure Developer CLI for interactive login and token retrieval:
  - Modify test script: `azd auth login && npx jest --config jest.integration.config.js`
  - Retrieve access token programmatically if not running in CI:

    ```javascript
    let token = process.env.SECRET_INGETRATION_TESTING_TOKEN;
    if (!token && !process.env.CI) {
      const credential = new AzureDeveloperCliCredential({
        tenantId: process.env.AZURE_TENANT_ID,
      });
      const tokenResult = await credential.getToken(
        '499b84ac-1321-427f-aa17-267ca6975798/.default',
        { tenantId: process.env.AZURE_TENANT_ID }
      );
      token = tokenResult.token;
    }
    ```

## Bonus: Renovate Pipeline Token

- Renovate can accept an OAuth token in place of a PAT. Use the same Azure CLI workflow to generate a token and supply it to the `RENOVATE_TOKEN` environment variable.
- Azure DevOps now accepts OAuth tokens even with `Basic` auth headers.

## Key Learnings & Wrapping Up

- ARM service connections, OpenID Connect, workload identity federation, service principals, and app registrations are the future of secure Azure DevOps automation
- PATs are simple, but not suitable for robust or scalable automation
- Migration is more accessible than it seems, especially with shared community resources

## Acknowledgements

Special thanks to Jesse Houwing and Joost Voskuil for critical guidance, and to the Azure DevOps Club for community support.

> If you’re facing similar challenges with Azure DevOps or GitHub, consider joining the [Azure DevOps Club](https://www.azuredevops.club/?ref=jessehouwing.net).

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/azure-devops-say-goodbye-to-personal-access-tokens-pats/)
