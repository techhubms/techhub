---
layout: "post"
title: "Setting Up Security Policies in Microsoft 365 Trial Tenants"
description: "This guide by Dellenny details step-by-step procedures for establishing core security policies in a Microsoft 365 trial tenant. It covers multi-factor authentication, blocking legacy authentication, configuring Defender for Office 365 protection, enforcing password settings, using Conditional Access, managing admin roles, and enabling security alerts to create a secure cloud environment from the start."
author: "Dellenny"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://dellenny.com/how-to-set-up-basic-security-policies-in-a-microsoft-365-trial-tenant/"
viewing_mode: "external"
feed_name: "Dellenny's Blog"
feed_url: "https://dellenny.com/feed/"
date: 2025-11-17 09:17:43 +00:00
permalink: "/2025-11-17-Setting-Up-Security-Policies-in-Microsoft-365-Trial-Tenants.html"
categories: ["Security"]
tags: ["Admin Roles", "Audit Logs", "Authentication Methods", "Azure Active Directory", "Blogs", "Cloud Security", "Conditional Access", "Defender For Office 365", "Email Security", "Entra ID", "Identity Protection", "Least Privilege", "Malware Protection", "Microsoft 365", "Microsoft 365 Certified: Fundamentals", "Multi Factor Authentication", "Password Policies", "Phishing Protection", "Privileged Identity Management", "Purview", "Security", "Security Defaults", "Threat Protection", "Trial Tenant"]
tags_normalized: ["admin roles", "audit logs", "authentication methods", "azure active directory", "blogs", "cloud security", "conditional access", "defender for office 365", "email security", "entra id", "identity protection", "least privilege", "malware protection", "microsoft 365", "microsoft 365 certified fundamentals", "multi factor authentication", "password policies", "phishing protection", "privileged identity management", "purview", "security", "security defaults", "threat protection", "trial tenant"]
---

Dellenny guides readers through setting up baseline security protections for Microsoft 365 trial tenants, highlighting practical steps to reduce vulnerabilities and secure cloud services.<!--excerpt_end-->

# Setting Up Security Policies in Microsoft 365 Trial Tenants

Author: Dellenny
Date: November 17, 2025

## Why Secure Your Trial Tenant?

Trial tenants in Microsoft 365 are still exposed to the internet. Even if they're for short-term use or demonstrations, they can be targeted if left with default settings. Applying basic security measures is essential to protect potential real data and to form habits for production environments.

## 1. Enable Multi-Factor Authentication (MFA)

**MFA is crucial for preventing unauthorized access.**

- **Security Defaults (Recommended for Trials):**
  - Go to Azure Active Directory / Entra ID Admin Center.
  - Navigate to Properties.
  - Select Manage Security Defaults.
  - Turn Security Defaults On.

- **Conditional Access (for Entra ID P1/P2):**
  - Go to Azure AD (Entra ID) → Security → Conditional Access.
  - Assign MFA requirements to all users/cloud apps.
  - Requires additional licensing.

## 2. Block Legacy Authentication

Legacy protocols (IMAP, POP, SMTP Basic) are common attack entry points. Disable them via Security Defaults or Conditional Access policies, focusing on blocking legacy authentication for all users/apps.

## 3. Establish Baseline Conditional Access Policies

For tenants with Conditional Access (P1/P2), implement:

- Require MFA for admins/users
- Block access from outside trusted locations
- Restrict by geography (optional)

## 4. Enable Defender for Office 365 Protection

Configure:

- **Anti-Malware Policies:** Zero-hour auto purge, attachment filtering, malware detection response
- **Anti-Phishing Policies:** Spoof intelligence, impersonation protection, Safe Links, Safe Attachments

## 5. Enforce Password Protection Settings

- Enable password expiration
- Use banned-password lists
- Encourage passwordless authentication using Microsoft Authenticator or Entra ID → Authentication Methods

## 6. Set Up User Risk Policies (If Available)

With Azure AD Identity Protection:

- Require password reset for medium+ risk users
- Require MFA based on sign-in risk
- Start with report-only mode, move to enforce

## 7. Review Admin Roles & Least Privilege

- Limit Global Admin accounts (ideally two)
- Use Privileged Identity Management (PIM)
- Assign specific admin roles for tasks

## 8. Enable Security Alerts & Notifications

Set up alerts for sign-in risks, identity protection, Defender notifications, audit logs, and unified audit logs in Purview for visibility and auditing.

---

By following these steps in your Microsoft 365 trial tenant, you create an effective baseline security posture. These practices help prevent common mistakes and establish routines that scale smoothly to production environments.

This post appeared first on "Dellenny's Blog". [Read the entire article here](https://dellenny.com/how-to-set-up-basic-security-policies-in-a-microsoft-365-trial-tenant/)
