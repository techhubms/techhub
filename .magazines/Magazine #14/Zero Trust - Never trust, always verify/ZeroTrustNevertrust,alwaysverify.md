# Zero Trust - "Never trust, always verify"

In May 2021, President Joe Biden signed an executive order to adopt the
Zero Trust security model for federated agencies. This has become a top
priority for the US government. Federated agencies have until September
2024 to implement the Zero Trust model. The United States has
experienced firsthand how cyber threats are becoming more sophisticated.
In May 2021, the Colonial Pipeline, an American oil pipeline, fell
victim to a ransomware attack that resulted in a six-day shutdown. The
attack caused fuel shortages, flight rescheduling, filling stations
running out of fuel, and skyrocketing fuel prices.

At the end of 2020, SolarWinds, a system management tool for monitoring
networks and infrastructure, was also hacked. A backdoor was introduced
into the system, allowing hackers to break into the networks of
thousands of organizations, including federated agencies. The scale of
the hack was enormous, making it the biggest hack ever identified. The
SolarWinds hack was a big wake-up call for the US government, leading to
the adoption of the Zero Trust security model as a response.

## What is Zero Trust?

Zero Trust is a security model based on the \"never trust, always
verify\" principle. This means that all users, devices, and applications
must be verified before they are granted access to the network. Zero
Trust is not a product that can be purchased but rather a strategy for
securing solutions. However, some products, such as Azure, can be used
to implement Zero Trust. John Kindervag developed the model in 2010, but
it took almost a decade before the industry started to embrace it. This
was due to a combination of factors, including a shift to mobile and
cloud solutions and the professionalization of cybercrime.

> "Traditionally we would place a firewall in front of our network and
> use implicit trust within our network"

## Why do we need Zero Trust

Times have changed, and we have adopted a more dynamic way of working.
In particular, after Covid, people have shifted from working in offices
to working from home. This means that people access workloads from
unsecured public networks and use different devices such as mobiles or
tablets. Consequently, applications should be available from more than
just the corporate network. Previously, we had only one entry point for
applications within the network. Nowadays, this is no longer feasible,
and we have shifted away from a closed perimeter. As a result, our
infrastructure has become more vulnerable to attacks from different
angles.

Cybercriminals always strive to be one step ahead and are becoming
increasingly intelligent and creative in their efforts to infiltrate our
networks and systems. They seek out weak spots in our security
perimeter, and sometimes those weak spots are the people themselves.
Social engineering has become so sophisticated that psychological
manipulation is used to gain access to high-privilege accounts. We used
to think weak spots were only present in our software and hardware.
However, once a cybercriminal has access to an employee account, they
can gain access to internal systems and valuable data. Zero Trust is a
response to this threat.

## Principles of the Zero Trust Model

**Verify explicitly**

The traditional security model relied on implicit trust, assuming
everything on the network was safe and anyone inside the network had
unrestricted access. However, this assumption is outdated, and we can no
longer rely on the idea that everything is safe behind the firewall.
With Zero Trust, we verify every identity, regardless of whether the
request comes from inside or outside the network. We aim to authenticate
and authorize all data points, as Zero Trust assumes that bad actors can
be found everywhere, including inside your organization.

**Use least privilege access**

Instead of granting sweeping access to identities, Zero Trust principles
dictate that we should provide the least privileged access. Use Identity
Access Management (IAM) to assign an identity only the minimal access
rights required to complete an operation. In many cases, it is not
necessary to give an identity permanent access, especially when dealing
with highly privileged access. Instead, use Just-In-Time (JIT) and
Just-Enough-Access (JEA) mechanisms.

**Assume breach**

Assume that there are malicious actors on the network and take steps to
protect resources accordingly. When dealing with a hack, minimizing the
blast radius is important. One way to achieve this is to isolate
workloads as much as possible through network segmentation. However, be
careful to keep your architecture simple, as complexity can introduce
additional security risks.

