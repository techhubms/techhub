---
layout: post
title: 'Going Passwordless: Implementing Passkeys in ASP.NET Core'
author: dotnet
canonical_url: https://www.youtube.com/watch?v=U4_KcjJOxOE
viewing_mode: internal
feed_name: DotNet YouTube
feed_url: https://www.youtube.com/feeds/videos.xml?channel_id=UCvtT19MZW8dq5Wwfu6B0oxw
date: 2025-11-14 09:00:06 +00:00
permalink: /coding/videos/Going-Passwordless-Implementing-Passkeys-in-ASPNET-Core
tags:
- .NET
- 1Password
- ASP.NET Core
- Authentication
- Device Security
- Passkeys
- Passwordless Authentication
- Phishing Resistance
- Public Key Cryptography
- Security Key Integration
- WebAuthn
- Yubikey
section_names:
- coding
- security
---
dotnet provides a thorough and practical session on integrating passkey-based passwordless authentication using the WebAuthn protocol within ASP.NET Core applications, including support for hardware and software security keys.<!--excerpt_end-->

{% youtube U4_KcjJOxOE %}

# Going Passwordless: Implementing Passkeys in ASP.NET Core

## Overview

This session focuses on replacing traditional passwords with passkey authentication, leveraging the WebAuthn protocol in ASP.NET Core applications. Passkeys use strong public key cryptography to enable simpler and more secure authentication experiences, reducing the risk of phishing attacks.

## Key Topics

- **What Are Passkeys?**
  - Passkeys offer a passwordless authentication method using device-based security or dedicated hardware keys.
  - They rely on public key cryptography for robust, phishing-resistant sign-in.

- **WebAuthn Protocol**
  - Foundation of passkey implementations for modern authentication.
  - Enables support for a range of authenticators: built-in device features (TouchID, Windows Hello), password managers (1Password), and hardware security keys (Yubikey).

- **Integration in ASP.NET Core**
  - Practical guidance on incorporating passkey authentication setups into .NET projects.
  - Step-by-step demonstration and discussion of the required code changes, supported device flows, and best practices.

- **Supported Scenarios**
  - Authentication via operating system credential managers
  - Integration with password managers for cloud sync
  - Usage of dedicated hardware devices for added security

## Why Choose Passkeys?

- Completely removes passwords from the authentication process
- Strong resistance to phishing and credential theft
- User-friendly and compatible with modern devices and platforms

## Next Steps

- Explore the fundamentals of passkey authentication in your own ASP.NET Core projects
- Assess the compatibility of various authenticators (OS, hardware, or software-based)
- Review the related .NET Conf 2025 playlist for more advanced security techniques

*Watch the full session and access additional resources at the .NET Conf YouTube playlist: [https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXtIlvq1tuORUtZqVG-HdCt](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXtIlvq1tuORUtZqVG-HdCt)*
