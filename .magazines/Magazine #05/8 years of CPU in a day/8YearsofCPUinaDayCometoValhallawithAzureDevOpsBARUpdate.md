Brian A. Randell and Ian Griffiths, DuoMyth

The idea seemed simple: bring the global community together so they
could share and learn how to best use DevOps on the Microsoft stack
using Visual Studio Team Services, Microsoft Azure, with Visual Studio
2017 and Xamarin tools. Do this one day a year and make the event
global. This meant marshalling people, locations, and resources across
the globe. But having a global meet up wasn't enough. Xpirit wanted it
to be hands-on.

On June 16, 2017, Xpirit kicked off its first Global DevOps Bootcamp in
New Zealand and finished at the Western part of the continental United
States. With events in Australia, India, Europe, South and North
America, we were chasing the sunrise to have VMs ready to go at each
location. During the event, each attendee had access to their own
private dual-core Windows virtual machine running in Azure. At peak,
1,500 VMs were running using 3,000 cores of compute and over 180
terabytes of storage. Each attendee had their own private environment to
work with Visual Studio, Docker, and the other tooling needed to work
with the hands-on labs and get their hands dirty with DevOps in a lab
environment. All made possible through hundreds of individuals putting
hard work on the event, the power the Azure cloud, and Valhalla.

# Come to Valhalla

Valhalla is the name of our solution built on top of Azure that makes
events like the Global DevOps Bootcamp, the DockerCon 2017's hands-on
lab pavilion, and even 20-person hands-on workshops easy to run
providing one or more virtual machines for each attendee. The first
event hosted on Valhalla was a Microsoft ALM lab back in early 2014.
Since then, Valhalla has provided a solid foundation for running
traditional instructor led training classes, road-shows for Microsoft in
the United States, Europe, and Middle-East as well as DockerCon 2016 and
DockerCon 2017 where each lab attendee received three Linux-based VMs
and/or three Windows-based VMs work with various Docker and Windows
Containers related labs.

Valhalla was born out of a need to reduce the costs related to providing
hands-on access to Microsoft's ALM stack. We had been working with folks
in the Microsoft US subsidiary focused on helping them find ways to make
on site events at customer locations and open events at Microsoft
offices better by providing customers an opportunity to try the latest
versions of Visual Studio and Team Foundation Server in a hands-on
experience, on demand. We had been looking at using Azure hosted virtual
machines. But there were some issues. One big non-technical impediment
was the original billing model. In June 2013, Microsoft moved to a per
minute billing model as well as they stopped charging for compute when
VMs were not running. This change pushed things over the proverbial
hump. We knew we could build an affordable system on top of Azure.
Moving from just ALM related content, it expanded to be a
general-purpose system to provide "students" with access to a one or
more VMs, all within a managed environment in the cloud.

Over the summer of 2013, we designed the system and figured out funding.
In September, we started writing Valhalla originally in partnership with
our friends at Endjin. In the beginning, the VMs were to run on top of
the IaaS infrastructure where each virtual machine was allocated with a
Cloud Service. We designed the system to support a single payee model as
well as a shared payee model. We used web sites hosted in cloud
services, pushing messages to queues and worker roles to process
commands from the queues with table and blob storage as our persistence
stores. We wanted to make the system easy to use and manage yet be
flexible when it came to the types of content to support. In the
beginning, we only supported Windows VMs. As Azure changed, so did we,
adding support for Premium SSD storage and Linux VMs.

# Valhalla Today

Naturally, creating a single virtual machine in Azure using the Azure
Portal is easy. The UI guides you through the steps until you click the
final button.

![](./media/image1.png)

If you're running regular events or even one large event you'll
obviously want to automate this---nobody wants to walk through this VM
creation wizard 1,500 times. And while you might think just a little bit
of PowerShell or an ARM template makes it all "simple", it turns out
it's a bit more complicated.

From the beginning, we designed Valhalla to support flexible class
deployments (type of VMs, number of VMs, number of delegates) as well as
multiple subscriptions. In fact, having multiple subscriptions is a key
way to scale with Azure both for performance as well as scale out. If
you run events needing many hundreds of VMs you will rapidly discover
that the simple resource structure you get if you create a VM through
the Azure portal does not scale---you will run into Azure's
per-subscription resource limits, such as the default limit of 50
Virtual Networks. You can get this limit lifted with a support request
but there's a hard limit of 500, so you can't use the simplistic
one-VNET-per-VM model beyond that point (but equally, there can be
issues if you put all your VMs on a single VNET). With careful resource
design you can create thousands of VMs in a single subscription but you
are then likely to run into Azure's per-subscription API rate limiter,
which can slow you down or even cause operations to fail entirely. So
multiple subscription support becomes a must-have at sufficiently large
scale.

At the simplest level, we want to provide a person with access (RDP or
SSH) to one or more virtual machines for a period of time. In most
scenarios, we provide access to the delegates via a custom e-mail
message that we send out using the SendGrid service. That said, proctors
at an event can hand out the delegates access information instead.

We define what VM(s) a user gets in something we call a *[bundle
definition]{.underline}*. A bundle defines a number of pieces of data
including the recommended Azure VM size to use, whether the VM needs to
be accessible through a public IP, and more.

For storage, Azure supports two types for virtual machine hard drives
(VHDs): basic and premium. The difference at its core is simple: premium
storage is backed by solid-state drives (SSDs). While more expensive to
run long term, we've built optimizations into Valhalla to keep them
around only as long as needed. You only pay for the time the VHDs are
allocated. Premium storage can provide a more performant experience when
using interactive Windows sessions in particular. We support just about
any VM that you would have access to in an Azure subscription.

