---
layout: "post"
title: "Making Windows Terminal Awesome with GitHub Copilot CLI"
description: "This guide explores personalizing and optimizing Windows Terminal for developer workflows using GitHub Copilot CLI. Kayla Cinnamon walks through detailed steps to customize terminal profiles, launch Copilot CLI efficiently, integrate Oh My Posh for enhanced prompts, and display GitHub Copilot stats right in the shell. Includes tips for PowerShell, pane management, restoring sessions, nerd fonts, retro terminal effects, and practical GitHub Copilot CLI usage for daily coding activities."
author: "Kayla Cinnamon"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/blog/making-windows-terminal-awesome-with-github-copilot-cli"
viewing_mode: "external"
feed_name: "Microsoft Blog"
feed_url: "https://devblogs.microsoft.com/feed"
date: 2025-12-11 17:00:28 +00:00
permalink: "/2025-12-11-Making-Windows-Terminal-Awesome-with-GitHub-Copilot-CLI.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "CLI Development", "Coding", "Command Line Tools", "Customization", "GitHub Copilot", "GitHub Copilot CLI", "Microsoft", "Microsoft For Developers", "Nerd Fonts", "News", "npm", "Oh My Posh", "PowerShell", "Profile Settings", "Prompt Customization", "Retro Terminal Effects", "Terminal Panes", "Terminal Profiles", "Windows Terminal"]
tags_normalized: ["ai", "cli development", "coding", "command line tools", "customization", "github copilot", "github copilot cli", "microsoft", "microsoft for developers", "nerd fonts", "news", "npm", "oh my posh", "powershell", "profile settings", "prompt customization", "retro terminal effects", "terminal panes", "terminal profiles", "windows terminal"]
---

Kayla Cinnamon guides developers on making Windows Terminal highly personalized for coding, highlighting GitHub Copilot CLI setup, custom profiles, and integrating useful prompt features.<!--excerpt_end-->

# Making Windows Terminal Awesome with GitHub Copilot CLI

Kayla Cinnamon offers practical advice for developers seeking to enhance their Windows Terminal setup and streamline command-line development with GitHub Copilot CLI.

## Why Personalize Windows Terminal?

Windows Terminal provides flexibility for developers to tailor the environment, making coding more efficient and enjoyable. Personalization ranges from backgrounds and pane layouts to advanced prompt styling.

## Using GitHub Copilot CLI in Terminal

- **GitHub Copilot CLI** enables conversational code assistance straight from the terminal, eliminating the need for IDEs.
- Useful for troubleshooting unfamiliar languages, running shell commands, and automating repetitive tasks during development.
- Installation: Run `npm install -g @github/copilot` globally, then launch with `copilot`.

```shell
npm install -g @github/copilot
```

- The CLI offers prompt-based command suggestions and even helps with shell-specific tasks like managing PowerShell history.

### Always Show Copilot Banner

- Force the Copilot banner on each launch by:
  1. Setting `"banner": "always"` in the config.json (`C:\Users\USERNAME\.copilot\config.json` or `~/.copilot/config.json` for WSL).
  2. Using `copilot --banner` flag manually.

### Running Shell Commands in Copilot CLI

- Prepend shell commands with `!` to execute within Copilot CLI, staying in the Copilot context.

## Terminal Customizations

### Create a Copilot CLI Profile

- Duplicate your PowerShell profile and update name, icon, and starting directory.
- Add `-c copilot` to command line settings to auto-launch Copilot CLI:

```shell
"C:\Program Files\PowerShell\7\pwsh.exe" -c copilot
```

### Utilize Pane Functionality

- Open additional panes per profile (`Alt` + profile select), close with `Ctrl + Shift + W`.

### Restore Tabs on Relaunch

- Enable "Restore window layout and content" in Startup settings for continuity.

### Custom Backgrounds & Retro Effects

- Apply backgrounds per profile or globally via Appearance settings.
- Enable "Retro terminal effects" for a glowing aesthetic and scan lines.

## Prompt Customizations with Oh My Posh

- Enhance prompt appearance and info with [Oh My Posh](https://ohmyposh.dev/).
- Install via `winget install JanDeDobbeleer.OhMyPosh --source winget`.
- Download a [Nerd Font](https://www.nerdfonts.com/font-downloads) for full glyph support.
- Choose and apply themes, add segments for Git info, npm, React, or even your Spotify song.

### Display GitHub Copilot Usage Stats

- In Oh My Posh v28.1.0+, add the Copilot segment to show premium and chat quota:

{% raw %}

```json
{
  "type": "copilot",
  "template": "  {{ .Premium.Percent.Gauge }} ",
  "cache": { "duration": "5m", "strategy": "session" },
  "properties": { "http_timeout": 1000 }
}
```

{% endraw %}

- Authenticate with `oh-my-posh auth copilot` to enable stats.

## Additional Resources

- For full setup, see [Oh My Posh docs](https://ohmyposh.dev/docs/installation/windows) and [GitHub Copilot CLI repo](https://github.com/github/copilot-cli).

## Connect

Reach out to Kayla Cinnamon on [Bluesky](https://bsky.app/profile/kaylacinnamon.bsky.social) or [X](https://x.com/cinnamon_msft) for questions.

---

*Get the most out of your terminal command line and let your coding environment truly work for you!*

This post appeared first on "Microsoft Blog". [Read the entire article here](https://devblogs.microsoft.com/blog/making-windows-terminal-awesome-with-github-copilot-cli)
