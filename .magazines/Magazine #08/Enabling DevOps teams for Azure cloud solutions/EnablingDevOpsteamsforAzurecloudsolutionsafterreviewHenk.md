# Enabling DevOps teams for Azure cloud solutions

## Introduction

Digital transformation enables companies to realize innovations and
deliver products and services with higher quality in order to exceed
customer expectations (better), reduce prices (cheaper) and shorten the
time-to-market (faster). However, this transformation requires
organizational as well as technological changes.

In this article we will explain an organizational and platform-agnostic
technology architecture that helps in realizing these digital
transformation goals. The second half of the article contains an example
of an implementation of this architecture on Azure.

## Adopting DevOps to enable digital transformation

The keywords for achieving better, cheaper, faster products and services
are *flow* and *value*, and this is what a DevOps way of working aims to
achieve. To help us focus on the creation of flow and value in the
delivery of products and services, we use the DASA DevOps principles as
guidance (see Figure 1).

![Afbeeldingsresultaat voor dasa devops
principles](./media/image1.png)


Figure 1: DASA DevOps principles (source:
https://www.devopsagileskills.org/dasa-devops-principles/)

These principles focus on organizations and the individuals in those
organizations. They describe what actions, behavior and other aspects
are required from these organizations in order to migrate to, or adopt a
DevOps way of working. It would take too long to explain the principles
in detail, but we will explain them briefly to be able to understand the
reasoning behind the architectures described in this article. For more
detailed information, please refer to the DASA website and resources.

The principles have been listed in their order of priority. The first
one focuses on the creation of value for the customer, because this is a
fundamental value for an organization to be able to survive in this
digital age. The second principle means breaking down siloes by
structuring an organization around products and services instead of
processes and subject-matter expertise. The third principle is saying:
"You build it, you run it" and means that you are responsible for the
products and services until they cease to exist. Achieving flow in work
that needs to be done and decreasing dependencies between teams and
individuals is what principle number four aims to achieve. To be able to
change in incremental steps, a rigorous continuous improvement process
must be adopted as stated in principle five. And last but not least,
improving continually also means automating everything that is
repetitive when possible. By doing this, principle six aims to increase
quality and maximize flow.

In order to allow teams and individuals to apply these principles they
must have, or grow towards, an organization that is geared towards
increasing the flow of value to the customer. In addition, they also
need the technical resources to support them in this mission. In the
next two paragraphs we will zoom in on the organizational and technical
architectures that enable this.

### Organizationally enabling DevOps teams

When looking at an organization before a digital transformation, IT
plays a supporting role to the business (see the left hand side of
Figure 2). Business-units have a cross-backlog demand in order to get
their required products and services to production. Development and
operations are separated into a change and run organization among other
siloed organization structures. This way of organizing has a negative
impact on the optimal flow in the software delivery value chain because
of organizational and technical dependencies. In addition, the siloes
cause hand-over moments and loopbacks in the delivery process, which is
also inefficient.

![](./media/image2.png)


Figure 2: Organization architecture as-is and to-be

In the new situation (see right hand side of Figure 2), the teams are
organized around autonomous business capabilities, which means that they
can develop their products and services without disturbing, or being
disturbed, by other developments. The teams are cross-functional, making
them capable of developing and running their products and services until
they are no longer required. Technically they are supported by a
self-service (cloud) platform which enables them to rigorously automate
and quickly innovate by incorporating new platform services to their
offering. This is done in an incremental, continuous improvement way of
working.

### Technically enabling DevOps teams

In order to provide teams with the ability to operate and act according
to the DevOps principles, a high and mature level of autonomy and
agility is required. This poses requirements and constraints on the
technical architecture, but also on the way governance and control is
achieved.

DevOps teams will need to be able to have end-to-end responsibility for
their value proposition and the corresponding implementation of
architecture and applications. Nowadays most teams are very capable of
doing this for traditional software development, focusing on delivering
the application.

A digital transformation that incorporates a cloud platform presents new
opportunities. The practices need to embrace a similar approach for the
infrastructural part of the software solutions as well. DevOps teams are
facilitated in this by a self-service cloud platform (see Figure 3).

![](./media/image3.jpeg)


Figure 3: Self-service (cloud) platform logical architecture

**Achieving full autonomy through infrastructure-as-code**

Modern cloud platforms allow complete automation for provisioning the
resources they offer. This automation enables a team to treat the
infrastructural aspects of a software solution in the same way as the
implementation of business and customer functionality. This practice of
"Infrastructure as Code" creates scripts and templates in a
cloud-platform specific format that is maintained, tested, and uses
build and release pipelines like conventional code. This flow will allow
a team to automate all aspects of the application parts they need to
build and host on cloud resources. It also allows the team to
continuously improve the infrastructure, because it becomes trivial to
remove existing resources after changes and to reprovision them, instead
of having to change and maintain previously deployed resources. A team
can become completely autonomous when it is able to achieve this level
of automation in cloud solutions. The team can provision and deprovision
resources for these solutions as value-stream specific cloud resources
(see the Cloud resources value stream A-D in Figure 3).

**Maintaining governance and control with full-autonomy**

The biggest challenge in delivering value after adopting the full
automation of infrastructure involves the governance and control over
the cloud platform. A self-service cloud platform should give autonomy
and agility to teams, and also provide the appropriate level of
governance and control to an organization. While the team needs to be
able to be agile, move fast and be independent, the organization needs
to be compliant and provide traceable processes and be in control of
costs and security of the cloud platform hosting.

Fortunately, cloud platforms offer various features to have this level
of control while still providing self-service capabilities to the teams.
The whole purpose of this approach is to enable the value stream teams
to provision their own cloud resources, within the constraints offered
and required by the organization. The anti-pattern to this is creating a
single point in the organization, such as a team, where the teams have
to request and acquire the cloud resources they need. However, having a
single point of administration will block and slow down teams that want
agility and speed.

The self-service cloud platform should offer cross-cutting functions
(see Cross-cutting functions in Figure 3) by using the cloud intrinsic
features for monitoring, cost management, and security. Each cloud
platform implements these features in a different manner. From a
cloud-agnostic point of view, the monitoring features should allow both
teams and the organization at an aggregate level to monitor the health
and security of the hosted solutions. This includes resource
utilization, ownership of resources, and active security status, to name
a few.

Additionally, a shareable set of cloud resources can provide a layer of
structure and boundaries for the teams, on top of which they can build
their solutions (see the Shared Cloud Resources in Figure 3). This can
range from shared security features to networking topologies that make
sure that certain quality and safety standards are being met
automatically by the teams.

**Limiting access to resources**

Another aspect to consider is the use of authorization and role-based
access control. Using these security aspects, it is possible to limit
the rights of principals to create cloud resources or certain types
thereof. In lieu of the full automation, compliance and security, one
could go as far as removing all rights from regular user accounts,
except read-only access. The rights to create and manage resources is
only given to service principals (non-human accounts) that are assigned
as the identities for build and release pipelines. This forces the use
of the pipelines for resource management and disallows direct manual
intervention, increasing compliancy and traceability of the cloud
solution as well as the level of security. The strict authorization can
be applied to all environments or at least the critical ones, for
instance production. Since no human can make changes and everything is
automated, approval processes can be simplified to check for the proper
use of blessed templates and scripts. In time it might become apparent
that approval is not even required anymore. At that point removing the
approval altogether will increase the agility and speed for the value
streams even further. The use of strict authorization should be applied
with caution though, as it can severely limit the teams when applied too
rigorously, and effectively take away the necessary privileges for a
team to be able to self-service its cloud resources.

**Transition to self-service with a cloud platform team**

A dedicated team can help during the transition to the self-service
cloud platform. This "cloud platform team" can accelerate the cross-team
self-service features and functionalities. The purpose for this team is
to implement the cross-cutting functions and the shared resources, as
well as guidance for the teams and help during adoption and
transitioning to the self-service platform. The team can create
"blessed" templates and scripts for the teams to use in provisioning
value stream-bound resources. These automation artifacts have been
tested and security hardened to make sure the security baseline for the
teams is met by default when they are utilized in provisioning build and
release pipelines. The underlying shared resources give the teams the
harness of enough freedom while maintaining a secure and compliant
implementation for the cloud infrastructure.

The cloud platform team is a temporary team and should dedicate itself
to delivering the cross-cutting and shared features, onboarding the
value stream teams, and making themselves redundant.

The value stream teams can take over the responsibilities of the cloud
platform team as a community effort. Since all provided features are
treated like code, the way community contributions are made can work the
same way for the delivered infrastructural artifacts.

In addition, they lead by example in showing the behavior and mindset
that is required for the new way of working. Please note that the impact
on cultural change should not be underestimated. To be successful, it
can even be beneficial to add a dedicated coach to the platform team to
accelerate this change.

## Implementing a self-service cloud platform on Azure

The first half of this article explained what is required
organizationally and technically to maximize the creation and increase
of flow of value to the customer. This second half contains an example
of an implementation of the technical architecture in Azure.

The Microsoft Azure platform accompanied by Azure DevOps (previously
known as Visual Studio Team Services) is well suited to implement the
self-service cloud platform. Azure offers advanced resource management
and monitoring capabilities. Its automation engine is called Azure
Resource Manager (ARM), which can be automated by using ARM templates or
the Azure Command-Line Interface (CLI). Either of these allow full
automation of provisioning and managing Azure resources.

At the highest level, Azure uses the notion of an enterprise and
subscriptions. The enterprise is a representation of the organization
that uses Azure, and its subscriptions are administrative units of
ownership and rights. The subscriptions align well with the value
streams, where each team can be an owner or contributor, depending on
whether full or nearly full management rights are allowed. The resource
management in Azure is governed by security policies at various levels.
From data plane to control plane you can define authorization at a
coarse and very fine grained level. By giving the teams respective
rights, they can create all resources anywhere within the subscription,
or within resource groups as contributors. The latter is a way to allow
teams to create resources in a more controlled way, because additional
permissions can be set at a resource group level. It avoids giving the
teams full administrative rights to the subscription.

For the cross-cutting functions Azure has several features offering the
monitoring, compliance, security and cost management capabilities
required. Azure Monitor, Azure Security Centre, and Azure Cost
Management are ready-to-use features that combine information gathered
from and across the subscriptions for the value streams. The governance
and compliance can be taken care of at this higher aggregation level.
Azure DevOps, even though not part of the Azure cloud platform per se,
is the single point of arranging the build and release pipelines for
provisioning. It can provide the full end-to-end traceability for
compliancy reasons, from code to hosting environment. Azure DevOps
combines source code management with work item tracking and pipelines to
environments after approval and passing quality gates. Leveraging these
features allows teams to stay compliant because every change to code and
environments is tracked and audited in Azure DevOps.

![/var/folders/dt/3kg63xh10gb85bk2my4gr6540000gn/T/com.microsoft.Word/WebArchiveCopyPasteTempFiles/cidD9131DE8-2EA2-BF4E-B76E-220189CF57B5.png](./media/image4.png)


Figure 4: Cross-cutting functions in Azure

The next example in Figure 5 illustrates how shared cloud resources can
be used to provide a secure default self-service cloud platform. The
general idea of the scenario in the example is to allow the teams to
provision web resources, while still keeping control over public
availability and securing their resources. The intent is to give freedom
and protect against unwanted disclosure and exposure of internal
network-reachable resources.

![/var/folders/dt/3kg63xh10gb85bk2my4gr6540000gn/T/com.microsoft.Word/WebArchiveCopyPasteTempFiles/cid64DC1FE0-58A8-AA42-AF9D-228E336A92C4.png](./media/image5.png)
\
![/var/folders/dt/3kg63xh10gb85bk2my4gr6540000gn/T/com.microsoft.Word/WebArchiveCopyPasteTempFiles/cidC2F07ADC-EE60-FE43-B35C-BDEA51ED7BA0.png](./media/image6.png)


Figure 5: Example self-service platform hybrid-architecture implemented
on Azure

Each value stream and team is given their own subscription. Within these
subscriptions virtual private networks are created to isolate value
streams from each other. Hosting plans are created inside the
subscriptions and the team can provision web apps as they see fit. The
design of these web apps does not allow any outbound connectivity. This
avoids exposing anything immediately after creation and provides a
secure, default approach.

To be able to release web applications for the first time, changes need
to be made at the shared resources level. While this is blocking to some
extent, it does provide control in terms of which web application is
allowed access to the public internet and when. This provides an
opportunity to make sure that only approved and validated web resources
are disclosed. It only has to happen once during the initial release, so
it should not be a big nor lasting hurdle in the value stream flow. The
technical implementation for retaining access over public facing web
applications is the Web Application Firewall and Gateway. This Azure
resource has to be configured so that it allows inbound and outbound
HTTP and HTTPS traffic, all by automated scripts, and obviously after
approval. By keeping this resource at a shared and governed level, the
organization retains its ability to have control over web-exposed
solutions, while giving the teams freedom to create any resources up to
the point that they need to be released externally.

One other security measure in the example is the use of Application
Service Environments (ASE). The scenario shows connectivity to an
on-premise infrastructure via a Virtual Private Network (VPN) Gateway.
It is non-trivial to create a VPN tunnel to on-premises networks.
Keeping the connection at a shared resource level makes it reusable over
the various value streams and teams, in addition to providing a single
point of entry into the on-premises network. The VPN Gateway provides
another control mechanism for securing access to the on-premises
resources by specifying advanced access rules for allowed network
traffic to and from it. Each value stream Virtual Network is given a
peering to the shared virtual network that includes the VPN and Web
Application Gateways.

The shared resources are created by the initial cloud platform team,
which behaves and operates like any of the other value streams. While
the team still exists, it provides a different value stream, consisting
of the self-service platform's shared resources for the other teams and
value streams to utilize. In a similar fashion, the underlying VNETs and
peerings are also not created by the teams themselves, but by the
platform team instead.

## Summary

Companies with the aim to deliver better and cheaper products and
services in a faster way need to make a digital transformation. They
should adopt or migrate to a DevOps way of working to increase the flow
of value to the customer. To achieve this, organizational and technical
changes are required to enable teams and individuals. A temporary cloud
platform team can help to make the transformation happen. The technical
resources can be implemented in Azure, as shown in the example of a
self-service platform in Azure. When both aspects (organization and
technique) are applied in coherence, a company's teams and individuals
are lined up to achieve the digital transformation goals.
