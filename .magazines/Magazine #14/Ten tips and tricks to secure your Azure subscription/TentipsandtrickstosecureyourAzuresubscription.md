Laurenz Ovaere

Creating a new Azure subscription can be done in a few clicks but
adopting the cloud in your organization takes more time and effort. At
Xpirit, we help our customers on their cloud journey and one of the
important factors is to make sure that your Azure environment is secure
when migrating your workloads. In this article, we list ten tips and
tricks that are a good starting point to make sure you can benefit from
the possibilities of the cloud in a secure way.

# 1. Protect your Azure Active Directory account with MFA

Azure Active Directory (Azure AD or AAD for short) is the identity
provider for Azure and takes care of the authentication and
authorization scenarios. Every time you access the Azure platform you
will need to pass the AAD authentication in order to prove that you are
really the person you pretend to be. A common way to do this is by
providing your username and password. However, research shows that it is
a lot safer to combine this with multi-factor authentication (MFA). With
MFA you must confirm your authentication in a mobile app before access
is granted to your account.

MFA based on a notification in the Microsoft Authenticator app is a free
option available to any Azure AD user including the free tier. Higher AD
tiers can also choose to receive a phone call, a text message or use a
hardware token as extra verification.

Enabling MFA is no longer enough to protect your environment. The
Microsoft Authenticator app evolves over time in order to avoid new
types of attacks such as MFA bombing. In this type of attack, users are
overloaded by approval requests from an attacker in the hope they click
approve. The number matching feature is one example that avoids
incorrect approvals by requesting a matching number shown on the logon
screen of the user and will be enforced later this year. Showing
additional context information to the user like the origin of the
request is a second example that can further improve the security. This
option needs to be enabled as an Authentication method policy in Azure
AD.

# 2. Enable conditional access or make sure you enable the security defaults on your free Azure AD tenant

The Azure AD security defaults are a set of rules that can be used to
get started with a preconfigured set of security settings in Azure AD.
These are available to all Azure AD tiers including the free tier. In
fact, you need to enable the security defaults to enforce MFA setup on
the Azure AD free tier. This will also block legacy authentication
protocols that are allowed to bypass multifactor authentication. With
the security defaults enabled, you have no control when the MFA is
prompted to the user. The MFA setup is enforced but Azure AD will decide
automatically based on signals like location, device, role and task if a
prompt is appropriate.

In case you want more granular control over when MFA is prompted to the
user, you need to use conditional access, which is available in the
premium Azure AD tiers. This will also give you the ability to block
access from certain non-authorized locations, block risky sign-in
behavior or block access from non-compliant devices. These are all
examples of conditions you can define for certain users or groups before
they get access. Enabling conditional access can improve your setup in
two ways. First, you can be stricter before you grant access, which
limits the attack surface of your environment. If you have no access to
your Azure environment from a non-compliant device neither will an
attacker. Second, you can make a distinction between types of access and
tighten the authentication requirements for administrators compared to
standard users. This will increase your security without too much impact
on all your users.

# 3. Use just-in-time access for tasks that require higher privileges

After successful authentication, Azure role-based access control (RBAC)
is the authorization system that provides fine-grained access control to
Azure resources. The roles assigned to your user profile define which
actions you can take with a certain resource. Administrators typically
have extra roles assigned to their profile to take privileged actions to
Azure resources. The problem is these role assignments are permanent and
so could be used by attackers that have access to your account or open
doors for accidents by yourself.

Privileged Identity Management (PIM) is an Azure AD premium feature
available in the P2-tier that fixes these problems. Azure AD roles with
higher privileges are no longer assigned permanently with PIM but
just-in-time after a request by the user and for a limited duration.
This method comes with a lot of benefits. For example, you can require
extra approvals by other members before the access is granted, you can
enforce an extra MFA prompt or ask the user to enter a reason why they
need this access for the time duration requested. The last one results
in a clear audit history for your environment.

# 4. Use managed identities where possible

Beside users, we also have services that need access to our Azure
resources. For example, an Azure Web App that writes data to an Azure
SQL database, an Azure Function App that reads messages from Azure
Service Bus, etc. Both use cases can be solved by sharing secrets with
our services. However, when relying on secrets we also need to manage
them and store them securely. This is where managed identities for Azure
comes in.

With Managed identities for Azure, our services have an identity
assigned in Azure AD. We can assign specific roles to that identity just
like we do for users with Azure role-based access control (RBAC). The
service itself will use its identity in the background to obtain an
Azure AD token that can be used to access the requested service. The
advantages are that there are no secrets to manage, it is more secure
and free to use.

# 5. Store secrets, keys and certificates in Azure Key Vault

After implementing Managed identities for Azure you won't have a lot of
secrets anymore to manage. However, there are two scenarios that still
require key management. First, services that do not integrate with Azure
AD still use secrets, keys or certificates that you need to store in a
safe place. Azure Key Vault is a well-known Azure service designed to
store these in a safe way.

