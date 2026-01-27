---
external_url: https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/introducing-image-customizer-for-azure-linux/ba-p/4454859
title: Introducing Image Customizer for Azure Linux
author: Kavya_Nagalakunta
feed_name: Microsoft Tech Community
date: 2025-09-18 15:00:00 +00:00
tags:
- Azure Linux
- Base Images
- Chroot
- CI/CD
- Cloud Security
- Container Tools
- Custom Images
- Declarative API
- Dm Verity
- Image Customization
- Image Customizer
- ISO
- Linux
- Microsoft
- Open Source
- OS Guard
- Production Ready
- QCOW2
- Ubuntu
- VHD
- WSL2
section_names:
- azure
- devops
primary_section: azure
---
Kavya_Nagalakunta presents Image Customizer—a new open-source tool from the Azure Linux team—enabling fast, reliable, and secure customization of Azure Linux images and seamless CI/CD integration.<!--excerpt_end-->

# Introducing Image Customizer for Azure Linux

The Azure Linux team announces **Image Customizer**, an open-source tool designed to make customizing Azure Linux images faster, more reliable, and more secure. Already adopted by teams like LinkedIn, Azure Frontdoor, and Azure Nexus, this tool enables IT professionals and DevOps engineers to tailor Azure Linux images directly—without the need to boot a VM.

## Key Features and Benefits

### Direct, Reliable Customization

- Modify well-tested Azure Linux base images using a chroot-based method.
- Avoids VM booting, reducing overhead and potential failures.
- Lowers risk of non-bootable images compared to building from scratch.

### Security with dm-verity and OS Guard

- Full support for dm-verity ensures improved image integrity.
- Compatible with Azure Linux OS Guard images, bolstering the security profile.

### Streamlined Integration (CI/CD)

- Distributed as a container with all dependencies pre-bundled.
- Easily integrated into CI/CD pipelines for automated image preparation.
- Minimal dependencies required; no SSH necessary.

### Versatile Input and Output Support

- **Input formats**: vhd, vhdx, qcow2, PXE bootable artifacts, raw, iso.
- **Output formats**: vhd, vhd-fixed, vhdx, qcow2, raw, iso, cosi.
- Perform a wide range of actions: add/remove/update packages, modify files/directories, create/update users, change partitions, enable/disable services, view image history, and more.
- Full API documentation: [Image Customizer configuration](https://microsoft.github.io/azure-linux-image-tools/imagecustomizer/api/configuration.html).

### Platform Compatibility

- Officially verified on Ubuntu 22.04, Azure Linux 3.0, and Windows Subsystem for Linux (WSL2).
- Likely works on other modern Linux distributions as well.

### Consistent and Predictable Builds

- Use package snapshot features to ensure only packages available as of a specific timestamp are included.
- Avoids accidental changes from future package updates.

## Getting Started

1. **Prepare a configuration file** detailing your desired changes using the [Declarative API](https://microsoft.github.io/azure-linux-image-tools/imagecustomizer/api/configuration.html).
2. **Select a base Azure Linux image** to customize. See [Base Images Guide](https://github.com/microsoft/azurelinux/blob/3.0/toolkit/docs/quick_start/quickstart.md).
3. **Run Image Customizer** using either the container (recommended) or standalone binary ([Quick Start Instructions](https://microsoft.github.io/azure-linux-image-tools/imagecustomizer/quick-start/quick-start.html)).
4. **Review results**: quickly obtain a custom Azure Linux image ready for deployment.

## Full Documentation and Support

- [Main documentation](https://aka.ms/ImageCustomizer)
- [Command-line options](https://microsoft.github.io/azure-linux-image-tools/imagecustomizer/api/cli.html)
- [Github repository](https://github.com/microsoft/azure-linux-image-tools) – for bug reports, feature requests, or contributions

## Upcoming Community Call

Learn about advanced scenarios, best practices, and see a live demo of Image Customizer.

- **Date & Time**: September 25th, 2025, 8:00 AM PST
- **Join via Teams**: [Azure Linux External Community Call](https://teams.microsoft.com/l/meetup-join/19%3ameeting_ZDcyZjRkYWMtOWQxYS00OTk3LWFhNmMtMTMwY2VhMTA4OTZi%40thread.v2/0?context=%7b%22Tid%22%3a%2272f988bf-86f1-41af-91ab-2d7cd011db47%22%2c%22Oid%22%3a%2271a6ce92-58a5-4ea0-96f4-bd4a0401370a%22%7d)
- **Community Call Schedule**: [Support & Help](https://learn.microsoft.com/en-us/azure/azure-linux/support-help#stay-connected-with-azure-linux)

## Community Feedback and Contribution

Contribute feedback, file bug reports, or improve the tool at the [azure-linux-image-tools GitHub repo](https://github.com/microsoft/azure-linux-image-tools). The development team welcomes contributions and collaboration with the community.

---

**Acknowledgements**: Thanks to the Image Customizer team, including Adit Jha, Brian Telfer, Chris Gunn, Deepu Thomas, Elaine Zhao, George Mileka, Himaja Kesari, Jim Perrin, Jiri Appl, Lanze Liu, Roaa Sakr, Kavya Nagalakunta, and Vince Perri.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/linux-and-open-source-blog/introducing-image-customizer-for-azure-linux/ba-p/4454859)
