---
notes: Hey Olaf! Waar ik screenshots gebruik heb ik zowel een dark als light theme screenshot toegevoegd. Ik heb de screenshots in een aparte map gezet, zodat je ze makkelijk kan vinden.
---

# Stop Creating Content With ChatGPT!

This year, I had the pleasure of speaking at NDC Oslo. I got to deliver a session on a topic I'm very passionate about: using different forms of generative AI to generate self-guided meditation sessions. You can read about it in [XPRT Magazine #16][1]. The day before the conference, I attended a community event in an attempt to unwind. At that event, I met one of the organisers of NDC Oslo. He asked what topic I was speaking on, and out of 100 sessions at that event, he knew precisely what session I was giving. And not for a reason I'm proud of, you see, I submitted a session abstract that I created with ChatGPT. I was happy enough with the result that I immediately submitted the abstract instead of reviewing it closely. Writers will tell you: "write drunk, edit sober". And reading the abstract back, I realised that ChatGPT must have been drinking at the time. The result was that I was roasted, in public, for my session abstract by the event organiser for fifteen minutes straight. And how did he know? Well, here's the first paragraph of the abstract:

> In an era where technology and mindfulness intersect, the power of AI is reshaping how we approach app development. This session delves into the fascinating world of utilising artificial intelligence to expedite and streamline the development process of a mobile meditation app. We'll explore how Azure AI Speech, DALL-E, Azure OpenAI, and GitHub Copilot converge to eliminate the need for visual designers, voice actors, and sound designers, thereby revolutionising the traditional development workflow.

Can you see the telltale signs of (Chat)GPT? The buzzwords, the overpromising, the lack of focus? ChatGPT loves to "delve" into things. It will also start every abstract with "In a world / In an era". That the session was even accepted was a miracle. The abstract is too verbose, too buzzwordy, and oozed a lack of focus. It's not the kind of content I wanted to be known for.

I learned from that experience. Don't write your session abstract with ChatGPT. Don't take the easy route. Don't be lazy. Instead:

## Build An Automated Abstract Generator With GitHub And Prompty

I'm convinced that we can create better content through Large Language Models (LLM). It levels the playing field and makes writing good content more accessible for people like me. People who like to talk and present and not write anything down. People who are not native speakers. People who are not writers. So I doubled down and built a system to help me generate better session abstracts. I used GitHub Actions to automate the process, and Prompty to generate the content. Here's how I did it:

I created a new issue template on my [GitHub repo][2] to get started. An issue template is a YAML file that will be used as a template when creating a new issue. This will force me to use a specific format whenever I want to generate a new session abstract. The template contains four fields that answer the following questions:

1. Who do you think this talk is for?
1. What do you think you'll learn from this talk?
1. What's something you'll be able to accomplish with the information gained from this talk?
1. What is the two-sentence summary of the talk?

These questions came from a very talented speaker: Arthur Doler. He mentioned that he asks himself these questions when he creates his session abstracts. And I agree: If you can't answer these questions, you're not ready to submit your session abstract.

If you're working with LLMs and have yet to learn Prompty, [give it a try][6]! Prompty is a VS Code extension allows you to write prompts for LLM combined with the settings and examples needed for that prompt. This converges all your prompt-related settings in one file, which allows you to track changes over time in your git history. Next to that, Prompty comes with a rich dev and test experience. I won't cover all of Prompty's features in this article. It's a great tool for developing any application that leverages LLMs. It also seamlessly integrates with Prompt Flow, Langchain and Semantic Kernel, all the leading LLM orchestration tools. After installing the extension, simply right-click in VS Code's Explorer and choose "New Prompty". Below is the Prompty I created for generating a new session abstract:

