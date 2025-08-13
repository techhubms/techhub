By Loek Duys

 

As organizations move more and more operations to the cloud, ensuring
these operations run securely is crucial. We use hardware tokens,
complex passwords, One-Time-Passwords, and authenticator apps to
authenticate human accounts. The question remains: how do we securely do
this with system accounts? In this article, I'll walk you through the
options and give examples of how to use each option best.

# The Principle of Least-Privilege

 

Before diving into more details, knowing about the least-privilege
principle is essential.

The principle of least privilege is an essential aspect of security in
the cloud. It involves granting the minimum level of access necessary to
perform a specific task. Minimizing permissions helps reduce the risk of
security breaches and unauthorized access to sensitive information.

## System accounts in Azure 

Regarding system accounts in Azure, applying the principle of least
privilege requires careful consideration of the level of access each
account requires. For example, you may have a system account that only
needs access to a specific subset of resources, such as read-only access
to a database. In this case, granting full administrative access to the
account would be unnecessary and increase the risk of security breaches.

 

# Three types of Identity in Azure

 

To authenticate services to other services running inside Azure, you can
choose from various options, such as Service Principals, Managed
Identities, and Federated Identities. Each has benefits and drawbacks.

## Service Principals

Using a Service Principal was the earliest method to authenticate
systems to Azure Active Directory (AAD). A Service Principal is an
identity created for use with applications, services, and automation
tools to access specific Azure resources. You use them to authenticate
and authorize applications to access specific Azure resources. Service
Principals are similar to user accounts, but you use them for
non-interactive scenarios. To authenticate using a Service Principal,
you must provide the Client's Identifier and a Client Secret or Client
Certificate. Both passwords and certificates have an expiration date, so
your authenticating system needs to be able to deal with secret
rollovers. The authenticating system does not need to run inside Azure;
it can run anywhere as long as internet access is available. You can see
the way this works in Figure 1.

![](./media/image1.png)


Figure 1: Using a Service Principal

### When to use

Use this approach when you have complete control over the system
requesting access. For example, Service Principals work very well when
creating resources in Azure using a GitHub Actions pipeline. GitHub
Actions has built-in functionality to pass the Client Secret of your
Service Principal to the tasks that create the resources. You have
complete control over both systems, making this a viable option. Make
sure to assign the proper rights to the Service Principal, for example,
by assigning it the Azure Role Based Access Control (RBAC) role
'Contributor' at the scope of a resource group or (at most)
subscription. Having the Contributor role will allow the Principal to
create resources but not access the data stored inside the resources,
nor does it allow the assignment of roles.

## Managed Identities

A Managed Identity is a Service Principal managed by Azure. You can use
it to authenticate specific Azure resources to access other Azure
resources. The main difference with regular Service Principals is that
you don't need to store credentials to use them; Azure manages this and
secret rollover for you. The downside of Managed Identity is the
authenticating system must run inside Azure to get a Managed Identity
assigned.

### When to use

Managed Identities can only be assigned to Azure resources, limiting
their use to Cloud services running within Azure. In my opinion, you
should rely on Managed Identities as much as possible for authentication
between Azure services. For example, you can configure a Managed
Identity with an Azure Web App and allow it access to an Azure SQL
Database, as displayed in Figure 2.

![Diagram, letter Description automatically
generated](./media/image2.png)


Figure 2: Example of Managed Identity

Similarly, you can use Managed Identity to allow your Web App to access
services like Storage Accounts, Key Vaults, Service Bus, etc. Most
popular Azure services support Managed Identity nowadays.

The Managed Identity connected to the Web App must be allowed to access
the Azure SQL Database. As with Service Principals, you can do this
using Azure RBAC. In this case, the Principal needs to be allowed to
access data but not to modify the resource itself. You can do this by
creating a SQL user inside the database to log in using the Managed
Identity. Next, you need to assign database-specific roles to it. This
way, the Managed Identity can only be used for a single purpose, with
minimal privileges.

## Federated Identities

A Federated identity in Azure is authenticated to AAD using and trusting
an external identity provider (IDP). You first authenticate a system
with the external IDP and gain access to Azure resources through
Identity Federation. When two IDPs are federated, one IDP trusts tokens
issued by the other.

A Federated identity allows you to access Azure resources running
outside Azure without needing Service Principal credentials. Because you
are using an external IDP, you can also control the way it issues access
& identity tokens, which opens up interesting scenarios. For example,
you could let a locally running containerized IDP issue a token for a
Service Account in Kubernetes. You can configure Azure AD to trust the
containerized IDP and use its token to authenticate a Service Principal.
It turns out this scenario already exists under the name' Workload
Identity'. You can see how it works in Figure 3.

![Diagram Description automatically
generated](./media/image3.png)


Figure 3: Example of federated Identity

### When to use

Using federated Identity works well when running workloads in Azure
Kubernetes Service. But the mechanism also works outside of Azure. It
doesn't matter where your code runs as long as you have correctly
configured Azure Active Directory, and AAD can reach your local IDP's
metadata endpoint over the public internet.

Federated Identity can also work when allowing systems you do not own to
access your AAD-controlled resources. Unfortunately, Microsoft does not
allow the remote IDP to be Azure Active Directory. Other than that, you
can configure trusts to any remote OAuth-compliant IDP. Imagine you have
a business partner who wants to access your Web API, protected by AAD,
from one of their systems. They protect their systems using Duende's
Identity Server platform. Instead of providing them with (expiring)
credentials of a Service Principal, you could set up Federated Identity
as described in Figure 4.

![Diagram Description automatically
generated](./media/image4.png)


Figure 4: Business federation example

# Conclusion

 

The move to the cloud brings a new set of security challenges. However,
by understanding the various options available in Microsoft Azure and
Azure Active Directory, you can secure your workloads and prevent an
identity crisis. Whether you are using Service Principals, Managed
Identities, or Federated & Workload Identity, applying the
least-privilege principle to reduce the risk of security breaches and
unauthorized access to sensitive information is essential.

*Images generated using https://www.websequencediagrams.com/*

Notes not part of the article:

title Example of Service Principal

Workload -\> Azure AD: Client Id + Secret/Certificate

Azure AD -\> Workload: Provide AAD access token

Workload -\> AAD Protected Resource: Access protected resource using
token

title Example of federated Identity

Kubernetes-\>Pod: Provide service account based token to Pod

Pod-\>Azure AD: Send local token to request AAD token

Azure AD-\>Kubernetes: Validate token using Kubernetes metadata endpoint

Azure AD-\>Pod: Return AAD access token

Pod-\>Azure Resource: Access Azure Resource using AAD access token

Example of using Managed Identity

note left of Azure Web App : Preconfigured with Managed Identity

Azure Web App -\> Azure AD: Get token using Managed Identity

Azure Web App -\> Cosmos Db: Access database using AAD access token

title Another example of federated Identity

Identity Server-\>Workload: Provide process based token

Workload-\>Azure AD: Send local token to request AAD token

Azure AD-\>Identity Server: Validate token using metadata endpoint

Azure AD-\>Workload: Return AAD access token

Workload-\>Web API: Access Azure Resource using AAD access token
