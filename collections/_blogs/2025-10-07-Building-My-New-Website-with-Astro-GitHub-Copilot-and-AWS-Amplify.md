---
title: Building My New Website with Astro, GitHub Copilot, and AWS Amplify
external_url: https://dev.to/playfulprogramming/building-my-new-website-with-astro-github-copilot-and-aws-amplify-3eoc
tags:
- AI
- Amazon EventBridge
- Astro
- Astro AWS Amplify Adapter
- AWS
- AWS Amplify
- AWS Lambda
- AWS Route 53
- Blogs
- Branch Based Environments
- CI/CD
- Copilot Personas
- Cron Scheduling
- Custom Domains
- DevOps
- GitHub Codespaces
- GitHub Copilot
- Lighthouse Performance
- Production Deployments
- Prompting Workflows
- SEO
- Staging Environments
- Static Site Generation
- TypeScript
- VS Code
- Webdev
- Webhooks
author: Emanuele Bartolesi
primary_section: github-copilot
date: 2025-10-07 12:41:50 +00:00
section_names:
- ai
- devops
- github-copilot
feed_name: Emanuele Bartolesi's Blog
---

Emanuele Bartolesi walks through rebuilding his personal website from scratch with Astro, using GitHub Copilot via a “personas” workflow, developing entirely in GitHub Codespaces, and deploying with AWS Amplify—plus a small AWS Lambda + EventBridge job to trigger scheduled rebuilds.<!--excerpt_end-->

## Overview

