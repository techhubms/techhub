---
layout: "post"
title: "Safeguarding VS Code Against Prompt Injections: Securing GitHub Copilot Chat"
description: "This in-depth analysis by Michael Stepankin explores recent vulnerabilities found in the Copilot Chat extension for Visual Studio Code, focusing on indirect prompt injection risks. The article breaks down how attackers can exploit agent tools to leak tokens or execute code, the fixes and mitigations implemented by the VS Code and GitHub teams, and best practices for developers to securely use AI-powered code assistants."
author: "Michael Stepankin"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/security/vulnerability-research/safeguarding-vs-code-against-prompt-injections/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-08-25 16:01:12 +00:00
permalink: "/2025-08-25-Safeguarding-VS-Code-Against-Prompt-Injections-Securing-GitHub-Copilot-Chat.html"
categories: ["AI", "Coding", "GitHub Copilot", "Security"]
tags: ["Agent Mode", "AI", "AI Security", "Application Security", "Coding", "CVE", "Developer Containers", "Editfile Tool", "Exploit", "Fetch Webpage Tool", "GitHub Codespaces", "GitHub Copilot", "GitHub Security Lab", "Language Models", "LLM", "MCP Server", "News", "Prompt Injection", "Sandboxing", "Security", "Security Mitigation", "Simple Browser Tool", "Supply Chain Security", "Token Leakage", "Tool API", "VS Code", "Vulnerability Research", "Web Application Security", "Workspace Trust"]
tags_normalized: ["agent mode", "ai", "ai security", "application security", "coding", "cve", "developer containers", "editfile tool", "exploit", "fetch webpage tool", "github codespaces", "github copilot", "github security lab", "language models", "llm", "mcp server", "news", "prompt injection", "sandboxing", "security", "security mitigation", "simple browser tool", "supply chain security", "token leakage", "tool api", "vs code", "vulnerability research", "web application security", "workspace trust"]
---

Michael Stepankin details prompt injection vulnerabilities in the VS Code Copilot Chat extension, examining how attackers target agent tools and how the GitHub and VS Code teams have responded. The article is vital reading for developers using AI-powered assistants.<!--excerpt_end-->

# Safeguarding VS Code Against Prompt Injections: Securing GitHub Copilot Chat

*Author: Michael Stepankin*

Prompt injection attacks are an emerging risk for developers using AI-powered code assistants like GitHub Copilot Chat in Visual Studio Code. This article documents discovery and mitigation of several vulnerabilities, showing how attackers can exploit agent tools to leak tokens or execute malicious code, and how newly implemented protections help reduce these risks.

## How Copilot Chat Agent Mode Works

- The Copilot Chat extension for VS Code enables use of multiple LLMs, built-in tools, and MCP servers to write code, make commits, and integrate with external systems.
- When users ask questions (e.g., about a GitHub Issue), agent mode compiles project context, tool definitions, and user selection into a prompt for the selected model.
- The LLM may call tools (such as `get_issue`, `read_file`, or `fetch_webpage`) to gather info or perform actions.

## Discovered Vulnerabilities: Exploiting Indirect Prompt Injection

### 1. Trusted URL Parsing Flaws in `fetch_webpage`

- Attackers could trick Copilot into exfiltrating secrets (like GitHub tokens) using carefully crafted prompts injected into public GitHub issues.
- VS Code's trust check failed to parse URLs correctly, allowing requests to untrusted domains resembling trusted domains.
- Once the model executed `read_file` and `fetch_webpage` with attacker-controlled URLs, it could silently leak secrets—without user confirmation.
- **Mitigation**: Now, fetch tool always asks for confirmation on first encounter with new URLs, and domain trust logic is stricter.

### 2. Data Exfiltration via Simple Browser

- Attackers could use the Simple Browser tool to load arbitrary external sites and leak tokens by chaining tool invocations (e.g., `read_file` then open URL with token as parameter).
- **Mitigation**: Simple Browser now also prompts for confirmation before opening unknown URLs.

### 3. Editing Sensitive Files with `editFile`

- The `editFile` tool could be leveraged to modify configuration files (such as `settings.json`) and trigger unintended code execution or change workspace behavior.
- Edits were auto-saved before confirmation, potentially allowing monitored files to trigger downstream effects.
- **Mitigation**: The agent now restricts file edits to within the workspace, and upcoming releases will prompt for confirmation when editing sensitive files.

### 4. Model Jailbreaking Techniques

- Attackers use various tricks to exploit model compliance: time-based conditions, referencing user/system prompts, and mimicking internal system prompts.
- Modern models have some resistance, but social engineering of instructions can still bypass safeguards.

## Security Features and Best Practices

**Improvements rolled out:**

- Full transparency of available tools (including those from extensions and MCP servers)
- Manual selection and tool set configuration
- Workspace/file access confirmations
- Requirement to trust newly added MCP servers
- Enterprise policies to disable dangerous features
- Defense-in-depth: information control flow, dual-LM approaches, RBAC, and more under review

### Best Practices for Developers

- **Use Workspace Trust:** Open untrusted code in restricted mode to block sensitive actions.
- **Sandbox with Developer Containers or Codespaces:** Run Copilot and tools in an isolated Docker container or ephemeral Codespace to prevent local damage.
- **Monitor for Updates:** Stay current—latest VS Code and Copilot Chat versions have stronger protections.
- **Review Tool Access:** Double check enabled tools and MCP servers, limiting to trusted sources.

## Conclusion

Prompt injection is an inherent challenge for text-based AI assistants interacting with development environments. As attackers continue to find new ways to manipulate LLM agent behaviors, sustained vigilance, layered mitigations, and developer discipline are essential. Advancements like stricter tool controls, alerting, and workspace isolation are making Copilot Chat safer, but no solution is foolproof yet. Use sandboxing and maintain awareness when leveraging AI-powered dev tools.

---

*References and further reading:*

- [Safeguarding VS Code against prompt injections](https://github.blog/security/vulnerability-research/safeguarding-vs-code-against-prompt-injections/)
- [GitHub Copilot Chat Tools Implementation](https://github.com/microsoft/vscode-copilot-chat/tree/main/src/extension/tools/node)
- [Workspace Trust in VS Code](https://code.visualstudio.com/docs/editing/workspaces/workspace-trust)
- [Developer Containers](https://code.visualstudio.com/docs/devcontainers/containers)
- [GitHub Codespaces](https://github.com/features/codespaces)
- Vulnerability research: [EmbraceTheRed](https://embracethered.com/blog/posts/2025/github-copilot-remote-code-execution-via-prompt-injection/) | [Persistent Security](https://www.persistent-security.net/post/part-iii-vscode-copilot-wormable-command-execution-via-prompt-injection)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/security/vulnerability-research/safeguarding-vs-code-against-prompt-injections/)
