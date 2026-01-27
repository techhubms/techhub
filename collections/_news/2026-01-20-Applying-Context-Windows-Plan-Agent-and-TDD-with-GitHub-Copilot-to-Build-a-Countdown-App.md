---
external_url: https://github.blog/developer-skills/application-development/context-windows-plan-agent-and-tdd-what-i-learned-building-a-countdown-app-with-github-copilot/
title: Applying Context Windows, Plan Agent, and TDD with GitHub Copilot to Build a Countdown App
author: Chris Reddington
feed_name: The GitHub Blog
date: 2026-01-20 17:00:00 +00:00
tags:
- AI Agents
- Animation
- Application Development
- Context Window Management
- Copilot Chat
- Countdown App
- Custom Agents
- Developer Skills
- Fireworks.js
- JavaScript
- Open Source
- Plan Agent
- Rubber Duck Thursdays
- Software Architecture
- Tailwind CSS
- TDD
- Test Driven Development
- Tutorial
- TypeScript
- UI Performance
- Vite
- VS Code
section_names:
- ai
- coding
- github-copilot
primary_section: github-copilot
---
Chris Reddington guides readers through his journey of building a celebratory countdown app with GitHub Copilot. The piece highlights context window management, leveraging the Plan agent, and practicing TDD, offering actionable advice for developer workflows.<!--excerpt_end-->

# Applying Context Windows, Plan Agent, and TDD with GitHub Copilot to Build a Countdown App

## Introduction

In this case study, Chris Reddington shares his experience from a live Rubber Duck Thursdays stream, building a countdown app using GitHub Copilot and a variety of Copilot agents. The process covers practical AI integration, context engineering, requirement clarification, modularity, and TDD (Test-Driven Development) — all in the public eye with an engaged developer community.

## Key Learnings and Techniques

### 1. Context Window Management

- Chris emphasizes the importance of keeping AI sessions focused by starting new Copilot Chat sessions when old context is irrelevant.
- Only essential information from prior sessions (saved custom instructions and relevant requirements) is preserved to avoid crowding the AI's context window.

### 2. Requirement Discovery with the Plan Agent

- Leveraging Copilot's Plan agent helped transform rough, ambiguous requirements (like interactive time zone selectors) into actionable development plans.
- The Plan agent asks clarifying questions, surfaces edge cases, and supports structured architectural decision-making.

### 3. Test-Driven Development (TDD) with Copilot

- Chris adopted a TDD workflow, writing failing test cases first with Copilot before implementing the features.
- This included core countdown logic, time zone utilities, state management, and celebration handling at year rollovers.
- Copilot suggested and iterated on tests, revealing and correcting logic bugs (e.g., year rollover celebrations).

### 4. Modular, Iterative Implementation

- The app was architected in well-defined modules:
  - `src/countdown.ts`: Pure logic (dates, time unit calculation)
  - `src/main.ts`: DOM manipulation and timers
- Animations (fireworks, starfields), theme controllers, and performance were considered from the outset.
- The world map feature highlighted iterative development: an initially failed implementation was left in for authenticity, with plans for future refactor.

### 5. Custom Copilot Agents

- Specialized Copilot agents assisted with UI performance, architectural review, and accessibility considerations (reduced motion, animation tuning)
- Examples included the UI Performance Specialist, demonstrating extensibility of AI assistance in developer workflows.

### 6. Community Engagement & Multi-Theme Features

- Viewer suggestions influenced features (e.g., time zone selection, celebration triggers)
- Multiple themes (city skyline and GitHub contribution graph) and open sourcing were incorporated

### 7. Practical Tools and Architectures Used

- [Vite](https://github.com/vitejs/vite), [TypeScript](https://github.com/microsoft/TypeScript), and [Tailwind CSS v4](https://github.com/tailwindlabs/tailwindcss) for project scaffolding and styling
- [Fireworks.js](https://github.com/crashmax-dev/fireworks-js) for celebratory visuals
- Context management via Copilot Chat sessions and custom instruction files

## Live Demo and Repository

- Live app: [https://chrisreddington.github.io/timestamp](https://chrisreddington.github.io/timestamp)
- Source code: [http://github.com/chrisreddington/timestamp](http://github.com/chrisreddington/timestamp)

## Practical Takeaways

- **Context engineering** is as crucial as prompt engineering; keep your AI session focused.
- **Use AI agents to clarify requirements** and surface edge cases early.
- **TDD with Copilot bridges quality gaps** caused by complex, iterative requirements.
- **Embrace visible failures and iterations**; authentic development includes mishaps.
- **Iterate on feature requests live** — audience input can improve and shape outcomes.

## Conclusion

This article demonstrates how integrating Copilot's advanced features (like Plan agents, custom agents, and context management) with modern development best practices leads to more robust, well-tested, and enjoyable software. Genuine, iterative building — including visible bugs and laughable fails — foster learning and make the development process inspiring and accessible.

---

*By Chris Reddington. For more, join the Rubber Duck Thursdays stream or check out the open source [Timestamp app](https://github.com/chrisreddington/timestamp/).*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/developer-skills/application-development/context-windows-plan-agent-and-tdd-what-i-learned-building-a-countdown-app-with-github-copilot/)
