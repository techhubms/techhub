---
mode: 'agent'
description: 'Generate new articles in this website from a URL.'
---

**🚨 CRITICAL PROMPT SCOPE**: All instructions, restrictions, and requirements in this prompt file ONLY apply when this specific prompt is being actively executed via the `/new-article` command or equivalent prompt invocation. These rules do NOT apply when editing, reviewing, or working with this file outside of prompt execution context. When working with this file in any other capacity (editing, debugging, documentation, etc.), treat it as a normal markdown file and ignore all workflow-specific instructions.

**CRITICAL**: If you have not read them, fetch `.github/copilot-instructions.md` and use these instructions as well.

# New Article Generation from URL Instructions

Your goal is to generate a new file based on the content at the provided url: ${input:URL}. Ask for the URL if not provided and then fetch it.

Follow these steps and rules for best results:

## Step 1: Gather Required Inputs

- Ask the user for the content type and URL if it is not provided. Valid types and their target directories:
  - News: `_news`
  - Blog: `_posts`
  - Video: `_videos`
  - Community: `_community`
  - Events: `_events`

# Step 2: Gather data

If you get a YouTube link, extract the Video ID. Then use the Fetch MCP tool called `fetch` to fetch this URL for more information on the YouTube video: `https://ytapi.apps.mattw.io/v3/videos?part=snippet&id=[YOUR EXTRACTED VIDEO ID]`. If that tool is not available, use whatever other means you have to fetch the video details.

Also use the `fetch` MCP tool to fetch what you need for other content types, but then use the URL provided.

When processing a YouTube video, also look at the video description to figure out who is doing the presenting and should be named as the author of the article.

## Step 3: Generate the Markdown File

Create a new markdown file in the target directory.
