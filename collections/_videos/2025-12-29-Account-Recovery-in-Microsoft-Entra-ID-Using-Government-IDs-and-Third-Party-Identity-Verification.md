---
external_url: https://www.youtube.com/watch?v=WYji1oV7GQI
title: Account Recovery in Microsoft Entra ID Using Government IDs and Third-Party Identity Verification
author: John Savill's Technical Training
feed_name: John Savill's Technical Training
date: 2025-12-29 16:09:09 +00:00
tags:
- Account Recovery
- Au10tix
- Azure Cloud
- Cloud
- Cloud Security
- Entra Id
- Government ID
- Identity
- Identity Verification
- Microsoft
- Microsoft Azure
- Microsoft Entra ID
- Passkeys
- Passwordless
- Phishing Resistance
- Self Service Password Reset
- Third Party Authentication
- Azure
- Security
- Videos
section_names:
- azure
- security
primary_section: azure
---
John Savill's Technical Training examines Microsoft's new Entra ID account recovery process using government-issued IDs and third-party verification, providing practical guidance and an architectural overview for Azure security practitioners.<!--excerpt_end-->

{% youtube WYji1oV7GQI %}

# Account Recovery in Microsoft Entra ID Using Government IDs and Third-Party Identity Verification

## Introduction

Microsoft Entra ID (formerly known as Azure Active Directory) now offers advanced account recovery options that leverage government-issued IDs and third-party identity verification providers. This feature is designed to eliminate reliance on passwords, SMS, or traditional helpdesk processes for account recovery, enhancing both user experience and security.

## The Problem with Traditional Recovery

- **Passwords and SMS-based recovery** are increasingly vulnerable to phishing and SIM swap attacks.
- Self-Service Password Reset (SSPR) can be ineffective for users who lose access to all registered recovery methods.
- Helpdesk resets require manual verification, prone to social engineering risks.

## The Shift to Passwordless and Passkeys

- Microsoft is investing in passwordless authentication (passkeys), offering phishing-resistant and device-bound identity proofs.
- Device-bound and synced passkeys offer flexibility but demand secure recovery mechanisms in case of device loss.

## The New Entra ID Account Recovery Feature

- **Account recovery can be triggered using official government-issued ID documents.**
- Third-party identity verification services (like AU10TIX) are integrated to validate user identities securely without manual helpdesk intervention.

## How It Works

1. **User initiates account recovery:** If all standard sign-in and recovery options fail, users are prompted to verify their identity.
2. **Submit government ID:** Users use their device to submit a scan or photo of their government-issued identification.
3. **Third-party verification:** Trusted providers validate the identity document and confirm user identity.
4. **Restoring access:** Upon successful verification, access to the Microsoft Entra ID account and associated Azure/cloud services is restored.

## Implementation and Setup (as covered in the video)

- Administrators can enable this feature through the Entra ID portal.
- Configuration requires linking supported third-party verification services.
- Detailed setup steps and recommendations can be found in the [Microsoft documentation](https://learn.microsoft.com/entra/identity/authentication/how-to-account-recovery-enable).

## Security Benefits

- More resilient against phishing and social engineering attacks.
- Reduces helpdesk workload and risk of human error in verification.
- Strengthens security posture for Azure and Microsoft 365 environments.

## Challenges and Considerations

- User privacy and compliance with local regulations regarding ID handling.
- The reliability of third-party verification providers.
- Adoption and user training for new recovery workflows.

## Additional Resources

- [Whiteboard Overview (PNG)](https://github.com/johnthebrit/RandomStuff/raw/master/Whiteboards/SSAR.png)
- [Microsoft Documentation](https://learn.microsoft.com/entra/identity/authentication/how-to-account-recovery-enable)
- [Azure and Security Playlists by John Savill](https://youtube.com/playlist?list=PLlVtbbG169nEv7jSfOVmQGRp9wAoAM0Ks)

## Summary

This feature represents a major update to Microsoft Entra ID's security and user management capabilities, aiming to make cloud identity management more secure and less dependent on legacy recovery processes. Organizations using Azure and related Microsoft services should evaluate enabling this feature for enhanced account recovery security.
