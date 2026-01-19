---
external_url: https://blog.fabric.microsoft.com/en-US/blog/securely-access-vpc-protected-amazon-s3-buckets-in-microsoft-fabric-with-entra-integration-preview/
title: Securely Access VPC-Protected Amazon S3 Buckets in Microsoft Fabric Using Entra Integration
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-12-01 09:00:00 +00:00
tags:
- Amazon S3
- AWS IAM
- Cloud Security
- CloudTrail
- Cross Cloud Access
- Data Governance
- Identity Management
- Microsoft Entra ID
- Microsoft Fabric
- OIDC
- On Premises Data Gateway
- S3 Shortcut
- Service Principal
- VPC
- Zero Trust
section_names:
- azure
- ml
- security
---
Microsoft Fabric Blog describes how organizations can connect VPC-protected Amazon S3 buckets to Microsoft Fabric securely via Entra ID integration, using OIDC and an on-premises data gateway.<!--excerpt_end-->

# Secure Access to VPC-Protected Amazon S3 Buckets in Microsoft Fabric Using Entra Integration

Organizations often face challenges integrating cloud storage across platforms, especially when security policies restrict resources inside Virtual Private Clouds (VPCs) or behind firewalls. This guide demonstrates how Microsoft Fabric, combined with Microsoft Entra ID and OpenID Connect (OIDC), enables secure, identity-based connectivity to Amazon S3 buckets—bypassing the need to manage AWS access keys and maintaining strict network boundaries.

## Overview

- **Entra ID-Based Identity Authentication**: Uses service principal registration in Microsoft Entra ID to establish OIDC trust with AWS IAM, granting S3 access without long-lived keys.
- **Network Access via Data Gateway**: The on-premises data gateway allows Microsoft Fabric to reach S3 buckets within private networks or VPCs, keeping traffic internal and secure.

## Integration Steps

### 1. Configure Entra–AWS Trust for S3 Shortcuts

- Register service principal in Microsoft Entra ID.
- Set up OIDC trust relationship between Entra and AWS IAM.
- Grant necessary S3 permissions using IAM role and policy.
- [Full guide here](https://blog.fabric.microsoft.com/blog/24780?utm_source=chatgpt.com).

### 2. Enable Private Network Access

- Configure VPC endpoint or network path to S3.
- Install and configure the on-premises data gateway.
- Verify private connectivity between gateway and S3.
- [Walkthrough: Creating a shortcut to a VPC-protected Amazon S3 bucket](https://blog.fabric.microsoft.com/blog/creating-a-shortcut-to-a-vpc-protected-amazon-s3-bucket).

### 3. Create Fabric Shortcut for S3

- Use Entra service principal integration in Fabric.
- Select the connection and on-premises data gateway when creating the shortcut.
- Complete shortcut setup for the protected bucket or folder.

## Security Best Practices

- Use distinct service principals per AWS IAM role for isolation and auditability.
- Rotate and securely store service principal secrets.
- Monitor AWS CloudTrail logs for role assumption and STS activities.

## Current Limitations

- Currently limited to service-principal authentication; OAuth and workspace identity support not yet available.

## Summary

Using this integration, Microsoft Fabric delivers unified analytics across S3 data contained within protected network boundaries, with secure identity management and streamlined governance.

- [Get started guide](https://learn.microsoft.com/fabric/onelake/amazon-storage-shortcut-entra-integration)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/securely-access-vpc-protected-amazon-s3-buckets-in-microsoft-fabric-with-entra-integration-preview/)
