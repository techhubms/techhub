---
external_url: https://devblogs.microsoft.com/azure-sdk/announcing-the-end-of-support-for-node-js-18-x-in-the-azure-sdk-for-javascript/
title: Azure SDK for JavaScript Ending Support for Node.js 18.x After July 2025
author: Maor Leger
feed_name: Microsoft DevBlog
date: 2025-02-11 22:10:32 +00:00
tags:
- Azure SDK
- JavaScript
- LTS
- Node.js
- Node.js 18.x
- Node.js 20.x
- Nodejs
- npm Engines
- SDK Support Policy
- Typescript
section_names:
- azure
- coding
primary_section: coding
---
Authored by Maor Leger, this article details the timeline and implications of ending Node.js 18.x support in the Azure SDK for JavaScript, providing guidance on upgrading and support policies.<!--excerpt_end-->

## Azure SDK for JavaScript Ending Support for Node.js 18.x

**Author:** Maor Leger

### Overview

After July 10, 2025, the [Azure SDK for JavaScript](https://github.com/Azure/azure-sdk-for-js) will no longer support Node.js 18.x, as Node.js 18.x reaches its end-of-life on April 30, 2025. Users are strongly advised to [upgrade to an Active Node.js Long Term Support (LTS) version](https://nodejs.org/en/download) to guarantee access to the latest features and security fixes.

---

### Why End Support for Node.js 18.x?

Node.js uses a well-defined release schedule, granting each even-numbered version an Active LTS period, followed by Maintenance LTS and eventual end-of-life status. After leaving Maintenance, a Node.js version no longer gets bug fixes or security patches. To maintain secure and up-to-date libraries, the Azure SDK team regularly drops support for Node.js versions that reach end-of-life. According to the [Azure SDK for JavaScript support policy](https://github.com/Azure/azure-sdk-for-js/blob/main/SUPPORT.md#microsoft-support-policy), this change can occur without a major version increment.

---

### What Happens on July 10, 2025?

- The Azure SDK for JavaScript will set Node.js 20.x as the minimum supported version in its [engines field](https://docs.npmjs.com/cli/v11/configuring-npm/package-json#engines).
- Users running Node.js 18.x will see deprecation warnings when installing newer SDK versions. If `engine-strict=true` is enabled, the installation will fail with an npm error.
- Even though new versions of the SDK will not officially support Node.js 18.x, library versions released without Node.js 18.x support may still function on that version. However, continued use is not recommended as no support, security, or feature updates will be provided.
- Users can continue to use older versions of the libraries with Node.js 18.x but are strongly encouraged to upgrade.

---

### Azure SDK Support Policies

- See the full [Azure SDK support policy](https://azure.github.io/azure-sdk/policies_support.html#azure-sdk-dependencies).
- Review the [Azure SDK for JavaScript support policy](https://github.com/Azure/azure-sdk-for-js/blob/main/SUPPORT.md#microsoft-support-policy) for details on maintenance and support procedures.

---

### Recommendations

To avoid disruption and maintain security compliance:

- Upgrade your environments to the latest Active Node.js LTS version as soon as possible.
- [Download the latest LTS version](https://nodejs.org) from the Node.js website.

Staying current ensures continued support, access to new features, and the security of your JavaScript applications using Azure SDK libraries.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/announcing-the-end-of-support-for-node-js-18-x-in-the-azure-sdk-for-javascript/)
