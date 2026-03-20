---
section_names:
- dotnet
- security
feed_name: Nick Chapsas YouTube
tags:
- .NET
- AutoMapper
- C#
- Denial Of Service (dos)
- Dependency Risk
- Library Security
- NuGet Packages
- Object Mapping
- Recursion
- Security
- Security Vulnerability
- Server Crash
- Stack Overflow
- Videos
external_url: https://www.youtube.com/watch?v=FQdu5cyvb5k
title: How AutoMapper Can Crash Your .NET Server
date: 2026-03-20 11:45:00 +00:00
primary_section: dotnet
author: Nick Chapsas
---

Nick Chapsas discusses a high-severity AutoMapper vulnerability that can crash a .NET server via uncontrolled recursion, and examines how serious the real-world impact is and what developers should pay attention to.<!--excerpt_end-->

# How AutoMapper Can Crash Your .NET Server

Nick Chapsas covers a newly reported **high-severity vulnerability in AutoMapper** that can **crash a .NET server** due to **uncontrolled recursion**.

## What the issue is

- The vulnerability centers on **uncontrollable recursion** when AutoMapper attempts to map object graphs.
- In certain scenarios, this recursion can spiral until the process hits limits (for example, stack exhaustion), resulting in a **server crash**.

## Why it matters

- AutoMapper is commonly used in .NET applications for **object-to-object mapping**.
- A crash caused by uncontrolled recursion can become a **denial-of-service style reliability/security problem** if an attacker can influence the mapped input or object graph.

## “Is it really that big of an issue?”

- The video explicitly questions whether the vulnerability is as severe in practice as it’s being presented.
- The real impact depends on how AutoMapper is used in your application and whether untrusted input can trigger problematic mapping paths.

## Links from the description

- Dometrain courses: https://dometrain.com/courses/?ref=nick-chapsas&promo=youtube&coupon_code=youtube&coupon_code=HANDSON30
- GitHub: https://github.com/Elfocrash
- Twitter/X: https://twitter.com/nickchapsas
- LinkedIn: https://www.linkedin.com/in/nick-chapsas


