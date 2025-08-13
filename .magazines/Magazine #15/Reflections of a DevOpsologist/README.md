# Reflections of a DevOpsologist

> "DevOps is the union of people, processes and products to enable continuous delivery of value to our end users." _Donovan Brown_

At heart, I'm a developer. I love to sling code and be part of a team that slings code. But I'm also fascinated by _people_ - and the intersection of culture and tech is what has drawn me to DevOps.

As I reflect over my journey, I see a lot of people who invested in me. Without them I would not be where I am today. Quite literally - I am an immigrant to the US and my career brought me here. I've also worked hard when opportunity presented itself. I've been mentored and have mentored others. I have learned some things along the way that I hope I can communicate through my story.

But wait - before I start: what is a DevOpsologist? I forget where I heard the term from (it's not mine) but I love the sentiment. It comes from DevOps and the suffix "-ology" (the study of something, a branch of learning). DevOps is continually evolving and changing, and I don't think we'll ever "arrive". Calling myself a DevOpsologist reminds me that there is always more to learn!

# The Story

## The Journey Begins

It was summer (at least in the Southern hemisphere) of 2004. I had just moved from Johannesburg to East London, South Africa where I joined a team of about 20 developers for a financial services company. I had been hired on as a senior developer - and little did I know that the next six years would come to define so much of my career.

I was now about three months into the job. My previous job hadn't been anywhere close to anything from Microsoft. It was all C++, [CORBA](https://en.wikipedia.org/wiki/Common_Object_Request_Broker_Architecture) and Linux. Now here I was, a senior developer at a team using SQL Server and Web services built on .NET Framework 1.1. I had used CVS for source control before, and the new team was using WinCVS. It wasn't pretty. There was almost no process in place - we deployed by using Visual Studio's "right-click Publish" feature and fat-fingering was so common it was expected.

I remember thinking to myself, "I may not be the most experienced .NET developer, but there must be a better way to manage how we deliver code." So I opened up a browser and used my favorite search engine, WebCrawler, to see if I could find anything better.

And I did. I found a tool that was so new it was still in beta. The installation took about a week - at one point the install failed and I couldn't recover. In fact, the failure was so bad I ended up formatting the entire machine and starting again. But I persisted - and finally stood up a shiny new instance of Team Foundation Server (TFS) 2005 beta 2.

## The Glorious TFS Days

So began my journey into DevOps. Except that the term DevOps hadn't been coined yet. It was still "Application Lifecycle Management" (ALM). I don't even think I heard that term until around 2008. But even if I didn't know what it was called, I was doing it. Mostly by instinct.

We started by getting all of our code into TFS. We even started using automated builds with MSBuild! We could at least build from a known source of truth, rather than hoping what we had on our dev machines was the latest code.

Our next phase was adopting unit testing. I remember some very heated debates with the team. "We have too much code and our coverage will be practically zero!" was a common sentiment. I managed to convince the team that 0.2% code coverage is better than 0%, so we started by just ensuring that if we touched a method (or added one) that we would only deploy if we had a unit test for that changed code. Before long, we were in the 60% range for code coverage. Small, incremental changes added up having a large impact over time.

We also started using Work Item Tracking. We all underwent Prince2 training (it's a flavor of Waterfall from the British Government) and customized our templates, work items and reports to match. Back then we had to create our own Release Management tool since there wasn't one in TFS till years later.

## Jump to Consulting

In 2008, I attended TechEd Africa in Durban, South Africa. This was my first large tech conference experience. I went to a talk by Chris Menegay, who ran a small consulting firm in Dallas, Texas called Notion Solutions. Chris had an ALM talk that featured TFS. I knew I wanted to move into consulting at some stage, but until that point I had no idea what to consult in! Hearing Chris talk about his team of ALM consultants, I knew that was what I wanted to do. However, my first child had just been born, and I didn't think it was the right time to leave a job with a steady salary.

I attended TechEd Africa in 2009 - and Chris was a guest speaker again! This time I was ready for a change - so after his talk, I had a 5 minute conversation with him. I remember asking him if he did any work in South Africa, and he replied he could send a consultant over (later I found out he was thinking of Donovan Brown who worked for Chris at that stage). "No, I'm looking to get into consulting," I responded. Chris and I had a chat with a key Microsoft figure - and someone I owe a debt to for all the help in my early days - Ahmed Salijee, who headed up Visual Studio sales for Microsoft in South Africa. 

For some reason, Chris took a chance on me - and a few months later I started the South Africa branch of Notion Solutions! Chris put a few thousand dollars in so I had a steady salary, but I did everything. I was calling. I was delivering. I was invoicing - I learned so much during that time. Fortunately, I had the collective minds of Notion Solutions backing me, so I started confidently, even though I didn't really know what I was doing. That was a good way to grow.

## How did I get here?

In September 2010 Chris flew me out to a Notion Solutions gathering in Irving, Texas. It was a rare occasion to have all the Notion Consultants in a single place. I remember feeling awed. There I was in the same room as some of my "ALM heroes" such as Chris Menegay, Dave McKinstry, Abel Wang, Donovan Brown, Steve St. Jean and Ed Blankenship. "How did I get here?" I wondered - but I was determined that I would learn all I could from these folks and wouldn't take my good fortune for granted!

Most of the Notion Team were Microsoft Most Valuable Professionals (MVPs) and I was inspired to attain that award too. So I started my blog - [Colin's ALM Corner](https://colinsalmcorner.com) which is still going today. In 2011, I was awarded my first MVP award. In 2012, I got to attend my first MVP Summit in Redmond, WA. There I met some folks that I am still connected to this day - leaders in the ALM community such as Marcel de Vries, René van Osnabrugge, Pieter Gheysens, Brian Randell, Nino Loje, Mickey Gousset, Esteban Garcia, Martin Hinshelwood and many others - including Steven Borg.

## Moving to America

Steve and I connected really well - and eventually he offered me a position at his company Northwest Cadence. Northwest Cadence was based in Seattle and was also a small ALM consultancy. Northwest Cadence persevered through all the legal processes to move me and my family over to Seattle in 2016, and I've been in the US since.

Steve inspired me - he is an excellent communicator who knows something about everything. And his mastery of lean processes and agile was amazing. I remember thinking, "When I grow up, I want to be just like Steve!" I learned so much during my Northwest Cadence days, and still think in terms of flow, efficiency and queuing theory.

In 2018, Steve merged his company into Chicago-based 10th Magnitude. 10th Magnitude was doing some great Azure work, but was finding more and more customers wanted to modernize their software processes as they migrated their data centers to the cloud. The folks from Northwest Cadence brought a wealth of process consulting and DevOps to 10th Magnitude, so it was a really good fit.

## Moving into... sales?

Soon after joining 10th Magnitude, there was an opening for a Solution Architect. I looked at the position and chatted to a few folks, and discovered it was a technical sales role. "I'm not a salesman, I'm a consultant!" was my initial reaction. However, I negotiated with the leadership and took the role on the basis that I would sell for 30% of the time, and consult for the other 70%.

That never turned out to be the case. I ended up selling far more than consulting. But one day I had an epiphany: I love to solve complex problems. And I was doing that in my sales process! I would meet with customers, and spend time to understand their environments, people and challenges. I then crafted services and deals that we would deliver to our customers to help them achieve their goals. While I was now in a sales role, I was able to do what I love doing - use tech and cultural engineering to solve problems.

## What is this Git thing?

I remember when TFS introduced Git repositories around 2013. I couldn't see the appeal - who would use a source control system that let you overwrite history? However, after spending some time with it I started to see the light - so much so that I did a talk at VSLive for a couple of years where I hypothesized that you can't really do modern development if you're _not_ on Git!

And then - Microsoft purchased GitHub in 2018. So - reluctantly - I started to figure out how to use the platform. I preferred Team Foundation Server - which had changed names a couple of times and is now Azure DevOps. That is, until GitHub released GitHub Advanced Security (GHAS).

## AppSec is the Future

I have a development background - Security was always the team that "prevented you going to prod". I didn't speak security, and I had never met a security professional that spoke developer.

But GHAS was different. I instinctively guessed that this was a tool that I wanted to align with. Over time, I was able to verbalize this instinctive feeling - it's simply _security tools for developers_. I recognized that this was a game-changer.

At this stage I was the DevOps Practice lead at 10th Magnitude, and I created one of the first GHAS partner offerings. We had a 2-week GHAS Adoption service and were able to help onboard a few companies to GHAS in the early days.

## Moving to GitHub

I also got to deliver some GitHub/10th Magnitude Roadshows. I met a few Hubbers during that time - including Kevin Alwell, a Solution Engineer on the east coast. In 2021, I applied for a Solution Engineering position at GitHub - and about two days later, Kevin called me out of the blue. After catching up, he told me there was a Solution Engineer role he thought I would be good for... and the rest, as they say, is history.

# Reflections

I've had an incredible journey - and still have lots to look forward to! The advent of generative AI through ChatGPT and GitHub Copilot is just beginning to revolutionize development as we know it. If software has eaten the world, it's now AI's turn!

Since I am a self-confessed DevOpsologist, I proclaim to be constantly learning. So what have I learned over the years working in DevOps?

## Find your inspiration outside of your career

My faith and my family are the core of my identity. I love being an SE and a technologist - but that's what I do, not really who I am. This has been vital to handling pressure and hard times - when work sucks (as it inevitably will be from time to time), I don't feel that _I suck_. I've found that, ironically, living for something _other_ than work and technology has led to more fulfillment in work and tech! So find something that you can be passionate about that isn't your work - and your work will actually improve!

## People matter

I love to sling code. I love being a geek. But no matter how technically proficient I am, and no matter how amazing the tech is, people are still the heart of DevOps. I see a lot of companies that fail not because they are not smart, and not because they have the wrong tools, but because they don't prioritize people and culture.

One of my favorite Laws is Conway's Law. Conway (a programmer) introduced an idea in 1967 that showed a "homomorphic force" between the communication structure of a company and the architectures it produced. In other words, the culture plays a vital role in shaping the technology.

If you haven't yet read [Team Topologies](https://teamtopologies.com/) do yourself a favor. Stop debating if you should implement microservices and spend some cycles on designing your _Teams_. In other words, people matter - so align with that force so that you're not fighting it all the time.

I have had to learn (and still am learning) that _how_ you say something is just as important as _what_ you say. This was something I had to learn as a consultant - and I still have to work on it every day. I can come off as cutting and dismissive - a byproduct of my wiring to see to the heart of a problem very quickly. But I have to constantly think about how I communicate what I see. Because people matter.

## Stay teachable

A corollary of the above is that _you matter_. And if you matter, then you're worth investing in. One of the best ways to invest in yourself is to ask for feedback (or consider unsolicited feedback carefully). We all have blind spots, biases and areas we can grow in. If you are not able to admit when you are wrong - and learn and grow - then you're going to cap your potential.

Managers, peers and customers have all at some time or other given me feedback about something. Each time I try to figure out what I can learn from that feedback. At times, I have "chewed the meat and spat out the bones". Not all feedback is always correct - so I try to find the things that I can learn and grow from - and ignore the rest.

## Keep learning

This is a core strength of mine (from [Gallup's CliftonStrenths](https://www.gallup.com/cliftonstrengths/en/strengthsfinder.aspx)). So learning comes easily to me - that's not the case for everyone. But I believe that everyone should always be learning. And it's close to being a requirement in DevOps given the pace of the software industry.

I could have ignored GitHub Advanced Security - after all, I'm not a security professional. However, I applied myself to learn about it - and it's been key to my success in the past few years. Sometimes, you'll need to just knuckle down and learn about something even if it's not "in your lane" - you'll be surprised at what might happen!

## Take calculated risks

I didn't know if consulting would be a good long-term choice for my career. I didn't know if moving to the US would be good for my family. I didn't know if moving into sales would be something I could do long-term. I approached each decision as rationally as I could, seeking input from friends, family, peers and managers as appropriate. But I never let myself get into "analysis paralysis". At some point, I had to take some calculated risks. Thankfully, I've had a good run, even when there was uncertainty.

# In closing

Being in DevOps has been incredibly fulfilling - from the work I've been able to do, to the people I've met, to the ways I've grown and developed as a person. Every day I count myself fortunate to have a job I love - and to be in an industry that's continuously changing and evolving. I hope that my story and some of my learnings can inspire you to keep growing - and hopefully, I get to hear your story some day.
