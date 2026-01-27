---
external_url: https://devblogs.microsoft.com/blog/jsts-modernizer-preview
title: AI-Assisted JavaScript/TypeScript Modernizer Preview in VS Code Insiders
author: Sayed Ibrahim Hashimi
feed_name: Microsoft Blog
date: 2025-12-09 17:41:29 +00:00
tags:
- AI Assistance
- App Modernization
- Code Modernization
- Copilot Chat
- Dependency Upgrades
- Extension
- JavaScript
- Node.js
- npm
- Package.json
- TypeScript
- VS Code
- VS Code Insiders
section_names:
- ai
- coding
- github-copilot
primary_section: github-copilot
---
Sayed Ibrahim Hashimi presents a preview of the JavaScript/TypeScript Modernizer for VS Code Insiders, demonstrating how developers can leverage GitHub Copilot to automate dependency upgrades and code modernization tasks.<!--excerpt_end-->

# AI-Assisted JavaScript/TypeScript Modernizer Preview in VS Code Insiders

Keeping JavaScript and TypeScript projects current is a significant challenge, particularly when it involves upgrading npm dependencies or adopting new frameworks. The JavaScript/TypeScript Modernizer, featured in the GitHub Copilot App Modernization extension for Visual Studio Code Insiders, streamlines this process by harnessing GitHub Copilot's AI capabilities.

## What is the JS/TS Modernizer?

- **AI-driven Upgrade Helper:** Uses GitHub Copilot under the hood to analyze your project and propose an upgrade plan.
- **Automated Dependency Updates:** Automatically upgrades npm packages by updating version numbers in package.json and running installations.
- **Code Change Suggestions:** Offers guidance and applies code changes required for compatibility with updated packages.
- **Interactive Copilot Chat:** Guides users through each modernization step, allowing for approvals and feedback.

## Getting Started

1. **Requirements:**
   - VS Code Insiders (Modernizer available only on Insiders build for now)
   - Node.js and npm installed
   - GitHub Copilot account enabled in VS Code
   - GitHub Copilot App Modernization (Preview) extension installed from the [Marketplace](https://marketplace.visualstudio.com/items?itemName=vscjava.migrate-java-to-azure)
   - Enable the experimental Modernizer setting by adding `"appmod.experimental.task.typescript.upgrade": true` in your VS Code settings JSON, then restart VS Code.

2. **Modernization Process:**
   - Open your JS/TS project folder in VS Code (ensure package.json is present).
   - Access the *GitHub Copilot App Modernization* panel from the Activity Bar.
   - Click "Upgrade npm Packages" to start the workflow.
   - Watch as Copilot Chat walks through:
     - Analyzing outdated dependencies
     - Proposing updates
     - Applying changes to package.json and source files
     - Suggesting code modifications for breaking API changes
     - Iterating with build/test verification
   - Review the changes using source control. Nothing is auto-committed; you control final approval.

## Known Issues and Tips

- Only supports one project at a time (best to upgrade projects individually in mono-repo setups).
- Being a preview feature, expect some issues or incomplete modernizations. Feedback is welcomed at [webtoolsoutreach@microsoft.com](mailto:webtoolsoutreach@microsoft.com).

## Conclusion

The JS/TS Modernizer, powered by GitHub Copilot, reduces manual work and simplifies the process of upgrading and modernizing JavaScript/TypeScript applications in VS Code Insiders. The interactive Copilot Chat experience helps developers update dependencies and code securely, efficiently, and with confidence.

**Ready to modernize your app?** Install the [GitHub Copilot App Modernization (Preview) extension](https://marketplace.visualstudio.com/items?itemName=vscjava.migrate-java-to-azure), enable the Modernizer setting, and try it on your project today.

This post appeared first on "Microsoft Blog". [Read the entire article here](https://devblogs.microsoft.com/blog/jsts-modernizer-preview)
