---
external_url: https://r-vm.com/automating-my-git-workflow-vscode-copilot-chat-terminal-auto-approval.html
title: Automating My Git Workflow in VS Code with Copilot Chat, Custom Prompts, and Terminal Auto-Approval
author: Reinier van Maanen
feed_name: Reinier van Maanen's blog
date: 2025-07-11 00:00:00 +00:00
tags:
- Automation
- Code Review
- Configuration
- Copilot Chat
- Custom Prompts
- Developer Tools
- Development
- Git
- PowerShell
- Productivity
- Python
- Software Engineering
- Terminal
- Terminal Auto Approval
- Version Control
- VS Code
- Workflow
section_names:
- ai
- coding
- devops
- github-copilot
primary_section: github-copilot
---
In this detailed post, Reinier van Maanen demonstrates how to automate Git workflows in Visual Studio Code using GitHub Copilot Chat, custom prompts, and the experimental terminal auto-approval feature, providing practical configuration steps and benefits.<!--excerpt_end-->

## Automating My Git Workflow in VS Code with Copilot Chat, Custom Prompts, and Terminal Auto-Approval

**Author:** Reinier van Maanen

With the latest release of Visual Studio Code ([v1.102](https://code.visualstudio.com/updates/v1_102#_terminal-auto-approval-experimental)) on July 9, I streamlined my Git workflow by integrating Copilot Chat’s new features, custom prompts, and the experimental terminal auto-approval system. This approach enables automation of reviewing changes, generating commit messages, and pushing updates, combining minimal manual intervention with maximum safety.

---

### Updated Version

A new and improved workflow is available—see [this blogpost](2025-07-25-Improved-Git-Workflow-Custom-Prompt-upcoming-VS-Code-change-warning.html) for the latest updates.

---

### Index

- Some visuals
- Key Features Used
- Workflow triggered by /pushall
- How I Set It Up
- Settings
- Prompts
- Python Delay Script for Safety
- Benefits
- References

---

## Some Visuals

To start the workflow, type `/pushall` in Copilot Chat. The process initiates as follows:

![Custom Prompts in VS Code](/assets/auto-approve-terminal-commands/slashcommand.png)

1. **Starting with /pushall command**
2. **Git Diff and Commit Suggestions:**
   Copilot uses 'git diff' and recommends a descriptive commit message.
   > _Suggested commit message: "Add links to VS Code v1.102 release notes and Copilot customization docs"_

This message clearly references the improvements made by adding relevant links.

![Aborting the workflow](/assets/auto-approve-terminal-commands/controlc.png)

- _Aborting with Ctrl+C_

![Workflow in action](/assets/auto-approve-terminal-commands/continued.png)

- _Workflow executing_

---

## Key Features Used

**Terminal Auto-Approval (Experimental):**

- **Auto-approve safe commands:**
  - Configured `github.copilot.chat.agent.terminal.allowList` to permit safe commands (e.g., `git status`, `git add`, `git commit`) without manual approval.
- **Deny-list for safety:**
  - Dangerous commands (e.g., `rm`, `curl`) are denied via the `denyList` for safety.

**Custom Prompts and Instructions:**

- **Prompt files:**
  - Reusable `.prompt.md` files in `.github/prompts` direct Copilot Chat through the custom workflow (review, commit, rebase, push) and are invoked by simple commands like `/pushall`.
- **Instruction files:**
  - `.github/copilot-instructions.md` enforces sensible defaults (like always using English), ensuring Copilot follows pre-defined preferences.
- **Agent mode:**
  - Enables Copilot Chat to execute multi-step workflows, automating all stages from staging changes to pushing commits on a single prompt.

For official docs, see [Customize Copilot with Instructions and Prompts](https://code.visualstudio.com/docs/copilot/copilot-customization).

---

## Workflow Triggered by /pushall

1. **Review changes:**
   - Copilot lists changed files and diffs (`git --no-pager diff`).
2. **Generate commit message:**
   - Copilot suggests a descriptive commit message based on the changes.
3. **Stage and commit:**
   - Safe commands (`git add .`, `git commit -m "..."`) are auto-approved and executed.
4. **Rebase and resolve conflicts:**
   - `git pull --rebase` is run; Copilot guides through conflict resolution, running only allowed commands.
5. **Push to remote:**
   - Executes `git push` if all prior steps succeed.

---

## How I Set It Up

### Settings

My workspace `settings.json` contains:

```json
{
  "github.copilot.chat.agent.terminal.allowList": {
    "git": true,
    "echo": true,
    "cd": true,
    "ls": true,
    "cat": true,
    "pwd": true,
    "dir": true,
    "Write-Host": true,
    "Set-Location": true,
    "Get-ChildItem": true,
    "Get-Content": true,
    "Get-Location": true,
    "less": true,
    "more": true,
    "python3": true
  },
  "github.copilot.chat.agent.terminal.denyList": {
    "rm": true,
    "rmdir": true,
    "del": true,
    "kill": true,
    "curl": true,
    "wget": true,
    "eval": true,
    "chmod": true,
    "chown": true,
    "Remove-Item": true
  }
}
```

### Prompts

The Git automation workflow is stored in `.github/prompts/pushall.prompt.md` and triggered with `/pushall` in chat.

```md
---
agent: 'agent'

---

# Step-by-Step Git Commit, Rebase, Conflict Resolution, and Push Workflow

Follow these steps exactly...

## Step 1: Review all changed files and suggest a commit message

- Use `git --no-pager diff` to show changes
- Suggest a descriptive commit message
- Notify the user: 10 seconds to abort before execution
- Run a Python delay script giving a window to intervene

## Step 2: Stage and commit all changes

- Stage: `git add .`
- Commit: `git commit -m "<commit message>"`

## Step 3: Pull and rebase

- `git pull --rebase`
- Resolve conflicts as needed

## Step 4: Push

- `git push`

_Summary_: This ensures your commits are reviewed, integrated with remote changes, all conflicts resolved, and your history remains clean.
```

### Python Delay Script for Safety

A custom Python script (`.github/prompts/pushall.10-second-delay.py`) ensures the user can abort before any commit/push occurs.

```python
import time
import sys
import signal

aborted = [False]

def handle_abort(signum, frame):
    aborted[0] = True

def main():
    print("\nYou have 10 seconds to abort this process (Ctrl+C or send SIGTERM) if you want to stop the push workflow.")
    print("If you do nothing, the workflow will continue and your changes will be committed and pushed.")
    print("If you abort, the workflow will stop and nothing will be pushed.\n")
    sys.stdout.flush()
    signal.signal(signal.SIGINT, handle_abort)
    signal.signal(signal.SIGTERM, handle_abort)
    start = time.time()
    while time.time() - start < 10:
        time.sleep(0.1)
        if aborted[0]:
            print("\nAborted by user. Exiting with code 1.")
            sys.exit(1)
    print("\nNo abort detected. Proceeding with the workflow. Exiting with code 0.")
    sys.exit(0)

if __name__ == "__main__":
    main()
```

This approach ensures there’s always an opportunity to review or cancel before pushing any changes.

---

## Benefits

- **Speed:** Single chat command for routine Git operations.
- **Safety:** Only approved commands run automatically, minimizing risk.
- **Consistency:** Prompts and instructions enforce process standards.

---

## References

- [VS Code v1.102 Release Notes](https://code.visualstudio.com/updates/v1_102#_terminal-auto-approval-experimental)
- [Customize Copilot with Instructions and Prompts](https://code.visualstudio.com/docs/copilot/copilot-customization)

_This article was co-written with GitHub Copilot Chat_

This post appeared first on "Reinier van Maanen's blog". [Read the entire article here](https://r-vm.com/automating-my-git-workflow-vscode-copilot-chat-terminal-auto-approval.html)