## Implement Zero Trust

The five steps to approach Zero Trust.

1.  Define the protect surface. Break down your environment into smaller
    pieces that you need to protect.

2.  Map the transaction flows. Investigate dependencies, inbound and
    outbound connections and how data flows through the network.

3.  Architect a Zero Trust environment. Use the Zero Trust principles to
    design an architecture to protect your protect surface.

4.  Create Zero Trust security policies. Use the Kipling method (who,
    what, when, where, why, how) to develop security policies.

5.  Monitor and maintain. Monitor signals to detect any risks, remediate
    risks and improve the Zero Trust Architecture and security policies.

An organization\'s attack surface refers to the areas in which bad
actors can gain unauthorized access to the network. The attack surface
is typically quite large because the entire internet can be considered
part of it. We refer to the applications or systems that we want to
secure with Zero Trust as protect surfaces. An organization may have
multiple protect surfaces, each containing a DAAS (Data, Applications,
Assets, Services) element. These resources are defined within each
protect surface.

To illustrate how to apply the principles of Zero Trust in practice, I
will use the SmartMoney application from the fictional company
OneFinance as an example. Please note that this article does not provide
an exhaustive list of all Azure services and features that can be used
to protect applications. Instead, the focus is on the SmartMoney
application.

## Use the principles of Zero Trust to secure SmartMoney

SmartMoney is an application developed by the fictional company
OneFinance, which manages financial data for thousands of customers.
SmartMoney helps customers gain insight into their personal finances and
provides advice on becoming financially independent. The customer
service department is responsible for managing all of the customer data.
Based on this data, the expert department provides advice to customers
on how to save costs and create monthly budgets. Two years ago,
OneFinance migrated all of its workloads to Microsoft Azure. Employees
use their Azure AD account to authenticate. The SmartMoney solution is
split into a frontend application and backend application that contains
a set of APIs. Data is stored in Azure SQL and Azure storage account.

During the COVID-19 pandemic, OneFinance, like many other companies,
allowed employees to work from home to prevent business interruption.
Before the pandemic, the application was only accessible from the office
IP address. The list of allowed IPs was extended to ensure employees
could work from home with the application.

OneFinance has many applications, some of which are used internally by
employees, while others are publicly available to customers. SmartMoney
is identified as a protected surface that we want to protect by
following the Zero Trust principles. Other protected surfaces could
include the HR system, the intranet, or the public website.

![](./media/image1.png)

*https://xebiagroup.sharepoint.com/:u:/r/sites/xpirit/Gedeelde%20%20documenten/Xpirit%20Group/Community/Magazine/Magazine%20%2314/Zero%20Trust%20-%20%E2%80%9CNever%20trust,%20always%20verify%E2%80%9D/SmartMoney-before.drawio.svg?csf=1&web=1&e=zDYAeD*

The Zero Trust security model consists of six defense areas: identity,
endpoint, applications, data, network, and infrastructure. Each of these
areas provides a layer of protection. In this article, I will focus on
four defense areas to secure SmartMoney. I will begin with the network
defense area.

## Create a micro-perimeter and use network segmentation

Traditionally, we would have centralized network-based perimeters to
secure workloads in the network. A firewall is placed in front of the
network to keep malicious users outside. Each workload running inside
the network has the same attack surface. In this approach, inside the
network, all requests are trusted. With Zero Trust, we create
micro-perimeters for each protect surface.

We should assume that a breach will occur at some point and that a
malicious user get access to the network. An attacker could gain access
through one of our applications if there is a backdoor or vulnerability
in any of the third-party packages. We should isolate workloads by using
network segmentation to minimize the blast radius. Each workload can be
placed in its own network or subnet, and network security groups can be
used to allow traffic only for specific purposes. All traffic should be
denied by default.

In figure 1.0, you'll find the new architecture for the SmartMoney
application. In the rest of the article, I'll guide you through the
implementation and how each service provides protection based on the
Zero Trust principles.

