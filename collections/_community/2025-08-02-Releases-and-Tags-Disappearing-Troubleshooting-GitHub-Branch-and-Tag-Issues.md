---
layout: post
title: 'Releases and Tags Disappearing: Troubleshooting GitHub Branch and Tag Issues'
author: Discommodian
canonical_url: https://www.reddit.com/r/github/comments/1mfuewy/releases_and_tags_disappearing/
viewing_mode: external
feed_name: Reddit GitHub
feed_url: https://www.reddit.com/r/github/.rss
date: 2025-08-02 16:13:34 +00:00
permalink: /devops/community/Releases-and-Tags-Disappearing-Troubleshooting-GitHub-Branch-and-Tag-Issues
tags:
- Actions
- Branches
- Docker
- Git
- Git Push
- GitHub
- Snyk
- Tags
- Webhooks
section_names:
- devops
---
Discommodian shares their experience with unpredictable disappearance of tags, branches, and releases in their GitHub repository, seeking advice from the community.<!--excerpt_end-->

## Introduction

Discommodian details a perplexing issue encountered while managing a GitHub repository with multiple branches, tags, and releases. Despite following established git procedures and having limited GitHub automation, certain repository elements (tags, branches, releases) periodically disappear seemingly without cause.

## Workflow Overview

- **Platform:** Mac
- **Repository Setup:** Local clone with two main working branches
- **Branch Management:**
  - Works mainly on `main` and a secondary branch
  - Uses git commands to push tags and branches
    - Example for pushing all tags: `git push --tags`
    - Switching and pushing branches: `git checkout branch`, then `git push origin branch`
  - Both branches expected to appear in GitHub
- **Release Process:**
  - New releases are created through the GitHub web interface
  - The release process on GitHub also creates a tag

## Observed Issues

- Tags created and pushed locally appear on GitHub as expected, but later disappear
- Secondary branch sometimes disappears from GitHub
- Recently, even a GitHub release has vanished
- These issues do not align with normal git or GitHub behavior

## Automation and Integrations

- One previously active Snyk webhook, currently inactive
- GitHub Action exists for building a Docker container
- The problem occurs even when pushing to a branch that does not trigger the GitHub Action

## Attempts at Resolution

- Checked active webhooks and automations for interference
- Verified that workflow and git commands are standard practice

## Seeking Input

Discommodian appeals to the developer community for insights, suspecting a misunderstanding or subtle misconfiguration in repository or workflow management that is causing elements (tags, branches, releases) to disappear, despite seemingly correct usage of GitHub and git.

This post appeared first on "Reddit GitHub". [Read the entire article here](https://www.reddit.com/r/github/comments/1mfuewy/releases_and_tags_disappearing/)
