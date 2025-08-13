**Encouraging Inner Source**

Martin Woodward - Director of Developer Relations, GitHub.

There are many things that developers can argue about. Tabs vs spaces,
vim vs emacs, the position of '{' after an if statement -- and don't get
us started on variable names. However, one thing that unites developers
is the love of solving problems and the joy you get from a particularly
well executed solution. I can't remember the number of times I've got a
bit of code just right and I spend the next 10 minutes just switching a
toggle on and off to re-run that bit of code and delight in how well it
works. This is usually the point when you go show it off to your
team-mates. The best software engineers love to share and love to get
better at the craft of writing software.

Many times, this is how open source projects start. You work on a
problem at home, come up with a solution you like so you post it to
GitHub. Before you know it, someone else looking for the solution to a
similar problem discovers your solution and tells you about an issue
they found, and ideally a suggested fix that you can pull from them that
makes your code better.

If you think about open source projects, it's amazing that they work at
all. A collection of individuals around the planet all working in
different locations, different timezones, often speaking different
languages and using the software to solve a problem in different
applications all with different business needs and different deadlines.
And yet it does. 99% of the applications we see deployed today contain
some open source, and even more amazingly 80-90% of the code you ship is
made up of your open source dependencies with 10-20% being the code that
you wrote.

But inside your organization, you also have lots of shared code and
shared logic. It might be how you validate and format customer reference
numbers, common logic for talking to in-house developed services or
Terraform code used to deploy an application into production. And yet
often we find we are better at sharing code with random strangers on the
internet than we are with the team sitting upstairs in your own company.
This is why many leading companies like Microsoft, Google, IBM, NASA and
more turn to inner source practices to support sharing inside their
organizations.

What is 'inner source'

Inner source is the sharing of knowledge, skills and code inside your
organization using open source style workflows. Easy to say -- but what
does that actually mean in practice?

It means providing systems inside your organization to allow people to
share with each other. It can be as simple as a file share or a
SharePoint site, but with modern tools we can do better than that. For
years we've had scalable source control systems inside most companies.

However these source control systems are typically configured with
restricted read/write access. Only the teams who need access to source
control have it and they typically guard that access jealously. Inside a
company, when a developer gets access to source control it traditionally
was always read/write access and there would be much grumbling when the
'dumb' team from across the building checked in some change which broke
your build and stopped your team from being its usual awesome self that
day. It is a maxim of any enterprise that the further a team is from
your team in the org chart or geographically, the more likely they are
to do 'dumb' things. When you realise this is true in every company and
in every team you work in you quickly realise it can't in fact be true
-- maybe it's communication challenges that are to blame. Also, why are
the processes in place in your organization such that a team is able to
mistakenly block you from working?

By adopting open source style collaboration inside the organization, you
set up a version control system that is distributed. You have permissive
read access but keep write access restricted to the maintainers of that
code (i.e. the same small core team as before). Others in the company
can read the source code of everyone else, take a fork of it and then
send over a pull-request if they would like to suggest some changes and
have them reviewed by the team who maintain that component. GitHub is
obviously particularly well suited to supporting these types of
workflows as you can set your projects as private or shared with anyone
in your organization and then easily control the behaviour of forks and
pull requests -- note that this is still all private to your
organization. You are just following the workflows of open source, with
the crucial difference that you are not making the code public to your
competitors.

It turns out that inside our companies, we also have many teams that are
geographically distributed. They are working on different project and
often have different deadlines. There are many more similarities and
lessons we can learn from working in the open that we can apply to
cross-company collaboration with inner source.

It's not just setting up version control to allow sharing, and indeed to
make it so that sharing by default becomes the norm inside the company.
You want to look at the contribution funnel to understand how to inner
source and open source projects work.

![](./media/image1.emf)


Figure 1 -- The Contribution Funnel

With inner source and open source projects, the vast majority of people
will just consume a shared component. This is perfectly natural and to
be encouraged, however you want to maximise the number of people coming
into the top of the contribution funnel. Therefore discoverability is
important -- how do people find shared components inside your
organization? Again, GitHub has tooling built in to help this but if you
are implementing inner source using other tooling then you need to pay
close attention to discoverability. Even if you are using GitHub, are
you providing a clear ReadMe file that helps people identify what the
project is about and help them find and consume the code easily? Are
they able to navigate the code and scripts to find what they want to
re-use?

A small percentage of the consumers contribute back time. That might be
telling you about issues / bugs, helping document areas or even telling
their colleagues about your component and advocating for it. Therefore
you need to think about how you help maximise the number of people
coming down into that funnel. How do people find out how to contribute
time in a useful way to you, what information do you need to capture in
a bug / issue etc, where do you most need help? You also want to be
welcoming to these people as they come in so that they feel part
invested in the project and the internal community around it.

A small percentage of the people contributing time will contribute code.
Again -- how can you maximise that funnel? You want to make sure that
you are responsive to pull requests, you want to make sure any coding
guidelines are documented so folks know how you would like code
contributed. You want to make sure you have an easily repeatable build
environment with clear and easy to reproduce dependencies. Ideally you
should also have an automated CI build set up that runs on every change
but also runs every time someone send over a pull request with a
suggested code submission -- that way the person contributing code gets
instant feedback if the code compiles, passes your tests, meets your
quality bar and is ready for human review.

And finally, of the people contributing code, only a small percentage
will help maintain that code going forwards. It may be that you want to
strictly control access to the list of maintainers to people on your
team -- and that is fine. But understand that if you choose to do that
then you are forcing the other group to permanently work on a fork of
your codebase which means you will end up with diverging codebases over
time. So you should ask yourselves what is in the best interest of your
organization and your shareholders and take the decisions about joint
ownership based on that. Remember -- it's not your team that owns the
code. You are paid to maintain it on behalf of the company and its
shareholders who are the people that own it.

Having all the tooling in place to support inner source is great, but
that is no good unless you have the culture in place to change how
people work. As we mentioned, developers naturally love to share cool
things so you have that working in your favour. However there are
emotional barriers in place that often hinder sharing. People feel a
strong sense of 'ownership' of source code. Often times they are worried
about the judgement of colleagues across the organization about the
quality of that code which might make them hesitate to share it. They
might worry about their ability to share knowledge with-in an
organization without getting intro trouble. They might also not want to
pay the teamwork tax of communicating with others or taking dependencies
on other teams that outside their managers direct reporting line and
sphere of influence.

Therefore, as an organization you need to strive to build an economy of
sharing with-in the company. You need to positively encourage
individuals and teams that share and highlight their achievements. You
should also look at your incentives for your engineering teams. When
rewarding them, do you look at what impact they have had alone or do you
also look for evidence about what impact they have had on other teams
and what work they have done that has built on the work of others? By
making those questions part of your core incentive model you not only
encourage teams to find opportunities to work with each other you also
encourage them to highlight the fact they have worked with other teams
rather than trying to take all the credit. This ensures that all the
people involved feel that they have been recognised and are getting
rewarded.

By encouraging a culture in your organization that rewards
collaboration, that allows developers and ops teams to learn from each
other and share best practices you also build a culture that has a
growth mindset and that is always looking to learn, to get better and to
improve. Not only is that exactly the sort of place that I want to work,
you'll find it's the kind of company culture that will attract and
retain lots of high performing talent. More importantly, it definitely
makes work a lot more fun and rewarding. I highly encourage you give
inner source a try.
