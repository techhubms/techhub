Rob Bos and I launched a new website, the [Microsoft Tech Hub](https://tech.hub.ms), where we collect technical content in the Microsoft space. It’s categorized, grouped, and filterable so it’s easy to find exactly what you’re interested in.

The content is mainly news, blogs, videos, and community articles. And the topics are Azure, AI, ML, GitHub Copilot, .NET, Security, and DevOps (honestly, this last one is a bit of a catch-all).

Also, every week we publish a roundup of [last week’s highlights](https://hub.ms/2025-08-25-Weekly-AI-and-Tech-News-Roundup.html). My colleagues are creating high-quality videos on [GitHub Copilot features](https://hub.ms/github-copilot/features.html) and the latest [Visual Studio Code updates](https://hub.ms/github-copilot/vscode-updates.html). And of course, there are many more planned features!

This blog post, and a few others coming soon, will give some context and also explain how the site works and some things we ran into.

[![The Homepage of TechHub](https://r-vm.com/assets/techhub/homepage.png)](https://hub.ms)

The Homepage of TechHub (click to open)

[![The GitHub Copilot News Section](https://r-vm.com/assets/techhub/ghc-news.png)](https://hub.ms/github-copilot/news)

The GitHub Copilot News Section (click to open)

## Why?

I like to stay up to date but noticed I couldn’t keep track of everything I’m interested in and started to lag behind on a few topics. One of those topics was AI, so I compiled a list of about 80 keywords and asked my colleague Rob Bos if we could create an “A to Z” kind of story for others struggling. He responded enthusiastically, so off we went.

While working on that, in traditional Xebia fashion, people told us “You can fill a magazine with that…”, so of course we pursued that idea :) The magazine evolved into a website that was not just about AI (and using AI), but about all the topics mentioned earlier.

Each of these topics also has a nice shortcut (just a bit slower to resolve due to a free service in between):

 
- [ai.hub.ms](https://ai.hub.ms) redirects to the AI section
 
- [ghc.hub.ms](https://ghc.hub.ms) redirects to the GitHub Copilot section
 
- [all.hub.ms](https://all.hub.ms) redirects to the Everything section
 
- [azure.hub.ms](https://azure.hub.ms) redirects to the Azure section
 
- etc.

If you prefer shorter links, [hub.ms](https://hub.ms) also works!

## How does it work?

It is basically a very fancy RSS scraper with AI. Currently, there are about 70 feeds configured (and growing), and for each article or video we try to get as much content as possible and use AI to categorize, tag, summarize, etc. To avoid taking traffic away from the original authors, links point back to the original sources; the content is used for value‑adds like the weekly roundup.

## Who maintains this?

I currently build and maintain the entire website, while Rob Bos and other Xebia colleagues add a lot of the GitHub Copilot content and give me valuable feedback. I’d love it if people would open Pull Requests on [the GitHub repo](https://github.com/techhubms/techhub) to make this even better!

Direct content contributors so far are a few of my [Xebia](https://xebia.com) colleagues:

 
- [Rob Bos](https://github.com/rajbos)
 
- [Fokko Veegens](https://github.com/FokkoVeegens)
 
- [Liuba Gonta](https://github.com/liubchigo)
 
- [Randy Pagels](https://github.com/PagelsR)

## Future plans

 
- The first thing I want to do is less visible: Move away from a static website as it is too limiting
 
- Add full NLWeb support
 
- Create personalized roundups, where you can leave a prompt and you get an update tailored to your needs
 
- Of course, podcast support for these personalized roundups
 
- Update the A(i) to Z page with the most recent developments
 
- Better tagging and proper (semantic) searching
 
- LinkedIn support
 
- Automatically add events
 
- Reddit support—I initially implemented this, but I felt it was too hacky, using Playwright to scrape the content

None of these will happen for at least another month, though, as I’ll be enjoying a long holiday first.

## Topics for the blog posts

There are a bunch of things I plan to blog about as well:

 
- Creating the site with Jekyll, a dev container, GitHub Pages, and a custom domain
 
- My experience using GitHub Copilot to build the website: GPT-4.1 vs Claude, premium requests, custom prompts, terminal auto-approve, instructions, etc.
 
- Using GitHub Models to analyze content and moving to Azure OpenAI due to rate- and token-limiting
 
- Creating the weekly roundups and more (ongoing) struggles with the amount of data
 
- End-to-end tests and fixing bugs with the Playwright MCP server
 
- GitHub Pages limitations and transitioning to Azure Static Web Apps
 
- Scaling issues and transitioning to Azure App Services
 
- All of the future plans listed above :)