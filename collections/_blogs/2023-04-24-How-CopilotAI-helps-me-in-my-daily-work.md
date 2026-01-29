---
external_url: https://devopsjournal.io/blog/2023/04/24/how-Copilot-helps-me-in-my-daily-work
title: How Copilot/AI helps me in my daily work
author: Rob Bos
feed_name: Rob Bos' Blog
date: 2023-04-24 00:00:00 +00:00
tags:
- AI Code Generation
- Automation
- Azure Functions
- Deployment Protection Rules
- GitHub Actions
- Node.js
- OAuth
- AI
- Azure
- GitHub Copilot
- Coding
- Blogs
section_names:
- ai
- azure
- github-copilot
- coding
primary_section: github-copilot
---
In this article, Rob Bos shares insights into leveraging AI tools, especially GitHub Copilot, to accelerate application and code generation workflows, focusing on GitHub Actions deployment protection and app setup. <!--excerpt_end-->

## Introduction

Rob Bos recounts an experience during an innovation day at work, where he needed to both generate extra code and build a new application quickly. The primary goal was to try out GitHub's new Deployment Protection Rules for controlling when GitHub Actions jobs roll out to specific environments.

## Setting Up Deployment Protection with GitHub Apps

Deployment Protection Rules require creating a GitHub App, which handles incoming webhook payloads when a target environment is reached. Rob already operated an Azure Function App and decided to add an HttpTrigger function for logging and inspecting webhook payloads—crucial since documentation on these payloads was unavailable. Here, AI tools played a transformative role:

- **Code Generation:** Instead of manually searching for documentation and boilerplate, Rob employed GitHub Copilot. Copilot provided correct function definitions and payload logging setup, helping him publish the function quickly and connect it to a GitHub App.
- **Payload Analysis:** The deployed function allowed logging and examination of webhook payloads, which included details like trigger context and callback URLs, streamlining experimentation and validation.

## Building a Node.js App for User Configuration

The project also required a new Node.js web application for user environment configuration and authentication via GitHub OAuth:

- **AI-Enhanced Development:** Although not a seasoned JavaScript developer, Rob used AI (such as Bing integration in Edge) to scaffold a Node.js app with GitHub authentication and configuration storage. AI-generated code snippets were often functional and could be copied directly with minimal edits.
- **Rapid Debugging:** When encountering errors, Rob was able to consult Bing AI for solutions, speeding up troubleshooting and iterative enhancement.

## Security Considerations with AI-Generated Code

While AI-powered tools significantly sped up app creation, Rob notes generated code wasn’t flawless—AI sometimes introduced security issues, such as safe credential handling. He stresses that security validation steps, like using GitHub Advanced Security and CodeQL scans, remain crucial, especially as AI is not infallible in adhering to best security practices. Rob also references his LinkedIn Learning course for deeper dives into GitHub Advanced Security.

## Conclusion

With AI assistance, Rob completed most of his setup—including an Azure Function and a Node.js app—with minimal manual coding. He emphasizes:

- The importance of developer experience in debugging AI-generated solutions.
- That beginners might face more challenges or require additional research.
- Optimism for the future of Copilot and AI in improving software development productivity.

In summary, Rob’s experience illustrates that, while promising, AI code generation is most effective when paired with domain expertise and strong cybersecurity practices.

This post appeared first on Rob Bos' Blog. [Read the entire article here](https://devopsjournal.io/blog/2023/04/24/how-Copilot-helps-me-in-my-daily-work)
