---
external_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/increase-security-for-azure-vms-trusted-launch-in-place-upgrade/ba-p/4440584
title: 'Upgrade Azure VMs with Trusted Launch: In-Place Security Enhancement Now Available'
author: AjKundnani
feed_name: Microsoft Tech Community
date: 2025-09-04 05:50:05 +00:00
tags:
- Azure Security Benchmark
- Azure Virtual Machines
- Boot Integrity Monitoring
- Cloud Security
- Compliance
- FedRAMP
- Gen1 VM
- Gen2 VM
- HIPAA
- in Place Upgrade
- Infrastructure Security
- Microsoft Azure
- PCI DSS
- Scale Set
- Secure Boot
- Trusted Launch
- Vtpm
section_names:
- azure
- security
---
AjKundnani introduces the new in-place upgrade support for Trusted Launch in Azure Virtual Machines and Scale Sets, helping administrators strengthen security without complex migrations.<!--excerpt_end-->

# Upgrade Azure VMs with Trusted Launch: In-Place Security Enhancement Now Available

## Introduction

Trusted Launch in-place upgrade support is now available for Azure Virtual Machines (VMs) and Scale Sets, enabling organizations to enhance the foundational security of their cloud infrastructure without complex migrations, downtime, or service interruption. This capability is generally available for existing Gen1 (BIOS) and Gen2 (UEFI) VMs and Uniform Scale Sets and in private preview for Flex Scale Sets.

## What Is Trusted Launch?

[Trusted Launch](https://learn.microsoft.com/azure/virtual-machines/trusted-launch) is an Azure native capability that provides multiple foundational security features to virtual machines and scale sets:

- **Secure Boot**: Ensures only signed, trusted code runs during system startup, protecting against rootkits and bootkits.
- **vTPM (virtual Trusted Platform Module)**: Acts as a secure vault for encryption keys and boot measurement, supporting attestation of system integrity.
- **Boot Integrity Monitoring**: Uses guest attestation extensions to continuously verify that VMs boot into an uncompromised state.

Utilizing Trusted Launch adds cryptographic verification to your VM’s boot process, maintaining the trustworthiness of guest operating systems and supporting regulatory compliance with standards such as [Azure Security Benchmark](https://learn.microsoft.com/en-us/azure/governance/policy/samples/azure-security-benchmark), [FedRAMP](https://www.fedramp.gov/), [HIPAA](https://www.hhs.gov/hipaa/for-professionals/security/laws-regulations), and [PCI-DSS](https://www.pcisecuritystandards.org/).

## Benefits of In-Place Upgrade Support

- **No Downtime or Migration:** Instantly enable foundational security features for existing resources—no complex rebuilds required.
- **Comprehensive Coverage:** Support is available for Gen1 and Gen2 VMs, Uniform Scale Sets, and Flex Scale Sets (in private preview).
- **Compliance Readiness:** Helps meet industry and regulatory security requirements.
- **No Additional Cost:** These enhanced security features are offered at no extra charge.

## How to Upgrade: Steps & Resources

The upgrade process varies based on your VM or Scale Set type. Find the correct documentation for your scenario:

- **Gen1 Virtual Machine:** [Upgrade existing Azure Gen1 VMs to Trusted Launch](https://aka.ms/TrustedLaunchUpgrade/Gen1VM)
- **Gen2 Virtual Machine:** [Enable Trusted Launch on existing Azure Gen2 VMs](https://aka.ms/TrustedLaunchUpgrade/Gen2VM)
- **Virtual Machine Scale Set:** [Upgrade existing Azure Scale Set to Trusted Launch](https://aka.ms/TrustedLaunchUpgrade/ScaleSet)
- **Flex Scale Sets (Private Preview):** [Sign up for preview](https://aka.ms/TrustedLaunchUpgrade/FlexPreview)

### Pre-Requisites

Before upgrading, verify:

- VM/Scale Set size is [supported for Trusted Launch](https://learn.microsoft.com/azure/virtual-machines/trusted-launch#virtual-machines-sizes).
- Running a [supported operating system](https://learn.microsoft.com/azure/virtual-machines/trusted-launch#operating-systems-supported).
- Not dependent on [unsupported Azure features](https://learn.microsoft.com/azure/virtual-machines/trusted-launch#unsupported-features).
- If Azure Backup is used, [migrate to Enhanced Backup policy](https://learn.microsoft.com/en-us/azure/backup/backup-azure-vm-migrate-enhanced-policy).
- [Disable Azure Site Recovery](https://learn.microsoft.com/azure/site-recovery/concepts-trusted-vm#migrate-azure-site-recovery-protected-azure-generation-2-vm-to-trusted-vm) beforehand, and re-enable after upgrade if necessary.

### Best Practices

- Review all documentation and known issues for your VM/Scale Set type.
- Test the Trusted Launch upgrade on a non-production resource to check for prerequisite or functionality issues.
- [Create restore points](https://learn.microsoft.com/en-us/azure/virtual-machines/create-restore-points) for critical VMs before starting the upgrade.

### Limitations

- Secure Boot and vTPM can only be enabled for Gen2 (UEFI) operating systems.
- In-place roll-back from Trusted Launch to Gen1 (BIOS) is not supported. You must restore from backup if full rollback is required.

## Compliance & Regulatory Coverage

Trusted Launch helps organizations maintain compliance with standards such as Azure Security Benchmark, FedRAMP, HIPAA, PCI-DSS, and others by ensuring secure configurations and verified boot sequences.

## FAQs and Support

- Upgrade support is generally available for Gen1 and Gen2 VMs and Uniform Scale Sets; private preview for Flex scale sets.
- Upgrades do not affect unrelated VMs or scale set clusters.
- Detailed instructions, limits, and rollback steps are available in the official documentation for each resource type (see links above).

## Conclusion

Trusted Launch is a foundational Azure security capability that is now easier to adopt with the in-place upgrade option. Strengthen your cloud infrastructure security, improve compliance, and minimize operational disruption by upgrading your Azure VMs and Scale Sets today.

---

For further details, step-by-step guides, and known limitations, refer to:

- [Official Trusted Launch Documentation](https://learn.microsoft.com/azure/virtual-machines/trusted-launch)
- [Upgrade Support](https://aka.ms/TrustedLaunchUpgrade)

Authored by AjKundnani

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/increase-security-for-azure-vms-trusted-launch-in-place-upgrade/ba-p/4440584)
