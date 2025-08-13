# Scaling Scrum Professionally using Nexus and Visual Studio Team Services

Jesse Houwing

If you have been using Scrum to develop products, you have probably
found that the Scrum Guide only describes the core rules of Scrum,
regardless of scale. A lot of companies want to use Scrum to work on
multiple products or to develop a comprehensive product that requires
multiple teams. The question then arises of how to organize product
ownership. Even more common are situations in which the eyes are bigger
than the stomach, where the list of features is much longer than their
current team can deliver and where companies need to scale to multiple
teams to deliver more items of the backlog within a given timeframe.

All of these cases require some form of scaling, but the element which
is scaled is different in each of these situations. Yet in all of these
cases, dependencies are increased, between teams, between Product
Backlog items and sometimes even between different products. Managing
these dependencies requires changes in the way people work (processes
and people) and this is difficult without the help of supporting tools.

Many organizations have been scaling Agile and Scrum for years. And for
a long time there was a list of commonly used practices and patterns
used in the Agile Community, but nothing fully defined. More recently,
scaling frameworks such as SAFe and LeSS have been released. SAFe
appears to be a complete methodology that tries to encompass large and
complex organizations from portfolio management and budget allocation
all the way down to the team organization. On the other hand, LeSS
mainly focuses on large development organizations with a single product
or product family, while it also includes mandatory organizational
changes.

Scrum.org has released its vision on scaling in the same succinct way
that Scrum is defined, i.e. with the release of the Nexus Guide[^1] that
introduces the Nexus Framework, as well as the Scaling Professional
Scrum course and assessments. The course introduces the Scaled
Professional Scrum practice library. The iterative and incremental
approach to scaling is what makes Nexus stand out from the alternatives.
The Nexus framework strongly emphasizes technical excellence in order to
scale Scrum sustainably, recognizing that many people who use Scrum are
still having trouble delivering a Done increment of working software as
one team. Obviously, things only get worse when scaling up, when more
teams are involved.

Personally, I am much more comfortable with this empirical approach of
technical excellence than any big-bang scaling effort which tries to
change the tools, technology, teams and organization all at once.

Microsoft has developed a Scrum template in the past, and the Agile
experience in Visual Studio Team Services has recently been revamped.
Most of these new agile features have also shipped as part of Team
Foundation Server 2015 update 1.

In this article we will focus on the way in which Nexus relates to Scrum
and then look at some of the challenges experienced by teams and how
tools such as Visual Studio Team Services can support them.

**Introducing Nexus**

In order to scale Scrum, the creators of Nexus stayed away from a
prescriptive approach to scaling, because experience shows that no
single solution to Scrum, let alone at scale, works in every possible
environment, not even within the same organization. Over time, and with
every release of the Scrum Guide since 2010, Scrum has been simplified
by removing things that were too prescriptive. No scaling method or
process should undo those simplifications. This is why it shouldn't be a
surprise that Nexus is a lot like Scrum.

As Ken Schwaber, co-creator of Scrum.org describes it, Nexus uses Scrum
to scale Scrum. Nexus focuses on complex product development in which
multiple teams work on the same product or product family.

When working on one product with one or two teams, Scrum suffices and
the overhead of a scaling framework doesn't add value, it may even do
harm. When working with multiple products and multiple teams, each
product can use Scrum or Nexus to manage their product development, in
addition, portfolio management may be required for budgeting and
prioritization across products, projects and other investments. However,
Nexus does not cover this topic.

Balancing multiple products across a single team tends to be chaotic. As
long as the work is not complex, this may be possible, but for complex
work such as software development, this approach is not sustainable.
Kanban can help organize the work, but only if the level of complexity
can be kept to a minimum (support, operations and maintenance are
examples that tend to work relatively well in this model).

The various combinations can be visualized in the following way

