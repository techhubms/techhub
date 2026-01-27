---
external_url: https://github.blog/changelog/2025-09-11-actions-macos-26-image-now-in-public-preview
title: Using the macOS 26 Image in GitHub Actions Workflows
author: Allison
feed_name: The GitHub Blog
date: 2025-09-11 20:10:13 +00:00
tags:
- Actions
- ARM64
- CI/CD
- Continuous Integration
- GitHub Actions
- Ios Development
- Macos 26
- Release Management
- Runner Images
- Workflow Automation
- Xcode
section_names:
- devops
primary_section: devops
---
Allison discusses the public preview release of the macOS 26 image for GitHub Actions, detailing how developers can use the latest Xcode and workflow labels for iOS CI/CD tasks.<!--excerpt_end-->

# Actions: macOS 26 Image Now in Public Preview

Developers can now leverage the latest Xcode technology by utilizing the new **macOS 26 image** in their [GitHub Actions](https://github.com/features/actions) workflows. This allows building and testing iOS applications on up-to-date infrastructure directly within the CI/CD process.

## Supported Runner Labels

The following labels are available for specifying the new macOS image in your workflow YAML:

- `macos-26`
- `macos-26-xlarge`

You can configure your workflow with these labels:

```yaml
runs-on: macos-26
```

Or for a larger runner:

```yaml
runs-on: macos-26-xlarge
```

## Limitations and Compatibility

- The macOS 26 runner image is currently **only available on arm64 macOS runners**. It is **not** supported on Intel-based runners.
- This image includes updated tools and tool versions versus previous macOS images (e.g., macOS 15).
- For more details about installed software, refer to the [runner-images repository](https://github.com/actions/runner-images).

## Getting Started

To try out the new image, update your `runs-on:` field in your GitHub Actions workflow YAML to one of the supported labels. This will enable ARM-based macOS 26 workflows for your builds and tests.

For issues or questions, visit the [runner-images repository](https://github.com/actions/runner-images) to view software lists or submit feedback.

---

**Further Reading:**

- [Full GitHub Blog Announcement](https://github.blog/changelog/2025-09-11-actions-macos-26-image-now-in-public-preview)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-11-actions-macos-26-image-now-in-public-preview)
