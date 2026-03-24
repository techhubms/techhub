---
primary_section: github-copilot
section_names:
- ai
- github-copilot
tags:
- AI
- AI & ML
- AI Integration
- Application Development
- BYOK
- Caching
- Copilot Chat
- Copilot CLI
- Copilot SDK
- Developer Skills
- Environment Variables
- Express
- GitHub Copilot
- GitHub Copilot SDK
- GitHub Issue
- GitHub REST API
- Graceful Degradation
- Issue Triage
- JSON RPC
- Mobile App Architecture
- News
- Node.js
- OAuth
- Prompt Engineering
- React Native
- Server Side Integration
- Session Lifecycle
- Timeout Handling
title: Building AI-powered GitHub issue triage with the Copilot SDK
author: Andrea Griffiths
external_url: https://github.blog/ai-and-ml/github-copilot/building-ai-powered-github-issue-triage-with-the-copilot-sdk/
feed_name: The GitHub Blog
date: 2026-03-24 16:00:00 +00:00
---

Andrea Griffiths walks through building IssueCrush, a React Native app that uses the GitHub Copilot SDK (via a Node.js backend) to generate actionable GitHub issue summaries, covering architecture constraints, session lifecycle management, prompt structure, caching, and fallback behavior when AI is unavailable.<!--excerpt_end-->

# Building AI-powered GitHub issue triage with the Copilot SDK

