---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/adding-ai-personality-to-browser-games/ba-p/4490892
title: Adding AI Personality to Browser Games with Microsoft Foundry Local
author: Lee_Stott
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-10 08:00:00 +00:00
tags:
- AI
- AI Integration
- Asynchronous Design
- Browser Games
- Community
- Context Aware AI
- Event Driven Architecture
- Game Development
- JavaScript
- Microsoft Foundry Local
- Node.js
- Open Source
- Progressive Enhancement
- Real Time Commentary
- SLM
- Small Language Models
- Space Invaders
section_names:
- ai
---
Lee Stott explains how to enhance browser games with AI-powered dynamic commentary using Microsoft Foundry Local. The post details a JavaScript-based architecture for adding real-time, context-aware feedback to games.<!--excerpt_end-->

# Adding AI Personality to Browser Games with Microsoft Foundry Local

## Introduction

Browser games often deliver a static, predictable user experience with fixed game messages and scripted NPC responses. Lee Stott explores how to move beyond static responses by integrating real-time, AI-powered commentary into a classic Space Invaders-style browser game using [Spaceinvaders-FoundryLocal](https://github.com/leestott/Spaceinvaders-FoundryLocal), vanilla JavaScript, and [Microsoft Foundry Local](https://foundrylocal.ai).

This approach teaches how to:

- Integrate local AI into client-side games (no cloud dependency)
- Design personality-driven AI systems that enhance gameplay
- Implement context-aware commentary that adapts to game state
- Build resilient architectures with optional, non-blocking AI features

## Why Use Local AI for Games?

Cloud-based AI increases operational costs, introduces privacy concerns, restricts offline play, and adds latency to real-time experiences. By running Small Language Models (SLMs) locally with Foundry Local, games avoid API costs, protect player privacy, and provide near-instant inference, making real-time and offline play practical—even on modest hardware. SLMs are ideal for generating entertaining, contextually relevant commentary and can be included as an architectural enhancement rather than a critical dependency.

## Architecture Overview: Progressive Enhancement

The solution employs progressive enhancement:

- **Zero-dependency core**: The core game, written in vanilla JavaScript, is fully functional without AI.
- **Optional AI Layer**: A local Node.js proxy connects to Foundry Local, enabling AI commentary. The game polls this backend for commentary based on gameplay events like waves completed, accuracy, combo streaks, and power-up use.
- **Graceful degradation**: If AI is unavailable, the game uses generic fallback messages instead.

**Flow Example:**

1. Player completes an event (e.g., defeats a wave)
2. The game sends context (score, accuracy, wave) to the local proxy
3. The backend builds a context-rich prompt and requests a completion from Foundry Local
4. The response is injected as dynamic in-game commentary

## AI Module Example

```javascript
// llm.js - AI integration module
export class GameAI {
  constructor() {
    this.baseURL = 'http://localhost:3001';
    this.available = false;
    this.checkAvailability();
  }
  async checkAvailability() {
    try {
      const response = await fetch(`${this.baseURL}/health`, { method: 'GET', timeout: 2000 });
      this.available = response.ok;
      return this.available;
    } catch (error) {
      this.available = false;
      return false;
    }
  }
  async generateComment(gameContext) {
    if (!this.available) return this.getFallbackComment(gameContext.event);
    try {
      const response = await fetch(`${this.baseURL}/api/comment`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(gameContext)
      });
      if (!response.ok) throw new Error('AI request failed');
      const data = await response.json();
      return data.comment;
    } catch (error) {
      return this.getFallbackComment(gameContext.event);
    }
  }
  getFallbackComment(event) {
    const fallbacks = {
      wave_complete: 'Wave cleared!',
      player_hit: 'Shields damaged!',
      game_over: 'Game Over. Try again!',
      high_score: 'New high score!',
      power_up: 'Power-up collected!'
    };
    return fallbacks[event] || 'Good job!';
  }
}
```

## Backend Proxy Example

The backend uses Express and foundry-local-sdk:

```javascript
// server.js - Node.js backend proxy
import express from 'express';
import { FoundryLocalClient } from 'foundry-local-sdk';

const app = express();
const foundry = new FoundryLocalClient({ endpoint: process.env.FOUNDRY_LOCAL_ENDPOINT || 'http://127.0.0.1:5272' });

app.use(express.json());
app.use(express.cors());

// Health check endpoint
app.get('/health', (req, res) => {
  res.json({ status: 'AI available', model: 'phi-3.5-mini' });
});

// Commentary request endpoint
app.post('/api/comment', async (req, res) => {
  const { event, score, accuracy, wave, lives, combo } = req.body;
  const prompt = buildCommentPrompt(event, { score, accuracy, wave, lives, combo });
  try {
    const completion = await foundry.chat.completions.create({
      model: 'phi-3.5-mini',
      messages: [
        { role: 'system', content: `You are an AI commander providing brief, encouraging commentary for a Space Invaders game. Be energetic, supportive, and sometimes humorous. Keep responses to 1-2 sentences maximum. Reference specific game metrics when relevant.` },
        { role: 'user', content: prompt }
      ],
      temperature: 0.9,
      max_tokens: 50
    });
    const comment = completion.choices[0].message.content.trim();
    res.json({ comment, model: 'phi-3.5-mini', timestamp: new Date().toISOString() });
  } catch (error) {
    res.status(500).json({ error: 'Commentary generation failed' });
  }
});
```

## Game Loop Integration

AI requests are never allowed to block the game's main loop, maintaining smooth gameplay at 60 FPS. AI comments are requested asynchronously every few seconds after significant events.

## AI Personality and Contextual Responses

A personality system ensures that AI commentary is consistent, concise, and contextually relevant—reflecting player metrics such as accuracy and lives, and shifting tone according to circumstances (success, failure, combo, etc.).

## Key Takeaways

- AI commentary adds engagement and personalization to browser games without compromising performance
- Local execution with Microsoft Foundry Local brings cloud-grade AI experiences fully on-device
- Modular, event-driven integration guarantees maximum compatibility and scalability

**Full implementation and resources:**

- [Spaceinvaders-FoundryLocal GitHub](https://github.com/leestott/Spaceinvaders-FoundryLocal)
- [Microsoft Foundry Local Documentation](https://foundrylocal.ai)
- [MDN Game Development](https://developer.mozilla.org/en-US/docs/Games)

---

*Authored by Lee Stott. Explore the complete example and start building your own AI-powered browser games!*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/adding-ai-personality-to-browser-games/ba-p/4490892)
