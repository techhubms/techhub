---
section_names:
- ai
- devops
- github-copilot
feed_name: Manual Test
tags:
- Azure DevOps
- Champions program
- Change management
- Code review
- Copilot adoption
- Copilot enablement
- Copilot rollout
- Developer experience (DX)
- DevOps processes
- Engineering culture
- Flow state
- GitHub Actions
- GitHub Copilot
- Governance
- Growth mindset
- IDE integrations
- JetBrains IDEs
- Mean time to repair (MTTR)
- Premium requests
- ROI measurement
- SDLC
- Training program
- Usage metrics
- Videos
- Visual Studio Code
external_url: https://www.youtube.com/watch?v=b7WIU9NNBdY
title: "Webinar: Adopt, enable, and scale GitHub Copilot in an organization"
date: 2026-03-24 13:28:04 +00:00
primary_section: github-copilot
author: Rob Bos & Fokko Veegens
---

In this webinar, the presenters (DevOps consultants and GitHub trainers) share a practical approach to adopting and scaling GitHub Copilot: common rollout mistakes, how to run a PoC, how to build a champions network, and what to measure beyond “developer productivity” to show impact across the SDLC.<!--excerpt_end-->

The presenters (DevOps consultants and GitHub trainers) explain how to adopt, enable, and scale GitHub Copilot across an organization, based on patterns they’ve seen in customer rollouts.

{% youtube b7WIU9NNBdY %}

## Full summary based on transcript

### Who the session is for

The webinar targets organizations that:

- Have low GitHub Copilot adoption after buying licenses
- Aren’t sure where to start across different developer personas/teams
- Completed a small PoC and now need to scale with best practices

### Core idea: Copilot is a tool with a learning curve

- The presenters argue Copilot changes day-to-day engineering work and requires deliberate enablement.
- They describe a personal “learning journey” over ~6 months, with ongoing learning even after years of usage due to:
  - New features
  - New prompting techniques
  - Discovering new use cases

### Common mistakes seen in the field

#### 1) “Throw licenses at developers” without enablement

- Some developers will self-learn, but many won’t.
- Without training and time to learn, adoption stays low and advanced features (chat/agentic workflows) don’t get used.

#### 2) Over-focusing on ROI as “developer productivity”

- Measuring productivity is hard and can create bad incentives.
- Metrics like lines of code, PR count, or code coverage can be gamed.
- They suggest looking beyond “coding output” because developers spend much of their day on non-coding work.

#### 3) Giving Copilot only to certain individuals (e.g., only seniors)

- They recommend enabling whole teams, not just individuals.
- Juniors can benefit significantly if guided properly.
- They also suggest considering adjacent roles (e.g., product owner, scrum master) because Copilot can help across the SDLC.

#### 4) Ignoring culture and team dynamics

- Existing issues (low trust, low standards, weak quality practices) get amplified.
- “AI won’t fix low standards”; it can increase risk if teams ship unreviewed/low-quality output.

#### 5) No follow-up after initial training

- “Watch this YouTube video” isn’t enough for many teams.
- Ongoing sessions, deeper dives, and hands-on support are needed.
- Keeping up with rapid feature changes is part of the program.

#### 6) Treating Copilot as only a coding assistant

They describe a broader view where Copilot can help across the SDLC:

- Ideation and architecture discussions
- Non-functional requirements
- Documentation
- Production troubleshooting (using logs/errors to reduce MTTR)

### Feedback they hear from engineers

Common sentiments:

- “No training” or “just watch a video”
- “We don’t use GitHub, so why use GitHub Copilot?”
  - They counter that Copilot works in IDEs and can be used even if repos are in Bitbucket/Azure DevOps/GitLab (with fewer GitHub-specific integrations).
- “Only part of the team got licenses”
- Copilot perceived as “black box magic,” which can increase fear and unrealistic expectations

### Their rollout approach (5 blocks)

#### 1) Proof of concept (PoC)

Focus areas:

- Define success criteria (push back on vague “10x developers” goals)
- Decide how to measure success (and avoid easily gamed metrics)
- Identify where Copilot fits into existing DevOps/SDLC processes

They mention alternative impact measures such as:

- Mean time to repair/recover (MTTR) for production issues
- Whether engineers would be “happier or less happy” if Copilot were removed

#### 2) Preparation

- Identify and train “champions” (above-average Copilot knowledge)
- Assign a coordinator to organize the champions program
- Set expectations and reserve time for champions (they mention ~2–4 hours/week)

#### 3) Rollout

- Enable users with licenses + clear training paths
- Set up a central place for information (wiki + Teams/Slack channel)
- Track usage and adoption metrics from GitHub:
  - Inline completions vs chat usage
  - Models used
  - IDEs used

They describe using metrics as a feedback loop to help teams (not as a punitive tool).

#### 4) Premium requests management

- Define how premium request budgets are handled
- Use premium request usage as a signal:
  - Power users
  - Teams preferring specific premium models

#### 5) Aftercare

- Feed lessons learned back into the next rollout wave
- Continue enablement as an ongoing program

### Success factors they emphasize

- Enable whole teams (not individuals)
- Share internal “stories from the trenches” (practical use cases)
  - Examples mentioned: PlantUML, draw.io, generating diagrams from CLI/resource group data
- Measure and share impact across the organization
- Use enthusiastic champions who can relate to internal context and tech stacks
- Be authentic (show mistakes; embrace non-deterministic outcomes)
- Prefer in-person sessions for better engagement

### Framing: Developer experience and engineering culture

- They connect Copilot adoption to developer experience (DX): staying in flow, reducing friction, and improving satisfaction.
- They cite research that developers often spend only ~2 hours/day writing code, so optimizing only “coding time” misses much of the opportunity.
- They position Copilot as part of broader engineering culture (delivery, quality, production confidence), not just an IDE feature.

### Governance considerations (mentioned near the end)

They briefly note governance topics to address during rollout:

- Which features are enabled
- Legal/security involvement
- Policies for new functionality (e.g., whether to wait for GA)
- Staying up to date with changes