![](./media/image2.jpg)


https://xebiagroup.sharepoint.com/:u:/r/sites/xpirit/Gedeelde%20%20documenten/Xpirit%20Group/Community/Magazine/Magazine%20%2314/Zero%20Trust%20-%20%E2%80%9CNever%20trust,%20always%20verify%E2%80%9D/SmartMoney-after.drawio.svg?csf=1&web=1&e=NiWhJm

The SmartMoney application is publicly available. However, its access is
limited to IP addresses from the OneFinance office and employees\'
homes. This approach leads to a large attack surface because anyone on
the internet can potentially threaten the application. Even though
enabling VPN is not necessarily a Zero Trust improvement, we want to
reduce the attack surface as much as possible. By enabling VPN, we
ensure that the application is only privately available. However, we
should assume that at some point, a malicious user gains VPN access and
is inside the network. For this reason, the workload should be created
in an isolated VNET and preferably divided into multiple subnets. The
architecture shows that the frontend application is in a separate subnet
from the backend application. A network security group ensures that only
traffic from the frontend subnet is allowed to the backend subnet. Other
workloads from OneFinance, such as the ERP, run in their own VNET. No
traffic is allowed between the ERP and the SmartMoney VNET.

In the new architecture, I follow the hub-spoke model, a commonly used
architectural pattern. The hub is a central point for connectivity, and
all inbound and outbound traffic flows through it. The firewall in the
hub monitors and restricts traffic. Spokes can reuse services placed in
the hub.

**Zero trust principles**

  -----------------------------------------------------------------------
  Verify explicitly   Using network security groups, we can filter all
                      traffic in the network. Security rules allow us to
                      allow or block inbound and outbound traffic for
                      specific IP addresses and ports.
  ------------------- ---------------------------------------------------
  Least privilege     Network security group - service tags help ensure
                      that only AppServices in the frontend subnet can
                      communicate with an AppService in the backend
                      subnet.

  Assume breach       Each workload runs in an isolated network or
                      subnet.
  -----------------------------------------------------------------------

> "Security is a shared responsibility between cloud providers and their
> customers"

By default, PaaS services in Azure are publicly available. The
SmartMoney application utilizes a storage account and SQL database for
storing data. Although Azure provides secure services, it is important
not to assume that just by using Azure, your workloads are secured.
Microsoft explicitly states that security is a shared responsibility
between Azure and the customer.

Azure offers many options to secure your storage account. However, your
storage account may remain unsecured if you fail to make your containers
private or use Shared Keys over Azure AD to authenticate. If your PaaS
service is only used by resources in your VNET, it is recommended that
you use Private Link. Private Link ensures traffic flows over the
Microsoft backbone instead of the internet. A private endpoint and
private DNS zone are created when enabling Private Link. When the
backend application connects to the storage account, Azure detects that
Private Link is enabled. The private endpoint is now used to communicate
with the storage account.

In the architecture of SmartMoney, a new subnet is created for private
endpoints of the storage account and SQL server. Network security groups
are in place to only allow traffic from the backend subnet to the
private endpoint subnet. As a result, resources inside the frontend
subnet cannot reach those services directly.

**Zero trust principles**

  -----------------------------------------------------------------------
  Verify explicitly   By enabling private link, we ensure that only
                      traffic from within the network can access the PaaS
                      services.
  ------------------- ---------------------------------------------------
  Assume breach       If a vulnerability in the front-end application is
                      exploited, the malicious user will not have access
                      to the storage account or SQL server.

  -----------------------------------------------------------------------

We have implemented a micro-perimeter for the SmartMoney application by
placing it inside an isolated network. This will minimize the blast
radius in case of a breach. The different components of the application
are divided into subnets, and traffic is explicitly verified using
network security groups. This was the first step in protecting our
application. The next step is to secure the data.

## 

## Know your Data and secure it

> If you don't know your data, you can't properly secure it.

