Identity Access Management in Microsoft 365\
\
\
Where two worlds meet: Identity Access Management in Microsoft 365 and
Azure. A deep dive!

# Introduction

With the increasing importance of cloud services, the way of accessing
IT assets has changed. Assets are stored in the cloud and digital
identities (IDs) are becoming more used outside the corporate network to
access these assets. In corporate networks, one or more firewall systems
are used as a perimeter to protect access to IT assets. But what if you
are outside the corporate network? In the home office, the firewall in
the DSL router provides a basic protection against cyberattacks. For
users it is not clear what protection measures are taken in public
networks, such as a public WLAN at an airport or in a hotel. You have to
assume that public networks don\'t offer protection for a digital ID.
This is one of the main Identity Access Management (IAM) principles of
the Zero Trust Model. For more information about Zero Trust I want to
refer to the article "Zero Trust - "Never trust, always verify"" in this
magazine.

To be clear: the usage of IDs is no longer sufficiently protected
outside the corporate network and access to IT assets is inevitably at
risk. These attacks are not only a problem of private individuals, but
also of companies.

The ID (and the devices used by the identity) must be declared the new
perimeter and protected when used.

This is a very important task of an IAM system, that deals with two
management aspects:

1.  the management of digital identities (IDs)

2.  the management of these identities\' access to IT assets (resources
    such as files, emails, web pages, databases, etc.).

# Microsoft IAM

To manage digital IDs and accessing IT assets, the IAM ecosystem in
Microsoft 365 (M365) offers the following features:

-   a platform with a lot of functionality for managing IDs (objects)

-   a database for storing digital IDs (objects and their attributes)

-   ID authentication and authorization capabilities

The directory service Azure Active Directory (AAD) is the basis for all
these features and can manage both the Authorization (AuthZ) and the
Authentication (AuthN) of users:

![](./media/image1.png)


Figure 1: AuthN and AuthZ

Beyond these technical features, governing digital IDs (ID Governance)
and accessing IT assets (Data Governance) is another added value that
M365 delivers.

![](./media/image2.png)


Figure 2: Components of an IAM Solution

The Directory Service (DS) component provides the basis for storing the
digital ID in a database. Since some companies use several directory
services, special DS functions ensure that a federation is established
between the different directory services, e. g. to be able to
synchronize the objects afterwards. This is also the case when using an
on-premises Microsoft Active Directory (AD), which is connected with AAD
in the cloud to form a hybrid cloud infrastructure.

Single Sign On (SSO) in AuthN is used so that the user does not have to
re-authenticate each time when accessing IT assets. Multi-factor
authentication (MFA) is used as an additional security layer, in which
an additional secret is required from the user to which only he or she
has access. A token-based procedure is used to authenticate the session,
i. e. the connection to an IT asset. One of the newer features of AuthZ
is to handle authentication without passwords (Passwordless
Authentication). This functionality should please users, as they often
have to remember dozens of passwords to gain access to IT assets.

AuthZ is used to control resource access. In addition to role-based
access, attributes of the user object or certain conditions are often
evaluated. These methods are called Role Based Access Control (RBAC),
Attribute Based Access Control (ABAC), and Conditional Access (CA). For
managing privileged access there is a special form, the so-called
Privileged Access Management (PAM), often incorrectly referred to as
Privileged Identity Management (PIM). PIM is about managing identities
for administrators or superusers that control highly privileged access
to IT assets. PAM deals with the management of accessing valuable IT
assets such as access to privileged accounts from administrators.
Exploring PIM and PAM in M365 will be covered in a future magazine.

Whether PIM, PAM or IAM, the principle of Least Privileged Access always
applies, which is also a principle of the Zero Trust Model: at the time
of access, the digital ID receives a minimum of the rights and
permissions required to access the IT asset. In addition, these rights
and permissions are only granted during the needed time of access itself
and are automatically removed afterwards. This is also called
Just-In-Time (JIT) and Just-Enough-Access (JEA).

Please refer also to the article "Ten tips and tricks to secure your
Azure subscription" in this magazine. You'll find good tips to harden
your IAM infrastructure in M365 and Microsoft Azure.

In the age of automation, user management services are an important
component of an IAM system. This is not just about the automated
creation -- provisioning -- of digital IDs, but about the entire
identity lifecycle. This starts with hiring a new employee and goes over
to the change of access due to a change within the company, then to a
time-out, e. g. a sabatical and ends with leaving the company. Some of
these processes should be offered as self-service so the user can
request new access to IT assets themselves. This also includes changing
a password, which unfortunately still occupies the IT support
departments of companies far too much.

## DS - AAD

Microsoft Active Directory (AD) is probably the best-known and for many
years the most widespread identity store worldwide. However, AD could
not really be described as a mature IAM system, because too many
activities on domain controllers (On-premise Windows Servers that hold
the AD database) had to be carried out manually. Only complex automation
through code development and attaching of intelligent solutions from
third-party providers could add a certain variety of functions to the AD
. This has changed dramatically with Azure Actice Directory (AAD) as a
\"virtual\" counterpart of AD.

