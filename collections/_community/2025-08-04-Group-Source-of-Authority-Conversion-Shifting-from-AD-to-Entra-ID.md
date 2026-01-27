---
external_url: https://www.reddit.com/r/AZURE/comments/1mhees0/group_source_of_authority_conversion/
title: 'Group Source of Authority Conversion: Shifting from AD to Entra ID'
author: JohnSavill
feed_name: Reddit Azure
date: 2025-08-04 14:06:29 +00:00
tags:
- Active Directory
- Cloud Identity
- Entra Capabilities
- Entra ID
- Graph Explorer
- Group Management
- Group Writeback
- Hybrid Identity
- Identity Governance
- Mail Enabled Groups
- SOA Conversion
section_names:
- azure
- security
primary_section: azure
---
In this community contribution, JohnSavill discusses group source of authority conversion from Active Directory to Entra ID, offering practical steps and critical considerations for a successful identity shift.<!--excerpt_end-->

# Group Source of Authority Conversion: Shifting from AD to Entra ID

**Author:** JohnSavill

Nearly every organization today uses a hybrid identity environment that integrates on-premises Active Directory (AD) with Entra ID (formerly Azure AD). As organizations migrate more functions to the cloud, the emphasis is increasingly placed on Entra ID due to its modern features and identity management capabilities. A significant milestone in this journey is the ability to convert the source of authority (SOA) for groups—from AD to Entra ID.

## Key Sections Covered

- **Active Directory as the Initial SOA:** The traditional model where groups are authored and managed on-premises through AD, forming the basis for synchronization with cloud identities.
- **Entra ID Overview:** A summary of Entra ID's advanced capabilities for managing identities and enabling modern security scenarios not available in legacy AD.
- **Useful Entra Capabilities for Groups:** Highlights features such as dynamic group membership, granular access policies, and integrated governance.
- **Shift to the Cloud:** Explains the strategic shift organizations are making towards a cloud-first identity model, driven by Entra’s strengths.
- **Group Writeback Scenarios:** Reviews how cloud-authored groups can be written back to on-prem AD for backward compatibility or specific business needs.
- **Mail-enabled Group Considerations:** Discusses special scenarios for groups used in mail routing, including additional planning to ensure mail flow is not interrupted during SOA shifts.
- **Shifting the SOA:** Details the procedural steps for changing a group’s SOA, including pre-requisites and risk assessments.
- **Planning for SOA Changes:** Guidance on preparing the environment, understanding dependencies, and communication strategies when planning SOA migration.
- **Changing SOA Using Graph Explorer:** Technical walkthrough on how to use Graph Explorer for implementing SOA changes, with step-by-step command examples.
- **Post-Change Steps:** Covers the necessary actions after converting a group’s SOA, such as updating policies, verifying membership, and syncing settings.
- **Identity Governance and Management Shifts:** Emphasizes the enhanced control and visibility realized after migrating SOA, unlocking Entra ID’s full feature set for group management.
- **User Considerations:** Addresses the downstream impact for users, ensuring they maintain access and are protected during and after the transition.

## Key Takeaways

- Migrating the SOA from AD to Entra ID is a pivotal move for organizations seeking modern identity solutions.
- Planning and careful execution are essential, particularly for mail-enabled or business-critical groups.
- Tools like Graph Explorer simplify the technical implementation but require proper understanding and permissions.
- Successful SOA change enables full use of Entra ID’s cloud-native management, dynamic access, and governance features.

For a detailed walkthrough, refer to the YouTube guide by JohnSavill: [Group Source of Authority Conversion!](https://youtu.be/VpRDtulXcUw).

*Submitted by JohnSavill via Reddit.*

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mhees0/group_source_of_authority_conversion/)
