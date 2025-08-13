# Continuous Delivery 3.0: The next "next step"

*René van Osnabrugge*

"Continuous Delivery is the logical evolution of Agile[^1]" - this
statement was written by Kurt Bittner in 2013 in a report of Forrester.
Back in those days Continuous Delivery was not yet as far in the hype
cycle as today. In the Application Development hype cycle that Gartner
presented in 2015[^2], Continuous Delivery was on the rise and nowadays
it is in every year plan of any IT-related company.

But what exactly is Continuous Delivery, and what does Continuous
Delivery 3.0 add to this concept and what does it have in common with
Digital Disruption. In this article you will find answers to these
questions and learn why it is important to look beyond the hype cycle.

**Continuous Delivery**

![Xpirit Release
Pipeline](./media/image1.png)


Continuous Delivery is the fulfillment of the Agile promise. With over
95% of companies practising Agile[^3], it is a logical and required step
for most companies to look further. Agile brought us the benefits of
being able to produce high-quality software produced in a matter of
weeks. But the next challenge, delivering the software and delivering
business value to the customers, proved to be a whole different ball
game.

This is where Continuous Delivery comes in. Continuous delivery is a
software engineering approach in which teams produce software in short
cycles, ensuring that the software can be released reliably at any time.
Its objective is to build, test, and release software faster and more
frequently.

Ideally this means that all functionality is planned, realized and
released with the same repeatable process, called a pipeline. Quality
gates (like unit tests, continuous builds, acceptance tests, and
metrics) ensure that the functionality and quality is guaranteed. If
something is not right, the pipeline is stopped so that the error can be
fixed. Therefore, metrics should be in place to understand, trace and
pinpoint the error to make sure this does not happen again. Every piece
of software, whether it is a bug fix, a new feature or a change request,
is treated in this same manner.

This sounds perfect for new projects and new software, but how does this
work within existing scenarios and within organizations that have
already been building software for many years? Before I dive in to this,
I want to talk a bit about digital disruption.

**Digital Disruption: rethinking existing scenarios**

Digital Disruption has a strong relation with the ***Law of the handicap
of a head start***[^4], which describes how making progress in a
particular area often creates circumstances in which stimuli are lacking
to strive for further progress. This results in the individual or group
that started out ahead eventually being overtaken by others. In the
terminology of the law of the handicap of the head start, what was
initially an advantage subsequently becomes a handicap.

In many (larger and older) organizations we see this happening all the
time. Products or services that were once very innovative, are being
optimized, expanded, tweaked and polished. All is well until the moment
a digital disruptor steps into the market who completely reinvents the
product or service, taking over market share very quickly and
aggressively. We all know the disruptors around us. Uber who runs a taxi
service without taxis, AirBnB who provides accommodation but owns no
real estate, and so on.[^5]

![Digital disruption has already happened: Do you want to be the LION or
a LAMB?](./media/image2.png)


The disruptors are usually newly founded companies, bearing no legacy
and doing things differently right from the start. Something that is
impossible for existing companies. Or not? Because when you think of it,
the main thing the disruptors do differently is choosing reinvention
instead of optimization.

**Do old constraints still apply?**

The question that we need to ask ourselves is: "Why are experienced,
rich and large companies not disrupting the market themselves?". While
there is no generic answer that applies everywhere, one of the things
that makes companies slow and rusty is a lack of self-reflection.
Processes and tools have been in place for many years. The ways of
working have been in place for many years and these are being optimized
over and over again. Instead, companies should ask themselves the
question: "Why do we do this?"

The older companies started when hardware was very expensive and
limited. Software was written in a matter of months or years. It was
very important to write software that was right the first time because
it took long to compile and deploy. Procedures were developed to ensure
this was the case. The hardware was so expensive that it needed to be
shared. Because of this, people needed to guard this hardware so that it
was used correctly. Procedures were created to deploy software to these
machines because this could only be done by certain people, and so on.

This example pinpoints procedures and roles that were created because of
limitations or risks in the past. They are still in place because
***this is how we work.*** And in many cases that is the only reason
left when you look at it closely. Hardware is not expensive anymore, we
can write and compile software in minutes. Therefore, this needs a
different approach. And this goes for a lot of things, including
Continuous Delivery.

