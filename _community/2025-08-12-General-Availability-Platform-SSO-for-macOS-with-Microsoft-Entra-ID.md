---
layout: "post"
title: "General Availability: Platform SSO for macOS with Microsoft Entra ID"
description: "This announcement details the General Availability of Platform Single Sign-On (SSO) for macOS in conjunction with Microsoft Entra ID. It covers what Platform SSO is, key security and usability improvements, deployment steps, and enhancements since public preview, focusing on strengthening identity security and seamless authentication for enterprise Apple device users."
author: "veenasoman"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-entra-blog/now-generally-available-platform-sso-for-macos-with-microsoft/ba-p/4437424"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-12 15:00:00 +00:00
permalink: "/2025-08-12-General-Availability-Platform-SSO-for-macOS-with-Microsoft-Entra-ID.html"
categories: ["Security"]
tags: ["Authentication Logs", "Authentication Strength", "Azure File Shares", "Cloud Kerberos", "Community", "Compliance", "Device Registration", "Enterprise SSO Plug in", "Identity Management", "Intune Company Portal", "Just in Time Onboarding", "Macos", "MDM", "Microsoft Entra ID", "Microsoft Graph API", "Passwordless Authentication", "Platform SSO", "Secure Enclave", "Security", "Single Sign On", "Smart Card Authentication", "UserSecureEnclaveKey", "Zero Trust"]
tags_normalized: ["authentication logs", "authentication strength", "azure file shares", "cloud kerberos", "community", "compliance", "device registration", "enterprise sso plug in", "identity management", "intune company portal", "just in time onboarding", "macos", "mdm", "microsoft entra id", "microsoft graph api", "passwordless authentication", "platform sso", "secure enclave", "security", "single sign on", "smart card authentication", "usersecureenclavekey", "zero trust"]
---

Veena Soman from Microsoft announces the General Availability of Platform SSO for macOS with Microsoft Entra ID, outlining security features and deployment strategies for organizations.<!--excerpt_end-->

# General Availability: Platform SSO for macOS with Microsoft Entra ID

**Authors:** Veena Soman (Senior Software Engineer, Microsoft), Justin Ploegert (Principal Product Manager, Microsoft)

## Overview

Platform Single Sign-On (SSO) is now generally available for macOS with full support from Microsoft Entra ID. This feature, built on the Microsoft Enterprise SSO plug-in, delivers integrated and secure single sign-on experiences across Apple devices in enterprise environments.

## What is Platform SSO?

Platform SSO leverages macOS integration and the Microsoft Enterprise SSO plug-in to allow users to authenticate to their Macs using Microsoft Entra ID. Once authenticated, users experience seamless, passwordless sign-in across applications and browsers, reducing authentication fatigue and enhancing organizational security.

**Key Features:**

- Passwordless sign-in via UserSecureEnclaveKey or Smart Card methods
- Use of device-bound passkeys through Secure Enclave-backed credentials
- Synchronized local accounts using Microsoft Entra ID passwords
- Seamless SSO across native macOS apps and browsers including Edge, Safari, Firefox, and Chrome
- Streamlined device registration and Just-in-Time (JIT) compliance onboarding

## Security and Operational Benefits

- Designed for zero trust security models and hybrid work scenarios
- Strong biometric enforcement (leverage Secure Enclave for phishing resistant authentication)
- Enhanced telemetry for diagnostics and visibility
- Granular authentication strength controls and policies
- Rich authentication logging for auditing and compliance
- Direct integration with Microsoft Graph APIs for programmatic management
- Cloud Kerberos support allowing macOS devices to access Azure file shares natively

## Early Adopter Feedback

Since public preview in May 2024, Platform SSO has seen adoption in education, healthcare, finance, and tech industries. Organizations report:

- Simplified user onboarding
- Reduced helpdesk calls for credential issues
- Strengthened Zero Trust postures

## Deployment Guide

To enable Platform SSO in your organization:

1. Ensure macOS devices are on version 13+ (14+ recommended) and enrolled in MDM such as Intune.
2. Deploy the Microsoft Intune Company Portal app (v5.2504.0 or later).
3. Configure Platform SSO policies in Intune or your preferred MDM.

Links for additional guidance:

- [Detailed Platform SSO documentation](https://learn.microsoft.com/en-us/entra/identity/devices/macos-psso)
- [Microsoft Intune Company Portal setup](https://learn.microsoft.com/en-us/intune/intune-service/apps/apps-company-portal-macos)

## What’s New Since Public Preview

Platform SSO's GA introduces:

- Hardware biometric enforcement (Secure Enclave)
- Enhanced IT admin diagnostics and sign-in log detail
- More granular authentication policy control
- Programmatic configuration via new Microsoft Graph APIs
- Cloud Kerberos support

## Looking Forward

Upcoming features include JIT compliance remediation and a redesigned My Security Info interface. Future Company Portal versions will support newer macOS enhancements, including macOS Tahoe 26, for SSO.

## Learn More

- [macOS Platform Single Sign-on (PSSO) Overview](https://learn.microsoft.com/en-us/entra/identity/devices/macos-psso)
- [Microsoft Enterprise SSO plug-in for Apple devices](https://learn.microsoft.com/en-us/entra/identity-platform/apple-sso-plugin)
- [Platform SSO announcements and guides](https://techcommunity.microsoft.com/t5/microsoft-entra-blog/bg-p/Identity)

For organizations, deploying Platform SSO with Microsoft Entra ID is a significant step forward in securing enterprise Apple devices and streamlining the authentication experience for users.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-entra-blog/now-generally-available-platform-sso-for-macos-with-microsoft/ba-p/4437424)
