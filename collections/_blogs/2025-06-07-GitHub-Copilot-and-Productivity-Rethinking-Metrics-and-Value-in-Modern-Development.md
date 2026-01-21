---
external_url: https://devopsjournal.io/blog/2025/06/07/Copilot-and-productivity
title: 'GitHub Copilot & Productivity: Rethinking Metrics and Value in Modern Development'
author: Rob Bos
feed_name: Rob Bos' Blog
date: 2025-06-07 00:00:00 +00:00
tags:
- Agent Mode
- Automation
- Business Value
- Code Engagement
- Continuous Integration
- Developer Productivity
- Engineering Culture
- Generative AI
- Metrics
- ROI in Software Tools
- SDLC
- Software Development Lifecycle
section_names:
- ai
- devops
- github-copilot
---
In this insightful post, Rob Bos critiques current productivity metrics in engineering teams—especially with GitHub Copilot in the mix—and offers practical strategies for embracing AI tools for real business value.<!--excerpt_end-->

# GitHub Copilot & Productivity

*By Rob Bos – Date posted: 07 Jun 2025*

---

## Where the Current Focus on Productivity Is Wrong

Organizations often struggle to define what productivity genuinely means for software engineers, defaulting to metrics like lines of code accepted or produced. However, software development encompasses much more than just code. Engineers devote significant time to requirements gathering, architectural planning, documentation, and collaborative discussions. Despite these varied responsibilities, companies often reduce productivity to numerical outputs—an approach that ignores the realities of the job.

Many engineers find themselves battling for uninterrupted focus time amid meetings and process overhead. As a real-world example, some even start work later to gain quiet hours after colleagues leave. Defining productivity solely through code metrics is not only flawed, it's misleading.

### Metrics and Copilot

The challenges are evident with AI tools such as GitHub Copilot. The Copilot metrics API only accounts for lines of code accepted when a suggestion is accepted in full. Partial acceptances (e.g., accepting five out of ten suggested lines) are not counted, resulting in productivity data that is both incomplete and unhelpful. Despite this, some organizations strive to increase 'acceptance rates,' risking a culture where engineers accept code suggestions uncritically just to improve the numbers—a potentially dangerous outcome.

More positive, recent API updates now reflect both the number of suggestions shown and those (even partially) accepted, shifting the focus toward *engagement* rather than crude productivity measures.

![Picture of a confused engineer scratching their head surrounded with question marks](/images/2025/20250401/20250607-Confusion.png)

## What to Focus on Instead

Rob recommends refocusing measurement and adoption efforts around Copilot on the following areas:

- **Engineer Engagement:** Track not just whether Copilot is used, but how. Do engineers leverage only inline code suggestions, or more advanced features like Chat and Agent Mode? Engagement types signal tool adoption depth and business value orientation.

- **When Copilot Is Used:** Productivity is not uniform across an eight-hour day. Meetings and context-switching erode deep work time, with some engineers reporting only two hours daily to progress on actual tasks. With Copilot's pricing (e.g., $19/month for Business, $49/month for Enterprise), the ROI can be readily achieved if the tool saves an engineer a single hour per month, even disregarding training.

- **Downstream SDLC Metrics:** Look at process outcomes, such as changes in Pull Request (PR) volume and size, build/test failure rates, and defect rates in production. These reflect genuine workflow transformation from Copilot use, as opposed to superficial code metrics.

- **Talent Enablement:** Having AI and automation tools is becoming table stakes. Modern engineering environments—rich in tools like Copilot, solid IDEs, version control, and CI/CD pipelines—help keep and attract talent. Companies lagging in this tooling risk losing in the talent market.

- **Maximizing Value on Repetitive Work:** Copilot can automate boilerplate and routine coding, freeing engineers to focus on complex and interesting challenges. During onboarding and training phases, teams can direct Copilot to:
  - Improve test coverage
  - Refactor for maintainability
  - Address codebase TODOs (potentially via Agent Mode and using MCP servers to file issues)
  - Set up or enhance linting and continuous integration pipelines
  - Generate and implement config files (e.g., Dependabot)
  - Automate repetitive fixes or dependency updates

Such practical applications allow organizations to convert time savings directly into enhanced code quality, sustainability, and innovation.

## What’s Next?

Rob calls for a mindset change: stop equating productivity to lines of code. Instead, understand how GitHub Copilot and generative AI fundamentally alter the way software is built and value is delivered. There’s also potential in enabling non-engineering colleagues to benefit from these workflows, leveling up the entire organization’s efficiency.

He refers readers to his related blog post, [GitHub Copilot - Change the Narrative](/blog/2025/04/01/GitHub-Copilot-Change-the-Narrative), for deeper discussion on shifting engineering culture and orchestrating AI agent collaboration.

> Lets work in the right direction and embrace the future state of working by leveraging Generative AI more and more, and leave the past behind us. I think we will become more of an orchestrator of AI agents, and the agents can only go faster if we understand how to build a solid foundation to trust on.  

![Image of adding value to the end user](/images/2025/20250401/20250401-Value.png)

---

**Related resources:**

- [Make a contribution](https://github.com/rajbos/rajbos.github.io/blob/main//_posts/2025-06-07-Copilot-and-productivity.md)
- [Rob Bos on Xpirit](https://www.xpirit.com/xpiriter/rob)

---

This post appeared first on "Rob Bos' Blog". [Read the entire article here](https://devopsjournal.io/blog/2025/06/07/Copilot-and-productivity)
