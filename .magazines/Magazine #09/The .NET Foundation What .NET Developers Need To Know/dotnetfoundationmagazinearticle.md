# The .NET Foundation: What .NET Developer Need To Know

I've been the Executive Director of the .NET Foundation since early
2017. Usually when I talk to people about it, they say something like
"Wow, that's great... um... what's that?" Honestly, before I joined the
team, I didn't know much about the .NET Foundation, or software
foundations in general. The previous Executive Director of the .NET
Foundation, .NET open source icon Martin Woodward, let me know that he
was moving on to an exciting new role at Microsoft, and he wanted me to
consider taking his role in the .NET Foundation. Fortunately, I got to
learn about the .NET Foundation from Martin and Beth, and when I
understood what the .NET Foundation is, I was excited both to get
involved and to spread the word.

I think the best way to understand the .NET Foundation is to look at
what problems and challenges it endeavors to solve.

As a .NET developer, my experience with open source started out all
sunshine and rainbows: "Wow, people are just going to write software,
then give it away... including the source code?" Then after a while, I
started getting involved in contributing to open source projects, and
that was pretty awesome, too! I got to collaborate with some top notch
programmers, and we got to decide what features we wanted to build and
how things would work. Then, over time, open source became more
mainstream, and more companies started shipping code under open source
licenses and supporting open source projects. It was great!

But after a while, I started seeing some recurring challenges:

-   Projects I really liked would sometimes be abandoned. Often what
    seemed like a really strong, established project came down to just
    one or two people, working nights and weekends, and then they'd get
    a new job, have some kids, or just get burned out, and the project
    would grind to a halt.

-   Successful projects would start to get overloaded administrative
    burdens, miscellaneous costs for things like web hosting and
    certificates would grow, and all of those things would distract from
    fixing bugs and shipping features.

-   The growing interest in open source from large companies was great,
    but it brought some complications, too. How could community members
    collaborate effectively on a project that was mostly run -- and
    funded -- by a big company?

It turns out that many of these challenges aren't new, and different
open source communities have already tackled them by establishing
software foundations. You may be familiar with some of them by name,
even if you're not sure exactly what they do: Apache Foundation, Linux
Foundation, Eclipse Foundation, Software Freedom Conservancy, OpenJS
Foundation, etc. These foundations are all unique in their approaches
and communities, but at a high level all of them exist as independent
entities that are focused on keeping an open source ecosystem healthy
and growing.

The .NET Foundation is an independent organization (founded and
partially supported by Microsoft, but separate) focused on supporting
the open source .NET community. So let's talk about how it has
approached some of the challenges I mentioned above, and then look at
how we're going to build on that in the future.

## An independent home for .NET open source collaboration

One of the original key focus areas of the foundation was to allow for
healthy, authentic collaboration on the .NET platform that Microsoft had
been developing largely behind closed doors until 2014. At that time, as
.NET Core was being created as a new cross-platform development
platform, it was important that it be a real open source project,
developed in the open, and with significant community collaboration and
ownership. This was especially important due to the history of .NET as a
closed source product; in order for the community to see this as a true
open source, collaboratively run project, it couldn't be "owned" by
Microsoft. So it's not -- and if you look at the source code for .NET
Core, you'll see that the code is copyright .NET Foundation, not
Microsoft. All developers who contribute code to .NET Core sign a
Contributor License Agreement (CLA), and every commit is contributing
that code to the .NET Foundation. This allows for developers worldwide
-- independent developers in the community, developers working at
thousands of companies, and Microsoft employees -- all to collaborate on
the same codebase, since the source code is all under the ownership of a
neutral third party whose central focus is to support the open source
.NET community.

