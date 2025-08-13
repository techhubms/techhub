When we coin the term observability people often think "ah, you are
hype-wording what we already do, but we call it monitoring". In the DORA
2018 State of DevOps report[^1] this is being reported as respondents
not seeing a difference between Monitoring and Observability. That is
unfortunate, and we believe there are big differences. Let us start with
comparing the definitions.

***MONITORING***[^2] *is tooling or a technical solution that allows
teams to watch and understand the state of their systems. Monitoring is
based on gathering a predefined set of metrics or logs.*

***OBSERVABILITY***[^3] *is a measure of how well internal states of a
system can be inferred from knowledge of its external outputs".*

Looking at both definitions there is a fundamental difference between
them. Freely stated, that difference comes down to this. Monitoring and
dashboards are often used to get alerts in issues that have already
occurred in the past. Think of a disk-full message. It is great to get a
message but the issue itself is not resolved. We can categorize this as
"Known-Unknowns".

When we think of Observability, we should think of complex software
systems that sometimes fall over. Think of a distributed calculation
across multiple services that may provide a result. But is the perceived
result a result that is a combination of all required calculations? We
can classify this as "Unknown-Unknowns". To be able to answer these
kinds of questions, our complex systems need to gather information that
is queryable so we can determine whether calculations are fine.
Gathering insights in this way is very valuable.

If we are in the middle of a crisis, it is likely that multiple issues
are happening at the same time. The root causes of a failure often
consist of multiple chained events. Having extensive telemetry available
about your application(s) and its architectural landscape allows you to
define metrics that will help you determine the issues at hand. We need
smarter decision making.

## 

## Why aren't our current monitoring solutions sufficient?

The way we build software and infrastructure is changing rapidly. Trends
like Cloud, Containers, Microservice architectures, and serverless
(Functions as a Service) are fundamentally changing the way we build and
operate applications. Almost all modern applications are far more
distributed than their predecessors, and this requires a different
approach from an operations perspective.

Traditional monitoring solutions are built for an era in which
applications were less distributed and fewer things could go wrong
inside the application itself. In the current distributed era, in which
components call each other over the network instead of residing on the
same machine, chances of failures are a lot higher. This is a paradigm
shift we must consider when building our application by adding retry
mechanisms and other forms of resilience to our applications. A failing
service call does not mean our application doesn't work anymore. It is
something that we know will happen because of the way we shape our
application.

Because of this we must make a shift from measuring technical failures
only to start measuring the impact these failures have on our end-users.
It does not mean that any of these technical measurements are useless,
they still provide valuable information, but we should not create alerts
or dashboards based on these measurements.

## Observability vs Monitoring

Traditional monitoring is often done by creating several dashboards that
show the current health of an application. Experience tells us that this
is no longer sufficient. Have you ever had a customer calling you that
something did not work, and when you took a look at your dashboards,
everything was still green?

Observability takes a different approach compared to monitoring.
Observability is about instrumenting your code to be able to inspect
what goes wrong without having to change the application itself. The
term Observability comes from control theory. The definition fits modern
software development methodologies in which DevOps teams build and run
the applications instead of having a team that is dedicated to running
and monitoring applications. Because a single team is responsible, they
have far more insight in what an application does internally and what
could go wrong. They are also motivated to add instrumentation because
they will be the team that gets called out of bed when something goes
wrong.

## The three pillars of observability

Observability is gaining a lot of traction lately and there are many
vendors and open source projects jumping into the gap of building
solutions to help with observability. In this article we do not want to
focus on specific tools because we believe there is no single best
solution for all companies, teams and application architectures.

What almost all these tools have in common is that they define
observability based on three pillars: logging, metrics and distributed
tracing. Depending on whether the tool can cover all of these areas or
only one or two of them, the vendor will draw different diagrams of how
these pillars relate. There are tools that cover all three areas, and
that will tell you all pillars are closely related. And there are tools
that focus on only metrics. Combining logging and distributed tracing
will tell you these pillars are so different that you should approach
them separately with specific tools. As always, the truth is: it
depends. There are several factors that influence this: the complexity
of your application and your organization, or the number of messages or
daily users of your application.

![](./media/image1.png)


Figure Pillars of observability

