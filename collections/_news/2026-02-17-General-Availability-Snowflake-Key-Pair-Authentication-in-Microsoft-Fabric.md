---
layout: "post"
title: "General Availability: Snowflake Key-Pair Authentication in Microsoft Fabric"
description: "This article announces the general availability of Key-Pair authentication for Snowflake connections within Microsoft Fabric. It explains how this enhanced security feature benefits Power BI semantic models, Dataflow Gen2, Data pipelines, Copy jobs, and Mirroring. The article provides step-by-step instructions for configuring Key-Pair authentication via the Microsoft Fabric portal and REST API, emphasizing compliance and secure practices."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/snowflake-key-pair-authentication-generally-available/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-02-17 10:00:00 +00:00
permalink: "/2026-02-17-General-Availability-Snowflake-Key-Pair-Authentication-in-Microsoft-Fabric.html"
categories: ["Azure", "Security"]
tags: ["Automated Workloads", "Azure", "Compliance", "Data Integration", "Data Pipelines", "Data Security", "Dataflow Gen2", "ECDSA", "Identity Management", "Key Pair Authentication", "Microsoft Fabric", "News", "Power BI", "REST API", "RSA", "Security", "Snowflake"]
tags_normalized: ["automated workloads", "azure", "compliance", "data integration", "data pipelines", "data security", "dataflow gen2", "ecdsa", "identity management", "key pair authentication", "microsoft fabric", "news", "power bi", "rest api", "rsa", "security", "snowflake"]
---

Microsoft Fabric Blog, coauthored by Abhishek Narain, details the general availability of Key-Pair authentication for Snowflake connections—outlining secure setup in Fabric, REST API options, and benefits for enterprise compliance.<!--excerpt_end-->

# General Availability: Snowflake Key-Pair Authentication in Microsoft Fabric

*Coauthor: Abhishek Narain*

Ensuring secure connectivity to your data sources is increasingly critical for modern data estates. Microsoft Fabric has officially released Key-Pair authentication for Snowflake connections, moving it from preview to general availability (GA).

## Why Key-Pair Authentication Matters

- **Enhanced Security:** Key-Pair authentication provides passwordless login using RSA or ECDSA cryptographic keys, removing the risks associated with passwords. It supports encrypted private keys for stronger protection.
- **Enterprise Compliance:** The approach is ideal for service accounts and automated workflows. Snowflake's roadmap deprecates single-factor passwords in favor of more secure authentication.

Key-Pair authentication is now fully supported in:

- Power BI Semantic models
- Dataflow Gen2
- Data pipelines
- Copy jobs
- Mirroring

## Setting Up Key-Pair Authentication

### Using the Microsoft Fabric Portal

1. Go to the **Get Data** wizard or **Manage connections and gateways** settings.
2. Create or edit a Snowflake connection.
3. In authentication settings, select **Key-Pair** as the method.
4. Input your Username and Private Key. (A passphrase is only required if the private key file is encrypted.)

![Snowflake KeyPair Authentication with encrypted private key](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/02/image-8-1024x602.png)

### Using the REST API

For automated environments, you can now create Snowflake connections with Key-Pair credentials by leveraging the Fabric Connections REST API.

- See the [Connections – Create Connection documentation](https://learn.microsoft.com/rest/api/fabric/core/connections/create-connection?tabs=HTTP#keyvaultsecretreference) for further details.

## Learn More

- [How to setup Snowflake Key Pair authentication in Microsoft Fabric](https://learn.microsoft.com/fabric/data-factory/connector-snowflake#key-pair-authentication)

This upgrade helps organizations align with industry best practices for data security while leveraging Fabric's broad data integration capabilities.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/snowflake-key-pair-authentication-generally-available/)