Emanuele Bartolesi rebuilt his personal website, [emanuelebartolesi.com](https://emanuelebartolesi.com), from scratch to learn Astro by building instead of following tutorials. The project leaned heavily on GitHub Copilot (using a “personas” workflow), ran entirely in GitHub Codespaces, and was deployed to AWS Amplify using the Astro adapter. He also automated periodic rebuilds via AWS Lambda and scheduled them with Amazon EventBridge.

## Starting from zero with Astro (with Copilot as a coding partner)

Bartolesi started with:

- No templates
- No familiar frameworks
- Astro knowledge level at “zero”

Instead of using Copilot only for autocomplete, he used a “Copilot personas” approach:

- **Architect Copilot**: explain concepts like Astro layouts and partials
- **Implementer Copilot**: generate initial components and page structure
- **Reviewer Copilot**: check consistency and readability

He notes Copilot worked particularly well with Astro for:

- Component scaffolding
- Meta tag configuration
- Performance ideas (e.g., Lighthouse-related optimizations)

### Example: comment-to-code generation

He provided a short natural-language instruction (including an example link to a Packt page), and Copilot generated an Astro page scaffold with imports and data helpers.

```text
with the same style of the other pages, I would like to add a page dedicated to books I written or I am writing. like this one: https://www.packtpub.com/en-us/product/mastering-minimal-apis-in-aspnet-core-9781803237824
```

Copilot then generated Astro frontmatter including imports and data loading:

```astro
---
import BaseHead from '../components/BaseHead.astro';
import Footer from '../components/Footer.astro';
import Header from '../components/Header.astro';
import { getFeaturedBooks, getPublishedBooks, getUpcomingBooks, bookCategories, getBookStats } from '../data/books';

const featuredBooks = getFeaturedBooks();
const publishedBooks = getPublishedBooks();
const upcomingBooks = getUpcomingBooks();
const stats = getBookStats();
---
```

He reports Copilot also produced the rest of the page (HTML/Astro) and a TypeScript data class.

## Live coding on Twitch

He streamed early development sessions on Twitch to “learn in public,” including asking Copilot/ChatGPT questions and working through code he didn’t fully understand yet.

- Twitch link: [http://twitch.tv/kasuken](http://twitch.tv/kasuken)

## Developing entirely in GitHub Codespaces

A core constraint was:

> “I don’t want to install anything locally.”

He built the project in **GitHub Codespaces**, using the default Codespaces image, and highlights:

- Fast startup and previews
- No local Node.js version/dependency issues
- Portability across machines (same editor, extensions, environment)
- Copilot integrated directly in the environment

## Deploying with AWS Amplify (first time)

He chose AWS Amplify to:

1. Keep deployment automated from GitHub
2. Learn a new hosting platform

Amplify connected to GitHub and picked up defaults like `npm run build` and `dist/`.

He also comments on UI/UX differences versus Azure Portal, and that he needed time to become productive in the AWS console.

### Astro isn’t natively supported: using the Amplify adapter

He used the **AWS Amplify Adapter for Astro**.

Install:

```bash
npm install astro-aws-amplify
```

Configuration (with a conditional adapter enablement so local dev still works):

```js
import mdx from "@astrojs/mdx";
import sitemap from "@astrojs/sitemap";
import { defineConfig } from "astro/config";
import awsAmplify from "astro-aws-amplify";

// Conditionally use AWS Amplify adapter for deployments
const isAWSDeployment = process.env.NODE_ENV === 'production' || process.env.AWS_AMPLIFY === 'true';

// https://astro.build/config
export default defineConfig({
  site: "https://example.com",
  integrations: [mdx(), sitemap()],
  ...(isAWSDeployment && { adapter: awsAmplify() }),
  output: isAWSDeployment ? "server" : "static",
});
```

He references the AWS documentation and notes his main change was the environment check:

- Docs: [https://docs.aws.amazon.com/amplify/latest/userguide/get-started-astro.html](https://docs.aws.amazon.com/amplify/latest/userguide/get-started-astro.html)

He also contributed a change to the adapter project:

- PR: [https://github.com/alexnguyennz/astro-aws-amplify/pull/37](https://github.com/alexnguyennz/astro-aws-amplify/pull/37)

## Branch-based environments in Amplify

Amplify created an environment per branch automatically:

- `main` → production (`emanuelebartolesi.com`)
- `dev` → staging (`dev.emanuelebartolesi.com`)

Conceptual view:

```text
GitHub Repo
├── main → Amplify Production → emanuelebartolesi.com
└── dev  → Amplify Dev Env     → dev.emanuelebartolesi.com
```

## Automation: scheduled rebuilds via Lambda + EventBridge

Because the site uses components that pull data from external APIs (e.g., GitHub stats, RescueTime), he needed frequent rebuilds to refresh content.

He implemented:

- An **AWS Lambda** function to trigger the Amplify build via webhook
- **Amazon EventBridge** scheduling (cron) to run it every two hours during working hours

Lambda example:

```python
import json
import urllib.request

def lambda_handler(event, context):
  r = urllib.request.Request(
    'https://webhooks URL',
    data=json.dumps({}).encode('utf8'),
    headers={
      'Content-Type': 'application/json'
    },
    method='POST'
  )
  urllib.request.urlopen(r)
  return
```

EventBridge cron schedule:

```text
cron(0 7-17/2 ? * * *)
```

He frames this as a general principle:

> automate everything that can be automated — even personal projects.

## Route 53 and custom domains

He configured custom domains for both environments through **AWS Route 53**, describing it as smooth and integrated with Amplify:

```text
emanuelebartolesi.com     → Amplify Production (main branch)
dev.emanuelebartolesi.com → Amplify Dev Environment (dev branch)
```

He mentions SSL certificates being provided through Route 53.

## Lessons learned

### Working with dynamic data in Astro can be tricky

He found dynamic runtime data patterns more complex than static pages, sometimes requiring combining multiple Astro concepts (server endpoints, hybrid rendering) to achieve incremental updates/live API data behavior.

### The Amplify adapter still has room to grow

He wanted deeper understanding of how the adapter works for customization/troubleshooting, which led him to read the source and contribute upstream:

- Repo: [https://github.com/alexnguyennz/astro-aws-amplify](https://github.com/alexnguyennz/astro-aws-amplify)

## Open source website repo

He notes the site is open source and available as a starting point:

- GitHub: [https://github.com/kasuken/website](https://github.com/kasuken/website)

(Repository README content in the source includes performance/SEO goals, TypeScript usage, and deployment badges.)


[Read the entire article](https://dev.to/playfulprogramming/building-my-new-website-with-astro-github-copilot-and-aws-amplify-3eoc)

