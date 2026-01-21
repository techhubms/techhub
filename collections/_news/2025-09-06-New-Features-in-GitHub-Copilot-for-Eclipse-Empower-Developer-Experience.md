---
external_url: https://github.blog/changelog/2025-09-05-new-features-in-github-copilot-in-eclipse
title: New Features in GitHub Copilot for Eclipse Empower Developer Experience
author: Allison
feed_name: The GitHub Blog
date: 2025-09-06 02:56:23 +00:00
tags:
- Agent Mode
- API Integration
- Code Generation
- Copilot Chat
- Custom Instructions
- Developer Tools
- Eclipse
- IDE Integration
- Java Development
- Plugin Development
- Programming Productivity
- Spring Tools
- User Experience
- Workflow Enhancement
section_names:
- ai
- coding
- github-copilot
---
Allison highlights the latest GitHub Copilot improvements for Eclipse, detailing features like custom instructions, image-based chat context, richer workflow integration, and a new plugin API to help developers code smarter.<!--excerpt_end-->

# New Features in GitHub Copilot for Eclipse

GitHub Copilot in Eclipse just received a substantial update focused on delivering a smarter, more customizable development experience. Allison outlines the newest features and workflow changes that help Eclipse users get the most out of AI-powered coding.

## ✨ What's New

### Custom Instructions

Developers can now provide GitHub Copilot with custom instructions to tailor its suggestions and behavior to their unique context. This update helps Copilot deliver more targeted, relevant code completions within Eclipse. Learn more in the [official documentation](https://docs.github.com/copilot/how-tos/configure-custom-instructions/add-repository-instructions?tool=eclipse).

> _Currently available in Chat and Agent mode only._

### Image Support in Chat

Copilot now lets you incorporate images into the chat context. For example, upload a hand-drawn layout, and Copilot will generate the appropriate HTML code. This bridges the gap between visual concepts and actual implementation, directly within your IDE.

- Note: Not all underlying models support vision capabilities; unsupported cases are clearly flagged.

### Richer Context with Folders and Resources

You can now:

- Attach entire folders to the Copilot chat for broader project context
- Drag and drop resources into referenced files
- Use context menus in Package Explorer and Project Explorer for quick additions

These capabilities make it easier to collaborate and share structured project content during coding sessions.

### Improved User Experience

User feedback has shaped several workflow refinements:

- Confirmation dialog on new Agent mode conversations safeguards unsaved work
- Chat view now shows conversation titles and easier access to preferences
- Improved color schemes for both dark and light themes
- Copilot Chat is now included in Java EE and related perspectives by default
- Preference enhancements let you control What’s New page visibility

### Reduced Plugin Size

Splitting platform-specific binaries into fragments has led to a smaller overall plugin package—downloads and updates are now quicker and more efficient.

### New Public API for Chat Session Integration

A new public API lets plugin developers automatically start Copilot chat sessions:

- `com.microsoft.copilot.eclipse.commands.openChatView.inputValue`: Initial chat content
- `com.microsoft.copilot.eclipse.commands.openChatView.autoSend`: Automate chat submission

Example: The Spring Tools plugin uses this API to launch Copilot-powered chat directly from its workflow, enabling deeper IDE integration.

## 🛠 Try It Out

Start using these enhancements by exploring the [documentation](https://docs.github.com/copilot/how-tos/chat/asking-github-copilot-questions-in-your-ide?tool=eclipse).

## 💬 Share Feedback

User feedback continues to drive Copilot improvements. Report bugs or ideas in the following channels:

- [GitHub Copilot for Eclipse discussions](https://github.com/orgs/community/discussions/151288)
- [GitHub Copilot for Eclipse issues](https://github.com/microsoft/copilot-eclipse-feedback/issues)

---

These updates collectively empower Eclipse developers to work more efficiently and customize Copilot to their unique workflow needs.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-05-new-features-in-github-copilot-in-eclipse)
