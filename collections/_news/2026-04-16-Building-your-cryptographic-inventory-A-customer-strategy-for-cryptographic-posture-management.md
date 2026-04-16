---
date: 2026-04-16 16:00:00 +00:00
section_names:
- azure
- devops
- security
primary_section: azure
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/16/building-your-cryptographic-inventory-a-customer-strategy-for-cryptographic-posture-management/
author: Aviram Shemesh and Jennifer Rutzer
feed_name: Microsoft Security Blog
tags:
- Azure
- Azure Key Vault
- Azure Network Watcher
- Certificate Inventory
- Cipher Suites
- Code Scanning
- CodeQL
- Compliance
- Crypto Agility
- Cryptographic Inventory
- Cryptographic Posture Management
- DevOps
- DORA
- GitHub Advanced Security
- HSM
- IETF
- Key Rotation
- Microsoft Defender For Cloud
- Microsoft Defender For Endpoint
- Microsoft Defender Vulnerability Management
- Microsoft Sentinel
- News
- NIST
- OMB M 23 02
- PCI DSS 4.0
- PKI
- Post Quantum Cryptography
- PQC
- Quantum Safe Readiness
- Secrets Management
- Security
- SSH
- TLS
- TPM
- Vulnerability Management
- X.509 Certificates
title: 'Building your cryptographic inventory: A customer strategy for cryptographic posture management'
---

Aviram Shemesh and Jennifer Rutzer explain how to build a cryptographic inventory and run an ongoing cryptographic posture management lifecycle, using Microsoft Security tooling (like Defender and GitHub Advanced Security), Azure services (like Key Vault and Network Watcher), and partner CPM solutions to improve quantum-safe readiness.<!--excerpt_end-->

# Building your cryptographic inventory: A customer strategy for cryptographic posture management

Post-quantum cryptography (PQC) is coming—and for most organizations, the hardest part won’t be choosing new algorithms. It will be finding where cryptography is used today across applications, infrastructure, devices, and services so teams can plan, prioritize, and modernize with confidence.

Microsoft frames this as a practical foundation of quantum readiness:

> You can’t protect or migrate what you can’t see.

As described in Microsoft’s Quantum Safe Program strategy, cryptography is embedded across modern IT environments (applications, network protocols, cloud services, and hardware devices). It also changes constantly due to newly discovered vulnerabilities, evolving standards (for example NIST and IETF), and emerging regulatory requirements. Without a comprehensive inventory and lifecycle process, many organizations lack the visibility and agility needed to respond when vulnerabilities or mandates emerge.

The first and most critical step toward a quantum-safe future—and sound cryptographic hygiene in general—is building a comprehensive **cryptographic inventory**.

## What is a cryptographic inventory?

A cryptographic inventory is a living catalog of all cryptographic assets and mechanisms in use across your organization, for example:

| Category | Examples/Details |
| --- | --- |
| Certificates and keys | X.509 certificates, private/public key pairs, certificate authorities, key management systems |
| Protocols and cipher suites | TLS/SSL versions and configurations, SSH protocols, IPsec implementations |
| Cryptographic libraries | OpenSSL, LibCrypt, SymCrypt, other libraries embedded in applications |
| Algorithms in code | Cryptographic primitives referenced in source code (RSA, ECC, AES, hashing functions) |
| Encrypted session metadata | Active network sessions using encryption, protocol handshake details |
| Secrets and credentials | API keys, connection strings, service principal credentials stored in code, configuration files, or vaults |
| Hardware security modules (HSMs) | Physical and virtual HSMs, Trusted Platform Modules (TPMs) |

Why it matters:

- **Governance and compliance**: Many countries and the EU recommend or require some subset of organizations to do cryptographic inventorying. Examples cited include DORA, OMB M-23-02, and PCI DSS 4.0.
- **Risk prioritization**: A weak cipher on an internet-facing TLS endpoint is a different risk than an internal test certificate or local disk encryption using AES. An inventory helps you prioritize using exposure and data sensitivity.
- **Crypto agility**: When an algorithm is found vulnerable, an inventory helps you quickly identify what needs updating and where.

## Customer-led cryptography posture management lifecycle

Cryptography Posture Management (CPM) is described as an ongoing lifecycle (not a single product) built from tools, integrations, and processes.

A typical operating model:

1. **Define what you are managing** (inventory scope and critical assets).
2. **Define how you make decisions** (risk assessment and prioritization).
3. **Define how you execute change safely** (remediation and validation).
4. **Define how you keep it current** (continuous monitoring).

A CPM lifecycle can be run continuously as:

1. **Discover**: Collect cryptographic signals from code repositories, runtime environments, network traffic, and storage systems.
2. **Normalize**: Aggregate into a unified inventory with consistent schema (for example: certificate thumbprints, algorithm types, key lengths, expiration dates).
3. **Assess risk**: Evaluate assets against baselines, standards, and known vulnerabilities (weak algorithms, expired certificates, non-compliant configuration).
4. **Prioritize**: Rank by criticality and exposure (internal vs internet-facing) and compliance requirements.
5. **Remediate**: Rotate keys, update libraries, reconfigure protocols, replace weak algorithms, using automation where possible.
6. **Continuous monitoring**: Track ongoing changes (new code commits, certificate renewals, config drift, emerging vulnerabilities).