+-------------------+------------------------+------------------------+
|                   | ![](./med              | ![](./med              |
|                   | ia/image1.png) |
|                   | 0.43757108486439195in" | 0.43757108486439195in" |
|                   | height="0.4642246      | height="0.4            |
|                   | 281714786in"}![](./med | 642246281714786in"}**\ |
|                   | ia/image1.png){width=" | \                      |
|                   | 0.43757108486439195in" | One Product**          |
|                   | height="0.4642246      |                        |
|                   | 281714786in"}![](./med |                        |
|                   | ia/image1.png){width=" |                        |
|                   | 0.43757108486439195in" |                        |
|                   | height="               |                        |
|                   | 0.4642246281714786in"} |                        |
|                   |                        |                        |
|                   | **Multiple Products**  |                        |
+===================+========================+========================+
| !                 | Kanban\                | **Scrum**              |
| [](./media/image2 | or chaos               |                        |
| .png){width="0.58 |                        |                        |
| 20538057742782in" |                        |                        |
| height="0.4001618 |                        |                        |
| 5476815397in"}**\ |                        |                        |
| \                 |                        |                        |
| One Team**        |                        |                        |
+-------------------+------------------------+------------------------+
| !                 | Portfolio Management   | **Nexus**              |
| [](./media/image2 | with **Nexus** or      |                        |
| .png){width="0.58 | **Scrum** at for each  |                        |
| 20538057742782in" | product                |                        |
| height="0.4001    |                        |                        |
| 6185476815397in"} |                        |                        |
| !                 |                        |                        |
| [](./media/image2 |                        |                        |
| .png){width="0.58 |                        |                        |
| 20538057742782in" |                        |                        |
| height="0.40016   |                        |                        |
| 185476815397in"}! |                        |                        |
| [](./media/image2 |                        |                        |
| .png){width="0.58 |                        |                        |
| 20538057742782in" |                        |                        |
| height="0.4001    |                        |                        |
| 6185476815397in"} |                        |                        |
| !                 |                        |                        |
| [](./media/image2 |                        |                        |
| .png){width="0.58 |                        |                        |
| 20538057742782in" |                        |                        |
| height="0.4001    |                        |                        |
| 6185476815397in"} |                        |                        |
|                   |                        |                        |
| **Multiple        |                        |                        |
| Teams**           |                        |                        |
+-------------------+------------------------+------------------------+

Nexus does not attempt to resolve the challenge of an Agile
organizational transformation, nor does it resolve the common situation
in which one team tries to balance its capacity across multiple products
at the same time. It provides a simple framework in which multiple teams
can learn how they work best together on one or several products.

To better understand how Nexus is an extension of Scrum, let's look at
the differences and similarities between Scrum and Nexus.

  -----------------------------------------------------------------------
  **Scrum**                           **Nexus**
  ----------------------------------- -----------------------------------
  Scrum is a Framework, and it can    Nexus is a Framework, and to make
  encompass many good agile           it work, it needs to be extended
  practices. Practices that work for  with good agile practices.
  one product and one organization    Practices that work for your
  may work differently for other      product and organization may be
  products and organizations.         different than those of another
                                      organization. Due to the added
                                      complexity of working with multiple
                                      teams, these practices are even
                                      more important.

  Scrum focuses on teams of 3-9       Nexus focuses on 3-9 Scrum teams,
  people. Fewer people don't need the which as a whole are referred to as
  overhead of scrum. More people      the Nexus. Fewer teams don't really
  cause communication to break down.  need the overhead of a scaling
                                      framework. More teams will be
                                      unable to work together
                                      effectively.

  A Scrum Team has all the skills     A Nexus consists of teams who have
  required to deliver an increment of the skills required to deliver an
  working software.                   increment of working software.
                                      Nexus allows for the existence of
                                      component or layer teams, but all
                                      work done in the Sprint across all
                                      teams must deliver value together.

  A Scrum Team works on a single      A Nexus works on a single product
  Product.                            or product family.

  A product has a single Product      A product has a single Product
  Owner, though some responsibilities Owner, though he may delegate some
  may be delegated to the teams.      of his responsibilities to the
                                      teams.

  A Scrum Team delivers an integrated A Nexus delivers an integrated
  increment of working software every increment of working software every
  Sprint.                             Sprint.

  Sprints are 30 days or less.        Sprints are 30 days or less.

  The main events of Scrum are Sprint The main events of Nexus are the
  Planning, Daily Scrum, Sprint       same as those in Scrum with the
  Review and Sprint Retrospective.    addition of Product Backlog
  They are contained in a Sprint.     Refinement as a new time-boxed
                                      event.\
                                      \
                                      Most events are extended to handle
                                      cross-team interaction, indicated
                                      by the addition of a Nexus
                                      component. So there's a Nexus
                                      Sprint Planning, Nexus Daily Scrum
                                      etc.

  The main artifacts of Scrum are the The main artifacts of Nexus are the
  Product Backlog, Sprint Backlog and same as those in Scrum, with the
  the Increment.                      addition of the Nexus Sprint
                                      Backlog, which shows the
                                      dependencies between the Sprint
                                      backlogs of the individual teams.

  The Product Owner, Scrum Master and Nexus adds a Nexus Integration Team
  Development Team are the only roles (or NIT) which consists of the
  that exist in Scrum.                Product owner and those members of
                                      other teams required to facilitate
                                      integration.
  -----------------------------------------------------------------------

Even graphically Nexus looks remarkably similar to Scrum, which makes it
easy to understand for organizations that have been applying Scrum for
some time now. You can see the word Nexus popping up a number of times,
usually just before or just after a standard event.

![](./media/image3.png)

Figure

-   **Scrum Team** and **Development Team** - unchanged. The Scrum Team
    still consists of a Product Owner, a Scrum Master and a Development
    Team, consisting of people with all the skills required to deliver
    the work required.

-   **Product Backlog** -- Unchanged. It is still a single ordered list
    of Product Backlog items owned by a single Product Owner.

-   **Product Backlog Refinement** is introduced as an official event.
    It happens during the Sprint and there may be multiple events
    (sometimes in parallel) to which teams send one or more team
    members. It is important that these events are attended by the right
    representatives from the individual teams. One of the primary goals
    of these refinement events is to see how work can be broken down or
    combined to remove dependencies between teams.

-   **Sprint** -- Unchanged. It is still a time-box in which all the
    events of Scrum and Nexus take place.

-   **Sprint Planning** is extended. Before teams start their Sprint,
    the Product Owner shares the vision and the primary goals for the
    next Sprint. Delegates from each team come together to discuss
    Product Backlog items that are mutually dependent. Afterwards, each
    team launches into a normal Sprint planning meeting, so all teams
    plan their Sprints at the same time. This works best when all teams
    are in the same room, although this is not mandatory. This combined
    event is called the **Nexus Sprint Planning**.

-   Each team still has its own **Sprint Backlog** to ensure that all
    work is transparent within the teams. One additional artifact is
    created, the **Nexus Sprint Backlog**. This shows the Product
    Backlog items each team will work on during the Sprint. It also
    visualizes the dependencies between these items and teams.

-   The **Nexus Daily Scrum** happens on a daily basis, and takes place
    before each individual team meets in their Daily Scrum. In the Nexus
    Daily Scrum delegates from all teams come together to flag any
    impediments or dependencies from other teams that will impact the
    plan for the next 24 hours. The primary source of inspection of a
    Nexus Daily Scrum is the state of integration of the work of the
    Nexus.

-   All work that's been done, i.e. work that is fully integrated across
    all teams and delivered as an **integrated Increment** of product,
    is presented at the **Nexus Sprint Review** for feedback. All teams
    deliver this review together. There are no individual Sprint Review
    events.

-   The Sprint ends with the **Nexus Retrospective**, during which all
    teams together discuss how the Sprint went at the Nexus level and
    which things may need to be improved. The outcomes are taken into
    the Sprint Retrospectives of the individual teams. At the end all
    teams come together to share changes to their process, team or
    tools, and share anything that might be useful for the other teams.

-   Nexus introduces one new "virtual" team, the **Nexus Integration
    Team**. Its role in the Nexus is to facilitate collaboration and
    integration across the teams, and remove impediments at the Nexus or
    organizational level. The team is made up of the Product Owner and
    the "right selection" of team members from all Scrum Teams. Its
    composition may change over time, as the problems they try to solve
    changes.

Nexus assumes that the Scrum Teams will know how to solve the problems
they are facing and how to best implement the work on the Product
Backlog. It does not provide prescriptive guidance on how to conduct a
Sprint Planning meeting with 9 teams, nor does it specify how to remove
dependencies between teams. This is where Scaled Professional Scrum
provides building blocks to extend the Scrum framework to the level of
3-9 Scrum Teams developing and sustaining one product.

**Introducing Scaled Professional Scrum**

The main reason why Nexus can be explained in only 10 pages is also the
reason why Scrum.org is developing Scaled Professional Scrum (SPS).
Nexus provides a framework that shows how to work together and how to
improve incrementally. Scaled Professional Scrum is a library of
practices that have been proven in the field in different situations. It
also includes practical guidance and a two-day workshop to gain hands-on
experience. You may compare it to the good old Gang-of-Four Patterns and
Practices book. If you use Nexus and Scaled Professional Scrum, you will
discover what is the best way to scale Scrum for *your* organization.

You can test your knowledge of Nexus, Scrum and common scaling practices
by taking the Free Nexus Open assessment. Certification can be achieved
by passing the Scaled Professional Scrum assessment.

**Combining Nexus with the practices from SPS and tools from Visual
Studio Team Services**

We have observed how even one Scrum Team often has a hard time producing
a releasable increment at the end of each Sprint. This becomes
exponentially more difficult when you are integrating and testing work
that needs to happen across multiple teams, at least every month, or
less frequently.

Most agile practitioners, coaches and trainers will swear by physical
boards, and actually holding sticky notes in their hands. Indeed, it
changes the level of involvement of people, and it may give them a
feeling of accomplishment when physically moving a task or PBI to Done.
Yet when working with nine teams, especially if they're not all in the
same location, it is unlikely that teams can be effective with physical
boards only. It has become common to work together remotely, in
different time zones or have team members working part-time for the
company, tools and technology become extremely important for providing
the required support to deal with these collaboration issues.

While the physical world works best to facilitate communication,
collaboration and knowledge sharing, technical practices such as Source
Control, Continuous Integration and automated testing help teams to work
together and integrate at a technical level. Any team who has used these
tools is unlikely to go back.

Let's take some of the most common scaling challenges and look at how
people, processes and technical practices can help us 'resolve or reduce
these.

-   **Coordinating dependencies**\
    The first way to resolve dependencies between teams is to
    proactively identify them, and try to work around them. This is why
    Nexus introduces Product Backlog Refinement as an official event.
    When working with multiple teams, Refinement is no longer an
    optional activity. Instead, it becomes a crucial forward-planning
    event. Refinement is not about writing User Stories and acceptance
    criteria; it is about figuring out which dependencies exist in the
    current Product Backlog and how to reduce or completely remove the
    dependencies. The means to do so may include knowledge exchange,
    shared code ownership, rewriting Product Backlog items or
    decomposing or splitting functionality.\
    \
    To reduce dependencies between teams and increase their autonomy, it
    helps to have teams that are able to work independently in defined
    areas of the product you're building. Cross-functional teams
    centered around one or multiple related feature areas are strongly
    recommended as opposed to component or layer teams.\
    \
    However, some coordination across teams will always be required, and
    the Nexus Sprint Backlog makes these dependencies transparent. The
    teams share useful updates on integration and other dependencies in
    the Nexus Daily Scrum, prior to the individual teams' Daily Scrum.

At a higher level, the Product Backlog is an ordered list of items. To
ensure that the Product Backlog does not become fragmented, it is
important to assign work to teams only just before or at the start of
each Sprint. If you don't, you will end up with one Product Backlog for
future work and a Product Backlog for the work each team has claimed.
This is detrimental to transparency. It will be very hard to see how
work on one team's backlog relates to that of another team, and it may
be that more valuable work is delayed when one team is delivering faster
than another.\
\
In many tools, including Visual Studio Team Services, it is easy to lose
the grand overview of the backlog when work is preassigned to Sprints
and teams. Visual Studio Team Services provides some good alternatives:
it is better to rely on the Forecasting capabilities[^2] to identify
which work will be delivered in a future Sprint combined with
Tagging[^3] so teams can pre-emptively volunteer for specific items on
the Product Backlog.

![](./media/image4.png)


Figure

Visual Studio Team Services also provides integration with tools such as
HipChat and Slack, which helps teams communicate quick status updates
and find answers to questions from other teams more effectively. These
digital channels make it easy to share links to Product Backlog Items,
Build artifacts, etcetera.

-   **Delivering a fully tested product every Sprint\
    **When working on a large product in short Sprints it is easy to
    spend most of the Sprint's time on testing, or to simply not run all
    tests during each Sprint. Defects may sneak into areas of the
    product when it is least expected, causing teams to accumulate
    technical debt, often unknowingly. Testing is essential to deliver a
    fully integrated increment of working software. It should be part of
    every team's definition of Done and should thus be performed every
    Sprint. Testing is an important strategy to identify unwanted
    dependencies that were not caught proactively.

> ![Adaptable and open](./media/image5.png)
> 

Figure

> The only way to prevent a manual test-avalanche, and omitting
> important tests, is automation. Automation can be applied at multiple
> levels, and Continuous Integration will ensure that the sources build,
> while running the unit tests will ensure that no breaking changes are
> introduced. Automated deployment to test environments for automated
> integration and system test ensures that components will work together
> as expected. With complex environments, it may also be required to
> automate the provisioning of such environments.
>
> Visual Studio Team Services provides Build, Test and Release
> Management features that can help your teams with these continuous
> integration problems.

-   **Effective meetings\
    **In a Nexus you'll have an absolute minimum nine people, three
    Development Teams, each consisting of three members, and up to nine
    Development Teams, each consisting of nine members (81). Six teams
    of six people might be an optimal size (36). Obviously, the Scrum
    Masters and Product Owner need to be added to this number. Effective
    meetings with such large groups are impossible or at best incredibly
    expensive for any given organization, unless this is facilitated
    effectively. This is why most Nexus events are attended by
    representation from each team. It is extremely important that the
    right people are selected to represent a team. Sending the Scrum
    Master or the team lead may not be the most effective way to smooth
    dependencies or resolve issues between teams. Instead, sending the
    person who has the most knowledge about the subject may be
    required.\
    \
    Some coordination around stakeholder communication and sharing of
    the outcomes of such meetings may be required to prevent a
    stakeholder from being overloaded. Tools such as Visual Studio Tea
    Services can help capture the outcomes of such meetings as Product
    Backlog items and Acceptance Criteria, but it may not be enough to
    just capture the information in a tool. The most effective way of
    communication remains face-to-face.\
    \
    The shared Nexus Sprint Review is hard to maintain as an effective
    and valuable meeting for all people attending when each team does a
    show-and-tell of their individual work performed during the last
    Sprint. Many teams have found that a "Science Fair" or "Open
    Space"[^4] format works well. In these cases, teams generally start
    with announcements and an explanation of the agenda. Each team then
    presents its Done functionality and receives feedback in multiple
    parallel slots, allowing stakeholders to attend the topics most
    valuable to them.

> Since meetings are most effective when conducted face-to-face and
> visually, having an open laptop or computer screen can reduce the
> value of a meeting by being a distraction. Yet, not many people take
> the time to create sticky notes so they can use free format ways to
> play with backlog items and tasks. Visual Studio Team Services can
> still help you make your meetings more interactive and effective
> though. Extensions such as "Print cards" enable you to quickly
> transform your digital backlog into a physical one[^5], allowing teams
> to conduct visual meetings where they can annotate, arbitrarily group,
> physically split in two items on a board, unbound by process and
> tools.

While the above are some common challenges and practices, you may find
yourself in a different situation. Don't be afraid to experiment, try
out new things and continuously try to improve the way you work as an
individual, a Scrum Team and a Nexus.

**Conclusion**

Nexus is an effective way to scale Scrum; it was developed by one of the
original founders of Scrum. Unlike SAFe and LeSS, which enforce a lot of
structure and process on organizations, it takes into account that not
all organizations are equal and that they don't have to be in order to
scale effectively. It introduces a number of new concepts to help teams
collaborate effectively when they grow past a certain size (generally
when there are three or more teams), but it tries to limit its
prescriptiveness to a minimum.

"Scrum is simple, but hard" is an often-heard phrase. Nexus isn't any
different. While the framework is remarkably simple, it's very hard to
carry out complex, creative software development with multiple teams and
deliver new, releasable software every 30 days or less.

With technical excellence, a focus on continuous improvement and hard
work, it is possible though. You will find some great examples to try in
the SPS curriculum and there are boundless other great resources for
ideas. Nexus as a framework allows me extend it by borrowing some
practices from comprehensive methodologies such as SAFe and try them to
see how they can be adapted to resolve a team's challenge.

Tools such as Visual Studio Team Services provide a complete environment
to support one or more teams to work together effectively. They cover a
broad spectrum from work management through to source control all the
way through to building and releasing your software to production.
Configuring them correctly and using the right features to accomplish
the job is paramount to getting the best value. With a little guidance
and an empirical approach to applying the tools and practices, amazing
results are possible.

A simple framework with the support of these comprehensive tools can
help you effectively scale your product development from a single scrum
team to nine teams, and if need be work around the hurdles caused by
distribution across time and place.

Jesse Houwing is a Professional Scrum Trainer with Scrum.org and a
Microsoft ALM MVP. Together with his colleagues at Xpirit he helps teams
improve their process, skills, tools and environment to work together
effectively. Scrum and Visual Studio Team Services are his tools of
choice.

[^1]: https://www.scrum.org/Resources/The-Nexus-Guide

[^2]: https://msdn.microsoft.com/library/vs/alm/work/scrum/velocity-and-forecasting

[^3]: https://msdn.microsoft.com/en-us/Library/vs/alm/Work/track/add-tags-to-work-items

[^4]: http://openspaceworld.org/

[^5]: https://marketplace.visualstudio.com/items/ms-devlabs.PrintCards
