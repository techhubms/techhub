*"DevOps is a way of working that is becoming more and more popular with
software development teams to increase their agility and to be able to
build better software faster."* The problem in this description is the
focus on "development teams". DevOps isn't about development or
operations teams, but it's a way to deliver better customer value in any
way possible. Development and operations are traditional teams that are
at the center of this process and this is why most organizations only
focus on this area of DevOps. The only way to really succeed in DevOps
is by creating teams that can take full ownership of the customer
journeys they build and operate.

DevOps requires teams that have an entrepreneurial spirit and consist of
engineers who operate with courage, and who are willing to learn and
improve themselves. This mindset is needed to build and deliver the best
customer experience possible.

![](./media/image1.tiff)


*Figure 1: Amoeba-like structure of startups without hierarchy vs
traditional hierarchical organization*

Silicon Valley startups like Facebook, Uber & Spotify made the DevOps
movement popular. What is the big difference between them and your
organization? These digital native companies have embraced DevOps from
the get-go and have an organizational structure that matches this
mindset. Conway's law states: "*Any organization that designs a system
will produce a design whose structure is a copy of the organization\'s
communication structure.*" And since most organizations weren't created
with DevOps in mind, there is no real fit with DevOps.

A common problem that these startups have solved, is that they don't see
IT as a cost center but as part of the value chain delivering customer
value. This is the core of DevOps: delivering customer value.

Some people only see DevOps as breaking down the "wall of confusion"
between developers and operations. As long as these departments or teams
are driven by different goals they will never work in harmony.

![](./media/image2.tiff)


*Figure: cost driven IT organizations*

We've seen and helped many organizations taking the journey to a DevOps
culture and regularly see organizations experiment (which is a good
thing) with implementing DevOps. There are a couple of anti-patterns we
often come across:

![](./media/image3.tiff)


These three team set-ups often start as a good initiative when people in
an organization want to increase speed to be able to cater better to the
rapidly changing world around them.

When DevOps is an initiative that is only supported by dev teams (A),
they're stating that they don't need operations, and this can be fine
for really small applications. This often becomes more difficult when
applications have to integrate into a larger application landscape, or
have demanding operational requirements.

The opposite is DevOps only being part of Operations (B). This often
happens when organizations try to modernize their Operations department
and try to adopt DevOps. This approach will probably help in automating
some processes, but it lacks integration with Development teams to be
able to build better software faster.

One of the most common patterns we've seen in organizations is where a
new DevOps team is created between Dev & Ops. This might initially be a
logical choice that closes the gap between the teams, but this often
strands with the DevOps team becoming the new Ops. If more development
teams join the movement, this team gets overloaded with operational
tasks and in the end, there is no difference between options B & C.
However, this approach can work successfully when this team is a
temporary or a virtual team.

These antipatterns have one thing in common. They acknowledge the gap
between Development and Operations and try to fix it with tools or an
extra team.

## Common pains of failing DevOps implementations

A lot of organizations are struggling to get their daily work done.
They're focusing on adding more and more features, but they forget to
look at the bigger picture: the processes in place that dictate a
rigorous methodology, instead of a lean one.

Think about your engineers telling you about not being able to touch a
certain server, or not being able to measure the speed or usage of
features. Or when they rely on the work of another team and they have to
wait for that work to be finished before they can continue with their
part of the change.

### Problems caused by software architecture

A pattern we often see is that all layers of a solution are separated
into services with communication between them, in an attempt to increase
the decoupling of the individual services. In reality, these services
still cannot be updated independently, because the contracts between
them are so rigorous that a service cannot talk to another service with
an older version.

Following from that, we still see organizations horizontally dividing
teams according to the architecture of their software: the front-end,
back-end, and services are separated into different teams, with
hand-offs to another team for each layer. This means that multiple teams
are involved for every change in the communication between these layers.
These types of ceremonial steps take too much time with too many people.
Stop with all this layering!

