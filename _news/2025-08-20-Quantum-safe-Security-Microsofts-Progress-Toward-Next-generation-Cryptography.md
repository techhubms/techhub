---
layout: "post"
title: "Quantum-safe Security: Microsoft's Progress Toward Next-generation Cryptography"
description: "This in-depth news feature, authored by Mark Russinovich and Michal Braverman-Blumenstyk, details Microsoft's strategy and practical steps toward quantum-safe security. It explores advances in post-quantum cryptography (PQC), contributions to global standards, development of cryptographic accelerators, product updates across Microsoft platforms, and the Quantum Safe Program's multi-phase roadmap. The article guides organizations on transitioning their infrastructures to resist future quantum threats."
author: "Mark Russinovich and Michal Braverman-Blumenstyk"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/security/blog/2025/08/20/quantum-safe-security-progress-towards-next-generation-cryptography/"
viewing_mode: "external"
feed_name: "Microsoft Security Blog"
feed_url: "https://www.microsoft.com/en-us/security/blog/feed/"
date: 2025-08-20 16:00:00 +00:00
permalink: "/2025-08-20-Quantum-safe-Security-Microsofts-Progress-Toward-Next-generation-Cryptography.html"
categories: ["Azure", "Security"]
tags: ["Adams Bridge Accelerator", "Azure", "Caliptra", "Cryptographic Standards", "Cybersecurity", "Hybrid Key Exchange", "Linux", "Microsoft Entra", "Microsoft Quantum Safe Program", "ML DSA", "ML KEM", "News", "NIST", "Open Quantum Safe Project", "Post Quantum Cryptography", "PQC", "Quantum Computing", "Security", "SymCrypt", "TLS 1.3", "Windows Insiders"]
tags_normalized: ["adams bridge accelerator", "azure", "caliptra", "cryptographic standards", "cybersecurity", "hybrid key exchange", "linux", "microsoft entra", "microsoft quantum safe program", "ml dsa", "ml kem", "news", "nist", "open quantum safe project", "post quantum cryptography", "pqc", "quantum computing", "security", "symcrypt", "tls 1 dot 3", "windows insiders"]
---

Mark Russinovich and Michal Braverman-Blumenstyk present an authoritative overview of Microsoft's quantum-safe security strategy, covering cryptography advances, standards collaboration, and practical steps for resilience against quantum-era threats.<!--excerpt_end-->

# Quantum-safe Security: Microsoft's Progress Toward Next-generation Cryptography

**By Mark Russinovich and Michal Braverman-Blumenstyk**

Quantum computing is rapidly advancing and promises strong computational capabilities, but also threatens today’s public-key cryptography and digital signature schemes. Microsoft is actively preparing for the quantum era, spearheading efforts to upgrade cryptography, contribute to standards, and help organizations safeguard systems against future quantum-capable adversaries.

## The Impending Quantum Security Challenge

Quantum computers could break current public-key cryptographic algorithms, undermining authentication, confidentiality, and data integrity. Migrating to post-quantum cryptography (PQC) demands coordinated global action and modernization of legacy systems.

## Microsoft’s Multi-pronged Quantum-safe Strategy

- **Collaborations & Standards:** Microsoft partners with NIST, IETF, ISO, and others to help define and implement quantum-safe encryption standards.
- **Investment:** Microsoft’s work spans quantum hardware (e.g., Majorana 1 processor, error correction codes) and security, including PQC research, algorithm testing, and large-scale cryptographic system experiments like Project Natick.
- **Open Initiatives:** Founding member of the Open Quantum Safe project and leader in integrating PQC into industry protocols.

## Notable Milestones

- Participation in NIST PQC algorithm calls and contributing to ISO standards (e.g., FrodoKEM)
- Prototyping PQC-protected VPN tunnels
- Announced the Adams Bridge Accelerator and Caliptra 2.0 – open-source cryptographic hardware to accelerate PQC adoption
- PQC capabilities previewed for Windows Insiders and Linux, including updates to Microsoft’s SymCrypt library

## Microsoft Quantum Safe Program (QSP)

Launched to unify and accelerate Microsoft’s transition and help partners become quantum-safe, QSP aligns with global regulatory guidance and includes:

- Global program governance
- Multi-phase and modular migration approach
- Roadmap for default quantum-safe services by 2029, well ahead of government deadlines

### Three Key Phases

1. **Foundational security components:** Integrating PQC into cryptographic libraries (SymCrypt, CNG) and supporting both classic and hybrid key exchange (e.g., in TLS 1.3 with SymCrypt-OpenSSL).
2. **Core infrastructure services:** Prioritizing secure services like Microsoft Entra authentication, key management, and signing.
3. **Ecosystem-wide enablement:** Embedding PQC throughout Windows, Azure, Microsoft 365, AI services, and more.

## Technical Highlights

- Support for ML-KEM and ML-DSA algorithms
- Hybrid and pure PQC-ready key exchanges for HNDL (Harvest Now, Decrypt Later) threat defense
- Early testbed deployments for Windows Insiders and Linux

## Guidance and Call to Action

Organizations should assess cryptographic asset risk, plan phased migrations to PQC, and adopt crypto-agility practices. Microsoft provides practical resources and recommendations for a proactive quantum-safe transition.

## Further Reading & Resources

- [Microsoft Security Solutions](https://www.microsoft.com/en-us/security/business)
- [Quantum-safe Security: Blog Source](https://www.microsoft.com/en-us/security/blog/2025/08/20/quantum-safe-security-progress-towards-next-generation-cryptography/)
- [Quantum Safe Program Policy](https://aka.ms/QSP/policy-2025)
- [Project Natick](https://www.microsoft.com/en-us/research/project/post-quantum-crypto-tunnel-to-the-underwater-datacenter/?msockid=3bca5ca4c7dc6d23090c4ab8c6836ce0)

---

*Transitioning to quantum-safe cryptography is a complex but urgent process. Microsoft’s ongoing leadership and extensive technical solutions provide strong guidance and tools for organizations preparing for this new era of security.*

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/08/20/quantum-safe-security-progress-towards-next-generation-cryptography/)