The Copilot SDK lets you add the same AI that powers [Copilot Chat](https://copilot.microsoft.com/) to your own applications. Andrea Griffiths built an issue-triage app called **IssueCrush** to see what this looks like in practice and shares the patterns that made the integration workable.

## Problem: issue triage overload

Maintaining active GitHub repositories often means dealing with a pile of issues that include:

- Bugs
- Feature requests
- Questions better suited for discussions
- Duplicates (sometimes from years ago)

Triaging them is high-overhead context switching: reading titles and descriptions, checking labels, assessing priority, and deciding next steps.

## Enter IssueCrush: swipeable issue triage

IssueCrush presents issues as swipeable cards:

- Swipe left to close
- Swipe right to keep
- Tap **Get AI Summary** to have Copilot read the issue and return a short, actionable summary and recommendation

![Screenshot of a developer workspace showing a VS Code editor open to the IssueCrush project, with a package.json file visible and a terminal running a server that logs AI-generated issue summaries using the GitHub Copilot SDK. On the right, an iPhone simulator displays the IssueCrush mobile app, listing a GitHub issue with labels, a progress indicator, and a ‘Get AI Summary’ button, along with swipe-style approve and reject buttons at the bottom.](https://github.blog/wp-content/uploads/2026/03/Screenshot-2026-03-19-at-11.37.43-AM.png?resize=1305%2C835)

## The architecture challenge: where the Copilot SDK can run

A key constraint: **React Native apps can’t directly use Node.js packages**, and the **Copilot SDK requires a Node.js runtime**.

Internally, the SDK:

- Manages a local **Copilot CLI** process
- Communicates with it via **JSON-RPC**

Because the SDK depends on a CLI binary and Node, the integration needs to be **server-side**, not inside the React Native app.

### Chosen pattern: server-side SDK, client-side GitHub data access

![Architecture diagram showing a React Native and web client communicating over HTTPS with a Node.js server. The server runs the GitHub Copilot SDK, which manages a local Copilot CLI process via JSON-RPC. The CLI connects to the GitHub Copilot service, while the client separately interacts with GitHub OAuth and the GitHub REST API for issue data.](https://github.blog/wp-content/uploads/2026/03/Screenshot-2026-03-19-at-11.39.25-AM.png?resize=1318%2C921)

Why this setup works:

- **Single SDK instance shared across clients**, reducing overhead and repeated auth handshakes.
- **Server-side secrets for Copilot authentication**, keeping credentials off the client.
- **Graceful degradation** so triage still works if the Copilot service is down or times out.
- **Centralized logging and monitoring**, because prompts and responses pass through the server.

### Prerequisites

1. Install the [Copilot CLI](https://docs.github.com/en/copilot/how-tos/set-up/install-copilot-cli) on the server.
2. Have a GitHub Copilot subscription, or configure **BYOK**: [BYOK configuration docs](https://github.com/github/copilot-sdk/blob/main/docs/auth/byok.md).
3. Authenticate the CLI:
   - Run `copilot auth` on the server, or
   - Set the `COPILOT_GITHUB_TOKEN` environment variable.

## Implementing the Copilot SDK integration

The Copilot SDK uses a session-based model:

- Start a client (spawns the CLI)
- Create a session
- Send messages
- Clean up

```javascript
const { CopilotClient, approveAll } = await import('@github/copilot-sdk');

let client = null;
let session = null;

try {
  // 1. Initialize the client (spawns Copilot CLI in server mode)
  client = new CopilotClient();
  await client.start();

  // 2. Create a session with your preferred model
  session = await client.createSession({
    model: 'gpt-4.1',
    onPermissionRequest: approveAll,
  });

  // 3. Send your prompt and wait for response
  const response = await session.sendAndWait({ prompt });

  // 4. Extract the content
  if (response && response.data && response.data.content) {
    const summary = response.data.content;
    // Use the summary...
  }
} finally {
  // 5. Always clean up
  if (session) await session.disconnect().catch(() => {});
  if (client) await client.stop().catch(() => {});
}
```

## Key SDK patterns

## 1. Lifecycle management

Lifecycle sequence:

- `start()` → `createSession()` → `sendAndWait()` → `disconnect()` → `stop()`

Failing to clean up sessions can **leak resources**. Use `try/finally` around every session interaction.

The `.catch(() => {})` on cleanup avoids masking the original error with cleanup failures.

## 2. Prompt engineering for issue triage

Instead of dumping raw text, provide structured context:

```javascript
const prompt = `You are analyzing a GitHub issue to help a developer quickly understand it and decide how to handle it.

Issue Details:
- Title: ${issue.title}
- Number: #${issue.number}
- Repository: ${issue.repository?.full_name || 'Unknown'}
- State: ${issue.state}
- Labels: ${issue.labels?.length ? issue.labels.map(l => l.name).join(', ') : 'None'}
- Created: ${issue.created_at}
- Author: ${issue.user?.login || 'Unknown'}

Issue Body: ${issue.body || 'No description provided.'}

Provide a concise 2-3 sentence summary that:
1. Explains what the issue is about
2. Identifies the key problem or request
3. Suggests a recommended action (e.g., "needs investigation", "ready to implement", "assign to backend team", "close as duplicate")

Keep it clear, actionable, and helpful for quick triage. No markdown formatting.`;
```

A key observation: **labels and author context** can materially affect triage recommendations.

## 3. Response handling and timeouts

`sendAndWait()` returns once the session goes idle. Validate the response shape before reading nested properties.

```javascript
const response = await session.sendAndWait({ prompt }, 30000); // 30 second timeout

let summary;
if (response && response.data && response.data.content) {
  summary = response.data.content;
} else {
  throw new Error('No content received from Copilot');
}
```

The second argument is a timeout (ms). Set it high enough for complex issues, but low enough to avoid long spinner waits.

## Client-side service layer (React Native)

The mobile app wraps backend calls in a service class to handle initialization and error states.

```typescript
// src/lib/copilotService.ts
import type { GitHubIssue } from '../api/github';
import { getToken } from './tokenStorage';

export interface SummaryResult {
  summary: string;
  fallback?: boolean;
  requiresCopilot?: boolean;
}

export class CopilotService {
  private backendUrl = process.env.EXPO_PUBLIC_API_URL || 'http://localhost:3000';

  async initialize(): Promise<{ copilotMode: string }> {
    try {
      const response = await fetch(`${this.backendUrl}/health`);
      const data = await response.json();

      console.log('Backend health check:', data);
      return { copilotMode: data.copilotMode || 'unknown' };
    } catch (error) {
      console.error('Failed to connect to backend:', error);
      throw new Error('Backend server not available');
    }
  }

  async summarizeIssue(issue: GitHubIssue): Promise<SummaryResult> {
    try {
      const token = await getToken();
      if (!token) {
        throw new Error('No GitHub token available');
      }

      const response = await fetch(`${this.backendUrl}/api/ai-summary`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ issue, token }),
      });

      const data = await response.json();

      if (!response.ok) {
        if (response.status === 403 && data.requiresCopilot) {
          return {
            summary: data.message || 'AI summaries require a GitHub Copilot subscription.',
            requiresCopilot: true,
          };
        }
        throw new Error(data.error || 'Failed to generate summary');
      }

      return {
        summary: data.summary || 'Unable to generate summary',
        fallback: data.fallback || false,
      };
    } catch (error) {
      console.error('Copilot summarization error:', error);
      throw error;
    }
  }
}

export const copilotService = new CopilotService();
```

## React Native UI integration

The UI uses straightforward state management: call the service, then cache the result on the issue object.

```javascript
const [loadingAiSummary, setLoadingAiSummary] = useState(false);

const handleGetAiSummary = async () => {
  const issue = issues[currentIndex];
  if (!issue || issue.aiSummary) return;

  setLoadingAiSummary(true);
  try {
    const result = await copilotService.summarizeIssue(issue);
    setIssues(prevIssues =>
      prevIssues.map((item, index) =>
        index === currentIndex ? { ...item, aiSummary: result.summary } : item
      )
    );
  } catch (error) {
    console.error('AI Summary error:', error);
  } finally {
    setLoadingAiSummary(false);
  }
};
```

Once cached, revisiting the card shows the summary instantly without a second API call.

## Graceful degradation

AI services fail sometimes (network issues, rate limits, outages). The server handles two failure modes:

- **Subscription/auth problems** return a **403** so the client can show a clear message.
- Everything else falls back to a non-AI summary based on metadata.

```javascript
} catch (error) {
  // Clean up on error
  try {
    if (session) await session.disconnect().catch(() => {});
    if (client) await client.stop().catch(() => {});
  } catch (cleanupError) {
    // Ignore cleanup errors
  }

  const errorMessage = error.message.toLowerCase();

  // Copilot subscription errors get a clear 403
  if (
    errorMessage.includes('unauthorized') ||
    errorMessage.includes('forbidden') ||
    errorMessage.includes('copilot') ||
    errorMessage.includes('subscription')
  ) {
    return res.status(403).json({
      error: 'Copilot access required',
      message: 'AI summaries require a GitHub Copilot subscription.',
      requiresCopilot: true,
    });
  }

  // Everything else falls back to a metadata-based summary
  const fallbackSummary = generateFallbackSummary(issue);
  res.json({ summary: fallbackSummary, fallback: true });
}
```

Fallback summary generation:

```javascript
function generateFallbackSummary(issue) {
  const parts = [issue.title];

  if (issue.labels?.length) {
    parts.push(`\nLabels: ${issue.labels.map(l => l.name).join(', ')}`);
  }

  if (issue.body) {
    const firstSentence = issue.body.split(/[.!?]\s/)[0];
    if (firstSentence && firstSentence.length < 200) {
      parts.push(`\n\n${firstSentence}.`);
    }
  }

  parts.push('\n\nReview the full issue details to determine next steps.');
  return parts.join('');
}
```

## Additional implementation notes

- A `/health` endpoint indicates whether AI is available; clients can hide the AI summary button entirely.
- Summaries are generated **on-demand**, not precomputed, to control costs.
- The SDK is loaded via `await import('@github/copilot-sdk')` rather than top-level `require`, so the server can start even if the SDK has issues.

## Dependencies

```json
{
  "dependencies": {
    "@github/copilot-sdk": "^0.1.14",
    "express": "^5.2.1"
  }
}
```

You need the [Copilot CLI](https://docs.github.com/en/copilot/how-tos/set-up/install-copilot-cli) installed and available on the system PATH. For Node.js requirements, see the [SDK repository](https://github.com/github/copilot-sdk).

## What the author learned

- **Server-side integration is the right call** (CLI + Node runtime; keep credentials on the backend).
- **Prompt structure beats prompt length** (organized metadata improves summaries).
- **Always provide a fallback** (AI should not be a single point of failure).
- **Clean up sessions** (`disconnect()` then `stop()`; use `try/finally`).
- **Cache summaries** (avoid repeated calls and latency).

## Try it yourself

- Source: [AndreaGriffiths11/IssueCrush](https://github.com/AndreaGriffiths11/IssueCrush)
- SDK: [Get started with the Copilot SDK](https://github.com/github/copilot-sdk)
- Guide: [Getting Started](https://github.com/github/copilot-sdk#getting-started)
- Community: [SDK discussions](https://github.com/github/copilot-sdk/discussions)


[Read the entire article](https://github.blog/ai-and-ml/github-copilot/building-ai-powered-github-issue-triage-with-the-copilot-sdk/)

