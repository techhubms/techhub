---
external_url: https://devblogs.microsoft.com/all-things-azure/tutorial-videos-setting-up-github-copilot-for-your-company/
title: 'Tutorial Videos: Setting up GitHub Copilot for Your Company'
author: Eldrick Wega
feed_name: Microsoft All Things Azure Blog
date: 2025-11-14 11:40:23 +00:00
tags:
- Agentic Platform
- All Things Azure
- Azure Subscription
- Code Review
- Configuration
- Developer Productivity
- Developer Tools
- GitHub
- GitHub Enterprise
- GitHub Licences
- IDE Integration
- Licensing
- Microsoft Entra ID
- Operations
- Organization Onboarding
- Permissions
- Setup Guide
- Single Sign On
- SSO
- Trial Account
section_names:
- ai
- azure
- devops
- github-copilot
- security
primary_section: github-copilot
---
Eldrick Wega shares a succinct video series on configuring GitHub and GitHub Copilot for enterprises, highlighting licensing, SSO, Azure connectivity, and agentic platform setup.<!--excerpt_end-->

# Tutorial Videos: Setting up GitHub Copilot for Your Company

This guide features a set of brief, focused videos and resource links designed to help organizations onboard GitHub Copilot efficiently. The walkthroughs target technical teams looking to configure GitHub Enterprise, integrate with Microsoft Entra ID for SSO, connect Azure subscriptions, and assign Copilot licenses both on and off GitHub Enterprise.

## Contents

- GitHub licenses overview (Enterprise, Advanced Security, Copilot)
- SSO configuration with Microsoft Entra ID
- Azure subscription setup
- GitHub Copilot license assignment for various user scenarios

---

### Video 1: GitHub Licenses Overview (~8 mins)

- Explains GitHub license types including Copilot
- Covers provisioning roles
- Key docs:
  - [GitHub Enterprise Owner](https://docs.github.com/en/enterprise-cloud@latest/admin/concepts/enterprise-fundamentals/roles-in-an-enterprise)
  - [Microsoft Entra ID Application Administrator](https://learn.microsoft.com/en-us/entra/identity/role-based-access-control/permissions-reference#application-administrator)
  - [Azure Subscription Owner & Billing Access](https://learn.microsoft.com/en-us/azure/cost-management-billing/manage/manage-billing-access)

### Video 2: Configure SSO for Your GitHub Tenant (~15 mins)

- SSO setup between GitHub and Microsoft Entra ID
- Guidance for Entra ID Administrators
- Step-by-step generator: [Custom Instruction Steps](https://ghec-sso-setup.azurewebsites.net/)

### Video 3: Add Azure Subscription (~4 mins)

- Adding Azure subscription to GitHub
- Roles for Entra ID Admin and Subscription Owner
- Docs: [Connect Azure Sub](https://docs.github.com/en/enterprise-cloud@latest/billing/how-tos/set-up-payment/connect-azure-sub)

### Video 4: Assign GitHub Copilot Licenses (GHE Users) (~9 mins)

- Assigning Copilot within GitHub Enterprise Cloud
- Organization setup:
  - [Create GitHub Organization](https://docs.github.com/en/enterprise-cloud@latest/enterprise-onboarding/setting-up-organizations-and-teams/setting-up-an-organization#creating-a-new-organization)
  - [Enable Copilot for Organizations](https://docs.github.com/en/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-for-enterprise/manage-access/grant-access#enabling-copilot-for-organizations)
  - Related resources: [Coding Agent](https://www.youtube.com/watch?v=1GVBRhDI5No), [Code Review](https://www.youtube.com/watch?v=HDEGFNAUkX8)

### Video 5: Assign GitHub Copilot Licenses (Non-GHE Users) (~8 mins)

- Assigning Copilot for IDE/CLI users outside GitHub Enterprise
- [Assign Licenses to Users/Teams](https://docs.github.com/en/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-for-enterprise/manage-access/grant-access#assigning-licenses-to-users-or-teams)

---

**Notes:**

- Expect updates as onboarding processes evolve
- Bookmark for quick setup reference
- Test features with a [GitHub Enterprise Trial](https://docs.github.com/en/get-started/onboarding/getting-started-with-the-github-enterprise-cloud-trial)

---

For technical leads, administrators, and DevOps teams, these resources clarify permissions, SSO security, and license management using GitHub, Copilot, Azure subscription linkage, and Microsoft Entra ID.

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/tutorial-videos-setting-up-github-copilot-for-your-company/)
