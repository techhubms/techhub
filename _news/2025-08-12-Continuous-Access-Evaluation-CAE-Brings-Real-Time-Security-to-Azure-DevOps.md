---
layout: "post"
title: "Continuous Access Evaluation (CAE) Brings Real-Time Security to Azure DevOps"
description: "This news release announces the integration of Continuous Access Evaluation (CAE) with Azure DevOps. CAE, provided by Microsoft Entra ID, enables near real-time enforcement of Conditional Access policies, minimizing the exposure window after critical security events. Developers using the .NET client library should update their apps to handle CAE-specific token rejections by implementing claims challenge logic. Additional support for Python and Go is forthcoming."
author: "Angel Wong"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/devops/real-time-security-with-continuous-access-evaluation-cae-comes-to-azure-devops/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/devops/feed/"
date: 2025-08-12 14:39:05 +00:00
permalink: "/2025-08-12-Continuous-Access-Evaluation-CAE-Brings-Real-Time-Security-to-Azure-DevOps.html"
categories: ["Azure", "Coding", "DevOps", "Security"]
tags: [".NET Client Library", "Azure", "Azure & Cloud", "Azure DevOps", "Claims Challenge", "Coding", "Conditional Access", "Continuous Access Evaluation", "DevOps", "Identity Management", "Incident Response", "MFA", "Microsoft Entra ID", "News", "Real Time Security", "Security", "Security Enforcement", "Token Revocation"]
tags_normalized: ["dotnet client library", "azure", "azure and cloud", "azure devops", "claims challenge", "coding", "conditional access", "continuous access evaluation", "devops", "identity management", "incident response", "mfa", "microsoft entra id", "news", "real time security", "security", "security enforcement", "token revocation"]
---

Angel Wong introduces support for Continuous Access Evaluation (CAE) on Azure DevOps, discussing its impact on real-time security and the implications for developers using Microsoft Entra ID.<!--excerpt_end-->

# Real-Time Security with Continuous Access Evaluation (CAE) comes to Azure DevOps

**Author:** Angel Wong

Continuous Access Evaluation (CAE) is now available on Azure DevOps, enhancing the platform’s security by enabling near real-time enforcement of Conditional Access policies using Microsoft Entra ID.

## What Is CAE?

CAE is a feature of Microsoft Entra ID designed to quickly enforce Conditional Access (CA) policies in near real time. Traditionally, access tokens issued by Microsoft Entra for Azure DevOps were valid for up to an hour, allowing continued access even after critical account changes like password resets or user disablement. With CAE, access can be revoked almost immediately after events such as:

- User deletion or disablement
- Password changes or resets
- Admin-triggered token revocations
- Multi-factor Authentication (MFA) enablement
- IP/location changes

CAE operates by establishing a two-way communication between Microsoft Entra and Azure DevOps, providing enforcement at the time of access instead of just at token issuance. This approach significantly reduces the window of exposure for compromised accounts or policy violations, strengthening overall security and improving incident response times.

For further details and technical implementation, see [Microsoft Entra documentation on CAE](https://learn.microsoft.com/en-us/entra/identity/conditional-access/concept-continuous-access-evaluation).

## What’s Changing for Developers?

Developers using the latest [.NET client library](https://www.nuget.org/packages/Microsoft.TeamFoundationServer.Client/20.259.0-preview) must update their applications to handle CAE-specific scenarios. When a token is rejected under CAE, the service will respond with a 401 Unauthorized error that includes a claims challenge. Apps should be designed to extract this claims challenge, obtain a fresh token, and retry the request. Support for CAE in Python and Go client libraries will be available by the end of 2025.

To learn more about the claims challenge flow, refer to the [Microsoft Entra documentation on claims challenges](https://learn.microsoft.com/entra/identity-platform/claims-challenge?tabs=dotnet). Upcoming blog updates will include example code for handling these scenarios with the .NET client library.

For questions or feedback, readers are encouraged to comment on the official blog announcement.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/real-time-security-with-continuous-access-evaluation-cae-comes-to-azure-devops/)
