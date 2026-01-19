---
layout: post
title: 'Order of Operations for Web Agency: Building, Deploying, and Transferring Client Websites'
author: DoodleMoodle542
canonical_url: https://www.reddit.com/r/github/comments/1mgba7n/need_help_web_ai_agency/
viewing_mode: external
feed_name: Reddit GitHub
feed_url: https://www.reddit.com/r/github/.rss
date: 2025-08-03 05:28:12 +00:00
permalink: /devops/community/Order-of-Operations-for-Web-Agency-Building-Deploying-and-Transferring-Client-Websites
tags:
- Client Handoff
- DNS Configuration
- Domain Management
- GitHub
- Lovable.dev
- Project Transfer
- SSL
- Vercel
- Web Agency Workflow
- Website Deployment
section_names:
- devops
---
Authored by DoodleMoodle542, this article details a practical workflow for web agencies: from building client sites on Lovable.dev to deploying on Vercel, managing domains, and efficiently transferring ownership.<!--excerpt_end-->

## Step-by-Step Workflow for Web Agency Projects

**Author: DoodleMoodle542**

This guide describes an effective order of operations for agencies building and deploying client websites. The workflow ensures smooth setup, deployment, and transfer of ownership, with all technical steps included.

### 1. Build the Website on Lovable.dev

- Create your client's site using Lovable.dev under your agency account.
- When finished, export the complete site code. You have two options:
  - Export directly to your GitHub repository, or
  - Export to a ZIP file and upload it manually to GitHub.

### 2. Deploy the Site on Vercel

- Set up a free Vercel account (if you haven't already).
- Connect your GitHub repository containing the website code to Vercel.
- Vercel will automatically deploy the site, providing a preview URL in the format `yourproject.vercel.app`.

### 3. Buy and Connect a Domain

- Determine domain purchasing responsibility:
  - Your agency can purchase the domain (using a registrar like Namecheap or GoDaddy), **or**
  - The client purchases the domain and grants you access to their registrar account.
- In the Vercel dashboard, navigate to your project’s **Settings > Domains**.
- Connect the new domain for the project.
- Update the domain's DNS settings to point to Vercel's servers. Vercel provides necessary DNS configuration instructions.

### 4. Transfer Project Ownership to the Client

- Ask the client to register for a Vercel account if they don't already have one.
- In the Vercel dashboard, select your project and go to **Settings > Transfer Project**.
- Enter the client’s email, associated with their Vercel account, and confirm the transfer.
  - As a result:
    - The client’s Vercel account receives ownership of the live website.
    - The GitHub repository stays connected.
    - The custom domain remains configured.
    - SSL remains enabled and valid.

### Result: Client Independence

Your client now has complete ownership and control over their live website, including code updates, domain management, and SSL.

---

This structured process standardizes agency project delivery, automates deployments, and ensures smooth transitions for both the agency and the client.

This post appeared first on "Reddit GitHub". [Read the entire article here](https://www.reddit.com/r/github/comments/1mgba7n/need_help_web_ai_agency/)
