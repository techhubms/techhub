# How to move to the cloud according to the Microsoft Cloud Adoption Framework

The first question is whether moving to the cloud is the right thing for
you. After all, each organization is unique. What are your business
objectives? What´s the required investment versus the return? Does a
cloud-based business model fit in with your organization, or do you need
to change the model or the organization? What´s the technology and
expertise that´s required? Is it something you can do yourself, or do
you need expert assistance?

Xpirit has supported dozens of organizations, ranging in size from
global corporations to SMEs, in successfully transitioning to the cloud
using the proven Microsoft platform. A valuable part of this platform is
the Microsoft Cloud Adoption framework, which assists you in
investigating your motivation and clarifying your strategy. The
following image shows the key areas of your investigation.

![Graphical user interface, application, Teams Description automatically
generated](./media/image1.png)


If you are considering a migration to Azure, using the Microsoft Cloud
Adaption Framework (CAF) is one of the paved roads you could follow. The
CAF helps you to define and implement a cloud strategy. It contains
documentation, tools, and best practices. It also describes a cloud
adaption lifecycle that has eight phases. One of them is the 'Ready'
phase, and this is the phase in which the cloud environment is prepared
for the planned implementation. One way to do this is to implement a
Landing Zone.

## Getting started and strategy

The Microsoft Cloud Adoption framework helps you focus on your business
objectives. Why would you move your business to the cloud? Is it because
of cost savings, or do you want to enable a new business model for your
products and services? For instance, do you want to upscale to match
market demand?

If moving to the cloud is the right decision to achieve your business
objectives, the next step involves investigating the steps you need to
undertake. This means taking stock of your digital estate, including a
list of all your technical servers. You must decide whether you need to
move, rearchitect, rehost, rebuild, refactor, or maybe retire them,
i.e., the five Rs of rationalization.

## From the organizational perspective, you must ensure that your skill set matches what you want to achieve or that you can grow to match the required skillset. Most likely, the organization of your business also needs to change to embrace the cloud and move to the cloud truly. These changes require a broad commitment to ensure your success. Moreover, to truly embrace the cloud, companies need to adopt a DevOps way of working, which leads to rethinking your organization structure. At Xpirit, we help companies get started migrating to and working with the cloud. At the same time, we guide them to implement a DevOps approach from a technical perspective as well as from an organizational perspective.

## Creating the right foundation

When ready to move to the cloud, a key success factor is a solid and
secure foundation for your applications. As a company, you will still
need to consider cross-cutting concerns such as security, networking,
compliance, costs, etc. However, your teams may be responsible for this
and should be enabled to do so effectively and efficiently. Here the
concept of a landing zone comes in, which is the concrete implementation
of the foundation that will help your teams to be effective.

What is an Azure landing zone?

"An Azure landing zone is the output of a multi-subscription Azure
environment that accounts for scale, security governance, networking,
and identity. An Azure landing zone enables application migration,
modernization, and innovation at an enterprise scale in Azure. This
approach considers all platform resources required to support the
customer\'s application portfolio and doesn\'t differentiate between
infrastructure or platform as a service."

Landing zones can be easily customized to fit your organization's
requirements seamlessly and thus be the enabler for your migration.

## Using landing zones when migrating to the cloud

Imagine a company called FreeBirds. They have run their software in a
data center in the Netherlands for over ten years. Their business is
growing rapidly in terms of the number of customers, but it's also been
a long time since all their customers came only from the Netherlands.
FreeBirds decided to move their applications to the Azure cloud. One of
the reasons for this decision is to reduce costs by fully leveraging the
elasticity of the cloud. Another reason is to be able to run their
applications across the globe and thereby increasing the speed of the
application as well as its resiliency and scalability. Building a
Landing Zone on Azure using a Hub and Spoke architecture is ideal for
this scenario.

### Ready-made foundation

When explaining the concepts of the Azure Landing Zone, it is helpful to
do this in the analogy of building a home. If you want to build a new
home, you could do everything yourself: dig the foundations, lay the
bricks, and do all the plumbing. The same goes for the Azure cloud. You
could start to build the infrastructure manually yourself. As with the
house, you might find this very time-consuming, and there would be the
risk of making many mistakes. It would be much easier to use ready-made
foundations and a blueprint that shows you exactly how to do things and
implements best practices. You could still customize the structure to
personal needs, but the building would be architecturally sound, safe,
and faster to build. An Azure Landing Zone is precisely like that. It
will cover network architecture, security, identity, and governance,
allowing DevOps teams to start building right away on a perfectly
laid-out foundation.

### Architecture

Landing Zones are often implemented using a Hub and Spoke architecture.
In this type of architecture, you have a central hub. The network in the
corner acts as a central point of connectivity to on-premises resources
for many spoke virtual networks, as shown in the image below. You often
find other resources in the hub shared among spokes, for example, an
Azure Firewall or a Log Analytics workspace for central log management.

![Chart, diagram Description automatically generated with medium
confidence](./media/image2.png)


### Hub and spoke architecture.

The middle part of the image above shows the hub virtual network. It
contains the resources needed to provide connectivity to the on-premises
network on the left. That connection is often established using a VPN
Gateway or an Azure Express Route. The connection in the hub network can
then be used by multiple spokes, as shown on the right. Each spoke
virtual network holds one workload, and the spokes allow you to isolate
your workloads from those of other teams. For example, you can use
spokes to run Virtual Machines (VM), Azure Web Apps, or databases.

