---
external_url: https://devblogs.microsoft.com/dotnet/enhanced-security-is-here-with-the-new-trust-publishing-on-nuget-org/
title: New Trusted Publishing Enhances Security on NuGet.org
author: Evgeny Tvorun, Sean
feed_name: Microsoft .NET Blog
date: 2025-09-22 15:40:00 +00:00
tags:
- .NET
- API Key Rotation
- CI/CD
- GitHub Actions
- NuGet
- OIDC
- OpenSSF
- Package Security
- Policy Management
- Securing Software Supply Chain
- Security Best Practices
- Trusted Publishing
- Workflow Automation
section_names:
- coding
- devops
- security
primary_section: coding
---
Evgeny Tvorun and Sean highlight Trusted Publishing on NuGet.org, guiding developers to secure package publishing by using GitHub OIDC tokens instead of long-lived API keys.<!--excerpt_end-->

# New Trusted Publishing Enhances Security on NuGet.org

Trusted Publishing is now available on NuGet.org, offering a simpler and more secure way to publish NuGet packages with GitHub Actions. Instead of managing long-lived API keys, developers can use short-lived OpenID Connect (OIDC) tokens from GitHub to request temporary NuGet API keys. These temporary keys are valid for about one hour, reducing the risks of secret leaks and minimizing manual key rotation.

## Why Use Trusted Publishing?

- **No long-lived secrets**: Sensitive credentials are not stored in your repository or CI.
- **Short-lived credentials**: NuGet API keys are generated just-in-time and expire quickly.
- **One token per publish**: Each workflow job gets a unique, short-lived API key for each publish operation.

[Read the official announcement](https://devblogs.microsoft.com/dotnet/enhanced-security-is-here-with-the-new-trust-publishing-on-nuget-org/) and [Detailed documentation](https://aka.ms/nuget/trusted-publishing).

## Getting Started

1. **Access Trusted Publishing:**
   - Sign in to [nuget.org](https://www.nuget.org/)
   - Go to your user menu in the top right
   - Click on **Trusted Publishing**
2. **Create a Trusted Publishing Policy:**
   - Specify the package and repository owner
   - Define the repository (GitHub org/user and repository name)
   - Select the workflow file (e.g., `release.yml` in `.github/workflows/`)
   - Optionally, configure environment settings if used in your workflow
3. **Configure GitHub Actions Workflow:**
   - Enable OIDC in your workflow's permissions
   - Use the `NuGet/login@v1` action to obtain a temp API key
   - Push your package using the temporary key

### Minimal GitHub Actions Example

```yaml
permissions:
  id-token: write # required for GitHub OIDC

jobs:
  build-and-publish:
    permissions:
      id-token: write
    steps:
      # Build steps for your .nupkg package
      - name: NuGet login (OIDC → temp API key)
        uses: NuGet/login@v1
        id: login
        with:
          user: contoso-bot  # Use your nuget.org profile name
      - run: dotnet nuget push artifacts/my-sdk.nupkg --api-key ${{ steps.login.outputs.NUGET_API_KEY }} --source https://api.nuget.org/v3/index.json
```

## How Trusted Publishing Works

1. GitHub issues an OIDC token to your workflow job
2. The NuGet login action exchanges the token for a short-lived API key from nuget.org
3. NuGet.org validates the request and policy
4. The workflow uses this API key to securely publish the package

## Policy Ownership and Lifecycle

- **Private repo bootstrap (7 days):** New policies for private repos are active for 7 days, and then become permanent after the first successful login.
- **Owner bound:** Policies are tied to package and repository ownership.
- **Org changes respected:** Losing org access disables the policy; restoring access re-enables it.

## Migrating to Trusted Publishing

- Set up a Trusted Publishing policy on nuget.org
- Remove long-lived NuGet API keys from your repo/CI
- Implement the `NuGet/login@v1` action in your workflow
- Use the returned API key for pushing packages

## Additional Resources

- [Trusted Publishing Docs](https://aka.ms/nuget/trusted-publishing)
- [OpenSSF Guidelines](https://openssf.org/)
- [Securing Software Repos working group](https://openssf.org/technical-initiatives/repository-security/)

By adopting Trusted Publishing, package authors can publish more securely and avoid manual key management, contributing to a more trustworthy .NET ecosystem.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/enhanced-security-is-here-with-the-new-trust-publishing-on-nuget-org/)
