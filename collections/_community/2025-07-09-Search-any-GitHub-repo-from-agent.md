---
external_url: https://www.reddit.com/r/GithubCopilot/comments/1lvx08d/search_any_github_repo_from_agent/
title: Search any GitHub repo from agent
author: digitarald
feed_name: Reddit Github Copilot
date: 2025-07-09 22:57:13 +00:00
tags:
- API References
- Chat View
- Copilot Instructions.md
- Development Tools
- Embeddings
- GitHub Repositories
- Githubrepo
- Prompt Discovery
- Repo Search
- VS Code Agent
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
In this article, digitarald highlights a powerful yet often overlooked feature of the VS Code agent: searching within GitHub repositories using embeddings for efficient code and prompt discovery. The author demonstrates this capability using the 'awesome-copilot' repository and shares an additional tip for enhancing copilot-instructions.md files to improve reference searching. The article provides insights that can streamline your development workflow and make Copilot even more effective.<!--excerpt_end-->

### Unlocking GitHub Repo Search in VS Code Agent

digitarald explains a little-known but highly useful ability of the VS Code agent: you can search within a GitHub repository directly using the agent. Simply ask the agent to "search in a repo", or use the `#githubRepo` tag to force the feature. The demonstration uses the [awesome-copilot](https://github.com/github/awesome-copilot) repository, which has grown to over 100 entries, allowing for efficient discovery of new modes and prompts right from the chat view.

#### How It Works

The VS Code agent utilizes a GitHub repo embeddings search index, which is automatically available for all repos. This index makes it possible to instantly search and surface relevant code snippets or documentation within the chat interface of VS Code.

#### Enhancing Copilot with API References

As a bonus tip, the author recommends mentioning repositories in your `copilot-instructions.md` file for API references. When planning, the agent will search these mentioned repos, improving the quality and relevance of generated suggestions or plans.

#### Key Takeaways

* Leverage the VS Code agent to search and discover code or prompts within any repo using embeddings.
* The feature is easily triggered by asking the agent or using `#githubRepo`.
* Improve your workflow further by listing relevant repos in `copilot-instructions.md` to enhance API reference lookups.

This tool capability enables developers to make more effective use of their repositories and GitHub Copilot, streamlining how prompts and code are discovered and utilized.

This post appeared first on Reddit Github Copilot. [Read the entire article here](https://www.reddit.com/r/GithubCopilot/comments/1lvx08d/search_any_github_repo_from_agent/)
