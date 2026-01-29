---
external_url: https://azure.microsoft.com/en-us/blog/azure-mandatory-multifactor-authentication-phase-2-starting-in-october-2025/
title: 'Azure Mandatory Multifactor Authentication: Phase 2 Launches October 2025'
author: Joy Shah and Neha Kulkarni
feed_name: Microsoft Security Blog
date: 2025-09-05 15:00:00 +00:00
tags:
- Azure CLI
- Azure Policy
- Azure Portal
- Azure PowerShell
- Azure Resource Manager
- Cloud Security
- Identity Management
- MFA
- Microsoft Entra ID
- Multifactor Authentication
- Resource Management
- Safe Deployment
- Azure
- Security
- News
section_names:
- azure
- security
primary_section: azure
---
Joy Shah and Neha Kulkarni explain Microsoft's rollout of Phase 2 mandatory multifactor authentication enforcement on Azure Resource Manager operations, with critical details on its October 2025 launch, technical scope, and customer action points.<!--excerpt_end-->

# Azure Mandatory Multifactor Authentication: Phase 2 Launches October 2025

**Authors:** Joy Shah and Neha Kulkarni

Microsoft Azure is set to begin Phase 2 of its multifactor authentication (MFA) enforcement for Azure Resource Manager operations, effective October 1, 2025. This move is aimed at further enhancing cloud security following the initial rollout for administrative portal sign-ins.

## Background

Microsoft research shows that MFA can block over 99% of account compromise attacks. In response to rising cyber threats, Azure began enforcing MFA for Azure Public Cloud sign-ins in August 2024.

### MFA Enforcement Phases

- **Phase 1:** Mandatory MFA for sign-ins to Azure Portal, Microsoft Entra admin center, and Intune admin center (rolled out to 100% of tenants by March 2025).
- **Phase 2 (starting October 1, 2025):** Requires MFA for users performing Azure resource management operations via any supported client, such as:
  - Azure CLI
  - Azure PowerShell
  - Azure Mobile App
  - REST APIs
  - Azure SDK client libraries
  - IaC tools

Phase 2 enforcement will be applied gradually using [Azure Policy](http://aka.ms/AZUREPOLICY) and Microsoft's [safe deployment practices](https://azure.microsoft.com/blog/advancing-safe-deployment-practices/).

## Customer Impact

- Users must complete MFA before performing resource management actions in Azure.
- Workload identities (managed identities and service principals) are **not impacted** by either phase of enforcement.
- Azure Entra Global Administrators have been notified with detailed preparation steps via email and [Azure Service Health notifications](https://learn.microsoft.com/en-us/azure/service-health/service-health-notifications-properties).

## How to Prepare

1. **Enable MFA for Users**:
   - Ensure all users can authenticate with MFA by October 1, 2025.
   - Identify affected users using [these steps](https://learn.microsoft.com/en-us/entra/identity/authentication/how-to-mandatory-multifactor-authentication).

2. **Assess Impact with Azure Policy**:
   - Assign built-in Azure Policy definitions to block resource management actions when MFA is not used. Gradual rollout by resource scope, type, or region is supported.

3. **Update Azure CLI and PowerShell**:
   - Use at least Azure CLI version 2.76 and Azure PowerShell version 14.3 for best compatibility.

4. **If Needed, Postpone Enforcement**:
   - Global Administrators can request a deferral via the [Azure Portal](https://aka.ms/postponePhase2MFA).

## Additional Resources

- [Prepare for mandatory MFA enforcement](https://learn.microsoft.com/en-us/entra/identity/authentication/concept-mandatory-multifactor-authentication?tabs=dotnet#prepare-for-mandatory-mfa-enforcement)
- [Scope of enforcement](https://learn.microsoft.com/en-us/entra/identity/authentication/concept-mandatory-multifactor-authentication?tabs=dotnet#scope-of-enforcement)
- [Notification channels](https://learn.microsoft.com/en-us/entra/identity/authentication/concept-mandatory-multifactor-authentication#notification-channels)
- [Safe deployment practices](https://azure.microsoft.com/blog/advancing-safe-deployment-practices/)

## Next Steps Checklist

- Enable MFA for all users handling resource management
- Review and assign [Azure Policy definitions](https://aka.ms/MFAforAzureSelfEnforce)
- Update CLI and PowerShell clients
- Use notification channels for updates and support

For further information, reference the Microsoft Security Blog and official Azure documentation links provided above.

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/azure-mandatory-multifactor-authentication-phase-2-starting-in-october-2025/)