With AAD as an identity store, Microsoft has for the first time in
recent years provided a complete solution for IAM, PAM and PIM as a
cloud service in M365 and Microsoft Azure. With proper AAD licensing,
features such as MFA, SSO, Passwordless Authentication, Password hash
synchronization, Pass-through Authentication (PtA) and Conditional
Access (CA) can be enabled with just a few clicks.

Microsoft has declared war on identity theft with these features. Let's
take a closer look at what exactly is behind all the functions and which
services can be used within the IAM ecosystem in the Microsoft Cloud.
The main focus will be on the components AuthN Services and AuthZ
Services.

## AuthN Services & Components

Hybrid Identities

When companies operate in a hybrid world -- on-premises and in the cloud
-- they often prefer to use on-premises AD as the source of truth for
AuthN requests. This works best if the on-premises AD is coupled with
the AAD with Microsoft\'s own synchronization service Azure AD Connect.
In most cases, AD FS (Active Directory Federation Services) is used as a
coupling system. This allows the Microsoft Cloud to send its AuthN
requests from the customer\'s tenant to the domain controller in the
corporate network. AD FS can be published to the Internet via a Web
Application Proxy (WAP Server). The weak point is the availability of
the systems for establishing the federation (AD FS) and the WAP systems.
Without resiliency, authentication is at risk and will stop working if
the systems fail to function. A remediation could be:

-   Password hash synchronization or

-   Pass-through-Authentication (PtA)

With PtH, the cloud takes over the authentication of digital IDs.
Password hash synchronsation and PtA works without the use of AD FS and
WAP. Those are simpler variants than the usage of AD FS and WAP. Anyway,
both variants and the use of AD FS and WAP are still recommended. Since
AD FS and WAP require very complex and failsafe infrastructures,
Microsoft will probably say goodbye to it in the long run. Hopefully
customers will follow suit. When it comes to hashes, many people still
think of the Mimikatz attack, a password stealing method by which the
hash of a password can be successfully reused if the right protective
measures have not been taken. For peace of mind: the password hash is
double-hashed before it is synchronized via AAD Connect to the cloud.
The following figures are intended to illustrate the differences between
password hash synchronisation and PtA:

![Ein Bild, das Diagramm enthält. Automatisch generierte
Beschreibung](./media/image3.png)


Figure 3: Password hash synchronization

![Ein Bild, das Diagramm enthält. Automatisch generierte
Beschreibung](./media/image4.png)


Figure 4: Pass-through Authentication

Cloud Native Identities

The best and most secure option is achieved with cloud native
authentication capabilities, which can be supplemented with additional
security measures from an Identity Provider (IdP) such as AAD. Cloud
native authentication only needs access to the cloud. Connections to or
from domain controllers in the corporate network are no longer required,
nor is there any synchronization of user objects. The user objects are
therefore provisioned directly in the cloud and only stored once. This
differs to a hybrid identity, which exists in both the on-premises AD
and the AAD.

Multi Factor Authentication (MFA) in M365

MFA is offered in M365 as well as in AAD in various forms:

-   Legacy MFA

-   Azure AD MFA.

Both variants are included in most subscriptions based on the free tier
of AAD. Legacy MFA is enabled directly on the user object. However, we
recommend the variant that activates MFA for all user objects by
activating the security standards in AAD or Azure Portal (see the
article "Ten tips and tricks to secure your Azure subscription" in this
magazine). Unfortunately, exceptions cannot be made here, and this
option is not available with the Conditional Access (CA) baseline. These
must then be deactivated beforehand. Enhanced CA security policies, in
turn, require disabling security standards and additional licenses
(Azure AD P1 or Azure AD P2) to enable conditional access policies and
get the full functionality for IAM.

Conditional Access in M365

Conditional access relies on signals to make decisions for general
access and enforce finegrained access to IT assets. The whole thing
works in real time and is one of the greatest achievements in IAM.
Policies can be assigned to individual users, user groups or the entire
company and are super flexible. With policies in monitoring mode, the
effects can be checked in a simulation before the actual assignment. It
would not be the first time that administrators lock themselves out of
the tenant because of a policy that is too restrictive. Therefore,
exactly two phases are supported:

1.  Collect session details i. e. about the user, the device or the
    location

2.  Enforce a CA policy in realtime

![Ein Bild, das Diagramm enthält. Automatisch generierte
Beschreibung](./media/image5.png)


Figure 5: Conditional Access

The downside: Microsoft get's paid for the complexity of the policy with
the different requirements for licensing.

Future of Identity in M365

Microsoft Entra is a family of products and a very young representative
of services within the Microsoft IAM ecosystem. Entra offers a platform
with which digital IDs can be subject to governance and permissions.
Furthermore the access can be monitored and managed. So far, nothing new
for an IAM system. With the Verify ID service, however, there is a new
service offering, cross-cloud for multi-cloud environments, for a
managed verifiable credential service. It is based on open standards.
Not only will employees benefit, but also customers and partners who
previously had to come up with their own verifying process for IDs.

How does it work?

