![](./media/image1.jpeg)


Reflecting on lessons learned using value streams in leading a DevOps
transition --- Martijn van der Sijde & Jasper Gilhuis

# A short history

In the past, technology did not have a dominant share in the products
and services of companies. IT was managed in a decentralized manner, and
was closely related to the business, directly supporting it. The world
was not changing every second and products and services were not
digital. Its role was to support the business, and this is why IT was
seen as a cost center. Over time, digitalization of products and
services slowly started to strengthen the role of IT, which resulted in
larger IT departments. At that time, IT was often organized around
software delivery value streams\* with all required roles and expertise
located on the same office floor. When you looked inside these software
delivery value stream organizations, they were organized horizontally
(frontend, backend, operations, etc.) and in that way they were siloed.
Because these silos are often reflected in the software architecture as
described in Conway's Law\*\*, technology was not yet optimized for
maximum flow. However, from an organizational perspective, the
department was aligned in order to foster a climate of working towards a
common goal. Technical dependencies caused by the aforementioned silos
were manageable and also aligned for future optimization. These
opportunities for optimization made Patrick Debois coin the term DevOps
in 2009. The goal of DevOps was to implement changes that would enable
the IT department to make steps towards optimizing the speed of
high-quality software delivery by bringing development and IT operations
closer together. Unfortunately, around the same time the idea of IT as a
cost center was being strengthened due to a worldwide economic
recession. This forced companies, banks and insurance companies to cut
costs. As a result, IT was centralized in shared service centers and
centers of excellence that focused on technical disciplines, or it was
outsourced. Centralization of IT allowed these companies to optimize the
utilization of their resources, but later on it became apparent that
this is not optimal when you want to increase the speed of high-quality
software delivery.

[\<\*\* Call out/sidenote]{.mark}\>

+-----------------------------------------------------------------------+
| **Conway's Law**                                                      |
|                                                                       |
| *\"organizations which design systems \... are constrained to produce |
| designs which are copies of the communication structures of these     |
| organizations.\"*                                                     |
|                                                                       |
| --- *M. Conway*                                                       |
|                                                                       |
| Architectures of systems will be designed and shaped in the way that  |
| the organization to deliver these systems is shaped. If you have      |
| technology value stream teams organized around a frontend application |
| or around backend services or other splits, the architecture will     |
| reflect this.                                                         |
|                                                                       |
| The idea of reversing this is to design the architecture you want,    |
| with low coupling and high cohesion and form your technology value    |
| streams around that, enabling you to maximize the flow of value.      |
+=======================================================================+
+-----------------------------------------------------------------------+

# Back to the future

Nowadays we are living in a world that is changing every second. Because
of digitalization everyone and everything is connected with each other.
Marc Andreessen's article on "Software is eating the world"[^1] is being
confirmed by changes in industries every day. Products and services are
largely digital. The idea that "*IT is the business*" is making its way
into the board room. This new mantra is giving IT a leading strategic
role in the organization. If you doubt whether this is true, try the
following: ask a banking executive what would happen if he would send
all his bankers home. Then ask him or her to imagine how long it would
take before the bank would be out of business. After that, ask how long
this would take if all IT-operations personnel, the people who prevent
or solve system failures, would be sent home. Or how long this would
take if all software developers were not there, causing a halt to all
changes in software required for selling products or for complying with
regulations.

In this rapidly changing world, the business and thus IT needs to be
able to respond quickly to external developments and threats in order to
achieve their business goals. Competitors who are able to add new
features more quickly and deliver these to customers faster will
outsmart the competition. To be able to keep up, it requires companies
to be able to innovate and adapt rapidly, whilst at the same time
maintaining high quality and security. The good thing is that the
characteristics of software and cloud-facilitated services and
infrastructure, on which digital products and services are based, enable
this when applied correctly. This is why IT is turning into a profit
center instead of a cost center. To be able to maximize the benefits of
IT as a strategic innovation driver, the focus needs to be on delivering
value, using software delivery value streams, while optimizing its speed
and quality. Applying DevOps principles and practices help to realize
this.

To help kick-start this optimization, we must identify value stream(s)
and choose one to start with. In a way we are reverse engineering to
where we came from before the centralization into centers of excellence
took place. But in the case of reversing Conway's Law, the organization
and technology value stream teams will have to follow the technology,
instead of the technology following the way IT departments are
organized. This is because our point of departure is the technology and
the teams around it, while focusing on maximum flow of value. In other
words: back to the future of the value stream.

[\<\* Call out Value Stream\>]{.mark}

![](./media/image2.png)

# How to carefully select a value stream to start with

With DevOps we concentrate on the software development value streams and
the products and services they deliver. As stated in Gene Kim's DevOps
Handbook[^2] you should start by carefully selecting your first value
stream. This is key because this selection determines the difficulty of
the transformation, both technically and culturally. This is important
from a change management perspective, because you need to increase your
chances on a first success to be able to scale your transformation
throughout the organization. In addition, the people leading this DevOps
initiative must also be given the opportunity to experiment and learn to
do this transformation. The following steps can guide you in selecting
the right value stream:

1.  First identify domains that deliver products and services. A way to
    do this is by looking for logical clusters of applications.

2.  Evaluate these domains on a set of criteria. These criteria cover
    the areas of people, process and technology. Examples are:
    importance of time-to-market and ability to innovate, willingness
    for change, opportunity for quick wins. Creating a weighted score
    based on these criteria helps you choose and select a domain and
    value stream you want to focus on.

