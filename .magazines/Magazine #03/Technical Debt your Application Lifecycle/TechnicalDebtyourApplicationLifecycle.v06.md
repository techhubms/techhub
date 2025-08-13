Addictions are a serious problem. Using a survey assessing alcohol usage
and personal behaviors can quickly help discover the seriousness of the
problem. If only assessing the impact of technical debt was that easy.
Addictions are usually ignored until the effects of the addiction are
really bad. The same can be applied to technical debt in your
application lifecycle. It is there, but often it only surfaces when
gaining control is difficult, requiring a lot of work and cost. This
article provides guidance on acknowledging technical debt and helps you
to regain control.

**Acknowledging technical debt**

Today, many teams spend a lot of time and effort in optimizing processes
of their Application Lifecycle. We often look for ways to improve our
continuous integration, automated deployment, testing, architecture,
continuous delivery, agile practices and automated provisioning. Many of
these topics require technical skills or require organizational stamina
to result in change. Technical debt is just as important as the other
topics, but it rarely gets the attention it deserves. One of the
characteristics of technical debt is that it is spread throughout the
application lifecycle.

+-----------------------------------------------------------------------+
| \[\[inzet\]\] What is technical debt?                                 |
|                                                                       |
| Technical debt can be summarized as 'During the planning or execution |
| of a software project, decisions are made to defer necessary work'.   |
| Over time this infects the maintainability of the codebase and will   |
| require effort to keep resolving issues. Several forms of this debt   |
| can exist and are referred to as architectural debt, and design debt. |
| The practices and approaches described in this article are applicable |
| to most forms of technical debt.                                      |
+=======================================================================+
+-----------------------------------------------------------------------+

A shortcut built in to the codebase can be perfectly *OK* at the moment
of implementation, but we tend to overlook consequences for the future.
In practice, the less visible these choices are, the more they can hurt.
Nowadays, effective and well organized teams can ship software towards
production in a fast manner. However, effects of shortcuts can easily
result in costly production issues.

 

When teams *do* have code quality on their radar, they tend to focus on
code syntax only. But that only covers the easy parts. Substantial
rework or refactoring is often not accounted for, resulting in technical
debt being built in. Acknowledging technical debt is the first step,
just like admitting the addiction, but how do we convince our product
owner and stakeholders that we need to undertake additional efforts for
something that currently seems to do the job?

**Getting in control**

While acknowledging is the first step to improving, taking actual
measures is the next step. As a development team, we spend time refining
our backlog, m focusing mostly on getting the right amount of functional
detail into the user story. However, getting user stories right can be
challenging.

There are many ways of getting your stories better. There are many
techniques available for working on stories; one great example is
provided by Gojko Adzic[^1]. He has taken a hamburger as a model for
breaking down user stories. It helps you to identify tasks and possible
other options, providing you with an overview. It enables you to sort
these on complexity and effort, allowing you to take a conscious
decision on how to approach the story. Using techniques like these
allows you to approach stories in a way in which both technical and
non-technical people -- e.g. a product owner -- understand the mechanics
of the story.

As stated before, it is of key importance that we acknowledge technical
debt. When working out the details of a story you should be able to
focus on existing technical debt or places it may occur. A conscious
mind is required for this, danger can be just around the corner, for
example the lack of domain knowledge or just *the rush* to get things
*done*.

**Quantify technical debt**

Identifying and measuring technical debt can be a daunting task.
Fortunately, there are tools available to help you achieve this. One of
them is SonarQube[^2], an open source tool that helps you to identify
and measure technical debt. You can integrate it in your builds and get
it to automatically generate technical debt measures. While having this
at build time is perfect for many teams, some like to have this feedback
even sooner. Tools like SonarLint for Visual Studio[^3] can help
developers identify critical issues on-the-fly. Rule sets defined by the
team can be enforced immediately to prevent the occurrence of known
technical debt issues. Remember, facing the facts is a good thing!

Besides direct feedback during development, feedback based on telemetry
in your application is equally important. Insight into what parts are
actively used compared to less active parts helps determine the actual
need to fix certain technical debt items in these areas. It provides
developers as well as business and stakeholders with valuable insights
they may not have had previously. Prioritizing on what to do first can
now be based on actual data.

Technical debt will be in your application lifecycle. Acknowledging that
is important, fiercely trying to prevent any form will get you nowhere.
Budget is never an unlimited resource, which means choices need to be
made. Technical debt is something that needs to be worked on to prevent
it from increasing to unacceptable levels. Although at first sight this
usually does not add 'real value\' for the product owner or stakeholder,
the development teams are aware of its importance.

**Making it visible**

Having technical debt measured by tools allows you to add significance
to it. Making product owners and stakeholders aware of this significance
is of great value. To do so, teams need to make sure that their
technical issue is well understood and that the team should be able to
quantify or compare it to other (mostly functional) items on the product
backlog. It is considered good practice to make stories out of technical
debt items and provide these with an estimate. Being able to compare
stories in your backlog makes it easier to 'sell' it to the business.

The following diagram may help to make technical debt in a particular
area understandable and comparable. Every technical debt issue should be
plotted in this diagram.

![](./media/image1.png)


A debt story that holds little business value and requires little
technical challenge to resolve it (B), may sound like a quick win, but
may not be worth the effort. Technically challenging items with low
business value (A) may result in significant time spent on something
that has little results. Technically challenging and high business value
items (C) sound like a challenge but could easily be worth the effort to
the business. Some technical debt may occur in areas where fixing is
simple and business value is high (D).

**Measuring Effects**

Acting on technical debt is important and the metrics provided by tools
will allow you to verify whether your efforts are actually helping.
Retrospectives can be used to zoom in on how the technical debt is
evolving in your product. What kind of measures would be helpful?

 

A very important metric is your team's velocity. A stable velocity
implies a steady flow of business value delivered over time. Handling
technical debt in a sustainable way should improve the overall quality
of the product. However, spending time on debt is time that is not spent
on actual value. Therefore, velocity may drop a bit at first, but could
well increase over time while your application is better equipped to
stand the test of time. Teams should also be comparing their velocity to
business value delivered. Solely looking at velocity or value could be
deceiving. A little drop in velocity could very well deliver more value.

Measuring your technical debt and comparing this to your velocity and
your business value is crucial to your application lifecycle.

**Summary**

As a developer you want to write high-quality software with minimal
technical debt, resulting in high value being delivered. User stories
require enough attention from a technical and non-technical viewpoint.
Various techniques are available on getting it right. *Everyone*
involved in the application lifecycle should understand the importance
of technical debt management and should put his best effort into
minimizing risks and achieving great results! 

[^1]: http://xpir.it/mag3-techdept1

[^2]: <http://www.sonarqube.org/>

[^3]: <http://xpir.it/mag3-techdebt2>