Changing the architecture from this model can be quite challenging. Give
a team control over every aspect of the parts of the architecture
they're responsible for. For instance, if they need to add a field to
the database, create an easy way for them to do so, without an entire
committee to approve those changes. If you can update that database more
easily, you have removed an obstacle for the team. Update the parts of
the process which check that the database is in a good state while
deploying to the next environment, and you will experience increasing
confidence during the release process.

### Problems caused by Infrastructure

The divide between developers and operational engineers still is a big
issue for some customers. Developers do not feel responsible for the
software that's running in production, have no insight into telemetry,
and point their fingers at operations for almost all issues. At the same
time, operational engineers are not asked to attend meetings when new
features are being worked out. Teams structured like this cannot change
the infrastructure easily, and have to ask someone else to make these
changes. When development starts to iterate faster and deploy in smaller
increments, infrastructure becomes a common hurdle. If a dev team has to
ask the ops team to provision a server and has to wait more than a week,
they're stalling the first team. Organizations find the move to the
cloud hard to facilitate because people who need a change to network
infrastructure often aren't empowered to make that change themselves,
resulting in slow-moving development teams that have trouble delivering
value to the end-user. Break down the barriers to infrastructural
changes, and move to infrastructure as code to get a better
understanding of the necessary elements in your architecture. That way,
changes are better traceable, will have less impact, and can be reverted
more easily.

### Culture & people problems

