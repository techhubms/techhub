---
external_url: https://github.blog/changelog/2026-02-26-github-actions-now-supports-uploading-and-downloading-non-zipped-artifacts
title: GitHub Actions Enhances Artifact Uploads and Downloads with Non-Zipped Support
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-26 22:36:48 +00:00
tags:
- Actions
- Actions/download Artifact
- Actions/upload Artifact
- Artifact Management
- CI/CD
- Developer Experience
- DevOps
- GitHub
- GitHub Actions
- Improvement
- News
- Release Management
- V7.0.0
- V8.0.0
- Workflow Automation
- Workflow Improvements
section_names:
- devops
---
Allison details the new support for uploading and downloading non-zipped artifacts in GitHub Actions, explaining how this improvement streamlines developer workflows and resolves multiple friction points.<!--excerpt_end-->

# GitHub Actions: Uploading and Downloading Non-Zipped Artifacts

GitHub Actions now allows developers to upload and download artifacts without automatic zipping, addressing long-standing user feedback regarding workflow friction caused by mandatory compression.

## What Changed?

- With version 7 (`v7`) of [`actions/upload-artifact`](https://github.com/actions/upload-artifact/releases/tag/v7.0.0), users can set the `archive` parameter to `false`, preventing files from being zipped during upload.
- Version 8 (`v8`) of [`actions/download-artifact`](https://github.com/actions/download-artifact/releases/tag/v8.0.0) supports downloading these non-zipped artifacts.
- The default behavior still zips artifacts (for compatibility), so opting into this feature requires explicitly setting `archive: false`.
- Older artifacts and prior action versions will continue the existing zipped behavior.

## Key Benefits

- **No Unzipping for Single Files:** Downloading a single file through the browser no longer requires manual extraction.
- **Native File Preview:** If the file type is browser-supported (e.g., images, markdown, standalone HTML), files can be viewed directly inside the browser, including on mobile.
- **No Double Compression:** Uploading already compressed files (like .zip or .tar.gz) won't cause 'zip within a zip' issues.

## How to Use

- In your GitHub Actions workflow, use `actions/upload-artifact@v7` with `archive: false`:

  ```yaml
  - name: Upload Artifact Without Zipping
    uses: actions/upload-artifact@v7
    with:
      name: my-artifact
      path: ./build/output
      archive: false
  ```

- For downloads, make sure to use `actions/download-artifact@v8`.

## Impact

This enhancement directly addresses feedback around artifact handling, removing unnecessary steps developers previously performed during CI/CD, and improving integration with browser-based workflows.

## Version Considerations

- The default value for `archive` remains `true` to avoid breaking changes; explicitly set to `false` to use this new feature.
- Upgrade both upload and download actions to the mentioned versions to ensure compatibility.

For further details, check the official [release notes for upload-artifact v7](https://github.com/actions/upload-artifact/releases/tag/v7.0.0) and [download-artifact v8](https://github.com/actions/download-artifact/releases/tag/v8.0.0).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-26-github-actions-now-supports-uploading-and-downloading-non-zipped-artifacts)
