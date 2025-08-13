It was April, 3^rd^ 2014 when Anders Hejlsberg, father of the C#
language, got on stage during the keynote at the Build conference in San
Francisco and released the .NET Compiler Platform ("Roslyn") as open
source and made the first pull request. That same keynote, Scott
Guthrie, Executive Vice President of Cloud & Enterprise group and one of
the original creators of the ASP.NET web stack, announced the creation
of the .NET Foundation. This was a pivotal point in .NET's open source
journey which spawned the avalanche of releasing software as open source
at Microsoft. This is the story of the .NET Foundation.

> on November 12^th^\[2014\], we announced .NET Core, a new open source
> project to build a cross-platform .NET, which started with just four
> libraries

The ASP.NET web stack had been open source since 2008 as well as the F#
language in 2010, but with the C# and Visual Basic.NET compiler now open
source, this opened the door for the entire .NET platform. Later that
year on November 12^th^, we announced .NET Core, a new open source
project to build a cross-platform .NET, which started with just four
libraries. We wanted to "do open source right" by starting from the
beginning in the open and we did it on GitHub. The announcement landed
#1 on Hacker News for most of the day, beating the Philae probe landing
on a comet. It was a big deal.

Getting to that point was also a big deal. It took a lot of work from
many people inside and outside Microsoft.

Looking at ASP.NET's open source history, the source code was open and
the community could contribute issues and code. However, the work from
Microsoft wasn't *truly* done in the open at the time. Bits were worked
on internally and then "dropped" into the repo (Codeplex back then).
Still, it was a first step into changing the way we build software.

We (the "Developer Division" engineering team at Microsoft) knew that we
needed to change the approach to how we were working. We were coming off
the Windows 8 hangover and most of the industry was moving to open web
technologies and open standards. We needed to modernize our platform in
order to grow. The only way we were going to succeed was with the help
of the community.

**Why did we do it?**

So, why did we need an open source software foundation? It was S.
Somasegar (Soma) that pushed this idea to us. Soma was the Corporate
Vice President of Developer Division at the time and our executive
sponsor. Soma believed that the survival of the .NET ecosystem depended
on the open source community and we needed a foundation to foster it. He
approached my manager, Jay Schmelzer, who owned the .NET Framework and
languages, and we started working. We looked to the ASP.NET team run by
Scott Hunter, a separate team in the Azure group back then, as the role
model open source project at Microsoft. Soma knew that we needed to
change the perception of Microsoft in the open source world and the
creation of the .NET Foundation, and the open sourcing of the platform
would prove to be a strong step.

We also had projects from the community as well as our own that needed
help; not just legal and licensing help but basic development services
like code signing and CI/CD. We also had customers that needed to trust
and rely on .NET. I was the community manager for the .NET platform team
before any of our stuff was open source. And I was on the v-team that
stood up the .NET Foundation itself. We were going through a culture
change internally and our customers needed to also come with us.

> The challenge was to make sure we didn't lose trust -- to make sure
> our customers understood that open sourcing .NET was not the end of
> the platform, but the beginning

Many of our customers expected all the software they used to come from
Microsoft. It was a direct result of us creating a hugely successful
closed source ecosystem. Microsoft also didn't have the greatest track
record with some of the open source projects we did release -- where
they were basically "thrown over the wall" and abandoned. The challenge
was to make sure we didn't lose trust -- to make sure our customers
understood that open sourcing .NET was not the end of the platform, but
the *beginning*. We had to get it right.

**How did we start?**

The .NET Foundation needed to be an independent organization, but it
also needed heavy Microsoft backing so our customers would feel safe. We
also wanted to bring in commercial partners to help us modernize the
platform. Initially, Samsung and Red Hat joined us in those efforts, and
then eventually we expanded these partners to form the technical
steering group and corporate sponsors we have today.

Because we also had existing open source projects maintained by the
community that already had their own governance models, we decided to
build the infrastructure slowly and learn along the way. And let's face
it, we didn't know what we were doing, so we needed to go with a modest
approach to governance. There was a joke at the time; create the
"minimal viable foundation". So that's what we did. Believe me when I
say there were some people who didn't think we could do it at all.

We consulted lots of people. Robin Ginn, who was also on the .NET
Foundation v-team, played a critical role introducing us to open source
leaders. She was working for MS Open Tech at the time and has a vast
network in the open source community. Many leaders including Miguel de
Icaza, Ross Gardler and Jim Zemlin guided our thinking. As a community
manager for a closed product line, I soaked up open source learnings
like a sponge. It was a whole new world for me. The open source
community is huge, and I had (and still have) a lot to learn.