In the scenario of FreeBirds, this would be a perfect fit. A central
team, often called a platform team or something similar, can build and
manage this Hub with all the central services. Each DevOps team creates
and delivers applications end-to-end and becomes one or more spokes.
This central team can ensure a safe adaption of the cloud, for example,
by forcing all traffic to the internet to go through a firewall. They
can also use Azure Policies to set guardrails for the DevOps teams,
ensuring that they cannot deploy, for example, a database in an insecure
way or forcing a daily backup. It is essential to ensure that these
significant teams do not become bottlenecks for other DevOps teams. This
means that the platform team must fully deliver their services in a
self-service and automated manner as much as possible.

### Valuable scenario

A Landing Zone is not only useful in a scenario like that of FreeBirds,
i.e., a company moving away from its on-premises data center. It will
also fit in hybrid scenarios and fully cloud-native environments that do
not contain any on-premises connectivity. All the benefits remain, such
as easy onboarding on the cloud, security, identity, and governance. The
Hub is also often used to provide central services to all teams that
would otherwise be too expensive or complex. An excellent example of
such an expensive and complicated offering is API Management, a service
to expose APIs to the outside world. When run in a production-ready way,
it easily costs over 3000 euro's a month. This is too much for each
DevOps team to run independently, and thus it is often managed
centrally. A service like Kubernetes, AKS on Azure, is another service
that is often managed centrally. Besides being expensive, it's also a
complex service to operate. Bringing that to a central team allows the
other DevOps teams to focus on their business value instead of using
their infrastructure.

## Xpirit Managed Landing Zone

The Landing Zones of the Microsoft Cloud Adoption Framework provide a
solid foundation with a complete set of generic functionality for moving
your services, applications, and data into the cloud within weeks. And
if you choose an Xpirit Managed Landing Zone, we take care of ownership
and maintenance tasks, allowing you to focus on your core business.
Moreover, we will ensure your system is always up to date by
implementing new features as soon as they become available. What sets us
apart is our intimate service provision with direct contact with the
right expert, combined with our extensive experience and expertise in
the domain of the Microsoft platform.

At Xpirit, we offer out-of-the-box landing zones that derive their value
from our seven years of experience in building them. We have identified
multiple flavors of these landing zones based on the domain that
customers work in. In addition, we have automated and optimized them,
allowing them to be deployed and up and running within two weeks.

For example, our Business Apps/Independent Software Vendor (ISV) landing
zone helps ISVs create the scale required to meet market demand. This
landing zone allows them to make a fast and reliable transition to a
worldwide scale. Typically these companies have one or more apps that
need to be enabled worldwide or to harness the scalability and
reliability principles of the Azure Cloud.

Of course, we can review your landing zone or create a custom zone that
suits your organization.

### Hub Spoke Model

The Xpirit Landing Zones use the Hub and Spoke architecture. Individual
applications run in spokes, each with a domain-specific workload, while
the hub provides generic services such as connectivity to the cloud and
proven security features. Naturally, the Xpirit model pays extra
attention to security with features that comply with ISO 27001, Azure
Security Benchmarks, and CIS.

### Comprehensive services with an ultra-short migration timeframe

We use a swift combination of the re-host and re-architecture cloud
rationalizations -- part of the Cloud Adoption Framework -- to move
logical components from a data center to the Hub and Spoke model. A
primary application can be up and running in a spoke within three weeks,
which means that a fast startup is possible using the generic features
of the high-performance hub architecture. And if you have any specific
needs requiring more specialized functionality or support for your
applications, we can still support you. After the re-host and
re-architecture phase is completed, we can look at the other R's of
cloud rationalization to improve the application. In addition to
managing the hub, we can also order a spoke application and help you
improve your application.

### Intimate service provision through a single point of contact

Traditional service provision is structured in terms of first and
second-degree support lines. Usually, you won't get to speak to an
engineer directly when you create your ticket but will receive automated
responses. This means you often must wait to talk to a service team
member who may not be completely aware of your specific situation. Our
team wanted to innovate this approach and create a much more intimate
form of service provision by making our Landing Zone engineers directly
available to support our customers.

The Xpirit Managed Landing Zones team consists of knowledgeable Landing
Zone experts with years of experience in enabling and supporting cloud
migrations and consultancy, coding for specialized architectures, and
advanced interfaces. You communicate directly with the right engineer,
who can always rely on the know-how of our entire Xpirit team, should
this be required. Moreover, our service is available 24/7: we have the
most qualified expert ready to help you during business hours and direct
operation support on standby at night. As a result of the single point
of contact and the short communication lines, you reap the benefit of
faster troubleshooting.

### Guaranteed response times

Following a short but thorough intake process, you receive a transparent
service-level agreement, including guaranteed response and recovery
times for incidents. We align our service levels to the severity of a
possible incident, varying from incidents that would mean the
discontinuation of your business to the lesser impact that slows down
service to less urgent issues. The organization of our service team
allows us to meet the agreed response times on a 24/7 basis.

To continuously improve our support system, we use a net promotor system
to regularly assess our clients' experiences and monthly meetings with
our clients and their service managers.

### Sustainable problem solving

Thorough engineering will improve your experience with the platform. A
defining feature of our service is our policy of preventing recurring
incidents and avoiding temporary workarounds and patches. We perform a
post-mortem for every incident by thoroughly investigating the root
cause of a problem and finding a sustainable solution. This
problem-solving approach and properly fixing anomalies instead of quick
patching will save time.

Our extensive set of automated tools with various validations and alerts
often allows us to predict and be ahead of any problems before they
occur. For instance, we use Azure Monitor to monitor the used components
within your Azure tenant continuously, allowing us to take preventative
measures before any workload or congestion problems occur. Moreover,
thanks to Xpirit's close collaboration with Microsoft, we have direct
and short contact lines with their team of cloud engineers.