Verified ID automates identity verification based on open standards and
supports privacy-compliant interactions between businesses and users.

![](./media/image6.png)


Figure 6: Verifiable Credentials

## AuthZ Services & Components

The complete ecosystem in Microsoft Azure and M365 for assigning rights
and permissions is based on the RBAC principle. Pre-builtin roles (I
guess if you take all services into account, you get several hundreds in
number) with fix assigned rights & permissions allow administrators to
assign users the appropriate access permissions. This is done by a user
becoming a member of a role group. The role group in turn is assigned
the roles that have rights and permissions. The rights and permissions
are inherited to the user. Roles are dispearsed over M365 services and
AAD. Some are used in AAD and the service, some are unique in AAD only
and not visible to the service and some are used by more the one
services. This can be very confusing.

![Ein Bild, das Diagramm enthält. Automatisch generierte
Beschreibung](./media/image7.png)


Figure 7: Role-based authorization assignment in M365

You can very quickly get lost in the depths of rights and authorization
administration in the Microsoft 365 Admin Center (see figure Figure 8:
Role Assignments in M365). It is advised to get along with the roles and
role groups proposed by Microsoft. This is especially true at the
beginning of the M365 cloud journey. The existing roles are sufficient
for the delegation of special rights and authorizations and can be
assigned via policies.

![Ein Bild, das Text enthält. Automatisch generierte
Beschreibung](./media/image8.png)


Figure 8: Role Assignments in M365

However, a role-based access concept is needed that makes the control
and coordination of access to IT assets transparent and reflects the
current status. If your company is subjected to a security audit, e.g.
according to ISO 27001 or BSI Grundschutz, the auditor can ask you about
this concept. So remember that you are writing one. The HR-based
lifecycle plays an important role: administrators do not want to
implement role changes on demand. These should be requested by the user
based on self-service or automatically adapted based on a job or
department change. Then the trigger has to come from the HR department.
The principle of SoD -- Segregation of Duties must always be observed:
Administrators may not add or remove themselves from certain role
groups. This requires at least one other administrator to monitor and
implement the process.

## User Management

To be honest, there are not many great features in the area of automated
provisioning and deprovisioning of user objects. However, this is
increasingly due to the fact that the creation of new user objects, e.
g. as part of a new hire, is not an independent discipline of IT. This
is based on the HR-based lifecycle. Typically, it is the HR department
who has to be the first to take action when it comes to the maintenance
of master data for people. Only when a master date is successfully
created, e. g. in SAP, can a trigger can be fired. This automatically
creates a user object with all the required attribute values in the DS
database and provisions the user object. Two processes exists, the
HR-Based Lifecycle and the Identity Lifecycle, which absolutely should
not be decoupled from each other.

In IT, we use IAM to focus on the identity lifecycle rather than the
HR-based one. However, we must ensure that interfaces for provisioning
and deprovisioning from master data maintenance systems are correctly
addressed. The initial filling of the identity store with digital IDs is
therefore not the sole responsibility of IT. It always requires a
fireing trigger from the master data maintenance systems, especially
since licensing in Microsoft 365 is user-based. License management, like
IAM, is therefore dependent on the correct maintenance of user master
data.

Self-service Password Reset (SSPR) by the user is old hat. Especially
with this self-service offer, IT support departments can be
significantly relieved. According to Gartner, these requests accounted
for 20% of all requests from employees if a self-service password reset
cannot be used. There are several reasons that require a password reset,
including:

-   User objects are locked

-   Users have been forgotten their passwords

-   Passwords have expired and were not renewed in time

-   Password changes by the user failed

The following self-service portal is available to the user in Microsoft
365 and Microsoft Azure. These portal can be reached via
myaccount.microsoft.com by the user. Security info and paswords can be
managed here:

![](./media/image9.png)


Figure 9: User Self-Service Dashboard

![Ein Bild, das Text enthält. Automatisch generierte
Beschreibung](./media/image10.png)


Figure 10: Manageing security information

However, SSPR must first be enabled in the Entra Admin Center:

![](./media/image11.png)


Figure 11: Enabling SSPR

Starting in January 2024, legacy policies for multi-factor
authentication and self-service password reset will be discontinued.
From then on, all methods will be managed via authentication method
policies, including passwordless authentication:

![An image that contains text. Automatically generated
description](./media/image12.png)


Figure 12: Configuring Authentication Methods

# Summary

In summary, there are five steps for securing the identity
infrastructure via an IAM system that are very helpful.

Before you begin your journey to protect identities and use enhanced IAM
solutions, you should protect not only privileged accounts but also all
user accounts via MFA.

Step 1 - Strengthen your credentials, especially password length

Step 2 - Reduce your attack surface area, especially using modern
authentication protocols and controlling entrypoints for authentication

Step 3 - Automate threat response, especially by using conditional
access policies

Step 4 - Utilize cloud intelligence and automate IAM processes,
especially by monitoring Azure AD

Step 5 - Enable end-user self-service, especially for self-service
password reset (SSPR) and configuring security information

This could avoid approximately 90% of all successful cyberattacks on
digital identities.