Second, when you choose to manage the encryption keys for Azure Disk
Encryption yourself with Customer Managed Keys (CMK), you will also need
to store these keys in Key Vault. In contrast, Platform Managed Keys
(PMK) are managed by Azure in the background but provide less
flexibility.

Combining both managed identities and Azure Key Vault makes the solution
even better. An Azure AD integrated service can use managed identity to
access Azure Key Vault and request a secret, key or certificate. That
way our Key Vault is the single dedicated place for secret information
and we can use Azure role-based access control (RBAC) to manage the
access rights.

# 6. Organize your Azure resources effectively to improve your access and policy management

When the number of resources grows, it becomes very hard to control your
role-based access control lists or Azure policies at the resource level.
However, to manage access on a higher level while still applying the
principle of least privilege, you need to combine resources with the
same access rights. Azure resources can be organized on four different
levels: management groups, subscriptions, resource groups and the
resources itself. An effective organization makes it easier to manage,
track costs and secure your resources.

There is no generic way of organizing resources that works for everyone
but here are some rules of thumb to get you started. For application
development, production and non-production resources are typically
managed in different subscriptions to make a clear boundary between both
and ease the access and policy management. Resources owned by separate
teams can be stored in different subscriptions. And lastly, management
groups can be used to group subscriptions that belong to the same
department and share some common access rights or policies.

# 7. Make private networking the default for your Azure resources

When creating resources in Azure, most of the time they are exposed to
the public internet. For example, an Azure Web App, a storage account or
a virtual machine are all accessible over the internet without further
modifications. There are situations where you want this public exposure.
Think about your company's public website. But in general, it is better
to start with private networking and explicitly expose certain
endpoints. By marking these endpoints as public explicitly, you can also
improve the protection if required. Adding Azure Application Gateway or
Azure Front Door for example can give you Web Application Firewall (WAF)
capabilities to further improve the security of these endpoints.

Marking endpoints as public or private is not as easy as it sounds. It
all starts with a good network design. The hub and spoke topology
discussed as part of the Azure landing zones in our previous magazine is
a good starting point. The hub in the virtual network can be connected
to an on-premise network using a VPN or ExpressRoute connection.
Ensuring a safe and private connection. Furthermore, the use of network
security groups, private endpoints, private DNS and VNET integration
will make sure your services use private networking to connect to each
other instead of the public internet. Note that some of these features
are only available in the higher pricing tiers. Part of our services at
Xpirit is to help you finding the right balance between costs and
features.

# 8. Protect your data with encryption

All the previous tips and tricks are here to avoid access of
unauthorized users to your Azure environment. Another extra way to
protect your environment is to make sure your data is not readable to a
potential attacker due to encryption.

At first, we can make use of encryption in transit. This means we will
encrypt data before sending it over the network. The typical example
here is TLS encryption used with HTTPS. By disabling HTTP endpoints or
redirecting traffic to HTTPS, we ensure our data is unreadable to
attackers.

Second, we also have encryption at rest. Azure Storage provides
automatic server-side encryption to storage accounts. This makes the
data unreadable to unauthorized users. Virtual machines running Windows
or Linux can also benefit from encryption with Azure Disks. This feature
lets you encrypt both the data and OS disks.

# 9. Make sure you apply pending updates

Applying updates is an important part of security and when choosing a
cloud platform like Azure, some updates are done by Microsoft. However,
a cloud platform comes with shared responsibilities and there are parts
of your environment that you are responsible for and that you need to
update regularly. It is important to know your responsibilities before
you use a certain service. I will give some examples below.

For Platform-as-a-Service (PaaS) products, as the name suggests the
platform is managed by Azure. For example, you don't need to apply
security patches to the .NET runtime on an Azure App Service. However,
applying package updates to your software is an example of components
you are responsible for. A tool like Dependabot can inform you about
pending package updates. More on this in our previous edition of our
magazine.

Virtual machines are Infrastructure-as-a-Service (IaaS) products that
require more maintenance from your side. The hardware components like
CPU, memory, disks, cooling, etc. are managed by Azure but you are
responsible for the installed software. Azure Automation can already
help you with the update management of your operating system, but the
other software patches require extra action from your side.

# 10. Monitor your environment and improve continuously

At last, even with all security measures in place it remains important
to monitor your Azure environment and improve continuously where needed.
Microsoft Defender for Cloud (formerly known as Azure Security Center)
integrates different security monitoring, compliance checks and alerts
in one single dashboard. This makes it a good place to look for
potential improvements to your environment.

# Conclusion

Hopefully these ten tips and tricks can get you started on your cloud
journey in a secure way. Please reach out if you have questions about
the security of your Azure environment. Our Cloud Security Scan offering
can evaluate the current state of your environment and define potential
security optimizations.