Organizations often fail to change the culture, because they don't
change the culture through all teams, but just a subset. It is very
important to let go of individual roles like 'developer' and 'tester',
and change the mindset to being a team of engineers. Of course, some of
them have a strong skillset associated with their old roles, but each
engineer in a team should understand the entire solution that their team
is responsible for, and should be able to pick up a multitude of tasks
within their team. That way, you are less reliable on that one employee
who always fixes firewall issues, or that deployment that got stuck
between two environments (and of course she isn't available right now!).

### Problems caused by the way of working

A common error we encounter frequently is that there is no clear
definition of 'work' for a team. Or when there is some understanding of
what constitutes work, there is no attention to the definition of
'done': when do we agree that the work has been completed? Is that when
a feature has been developed, tested by the developers and tossed over
to operations who have to implement the new version in the next
environment in the DTAP flow? Is making a change to an existing feature
'work'? Is running around deploying hotfixes 'work'? Defining a clear
definition of done can really change the attitude and the way people
work. A good way to clarify all the work that's being done, is to make
sure that all types of work are accounted for: business projects,
internal projects, changes and unplanned work[^1]. Especially that last
category is forgotten most of the time, while it is often the culprit of
not achieving a goal.

## Make DevOps work in your organization. 

Organizations that are leading the digital revolution are the
organizations that **really** embrace the true DevOps mindset. Customer
value driven teams that can work autonomously and that are all aligned
in their purpose: delivering better customer value.

![](./media/image4.tiff)


*Figure: Value-driven organizations*

So how do we move here from existing organizations? To introduce this
alignment you have to make changes to the hierarchical structure of your
organization and create an environment for creative, experimental,
entrepreneurial teams. This can be created in existing organizations by
cutting loose several (development and operations) teams from the
existing hierarchy and letting them work without the existing boundaries
and rules. These teams can be connected to the hierarchy where they
actually do benefit from the hierarchy, such as HR or finance.

![](./media/image5.tiff)


*Figure: Amoeba-like structure connected to traditional hierarchy*

We see a lot of organizations that only think about the financial costs
of their IT organization. Every possible solution has to be fully
specified, including the cost implications (both time and monetary) that
come with it. These organizations can't change course in an efficient
manner and experience a lot of pain testing new technologies. This also
results in employees that aren't free to experiment with new
technologies, which leaves them feeling disempowered. Whereas that
freedom can provide faster iterations, improve quality and even bring
lower costs for those teams, if done in the right way.

Start talking with teams about the value they can add instead of the
cost they incur to the organization. Then empower them to make the
necessary changes happen and give them responsibility for it! Of course:
this doesn't mean they need to be let loose and to run through walls,
but they do need to be more in control of all aspects of their work.

Give teams the freedom to spin up servers in a controlled and secured
network topology, include an engineer with specialized network security
skills in the team, and talk about the added value of doing so: can they
move faster? Can they test better and in a more controlled manner? Does
it add valuable insights to code quality? Does it help in improving site
reliability? Does it add better security and understanding of the
necessary boundaries? Many DevOps organizations formalize the creation
of servers and other moving parts of an environment by creating a
platform team: they provide a self-service solution in which teams can
initiate the requests for new servers, databases, etcetera. This is a
great way of empowering the teams to perform these tasks themselves.

## Factors of a DevOps organization that works well

One of the most important things to keep in mind is that there is
harmony between the four factors of building software, i.e. the software
architecture, the infrastructure, the people and the team that builds
the software and the way in which work is distributed to them.

![](./media/image6.tiff)


A commonly made mistake is changing only one or two of these areas in an
organization and then running into problems at a later stage because the
other areas are bound to become obstacles.

Changing everything at the same time seems like a really large change
and it is! But it enables teams to be really entrepreneurial and get the
best work done. Start by finding your biggest bottlenecks and then find
ways to remove them. The software architecture has to be set up to make
sure that it is not coupling systems in ways that block teams from
changing and releasing their part. Event-based microservices and Domain
Driven Design (DDD) are example patterns that support your architecture
to allow teams to create boundaries between areas of software for which
a team is responsible. It is possible to carve out small parts of
existing legacy systems and decouple them through messaging patterns and
patterns like the "Anti-Corruption Layer"[^2]. The best way to do this
is to start with by creating a small, single vertical slice that affects
all layers of the landscape, and prove that this architecture will work.
After that, you can cut an existing monolith into smaller chunks.

If all teams are using a shared infrastructure that they can't control
themselves or if they rely on others to do things like deployments or
adding databases to the landscape, this is likely to cause friction and
delays. Especially if the other team is a cost-driven team focused on
reducing costs. Public clouds such as Azure or AWS allow teams to set up
and maintain their own infrastructure based on Platform as a Service
(PaaS) features, making it easier to (re)create it through
Infrastructure as Code. When you can spin up an environment in minutes
instead of weeks, you can move a lot faster.

The way work is being distributed has to be aligned with how the teams
are set up. It should give the team enough freedom to come up with the
best solution for a functional problem instead of detailed instructions
created by an architect in an ivory tower far away from the team. The
focus should also be on the total cost of ownership instead of direct
customer value only. Short-term focus on customer value often increases
technical debt because shortcuts are used to be able to meet stakeholder
demands.

All these factors require teams of people who are willing to take risks
and have an entrepreneurial mindset. Without these people, DevOps is
almost guaranteed to fail. This mindset is only present if the company
culture will allow it. Martijn and René have written another article on
the DevOps mindset called: "Growing your DevOps mindset" that can also
be found in this magazine.

## Conclusion

Introducing DevOps isn't easy, especially in large organizations with a
large existing IT landscape. If you want to succeed in making the
transition to DevOps, make sure you keep the four dimensions in mind:
People, Work, Architecture & Infrastructure.

Remember to start with small experiments and choose a process that
affects more than one existing team: transform the current layers in
your horizontal organization into more flexible, vertically situated
teams that are cross-functional and that can deliver end to end business
value.

Experiment, fail, start over. That should be the mindset in a DevOps
organization. If you focus on delivering value together and empower your
people to have this DevOps mindset, they will achieve great things.

[^1]:
    > The Phoenix project -- Gene Kim, Keven Behr & George Spafford.

[^2]: Anti-Corruption Layer. A Design pattern from the book Domain
    Driven Design -- Eric Evans
