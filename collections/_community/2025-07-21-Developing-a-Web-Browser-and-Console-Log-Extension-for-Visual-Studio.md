---
external_url: https://www.reddit.com/r/VisualStudio/comments/1m5pxx2/possible_new_web_browserconsole_extension/
title: Developing a Web Browser and Console Log Extension for Visual Studio
author: ggobrien
viewing_mode: external
feed_name: Reddit Visual Studio
date: 2025-07-21 17:55:22 +00:00
tags:
- Browser Panel
- Console Log
- Developer Productivity
- Extension Development
- IDE Tools
- Olivier Dalet
- Proof Of Concept
- React
- User Interface
- VS
- WebView2
section_names:
- coding
---
ggobrien explores the idea of creating a Visual Studio extension that embeds a web browser and separate console log panel, enhancing the workflow for web developers.<!--excerpt_end-->

## Background

Working with React in Visual Studio often requires developers to switch between their code editor and the browser window to view results and debug using the browser's console log. While having multiple monitors helps alleviate this inconvenience, not everyone prefers or has access to that setup. Previously, Visual Studio included a browser panel, but the author notes this feature was removed, and it was unclear if it ever included a console log panel.

## Existing Solutions and Limitations

The author references an extension by Olivier Dalet that adds a browser as a panel in Visual Studio. However, this extension doesn't include a console log. Dalet also created a standalone project of a web browser with a console, but it's separate from the extension, and for unclear reasons, he never combined the two features.

## Proposal for a New Extension

The author decided to start developing their own extension. Their plan includes:

- A browser window panel that can be docked like any other Visual Studio window.
- A separate console log window, also dockable, with the possibility of combining both for a better workflow depending on user feedback.

As a proof-of-concept, the author created a simple embedded web browser using WebView2 with a colored console window beneath it. This shows the feasibility of the project, though they have not yet moved toward developing a full Visual Studio extension.

## Questions and Next Steps

The author is still exploring whether there are existing solutions or if there's real interest from other developers for such an extension. They are also considering whether to fork Dalet's project or build a new extension from scratch. The author notes there are merits to both approaches and is open to feedback from the community.

## Miscellaneous

The author ends with a touch of humor, referencing a YouTube video and linking to the Reddit post for further community discussion.

This post appeared first on "Reddit Visual Studio". [Read the entire article here](https://www.reddit.com/r/VisualStudio/comments/1m5pxx2/possible_new_web_browserconsole_extension/)
