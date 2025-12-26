---
layout: "post"
title: "Why Server-Side Request Forgery (SSRF) Is a Top Cloud Security Concern"
description: "This video features security expert Michael Howard discussing why Server-Side Request Forgery (SSRF) is one of the most serious security bugs in modern cloud applications. The discussion highlights the risks posed by SSRF, its increasing relevance in cloud environments, and why developers must understand and defend against this threat."
author: "Microsoft Developer"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=7nAp63gv2jE"
viewing_mode: "internal"
feed_name: "Microsoft Developer YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCsMica-v34Irf9KVTh6xx-g"
date: 2025-11-14 16:35:46 +00:00
permalink: "/videos/2025-11-14-Why-Server-Side-Request-Forgery-SSRF-Is-a-Top-Cloud-Security-Concern.html"
categories: ["Security"]
tags: ["Cloud Security", "Development Security", "Michael Howard", "Microsoft", "Security", "Security Best Practices", "Security Bug", "Security Risks", "Server Side Request Forgery", "SSRF", "Videos", "Vulnerability", "Web Application Security"]
tags_normalized: ["cloud security", "development security", "michael howard", "microsoft", "security", "security best practices", "security bug", "security risks", "server side request forgery", "ssrf", "videos", "vulnerability", "web application security"]
---

In this video, Microsoft Developer shares insights from Michael Howard, who explains why SSRF is a critical security threat developers must be aware of.<!--excerpt_end-->

{% youtube 7nAp63gv2jE %}

# Why Server-Side Request Forgery (SSRF) Is a Top Cloud Security Concern

**Featuring:** Michael Howard (Microsoft)

Server-Side Request Forgery (SSRF) has emerged as a significant security bug, particularly in modern cloud applications. In this video, Michael Howard discusses:

- **What is SSRF?**
  - SSRF is a web security vulnerability that allows an attacker to induce the server-side application to make HTTP requests to an arbitrary domain chosen by the attacker.
- **Why SSRF is dangerous:**
  - Traditionally considered obscure, SSRF now poses a real threat as more applications and services move to the cloud.
  - Attackers can exploit SSRF to access internal resources, metadata endpoints, or other services which are otherwise inaccessible.
- **Why developers must pay attention:**
  - Modern infrastructure increases the attack surface.
  - SSRF can bypass network restrictions and firewall controls.
- **Prevention and mitigation tips:**
  - Validate and sanitize user-supplied URLs.
  - Avoid responding to requests using sensitive server credentials.
  - Implement network segmentation and least privilege principles.

**Reference:**
Check out the [Microsoft Security Blog](https://msft.it/6059tOnlH) for deeper insights and best practices.

SSRF is now a high-priority issue that every developer working with web or cloud technologies should understand and actively protect against.
