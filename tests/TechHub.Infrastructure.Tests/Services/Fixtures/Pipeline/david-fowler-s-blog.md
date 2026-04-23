[

![David Fowler](https://miro.medium.com/v2/resize:fill:64:64/1*xIMgAFfe7TtYOd2L_Mrrgw.jpeg)

](https://medium.com/@davidfowl?source=post_page---byline--52f4b257b32a---------------------------------------)

3 min read

Jan 1, 2026

--

I built a thing: [https://tallyai.money](https://tallyai.money/)

Over Christmas, while most people were unplugging, I found myself doing a task I seem to repeat every few years: figuring out where my money actually went.

I’ve tried a lot of tools over the years. Same pattern every time: import data, stare at cryptic transaction names, get annoyed, stop caring. Mint was fine, but categorization was always the worst part. “ZELLE TO X” isn’t helpful. That’s babysitting…

This time I had some downtime and a coding agent, so I decided to build something for myself.

## Version 0: agent-first, everything implicit

The first version of Tally wasn’t really a product. It was an assumption.

The assumption was that I could just give a coding agent (I was using Claude Code) my CSVs and ask questions. I needed fast feedback, so I had the agent generate a script that emitted an HTML report. Open it in a browser, skim, iterate.

If I wanted a new view, I didn’t refactor anything. I just asked the agent to change the code and re-run it. That loop worked extremely well.

For me.
With an agent.
On my data.

It broke the moment a friend asked if they could run it themselves. Everything important lived in prompts and regenerated scripts. There was nothing reusable, explainable, or shareable.

## The pivot: rules, not reports

That’s when it clicked: the value wasn’t the visualization. It was the rules.

Categorization is about expressing how you think about money. Those rules are basically code, and coding agents are great at writing code. But I didn’t want the agent to be the system.

I wanted:

- Everything to run locally
- The option to use local models (with agents like open code)
- Simple, inspectable file-based rules
- No complex UI just to write basic expressions

So Tally evolved into a rule engine, a Swiss Army knife for transaction categorization.

Instead of one-off scripts, you define rules. Instead of prompts, you get deterministic execution. The core is small, local, and offline. You can hand it to someone else and say “run this” and get the same result every time.

Agents are used for the non-deterministic part of the picture, building the rules. They just stopped being the runtime.

Now an agent can help discover merchants, suggest new rules, or explain oddities. But the logic lives in files, not chat history.

## What it is (and isn’t)

Tally isn’t a budgeting app. It’s not a dashboard. It doesn’t connect to your bank.

It’s a local tool for turning messy transaction data into structured, explainable results, built to work with agents, not depend on them.

If that problem resonates with you, give it a try!

This was a fully vibe **engineered** Christmas project 😅

## Why Python and not .NET

The version 0, just in time agent + code spit html emitted a python script to execute on the fly to emit the html report I wanted. I just let the agent write the code initially, but figured I would dabble deep into Python to get a better understanding of the tools, ecosystem and experience building something more real with it.

So far so good:

- uv is really awesome and makes running python as easy as running a .NET app (uv run / dotnet run).
- Python has a built in parser for python itself. This makes it really easy to build embedded rule engines using python as a subset language.
- Using Pyinstaller it’s pretty easy to build a native executable with no dependency on python.

It’s not as fast as .NET execution-wise. Maybe I’ll rewrite it someday. For now, it’s more than good enough.