You can apply this across four domains:

- **Code**: Crypto primitives and libraries in source, found via source code analysis.
- **Storage**: Certificates, keys, and secrets stored on disk, in databases, in key vaults, or config files.
- **Network**: TLS/SSH sessions and handshake/cipher negotiations.
- **Runtime**: In-memory crypto library usage, active key material, process-level operations.

A key point is to define clear ownership for each stage and treat CPM as an operating model rather than a “one-and-done” scan, because environments change continuously.

## Building your inventory with Microsoft tools

The post outlines how many organizations already have Microsoft Security and Azure capabilities deployed that can produce cryptographic signals. The goal is to connect and normalize those signals into a single inventory, then extend coverage with partner solutions where needed.

| Microsoft tool | Cryptographic signals | Domain coverage | Public documentation |
| --- | --- | --- | --- |
| GitHub Advanced Security (GHAS) | Identifies cryptographic algorithm artifacts in code via CodeQL | Code | [Addressing post-quantum cryptography with CodeQL](https://github.blog/security/vulnerability-research/addressing-post-quantum-cryptography-with-codeql/) |
| Microsoft Defender for Vulnerability Management (MDVM) | Certificate inventory from devices with MDE agents (including asymmetric key algorithm details); detects cryptographic libraries and vulnerabilities | Runtime, Storage | [Certificate inventory](https://learn.microsoft.com/defender-vulnerability-management/tvm-certificate-inventory) and [Vulnerable components](https://learn.microsoft.com/defender-vulnerability-management/tvm-vulnerable-components) |
| Microsoft Defender for Endpoint (MDE) | Identifies encrypted traffic sessions (TLS, SSH) via network detection and response | Runtime, Network | [Network protection – MDE](https://learn.microsoft.com/defender-endpoint/network-protection) |
| Microsoft Defender for Cloud (MDC) | Secret scanning for private keys exposed on cloud infrastructure; DevOps security for code repositories | Storage, Code | [Protecting secrets in Defender for Cloud](https://learn.microsoft.com/azure/defender-for-cloud/secrets-scanning) |
| Azure Key Vault | Centralized inventory of keys, secrets, and certificates stored in Azure | Storage | [Azure Key Vault documentation](https://learn.microsoft.com/azure/key-vault/) |
| Azure Networking (Firewall, Network Watcher) | High-level indications of encrypted traffic and protocol metadata (TLS, encrypted communication types) | Network | [Azure Network Watcher overview](https://learn.microsoft.com/azure/network-watcher/network-watcher-overview) |

### Suggested starter playbook

1. **Code domain**: Enable GHAS and use CodeQL to scan for cryptographic algorithm usage; export results for centralized oversight.
2. **Runtime and storage**: Deploy MDE and MDVM; use certificate inventory to discover certificates and algorithms; review vulnerable cryptographic libraries flagged by MDVM.
3. **Network domain**: Enable network protection in MDE; if on Azure, configure Azure Network Watcher to capture traffic metadata and identify encrypted flows.
4. **Storage domain**: Audit Azure Key Vault for secrets/keys/certificates; use Defender for Cloud secret scanning to detect exposed keys in IaaS/PaaS.
5. **Normalize and centralize**: Consolidate outputs into a common view and schema (for example in a SIEM like Microsoft Sentinel) using exports/connectors and reporting workflows; mature toward automation and governed data pipelines.
6. **Assess and prioritize**: Define policy baselines (minimum key lengths, approved algorithms, expiration thresholds) and prioritize by risk.

## Accelerating with the partner ecosystem

For broader coverage across mixed environments, the post points to CPM partner solutions integrated with Microsoft Security and running on Azure. Named partners include Keyfactor, Forescout, Entrust, and Isara.

- **Keyfactor**: Keyfactor AgileSec discovers and continuously monitors cryptography, flags vulnerabilities, and supports remediation workflows.
- **Forescout**: Forescout Cyber Assurance on Azure provides real-time network risk including PQC/non-PQC communications.
- **Entrust**: Entrust Cryptographic Security Platform focuses on PKI, key/cert lifecycle management, and HSMs.
- **Isara**: ISARA Advance is deployed on Azure to automate discovery/inventory, quantify risk, prioritize, and remediate crypto/config changes.

## Getting started: a customer checklist

1. **Establish ownership** for cryptographic governance across security, infrastructure, and development.
2. **Start inventory collection** using Microsoft tools (or partners) across code, runtime, network, and storage.
3. **Define crypto policy baselines** (approved algorithms, minimum key lengths, validity periods, protocol versions) aligned to standards and compliance.
4. **Prioritize exposures** by criticality, exposure, and mandates.
5. **Plan remediation** (library updates, cert rotations, protocol reconfiguration) with runbooks/automation.
6. **Leverage partners** if you need broader coverage, faster deployment, or specialized capabilities.

The post closes by emphasizing CPM as an ongoing journey that must adapt as standards evolve, vulnerabilities emerge, and quantum computing advances.


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/16/building-your-cryptographic-inventory-a-customer-strategy-for-cryptographic-posture-management/)

