---
external_url: https://devblogs.microsoft.com/devops/retirement-of-global-personal-access-tokens-in-azure-devops/
title: 'Azure DevOps Retires Global Personal Access Tokens: Key Dates and Security Impact'
author: Angel Wong
feed_name: Microsoft DevOps Blog
date: 2025-12-12 14:15:06 +00:00
tags:
- Authentication
- Azure DevOps
- Credential Migration
- DevOps Workflows
- Identity Management
- Microsoft Entra
- Organizational Policies
- PAT Retirement
- PATs
- Personal Access Tokens
- Release Management
- Token Decommission
- Token Governance
section_names:
- azure
- devops
- security
---
Angel Wong details Microsoft's retirement plan for Global Personal Access Tokens in Azure DevOps, outlining key security implications and migration steps for practitioners.<!--excerpt_end-->

# Azure DevOps Retires Global Personal Access Tokens: What You Need to Know

Azure DevOps will retire the Global Personal Access Token (PAT) type, impacting developer authentication workflows across Microsoft ecosystems.

## Why Retire Global PATs?

Global PATs allowed users to authenticate across all accessible organizations, but their broad scope posed significant security risks. A single credential with wide reach creates a concentrated target, especially as user privileges expand. Microsoft is shifting away from global tokens to limit the impact of credential exposure.

**Key risks of global PATs:**

- Single point of failure for multiple organization access
- High-value target for attackers
- Difficult to enforce granular credential governance

## New Authentication Strategy

Microsoft recommends transitioning to short-lived, Microsoft Entra–backed authentication:

- **Short-lived tokens** reduce exposure risk
- **Entra-backed authentication** provides strong identity controls and governance
- Organizational-level token policies limit privileges

These improvements reflect broader security strategies already deployed at Microsoft and among Azure DevOps customers.

## Retirement Timeline

- **March 15, 2026**: Creation and regeneration of global PATs will be blocked
- **December 1, 2026**: All existing global PATs are fully decommissioned and will stop working

## What Should You Do?

If you rely on global PATs for DevOps workflows:

- Split authentication by individual Azure DevOps organization
- Migrate to Microsoft Entra-based, short-lived authentication
- Watch for guidance emails for users with active global PATs

## References

- [Retirement of Global Personal Access Tokens in Azure DevOps](https://devblogs.microsoft.com/devops/retirement-of-global-personal-access-tokens-in-azure-devops/)
- [Azure DevOps Blog](https://devblogs.microsoft.com/devops)

## Action Steps

1. Audit existing workflows for global PAT dependencies
2. Plan for authentication migration before the key dates
3. Upgrade CI/CD scripts and DevOps automation to use recommended authentication

## Additional Details

- Global PAT tokens with broad reach can lead to large-scale credential breaches
- Moving to Entra-backed authentication improves security posture through modern token management and identity control

**Contact:** Azure DevOps team for further migration support and best practices

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/retirement-of-global-personal-access-tokens-in-azure-devops/)