Each of the pillars has a specific usage and the combination of them
gives you the observability to be able to query applications when things
go south.

### Logging

Logging is something almost every developer is familiar with. Logs
capture events happening in your application and store them so you can
query them to get insights. A downside of plain logs is that they are
really hard to­ search, especially in a distributed system in which a
complete log of what happened to a user is divided over multiple
services. A solution is to use a centralized logging solution. Popular
choices when building an application on Azure are the ELK stack or
Application insights.

In addition to centralized logging, you will also need a way to store
your logs in such way that you can actually search your logs easier than
searching through plain text files. Structured logging is a way to turn
your plain log files into queryable log files. Serilog and log4net are
two libraries that can help you create structured logs in the .Net
ecosystem, and there are libraries available for most development
languages. Most centralized logging systems are made to store and query
structured logging and support logfiles created by these libraries.

As you can see in the sample, the method used in structured logging is
not that much harder than normal logging. However, the benefit is that
the User and Duration object are now queryable in central logging tools,
which means that it is a lot easier to see all logs for a certain user
for example.

Most of the infrastructure components also have built in logging like
web servers, databases, firewalls etc. It can be valuable to also send
these logs to your centralized logging system to get a complete picture
of what happened in your application.

The main downside of logging is that it can become expensive quickly.
It's easy to add logs to your application but what happens when you get
thousands or millions of users a day. How long do you store logs, and do
you store all of them? Again, this question can only be answered with
"it depends".

First of all, we have to make a distinction between operational logging
and application logging. Operational logging is logging that is used to
track whether an application is working as intended while application
logging can be used to see what the application did functionally.
Application logging provides data that you always want to store for
longer periods of time and you want to store all of it. It does not even
have to be in your log system. It could be stored together with the
business data itself, such as audit trails or logs of all the statuses
an order had.

Don't mix these types of logging with operational logs which help you
track down bugs or problems in your application. People tend to store
these operational logs for long periods of time as well, but how useful
is that? How much information do they provide when you regularly deploy
new versions that might have completely different code paths? In
general, these logs should not be stored for long periods.

However, even if you store logs for about a month or less, this could be
quite expensive to store if you have large amounts of users in a complex
application. Because of that, most logging systems have an option to
sample the data. This means that only a percentage of the actual logs is
stored and the rest is thrown away. This has some downsides to it
because you cannot trace back all the issues you had because you do not
have all the logs. On the other hand, most logging tools have the option
to do dynamic sampling which does not randomly remove logs, but
especially keeps all the special events that behave differently from all
other operations.

### Metrics

Metrics are a way to store aggregated measurements as time series data.
This has a large advantage as it is a lot cheaper to store the data,
because you aggregate the events together per interval. Also, the growth
of the storage is constant because it writes records at fixed intervals.

The way we use metrics is different from logs because we lose some
specifics when storing metrics instead of logging all the events
separately. For example, you could store the average request duration in
a metric and you will only have the average number. When you log the
duration of each request you can get more accurate numbers like
maximums, minimums and calculate the average as well. Does this mean you
should log instead of using metrics? No! Both have their strengths and
they should be used in combination. Pick the right solution for the
information you want to store.

Averages are often named as examples when talking about metrics.
However, averages can hide important indicators when used in large
amounts of data. If you track the average request duration and 90% of
the requests are taking 0.01 second and 10% takes 2 seconds the average
request duration is 0.2 seconds. When you only see a 0.2 seconds average
you might think everything is quite okay but those users belonging to
the 10% that take 2 seconds per call won't agree with you. Instead of
averages you could therefore also look into percentiles.

### Distributed Tracing

The last pillar of observability is distributed tracing. Distributed
tracing can give you insights in how the flow of your application was.
So, whereas logging or metrics could help you measure how long certain
requests took, distributed tracing can help you investigate WHY a
request needed that response time and which components were used in the
flow of this request. This kind of information is useful especially in
distributed systems.

Distributed tracing tools provide insights that might look quite
familiar when you use the network tab in developer toolbars, that is
included in most browsers.

![Trace showing spans](./media/image2.png)


Figure 2 Example of an open source tracing tool (Jaeger)

