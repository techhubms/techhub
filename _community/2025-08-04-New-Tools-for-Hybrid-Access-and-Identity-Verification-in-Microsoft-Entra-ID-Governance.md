---
layout: "post"
title: "New Tools for Hybrid Access and Identity Verification in Microsoft Entra ID Governance"
description: "Joseph Dadzie introduces new Microsoft Entra ID governance features in public preview: Group Source of Authority conversion for legacy AD groups and Face Check in Entitlement Management, enabling modern cloud-based governance and high-assurance identity verification for hybrid, on-premises, and cloud environments."
author: "Joseph Dadzie"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-entra-blog/new-governance-tools-for-hybrid-access-and-identity-verification/ba-p/4422534"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-04 17:10:48 +00:00
permalink: "/2025-08-04-New-Tools-for-Hybrid-Access-and-Identity-Verification-in-Microsoft-Entra-ID-Governance.html"
categories: ["Azure", "Security"]
tags: ["Access Management", "Active Directory", "Azure", "Cloud Security", "Community", "Compliance", "Entitlement Management", "Entra ID Governance", "Face Check", "Group Source Of Authority", "Group Writeback", "Hybrid Access", "Identity Governance", "Microsoft Entra", "On Premises Applications", "Security", "Verified ID"]
tags_normalized: ["access management", "active directory", "azure", "cloud security", "community", "compliance", "entitlement management", "entra id governance", "face check", "group source of authority", "group writeback", "hybrid access", "identity governance", "microsoft entra", "on premises applications", "security", "verified id"]
---

In this article, Joseph Dadzie introduces new Microsoft Entra ID governance tools for hybrid environments, detailing features like Group SOA conversion and Face Check for secure, auditable access management.<!--excerpt_end-->

## Introduction

Identity governance is becoming increasingly vital as organizations operate across cloud, remote, and on-premises environments. To address the challenges in access management and compliance, Microsoft is adding two major capabilities to Microsoft Entra ID Governance: Group Source of Authority (SOA) conversion and Face Check in Entitlement Management.

## New Capabilities Announced

### Group Source of Authority (SOA) Conversion

This feature streamlines the governance of legacy Active Directory (AD) security groups by converting them into dynamic and policy-driven groups managed in Microsoft Entra ID. Key benefits include:

- **Cloud Management:** AD groups are managed in Entra ID, centralizing control and increasing flexibility and visibility.
- **On-Premises Cleanup:** Unused or redundant groups can be removed from AD after migration, improving manageability.
- **Governance for On-Premises Apps:** Modern governance policies, request workflows, expiration, and AI-assisted reviews can be applied to groups used by on-premises apps without breaking compatibility.
- **Group Writeback:** Ensures that changes made in the cloud propagate correctly to on-premises systems.

This capability helps organizations quickly modernize access control for legacy infrastructure while maintaining compatibility.

### Face Check in Entitlement Management

Integrating Microsoft Entra Verified ID and Face Check directly into Entitlement Management adds a new, high-assurance layer of identity verification. Features include:

- **Automated Identity Verification:** Matches a user’s selfie to a government-issued credential before granting access to sensitive resources.
- **Accelerated Onboarding:** Reduces delays and manual identity checks, particularly for contractors and guests.
- **Stronger Access Assurance:** Ensures only authorized individuals obtain access, improving compliance and auditability.

## Real-World Scenario: Contoso Finance

A practical example is provided with Contoso Finance, where the Finance department uses a business-critical on-premises application controlled by a traditional AD group. Moving governance to the cloud involves:

1. **Transitioning the AD Group:** The group becomes manageable from the cloud using Group SOA conversion, with changes synced back to AD via Cloud Sync.
2. **Lifecycle Governance:** The group is wrapped with an access package that enforces request-based access, approval workflows, and expiry policies.
3. **High-Assurance Verification:** For enhanced security, users must complete a privacy-respecting Face Check (selfie matched to government-issued ID) before gaining access.

This approach modernizes access management for a legacy app without requiring updates to the application itself.

## Key Takeaways

- Organizations can now bring structure, automation, and intelligence to identity and access management for hybrid and legacy environments.
- Modern cloud-native policy enforcement, approval workflows, time-bound controls, and real-time verification are available for both cloud and on-premises apps.
- No major changes are needed to legacy applications to benefit from these new governance capabilities.

## Getting Started

- Register for webinars such as "Cabin check-in: Ensure least privilege access" to see these tools in action.
- Read more via the Microsoft Entra Suite blog, trial guides, or start a free trial to experience the new features first-hand.

## Additional Resources

- [Microsoft Entra Suite Blog](https://aka.ms/EntraSuite/TEIBlog)
- [Microsoft Entra Suite Trial](https://signup.microsoft.com/get-started/signup?products=2ebf8ffa-7de1-4d14-9b15-238f5ca77671&ali=1&bac=1&signedinuser=reidschrodel%40microsoft.com)
- [Technical Documentation for Microsoft Entra](https://learn.microsoft.com/en-us/entra/)

*--Joseph Dadzie, VP Product Management*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-entra-blog/new-governance-tools-for-hybrid-access-and-identity-verification/ba-p/4422534)
