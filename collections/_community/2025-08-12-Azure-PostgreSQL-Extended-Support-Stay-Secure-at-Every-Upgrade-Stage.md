---
external_url: https://techcommunity.microsoft.com/t5/azure-database-for-postgresql/azure-postgresql-extended-support-stay-secure-at-every-stage-of/ba-p/4442283
title: 'Azure PostgreSQL Extended Support: Stay Secure at Every Upgrade Stage'
author: andreatapia
feed_name: Microsoft Tech Community
date: 2025-08-12 16:17:14 +00:00
tags:
- Azure Database For PostgreSQL
- Business Continuity
- Cloud Database Management
- Critical Workloads
- Database Lifecycle
- Database Security
- Downtime Mitigation
- Enterprise Workloads
- Extended Support
- Microsoft Azure
- PostgreSQL Upgrades
- Regulatory Compliance
- Security Patches
- Technical Support
- Version Retirement
- Azure
- Security
- Community
section_names:
- azure
- security
primary_section: azure
---
andreatapia presents a detailed breakdown of Azure Database for PostgreSQL Extended Support, explaining how organizations can keep databases secure when older PostgreSQL versions reach end-of-life.<!--excerpt_end-->

# Azure PostgreSQL Extended Support: Stay Secure at Every Upgrade Stage

**Author:** andreatapia  
**Date:** Updated Aug 12, 2025

## Overview

Upgrading PostgreSQL for mission-critical workloads is challenging, especially when operating under tight timelines or regulatory requirements. Microsoft now offers **Extended Support for Azure Database for PostgreSQL** to bridge the support gap once your database's PostgreSQL version reaches its official community end-of-life (EOL).

## What is Azure PostgreSQL Extended Support?

- **Automatic enrollment**: Once community support ends for your PostgreSQL version, Azure automatically enrolls eligible servers in Extended Support.
- **Duration**: Provides up to three additional years of security patches and critical bug fixes beyond the community EOL.
- **Exit strategy**: Automatic exit occurs once you upgrade to a supported version or after the three-year support window ends.
- **Support coverage**: All customers retain access to technical support through their existing Azure support plan.

## Who Needs Extended Support?

This offering is intended for organizations that:

- Require additional time to test and validate new database versions
- Must maintain compliance in regulated industries
- Want to avoid service downtime and ensure business continuity
- Cannot upgrade immediately following community EOL for PostgreSQL

## Recommendations for Using Extended Support Wisely

1. **Plan early:** Review and evaluate PostgreSQL versions 11, 12, 13, and upcoming 17, which brings in-place upgrade capabilities ([learn more](https://techcommunity.microsoft.com/blog/adforpostgresql/postgresql-17-general-availability-with-in-place-upgrade-support/4427284)).
2. **Test upgrades:** Use the additional time to minimize transition risks and validate critical workloads.
3. **Upgrade before support ends:** Reference [Microsoft's upgrade documentation](https://learn.microsoft.com/en-us/azure/postgresql/flexible-server/concepts-major-version-upgrade) to ensure a smooth transition.

## Why Extended Support Matters

- **Security**: Your workloads continue to receive the latest security patches and bug fixes from Microsoft, reducing the risk of vulnerabilities.
- **Continuity**: Avoid forced downtime and maintain access to Microsoft technical support.
- **Compliance**: Meet regulatory and industry mandates for supported and patched software.

## Timeline & Pricing

- Extended Support becomes a **paid feature starting April 2026**.
- **Automatic enrollment** for eligible servers according to these retirement dates:

| PostgreSQL Version | Community Retirement | Azure Standard Support End | Paid Extended Support Start | Paid Extended Support End |
| --- | --- | --- | --- | --- |
| 11  | Nov 9, 2025  | Mar 31, 2026 | Apr 1, 2026  | Mar 31, 2029 |
| 12  | Nov 14, 2024 | Mar 31, 2026 | Apr 1, 2026  | Mar 31, 2029 |
| 13  | Nov 13, 2025 | Mar 31, 2026 | Apr 1, 2026  | Mar 31, 2029 |
| 14  | Nov 12, 2026 | Dec 11, 2026 | Dec 12, 2026 | Dec 11, 2029 |

- Pricing will be announced in Q3 2025.

## Additional Resources

- [Extended Support FAQs](https://learn.microsoft.com/en-us/azure/postgresql/flexible-server/concepts-version-policy)
- [Upgrade Documentation](https://learn.microsoft.com/en-us/azure/postgresql/flexible-server/concepts-major-version-upgrade)

## Summary

Azure Database for PostgreSQL Extended Support provides a structured, secure, and supported way to maintain legacy database workloads as you plan and execute upgrades. It is a temporary solution to help you reduce risk, maintain compliance, and retain technical coverage after community support concludes.

---
For further updates and support, follow the [Azure Database for PostgreSQL blog](https://techcommunity.microsoft.com/t5/azure-database-for-postgresql/bg-p/AzureDatabaseforPostgreSQL) and subscribe for notifications.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-database-for-postgresql/azure-postgresql-extended-support-stay-secure-at-every-stage-of/ba-p/4442283)
