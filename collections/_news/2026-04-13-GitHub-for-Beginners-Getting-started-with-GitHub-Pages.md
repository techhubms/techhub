---
title: 'GitHub for Beginners: Getting started with GitHub Pages'
feed_name: The GitHub Blog
primary_section: devops
external_url: https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-pages/
author: Kedasha Kerr
date: 2026-04-13 15:00:00 +00:00
tags:
- CI/CD
- Custom Domain
- Deploy From Branch
- Developer Skills
- DevOps
- DNS
- Fork
- GitHub
- GitHub Actions
- GitHub For Beginners
- GitHub Pages
- GitHub Skills
- HTTPS
- News
- Next.js
- Permissions
- Repository Settings
- SSL Certificate
- Static Site Hosting
- Workflow
section_names:
- devops
---

Kedasha Kerr walks through how to publish a static site from a GitHub repository using GitHub Pages, covering both branch-based deployment and a GitHub Actions workflow, plus how to set up a custom domain with HTTPS.<!--excerpt_end-->

# GitHub for Beginners: Getting started with GitHub Pages

GitHub Pages is a free hosting option that can publish a static website directly from a GitHub repository. This guide shows two deployment approaches (from a branch or via GitHub Actions) and how to attach a custom domain.

## What you need

To publish with GitHub Pages, you’ll need:

- A GitHub account
- A project to deploy
- A few minutes to set up GitHub Pages

## Deploying to GitHub Pages

Start by navigating to the sample repository and forking it so you can follow along:

- Sample repository: https://gh.io/gfb-pages?utm_source=blog-episode-4&utm_medium=blog&utm_campaign=gfb-s3-2026

The sample repository contains a static website generated with **Next.js**, already pushed to GitHub and ready to deploy.

GitHub Pages supports multiple deployment methods. This article covers:

- Deploying from a branch
- Deploying with GitHub Actions

## Option 1: Deploy from a branch

1. In the repository, open the **Settings** tab.
2. In the left menu, select **Pages** (under **Code and automation**).
3. Under **Build and deployment**, choose **Deploy from a branch**.
4. Under **Branch**, select `main`.
5. Click **Save**.

This publishes the website from the `main` branch and makes it publicly available.

## Option 2: Deploy using GitHub Actions

1. In **Settings -> Pages**, under **Source**, select **GitHub Actions**.
2. Select **browse all workflows** to view available workflow templates.
3. Search for **next.js**.
4. In the **Next.js** workflow box, click **Configure** to open the workflow file.
5. Review the workflow file, paying attention to:
   - The permissions that are set
   - The build and deploy instructions
6. If no changes are required, click **Commit changes**.
7. Enter a commit message (or have Copilot generate one).
8. Ensure you’re committing to the `main` branch, then click **Commit changes**.
9. Open the **Actions** tab and wait for the workflow run(s) to complete.
10. Select the **Add GitHub Actions workflow for Next.js deployment** workflow run.
    - Note: there may be two runs with the same name. If the run you opened doesn’t show a website link in the “deploy” box, open the other run with the identical name.
11. Use the link in the “deploy” box to open the published site on GitHub Pages.

### Important note about visibility

Even if your repository is private, the published GitHub Pages website will still be **public**.

To see who most recently deployed the site, return to **Settings -> Pages**.

## Adding a custom domain

By default, GitHub Pages sites use a URL like:

- `USERNAME.github.io/REPOSITORY-NAME`

To use your own domain:

1. Configure DNS records with your domain provider.
   - Docs: https://docs.github.com/pages/configuring-a-custom-domain-for-your-github-pages-site/managing-a-custom-domain-for-your-github-pages-site?utm_source=blog-episode-4&utm_medium=blog&utm_campaign=gfb-s3-2026
2. Verify the domain at the organization or profile level.
   - Docs: https://docs.github.com/pages/configuring-a-custom-domain-for-your-github-pages-site/verifying-your-custom-domain-for-github-pages?utm_source=blog-episode-4&utm_medium=blog&utm_campaign=gfb-s3-2026
3. In the repository, go to **Settings -> Pages**.
4. Under **Custom domain**, enter your domain name.
5. Select **Save**.
   - GitHub automatically checks DNS configuration. A green checkmark indicates it looks good.
6. After verification, enable **Enforce HTTPS**.
   - This enables a free SSL certificate and forces HTTPS.

## Further reading

- GitHub Pages docs: https://docs.github.com/pages?utm_source=blog-episode-4&utm_medium=blog&utm_campaign=gfb-s3-2026
- Create a GitHub Pages site: https://docs.github.com/pages/getting-started-with-github-pages/creating-a-github-pages-site?utm_source=blog-episode-4&utm_medium=blog&utm_campaign=gfb-s3-2026
- About custom domains: https://docs.github.com/pages/configuring-a-custom-domain-for-your-github-pages-site/about-custom-domains-and-github-pages?utm_source=blog-episode-4&utm_medium=blog&utm_campaign=gfb-s3-2026


[Read the entire article](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-pages/)