*The first thing we needed to tend to when we were starting the .NET
Core project was the licensing of .NET Framework (our original Windows
implementation of .NET)*

The first thing we needed to tend to when we were starting the .NET Core
project was the licensing of .NET Framework (our original Windows
implementation of .NET). We needed patent clarification so we could
assure the community that Microsoft would not come after anyone for
using the code. .NET Framework's code is source open, meaning the code
is available but we didn't take contributions back in the true open
source sense (you can't make PRs). We called it reference source. We
changed the license for the reference source to MIT license so anyone
could copy the .NET Framework code. This was important for the Mono and
.NET Core implementations.

> we literally had PRs coming in the moment we opened the repo

We knew we made the right decision right away. When we first started
.NET Core the community was overwhelmingly helpful, and we literally had
PRs coming in the moment we opened the repo. Within a couple of months,
while we all were focusing on Linux, one person in the community,
[\@kangaroo](https://github.com/kangaroo), added macOS support to the
.NET Core runtime! We were deeply humbled by the energy. I recall
someone saying that the community had increased our core team size by
60% right off the bat.

Of course, it all didn't go smoothly. Engineering leads now had to be
accountable for public code reviews. We needed to have the same
processes for internal and external PRs. We needed to balance internal
conversations with public conversations. We needed to change our
marketing strategy. We needed to figure out how to explain completely
changing a direction in designs (project.json to csproj anyone?). How do
we get our customers to understand the "new way of software development"
from Microsoft? Making a sausage isn't pretty.

**Ushering culture change**

Exactly one year after announcing the .NET Foundation, we hired our
first Executive Director, Martin Woodward. I was still working as the
community manager and I was super excited to have someone that cares as
deeply about the community as me join the team. Martin started in the
Java community and has a lot of experience running open source projects
and using open source software. He was a key person in changing our
culture. He was actually backstage on April 3^rd^, 2014 at Microsoft
Build making sure the Roslyn code went public on Codeplex without any
hiccups, as he was the lead for Codeplex at the time. He also looked
after the Microsoft org on GitHub and did a lot of other great stuff for
our ALM business.

Martin worked to make the .NET Foundation real with an advisory
committee and technical steering group. He created the dotnet org on
GitHub and did a lot of the actual implementation of the "vision" of the
foundation. Lots of paperwork. He wanted to democratize the
contributions to enable anyone to contribute. He created value with
project services like contributor license agreements, build and
deployment services, code of conduct implementation, and conflict
resolution processes. Basically, all the stuff that takes people away
from making actual contributions (writing code, raising and discussing
issues, writing docs...).

> There were many sleepless nights looking after employee welfare and
> making sure we were building up the skills on our team to manage and
> work with the community together effectively

There were many sleepless nights looking after employee welfare and
making sure we were building up the skills on our team to manage and
work with the community together effectively. Martin wanted to make sure
we could innovate quickly, but still have an SLA to make our customers
comfortable. This requires employee resources way beyond just people
writing code. We needed "social engineers" working in our repos. We
needed to build a new muscle. But it allowed us to be extremely agile
and get instant feedback.

He also started the vision to create a user group consortium, to bring
all the .NET meetups around the world together to teach, learn, and
collaborate. He also began a blueprint for a much more open membership
model, as he knew eventually the foundation would need to scale. As a
community manager I worked closely with Martin. It was one of the best
times and proudest moments in my career. We all worked toward making the
.NET Foundation the center of gravity for .NET open source.

**New role, same passion**

Then I moved to product marketing. I became the Product Marketing
Manager for the .NET platform in late 2015. I decided to move to
marketing for two main reasons. First, after being a community manager
and developer advocate at Microsoft since 2007, it was time for me to
try something new. Second, I felt that the engineering team had become
good community representatives themselves as part of going open source.
They didn't really need me in that capacity anymore. Fortunately, I
remained (and still remain) an important part of the .NET Foundation
execution and strategy.

> Today we have over 75 projects in the foundation

In this new role, I worked with Martin to bring the .NET Foundation
message to a much broader audience. In November 2016, at one of our big
online developer events called "Connect", we announced Google joining
our technical steering group and brought in a bunch more projects. Today
we have over 75 projects and 550 repos in the foundation. I was also
able to help the .NET Foundation by building strategic relationships and
getting our presence into non-Microsoft events and placements.

**New leadership, more growth**

In February 2017 Jon Galloway became the next Executive Director. Jon
was a developer advocate and .NET expert for many years and it was a
natural fit for him to continue to drive the .NET Foundation forward.
Well-known in the .NET community, he has pushed to organize our user
groups scattered around the world into one cohesive community. He's
brought on a huge amount of new innovative .NET open source projects,
facilitated a partnership to provide free code-signing certificates and
signing services to member projects, spoken at many events, produces a
lot of technical content, and has been the keystone of "running the
business" for the .NET Foundation.

*We've expanded our meetups to over 300 groups in 60 countries, and
organized our largest online .NET Conf ever in September*

We've continued to push the .NET Foundation forward with Jon at the
helm. We've expanded our meetups to over 300 groups in 60 countries,
expanded our social and online footprint, conducted Hackfests and
participated in Hacktoberfest, and are bringing on more projects and
partners. Our annual, online .NET Conf in September was the largest
ever, and we anticipate it being even bigger this year with the launch
of .NET Core 3.0 on September 23 and many open source project leaders
delivering sessions (see [www.dotnetconf.net](http://www.dotnetconf.net)
for details).

Jon's passion for the community has clearly shown the progress we've
made. Jon is awesome at helping overworked teams streamline their
processes and cutting out costs associated with building open source
software. He wants project teams, large and small, to be successful.
You'll see that there is a varying degree of team sizes across the open
source projects in the foundation today.

**Growing up**

Even with all that success, there was still only so much the foundation
could do. The next step for the .NET Foundation was to scale. Microsoft
was the only company providing funding for the .NET Foundation and had
two of the three board seats. Although one seat out of the three board
seats was a community-held position, and the advisory council and
technical steering group consist of strategic non-Microsoft partners, we
knew it was time to go broader and get fresh ideas. It was time to grow
up.

Over the course of Jon's tenure, we've worked to make the vision Martin
laid out for an open membership model a reality. In December 2018, we
announced membership model changes so that the community will directly
guide foundation operations.

*The Board of Directors has expanded to seven members, one seat
appointed by Microsoft and the other six open to the wider .NET
community*

The Board of Directors expanded to seven members, one seat appointed by
Microsoft and the other six open to the wider .NET community for people
to volunteer for a seat on the Board. Board elections were completed in
March 2019 and will happen annually. Any person who has contributed in
any way to any .NET Foundation open source project is eligible to run
for the Board and to vote. This new structure is helping the .NET
Foundation scale to meet the needs of the growing .NET open source
ecosystem.

We had a ton of fantastic, diverse candidates run for the board. I was
truly impressed with many of the campaign pages and qualifications that
each person could bring to the table. In the end, the community elected
Ben Adams, Iris Classon, Jon Skeet, Oren Novotny, Phil Haack, and Sara
Chipps.

I am the one appointed to the Microsoft seat on the new Board of
Directors and I promise to always have the best interests of the .NET
platform and community in mind when making decisions.

**What are we working on now?**

Open source software foundations are important for the entire open
source ecosystem, including contributors, project leaders, consumers, as
well as businesses that depend on open source. The .NET Foundation's
role is to provide a center of gravity for .NET open source and to make
sure the code that everyone relies on lives beyond the initial creators.
We also foster the ecosystem by supporting our community in many
different ways.

The Board of Directors is in the process of defining action groups and
committees in the following areas: Membership, Technical Review,
Marketing, Corporate Relations, Community Outreach, Speaker Bureau and
Meetups, and Project Support.

Right now most groups are just being defined on goals and setting up to
scale out to the many volunteers. I'm leading the Marketing group with
Phil Haack and we just opened up our meetings to our broader set of
volunteers. It's exciting to see the passion our members have and a fun
challenge to help enable them to do their best work.

**How can you get involved?**

If you rely on .NET and want to see the ecosystem thrive, then become a
.NET Foundation member! Join in the member discussions on GitHub and
help us with our action groups. Anyone who has contributed anything to
the .NET open source ecosystem can become a member. You don't have to
contribute code, you could contribute to documentation, file an issue,
write a blog, run a meetup group or organize .NET events. We're looking
for members that have a wide variety of backgrounds, not only coders.

Get started here: <https://dotnetfoundation.org/become-a-member>

**Conclusion**

I am incredibly excited about the future of the .NET ecosystem and
honored to be on the .NET Foundation Board. The platform is expanding
and innovating constantly, our community is growing, and our customers
are growing with us. I am thoroughly enjoying the ride and know that the
future is very bright. I hope you get involved and participate with us!

You can learn more about .NET Foundation and get involved on the website
[dotnetfoundation.org](https://www.dotnetfoundation.org). You can reach
me on Twitter \@BethMassi.
