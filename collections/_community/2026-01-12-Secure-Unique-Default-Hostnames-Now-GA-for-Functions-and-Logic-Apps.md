---
layout: post
title: Secure Unique Default Hostnames Now GA for Functions and Logic Apps
author: YutangLin
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/secure-unique-default-hostnames-now-ga-for-functions-and-logic/ba-p/4484237
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2026-01-12 15:00:00 +00:00
permalink: /azure/community/Secure-Unique-Default-Hostnames-Now-GA-for-Functions-and-Logic-Apps
tags:
- App Security
- Automation
- Azure
- Azure App Service
- Azure CLI
- Azure Functions
- Azure Logic Apps
- Best Practices
- Cloud Security
- Community
- DNS Hygiene
- Hostname Management
- Resource Provisioning
- Secure Default Hostnames
- Security
- Serverless
- Subdomain Takeover
- Web Apps
section_names:
- azure
- security
---
YutangLin outlines the GA release of Secure Unique Default Hostnames for Azure Functions and Logic Apps, highlighting key security enhancements and practical guidance for Azure cloud developers.<!--excerpt_end-->

# Secure Unique Default Hostnames Now GA for Functions and Logic Apps

Azure has announced the general availability (GA) of Secure Unique Default Hostnames for Azure Functions and Logic Apps (Standard). This update expands the security model—previously limited to Web Apps—to the full Azure App Service ecosystem, providing stronger, standardized hostname behavior and better protection against DNS-related risks.

## Why This Feature Matters

Previously, App Service resources used a default hostname format like `<SiteName>.azurewebsites.net`. This exposed applications to security risks, particularly if leftover DNS records could be hijacked after a resource was deleted. Attackers could create a new resource with the same name, potentially leading to issues like subdomain takeover.

Secure Unique Default Hostnames mitigate these risks by assigning each resource a unique, randomized, region-scoped hostname, for example `<SiteName>-<Hash>.<Region>.azurewebsites.net`. This ensures:

- No duplication of hostnames by other users
- Applications avoid risks associated with dangling DNS entries
- More secure, reliable baseline across App Service

## What's New: GA Support

- Azure Functions and Logic Apps (Standard) now support Secure Unique Default Hostnames
- All App Service workloads now have access to this enhanced security model

## Azure CLI Support

The Azure CLI for Web Apps and Function Apps now supports the `--domain-name-scope` parameter. This enables customers to specify the hostname uniqueness scope during resource creation:

```sh
az webapp create --domain-name-scope {NoReuse, ResourceGroupReuse, SubscriptionReuse, TenantReuse}
az functionapp create --domain-name-scope {NoReuse, ResourceGroupReuse, SubscriptionReuse, TenantReuse}
```

This helps teams standardize automation and CI/CD workflows to use secure unique hostnames.

## Why Adopt This Now

Customers are encouraged to use Secure Unique Default Hostnames for all new deployments because it:

- Provides a stronger security posture
- Prevents DNS and subdomain takeover scenarios
- Delivers consistency as App Service evolves
- Follows modern deployment best practices

## Recommended Next Steps

- Enable Secure Unique Default Hostnames for all new Web Apps, Function Apps, and Logic Apps
- Update automation/CLI scripts to use the `--domain-name-scope` parameter
- Update processes and documentation to reflect the new hostname model

## Additional Resources

- [Public Preview: Creating Web App with a Unique Default Hostname](https://techcommunity.microsoft.com/blog/appsonazureblog/public-preview-creating-web-app-with-a-unique-default-hostname/4156353)
- [Secure Unique Default Hostnames: GA on App Service Web Apps and Public Preview on Functions](https://techcommunity.microsoft.com/blog/appsonazureblog/public-preview-creating-web-app-with-a-unique-default-hostname/4156353)

## Conclusion

The adoption of Secure Unique Default Hostnames is now the best practice for deploying resources in Azure App Service, improving DNS security and aligning with Microsoft's recommended deployment patterns.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/secure-unique-default-hostnames-now-ga-for-functions-and-logic/ba-p/4484237)
