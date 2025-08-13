Migrating your company IT into the Azure cloud is a complicated and
time-consuming process. You need to think about many things such as:
team collaboration, cost control, connectivity to on-premise systems,
governance and compliance, education on how to use the cloud, and still
facilitate efficient onboarding of DevOps teams. In this article, you
will read about my experiences in helping an airliner transform from
using a classic datacenter into Azure.

# The case

*About a year ago, the management team of the fictional airliner, "Not
Invented Aire" (NI-Aire), decided it was time to move their IT systems
into Azure. They came to this decision because of ever-increasing
friction between development teams and the datacenter operator. A few
years ago, they accepted that a dedicated operations team made all
changes to IT infrastructure and that it was acceptable for this to take
a few weeks to complete. Today, they are adopting ever more DevOps
practices. DevOps Teams must be enabled to deploy both software and
infrastructure changes whenever needed, and sometimes they need changes
multiple times per day. Their datacenter operator is unable to comply
with this need. Fortunately, we can accomplish this by using Azure. So,
all teams are now moving their workloads into the cloud. Before the
migration started, NI-Aire workers had some questions:*

-   *How do we connect with remaining on-premise systems?*

-   *How do we deal with security and compliance?*

-   *How can we share cloud knowledge and best-practices?*

-   *How can we get insights into our Azure consumption?*

# How do we connect with remaining on-premise systems?

Microsoft recommends using a Hub & Spoke architecture to provide
connectivity from cloud services to on-premise systems. A Hub & Spoke
architecture looks like the diagram shown in Figure 1.

![User](./media/image1.png)
![Female
Profile](./media/image2.png)
![Male
profile](./media/image3.png)
![](./media/image4.png)
![](./media/image4.png)
![](./media/image4.png)
![](./media/image4.png)


Figure : Hub and Spoke architecture

Instead of having to create a VPN connection from every team's
subscription to the on-premise network, the Hub & Spoke architecture
uses a single connection from a shared network, the **Hub** network. It
uses Azure ExpressRoute[^1] to create a link from the Hub network to the
on premise datacenter. The teams can then create their own (**Spoke**)
networks and connect them to the shared Hub network by *peering*[^2]
them.

![Checkmark](./media/image5.png)
In this example, two teams have peered a
Virtual Network with the Hub network. Each team uses dedicated Azure
subscriptions.

Connectivity with on-premise systems

We solved the connectivity problem, but unfortunately, we also
introduced two new ones. One of the issues that arise from adopting this
architecture is the management of the shared Hub network and the
ExpressRoute connection; which team owns these resources?

Another problem is the use of IP address ranges assigned to the virtual
networks. We need to make sure that teams don't use overlapping ranges,
as this would create traffic routing issues. Dealing with IP address
ranges and subnets can be complicated. So, ideally, we would like to
help teams by provisioning this bit of infrastructure for them. This
way, they don't need to worry about this complexity.

## Managing shared resources

To manage shared resources, NI-Aire chose to create a separate team;
named the **Platform team**. This team consists of Product team members
(part-time) and some dedicated people. It is dedicated to facilitating
the other teams (**Product teams**) in their cloud migrations, not just
by provisioning shared infrastructure, but also by offering hands-on
assistance, sharing best-practices, and education in cloud concepts.
Costs for shared resources are divided across all teams equally. Product
teams can make changes to shared infrastructure by making pull requests
on the Infrastructure as Code definitions, so the Platform team does not
become a bottleneck and ownership is shared between stakeholders.

## Provisioning infrastructure for teams

The Platform team chose to use Azure Blueprints[^3] for required
infrastructure deployments. With Blueprints, we can use Azure Blueprints
to deploy resources, like virtual networks and ExpressRoute, that adhere
to the company's standards and requirements. The main difference between
using Blueprints and using regular ARM templates for deployments is that
Blueprints allow us to set up a specific environment. For example, it
enables us to create resources and resource groups inside particular
subscriptions.

