---
external_url: https://github.blog/engineering/platform-security/post-quantum-security-for-ssh-access-on-github/
title: Post-Quantum Secure SSH Access on GitHub
author: brian m. carlson
feed_name: GitHub Engineering Blog
date: 2025-09-15 16:00:00 +00:00
tags:
- Cryptography
- Encryption
- Engineering
- Enterprise Security
- FIPS Compliance
- Git
- GitHub
- Key Exchange Algorithm
- OpenSSH
- Platform Security
- Post Quantum Cryptography
- Quantum Security
- Security Best Practices
- Sntrup761x25519 Sha512
- SSH
section_names:
- devops
- security
primary_section: devops
---
brian m. carlson details GitHub's implementation of post-quantum secure SSH key exchange, highlighting the updates, impact, and steps for developers to prepare.<!--excerpt_end-->

# Post-Quantum Secure SSH Access on GitHub

GitHub is strengthening the security of SSH access for Git data by introducing a post-quantum secure key exchange algorithm: `sntrup761x25519-sha512` (also referred to as `sntrup761x25519-sha512@openssh.com`). This update, effective September 17, 2025, is designed to protect against future quantum decryption attacks, ensuring data secures both now and in the future.

## What Is Changing?

- **New Algorithm:** SSH access points on GitHub will now support the hybrid post-quantum key exchange algorithm `sntrup761x25519-sha512`.
- **Scope:** Affects only SSH access for Git data (not HTTPS), and does not impact GitHub Enterprise Cloud with US data residency.
- **No Impact on Most Users:** Most clients using OpenSSH 9.0 or newer will automatically use the new algorithm if supported, with no configuration changes required.

## Why This Matters

Quantum computers, if sufficiently advanced, could break traditional cryptographic algorithms in the future. Attackers could store encrypted SSH sessions now and decrypt them later (“store now, decrypt later” attacks). By rolling out a hybrid algorithm—combining Streamlined NTRU Prime with Elliptic Curve Diffie-Hellman on X25519—GitHub is proactively improving cryptographic resilience.

## Rollout

- **Effective Date:** September 17, 2025
- **Platforms:** github.com and non-US GitHub Enterprise Cloud regions
- **Enterprise Server:** Included from GitHub Enterprise Server 3.19
- **US Region:** Not rolled out due to FIPS compliance constraints

## How to Prepare

- **For SSH Users:**
  - If your SSH client supports the new algorithm (e.g., OpenSSH 9.0+), it will be used automatically if preferred in your client's configuration.
  - No action needed unless you've altered SSH key exchange defaults.
- **Legacy Clients:**
  - Older SSH clients will continue to function but will not benefit from post-quantum security until upgraded.

### Checking Algorithm Support

- To list key exchange algorithms:

  ```
  ssh -Q kex
  ```

- To check which is used during connection:

  ```
  ssh -v git@github.com exit 2>&1 | grep 'kex: algorithm:'
  ```

- Refer to your SSH client documentation for other implementations.

## Next Steps

GitHub will continue monitoring advances in post-quantum cryptography and will add support for new, FIPS-compliant algorithms as they are standardized.

For detailed technical context and further updates, refer to the [GitHub Blog](https://github.blog/engineering/platform-security/post-quantum-security-for-ssh-access-on-github/).

This post appeared first on "GitHub Engineering Blog". [Read the entire article here](https://github.blog/engineering/platform-security/post-quantum-security-for-ssh-access-on-github/)
