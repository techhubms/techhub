---
layout: "post"
title: "GitHub Copilot Extensions: Building Custom Tools to Enhance Developer Productivity"
description: "Rob Bos presents a comprehensive guide to creating and deploying custom GitHub Copilot Extensions. This post covers extension types, step-by-step extension development, integrating with APIs and knowledge bases, leveraging the Copilot Extension SDK, working within Codespaces, and handling user interactions securely."
author: "Rob Bos"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devopsjournal.io/blog/2024/09/14/GitHub-Copilot-Extensions"
viewing_mode: "external"
feed_name: "Rob Bos' Blog"
feed_url: "https://devopsjournal.io/blog/atom.xml"
date: 2024-09-14 00:00:00 +00:00
permalink: "/2024-09-14-GitHub-Copilot-Extensions-Building-Custom-Tools-to-Enhance-Developer-Productivity.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["AI", "APIs", "Blogs", "Codespaces", "Copilot Extension SDK", "Copilot Extensions", "Custom Extensions", "Developer Productivity", "DevOps", "GitHub Apps", "GitHub Copilot", "LLM", "OpenAI", "Retrieval Augmented Generation", "User Confirmation", "VS Code"]
tags_normalized: ["ai", "apis", "blogs", "codespaces", "copilot extension sdk", "copilot extensions", "custom extensions", "developer productivity", "devops", "github apps", "github copilot", "llm", "openai", "retrieval augmented generation", "user confirmation", "vs code"]
---

In this detailed article, Rob Bos explores how developers can create custom GitHub Copilot Extensions. He describes extension types, provides a development walkthrough, and offers practical tips for integrating APIs and managing user interactions within Copilot Chat.<!--excerpt_end-->

Date posted: **14 Sep 2024**, 6 minutes to read

# GitHub Copilot Extensions

GitHub Copilot is evolving beyond code completion, now empowering developers to write their own extensions to meet tailored needs. In his in-depth guide, Rob Bos discusses how Copilot Extensions work, their types, practical examples, and step-by-step instructions for building and deploying them.

![Announcement image](/images/2024/20240914/Announcement.png)

## What are Copilot Extensions?

Copilot Extensions enable you to expand Copilot’s capabilities, integrating it with your own knowledge sources, APIs, or workflows, all from within GitHub Copilot Chat. Some ways to use these:

- **@workspace/#solution** – Access solutions in your IDE
- **@github** – Query the GitHub API directly to retrieve issues or pull requests (currently for Copilot Enterprise)

## Types of Copilot Extensions

