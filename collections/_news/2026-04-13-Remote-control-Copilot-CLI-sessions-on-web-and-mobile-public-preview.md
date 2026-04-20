---
author: Allison
date: 2026-04-13 14:15:43 +00:00
feed_name: The GitHub Blog
primary_section: github-copilot
section_names:
- ai
- github-copilot
external_url: https://github.blog/changelog/2026-04-13-remote-control-cli-sessions-on-web-and-mobile-in-public-preview
tags:
- /keep Alive
- /remote
- /update
- Agentic Coding
- AI
- Ask User Tool
- Autopilot Mode
- CLI Policies
- Copilot
- Copilot   Remote
- Copilot Business
- Copilot CLI
- Copilot Enterprise
- GitHub Copilot
- GitHub Mobile
- GitHub Web
- Interactive Mode
- News
- Permission Requests
- Plan Review
- QR Code Login
- Remote Sessions
title: Remote control Copilot CLI sessions on web and mobile (public preview)
---

Allison announces a public preview feature for GitHub Copilot CLI that enables remote control of running CLI sessions from GitHub on the web and in GitHub Mobile, including monitoring, steering messages, plan review, and policy-controlled permissions.<!--excerpt_end-->

# Remote control Copilot CLI sessions on web and mobile (public preview)

The [Copilot CLI](https://github.com/features/copilot/cli) is no longer limited to a local terminal experience. GitHub is launching `copilot --remote` in public preview, which lets you monitor and steer a running Copilot CLI session from GitHub on the web or in the GitHub Mobile apps.

## What `copilot --remote` does

When remote sessions are enabled, Copilot streams your CLI session activity to GitHub in real time.

- The CLI shows a session link and a QR code.
- You can open the link from another device to:
  - View and monitor the session
  - Send follow-up commands to the running session
- The CLI and GitHub stay in sync, so actions taken in one place show up in the other.
- Each remote session is private and visible only to the user who started it.

## Features supported in remote sessions

Remote sessions support the usual Copilot CLI capabilities, including:

- Sending mid-session steering messages, plus new messages to keep the agent going after the current turn completes
- Reviewing and modifying plans before implementation begins
- Switching modes:
  - plan
  - interactive
  - autopilot
  - or stopping the session entirely
- Approving or denying permission requests (based on your existing CLI settings)
- Responding to questions prompted via the `ask_user` tool

## Tips for getting started

- Run `/update` to use the latest version of Copilot CLI.
- Start a new remote session with `copilot --remote`, or enable it in an existing session with `/remote`.
- Make sure your working directory is a GitHub repository.
- Open the session link to follow along and interact from the web.
- For longer tasks, use `/keep-alive` to help keep your machine awake while the CLI is running.
- To try it on mobile:
  - Android: [beta release on Google Play](https://play.google.com/apps/testing/com.github.android)
  - iOS: [iOS TestFlight](https://testflight.apple.com/join/NLskzwi5)

## Admin policy note (Business/Enterprise)

If you’re using Copilot Business or Copilot Enterprise, an administrator must enable remote control and CLI policies before you can use the feature:

- [Enable remote control and CLI policies (docs)](https://docs.github.com/en/copilot/concepts/agents/copilot-cli/about-remote-access#administering-remote-access)

## Learn more / discussion

- [Copilot CLI documentation: steer remotely](https://docs.github.com/copilot/how-tos/copilot-cli/steer-remotely)
- [GitHub Community discussion](https://github.com/orgs/community/discussions/191814)


[Read the entire article](https://github.blog/changelog/2026-04-13-remote-control-cli-sessions-on-web-and-mobile-in-public-preview)

