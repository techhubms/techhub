---
external_url: https://devblogs.microsoft.com/devops/announcing-the-new-azure-devops-server-rc-release/
title: 'Azure DevOps Server RC Release: New Features, Lifecycle Policy, and Branding Updates'
author: Gloridel Morales
feed_name: Microsoft DevOps Blog
date: 2025-10-07 20:14:45 +00:00
tags:
- Azure DevOps Server
- Continuous Delivery
- Microsoft Azure
- Modern Lifecycle Policy
- Product Branding
- Release Candidate
- REST API
- Security Updates
- Software Licensing
- Team Foundation Server
- Test Plan Recovery
- TFX Validation
- Upgrade Path
- Azure
- DevOps
- News
section_names:
- azure
- devops
primary_section: azure
---
Gloridel Morales shares an overview of the Azure DevOps Server RC release, detailing new features, lifecycle policy changes, and the shift to continuous updates under Microsoft's branding strategy.<!--excerpt_end-->

# Announcing the Azure DevOps Server RC Release

**Author:** Gloridel Morales  

The Azure DevOps team is releasing the new Azure DevOps Server release candidate (RC), which aligns feature parity with the hosted (cloud) version and introduces significant changes for on-premises users.

## Key Highlights

- **Feature Additions:** New features from the hosted Azure DevOps platform, such as:
  - TFX validates whether a task uses an End of Life Node runner
  - REST API support for restoring deleted test plans and test suites
- **Upgrade Path:** Direct upgrades from any version of Team Foundation Server (TFS 2015 and newer) are supported.
- **Lifecycle Policy Transition:** This version moves Azure DevOps Server into Microsoft’s Modern Lifecycle Policy for more flexible, continuous support—dropping the previous fixed end-of-support dates.
- **Product Branding:** The release removes the year from the product name. Going forward, the product is simply "Azure DevOps Server," consistent with Microsoft’s continuous delivery approach. Support teams will identify versions by file version number rather than the year.
- **End of Support Notices:** TFS 2015 extended support ends on October 14, 2025. Organizations are encouraged to upgrade to stay protected and supported.

## What to Know About the RC Release

- **Trying the RC:** The RC can be tested in a lab environment. Direct upgrades from this RC to the future RTM release will be supported, ensuring a smooth transition for early adopters.
- **Security and Support:** Updates will continue for supported versions until their official end of extended support. If retirement is planned, Microsoft will provide three years' notice.
- **Licensing:** Customers can choose between monthly Azure or authorized resellers for licensing. Visual Studio subscribers get access to server and user licenses.
- **Cloud Migration:** Staying updated with the latest version of the server product will make future migrations to Azure DevOps Services easier and more efficient.

## Frequently Asked Questions

- **Q:** What’s changing with this release?
  - **A:** The product adopts Microsoft’s Modern Lifecycle Policy, eliminating major version releases and releasing updates more frequently to an ongoing version.

- **Q:** Will I still get updates for older versions?
  - **A:** Security updates continue for older versions until their fixed end-of-support dates.

- **Q:** Does the removal of the year from the branding affect support?
  - **A:** No; support teams will rely on file version numbers for identification.

- **Q:** How does this affect licensing?
  - **A:** Licensing options remain the same, and Visual Studio subscribers receive associated server and user licenses each month.

- **Q:** Can I migrate to Azure DevOps Services later?
  - **A:** Yes, using the latest server version eases migration to the cloud-hosted service.

## Additional Resources

- [Download Azure DevOps Server RC](https://learn.microsoft.com/azure/devops/server/download/azuredevopsserver?view=azure-devops#download-the-latest-release)
- [Release Notes](https://learn.microsoft.com/azure/devops/server/release-notes/azuredevopsserver?view=azure-devops)
- [Modern Lifecycle Policy](https://aka.ms/modernlifecyclepolicy)
- [Modern Support Lifecycle Commitments](https://learn.microsoft.com/lifecycle/policies/3-year-subset)
- [Azure DevOps Server Pricing](https://azure.microsoft.com/pricing/details/devops/server/?msockid=33e827d02f2c65b624e432212eb064f9)
- [Developer Community](https://developercommunity.visualstudio.com/AzureDevOpsServerTFS)

Organizations and developers are encouraged to explore the RC, utilize new features, and prepare for the shift to a more agile, continuously updated Azure DevOps Server platform.

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/announcing-the-new-azure-devops-server-rc-release/)
