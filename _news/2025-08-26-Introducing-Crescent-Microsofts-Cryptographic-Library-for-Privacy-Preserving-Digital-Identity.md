---
layout: "post"
title: "Introducing Crescent: Microsoft's Cryptographic Library for Privacy-Preserving Digital Identity"
description: "This post introduces Crescent, a cryptographic library from Microsoft Research that enhances privacy in digital identity systems. The article explains how Crescent enables unlinkability and selective disclosure for common identity credentials such as JSON Web Tokens and mobile driver’s licenses, using advanced zero-knowledge proofs that allow users to prove facts about their identity without exposing unnecessary personal information. Technical details on Crescent's architecture, its use of the Groth16 SNARK system, and a practical implementation example are provided, illustrating how it can be deployed without requiring existing issuers to upgrade infrastructure."
author: "stclarke"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/research/blog/crescent-library-brings-privacy-to-digital-identity-systems/"
viewing_mode: "external"
feed_name: "Microsoft News"
feed_url: "https://news.microsoft.com/source/feed/"
date: 2025-08-26 18:56:40 +00:00
permalink: "/2025-08-26-Introducing-Crescent-Microsofts-Cryptographic-Library-for-Privacy-Preserving-Digital-Identity.html"
categories: ["AI", "Security"]
tags: ["AI", "Authentication", "Azure AI Foundry", "Company News", "Credential Unlinkability", "Crescent", "Cryptography", "Digital Identity", "Groth16", "Identity Verification", "JSON Web Token", "Microsoft Research", "Mobile Driver’s License", "News", "Privacy", "Rust", "Security", "Selective Disclosure", "SNARK", "Zero Knowledge Proofs"]
tags_normalized: ["ai", "authentication", "azure ai foundry", "company news", "credential unlinkability", "crescent", "cryptography", "digital identity", "groth16", "identity verification", "json web token", "microsoft research", "mobile drivers license", "news", "privacy", "rust", "security", "selective disclosure", "snark", "zero knowledge proofs"]
---

stclarke explains how Crescent, a cryptographic library from Microsoft Research, provides privacy-preserving features for digital identity systems, preventing tracking and protecting user data with advanced cryptography.<!--excerpt_end-->

# Crescent: Bringing Privacy to Digital Identity Systems

Digital identities are now commonplace—embedded in our phones, workplace logins, and apps—offering convenience but raising concerns about privacy, particularly linkability and surveillance. When users present credentials such as mobile driver’s licenses or log into apps, these interactions can be tracked and linked, building detailed profiles over time.

## Introducing Crescent

[Crescent](https://github.com/microsoft/crescent-credentials) is a cryptographic library developed by Microsoft Research to address these concerns by providing *unlinkability* for popular digital identity formats, including JSON Web Tokens and mobile driver’s licenses. Crescent’s privacy features go beyond existing systems:

- **Unlinkability**: Prevents credentials from being linked across uses by removing or randomizing hidden identifiers.
- **Selective Disclosure**: Users can prove specific facts (e.g., age eligibility) without revealing unnecessary or identifying information.
- **No Issuer Migration Required**: Crescent works with current credential infrastructure, so issuers do not need to update their systems.

## Two Paths to Unlinkability

The article explores two approaches for privacy in identity systems:

1. **Specialized cryptographic signatures** (e.g., BBS signatures): Require significant upgrades to infrastructure.
2. **Zero-knowledge proofs with existing credentials**: Crescent follows this route, letting users prove statements about credentials (e.g., employment, age) without revealing underlying data, leveraging advances in zero-knowledge proof efficiency and preprocessing.

## Architecture and Cryptography

Crescent is based on zero-knowledge SNARKs, specifically the Groth16 proof system, which is compact, efficient, and noninteractive.

- **Proof Preparation**: Intensive computations are done once and stored securely on users’ devices.
- **Proof Presentation**: Uses randomization to prevent linkage of separate uses, enabling fast credential presentation even on mobile devices.
- **Security**: Credentials and keys remain private and can be integrated with secure hardware.

Figures in the original article illustrate the stages of proof preparation, credential presentation, and the overall system architecture.

## Sample Application

A reference implementation includes:

- **Employment Verification**: Proving workplace status with a JSON Web Token (without exposing user identity).
- **Age Verification**: Confirming age eligibility using a mobile driver’s license (without revealing actual age or identity).
- **Technical Stack**: Sample code is provided using Rust servers for issuers/verifiers and a browser extension wallet.

## Integration

Crescent protocols can be incorporated into higher-level identity frameworks like OpenID/OAuth or mobile driver’s license ecosystems. More information is available on the [Crescent GitHub repository](https://github.com/microsoft/crescent-credentials/) and in recent presentations at industry conferences.

## Resources

- [Crescent Project on GitHub](https://github.com/microsoft/crescent-credentials/)
- [Academic Preprint](https://eprint.iacr.org/2024/2013)
- [Presentation at Real-Word Crypto 2025](https://www.youtube.com/live/gnB76DQI1GE?t=3475s)
- [North Sec 2025 Conference](https://www.youtube.com/watch?v=9IT659uUXfs&t=13361s)
- [Azure AI Foundry Labs](https://ai.azure.com/labs)

This post appeared first on "Microsoft News". [Read the entire article here](https://www.microsoft.com/en-us/research/blog/crescent-library-brings-privacy-to-digital-identity-systems/)