# How do we deal with security and compliance?

Coming from a situation in which the datacenter operator managed virtual
machines, DevOps teams needed some guidance on operating the machines
they used in the cloud. For instance, they needed help with the
automated installs of anti-malware software and security patches. The
Platform team applies the 'carrot & stick' analogy.

## The carrot

The 'carrot' entices Product teams to use tools provided by the Platform
team. For example, all Product teams are free to choose how they deploy
their infrastructure. But to help them, the Platform team provides a set
of validated ARM templates they can use. For instance, they provide a
template to deploy a virtual machine preconfigured with an anti-malware
extension. The Platform team should provide hands-on assistance to the
Product teams.

## The stick

![Checkmark](./media/image5.png)
When a Product team decides they would
rather use PowerShell to deploy their virtual machine, it should still
have anti-malware software. But what if they forget to add it? This is
where the 'stick' comes in. We use Azure Policies to check whether all
Azure resources comply with company standards. For instance, we created
a Policy that checks whether anti-malware is deployed to all Windows
virtual machines. We combined the Policy with a remediation that
automatically deploys an anti-malware solution to a VM if needed. A
Policy can flag incompatible resources on the dashboard, remediate
incorrectly configured resources, or prevent them from being deployed at
all. We also use Policies to enforce resource naming guidelines, to deny
deployments to unwanted Azure regions, and to apply role-based access
control policies (RBAC). The Platform team does not only assign Azure
resources, but they also assign Policies to Product team subscriptions.
They do this by using Azure Blueprints, another nice feature of the
Blueprints service.

Dealing with security and compliance

# How can we share cloud knowledge and best practices?

If we're not careful, the Platform team can quickly become a bottleneck
when all Product teams depend on them to share knowledge of Azure
services and deliver shared templates. In reality, Product teamswill
soon become familiar with the Azure resources they use, so logically, it
should be them sharing [their]{.underline} knowledge. This way, the
Platform team only acts as a knowledge broker, not as the 'single source
of knowledge'. To give an example, at NI-Aire, Product team 1 wanted to
use Azure API Management, and Product team 2 was already using it. The
Platform team was aware of this, and connected the two groups and urged
them to cooperate. In the end, Team 2 produced an ARM template, and Team
1 was able to use this for their deployments.

## Optimized use of resources

Having every team deploying resources can introduce another problem: The
software architecture requires API's to run inside a virtual network to
allow communication with on-premise systems. For security reasons, these
back-end API's cannot be exposed to the internet directly. Currently,
only one SKU of API Management enables virtual network integration:
Premium. When compared to other Azure resources, the Premium SKU of
Azure API Management is relatively expensive. At this time, it costs
around €2300 per month. In terms of workload capacity, the Premium tier
is oversized. Therefore, if every Product team would run an instance,
NI-Aire would pay a lot of money for many under-utilized resources.

## Internal open-source

To mitigate the problem of under-utilized resources, the Platform team
deployed API Management into the same subscription that runs the Hub
virtual network (see Figure 1), named the **Platform Azure
subscription** (see Figure 2).

Figure : Platform subscription

To deploy API Management, they used the ARM template that Product team 2
created earlier, combined with additions required by Product team 1. The
problem of under-utilized resources is now mitigated by sharing the
resource amongst Product teams. Again, this created another problem. Who
owns this resource? Will the Platform team be responsible for fixing
issues and dealing with outages? This doesn't fit well in the DevOps
paradigm of '*You build it, you run it*'. We need a way to enable
Product teams to operate API Management and other shared resources
inside the Platform Azure subscription while keeping the Platform team
in control over their Azure subscription.

