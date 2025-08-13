---
layout: "post"
title: "Issuing Custom Claims Using Directory Extension Attributes in Microsoft Entra ID"
description: "This community guide by Farooque explains how to issue custom claims in Microsoft Entra ID using directory extension attributes. It details the process of registering attributes via Microsoft Graph, assigning them to users, and configuring claims in Enterprise Applications based on group membership. The guide also includes troubleshooting common claim mapping errors and step-by-step instructions for testing the configuration, allowing organizations to implement dynamic and conditional SSO claims tailored to business needs."
author: "Farooque"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/issuing-custom-claims-using-directory-extension-attributes-in/ba-p/4441980"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-11 05:54:53 +00:00
permalink: "/2025-08-11-Issuing-Custom-Claims-Using-Directory-Extension-Attributes-in-Microsoft-Entra-ID.html"
categories: ["Security"]
tags: ["Acceptmappedclaims", "Azure Active Directory", "Claims Issuance", "Cloud Identity", "Community", "Conditional Access", "Directory Extension Attributes", "Enterprise Applications", "Group Based Claims", "JWT", "Microsoft Entra ID", "Microsoft Graph API", "OIDC", "SAML", "Security", "Single Sign On", "User Attributes"]
tags_normalized: ["acceptmappedclaims", "azure active directory", "claims issuance", "cloud identity", "community", "conditional access", "directory extension attributes", "enterprise applications", "group based claims", "jwt", "microsoft entra id", "microsoft graph api", "oidc", "saml", "security", "single sign on", "user attributes"]
---

Farooque shares a practical walkthrough on issuing custom SSO claims in Microsoft Entra ID by leveraging directory extension attributes and group-based conditions. The article covers attribute registration, claim configuration, and troubleshooting.<!--excerpt_end-->

# Issuing Custom Claims Using Directory Extension Attributes in Microsoft Entra ID

## Overview

Organizations often need to pass custom user data (like internal IDs or sponsorship info) to applications during SSO. Microsoft Entra ID allows for this with directory extension attributes, which can be configured to issue claims conditionally (e.g., based on group membership).

This guide walks through the process of registering such attributes, assigning them to users, and setting up claims in Microsoft Entra ID Enterprise Applications.

## Step 1: Register Directory Extension Attributes

- Use Microsoft Graph Explorer to register extension properties (e.g., `sponsorid1`, `sponsorid2`) for your target application.
- **POST** to `https://graph.microsoft.com/v1.0/applications/{AppObjectId}/extensionProperties`
- **Request body example:**

  ```json
  { "name": "sponsorid1", "dataType": "String", "targetObjects": ["User"] }
  ```

- Repeat for each custom attribute needed.
- The API returns attribute names formatted as `extension_<AppClientID>_sponsorid1`.
- Record these names for later steps.

## Step 2: Assign Extension Attributes to Users

- Use Graph Explorer to update user objects and assign extension attributes.
- **PATCH** to `https://graph.microsoft.com/v1.0/users/{UserObjectId}`
- **Request body example:**

  ```json
  { "extension_<AppClientID>_sponsorid1": "ABC123" }
  ```

- Assign the corresponding attribute values per user as needed.

## Step 3: Create Conditional Claims in Enterprise Application

- In Microsoft Entra ID, go to **Enterprise Applications** > *[App Name]* > **Single Sign-On** > **Attributes & Claims**.
  1. Add a new claim (e.g., `sponsorClaim1`).
  2. Set claim conditions by selecting relevant user groups.
  3. Set the source attribute to the proper directory extension attribute (like `extension_<AppClientID>_sponsorid1`).
- Repeat for each custom claim/group required.

## Step 4: Handle Claim Mapping Errors

- If you encounter "Application requires custom signing key to customize claims":
  - In the app registration manifest, set:

    ```json
    "acceptMappedClaims": true
    ```

  - This permits claim customization without needing a custom signing key.

## Step 5: Test Your Configuration

- Initiate authentication using the application (e.g., via an OpenID Connect authorize URL).
- Log in with users from the defined groups.
- Inspect the resulting token at [https://jwt.ms](https://jwt.ms) to confirm expected custom claims (e.g., `sponsorid1` or `sponsorid2`) show up only for authorized users.
- Users outside the designated groups should not receive sponsor claims.

## Conclusion

Directory extension attributes in Microsoft Entra ID are a powerful method for delivering dynamic, conditional SSO claims. By combining these with group-based claim issuance, organizations can implement business logic-driven identity solutions tailored for complex enterprise requirements.

---
**Author:** Farooque

**Updated:** August 11, 2025

**Version:** 1.0

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/issuing-custom-claims-using-directory-extension-attributes-in/ba-p/4441980)
