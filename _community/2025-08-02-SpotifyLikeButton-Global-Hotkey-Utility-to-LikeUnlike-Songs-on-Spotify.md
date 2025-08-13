---
layout: "post"
title: "SpotifyLikeButton: Global Hotkey Utility to Like/Unlike Songs on Spotify"
description: "BuildBazaar shares a Windows project enabling global hotkeys to like or unlike Spotify songs without opening the app. Developed in C#, it uses the Spotify API and supports notifications. Linux alternatives using custom scripts and Waybar integration are mentioned. The source code and discussion links are provided."
author: "BuildBazaar"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/csharp/comments/1mfyig0/spotifylikebutton/"
viewing_mode: "external"
feed_name: "Reddit CSharp"
feed_url: "https://www.reddit.com/r/csharp/.rss"
date: 2025-08-02 19:05:44 +00:00
permalink: "/2025-08-02-SpotifyLikeButton-Global-Hotkey-Utility-to-LikeUnlike-Songs-on-Spotify.html"
categories: ["Coding"]
tags: ["C#", "Coding", "Community", "Dotfiles", "Global Hotkeys", "Hyprland", "Linux Scripts", "Music", "Open Source", "Spotify API", "Waybar", "Windows"]
tags_normalized: ["c", "coding", "community", "dotfiles", "global hotkeys", "hyprland", "linux scripts", "music", "open source", "spotify api", "waybar", "windows"]
---

BuildBazaar introduces SpotifyLikeButton, a C# tool that lets users like or unlike Spotify songs with global hotkeys. Explore how it works, its technical details, and alternative Linux scripts.<!--excerpt_end-->

## Overview

BuildBazaar presents *SpotifyLikeButton*, a Windows utility designed to let users efficiently like or unlike songs on Spotify using global hotkeys. This project addresses the inconvenience of switching to the Spotify app during activities like gaming or coding just to interact with the Like/Unlike song feature.

## Key Features

- **Global Hotkeys:** Default keys are F4 (Like) and F8 (Unlike). These are user-configurable.
- **Spotify API Integration:** The tool directly interacts with the Spotify API to perform the desired actions.
- **User Notifications:** Optionally play a sound or show a notification with the song info after performing an action.
- **Windows Only:** Designed for Windows; Linux users are pointed to an alternative solution.

## Developer Insights

BuildBazaar describes the motivation for the tool, arising from wanting to personalize Spotify’s song recommendations without interrupting workflow. Not engaging with Spotify’s Like system led to repetitive playlists, which this tool helps remedy.

## Troubleshooting & Community Feedback

One known issue concerns compatibility with Spotify accounts created through Facebook. The author invites feedback and bug reports for further troubleshooting.

## Linux Alternative

- **Custom Script:** For Linux (specifically with `ncspot`), BuildBazaar provides a custom script available in their dotfiles repo.
- **Keybinds and Waybar Integration:** The script can be tied to hotkeys in Hyprland and integrates with Waybar for notifications.
- **User Engagement:** The author offers to share more details if the community expresses interest.

## Useful Links

- [GitHub Repository: elurzen/SpotifyLikeButton](https://github.com/elurzen/SpotifyLikeButton)
- [Reddit Discussion and Feedback](https://www.reddit.com/r/csharp/comments/1mfyig0/spotifylikebutton/)

## Conclusion

This project combines practical C# coding, API integration, and workflow optimization, making it a handy tool for Spotify users who wish to enhance their music experience without frequent context switching.

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mfyig0/spotifylikebutton/)