```yml
---
name: CreateAbstract
description: A prompt that uses a set of questions and answers to create a new presentation abstract.
authors:
  - Matthijs van der Veer
model:
  api: chat
  configuration:
    type: azure_openai
    azure_endpoint: ${env:AZURE_OPENAI_ENDPOINT}
    azure_deployment: gpt-4o
  parameters:
    max_tokens: 3000
    temperature: 0.7
sample:
  answers: >
    <example answers go here>
---
system:
You are an expert in creating presentation abstracts. Our award-winning way of crafting an abstract is to ask a series of questions and use the answers to create a compelling abstract. 
I will give some examples of abstracts I like. Please match the wording, style and energy of the examples when crafting new ones.

## Examples:
Input:
### 1. Who do you think this talk is for?

Mostly developers, or others interested in automation

### 2. What do you think you'll learn from this talk?

How to use GitHub automation for content generation
The role of humans in reviewing GenAI output

How to chain different forms of GenAI to create unique content
### 3. What's something you'll be able to accomplish with the information gained from this talk?

Use GitHub actions to automatically create PRs
Use Azure AI services from a pipeline

### 4. What is the two-sentence summary of the talk?

Learn how to use GitHub actions to automate more than continuous integration or deployment. Leverage the GitHub's powerful platform the generate new and exciting content with Azure AI Services, while maintaining responsibility as a human.

Output:
# Automating Content Generation with GitHub Actions & Azure AI
In this session, you will learn how to harness the power of GitHub automation for content generation, leveraging GitHub Actions and Azure AI Services. Discover the role of human review and oversight in reviewing GenAI output and how to chain different forms of GenAI to create unique content. We'll explore a practical example of creating self-guided meditations using GPT-4, Azure AI Speech, and DALL-E 3.

This session is ideal for developers and anyone interested in automation. Expect many practical demos, including using GitHub Actions to automatically create PRs and integrating Azure AI services into your pipeline.


## What to avoid
- Never discriminate against any group of people
- Never use the words "delve", "equip", "empower", "navigate", "landscape", "enhance", "delve", "insight".
- Avoid terms like "in a world", or "in an era"

user:
{{answers}}
```

A Prompty file starts with some metadata and configuration. If you've worked with LLMs before, you will recognize some of the settings. In the file, I ask to use the `chat` API rather than the `completion` API and specify the deployment of my LLM. We also get to specify some parameters, like the temperature. This parameter is a value between 0 and 1, where 0 means the LLM will only generate the most likely text. The higher you set the temperature, the more unexpected the output will be. It allows the LLM to select less likely text, which resembles something that humans recognize as "creativity". I'm setting the temperature quite high, because this creativity can benefit my abstracts a lot. This is safe to do because I include examples of the output I expect in the propmt. Next to these settings, you can also add example input in the `sample` section. This allows us to execute the prompt locally and see the results. It's also a good example for others to see how the input should be structured.

A few things should stand out in the the second half of the file. I start off by giving instructions on the role that I want the AI to take when creating the abstract. This gives the LLM more context on the job I'm trying to make it do. I also include instructions to match the style of the examples. This is an important technique in prompt engineering, known as a "few-shot prompt", where we ground the model's response with some examples. If I would leave out the examples, you will get responses based solely on what the LLM things a session abstract should be, with all the GPT-quirks of my original abstract. In the prompt I shared, I only included one example, but the more you add, the better the LLM will copy your intended style and wording. At the time of writing, my prompt uses two examples of abstracts I wrote without AI, in combination with the questions and answers I described above. This grounds the model enough to match my style of writing, as well as the overal length and tone of the abstract (I prefer short abstracts with a positive tone).

The prompt also includes things that I want the assistant to avoid. This is important, because without it we fall prey to the same mistakes I made with ChatGPT. Some of ChatGPT's behaviour comes from the system prompt that's invisible to us, but a big part of the output comes from using the same model that we're using here: GPT-4o. By telling the model what to avoid, we can prevent some of the same mistakes from happening. In prompt engineering, we usually favour positive examples over negative examples, but in practice you need to include guardrails and limitations.

If you wonder why I exclude that specific list of words, it's because they're overrused in session abstracts starting around the time ChatGPT came out. Watch ["Delving Into The Landscape"][3] by Dylan Beattie on the subject.

## GitHub Actions

