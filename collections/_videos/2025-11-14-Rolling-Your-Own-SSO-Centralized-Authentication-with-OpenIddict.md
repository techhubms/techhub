---
external_url: https://www.youtube.com/watch?v=q4NhguFDP5s
title: 'Rolling Your Own SSO: Centralized Authentication with OpenIddict'
author: dotnet
viewing_mode: internal
feed_name: DotNet YouTube
date: 2025-11-14 07:30:06 +00:00
tags:
- .NET
- ADFS
- Application Integration
- Authentication
- Authorization
- Centralized Login
- Custom Authentication
- Entra ID
- Identity Management
- OpenIddict
- Security Best Practices
- Session Management
- Single Sign On
- SSO Architecture
- User Workflow
section_names:
- coding
- security
---
This practical guide by Dustin Kingen demonstrates how to implement centralized SSO using OpenIddict for multiple .NET applications, presenting design strategies and integration tips for improved authentication.<!--excerpt_end-->

{% youtube q4NhguFDP5s %}

# Rolling Your Own SSO: Centralized Authentication with OpenIddict

Managing authentication across multiple apps often leads to duplicated logic, fragmented user tables, and unreliable external providers. In his .NET Conf 2025 session, Dustin Kingen addresses these issues by presenting a practical approach to build a centralized Single Sign-On (SSO) server using OpenIddict.

## Why Roll Your Own SSO?

- **External Limitations:** Services like ADFS and Entra ID may be unavailable or unreliable due to project constraints or organizational policy.
- **Fragmentation:** Multiple apps managing separate user data create inconsistency and complicate session management.
- **Unified Workflow:** Centralizing authentication improves developer experience and enables consistent policies.

## Solution Overview

- **Separate Authentication Service:** Move login and session management out of individual apps into a dedicated service.
- **Technology Choice:** Built using OpenIddict, a robust .NET library for OpenID Connect and OAuth2.0 flows.

## Key Design Considerations

- **Unified User Store:** Single database for identities, reducing duplication and streamlining management.
- **OpenID Connect Flows:** Implement secure, modern authentication protocols.
- **Session Handling:** Prevent session fragmentation with centralized token issuance.

## Integration Best Practices

- **App Integration:** Use standard protocols for connecting new and existing apps to the SSO service.
- **Scalability:** Architect for growth; design with maintainability in mind.
- **Developer Experience:** Standardize integrations to simplify onboarding and troubleshooting.

## Blueprint Steps

1. **Assess Requirements:** List project constraints and platform compatibility.
2. **Design SSO Architecture:** Choose OpenIddict, plan deployment topology.
3. **Implement Central Auth Service:** Develop login, token, and user workflow endpoints.
4. **Integrate Applications:** Update apps to delegate authentication to new SSO server.
5. **Test and Document:** Validate flows, write integration guides.

## When to Use This Approach

- Projects unable to adopt managed providers (like Entra ID)
- Legacy app modernization
- Organizations needing full control over authentication logic

## Further Resources

- Watch the full session and other .NET Conf 2025 talks for more details: [https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXtIlvq1tuORUtZqVG-HdCt](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXtIlvq1tuORUtZqVG-HdCt)

---
Session by Dustin Kingen
