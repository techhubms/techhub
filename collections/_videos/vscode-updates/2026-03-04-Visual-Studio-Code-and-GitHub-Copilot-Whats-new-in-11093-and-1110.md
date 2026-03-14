---
external_url: https://www.youtube.com/watch?v=Jn9m8wa8ZoI
title: Visual Studio Code and GitHub Copilot - What's new in 1.109.3 and 1.110
author: Fokko Veegens
feed_name: Fokko at Work
date: 2026-03-04 18:00:00 +00:00
tags:
- Agent Hooks
- Agent Plugins
- AI Co-authoring
- Debug Panel
- Message Steering
- Refactoring Tools
- Skills
- VS Code
- VS Code 1.109.3
- VS Code 1.110
- AI
- GitHub Copilot
- Videos
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Fokko Veegens covers the highlights of VS Code 1.109.3 and 1.110, including agent hooks for deterministic control, mid-session message steering and queueing, skills as slash commands, the new agent debug panel with flowchart visualization, edit and ask mode changes, agent plugins from the marketplace, #usages and #rename refactoring tools, and AI co-author attribution for commits.<!--excerpt_end-->

{% youtube Jn9m8wa8ZoI %}

## Full summary based on transcript

Visual Studio Code versions 1.109.3 and 1.110 bring a wide range of improvements to GitHub Copilot, from deterministic agent control through hooks to a greatly improved debugging experience and a new plugin ecosystem. This highlights video covers the key features Fokko considers most relevant.

### Agent Hooks

Hooks give you deterministic control over Copilot by running scripts at specific points during a session.

**Available hook types:**

- **Session start** - triggered when a new agent session begins
- **Pre-tool use** - triggered right before Copilot uses a tool (e.g., the terminal tool)

**Setup:**

- Type `/hooks` in chat to see all available hooks and create a new one
- Select the hook type and provide a file name
- A hook file is created in the `.github/hooks/` folder

**Hook file format:**

- The hook file specifies a command to execute (e.g., a PowerShell script)
- The command type can vary per platform (Linux, Windows, macOS)
- The script can return a JSON response to Copilot with:
  - `continue` - whether Copilot should continue execution (`true` or `false`)
  - `stopReason` - message displayed in chat when execution is stopped
  - `systemMessage` - additional context for Copilot

**Example use cases:**

- Setting a proxy before tool execution
- Running linters after Copilot makes suggestions
- Filtering or blocking specific tool calls
- Enforcing policies before terminal commands

**Configuration:**

- Setting: `chat.hooks.enabled` - must be enabled for hooks to work
- Hooks in the project `.github` folder take precedence over user profile hooks
- Enterprise policies can disable hooks entirely

### Message Steering and Queueing

Mid-session steering allows you to send additional instructions to Copilot while it is actively working, a feature previously only available in the coding agent on github.com.

**How it works:**

- While Copilot is processing a request, type a new message in the chat input
- Three options appear:
  - **Steer with message** - message is handled after the current tool call completes
  - **Add to queue** - message is executed once Copilot has fully completed
  - **Stop and send** - stops current execution and sends your new message

**Configuration:**

- Setting: `chat.requestQueuing.defaultAction`
- Default value: `steer` (pressing Enter sends as a steering message)
- Can be changed to `queue` (stop and send is not available as a default action)

### Skills as Slash Commands

Skills are markdown-based definitions that provide Copilot with specialized behavior and additional tools for deterministic outcomes.

**Structure:**

- Skills live in a `skills/` directory with a subdirectory per skill
- Each skill requires at minimum a `skill.md` file
- The `skill.md` has a frontmatter header with `name` and `description`

**Invocation:**

- Copilot can automatically discover and use skills
- Manually invoke with `/skillname` (e.g., `/net` for a .NET design pattern skill)
- Ask Copilot "What skills do you have available?" to list all discovered skills

**Frontmatter settings:**

- `userInvocable` - set to `false` to prevent manual slash command invocation (default: `true`)
- `disableModelInvocation` - set to `true` to prevent Copilot from automatically picking up the skill

**Resources:**

- The "Awesome Copilot" GitHub repository contains a curated collection of skills

### Agent Debug Panel

