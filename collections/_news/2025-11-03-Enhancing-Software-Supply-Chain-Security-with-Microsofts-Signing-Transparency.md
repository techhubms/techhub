---
layout: post
title: Enhancing Software Supply Chain Security with Microsoft’s Signing Transparency
author: Mark Russinovich
canonical_url: https://azure.microsoft.com/en-us/blog/enhancing-software-supply-chain-security-with-microsofts-signing-transparency/
viewing_mode: external
feed_name: The Azure Blog
feed_url: https://azure.microsoft.com/en-us/blog/feed/
date: 2025-11-03 17:00:00 +00:00
permalink: /azure/news/Enhancing-Software-Supply-Chain-Security-with-Microsofts-Signing-Transparency
tags:
- Auditing
- Azure
- Azure Built in Security
- Azure Confidential Computing
- Code Signing
- Compliance
- Confidential Computing
- Confidential Ledger
- COSE
- Cryptographic Ledger
- Merkle Tree
- Microsoft Azure
- News
- SCITT
- Security
- Signing Transparency
- Software Supply Chain Security
- Transparency Logs
- Trusted Execution Environments
- Zero Trust
section_names:
- azure
- security
---
Mark Russinovich presents Microsoft’s preview of Signing Transparency, a new Azure-based transparency log service designed to strengthen software supply chain security through immutable signature records and confidential computing.<!--excerpt_end-->

# Enhancing Software Supply Chain Security with Microsoft’s Signing Transparency

*By Mark Russinovich*

Microsoft has announced the preview of Signing Transparency, a new service designed to address modern software supply chain threats that traditional code signing cannot fully prevent. Built on the Zero Trust principle of "never trust, always verify," Signing Transparency introduces an immutable, append-only log that verifiably records every software signature, with signing keys protected inside a confidential computing enclave.

## Why Transparency in the Software Supply Chain Matters

Software supply chains face sophisticated attacks, including misuse of stolen signing certificates and infiltration of trusted build systems. Traditional code signing provides some security, but attackers who gain access to signing keys can still distribute malicious updates. What’s needed is a mechanism that makes code signing verifiable and accountable at scale — so every signed artifact can be traced, and unexpected changes are detectable.

Signing Transparency ensures:

- **Every signature is logged in a tamper-evident, append-only ledger**
- **Anyone can audit when and what was signed, and by whom**
- **Attackers cannot silently insert malicious signatures**

Transparency logs, especially when paired with Trusted Execution Environments (TEEs), extend trust by ensuring any unauthorized use of a signing key is either made visible or must be actively concealed (which itself is a suspicious event). This increases confidence in the supply chain’s integrity.

## What is Microsoft’s Signing Transparency?

Signing Transparency is a cloud-managed Azure service acting as an independent notary for software signatures. It:

- Maintains an append-only, public ledger of all signing events, using cryptographic mechanisms (Merkle tree) and confidential hardware for integrity
- Accepts signed software artifacts (binaries, containers, firmware, etc.) and records a reference of the signature
- Provides all participants with a cryptographically signed receipt for each signing event
- Utilizes COSE (CBOR Object Signing and Encryption) envelopes, compliant with emerging supply chain integrity standards (SCITT)

### Key Components and Workflow

- **Countersigning COSE envelopes**: Adds a Microsoft countersignature to each artifact, creating a second layer of attestation. Any tampering is immediately detectable
- **Immutable Merkle tree ledger**: Every log entry is cryptographically linked, ensuring immutability and transparency
- **Receipt for every signing event**: Each receipt provides verifiable proof of inclusion and can be used for audits and compliance

> The ledger is backed by Azure Confidential Ledger and the Confidential Consortium Framework (CCF) running in a TEE, ensuring entries and cryptographic proofs remain highly secure and independently verifiable.

## How Does Signing Transparency Work?

- Developers or build systems sign artifacts, creating a COSE_Sign1 envelope (an industry-standard binary format)
- The signed object is sent to the Signing Transparency service
- The service verifies the signature and the signer's identity against policy, then issues a countersignature and appends the event to the ledger
- The entry includes hashes of the artifact, signature, signer identity, and is cryptographically cemented into a Merkle tree root
- The user receives a receipt containing proof of ledger inclusion (root hash, path, and signature)

### Practical Benefits

- **Tamper-evident Releases**: Any modification or replay of code releases is immediately detectable
- **Independent Verification**: Customers, auditors, and partners can independently check artifact authenticity without relying solely on Microsoft
- **Audit Trail and Compliance**: Transparent, verifiable records for each component simplify audits and respond quickly to incidents
- **Accountability**: Comprehensive logs deter insider threats and encourage policy compliance
- **Protection from Key Compromise**: Attempts to use compromised signing keys are logged and visible to all
- **Cross-layer Application**: Applies to all layers, including firmware (supported by standards like OCP-SAFE and Caliptra)

## Why This Matters Now

With supply chain attacks increasing, organizations require not just code integrity but also proof and rapid detection. Microsoft’s Signing Transparency service enables:

- Direct, non-repudiable verification of code releases
- Reduced risk and improved confidence when adopting third-party or open source software
- A foundation for modern compliance frameworks like Zero Trust, with concrete technical evidence

## Get Involved

You can learn more and sign up for the preview program [here](https://aka.ms/MSTPreviewSignUp). For deep technical details, see [Microsoft Azure Blog](https://azure.microsoft.com/en-us/blog/enhancing-software-supply-chain-security-with-microsofts-signing-transparency/).

This post appeared first on "The Azure Blog". [Read the entire article here](https://azure.microsoft.com/en-us/blog/enhancing-software-supply-chain-security-with-microsofts-signing-transparency/)
