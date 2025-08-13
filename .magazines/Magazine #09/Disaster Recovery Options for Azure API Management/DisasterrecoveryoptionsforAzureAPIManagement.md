# Disaster Recovery (DR) options for Azure API Management (APIM)

# Introduction

APIs are becoming mainstream in most organizations, which is why API
Management solutions are in high demand in order to standardize the way
APIs are published and also to enforce some security policies. In this
article, I will focus on a recurrent requirement that is ensuring some
business resilience and minimizing the impact of a service outage. The
requirement involves an architecture that supports a Disaster Recovery
(DR) scenario.

A common misconception is to think that it is up to the Cloud provider
to make sure no service outage will take place. However, the reality is
that some system maintenance has to be performed which causes some
planned downtime, and this is a shared responsibility. The Cloud
consumer has to ensure proper home work has been done to be resilient to
both planned and unplanned downtimes (outage), and sometimes even to
severe outages, impacting an entire region. In the rest of the article,
I will first highlight the risks we are trying to mitigate and the
various options we have at our disposal to ensure an appropriate
response.

# First things first. What are the risks?

What exactly are the risks we should mitigate when running Azure API
Management? The first thing to check is the Service Level Agreement
(SLA) associated to the service
(<https://azure.microsoft.com/en-us/support/legal/sla/api-management/v1_1/>).
In short, Microsoft guarantees 99.9% of service availability, leaving
room to approximately 8.76 hours of service unavailability over a year,
which is not bad. However, if you have a higher requirement, you may
need to opt for another solution.

Beyond the SLA of the service itself, there might be circumstances in
which unexpected events occur:

-   A regional outage: when such an event occurs, although rather rare,
    all the service instances deployed to that specific region become
    unavailable and unresponsive.

-   A customer-specific APIM instance is not responsive.

-   A customer-specific tenant encounters some issues not observed
    elsewhere.

-   A dependency is not responding. For instance, you might have a
    gateway policy that reaches out to a third-party service or to a
    custom backend whose purpose is only to validate a token or to
    enrich a received token, etc.

-   The backend services that are published to the API gateway might
    become unhealthy. It might be because of a service-specific issue
    (Cloud provider side) or because of poorly written code (Cloud
    consumer side) that does not sustain load very well.

The above list is certainly not exhaustive, but it already gives an idea
of all things that could go wrong and for which we need to find
compensation mechanisms.

# APIM's pricing tiers

APIM comes with the following pricing tiers:

  -----------------------------------------------------------------------
  Developer                           No SLA
  ----------------------------------- -----------------------------------
  Basic                               SLA 99.9%. This tier is described
                                      by Microsoft as *entry-level
                                      production use cases*

  Standard                            SLA 99.9%. This tier is described
                                      by Microsoft as *medium-volume
                                      production use cases*

  Premium                             SLA 99.9%. This is described by
                                      Microsoft as *High-Volume and
                                      Enterprise production use cases.*
                                      At the time of writing, this is
                                      also the only flavor that
                                      integrates with Azure virtual
                                      networks.
  -----------------------------------------------------------------------

At first sight, we might think that the only difference between those
tiers is the volume of requests they can handle since the SLA is the
same for all. However, one of the major differences is the fact that the
premium tier is the only one that can span across regions using
multi-region gateway units. To understand what it means, let's see what
composes an APIM instance:

-   The instance itself, holding the overall configuration: policies,
    products, subscriptions, etc. and that is bound to a git repository.

-   The API gateway that is in charge of applying policies and forwards
    the incoming requests (coming from API consumers) to the backend
    services.

The premium tier only offers to span multiple gateway units across
different regions, but the instance itself remains in a single region.
This means that in case of a regional outage, other gateway units will
still handle HTTP(s) traffic but no configuration change will be
possible until the region gets back to normal.

Let's see this in practice and explore some other options.

# Disaster Recovery Architecture

## Using the premium pricing tier

Figure 1 shows what can be achieved with the premium pricing tier:

![](./media/image1.png)


*Figure 1: Disaster Recovery with APIM Premium*

A single APIM instance deployed to Western Europe (in this example) that
has one gateway unit in Western Europe and another one in Northern
Europe. By default, an Azure load balancer will route the traffic to the
closest possible region. So, if a consumer request comes from Western
Europe, Azure will automatically try to forward it to the Western Europe
gateway, otherwise to Northern Europe. This behavior could be changed by
setting up a Traffic Manager in front of the gateways with another
routing method.

An important thing to note is that APIM does not handle the routing
logic towards the backend services. One must write custom policies to
achieve this. Here is an example, extracted from Microsoft
documentation[^1] that shows the logic in action:

![](./media/image2.png)


*Figure 2: policy-based routing*

But hey, isn't something missing here? What the above policy does is to
transfer the incoming request to the regional backend. So, if the
request is sent by the Western US gateway, it forwards to the US-hosted
backends, else to Asia, else it falls back to a third region here. While
this might sound logical, such an architecture sets your business
resilience at risk. What if the regional gateway is up and running but
the underlying backend services are down? One must take this into
account in the policy with the following logic:

-   try to forward to the backend services of the same region;

-   in case of failure, fall back to the other region.

There is a very good blog post
(<https://devblogs.microsoft.com/premier-developer/back-end-api-redundancy-with-azure-api-manager/>)
from Microsoft describing how to implement fault tolerance in a policy.

The benefits of using the premium pricing tier to have a DR architecture
are:

-   it is the easiest way to achieve DR;

-   Azure has some internal plumbing that detects whether gateway units
    are healthy or not, and routes incoming requests automatically to
    the healthy ones.

The main drawback is:

-   it comes with significant costs. Each gateway unit costs
    approximately €2000/month and of course, to be DR compliant, you
    need at least two of them.

Note that since April 2019, Microsoft has announced
(<https://azure.microsoft.com/en-in/updates/self-hosted-api-management-gateway/>)
a private preview of a self-hosted APIM gateway. At this stage, this is
still speculation, but this could mean that we could achieve
multi-region deployment of gateway units with the other tiers than the
premium one. Now, Microsoft might come with some restrictions since this
could defeat most reasons why the premium tier is chosen. Typically
these are: multi-region gateway units and virtual network integration.
By self-hosting the gateway, the latter could also be achieved easily.

## Alternative to the premium tier

Because the premium tier is quite expensive, it might not be suitable in
all the situations. Indeed, if a high SLA must be ensured but only a low
workload is foreseen, paying 4000+ euros a month might be overkill and
not every customer can afford this. I have seen customers building a DR
architecture with the standard tier in the following way:

![](./media/image3.png)


*Figure 3: DR with the standard tier*

In the above diagram, two different APIM instances are deployed, each in
their own region and each with their own bundled gateway unit. A Traffic
Manager (Front Door would also be ok) is required to route incoming
HTTP(s) requests to the gateway units. This time, no specific routing
policy is required since there is no region context, although you may of
course create a fault-tolerant policy as we have seen earlier. There
would be a variant though: since there is no more virtual network,
network peering could not be used any longer as a way to communicate
between regions. Fault-tolerant policies should use the other region's
gateway URL instead of the backend URLs themselves. In this scenario,
backend services are using public app services with IP restrictions,
whitelisting both gateways. If hosting the backend services within a
virtual network is a strong requirement, you'd need a reverse-proxy
between the API gateway and the backend services to bridge them all.

The main benefit of this approach is:

-   Running costs are substantially cheaper. The standard tier costs
    about €500/month, so four times cheaper than the premium tier.

The drawbacks are:

-   Since you run two different instances, the configuration should be
    pushed to both instances and make sure they remain in sync.

-   Each APIM instance has its own user/subscriptions, meaning that one
    cannot use the out-of-the-box developer portal to onboard new
    subscribers since each APIM instance has its own portal. In case of
    regional outage, one must ensure that API consumers can use the same
    subscription key whatever region they are redirected to. It is
    possible to use the Product Subscription delegation feature that
    lets you hook a custom page with a custom logic to register
    subscribers. That way, the custom page can use APIM's REST API to
    push changes to both instances.

-   Basic and Standard tiers do not integrate with virtual networks. By
    default, using APIM only, you can't host your backend services into
    a private network.

-   Overall, the approach is more convoluted.

# Conclusion

From my experience, the most frequent architecture to respond to DR
requirements implies the use of the premium tier. The other reason that
pushes organizations to go premium is the fact that, at the time of
writing, it is the only flavor that integrates with a virtual network.
Most companies still rely heavily on the network to secure their
workloads, which leads them to host the backend services inside a
virtual network, usually onto an Internal Load Balancer App Service
Environment also known as ILB ASE. Microsoft documents a PCI (Payment
Card Industry) compliant architecture based on an ASE[^2], which is why
it is a very common pattern. More than often, the premium tier is also
selected for a single-region deployment, only to comply with PCI
requirements.

That being said, the alternative described in this article may also be a
fit, certainly for smaller customers who do not especially have PCI-like
obligations.

[^1]: Policy-based routing towards backend services
    <https://docs.microsoft.com/en-us/azure/api-management/api-management-howto-deploy-multi-region>

[^2]: PCI compliant architecture
    <https://docs.microsoft.com/en-us/azure/security/blueprints/pcidss-paaswa-overview>
