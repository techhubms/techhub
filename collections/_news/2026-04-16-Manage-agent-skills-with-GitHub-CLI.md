---
section_names:
- ai
- devops
- github-copilot
- security
external_url: https://github.blog/changelog/2026-04-16-manage-agent-skills-with-github-cli
primary_section: github-copilot
author: Allison
title: Manage agent skills with GitHub CLI
feed_name: The GitHub Blog
tags:
- Agent Skills
- Agent Skills Specification
- AI
- AI Coding Agents
- Code Scanning
- Copilot
- DevOps
- Gh Skill
- Gh Skill Install
- Gh Skill Preview
- Gh Skill Publish
- Gh Skill Update
- Git Commit SHA
- Git Tags
- Git Tree SHA
- GitHub CLI
- GitHub Copilot
- Immutable Releases
- News
- Prompt Injection
- Provenance
- Public Preview
- Secret Scanning
- Security
- Skill Installation
- Skill Publishing
- SKILL.md Frontmatter
- Supply Chain Security
- Tag Protection
- Version Pinning
date: 2026-04-16 17:24:41 +00:00
---

Allison announces `gh skill`, a new GitHub CLI command for discovering, installing, updating, and publishing portable “agent skills” for AI coding agents (including GitHub Copilot), with a focus on version pinning and supply-chain integrity.<!--excerpt_end-->

# Manage agent skills with GitHub CLI

Agent skills are portable sets of instructions, scripts, and resources that teach AI agents how to perform specific tasks. GitHub has introduced a new GitHub CLI command, `gh skill`, to help you discover, install, manage, and publish these skills directly from GitHub repositories.

`gh skill` is in **public preview** and may change.

## What are agent skills?

- **Purpose:** A skill teaches an AI agent how to do a specific task using packaged instructions, scripts, and resources.
- **Portability:** Skills follow the open [Agent Skills specification](https://agentskills.io) and are intended to work across multiple agent hosts.
- **Supported hosts mentioned:** GitHub Copilot, Claude Code, Cursor, Codex, Gemini CLI (and others).

## Get started

1. Update GitHub CLI to **v2.90.0+**.
2. Discover and install skills using `gh skill`.

### Install and discover skills

```sh
# Browse skills in a repository and install them interactively
gh skill install github/awesome-copilot

# Or install a specific skill directly
gh skill install github/awesome-copilot documentation-writer

# Install a specific version using @tag
gh skill install github/awesome-copilot documentation-writer@v1.2.0

# Install at a specific commit SHA
gh skill install github/awesome-copilot documentation-writer@abc123def

# Discover skills
gh skill search mcp-apps
```

Skills are installed automatically into the correct directory for the chosen agent host. You can also target a host and scope explicitly:

```sh
gh skill install github/awesome-copilot documentation-writer --agent claude-code --scope user
```

## Version pinning and supply chain integrity

Because agent skills can include executable instructions that influence agent behavior, silent changes between installs are framed as a supply chain risk. `gh skill` uses GitHub primitives to provide package-manager-like guarantees.

### Key integrity features

- **Tags and releases:** Published releases are tied to git tags. `gh skill publish` can enable [immutable releases](https://docs.github.com/repositories/releasing-projects-on-github/about-releases) so release content can’t be altered after publication (even by admins).
- **Content-addressed change detection:** Each installed skill records the **git tree SHA** of the source directory. `gh skill update` compares local SHAs to the remote to detect real content changes.
- **Version pinning:** Use `--pin` to lock a skill to a specific tag or commit SHA. Pinned skills are skipped during updates.
- **Portable provenance via frontmatter:** When installing, `gh skill` writes tracking metadata (repository, ref, tree SHA) into the `SKILL.md` **frontmatter**, so provenance travels with the skill if it gets copied or moved.

### Pinning examples

```sh
# Pin to a release tag
gh skill install github/awesome-copilot documentation-writer --pin v1.2.0

# Pin to a commit for maximum reproducibility
gh skill install github/awesome-copilot documentation-writer --pin abc123def
```

## Publish your own skills

If you maintain a skills repository, `gh skill publish`:

- Validates skills against the [agentskills.io specification](https://agentskills.io/specification)
- Checks recommended repo settings that improve supply chain security:
  - tag protection
  - secret scanning
  - code scanning

Examples:

```sh
# Validate all skills
gh skill publish

# Auto-fix metadata issues
gh skill publish --fix
```

## Keep skills up to date

`gh skill update`:

- Scans known agent-host directories
- Reads provenance metadata from each installed skill
- Checks upstream changes

```sh
# Check for updates interactively
gh skill update

# Update a specific skill
gh skill update git-commit

# Update everything without prompting
gh skill update --all
```

## Supported agent hosts

| Host | Install command example |
| --- | --- |
| GitHub Copilot | `gh skill install OWNER/REPOSITORY SKILL` |
| Claude Code | `gh skill install OWNER/REPOSITORY SKILL --agent claude-code` |
| Cursor | `gh skill install OWNER/REPOSITORY SKILL --agent cursor` |
| Codex | `gh skill install OWNER/REPOSITORY SKILL --agent codex` |
| Gemini CLI | `gh skill install OWNER/REPOSITORY SKILL --agent gemini` |
| Antigravity | `gh skill install OWNER/REPOSITORY SKILL --agent antigravity` |

## Security notes and cautions

- Skills are **not verified by GitHub**.
- Skills may contain **prompt injections**, hidden instructions, or malicious scripts.
- It’s recommended to inspect skill content before installing; you can do this with:

```sh
gh skill preview
```

## Learn more

- Agent Skills spec: https://agentskills.io
- Discussion: GitHub Community discussions (linked from the original post)



[Read the entire article](https://github.blog/changelog/2026-04-16-manage-agent-skills-with-github-cli)