Data is the foundation of everything we do. Some of the largest
companies rely on data to generate revenue. However, data theft or a
ransomware attack can cause significant damage to these companies and
their end users. The first step in securing data is to discover and
identify the data you have. Once this is done, classify the data with a
sensitivity label so that you know which data requires a higher security
level than others.

Below is an example of data classification for the SmartMoney
application.

  -----------------------------------------------------------------------
  Highly confidential   Customer\'s personal and financial data.
  --------------------- -------------------------------------------------
  Confidential          Advice for customers based on their data.

  General               SmartMoney manuals for new employees.

  Public                Marketing text for the SmartMoney application.
  -----------------------------------------------------------------------

Based on the sensitivity label, we can identify the impact of a data
breach and data losses. Data discovery doesn't have to be a manual task.
Azure SQL includes Data Discovery & Classifications, a feature that
automatically scans your database to identify columns containing
sensitive data. It also monitors and audits query results, labeling them
with a sensitivity label. Based on the recommendations, you should take
action to secure this data.

By default, all data stored by Azure is encrypted at rest. Platform keys
are used to encrypt the data, but it's also possible to use customer
keys (BYOK). You might think good, that means I'm protected against data
breaches? Well no, encryption at rest means that your data is protected
if an intruder gets access to a data center and steals the drive that
holds your data. The data on the disk is encrypted, so useless for the
intruder. Whenever you access data on the disk, for example, when using
a storage account, the data is decrypted so you can use it. From this
point, it's your responsibility to protect the data. One of the first
things we can do is encrypt data in transit. In the SmartMoney
application, we want to ensure that both the frontend and backend
applications run on HTTPS. This protects us from the man-in-the-middle
attack. TLS is enabled for Azure storage accounts by default; it's
impossible to turn this off.

Encrypting data at rest and transit is the bare minimum we should always
do. In some cases, especially with financial data, adding additional
restrictions to sensitive information is essential. Assuming a breach
means considering worst-case scenarios, such as what a malicious user
could do with leaked credit card information.

The SmartMoney application allows employees to import bank transactions
from customers. However, some data, such as the account number, should
never be visible to customer service representatives. One way to achieve
this is by using dynamic masking in Azure SQL. This feature
automatically masks data when it is retrieved through a query. For
example, a credit card number would appear as XXXX-XXXX-XXXX-1234 when
masked.

If attackers gain access to database credentials, they can compromise
the entire database. This would allow the attacker to create a backup or
use SQL Management Studio to access sensitive data. To address this
issue, Azure SQL offers the Always Encrypted feature. With this feature,
data is encrypted at the client using a database driver and then stored
in the database. The data can only be viewed in plain text by the client
application. The data remains encrypted even if an admin accesses the
database using SQL Management Studio.

Data is encrypted using a Content Encryption Key (CEK), which is stored
in the database after being encrypted with a Customer Master Key (CMK).
Typically, the CMK is stored in Azure Key Vault. By using this approach,
sensitive data is protected in the event of SQL credentials being
compromised.

![](./media/image3.png)

