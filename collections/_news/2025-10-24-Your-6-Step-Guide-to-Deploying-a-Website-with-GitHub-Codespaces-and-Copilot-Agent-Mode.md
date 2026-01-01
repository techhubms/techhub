---
layout: "post"
title: "Your 6-Step Guide to Deploying a Website with GitHub Codespaces and Copilot Agent Mode"
description: "This guide by Shree Chinnasamy and Priyanka Vergadia walks you through building and deploying a personal website using GitHub Pages, Codespaces, and GitHub Copilot agent mode. You'll learn how to set up a repository, customize site templates, leverage Copilot agent mode for automation, and deploy your site without complex local setup."
author: "Shree Chinnasamy, Priyanka Vergadia"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/all-things-azure/your-6-step-guide-to-deploying-a-website-with-github-codespaces-and-copilot-agent-mode/"
viewing_mode: "external"
feed_name: "Microsoft All Things Azure Blog"
feed_url: "https://devblogs.microsoft.com/all-things-azure/feed/"
date: 2025-10-24 22:38:54 +00:00
permalink: "/2025-10-24-Your-6-Step-Guide-to-Deploying-a-Website-with-GitHub-Codespaces-and-Copilot-Agent-Mode.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: ["Agents", "AI", "All Things Azure", "App Development", "Coding", "Continuous Deployment", "Copilot Agent Mode", "Developer Productivity", "DevOps", "DevOps Workflow", "GitHub Codespaces", "GitHub Copilot", "GitHub Pages", "News", "Personal Website", "Repository Management", "Site Customization", "Static Website Hosting", "Version Control", "Web Development"]
tags_normalized: ["agents", "ai", "all things azure", "app development", "coding", "continuous deployment", "copilot agent mode", "developer productivity", "devops", "devops workflow", "github codespaces", "github copilot", "github pages", "news", "personal website", "repository management", "site customization", "static website hosting", "version control", "web development"]
---

Shree Chinnasamy and Priyanka Vergadia provide a practical walkthrough for developers to build and deploy a personal website with GitHub Pages, Codespaces, and Copilot agent mode, making streamlined web publishing accessible and efficient.<!--excerpt_end-->

# Your 6-Step Guide to Deploying a Website with GitHub Codespaces and Copilot Agent Mode

**Authors:** Shree Chinnasamy, Priyanka Vergadia

Building and deploying a personal website is a great way to share your work and own your narrative. In this step-by-step guide, you'll learn how to launch your first static website using GitHub Pages with the convenience of GitHub Codespaces and GitHub Copilot agent mode to automate and streamline the process.

## Prerequisites

- A GitHub account (Free or Pro plan)
- Basic familiarity with repositories and markdown

## Workflow Overview

1. **Create a Public Repository**
   - Initialize with a README and license.
   - Set the repository to Public for GitHub Pages deployment.
   - [Creating a new repository](https://docs.github.com/en/repositories/creating-and-managing-repositories/creating-a-new-repository)
2. **Clone and Customize a Template**
   - Use a public template or another website as your starting point. For instance, Shree cloned Jon Barron's academic website for customization.
   - Clone directly into GitHub Codespaces to expedite the setup and skip local environment configuration.
   - [Cloning a repository](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository#cloning-a-repository)
3. **Leverage Copilot Agent Mode**
   - Use GitHub Copilot Chat and agent mode to:
     - Understand repository structure
     - Automate changes to files (e.g., editing `index.html`, managing CNAME)
     - Run code and command iterations
   - See [Copilot agent mode documentation](https://github.blog/ai-and-ml/github-copilot/agent-mode-101-all-about-github-copilots-powerful-mode/) for usage details.
4. **Customize Your Website**
   - Edit website files, update content, upload images or logos.
   - Use Copilot to assist with context-aware code changes.
5. **Configure Deployment**
   - Set GitHub Pages to publish from your desired branch (commonly `/main`).
   - GitHub Pages hosts static sites directly from the repository.
6. **Go Live and (Optionally) Configure a Custom Domain**
   - Your GitHub Pages site (e.g., `https://username.github.io`) is now live.
   - Optionally, attach a custom domain: [About custom domains and GitHub Pages](https://docs.github.com/en/pages/configuring-a-custom-domain-for-your-github-pages-site/about-custom-domains-and-github-pages)

## Additional Tips

- Copilot can accelerate understanding and customizing templates.
- Document every step in `README` for future reference.
- Share your journey with the developer community.
- No advanced coding skills are required—GitHub's services handle much of the deployment complexity.

## Useful Resources

- [GitHub Codespaces](https://github.com/features/codespaces)
- [GitHub Copilot Docs](https://docs.github.com/en/copilot)
- [GitHub Pages Documentation](https://docs.github.com/en/pages)

With this workflow, you can quickly establish a professional web presence using modern DevOps automation and AI-powered code assistance.

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/your-6-step-guide-to-deploying-a-website-with-github-codespaces-and-copilot-agent-mode/)
