---
layout: "page"
title: "Visual Studio Code Updates"
author: "Fokko Veegens"
description: "Fokko Veegens showing you all the latest features of each Visual Studio Code release"
category: "GitHub Copilot"
---

Stay current with the latest Visual Studio Code features and improvements! Each month, Visual Studio Code releases new functionality that enhances your development experience, from productivity improvements to new GitHub Copilot integrations.

Fokko Veegens provides video walkthroughs of the most significant updates in each Visual Studio Code release, with a special focus on GitHub, GitHub Copilot, and AI features when they're available. You'll find comprehensive coverage of what's new and how to make the most of these features in your daily workflow.

{% comment %} Get all videos from the vscode-updates collection and sort by date (newest first) {% endcomment %}
{% assign vscode_videos = site.videos | where_exp: "video", "video.path contains 'vscode-updates'" | sort: "date" | reverse %}
{% assign latest_video = vscode_videos | first %}

## Latest VS Code Update

[**{{ latest_video.title }}**]({{ latest_video.url | relative_url }})

{% youtube latest_video.youtube_id %}

{{ latest_video.description }}

## Other VS Code Updates

{% for video in vscode_videos %}
{% if video.url != latest_video.url %}

- [**{{ video.title }}**]({{ video.url | relative_url }}) - {{ video.description }}

{% endif %}
{% endfor %}
