---
layout: post
title: How to Streamline GitHub API Calls in Azure Pipelines Using a Custom DevOps Extension
author: Tiago Pascoal
canonical_url: https://github.blog/enterprise-software/ci-cd/how-to-streamline-github-api-calls-in-azure-pipelines/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-07-24 16:00:00 +00:00
permalink: /azure/news/How-to-Streamline-GitHub-API-Calls-in-Azure-Pipelines-Using-a-Custom-DevOps-Extension
tags:
- Automation
- Azure
- Azure DevOps
- Azure DevOps Extension
- Azure Pipelines
- CI/CD
- Company
- Custom Task
- DevOps
- Enterprise Software
- GitHub API
- GitHub Apps
- Integrations
- JWT Authentication
- News
- Security
- Service Connection
- Token Management
- YAML Pipeline
section_names:
- devops
- azure
- security
---
Written by Tiago Pascoal, this post details how to build a custom Azure DevOps extension for secure, efficient, and centralized authentication with GitHub APIs from Azure Pipelines, streamlining automation and improving security controls.<!--excerpt_end-->

# How to Streamline GitHub API Calls in Azure Pipelines Using a Custom DevOps Extension

*By Tiago Pascoal*

Azure Pipelines, part of Azure DevOps, is a cloud-based continuous integration and delivery (CI/CD) service that allows automatic code building, testing, and deployment, with integrated support for GitHub repositories. By integrating with GitHub development flows, Azure Pipelines can be triggered by repository events and can report results back to GitHub using status checks.

Often, more advanced workflows require direct interaction with the GitHub API. This is necessary for tasks such as:

- Setting status checks on commits or pull requests.
- Posting comments on pull requests.
- Updating documentation or configuration files.
- Managing GitHub Issues.
- Integrating with GitHub Advanced Security.
- Managing releases and assets.
- Tracking deployments.
- Triggering GitHub Actions workflows as part of larger CI/CD scenarios.

This article outlines a standardized approach for securely and efficiently making GitHub API calls from Azure Pipelines via a custom Azure DevOps extension, centralizing credential management and simplifying authentication, particularly when using GitHub Apps.

## **GitHub API: REST vs. GraphQL**

- **REST API**: Uses standard HTTP endpoints and supports operations like managing repositories and pull requests. Authentication is typically via Personal Access Tokens, GitHub Apps, or OAuth tokens.
- **GraphQL API**: Allows flexible, efficient queries that can retrieve complex data sets in a single request, reducing overhead and improving performance.

Both can be used as the foundation for automation and integration.

## **Authentication Methods for GitHub API**

| Authentication Type      | Pros                                                | Cons                                                                                |
|-------------------------|-----------------------------------------------------|-------------------------------------------------------------------------------------|
| Personal Access Tokens  | Easy to set up, good for personal automation        | Tied to user accounts, broad permissions, requires manual rotation                  |
| OAuth Tokens            | Grants specific app permissions, refreshable        | Requires more setup, tied to user accounts, token management complexity             |
| GitHub Apps             | Fine-grained, installation-based, scalable, secure  | Complex setup, JWT implementation required, private key management responsibility   |

- **GitHub Apps** are recommended for CI/CD automation, providing short-lived, least-privilege tokens and detailed access controls.

## **Registering and Installing a GitHub App**

1. **Register the App** at the desired scope (enterprise, org, account) and set permissions.
2. **Install the App** on the desired orgs/repositories, selecting all or specific repos. Store the generated private key securely.

**Note:** Only 100 GitHub Apps can be registered per org/account.

## **GitHub App Authentication Flow**

1. The app signs a JWT with its private key to authenticate as itself.
2. The JWT is used to request an installation token, scoped to an org or repo.
3. The installation token, valid for one hour, is used for API access.

[Pictorial sequence diagram shows client obtaining JWT, installing the app, getting installation ID, requesting token, and then making API calls.](https://github.blog/wp-content/uploads/2025/07/image1.png)

### Generation Methods for Installation Tokens

- Command-line tools (e.g., [gh-token](https://github.com/Link-/gh-token))
- Custom scripts (bash, PowerShell)
- Azure Pipeline tasks (via marketplace or custom extensions)

## **Azure DevOps Extension for GitHub App Authentication**

### Benefits of Using Service Connections

- Securely store and manage confidential data (e.g., app private key).
- Centralized access control and credential sharing across pipelines.
- Enforce security and operational consistency.

[Sample Code Repository on GitHub](https://github.com/tspascoal/azure-pipelines-create-github-app-token-task)

### **Creating the Extension**

The extension package includes:

- **Custom service connection type**: securely store GitHub App settings, private keys, and API URLs.
- **Custom task**: generates installation tokens using stored credentials and makes them available to pipelines.

**Key Steps:**

- Define service connection schema and contribution in the manifest.
- Create a task implementation in TypeScript or PowerShell.
- Package extension and publish privately or via the marketplace.

**Example manifest snippet:**

```json
"contributions": [
  {
    "id": "github-app-service-endpoint-type",
    "description": "GitHub App Service Connection",
    "type": "ms.vss-endpoint.service-endpoint-type",
    "targets": ["ms.vss-endpoint.endpoint-types"],
    "properties": { ... }
  }
]
```

*See the [reference implementation](https://github.com/tspascoal/azure-pipelines-create-github-app-token-task/blob/1c0778fb64e344fcf237c06894795ce8547abf7c/vss-extension.json#L44) for full details.*

### **Using the Custom Task in Azure Pipelines**

The custom task can generate an installation token using a configured service connection and export it as a pipeline variable.

**YAML Example:**

```yaml
steps:
- task: create-github-app-token@1
  displayName: create installation token
  name: getToken
  inputs:
    githubAppConnection: my-github-app-service-connection

- bash: |
    pr_number=$(System.PullRequest.PullRequestNumber)
    repo=$(Build.Repository.Name)
    echo "Creating comment in pull request #${pr_number} in repository ${repo}"
    gh api -X POST "/repos/${repo}/issues/${pr_number}/comments" -f body="Posting a comment from Azure Pipelines"
  displayName: Create comment in pull request
  condition: eq(variables['Build.Reason'], 'PullRequest')
  env:
    GH_TOKEN: $(getToken.installationToken)
```

This will post a comment to any pull request that triggers the pipeline, without manual JWT or token management by the pipeline author.

### **Other Exported Variables:**

- `tokenExpiration`: ISO 8601 expiration timestamp
- `installationId`: ID of the installation for which the token was generated

## **Advantages of This Approach**

- **Enhanced Security**: Credentials are centrally managed and not exposed in pipelines.
- **Maintainability**: Standardizes and abstracts authentication logic.
- **Time Savings**: Reusable extension across projects, with shared connections and updates.
- **Automation**: Facilitates automated status checks, comments, issue management, deployment tracking, and more.

## **Conclusion**

Integrating Azure Pipelines and GitHub via GitHub Apps with a custom Azure DevOps extension allows organizations to:

- Remove manual complexity around API authentication.
- Enforce strong security practices and role-based access control.
- Leverage powerful automation features beyond basic CI/CD.

For full source code and more details, visit the [sample extension repository](https://github.com/tspascoal/azure-pipelines-create-github-app-token-task).

---
*Explore more posts on [enterprise software development](https://github.blog/enterprise-software/).*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/enterprise-software/ci-cd/how-to-streamline-github-api-calls-in-azure-pipelines/)