And it's worked! Since that time, we've seen a ton of community
contribution and involvement in .NET Core. 87% of our contributors are
outside of Microsoft, and over 61,000 pull requests from the community
have been accepted. Matt Warren has written a yearly blog post series
(<https://aka.ms/dotnet-oss-community-contributions>) where he analyzes
community contribution to .NET repositories, and it continues to show a
huge amount of momentum. The Cloud Native Computing Foundation shows
.NET as eighth on their list of the top 30 highest velocity open source
projects on GitHub.

## Supporting .NET Open Source Projects

Another important focus area for the .NET Foundation has been to support
community contributed projects. The .NET community has built some really
useful open source projects, but many of these are run by small teams of
volunteers. In order to build a healthy ecosystem, we want to do what we
can to make sure these projects can continue to grow and thrive over the
long term. This is important for everyone -- we want project leaders to
be successful and happy so they'll keep building and releasing great
projects; as consumers of open source, we want to be able to rely on
projects staying around, releasing updates, fixing issues, etc.

The .NET Foundation supports over 75 member projects in a lot of
important ways. One really important aspect is Intellectual Property
(IP) and Legal support. When a project joins the .NET Foundation, the
source code is contributed to the .NET Foundation and we set them up
with the same CLA system that's used for .NET Core so all future
contributions are contributed to the .NET Foundation. That's helpful for
the project leaders since we can legally defend any issues around code
ownership or infringement. It's also really important to consumers.
Instead of depending on "one or two random people on the internet", the
project is supported by a legal entity for the long term, so even if the
project leaders disappear (I like to say "wins the lottery" rather than
"gets hit by a bus"), the project can add new maintainers and continue
on.

We also provide a lot of services to projects to cut down on
administrative work and costs, so the project leaders can focus on
building software. This includes things like website hosting, devops
services, certificates, marketing support, and subscriptions to a lot of
online services. One example is secret sharing services like our
LastPass enterprise subscription to allow project leaders to securely
share passwords and other keys so that there's no single point of
failure.

There's also a lot of case by case support where a project needs help
with a specific issue. In one case, one day I noticed on Twitter a
community project that offers debugging symbols for NuGet packages was
shutting down. They'd been offering a service with both free and paid
levels of support; after running that for a few years, they decided that
the paid model wasn't earning enough to support the free service, and
they were going to have to shut it down. I reached out to them and,
together with the NuGet team, we worked to bring them on as a .NET
Foundation project and run the free service using .NET Foundation Azure
resources.

Another fun project was getting code signing certificates and services
for .NET Foundation projects. It's a best practice for open source
projects to sign their binary distributions (installers, NuGet packages,
etc.), but getting a code signing service requires that your project be
registered as a legal entity, and setting up code signing for builds is
a little complex. Oren Novotny, who was then an advisory council member
and has since been elected as a board member, came up with a great
solution where we would register projects as trade names under the .NET
Foundation.  We worked with DigiCert, a certificate provider, to get
discounted certificates for .NET Foundation project. We actually got the
.NET Foundation set up as a sub-certifying authority, which allows each
project to be issued a certificate in their own name. Then we set up a
code signing service on our .NET Foundation Azure subscription and
brought on any of our projects that wanted to make use of it. It took
several months of meetings, false starts, legal agreements, and
technical setup, but it really helps out our projects. Again, this was
Oren's idea, but I was really happy to be able to make it an official
.NET Foundation initiative and help get it done.

## Building a healthy worldwide .NET developer community

In addition to the open source collaboration and project support areas,
the .NET Foundation does a lot to support the worldwide .NET community.
We set up a Meetup Pro group to make it easier to find a local group,
and it's since grown to over 300 groups in 60 countries. We work with
Meetup leaders to organize local events. For instance, every September
the .NET Foundation helps run an online conference called .NET Conf, and
we work with our Meetup network to help them run local viewing parties
and follow-on events through the end of October. We send them swag packs
to give away to attendees, presentation materials, and help promote
their events. Some Meetups have turned these events into
mini-conferences that last a few days and bring in hundreds of
attendees.

## Scaling up

Being the executive director of the .NET Foundation is an interesting,
exciting, challenging job. I'm a Microsoft employee, and they donate a
lot of my time to the .NET Foundation, kind of like when a company
allows an employee to contribute to an open source project. I report to
our board of directors and work with our advisory council and technical
steering / corporate sponsor group. I just listed all the things the
.NET Foundation does; my job is to make all those things happen. I
manage everything from budget and business registrations, legal
agreements, new initiatives, communications, our swag store, local
events, and new things as they happen. Obviously, having just one person
do everything doesn't scale well, so another important part of my job is
to evolve the organization to allow for more people to get involved.

Recently, we made some big changes to help the community get more
involved: .NET Foundation Open Membership
([[https://dotnetfoundation.org/blog/2018/12/04/announcing-net-foundation-open-membership]{.underline}](https://dotnetfoundation.org/blog/2018/12/04/announcing-net-foundation-open-membership))
and a community elected board of directors. The .NET Foundation has been
a separate entity since it was founded, but two of our three directors
have been Microsoft employees and the third was appointed by Microsoft,
so it wasn't really that independent. We looked at a lot of other open
source software foundations and decided we liked how the GNOME
foundation worked: people who are active in the developer community can
apply to become members, and the members elect their own board. So our
new board has one Microsoft appointed member (Beth Massi), and the
remaining six directors are members who ran for the position. Each of
them serves for one year, after which they can run for re-election if
they want. It\'s important for two main reasons:

-   This very clearly gives control of the .NET Foundation to the open
    > source .NET Community. That allows them to decide what the
    > foundation does, and also gets the word out to the community so a
    > lot more people can get involved.

-   This is a good model to scale up what we can get done. Instead of
    > one executive director (me) doing all the work, we now have seven
    > board members and hundreds of community members who can form teams
    > and work on things they see as important.

## Time for Action!

One of the first things our new board of directors did was agree on some
new action groups. The whole idea is to scale up, from one person doing
all the day to day stuff (me) to teams of dozens of community members.
They're basically committees, but I liked the name action group since
committees can be really focused on talking and we want these action
groups to be focused on doing. Action!

Here's the list of action groups:

-   Project Support: As we bring on more projects, we need to scale up
    > better to review new project applications, get new projects set
    > up, handle case by case support issues, etc.

-   Outreach: Focused on reaching new developers, with a special focus
    > on diversity and inclusion. We'd like our membership, and the .NET
    > developer community, to be available and welcoming to everyone,
    > spanning genders, races, cultures, age groups, and blind spots we
    > weren't even aware of.

-   Membership: This team reviews new member applications, and helps
    > figure out what membership means -- benefits, ways to get
    > involved, etc.

-   Corporate Relations: .NET is heavily used in business, and this team
    > focuses on that relationship. It reaches out to corporate
    > sponsors, and looks at how we can better involve corporations and
    > corporate developers in.NET open source.

-   Speakers and Meetups: Now that we have a network of over 300
    > worldwide Meetups, this group works on connecting speakers and
    > meetup. We're working to set up a speaker bureau, speaker grants
    > program to cover travel, mentorship for new speakers, etc.

-   Technical Review: The goal of the technical review group is to
    > provide an independent viewpoint, separate from Microsoft, on the
    > technical direction of our projects.

-   Marketing: The main goal of marketing is to create consistent,
    > powerful storytelling in order to increase share of voice and
    > establish .NET Foundation industry relevance. We focus on the
    > marketing efforts for the .NET Foundation itself and .NET in
    > general.

-   Communications: This team focuses on communicating and coordinating
    > our regular communications with the .NET Foundation members and
    > broader .NET open source community.

An important goal of all of these action groups is to document how we do
things, so new people can get involved and we can eliminate single
points of failure. In the same way that the .NET Foundation has been
working for years to make sure projects are sustainable and not
dependent on individuals to keep functioning, we also need the .NET
Foundation itself to be set up as a sustainable and scalable
organization.

As expected, there are growing pains. A lot of things are easy to do
when it's one or two people following systems and policies that are only
in their head. However, they take a little time to document and turn
over to a team. The process has also required us to figure out how to
effectively communicate with larger teams. We've settled on GitHub
organizational teams for discussions and moved things like our monthly
newsletter and project onboarding to public GitHub repos. It's already
paying off -- things are moving faster, and having more people involved
is resulting in a lot higher quality. We've started with the Project
Support and Marketing teams since those processes were the best defined,
and in some cases partially documented; now we're working to roll those
practices out across our other teams.

## Get Involved!

If you're interested in getting involved, now is a great time! Start by
applying for individual membership
(<https://dotnetfoundation.org/become-a-member>). In order to apply, you
need to have contributed to the open source .NET community, but the
requirements aren't designed to keep anyone out. Contributions may
include code contributions, documentation, or other significant project
contribution, including evangelism, teaching, code, organizing events,
etc. If in doubt, please ask us at <contact@dotnetfoundation.org>. Once
you've joined the .NET Foundation as a member, you can get involved in
one of our action groups, and you can participate in our annual board
elections, either by voting or running for a seat.

If you're a .NET developer, the .NET Foundation is for you. We exist to
make sure that the .NET community is healthy and growing, and to support
the projects you care about. Join us!
