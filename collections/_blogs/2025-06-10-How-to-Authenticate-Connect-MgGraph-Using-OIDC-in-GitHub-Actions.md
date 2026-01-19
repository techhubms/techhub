---
external_url: https://jessehouwing.net/authenticate-connect-mggraph-using-oidc-in-github-actions/
title: How to Authenticate Connect-MgGraph Using OIDC in GitHub Actions
author: Jesse Houwing
viewing_mode: external
feed_name: Jesse Houwing's Blog
date: 2025-06-10 15:08:58 +00:00
tags:
- Access Token
- Automation
- Azure Entra ID
- GitHub
- GitHub Actions
- Microsoft Graph
- OIDC
- PowerShell
section_names:
- azure
- devops
- security
---
Jesse Houwing walks through authenticating maintenance PowerShell scripts to Microsoft Graph in GitHub Actions, using OpenID Connect and Azure CLI, for improved security and automation.<!--excerpt_end-->

## Overview

Jesse Houwing outlines his approach to running maintenance scripts against Azure Entra ID for managing GitHub-related tasks—such as removing dormant users and prompting proper notification email setup. Originally, these scripts were run interactively, but have since been transitioned to automated execution in GitHub Actions workflows.

## Transition to GitHub Actions & OIDC Authentication

Initially, the scripts were run with a user present, but Jesse migrated them to GitHub Actions for automation. To further improve the process and eliminate the need to store and manage secrets or tokens, authentication was converted from a static credential-based approach to OpenID Connect (OIDC) integration between GitHub Actions and Azure.

The post references the official Microsoft documentation for [Authenticating to Azure from GitHub Actions by OpenID Connect](https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure-openid-connect?ref=jessehouwing.net), which details how to set up Azure CLI and PowerShell sessions via OIDC in CI/CD pipelines. Jesse notes, however, that while documentation instructs on establishing an Azure PowerShell session with `enable-AzPSSession: true`, it does not clearly explain authentication for the Microsoft Graph PowerShell module via `connect-mggraph` using OIDC.

## Solution: Passing OIDC Token from Azure CLI to Microsoft Graph PowerShell

Jesse describes a workaround, developed with the support of GitHub Copilot, to authenticate `connect-mggraph` by:

1. **Using the az CLI** within the workflow to obtain an access token for `https://graph.microsoft.com`.
2. **Masking the token** as a GitHub Actions secret.
3. **Passing that token** securely to `connect-mggraph` for authentication.

Here is the core YAML that implements this workflow:

```yaml
- name: Azure CLI Login
  uses: azure/login@v2
  with:
    client-id: ${{ secrets.AZURE_CLIENT_ID }}
    tenant-id: ${{ secrets.AZURE_TENANT_ID }}
    allow-no-subscriptions: true

- name: Assign Costcenters
  run: |
    $accessToken = az account get-access-token --resource https://graph.microsoft.com `
      --query accessToken --output tsv
    write-host "::add-mask::$accessToken"
    $accessToken = $accessToken | ConvertTo-SecureString -AsPlainText -Force
    Connect-MgGraph -AccessToken $accessToken -NoWelcome
    # rest of the script here ...
  shell: pwsh
```

This method allows subsequent PowerShell commands to interact with Microsoft Graph securely and without the need for static credentials.

## Benefits and Summary

By moving to GitHub Actions and using OIDC, Jesse improved security (no hard-coded or stored secrets), streamlined automation, and enabled seamless Azure Graph and Entra ID management. This approach can be adapted for other scenarios where GitHub Actions need to authenticate PowerShell modules against Microsoft Graph.

## References

- [Authenticate to Azure from GitHub Actions by OpenID Connect](https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure-openid-connect?ref=jessehouwing.net)

---

**Author:** Jesse Houwing

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/authenticate-connect-mggraph-using-oidc-in-github-actions/)
