---
external_url: https://www.reddit.com/r/microsoft/comments/1mjyjre/passwordless_signons_mfa_app_hybrid_mode/
title: Passwordless Sign-On and MFA in Microsoft Hybrid Environments
author: SmoothRunnings
viewing_mode: external
feed_name: Reddit Microsoft
date: 2025-08-07 12:19:51 +00:00
tags:
- Active Directory
- Authenticator App
- Azure Active Directory
- Cloud Authentication
- Hybrid Identity
- Kerberos Cloud Trust
- MFA
- Microsoft
- Microsoft Entra ID
- Multi Factor Authentication
- On Premises Integration
- Passkeys
- Passwordless Authentication
section_names:
- security
---
SmoothRunnings discusses Microsoft's move toward passwordless sign-on and the potential changes for MFA in hybrid scenarios, featuring community input on Cloud Kerberos Trust and the continuity of the Authenticator app.<!--excerpt_end-->

# Passwordless Sign-On and MFA in Microsoft Hybrid Environments

A community member raises questions about Microsoft shifting away from passwords toward passwordless sign-on, specifically regarding:

- **Applicability in hybrid organizations** (with both on-premises and cloud resources)
- Uncertainty about the future of the Microsoft Authenticator app and alternatives for passkeys

**Key Discussion Points:**

- Passwordless authentication is feasible and encouraged by Microsoft, but hybrid scenarios raise challenges for seamless sign-on, particularly where local resources are involved.
- The conversation highlights 'Cloud Kerberos Trust' as a way to authenticate hybrid users against the cloud while retaining access to on-premises resources. This setup is referenced by the presence of a computer account called `AZUREADSSOACC` in Active Directory, indicating Azure AD SSO is configured.
- Clarification is provided on misinformation regarding the discontinuation of the Microsoft Authenticator app. Contributors express skepticism about the rumored retirement and emphasize that authentication options remain robust.

**Relevant Topics:**

- Microsoft's evolving authentication strategy (passwordless, passkeys)
- Hybrid identity and secure access across cloud/on-prem
- Kerberos Trust bridges between local AD and Azure
- Best practices for MFA in Microsoft ecosystems

**References:**

- [Cloud Kerberos Trust documentation](https://learn.microsoft.com/en-us/azure/active-directory/hybrid/cloud-kerberos-trust-overview)
- [Microsoft Authenticator overview](https://learn.microsoft.com/en-us/azure/active-directory/authentication/concept-authentication-authenticator-app)
- [Available Today: GPT-5 in Microsoft 365 Copilot](https://www.microsoft.com/en-us/microsoft-365/blog/2025/08/07/available-today-gpt-5-in-microsoft-365-copilot/)

This post appeared first on "Reddit Microsoft". [Read the entire article here](https://www.reddit.com/r/microsoft/comments/1mjyjre/passwordless_signons_mfa_app_hybrid_mode/)
