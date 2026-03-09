---
layout: "post"
title: "Azure Compute API 2025-11-01: securityType Field Now Always Non-Null in VM Responses"
description: "This announcement details a behavioral change in the Azure Compute API as of version 2025-11-01: all responses for Virtual Machines and Virtual Machine Scale Sets will always include a non-null securityType field. It explains the precise API changes, why the update was made, which automation or validation scripts might be affected, and the actions customers should take to adapt their deployments and code. An explicit example and recommendations for handling the updated response structure are provided."
author: "AjKundnani"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-compute-blog/upcoming-compute-api-change-always-return-non-null-securitytype/ba-p/4500387"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-09 14:16:14 +00:00
permalink: "/2026-03-09-Azure-Compute-API-2025-11-01-securityType-Field-Now-Always-Non-Null-in-VM-Responses.html"
categories: ["Azure", "Security"]
tags: ["API Response", "API Versioning", "Automation", "Azure", "Azure Compute API", "Azure Infrastructure", "Azure Resource Manager", "Community", "Compliance", "Confidential VM", "Deployment Scripts", "Security", "Securitytype", "Standard Security", "Trusted Launch", "Validation", "Virtual Machine Scale Sets", "Virtual Machines"]
tags_normalized: ["api response", "api versioning", "automation", "azure", "azure compute api", "azure infrastructure", "azure resource manager", "community", "compliance", "confidential vm", "deployment scripts", "security", "securitytype", "standard security", "trusted launch", "validation", "virtual machine scale sets", "virtual machines"]
---

AjKundnani details the upcoming change in Azure Compute API 2025-11-01, where VM and VMSS responses will always return a non-null securityType, impacting automation and validation scripts that rely on null checks.<!--excerpt_end-->

# Upcoming Change: Azure Compute API Will Always Return Non-Null securityType

**Author:** AjKundnani  
**Date:** March 9, 2026

## Summary

Starting with Azure Compute API version **2025-11-01**, all API responses for Azure Virtual Machines (VMs) and Virtual Machine Scale Sets (VMSS) will include a non-null `securityType` in every operation. This is a response-level change only: there is no effect on VM runtime behavior or resource configuration, but automation, validation logic, and post-deployment scripts that rely on `securityType` being null must be updated.

## Details of the Change

- Applies **only to API version 2025-11-01**. Older versions remain unaffected and will continue to return `null` for securityType if unspecified.
- For all create, update, and GET operations:
  - If `securityType` is omitted or set to `null`, the API will now return `securityType: "Standard"`.
  - Existing resources with explicitly set values (`TrustedLaunch`, `ConfidentialVM`) will continue to return the originally specified value.

**Input and Returned Values Table (API version 2025-11-01):**

| Input SecurityType | Returned Value |
|--------------------|---------------|
| <null>             | Standard      |
| Standard           | Standard      |
| TrustedLaunch      | TrustedLaunch |
| ConfidentialVM     | ConfidentialVM|

- This ensures every VM/VMSS resource reflects a defined security posture in response payloads.
- **No change to how VMs run, are provisioned, or behave at runtime.**

## Reason for the Change

- The `securityType` field now represents an explicit security model for the VM or VMSS.
- Always returning a set value, even if it was not provided at creation, removes ambiguity for consumers and enables precise, predictable scripting and validation.
- Aligns the API contract with actual runtime guarantees: all resources operate under a defined security model.

## Impact Assessment

- **Who is affected:**
  - Customers using **API version 2025-11-01** for automation, validation, or compliance.
  - Scripts, tests, or checks that depend on `securityType` being null to signal default or legacy configuration.
- **Who is NOT affected:**
  - Customers using API versions prior to 2025-11-01.
  - Existing deployments and infrastructure or tools that do not consider `securityType` in their logic.
- **Workloads themselves are unaffected.**

## Actions Required

- **If you will use API version 2025-11-01:**
  - Review and update any automation, validation, or post-deployment scripts that:
    - Rely on `securityType == null`
    - Treat missing or null `securityType` as a special case
  - Instead, script logic should treat `securityType: "Standard"` as the default or legacy configuration.
  - Remove or adjust null checks that could block logic or fail deployments under the new API behavior.
  - Test workflows early to ensure no regressions after API version update.
- **No action required for older API versions.**

### Example: GET Response (API version 2025-11-01)

If a VM is created **without specifying `securityType`**, the GET response now looks like:

```json
{
  "id": "<vmResourceId>",
  "name": "<vmName>",
  "type": "Microsoft.Compute/virtualMachines",
  "properties": {
    "securityProfile": {
      "securityType": "Standard"
    }
  }
}
```

## Next Steps

- Review and revise scripting logic before using Azure Compute API version 2025-11-01.
- Validate all automation and compliance checks that interact with the `securityType` field.
- Refer to official documentation and changelogs for updates as you plan upgrades or migrations.

---

*For more technical updates, see the [Azure Compute Blog](https://techcommunity.microsoft.com/t5/azure-compute-blog/bg-p/AzureCompute).*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/upcoming-compute-api-change-always-return-non-null-securitytype/ba-p/4500387)