A new structured debug panel replaces the previous markdown-heavy debug view for inspecting Copilot sessions.

**Access:**

- Open via command palette (F1 / Ctrl+Shift+P) and search for "Agent Debug Panel"

**Dashboard view:**

- Shows recent sessions with summary stats: model turns, tool calls, total tokens, errors, and total events

**Agent flowchart:**

- Visual flowchart of the entire session
- Scroll to zoom, drag to pan
- Expandable nodes showing:
  - Loaded instructions, skills, agents, and hooks
  - User messages with prompt sections and attachments
  - Tool calls with details (e.g., which files were read)
  - Model calls with token counts

**View logs:**

- Alternative structured log view with similar detail
- Useful for finding specific tool calls and their parameters

### Edit and Ask Mode Changes

Edit mode has been hidden by default and reimplemented as a custom agent, while ask mode has gained agentic capabilities.

**Edit mode:**

- Hidden from the mode menu by default
- Re-enable via setting: `chat.editMode.hidden` (uncheck to show)
- Now implemented as a custom agent definition (no longer a proprietary mode)
- Has a "Configure tools" option - most built-in tools are disabled by default
- Still restricts edits to the active file and explicitly attached files
- View the agent definition via "Configure custom agents" to create your own variants

**Ask mode:**

- Now has "Configure tools" - full agentic experience with tool access
- Can discover context using tools instead of requiring explicit context attachment
- Also backed by a custom agent definition that you can customize

### Agent Plugins

Agent plugins package multiple Copilot customizations (instructions, skills, custom agents) into a single installable unit.

**Plugin contents:**

- Custom instructions, skills, agents, and other customizations bundled together
- A `.github/plugin/` folder with a `manifest.json` for marketplace metadata
- A `README.md` explaining usage

**Installation:**

- Search for "agent plugins" in the VS Code extensions marketplace
- Default plugin sources: `github/copilot-plugins` and `github/awesome-copilot` repositories
- First install clones the repository locally in the background
- Also installable via the Copilot CLI (requires separate CLI installation)

**Example - Nuke Mode:**

- A plugin from the Awesome Copilot repository
- Activate with `/noop mode` (or the specific slash command for the plugin)
- Changes Copilot's response style: clear step-by-step plans, risk assessments, and structured explanations of changes

**Configuration:**

- Setting: `chat.plugins.enabled` - enable/disable the plugin feature
- Setting: `chat.plugins.marketplaces` - array of GitHub repository references to use as plugin sources
- Push marketplace settings via `.vscode/settings.json` to share with your team

### Tools for Usages and Rename

New `#usages` and `#rename` tools improve refactoring workflows in Copilot.

**Usage:**

- Use `#usages` to find all usages of a symbol
- Use `#rename` to rename a symbol across the codebase
- Example prompt: "Find #usages of logger and rename them to classLogger"

**Model compatibility:**

- Not all models pick up these tools equally well
- Gemini 2.5 Flash showed good results in the demo
- Explicitly mentioning the tool names in your prompt improves reliability

**Verification:**

- Use the Agent Debug Panel to verify whether the tools were actually invoked
- The flowchart shows exact tool calls: `list code usages` and `rename`

### AI Co-author Attribution for Commits

A new setting allows adding GitHub Copilot as a co-author on commits made with AI assistance.

**Configuration:**

- Setting: `git.ai.addCoAuthor`
- Options:
  - **Off** (default) - only you appear as author
  - **Chat and agent** - adds Copilot as co-author when agent or ask mode was used to generate code
  - **All** - also includes code completions and inline chat

**Result:**

- Commits show "Co-authored-by: Copilot <copilot@github.com>" in the commit metadata
- Visible in GitHub commit history alongside your name

**Known limitation:**

- Does not work when the commit message itself is generated by Copilot (bug as of this release)

### Additional Features Mentioned

Several other noteworthy features were briefly mentioned but not demoed in detail:

- **Chat conversation forking** - fork from a specific point in your chat conversation to a new conversation, carrying along all context
- **Copilot memory** - Copilot now has memory capabilities, especially useful in plan mode where it retains the generated plan
- **Conversation compaction** - Copilot automatically summarizes long conversations in the background, and you can now trigger this manually as well