Since tracing can span multiple systems, components or services, it's
important that it is possible to write tracing information. Because of
this the Open Tracing project was started to create a vendor neutral API
for distributed tracing. OpenCensus is another project aiming for the
same thing and the good thing is that these two projects are merging
together (opentracing.io & opencensus.io). Asp.Net Core also supports
this library from .Net Core 3 onwards out of the box, so services
created with this will automatically work together with tools like
Jaeger or Zipkin.

## What to measure

So now that we know how to measure things using the three pillars of
observability, what are the things we should measure? An easy way to
remember this is to "USE RED". This is an abbreviation that stands for:
USE (Utilization, Saturation & Error Rate) which we measure at a
resource scope, and RED (Rate, Errors & Duration) which we measure on a
request-based scope.

#### Resource scope 

Measuring things at a resource scope can be valuable to track down
performance problems in a system. What do Utilization, Saturation and
Error Rate mean?

-   Utilization: How much time was this resource actually busy
    responding to requests?

-   Saturation: Is the resource able to handle all requests or is work
    waiting to be picked up / queued?

-   Error Rate: How many errors does this resource produce?

#### Request-based scope

Measuring on a request-based scope can give you insight in how certain
functions are performing. We do this using Rate, Errors & Duration

-   Rate: Number of requests per second.

-   Errors: Request error rate, what is the percentage of failing
    requests?

-   Duration: Response time, Latency. How long did it take to handle the
    requests?

## How observability can change the way you build systems

A very well-known picture in the industry is the infinite DevOps loop.
Many occurrences and spin-offs exist. In most of them the term
monitoring is present. Now that we understand the differences and gaps,
we believe the term monitoring in that loop should also imply
implementing observability patterns and practices.

Changes are often made to systems without clearly knowing whether the
result will be better. Or if there are no unintended side effects. You
want to make decisions that are proven to be working by observability.
An often-heard answer is that it is too costly to change your
application. But there are some good patterns that you can apply that
will help you make the right decisions.

![https://miro.medium.com/max/700/1\*LFnVVlLrgrvXQ5ScrHVKjg.jpeg](./media/image3.jpeg)


Figure 3 Testing in Production

The above picture[^4] shows us a very good overview of patterns and
practices that can help you implement an observable system. Building an
observable system starts at the beginning of the DevOps loop.

There are numerous types of testing that are aimed at the coding and
testing phase, for instance unit tests, static code analysis, mutation
tests, UI/UX tests and so on. They can be applied to the application in
order to gain confidence in whether is being built is the right thing!
If your system is lacking these practices, it becomes nearly impossible
to obtain confidence in releases of that software. Just having these
kinds of test is not enough.

During the deployment and releasing phase there are many practices that
can be applied. Applying Canary releases and using Feature flags to
safely release your software without impacting users allow you to
release more confidently and use traffic routing to get a percentage of
your users hit the new system after switching a feature toggle.
Monitoring and observability then become key to be able to determine if
your system is in a correct state.

For feedback (i.e. bugs, issues, new features) to transition smoothly
there is a need to apply Site Reliability Engineering (SRE) practices to
the DevOps loop. Which means that your engineers (Developers, Operations
and your on-call support people and all others) should be aligned on how
to deal with that feedback.

## Conclusion

Many aspects of observability are of a technical nature, so making it
visible for the organization and your customers is a challenge. We
should apply all these practices and come up with tools and
visualizations that show that we are in control of the system and will
be able to meet our predefined goals. Many of the professionals and
subject matter experts use best of breed tooling to fit their needs.
This also implies that there is no single tool to rule them all. In a
true DevOps nature we do not want to enforce these choices but we need
consolidation on it. Ultimately it is all about what the end-user
experiences.

[^1]: DORA 2018 State of DevOps report
    <https://devops-research.com/2018/08/announcing-accelerate-state-of-devops-2018/>

[^2]: Definition of Monitoring taken from the DORA Report

[^3]: Definition of Observability from Control Theory
    (<https://en.wikipedia.org/wiki/Observability>)

[^4]: Image from blogpost by Cindy Sridharan, Testing in Production
    <https://medium.com/@copyconstruct/testing-in-production-the-safe-way-18ca102d0ef1>