**Continuous Delivery 3.0**

Now that Continuous Delivery is on top of the hype cycle, it is also on
top of the minds of many organizations. The assignment to the
development teams (or now often called DevOps teams) is simple: move
towards Continuous Delivery. This is exactly what most people do: start
optimizing the process of delivery.

But now that we have learned that reflection on the reason why and going
beyond optimization of your current tools and processes is important, we
can apply these same concepts to the delivery process and move beyond
the mere optimization and automation of the current process.

In order to make a real difference, to really gain speed and quality in
the delivery process, do not optimize what you have, but rethink what
you do and think about the next "next step". In most cases, optimization
and automation alone is not enough. The process needs to be thoroughly
looked at and rethought. This is what Continuous Delivery 3.0 is all
about.

***Do not optimize what you have, but rethink what you do and think
about the next "next step"!***

Let me illustrate this with a few examples.

**Machines, Virtual Machines, IaaS, PaaS, Containers**

10-15 years ago all software was running on physical hardware, which was
expensive and hard to scale. This completely changes with virtualization
technology. Machines could be created in a matter of minutes, and the
scalability issue was solved.

Many organizations currently think about the next step. To deliver new
functionality in a continuous way they even need more flexibility than
Virtual Machines running on physical hardware. Of course the cloud
offers this flexibility of unlimited space and unlimited machines, but
is moving your Virtual Machines to the cloud as IaaS (Infrastructure as
a Service) the right thing to do or is this an optimization?

In the Continuous Delivery 3.0 mindset, moving towards IaaS is perhaps
not the right thing to do. When you rethink the possibilities, and move
towards PaaS (Platform as a Service) and do not have to maintain
machines anymore, moving your application into containers[^6] [^7](for
example Docker) might be the next "next step", and therefore a better
choice.

**Manual Tests, GUI Tests, Test Automation**

One of the most important things within a Continuous Delivery pipeline
is the execution of automated tests. Writing automated tests is hard and
although a lot of companies invest heavily in the automation of tests,
testing is still a very manual job. When moving towards Continuous
Delivery, tests need to be automated to ensure quality over and over
again. Because manual testing has been done for many years, the logical
optimization is to automate these manual tests. In most cases this means
GUI testing. But again, this is not what brings you further.

The next "next step" goes beyond GUI testing. Test automation done right
should be the area of investment. As the Agile Test Pyramid describes, a
large base of fast and small technology-driven unit tests and only a
handful of End-to-End GUI Tests allow you to run your tests quickly and
reliably, and often this is the better approach.

**Rethink your processes**

These two examples illustrate how rethinking your process can be of
great benefit. Looking at the Continuous Delivery Pipeline, many more
examples can be thought of. For instance,

-   Feature Toggles instead of branches to disconnect your deployment
    from a release. Breaking down your monolith into smaller independent
    components to be able to have different release cycles for different
    components.

-   Building metrics and usage insights into your application to
    proactively act on user behavior instead of alerts and event to
    react on it.

These are all examples of how you can think ahead of the hype cycle.

**Summary**

Continuous Delivery 3.0 is all about thinking ahead. Instead of
optimizing the things you already do, start thinking about how you can
do it better or differently. Just like the digital disruptors disrupt
well-known concepts and companies by being different and acting
differently.

The best way to start within you own company is to ask the question:
"Why do we do this and do the old constraints still apply?"

When you start thinking about the next "next step", new and valuable
possibilities will open up, in addition to transforming your Continuous
Delivery process to the next level.

[^1]: Forrester: Continuous Delivery Is Reshaping The Future Of ALM,
    Kurt Bittner, July 22, 2013

[^2]: <http://xpir.it/mag3-cd30-1>

[^3]: <http://xpir.it/mag3-cd30-2>

[^4]: <http://xpir.it/mag3-cd30-3>

[^5]: <http://xpir.it/mag3-cd30-4>

[^6]: <http://xpir.it/mag3-cd30-5>

[^7]: <http://xpir.it/mag3-cd30-6>
