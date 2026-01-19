---
external_url: https://dellenny.com/shared-responsibility-model-in-cloud-computing-simplified/
title: Shared Responsibility Model in Cloud Computing Simplified
author: Dellenny
viewing_mode: external
feed_name: Dellenny's Blog
date: 2025-10-27 06:41:21 +00:00
tags:
- Azure App Service
- Azure Key Vault
- Azure Security
- Azure Security Center
- Azure SQL Database
- Cloud Security
- Compliance
- Data Encryption
- Defender For Cloud
- GDPR
- HIPAA
- IaaS
- Microsoft 365
- 'Microsoft 365 Certified: Fundamentals'
- Microsoft Azure
- PaaS
- RBAC
- Role Based Access Control
- SaaS
- Security Best Practices
- Shared Responsibility Model
section_names:
- azure
- security
---
Dellenny offers a straightforward breakdown of the Shared Responsibility Model in Microsoft Azure, clarifying which security tasks belong to Microsoft and which to cloud customers.<!--excerpt_end-->

# Shared Responsibility Model in Cloud Computing Simplified

Author: Dellenny

Organizations moving workloads to the Microsoft Azure cloud need clarity on how security responsibilities are divided. The Shared Responsibility Model provides a framework for understanding which aspects of security are managed by Microsoft and which fall to the customer.

## What Is the Shared Responsibility Model in Azure?

- **On-premises**: Your IT team manages everything, including hardware, OS, applications, and security controls.
- **In Azure**: Microsoft and customers share security responsibilities.
  - **Microsoft** secures the underlying infrastructure: physical datacenters, networking, and core services.
  - **Customers** are responsible for securing what they deploy—data, applications, access controls, and configurations.

## How Responsibilities Are Divided

The split can change depending on the Azure service type:

### 1. Infrastructure as a Service (IaaS)

- Microsoft manages: Physical hosts, networking, datacenter security.
- Customer manages: Virtual machines, operating systems, apps, network settings, data.
- **Example**: With Azure Virtual Machines, the user is responsible for OS patching, firewall setup, and identity management.

### 2. Platform as a Service (PaaS)

- Microsoft manages: Infrastructure, runtime, platform updates.
- Customer manages: App logic, code, and data access.
- **Example**: For Azure App Service or Azure SQL Database, Microsoft secures the platform, while users secure data and permissions.

### 3. Software as a Service (SaaS)

- Microsoft manages: Entire stack from infrastructure to application.
- Customer manages: Data integrity, user access, device security.
- **Example**: In Microsoft 365, Microsoft provides a secure service, but customers must define user, sharing, and governance settings.

## Importance for Azure Users

Misunderstanding this model can lead to security problems—most often from misconfiguration on the customer side, not infrastructure failure. Key reasons to understand your role:

- Strengthen the security posture
- Stay compliant with regulations (GDPR, HIPAA)
- Reduce risk from configuration or identity errors
- Make audits and incident response smoother by clarifying responsibilities

## Azure Security Best Practices

- **Use Azure Security Center (Defender for Cloud)** for continuous monitoring
- **Enable encryption** for data in transit and at rest with Azure Key Vault
- **Implement RBAC** for least-privilege account permissions
- **Enable MFA** (multifactor authentication) for all users, especially admins
- **Leverage Azure Policy** to automate compliance and enforce security standards

## Conclusion

The Shared Responsibility Model in Microsoft Azure highlights the partnership between provider and customer. For cloud security, Microsoft takes care of the platform; customers secure what they build and manage inside Azure.

## Further Reading

- [Shared Responsibility Model in Cloud Computing Simplified](https://dellenny.com/shared-responsibility-model-in-cloud-computing-simplified/)

---

*Originally published by Dellenny*

This post appeared first on "Dellenny's Blog". [Read the entire article here](https://dellenny.com/shared-responsibility-model-in-cloud-computing-simplified/)