At NI-Aire, we solved this issue by creating an 'internal open-source'
Azure DevOps repository to store ARM templates. This repo contains ARM
templates that are 'pre-approved for deployment' by the Platform team.
They comply with security guidelines and other best practices. Any
Product team can change these templates by doing a pull-request. A
member from another team can then review the change and approve it.
Platform team members monitor changes periodically as well. After the
change is approved, a new version of the ARM template will be made
available for use by publishing it to a package feed. Having Product
team members as part of the Platform team enables quick approval for
changes. The Platform team chose to use the Azure DevOps universal
package feed[^4] as the source to post and download ARM templates, as it
comes with built-in version support.

This process, which is shown in Figure 3, is designed to enable teams to
collaborate on Azure ARM templates, without introducing a bottleneck.

Figure : Collaboration on ARM templates

## Template repository

Over time, the shared ARM template repo grew to contain lots of
templates; for example, to deploy virtual networks and ExpressRoute.
This way, they can be used both by regular deployments and Blueprint
assignments.

Over time, teams added many useful templates:

-   Virtual networks that provide connectivity between services

-   Virtual machine with anti-malware software and automated updates

-   Log Analytics, with solutions that support VM updates

-   Azure Key Vault, for application and deployment secrets

-   Managed Identity, to securely connect between Azure resources

-   And, of course, API Management, to securely consolidate and expose
    APIs.

The shared repository became the first place where Product teams looked
when they needed to deploy a new resource into Azure. If the template
wasn't there yet, they would contribute a new one to the repository.

## Education

Some of the NI-Aire team members had no prior experience in working with
the Azure cloud and needed technical training. Again, the Platform team
cannot become a bottleneck, so they wrote a curriculum of online
training they could take. For example, Microsoft offers a free online
introduction to Azure[^5]. Pluralsight also offers some excellent
training for various levels of expertise.

Of course, it is also essential for teams to learn from each other.
Product team members regularly held '*Lunch and Learn*' sessions, during
which they would share something they learned with the rest of the
teams.

The Platform team created videos in which they explain the ideas behind
the tools. For example: 'How network connectivity works' and 'Why do we
have an 'Internal open-source' template repository?'. This way, how to
get started and quickly take off into the cloud can be explained to new
team members .

![Checkmark](./media/image5.png)
Share cloud knowledge and best-practices

# How can we get insights into our Azure consumption?

It is easy to spin up powerful resources in Azure. But how can we make
sure we're not wasting money on resources? It starts with providing
DevOps teams with insights into their generated costs. While there are
multiple strategies to choose from, we decided to use separate Azure
subscriptions per team. Each team gets two subscriptions; one for
testing (non-production) and one for running production workloads
(production). Costs are available at the subscription level, so there is
no need to filter consumption per team. Additional benefits of this
approach are:

-   Subscription resource limits -- Azure enforces some limitations on
    the amounts of resources we can create; for example, there is a
    limit of 20 VM Cores per subscription per region. It is less likely
    to become constrained by these limits when teams aren't sharing a
    single subscription since every team can use 20 cores.

-   ![Checkmark](./media/image5.png)
    Simple Role-Based Access Control --
    The Active Directory group of the team gets assigned access,
    'contributor' on the non-production subscription, and 'reader' on
    the production subscription.

Insights into Azure consumption

# Conclusion

I believe that the key to a successful cloud migration is to enable
Product teams with a self-service platform. They need to be able to
autonomously provision well-configured infrastructure in whatever way
they see fit, and to be able to efficiently collaborate with other
teams. With this article, I provided you with some insights into how we
solved these challenges at NI-Aire. If you need help migrating your
organization into the cloud, feel free to reach out. We'll help you
board, so you can quickly take off into the cloud.

[^1]: <https://azure.microsoft.com/en-us/services/expressroute/>

[^2]: <https://docs.microsoft.com/en-us/azure/virtual-network/virtual-network-peering-overview>

[^3]: <https://docs.microsoft.com/en-us/azure/governance/blueprints/overview>

[^4]: <https://docs.microsoft.com/en-us/azure/devops/artifacts/quickstarts/universal-packages>

[^5]: <https://docs.microsoft.com/en-us/learn/paths/azure-fundamentals/>