## Valhalla Architecture.

While Azure Storage is used for VMs, we use SQL Database (Microsoft's
name for its cloud version of SQL Server) as our main persistence store
for tracking classes, resources, etc. In earlier versions of Valhalla,
we used Azure Table Storage. While providing good performance and cheap
storage, the programming model left a bit to be desired. We found the
more traditional data programming model in SQL Database more productive.
We use SQL Database to store most data about the system.

![](./media/image2.png)

Again, as we've evolved Valhalla, we changed how we handle security. Our
original system had its own role-based security model. Our current
version relies on Azure Active Directory. Currently our system is mainly
accessed by ourselves and customers who need to manage classes and
events. Delegates don't need to log in to use their VMs. However, Azure
AD's flexibility means we can support other Azure accounts, MSAs,
Google, Facebook, and more.

As a cloud-based solution, the main UI of the system is implemented as
an Azure App Service Web App. We use slots to make it easy to deploy new
versions of a site.

Web Jobs handle long-running requests. Any time Valhalla needs to do
something in Azure, whether it's scanning an Azure subscription's
storage accounts for newly-updated VHD images, or creating VMs for an
event through the ARM (Azure Resource Manager) API, that work runs in a
Web Job.

We use Redis Cache to enable the Web Job to provide progress
notifications for its long running work to end users. We are using
SignalR to enable our web servers to push notifications to browsers, and
we are using SignalR's Redis Cache backplane to make it possible for our
Web Job to generate notifications that will be routed to whichever web
server is managing the connection back to the relevant end user. Under
the covers, this uses Redis Cache's pub/sub mechanisms.

We use Application Insights mainly for diagnostic purposes. If something
goes wrong, Application Insights is very good at providing a holistic
view---you can track an operation's progress through from some end
user's browser through to the Web App and then on through the Web Job.
Application Insights provides automatic interception of any operations
that use Azure Storage or SQL Azure, and the ability to discover all of
the events relevant to a particular request make it easy to get a good
overview of everything that happened up to the point of failure.

## DevOps

At this point it's pretty clear that Azure provides us with flexible
platform for our system and power on demand for our customers. However,
with just two guys running the company, it helps to have a good DevOps
process. Donovan Brown from Microsoft likes to say that "DevOps is the
union of people, process, and product to enable continuous delivery of
value for our end users." One advantage we have is we've been doing
software development for over fifty years between us. And our general
mindset is to change our process as needed to do more with less so we
can deliver value. Thus, we've got the people and process part down
pretty good (knowing it's a journey not a destination).

![](./media/image3.png)
From a product perspective, we're all-in
with the cloud with Visual Studio Team Services (VSTS). Back in 2013, it
wasn't completely obvious but we started managing our source code using
Git repos in VSTS. Over time, the flexible branching model has made it
easy for us to work distributed across two continents and eight hours'
time difference. We use the Scrum template with Product Backlog Items,
Tasks, and Bugs to plan and track our work. Our sprints are generally
thirty days.

Naturally we care about quality and thus we've worked to build various
automated quality checks into our process. It starts with unit tests
that we run from within Visual Studio as well as during our automated
build process. Our build process produces deployable packages, and runs
a full suite of unit tests and integration tests. Our builds run using a
continuous integration off master, and any feature branches which is
nice when we're working on different features.

![](./media/image4.png)

Our release process has evolved over time and this is a critical area to
keep us moving forward. With our various customers and events, updating
our bits in Azure is not something we want to be doing manually. We were
early adopters of the "new version" of Release Management that's built
into VSTS and it has served us well. We don't do a typical pipe line
development from dev to test to prod. What usually happens is we do lots
of deployments to one of our dev environments, then to test environments
followed by prod. The key is that we can have multiples of each type of
environment. They're different by purpose and customer but not by code.
In addition to the deployment task, which involves pushing out any core
service changes via ARM, the web sites, web jobs, as well as handling
slot swaps, we use Selenium to run automated regression tests against
the user interface of our web site. We have a specialized release
definition for running these end-to-end tests that creates a whole new
Valhalla environment (with the full set of Web Apps, SQL Server etc.
deployed to a dedicated resource group) so we can test all functionality
from scratch on a newly-deployed system, and this release then tears
down the environment if the tests all complete successfully.

As a side note, we use Azure Key Vault to manage secrets such as SQL
Server passwords and SendGrid credentials as part of DevOps. This
enables us to avoid storing any secrets in source control, and also to
keep them out of our build configuration. (VSTS's ARM deployment task is
able to look up the secrets itself when it needs them by accessing the
Key Vault directly.)

We track a wide range of data per event such as users, VM allocations,
etc. in addition to data gathered from Application Insights. We also
work to have post-mortems with customers to improve the system. New
feature work comes from feedback as well as from our experience at
running events, large and small. The larger events, like the Global
DevOps Bootcamp, help us add make the system more robust for all events.

## Lessons Learned

The saying goes "there's no place like production". Over the years we've
learned a number of things running on Azure. One key learning is that
multiple Azure subscriptions are necessary for large scale due to hard
resource limits and ARM API rate limiting. Another key learning is
*test, test, test*. Repeatability is vital. End-to-end testing is
especially valuable---our full-system tests have caught more regressions
than anything else. And as always, the only constant is change. Azure is
dynamic and is a fantastic platform on which to build solutions. It's
amazing to think that we can use over 180 terabytes of storage, have
1,500 VMs spun up burning eight *years* of CPU in a day and then give it
back to the cloud. Come to the cloud, we think you'll like it.