[*https://xebiagroup.sharepoint.com/:u:/r/sites/xpirit/Gedeelde%20%20documenten/Xpirit%20Group/Community/Magazine/Magazine%20%2314/Zero%20Trust%20-%20%E2%80%9CNever%20trust,%20always%20verify%E2%80%9D/SQL-always-encrypted.drawio.svg?csf=1&web=1&e=pWXZMY*](https://xebiagroup.sharepoint.com/:u:/r/sites/xpirit/Gedeelde%20%20documenten/Xpirit%20Group/Community/Magazine/Magazine%20%2314/Zero%20Trust%20-%20%E2%80%9CNever%20trust,%20always%20verify%E2%80%9D/SQL-always-encrypted.drawio.svg?csf=1&web=1&e=pWXZMY)

**Zero trust principles**

  -----------------------------------------------------------------------
  Assume breach                       By using Always Encrypted, a
                                      malicious user doesn\'t have access
                                      to sensitive data. Only the client
                                      application can decrypt the data.
  ----------------------------------- -----------------------------------

  -----------------------------------------------------------------------

Ransomware attacks can cause significant damage to an organization. An
attacker gains access to the network or PaaS service and encrypts all
data, making it inaccessible to the organization. The only way for an
organization to regain access to the data is to pay the ransomware to
the hackers. Often, hackers announce a successful attack to the public
to put more pressure on the organization to pay the ransom.
Unfortunately, many organizations start worrying about ransomware
threats when it\'s too late.

When following the Zero Trust approach, we should assume a breach from
the start of a project. This means acknowledging the potential for a
ransomware attack at any point. Therefore, we must protect our data
against such an event. As mentioned earlier, many companies rely on
their data, so protecting data should be their top priority. One way to
secure data is by taking backups. SmartMoney manages many documents for
customers in a storage account. Through Azure Backup Vault, we can take
backups from the blob storage. There are two approaches to taking
backups: operational and vaulted. Operational backups are a local
solution, meaning data is stored locally on the storage account. This
protects data from accidental deletion and corruption. With vaulted
backups, the data is moved and protected in the vault. Usually, with a
ransomware attack, the hacker will try to find backups and make them
unusable. Vaulted backups are stored elsewhere and therefore protect you
from ransomware attacks.

**Zero trust principles**

  -----------------------------------------------------------------------
  Verify explicitly                   Only user accounts with high
                                      privileges can access the vaulted
                                      backups.
  ----------------------------------- -----------------------------------
  Assume breach                       Using Vaulted backups protects us
                                      from ransomware attacks.

  -----------------------------------------------------------------------

Securing data is essential for organizations like OneFinance. One of the
main principles of Zero Trust is to assume a breach and keep it in mind
from the start of the project. In Azure SQL, we protected our sensitive
data using data masks and Always Encrypted. In addition, we used the
Backup Vault to protect data in the Azure storage account from
ransomware attacks. Up next is identity.

## Verify and secure identities

One of the goals of Zero Trust is to eliminate trust. In the past, we
would place a firewall in front of our network and implicitly trust all
users within. However, with Zero Trust, we should trust nobody, whether
they are inside or outside the network. All operations performed by an
identity should be verified to ensure that access is appropriate for
that identity.

In Microsoft Azure, all identities are centrally stored in Azure Active
Directory. OneFinance has several applications, such as SmartMoney, HR
system, ERP, and the intranet. To access each application, users need to
authenticate. We should use Single Sign-On (SSO) whenever possible to
allow users to use their same identity across applications. This
approach makes identities easier to maintain, reduces the security risks
of lost passwords, and provides a better user experience.

Azure Active Directory supports OpenID Connect, OAuth, and SAML for
implementing SSO. Users can view and access applications they have been
granted through the URL
[https://myapps.microsoft.com](https://myapps.microsoft.com/).

![](./media/image4.png)

*https://xebiagroup.sharepoint.com/:i:/r/sites/xpirit/Gedeelde%20%20documenten/Xpirit%20Group/Community/Magazine/Magazine%20%2314/Zero%20Trust%20-%20%E2%80%9CNever%20trust,%20always%20verify%E2%80%9D/my-apps.png?csf=1&web=1&e=z0XlHq*

B**enefits of using SSO**

-   One identity for all applications

-   Withdraw access from one central place and apply for all
    applications

-   Enforce strong authentication across all applications

The Zero Trust model requires verification of all external and internal
requests to ensure security. Insider threats or social engineering
attacks can lead to the exposure of employee credentials. We should
implement strong authentication by enabling Multi-Factor Authentication
(MFA) to prevent a malicious user from using compromised credentials.

> Accounts are more than 99.9% less likely to be compromised if you use
> MFA.

Multi-Factor Authentication (MFA) can be enforced for identities in
Active Directory. This requires users to provide an additional form of
identification. Verification methods include Microsoft Authenticated
App, FIDO2 security key, SMS, and Voice call. MFA can be enabled through
Security Defaults when using Azure AD free or standalone Microsoft 365
license or with Conditional Access when you have an Azure AD Premium or
Microsoft 365 Business. It can be enabled for specific users or groups.
It\'s important to exclude your break-glass account. This special
high-privilege account can be used in an emergency, and you want to
prevent it from being locked out.

After the COVID pandemic, many OneFinance employees continue to work
from home. SmartMoney stores a lot of sensitive data, and the company
must avoid this data falling into the wrong hands. Therefore, we want to
enforce MFA when employees try to log in to the application from outside
the office. Conditional access policies enable us to select which users,
devices, cloud applications, and locations require Multi-Factor
Authentication.

**Zero trust principles**

  -----------------------------------------------------------------------
  Verify explicitly   Enabling MFA provides greater certainty that users
                      are who they claim to be.
  ------------------- ---------------------------------------------------
  Assume breach       If user credentials are compromised the malicious
                      user can't use them because MFA is enabled.

  -----------------------------------------------------------------------

Using the least privileged access is one of the main principles of Zero
Trust. With Privileged Identity Management, you can provide time-based
and approval-based access. Users only get access to complete a specific
task using least privilege access, eliminating sweeping access. For
instance, if a user needs access to a storage account to read files in a
particular container, we only give them (or their group) read access to
the container, not the entire storage account.

When implementing the Zero Trust security model, you must assume a
breach. Let\'s imagine that a user account was compromised, and the
intruder used that account to access the storage account. If the least
privileged access principle wasn\'t followed, the intruder would have
access to the entire storage account, with all its consequences.
However, by only assigning access rights to complete specific tasks, the
blast radius of the breach would be minimal.

The customer service team manages financial information from customers
in the SmartMoney application. They create new records, update data, and
remove irrelevant information. The financial expert team can utilize the
reporting feature to analyze this data and provide personalized advice
to each client. The customer team should only have permission to create,
modify, and delete records, while financial experts should only have
permission to access the reporting feature.

Occasionally, customer data may need to be removed in SmartMoney, but
it\'s important to limit who can do so. PIM allows for the
implementation of Just-in-Time (JIT) access. Instead of granting
permanent access to an identity, an identity can be made eligible for a
role. If a user needs access, they must activate the assignment and
provide justification for why they need access. They also choose how
long they need this role. A manager must approve the assignment. Once
the time period has expired, the assignment is automatically removed. To
use PIM, an Active Directory P2 license is required.

![](./media/image5.png)

*https://xebiagroup.sharepoint.com/:i:/r/sites/xpirit/Gedeelde%20%20documenten/Xpirit%20Group/Community/Magazine/Magazine%20%2314/Zero%20Trust%20-%20%E2%80%9CNever%20trust,%20always%20verify%E2%80%9D/pim-assignment-activation.png?csf=1&web=1&e=6HT8Kr*

Implementing Just-in-Time (JIT) and Just-Enough-Access (JEA) provides
greater control over who has access to what and when. Typically, this is
configured only once. However, employees may receive promotions or
switch departments, so scheduling access reviews regularly in Privileged
Identity Management (PIM) is essential. We can check if access rights
are appropriate for each user through these reviews.

**Zero trust principles**

  -----------------------------------------------------------------------
  Least privilege                     Enabling JIT (Just-in-Time) and JEA
                                      (Just-Enough-Access) with PIM
                                      ensures that users only have access
                                      to complete a specific task for a
                                      short period of time.
  ----------------------------------- -----------------------------------

  -----------------------------------------------------------------------

> From January 2021 through December 2021, we've blocked more than 25.6
> billion Azure AD brute force authentication attacks - Microsoft

The number of login events can be massive, with users logging in to
access applications every day and sometimes multiple times per day. This
can result in millions of login events. Azure Identity Protection allows
us to monitor user sign-in patterns and detect risks such as anonymous
IP addresses, atypical travel, new countries, malware-linked IP
addresses, unfamiliar sign-in properties, leaked credentials, or
password spray.

With Identity Protection, you can enable a user risk policy to detect
compromised accounts or a sign-in risk policy to detect unusual
behavior. However, the number of signals can be overwhelming, and
removing false positives can be difficult. It\'s possible to automate
the response to risk detections, such as enforcing multi-factor
authentication (MFA) when a sign-in risk is detected. For example, if a
user signs in from a different country, we can ask that user for an
additional authentication step by enforcing MFA. This allows users to
self-remediate detected risks and stay productive without overwhelming
administrators with sign-in issues. In case of emergencies, a break
glass account can be used, but this can also lead to detected risks.
Fortunately, excluding users like the break glass account from the risks
policy is possible. Administrators can view sign-in, and user risk
reports in the Azure portal. To remove false positives, it\'s possible
to remove IP address ranges or countries from the detected signals. To
use Identity Protection, you need an Azure AD P2 license.

Securing identities is one of the most critical defense areas of Zero
Trust. Identities have access to applications and sensitive data. We
have enabled single sign-on (SSO) and enforced Multi-Factor
Authentication (MFA) by using conditional access. Privileged identity
management (PIM) ensures that we follow the least privilege access
principle of Zero Trust.

Misconfigurations are the leading cause of data breaches. In the next
section, I will discuss how to prevent and detect security risks in our
infrastructure.

## Use signals to protect your infrastructure

When implementing the Zero Trust model, we aim to monitor all traffic
going to and from a protect surface and remediate risks. We should not
view security as a one-time project but rather an iterative process. All
traffic is logged, and based on these logs, we can enhance security to
become more robust. Introducing new workloads can create new security
risks, so consistently monitoring the entire infrastructure for risks is
essential.

When developing solutions in the cloud, the number of signals that are
collected can be overwhelming. It is impossible to monitor all of these
signals and remediate risks manually. Azure provides Defender for Cloud,
which identifies and remediates risks across subscriptions. Defender has
capabilities for cloud security posture management (CSPM) and cloud
workload protection platform (CWPP). It constantly scans subscriptions
and resources for security issues. A security score is provided based on
identified security risks and recommendations. The higher the score, the
better your cloud environment is secured. CWPP protects workloads from
threats. Defender provides plans for servers, containers, databases, and
storage. Configured workloads are scanned, and risks are reported based
on their security level.

Defender for Storage analyzes the telemetry generated by blob storage.
Based on this data, alerts are triggered. Telemetry includes operations
on blobs such as create, update, and delete. This doesn\'t impact the
performance of the storage account. Detected risks include unusual
access to an account, malicious content uploads, data encryption,
unusual data extraction, etc. Azure uses a technique called reputation
analysis to detect malware. It means that files are hashed, and based on
that, the likelihood is calculated if that hash is malware. When risks
occur, alerts are triggered. It is essential to investigate alerts and
check for any false positives. For instance, an alert could be triggered
if a lot of data is downloaded at once. However, this might be an
employee with a valid reason.

> Through 2022, at least 95 percent of cloud security failures will be
> the customer's fault.

Azure uses the [blob.core.windows.net](http://blobl.core.windows.net/)
endpoint to create blob storage accounts. Publicly available storage
accounts are easy to discover; simply search for
\"site:\*.blob.core.windows.net\" on Google and you will get over 3
million results. A sophisticated hacker can easily write a script to
find publicly available storage accounts, query for the containers and
blobs, and find valuable data. From there, the hacker can discover the
company and target them to gain access to the access keys. Once they
obtain access, they have an entry point into the organization and can
infiltrate further by uploading malware. Defender for Storage detects
publicly available storage accounts and fires an alert. This is great
because we can remediate that risk immediately.

![](./media/image6.png)


*https://xebiagroup.sharepoint.com/:i:/r/sites/xpirit/Gedeelde%20%20documenten/Xpirit%20Group/Community/Magazine/Magazine%20%2314/Zero%20Trust%20-%20%E2%80%9CNever%20trust,%20always%20verify%E2%80%9D/defender-for-storage-alert.png?csf=1&web=1&e=8fGJDb*

This provides us with great information about our workloads in real
time. However, prevention is better than detection. We should detect any
misconfigurations before deploying them to production.

> Define the desired state of your infrastructure using code and scan
> for misconfigurations.

Using Infrastructure as Code (IaC) has many advantages, such as reducing
the risk of human error, ensuring consistency, enabling automation, and
saving costs. By implementing a good DevOps pipeline that includes pull
requests and reviews, faults can be detected early. Terraform is an
open-source IaC tool that works with many cloud services. The desired
state of the infrastructure is written in code and can be easily
deployed. Because Terraform is open-source, many extensions are
available for use. Security tools like tfsec and Checkov can scan
Terraform code for misconfigurations and security issues, allowing us to
provide engineers with feedback early in the DevOps pipeline.

![](./media/image7.png)

*https://xebiagroup.sharepoint.com/:i:/r/sites/xpirit/Gedeelde%20%20documenten/Xpirit%20Group/Community/Magazine/Magazine%20%2314/Zero%20Trust%20-%20%E2%80%9CNever%20trust,%20always%20verify%E2%80%9D/iac-security-alert.png?csf=1&web=1&e=FswYDA*

When we design a Zero Trust architecture, we want to do this on an
organizational level. The standards that are defined apply to all
workloads. With Azure Policies, we can govern our Azure resources from
one central place. For instance, using access keys is a potential risk
because anyone can use those access keys to access the storage account.
To ensure that storage accounts don't use access keys, we can activate
the policy: "Storage accounts should prevent shared key access". We can
deny or audit the resource depending on how the policy is configured.
When creating a new policy, you first want to use the audit option to
test which resources are affected. If you start using deny, you
potentially could block teams.

For the SmartMoney application, we have already enabled Private Link for
the storage account to ensure that the storage account is only reachable
in the VNET. OneFinance has many workloads, and some are using storage
accounts. To ensure that all storage accounts created in subscriptions
use Private Link, we can use the built-in policy 'Configure Storage
account to use a private link connection\'. This policy will
automatically configure Private Link if it's not deployed for a storage
account. Policies can be created at the management group, subscription,
and resource group level. OneFinance wants to enable it for storage
accounts in all subscriptions, so the best place to create this policy
is at the management group level.

Misconfigurations are the leading cause of data breaches. To prevent
this, it is recommended to use a defense-in-depth approach by creating
multiple layers of security. It\'s a best practice to implement both a
prevention and detection mechanism.

1.  Prevention - Declaratively define infrastructure using Terraform.

2.  Prevention - Use tools such as tfsec and Checkov to detect risks and
    security improvements early in the DevOps pipeline.

3.  Prevention - Use pull requests and reviews.

4.  Prevention/ Detection - Azure policies help align infrastructure
    with your organization\'s policies.

5.  Detection - Defender for Cloud to analyze signals, detect, and
    remediate risks.

## 

## Closing words

The world has changed, and we must accept that bad actors are
everywhere. Dynamic work environments, smart devices, and increasingly
sophisticated cybercriminals increase the attack surface. To protect
ourselves, we need a new security approach: Zero Trust. This approach
eliminates trust and assumes breach.

With Zero Trust, we don\'t trust anyone, not even our employees. In this
article, I explained the main principles of Zero Trust, why we need it,
and how to implement it. Using an example application, I demonstrated
how to follow Zero Trust principles using services in Azure. Note that
this was not a comprehensive list of all Azure services and features you
can use, but it should give you an idea of why we should use them.
