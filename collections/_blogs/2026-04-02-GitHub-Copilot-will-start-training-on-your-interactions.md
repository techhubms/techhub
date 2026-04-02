---
primary_section: github-copilot
date: 2026-04-02 08:10:25 +00:00
section_names:
- ai
- github-copilot
- security
title: GitHub Copilot will start training on your interactions
author: Jesse Houwing
tags:
- AI
- Blogs
- Copilot Enterprise
- Copilot For Business
- Copilot Free
- Copilot Pro
- Copilot Pro+
- Data Residency
- Data Usage Policy
- Developer Environment Security
- Device Management
- DNS Filtering
- Enterprise Licensing
- GitHub
- GitHub Copilot
- GitHub Enterprise
- Group Policy
- Hosts File
- Interaction Data
- Intune
- Managed Users
- Network Controls
- npm
- Opt Out
- Pi Hole
- pip
- Private Repositories
- Proxy Filtering
- Registry Settings
- Security
- Supply Chain Attacks
- VS
- VS Code
- VS Code Extensions
feed_name: Jesse Houwing's Blog
external_url: https://jessehouwing.net/github-copilot-will-start-training-on-your-interactions/
---

Jesse Houwing clarifies GitHub Copilot’s April 24 interaction-data policy change, explaining which subscription tiers may have interactions used for training, what is and isn’t included (like private repos), and practical ways enterprises can enforce license tiers and lock down developer environments.<!--excerpt_end-->

# GitHub Copilot will start training on your interactions

GitHub Copilot “training on your data” has been circulating widely, often framed as if it’s unavoidable and uncontrolled. Jesse Houwing argues that framing is misleading and outlines what the policy change actually means, plus what organizations can do to control it.

## What’s actually changing

- GitHub Copilot will enable training on data it receives through **GitHub Copilot Free, Pro, and Pro+** subscriptions **unless you opt out**.
- GitHub **won’t** “suddenly start training its models” on:
  - the contents of your **private repositories**, or
  - your **locally stored projects** “in the background.”
- It **will** use **interaction data** shared with Copilot (your prompts/interactions) to train and improve its systems for those tiers.

Source:
- Updates to GitHub Copilot interaction data usage policy (GitHub Blog)
  - https://github.blog/news-insights/company-news/updates-to-github-copilot-interaction-data-usage-policy/?ref=jessehouwing.net

## Which licenses are affected (and which aren’t)

- **Affected (unless opted out):**
  - Copilot **Free**
  - Copilot **Pro**
  - Copilot **Pro+**
- **Not affected:**
  - **Copilot for Business**
  - **Copilot Enterprise**

## Controls you can use to prevent non-commercial tiers

Houwing lists several ways to prevent users from using Free/Pro/Pro+ in an organization:

- **License assignment overrides user settings**
  - If your company assigns a **Copilot for Business** or **Copilot Enterprise** license to a GitHub user, it overrides the user’s individual settings.

- **Network-level blocking**
  - Centrally block access to Free/Pro/Pro+ through network controls such as:
    - **DNS filtering**
    - **Proxy filtering**
    - **Pi-hole**

- **Hosts file blocking**
  - Block access by adding **two lines** to a machine’s **hosts** file.

- **IDE policy packs (Visual Studio / VS Code)**
  - Use policy packs for **Visual Studio** and **Visual Studio Code** to enforce a minimum license tier.
  - Roll out via:
    - **Windows Group Policy**
    - **Intune**
    - other device management tools
  - These policy packs control:
    - **registry settings**, or
    - centralized config files you can deploy by other means.

- **GitHub Enterprise with Managed Users / Data Residency**
  - If you’re on GitHub Enterprise with **Managed Users / Data Residency**, it’s “much less of a problem” because:
    - users are on a **company-owned GitHub user**, and
    - access to **github.com** can be controlled.

## Security framing and trade-offs

- Houwing argues GitHub Copilot offers “many of these controls,” more than some competitors.
- He acknowledges a cost trade-off: assigning business/enterprise licenses costs money, but the same trade-off exists with other “free tier” tooling.

## Broader developer environment risks to consider

If the concern is data leakage or IP exposure, Houwing suggests also evaluating and controlling other common risk vectors, including:

- Personal cloud drive backups (e.g., **OneDrive**, **Dropbox**)
- Supply chain attacks against package ecosystems:
  - **npm**
  - **PyPI** (written as “pipy” in the post)
  - other package management tools
- Malicious **Visual Studio Code** or browser extensions
- Developers using other AI tools “many without any policy controls”

## Bottom line

Houwing’s conclusion is that organizations need tighter control over dev environments and devices:

- Unmanaged devices for contractors/new hires working on source code “isn’t an option anymore.”

He closes by offering to help organizations put controls in place.

[Read the entire article](https://jessehouwing.net/github-copilot-will-start-training-on-your-interactions/)