Armed with my new prompt, I set out to automate the process. I created a new GitHub Workflow that runs every time a new issue is created with the `abstract` label. I won't focus too much on how to build such a workflow, please go ahead and [steal mine][2]. Most importantly, I need a way to run this Prompty file in a GitHub Action. Normally, I would default to a C# implementation. The code would need to be compiled first, and I will likely get stuck picking the best argument parser library and never get anything done. Luckily, Propmty steps in for the rescue again. I right-clicked on the Prompty file in VS Code and chose "Add Prompt Flow Code". This generates a small Python script that runs your Prompty file using Prompt Flow. You can do the same for Langchain (Python) and Semantic Kernel (C#), but Prompt Flow is definitely the least complicated if you're just running a simple prompt and don't want to compile anything. Don't know how to write Python? Can I suggest GitHub Copilot?

This makes the implementation in our workflow quite simple. After installing the Python dependencies and logging on to Azure, here's everything we need to get our abstract as a comment on the issue:

```yml
- name: 📝 Create an abstract
  env:
  AZURE_OPENAI_ENDPOINT: ${{ secrets.AZURE_OPENAI_ENDPOINT }}
  run: | 
    python create-abstract_promptflow.py --answers "${{ github.event.issue.body }}" > abstract.txt
- run: gh issue comment $ISSUE --body "$(cat abstract.txt)"
  env:
    GH_TOKEN: ${{ secrets.GITHUB_TOKEN }}
    ISSUE: ${{ github.event.issue.html_url }}
```

This GitHub workflow will run the Python script, save the output to a file and then use the GitHub CLI to post a comment on the issue with the generated abstract. The result is a generated abstract that's posted as a comment on the issue. This allows me to review the abstract, which brings us to the next problem: Automation Bias.

![Screenshot of a generated abstract.][4]

## Automation Bias: Review Everything

Like many people, I suffer from automation bias. This specific form of cognitive bias is the tendency to favour information generated by automated systems over information provided by humans. At the heart of my faulty abstract was not just content I generated with ChatGPT, but also the fact I didn't review it close enough. I saw words appear on screen that looked like an abstract, and I submitted it. And while more automation is likely not the answer, I doubled down and added even more automation. By now that should not surprise you, dear reader.

I decided to introduce two LLM-powered review tasks. One task reviews the content of the abstract. It checks if all the user-provided answers made it into the abstract and some general insights into the content. The second task is a style check. This prompt checks if the abstract is "too ChatGPT" for my taste. It detects most of the buzzwords and phrases I want to avoid. This way, I can review the abstracts generated by Prompty, and make sure they're up to my standards.

Both prompts follow the same structure as the prompt for generating abstracts. They start off with a style and instructions and are followed up with examples. A big difference is the `temperature` was set to 0.2, making sure the LLM doesn't become too creative in its answers but sticks to the task at hand. Remember: "write drunk, edit sober". I also instruct the model to make ample use of emojis. This way, the review will attract my attention when something is wrong. Here's an example:

![Screenshot of a generated content review.][5]
![The writing coach enters the room.][7]

In the screenshots, you can see the reviews are quite thorough. The writing coach even correctly detected a mistake and then rewrote the abstract with only a tiny change. The reviews have been a great help. I still need to read the original text, but the reviews give me a good idea of what to expect. They also help me to spot mistakes I would otherwise miss. And even better: the first abstracts I've created this way are already being accepted at conferences.

## Conclusion

Generalist tools like ChatGPT are great for a lot of things. But if you're going to create content in a professional setting, you're better off using specialist tools or building your own! I would love to say there's no replacement for a human review, and I believe that, in the case of Generative AI, that's still true today. But that was also true for spellcheckers, autocorrect, UI tests and many other tools we now rely on. I believe we can create better content with generative AI with the right prompts, the right guardrails, and the right review process.

I believe LLMs can even the playing field for people who aren't able to write with the fluency of a native speaker. I'm using it as a tool to help me create better content, not to replace my own creativity. Every abstract that's generated holds my style, my wording, and my ideas. The conclusion of this article: make better prompts, put in the work, and above all: review everything that generative AI generates. You're the one responsible for the content, not the AI.

[1]: https://pages.xebia.com/xprt-magazine-16-protecting-tomorrow-infuse-innovation
[2]: https://github.com/MatthijsvdVeer/MondrianMuse/
[3]: https://www.youtube.com/watch?v=Hqs24Gm0y_g
[4]: images/new-abstract-dark.png
[5]: images/content-review-dark.png
[6]: https://github.com/microsoft/prompty
[7]: images/writing-coach-dark.png
