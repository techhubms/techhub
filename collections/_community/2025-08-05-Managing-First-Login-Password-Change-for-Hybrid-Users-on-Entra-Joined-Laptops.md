---
external_url: https://www.reddit.com/r/AZURE/comments/1mi1ffw/hybrid_users_entra_joined_laptops_force_password/
title: Managing First Login Password Change for Hybrid Users on Entra Joined Laptops
author: simdre79
feed_name: Reddit Azure
date: 2025-08-05 06:09:28 +00:00
tags:
- Autopilot V2
- Azure Active Directory
- Device Enrollment
- Entra Join
- First Login
- Hybrid Users
- Identity Management
- IT Support Standards
- Microsoft Entra ID
- Pass Through Authentication
- Password Management
- User Provisioning
section_names:
- azure
- security
---
Author simdre79 explores the difficulties of enforcing password changes at first login for hybrid users on Entra joined laptops, especially after transitioning from hybrid to Entra join. This article offers insights and practical issues encountered during device enrollment and user onboarding.<!--excerpt_end-->

## Introduction

Transitioning IT systems and procedures can present significant challenges, especially around user onboarding and account security. In this article, author simdre79 describes a specific concern: enforcing a password change on first login for new users in an environment that has moved from hybrid Azure AD joined to purely Entra joined (Azure AD joined) computers, while users themselves are still managed on-premises.

## Background

Previously, hybrid Azure AD joined computers allowed IT staff to set the 'require user to change password at next login' flag for new accounts, ensuring that the user must set their own password upon first sign-in. However, after switching to Entra join-only devices, this workflow no longer functions as expected.

### Current Setup

- **Device Enrollment:** Devices are provisioned through Autopilot v2. During Out-of-Box Experience (OOBE), student workers log in using a temporary password as the user, then hand off the device to the new employee after enrollment completes.
- **User Accounts:** Users are still provisioned and maintained on-premises.

## The Problem: Password Change at First Login

With Entra joined PCs, the on-premises password expired flag does not trigger the expected password change prompt at first login. Instead, users see the error: "the sign-in method you're trying to use isn't allowed." This is a known limitation documented by Microsoft (see [Microsoft Learn](https://learn.microsoft.com/en-us/entra/identity/hybrid/connect/how-to-connect-pta-current-limitations)). Even setting a temporary password in Azure AD does not resolve this if the user hasn't signed in to the device previously.

## Attempted Workarounds

- **Complex Temporary Passwords:** One workaround was to assign a complex, difficult-to-remember initial password, hoping users would change it upon first sign-in. However, this relies on user initiative, which cannot be guaranteed.
- **Manual Instructions:** The support staff's traditional method—setting a simple password and enabling 'require user to change password at next login'—is no longer effective due to the new authentication model.

## Desired Outcome

The goal is to enforce, at the system level, a process that requires all new users to change their initial password on their first login, regardless of user awareness or individual compliance.

## Observations

- Entra join breaks the simple on-prem 'force change password at next login' approach for hybrid users.
- Temporary passwords set in Azure Active Directory (AAD) offer no solution if the user has not previously authenticated on the device.

## Open Questions

- Are there alternative workflows or technical solutions for enforcing first-login password changes on Entra joined devices for hybrid users?
- What policies or technical features might Microsoft or third-party tools offer to bridge this gap?

## Conclusion

Enforcing robust password management policies during onboarding is essential, but changes in identity platform architecture (hybrid to Entra join) can disrupt established workflows. Organizations need to be aware of these limitations and consider both technical and procedural alternatives to maintain security and user experience.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mi1ffw/hybrid_users_entra_joined_laptops_force_password/)
