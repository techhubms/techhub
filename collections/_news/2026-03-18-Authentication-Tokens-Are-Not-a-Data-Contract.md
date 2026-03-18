---
title: Authentication Tokens Are Not a Data Contract
section_names:
- devops
- security
external_url: https://devblogs.microsoft.com/devops/authentication-tokens-are-not-a-data-contract/
primary_section: devops
author: Angel Wong
feed_name: Microsoft DevOps Blog
date: 2026-03-18 18:46:49 +00:00
tags:
- Access Tokens
- API Contracts
- Authentication
- Authentication Tokens
- Authorization
- Azure & Cloud
- Azure DevOps
- Azure DevOps REST API
- Breaking Changes
- Claims Based Identity
- DevOps
- News
- Opaque Tokens
- REST API
- Security
- Token Claims
- Token Encryption
---

Angel Wong warns that authentication tokens (including Azure DevOps scenarios) should not be treated as a stable data contract: token claims were never guaranteed, and additional encryption rolling out this summer will make token payloads unreadable, breaking apps that decode claims instead of using supported APIs.<!--excerpt_end-->

# Authentication Tokens Are Not a Data Contract

Authentication tokens exist to answer one question: *is this caller authorized to do this?*

They are not intended to be a stable data interface, a schema you can depend on, or an input into application logic.

If your application decodes tokens and reads claims from them, this is an important heads-up.

## Token Claims Were Never Guaranteed

Although tokens may appear readable today, that was never a promise.

- Microsoft has never publicly documented Azure DevOps token contents.
- Because token contents were not documented, token claims could always change at any time, for any reason.

Claims may:

- change
- become optional
- be renamed
- be removed
- stop being readable altogether

Relying on decoded token contents may work today, but it has always been an unsupported and fragile pattern across the industry.

## What’s Changing

**Coming this summer, Azure DevOps will further encrypt authentication tokens.** In some scenarios, these changes may take effect earlier as token formats continue to evolve.

Impact:

- Token payloads will no longer be readable by clients.
- Any application that depends on decoding tokens to extract claims will break.

Non-impact:

- Applications that already treat tokens as opaque will not be impacted.

## What to Do Instead

Use tokens only for validation and authorization. After validating a token, rely on supported APIs for data.

Recommended approach:

- Use tokens to prove who the caller is and what they’re allowed to do.
- Use supported APIs when you need actual data.
- Assume any token claim may change or disappear without notice.

Specifically, use the supported Azure DevOps REST APIs to retrieve user or organization data:

- https://learn.microsoft.com/en-us/rest/api/azure/devops/?view=azure-devops-rest-7.2

Those APIs provide stable contracts, documentation, and clearer expectations around change. Token claims do not.

If you find yourself decoding tokens to read values, that logic belongs elsewhere.

## Final Reminder

If your application depends on decoded token claims, consider this a warning to move off that pattern now—especially before encryption is enforced this summer.

Authentication tokens are for authentication and authorization, not data access. Treat them as opaque, and use supported APIs instead.


[Read the entire article](https://devblogs.microsoft.com/devops/authentication-tokens-are-not-a-data-contract/)