3.  Identify the software delivery value streams (and their mutual
    dependencies) delivering products and services in that domain.
    Design a logical grouping of the capabilities these value streams
    provide. The technology should be functionally cohesive and not
    logically cohesive in order to reduce dependencies as much as
    possible, providing opportunities for maximizing flow.

4.  Staff the team(s) for the value streams of the software delivery.
    You can use the DASA Competence Framework[^3] for guidance on the
    required hard- and soft skills.

# Setting goals and measuring the effect of your improvements

In short the reasons for adopting DevOps are: being able to produce
better products and services, delivered in a faster, and if possible,
cheaper manner. Better products and services can be detailed further
stating that the products and services need to be of high quality and
can be easily adapted to changes in requirements and opportunities for
innovation.

However, when do your improvements contribute to these goals? It is
known that only one out of three experiments will truly be successful
for your value stream. You need to know whether a change you have made
was an improvement or not. That is why metrics are a hot topic when
applying DevOps principles and practices, and rightfully so.

![](./media/image3.png)
So far so good. Every well-known DevOps
resource points you in that direction. Then the next question is: what
metrics to start with and how to measure them. Based on DORA
whitepapers[^4] we recommend that you start collecting data for four
metrics right away that tell you something about delivery speed and
product and service quality (see Figure 1 Aspects of software delivery
performance (-- source: State of DevOps report 2018).

The count of releases to production, the changes the release contains
and the number of reported incidents are metrics that are often already
registered or available in a change and incident

management system in a non-DevOps world. Or these metrics are part of
your team's backlog in an Agile planning and CI/CD system like Azure
DevOps\*\*\*. The time of incident creation and the time of incident
closure following a release can be registered, and this allows you to
calculate the time to restore. The same is true for the number of
releases to a specific environment. For the change failure rate, the
relation between these two data entities, a release and an incident,
needs to be made. A quick and easy way to do this is to look at the
creation dates of both, and relate them to each other in that way. This
leaves us with the lead time for changes. For automatically calculating
this, the basic CI/CD pipeline needs to be implemented (as mentioned
earlier) to be able to collect this data easily. The work item
administration will be the main source for the required data.

[\<\*\*\* Call out Azure DevOps]{.mark}\>

![](./media/image4.png)

Having these metrics in-place before the transformation allows you to
forecast what benefits your DevOps initiative can bring. It also allows
you to periodically measure whether your improvement experiments are
contributing to this. From here you can add additional metrics as you
see fit. You get the most value out of these metrics when they are
measured over the integrated work across all teams on a product. It is
important to note that these metrics do not measure individual or team
performance, but are value stream-related.

# What's next? Value stream mapping and continuous improvement

When we have our first technology value stream and have put the
foundation for metrics in-place, it is time to adopt a continuous
improvement process as a vehicle for making improvements. Or enhance
this process if it is already there. The basis for this is creating a
value stream map for the development value stream.

![](./media/image5.png)


Figure 3 Continuous improvement cycle

The value stream map will show the roles of people who take part in the
software delivery value stream from requirement until monitoring a
product in production. It shows which tasks they execute to make this
possible. The process to depict this will already be of help, because it
allows us insight into what is required to get software into production.
The next thing they will learn is where any inefficiencies occur in
their work. In other words, it will expose bottlenecks that prevent a
reduction in time-to-market and an increase in delivering quality. This
gives them input for experimenting with improvements that reduce these
bottlenecks. The metrics data will clarify whether an improvement had
the impact that was expected. Basic metrics need to be available from
the start as explained, but based on a specific improvement, these
metrics can be extended as required, as part of the continuous cycle.

# Three take-aways

Of course, the steps explained in this article are just a few highlights
of what is necessary to get a DevOps transformation up and running. For
example, we did not discuss giving people a DevOps foundation through
training, or the steps required to change management behavior. The focus
was on the elements that are related to the value stream. The three
things you should take away after reading this article are as follows:

1.  To determine where to start your DevOps transformation process,
    create a matrix of domains that you have identified. In this matrix
    you score each of the criteria that you think are important to
    arrive at a value stream selection. These criteria should take
    people, process and technology into account. It will tell you
    something about the relative amount of resistance to change that you
    can expect in the value stream teams, and how this value stream will
    benefit from technology optimization. It will also show the
    importance of time-to-market and innovation for the business of this
    value stream. The weighted score of this matrix will lead you to
    your first value stream and will enable you to start your DevOps
    transformation initiative.

2.  Before you start making improvements with your value stream, make
    sure you can report the following four metrics:

    1.  Deployment frequency

    2.  Lead time for changes

    3.  Time to restore service

    4.  Change failure rate

This will allow you to continually track progress towards the goals you
have set. It is important to be able to measure your own success, but it
is also important to be able to show your success to higher-level
management. So, start by making an analysis of the raw data currently
available in your release and ITIL management systems. If this data is
insufficient, then make improvements before you start optimizing your
value stream with DevOps principles and practices.

3.  The value stream map is the heartbeat of your continuous improvement
    process. Adopting a continuous improvement process with teams allows
    them to make any change required in a structured and measured way.
    The first step is to create a value stream map of your software
    delivery value stream. The bottlenecks that are identified can be
    prioritized based on the aspect that has the most added value for
    achieving particular business goals. You should update your value
    stream map on a regular basis, for example every three months.

[^1]: Marc Andreessen, <https://on.wsj.com/2oWxsdy>

[^2]: DevOps Handbook <http://bit.ly/2oYTgVT>

[^3]: DASA Competence Framework <http://bit.ly/2NDtrJj>

[^4]: Accelerate: State of DevOps 2018 <http://bit.ly/2oW0sSN>

    Forecasting the value of DevOps Transformations
    <http://bit.ly/2Qk9YLZ>
