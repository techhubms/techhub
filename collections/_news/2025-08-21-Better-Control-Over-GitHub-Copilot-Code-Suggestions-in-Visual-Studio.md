---
layout: post
title: Better Control Over GitHub Copilot Code Suggestions in Visual Studio
author: Simona Liao
canonical_url: https://devblogs.microsoft.com/visualstudio/better-control-over-your-copilot-code-suggestions/
viewing_mode: external
feed_name: Microsoft DevBlog
feed_url: https://devblogs.microsoft.com/visualstudio/tag/github-copilot/feed/
date: 2025-08-21 16:00:37 +00:00
permalink: /github-copilot/news/Better-Control-Over-GitHub-Copilot-Code-Suggestions-in-Visual-Studio
tags:
- Code Completions
- Code Suggestions
- Developer Productivity
- Editor Shortcuts
- IDE
- IntelliCode
- Next Edit Suggestions
- Partial Accept
- Programming Tools
- VS
section_names:
- ai
- coding
- github-copilot
---
Simona Liao presents updates that give developers more nuanced control over GitHub Copilot suggestions in Visual Studio. The article covers new options, shortcuts, and configurations improving focus and customization.<!--excerpt_end-->

# Better Control Over GitHub Copilot Code Suggestions in Visual Studio

GitHub Copilot continues to evolve as an AI-powered coding assistant, now offering developers more precise control over when and how code suggestions appear in Visual Studio 2022.

## Enhanced Control Features

Developers often find code suggestions helpful but sometimes intrusive. Responding to community feedback, Visual Studio now offers several features to improve the coding experience:

### 1. Minimize Distractions While Coding

- **Pause Completions When Typing:** A new debounce option delays Copilot suggestions until you pause typing, reducing interruptions.
- **Access:** Go to `Tools -> Options -> IntelliCode -> Advanced` and enable *wait for pauses in typing before showing whole line completions*.

### 2. Manual Trigger for Code Completions

- **Disable Automatic Suggestions:** Turn off automatic completions and trigger them only via keyboard shortcuts (`Alt + ,` or `Alt + .`).
- **Configuration:** Navigate to `Tools > Options > IntelliCode > General` and uncheck *Automatically generate code completions in the Editor*.
- **Experience:** A hint bar appears while Copilot generates suggestions; use Tab to accept available completions.

### 3. Manage Next Edit Suggestions (NES)

- **Hide and Reveal NES:** Next Edit Suggestions (NES) can now remain hidden by default until you’re ready to review.
- **Indicator:** A margin symbol in the editor gutter alerts you when NES are available. Click the indicator or press Tab to view, then Tab or ESC to accept or dismiss, respectively.
- **Further configuration:** Visit `Tools > Options > GitHub > Copilot > Copilot Completions` and check *Collapse Next Edit Suggestions*.

### 4. Partial Acceptance of Code Completions

- **Word-by-Word and Line-by-Line:** Accept Copilot suggestions incrementally:
  - `Ctrl + Right Arrow` for word-by-word acceptance.
  - `Ctrl + Down Arrow` for line-by-line acceptance.
- **Margin Options:** Margin indicators allow partial acceptance via UI as well.
- **Disabling:** To revert, go to `Tools > Options > IntelliCode > Advanced -> Whole-line completions`.

### 5. Community Feedback

This release implements features requested by developers in the [Developer Community](https://developercommunity.visualstudio.com/t/Copilot-completions-are-too-intrusive/10810929#T-ND10921637) and through surveys, aiming to ensure a comfortable and powerful Copilot experience.

## Useful Resources

- [Visual Studio Hub](https://visualstudio.microsoft.com/hub/) – centralized news, updates, and discussions
- [Next Edit Suggestions Documentation](https://learn.microsoft.com/en-us/visualstudio/ide/copilot-next-edit-suggestions?view=vs-2022)
- [Partial Accept Demo Video](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/08/partial-accept.mp4)
- [Completions on Demand Demo Video](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/08/completions-on-demand.mp4)

## Appreciation

Simona Liao encourages ongoing feedback from the developer community to further refine GitHub Copilot and Visual Studio for all coding workflows.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/better-control-over-your-copilot-code-suggestions/)
