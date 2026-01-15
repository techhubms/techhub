---
layout: post
title: Is SRE Just DevOps in New Packaging?
author: Rene van Osnabrugge
canonical_url: https://roadtoalm.com/2025/06/11/is-sre-just-devops-in-new-packaging/
viewing_mode: external
feed_name: René van Osnabrugge's Blog
feed_url: https://roadtoalm.com/feed/
date: 2025-06-11 08:33:40 +00:00
permalink: /devops/blogs/Is-SRE-just-DevOps-in-new-packaging
tags:
- Blogs
- DevOps
- Engineering Culture
- Incident Management
- Lead Podcast
- Operations
- Podcast
- Site Reliability Engineering
- SLAs
- SLIs
- SLOs
- Software Reliability
- SRE
- Team Structure
- Xebia
section_names:
- devops
---
Rene van Osnabrugge shares insights from the LEAD podcast, co-hosted with Geert van der Cruijsen, examining the real meaning of SRE and its interplay with DevOps in engineering organizations.<!--excerpt_end-->

## Is SRE Just DevOps in New Packaging?

**Author:** Rene van Osnabrugge

### Introduction

In 2024, Rene van Osnabrugge, alongside Geert van der Cruijsen, launched the LEAD podcast to explore the many dimensions of building an effective engineering culture. Through various episodes—with and without guests—they have discussed practices, mindsets, and team structures relevant to modern software development.

### SRE: Beyond a Buzzword

A recurring theme has been the emerging use of the term SRE (Site Reliability Engineer) in job descriptions. Rene notes that while many organizations begin hiring for SRE roles, these often translate to traditional 'operations' roles in new packaging. This trend is reminiscent of previous cycles with terms like 'microservices,' 'agile,' or 'scrum' sometimes used without fully understanding their underpinning concepts.

#### The Origins of SRE

SRE as a discipline originated at Google through Benjamin Treynor Sloss. SREs were introduced to move beyond traditional operations by integrating engineering rigor into the reliability, performance, and scalability of systems. Unlike standard ops roles, SREs are expected to write code, design systems, and actively engineer reliability at scale—often working to optimize across large and complex infrastructures.

#### SRE and DevOps: Conflict or Complement?

The podcast explores whether SRE and DevOps should be in conflict or can serve as complementary approaches. The DevOps philosophy, summarized as "you build it, you run it," brings accountability and requires teams to consider system quality, security, and reliability. However, as systems scale, expecting deep reliability expertise within all DevOps teams can be unrealistic. Here, SREs add value as specialists or consultants, supplementing teams rather than replacing the DevOps model.

At Google, for instance, SRE engagement is a privilege earned by teams meeting a high maturity threshold. SREs are not incident responders or ticket closers; their responsibilities include significant project work aimed at eliminating toil and architecting reliability improvements.

#### Reliability: Striking the Right Balance

A key insight is the importance of 'appropriate reliability.' Many teams chase ambitious uptime goals (like 99.999%) without assessing whether such targets make sense for their specific business context. For example, if a mobile app can remain functional for hours during backend outages without significant user impact, investing heavily in maximum uptime across the stack may be unnecessary.

#### Understanding SLAs, SLOs, and SLIs

- **SLA (Service Level Agreement):** External commitments, potentially with legal or financial implications.
- **SLO (Service Level Objective):** Internal reliability targets agreed upon by business and development teams.
- **SLI (Service Level Indicator):** Metrics actually measured (such as response times or success rates).

Using these concepts together enables organizations to set clear reliability targets, establish error budgets, and foster constructive trade-offs between reliability and ongoing innovation.

#### Hiring SREs: When Does It Make Sense?

The blog cautions against indiscriminately rebranding operations teams as SREs. True SREs should possess an engineering mindset; their value lies in proactively improving systems, not simply managing tickets. Even without a dedicated SRE team, mature DevOps teams can adopt practices like tracking SLIs and SLOs to clarify priorities and drive alignment.

### Conclusion

Rene emphasizes the importance of understanding the true purpose of SRE roles: not as a trendy job title, but as a commitment to smart, intentional reliability engineering. He encourages organizations to avoid buzzword-driven hiring and to thoughtfully consider the team's real needs before defining roles and responsibilities.

### Listen to the Original Episode

If you're interested in more details, you can [listen to the original episode on the LEAD podcast](https://lead-podcast.io).

This post appeared first on "René van Osnabrugge's Blog". [Read the entire article here](https://roadtoalm.com/2025/06/11/is-sre-just-devops-in-new-packaging/)