1. **VS Code Extensions for Copilot:**
   - Extend Copilot in your IDE, with access to IDE resources like open files.
   - Currently, these only work in VS Code.
   - Example: The `@azure` extension (currently in private preview) allows integration with an Azure knowledge base. Learn more in [Matt Olson’s post](https://github.com/molson504x/copilot-custom-extension).

2. **GitHub Copilot Extensions:**
   - Hosted online; callable from Copilot Chat in any supported UI (VS Code, github.com web UI).
   - Focus of this post.

## Getting Started: Writing Your Own Copilot Extension

To begin developing an extension:

- Read the [Copilot Extensions announcement](https://github.blog/news-insights/product-news/introducing-github-copilot-extensions/).
- Check the [official documentation](https://docs.github.com/en/copilot/building-copilot-extensions/about-building-copilot-extensions).
- Register a new GitHub App (can be public or private to your org) and configure its Copilot section.

Set up your GitHub App with a **public endpoint** which Copilot will use to send requests. Creating a [Codespace](https://github.com/features/codespaces) and using its public URL makes development and debugging straightforward.

![Screenshot of the Codespace URL GitHub App](/images/2024/20240914/app%20settings.png)

You can find many extension samples in the [copilot-extensions GitHub organization](https://github.com/copilot-extensions):

- **Blackbeard extension:** Returns pirate-themed responses
- **RAG extension:** Implements retrieval augmented generation
- **Function calling extension:** Parses user prompts and calls relevant functions
- **GitHub Models extension:** Interacts with LLMs via new model APIs

While these are helpful as references, advanced scenarios may encounter limitations.

## Using the Copilot Extension SDK

Rob recommends leveraging the [Copilot Extension SDK](https://github.com/copilot-extensions/preview-sdk.js), which (despite being alpha) streamlines extension development and solves many common problems. The repository contains numerous examples.

**Developing the Extension**

- Refer to the SDK repo's `README.md` under the `examples` folder.
- Install dependencies (`npm install`) and start the watcher (`npm run watch`).
- Make your Codespace port public and set the port's URL in the GitHub App settings.

Key technical details:

- Handle `POST` events for incoming requests.
- Send responses using `createTextEvent` and indicate completion with `createDoneEvent`.
- Optionally, use `createAckEvent` to notify users that you're processing their request (useful in long-running queries).

**Sample Pattern:**

```javascript
import { prompt, createAckEvent, createDoneEvent, createTextEvent, parseRequestBody } from "@copilot-extensions/preview-sdk";

const tokenForUser = request.headers['x-github-token'];
const payload = parseRequestBody(body);

let result = await prompt(payload_message.content, {
  messages: payload.messages,
  token: tokenForUser
});
response.write(createTextEvent(result.message.content));
response.end(createDoneEvent());
console.log('Response sent');
```

## Running and Debugging Extensions in Codespaces

Developing inside a GitHub Codespace lets you easily receive traffic and debug your extension. Use a separate GitHub App for development vs production, as all app traffic is routed to your endpoint during testing.

![Running in Codespace](/images/2024/20240914/Running-the-extension.png)

Remember to:

- Make the Codespace port public
- Set the URL in both GitHub App settings and the Copilot section
- Ensure your App is flagged for Copilot extensions in its settings

![Codespace settings](/images/2024/20240914/AppSettings.png)

## Invoking and Working with Extensions

Once the GitHub App is installed in an org with Copilot enabled, users can invoke the extension by starting prompts with `@your-app-name`. Only one extension can interact per thread for security reasons and receives the full conversation context.

![Prompt invocation example](/images/2024/20240914/01-Invoking-the-extension.png)

Extensions currently work in VS Code, VS Code Insiders, and github.com web interface. GitHub Copilot's content filter ensures only suitable prompts and safe responses are sent to and from the extension.

When first used, an extension prompts users for consent to access conversation content and basic user info (GitHub handle, approximate location). This enables personalized responses and can be revoked via [GitHub settings](https://github.com/settings/installations).

![Consent dialog screenshot](/images/2024/20240914/Allow-prompt.png)

## Advanced Use Cases: System Prompts and Confirmation Dialogs

You can embed system prompts or implement confirmation dialogs in your extension logic:

**Adding a System Prompt:**

```javascript
import { prompt, createAckEvent, createDoneEvent, createTextEvent, parseRequestBody } from "@copilot-extensions/preview-sdk";
// see above for sample usage
```

**User Confirmation Example:**

```javascript
import { getUserConfirmation } from "@copilot-extensions/preview-sdk";

response.write(
  createConfirmationEvent({
    id: "123",
    title: "Are you sure?",
    message: "Create an issue with the missing API surface."
  })
);
```

![User confirm dialog](/images/2024/20240914/PromptAccept.png)

## Conclusion & Outlook

GitHub Copilot Extensions unlock custom capabilities directly within Copilot Chat. You can:

- Connect to knowledge stores, APIs, databases, or your own LLM.
- Build agent-like flows to automate development tasks or information retrieval.
- Leverage user confirmation for sensitive or multi-step actions.

Rob plans to integrate Copilot Extensions with internal knowledge bases and encourages others to share extension ideas.

> "What ideas do you have to build with this? Let me know in the comments below!"

---

*Written by Rob Bos. For more, visit [Xpirit profile](https://www.xpirit.com/xpiriter/rob).*

This post appeared first on "Rob Bos' Blog". [Read the entire article here](https://devopsjournal.io/blog/2024/09/14/GitHub-Copilot-Extensions)
