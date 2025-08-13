**Put teams in control with Azure Security as Code**

DevOps is the current trend in software development where autonomous
teams should own the full life cycle of a product. In short, this means
they need to build and run the product they own. Teams become
responsible for writing the code, managing the infrastructure,
monitoring the application health, and supporting the product. By using
autonomous DevOps teams, organizations hope to be able to respond to the
ever-changing demands of their clients and be able to differentiate from
their competitors.

To enable teams to become autonomous and good owners, a heavy use of
automation is key. Build automation, release orchestrations, and
Infrastructure as Code are becoming mainstream and essential for
development teams. There are many tools and processes to enable teams to
do this. But there is one specific area for which very few teams have a
good automated solution. And that is the management of permissions and
security. This has several reasons. One of the most commonly heard is
that security keys should not end up in the hands of developers because
of obvious risks.

**What are the options for DevOps teams to perform permission
management?**

But how DO we enable DevOps teams to manage permissions? There are
several options to consider and let\'s take a look at a few examples for
a team working on Azure.

Firstly, teams could utilize the Azure Portal to manually manage the
permissions. That might work for small organizations and startups, but
as soon as you start to scale up, or when audit regulations come in to
play, the portal is very limited in functionality. It does not support
audit trails, and it requires high privileges for many users. And by
allowing certain users to be Azure subscription owners, the risk is that
they might break other infrastructure by carrying out manual changes.

The most important downside is that this approach requires a lot of
manual labor and it is impossible to reproduce the current state if you
would need to recreate your entire infrastructure.

**Hello ticket system!**

When permissions need to be limited to fewer people, a second solution
that is being used a lot in non-DevOps cultures is a ticketing system.
These kinds of solutions help in streamlining the process but have some
major downsides. When teams with the special privilege of setting these
permissions have to support many teams, or get a large number of
requests, queues might occur and thus these requests will block the flow
of development teams.

Another downside is that these forms and tools are far away from the
code and tools the development teams love to use. And last but not
least, there is only a paper audit trail of what was requested, but this
has no relation with what actually happened. There is no way to recreate
the environment from the audit trail in the ticket system.

**Automation is key**

The best option is to use automation for these kinds of repetitive
tasks. We\'ve seen several companies build their own self-service
portals and have even helped some of them release these types of
solutions. These self-service portals can streamline the permission
requests in a far superior way compared to ticketing systems, because
all requests are automatically handled by the application instead of
ending up in a queue for a human to process them. This automated process
can be extended to match your own requirements.

However, an automated self-service portal also has some downsides.
Building such an application requires quite some coding effort, and
since we\'re working with permissions and access, security bugs can be
easily introduced. And, if you want to build more complex requirements
such as audit trails or 4-eyes principles, your application becomes more
complex, which comes with substantial maintenance effort. Another
downside of automated self-service portals is something it shares with
the ticketing system: It is still a portal somewhere on the
inter/intranet that is far away from the tools the developers like to
use.

**Our Goal: Rethink the way in which development teams interact with
organizational silos.**

Permission management is one of the many examples where development
teams have a dependency on another team, department or system. Other
examples are firewalls, identity management, and computing resources. In
our case, we wanted to build a better solution for development teams to
perform permission management, but similar solutions can be created for
other scenarios.

In our scenario, we wanted to create a solution for development teams
that would tackle the difficulties of managing permissions with a number
of key goals in mind:

**A solution that works for developers**

Our solution should be something that is close to developers, integrates
in their workflow, and does not require all kinds of web applications
that are located somewhere on the inter/intranet.

We did not want to customize too much ourselves but we had a number of
requirements that we considered as \"must haves\". These requirements
were things like audit trails, 4-eyes principle on changes, automated
deployments, and versioning on changes

And last but not least, it should connect to developers. What do
developers love most? Code! So the solution we build has to be based on
code. Not just any code, but code that can easily be read by developers
and by non-developers.

**What can building a solution based on code offer us?**

The most frequently used communication mechanism between business and
developers is through documents. Documents that describe what needs to
be done. A very nice approach to make this a bit more structured and
readable by both humans and machines are structured files, For instance,
Yaml, XML or JSON, which is human and machine readable.

When requirements like permissions, firewall ports, etcetera are written
down in a structured format, all of a sudden this can be implemented
automatically by a machine. When we describe our permissions in
structured files, which is essentially just code, developers can change
it within their development workflow without having to make changes in
other solutions.

Let\'s take a look at an example file:

> resourcegroup: xpirit-asac-article
>
> \- userPrincipal: Asac-Group-Owners
>
> role: Owner
>
> \- userPrincipal: Asac-Group-Readers
>
> role: Reader
>
> \- userPrincipal: asac-user-01@roadtoalm.com
>
> role: Contributor

This snippet, with a few lines of code, describes the end state of users
and groups in a resource group. When you look at the snippet, you see
straight away what the resource group looks like. If you compare this
with requests in a ticketing system, you would have to take the start
state and all changes together to see what the current or end state
should look like.

This is great, but the main advantage of a structured file is that it
can be shared with the team, the business and operations. And even
better, it can be interpreted and executed by a machine. This makes
recreating environments a lot easier as well.

**Version Control as auditing tool**

Now that we have a solution for writing the requirements in code, we
should also have a solution for the audit trail, 4-eyes principle, and
versioning. Here is where source control, in our case Git, comes in.
Combining code with Git, which most developers are already accustomed
with, gives us most of the requirements we wanted, and for free! Git has
built-in functionality for versioning, branching and audit trails. Most
Git servers have pull request features which give us reviews, approvals
or 4-eyes principles. And when we add automated builds and releases, we
can also add automation to process these code changes into environment
changes.

The last advantage of having the permissions in code is that the team
itself is in full control of what the changes will be. They do not
require certain admins or central teams to manage their permissions for
them.

![Azure Security as Code](./media/image1.png){width="6.3in"
height="1.1677690288713911in"}

**Introducing Azure Security as Code**

As mentioned before, the principle of using structured files and Git
instead of manual work and ticketing systems is broadly applicable. Many
systems can be unlocked by applying the same principles. To give you a
hint of what is possible we would like to guide you through our use
case: 'Setting permissions on Azure'.

The first thing we thought about was the technology stack. We wanted a
library that was cross-platform and could be easily extended using the
Azure API. The eventual choices we made for our technology stack were as
follows.

**Yaml**

We use the yaml format to store the security configuration. Yaml can
easily be read by people, even the not so technical ones, and it is
great for merging in Git.

**AZ Command Line Interface (Azure CLI)**

All interaction with the Azure API is done with the Azure CLI. We choose
this because Azure CLI is a cross-platform implementation that is
broadly used. Microsoft promotes the API and makes sure it is always
up-to-date with the latest Azure API. Alternatives like PowerShell
Modules or C# libraries are sometimes lacking behind.

**PowerShell Core**

To write the orchestration of the scripts and make it easily usable as
cmdlets, we use Cross-Platform PowerShell. This runs on every platform
and is perfect to write the script flow, using the Azure CLI for doing
the real work.

**Docker**

As an optional service, especially for people on Linux or Mac, we
provide a Docker container where both the CLI and PowerShell are
installed together with the latest version of the Azure Security as Code
library.

**Installing the software**

The second thing we thought of was the usage of the library. How will
people consume the library? We wanted to make this is as easy as
possible and came up with two different distribution mechanisms:
installing via a Powershell Module, and running it in a Docker
container.

Because we understand that every use case is different, we made it an
open source library so people can extend and modify it to their needs.

Azure Security as Code can be installed from the **source code on
Github**, by installing the Powershell Module from the **PowerShell
Gallery** or by pulling and running the Docker container from **Docker
Hub**. This article uses the

PowerShell Modules.

To get started, open a PowerShell Windows (admin mode) and install the
Azure-SecurityAsCode Module. Because the modules use the AZ CLI
underwater, login to Azure with the Azure CLI as well.

> Install-Module Azure-SecurityAsCode
>
> #login with Azure CLI
>
> az login

Looking at the available cmdlets there are 3 main categories:

-   Get-Asac-All\[Resources\] -- This retrieves all resources of a
    specific type into separate yaml files. For example. all resource
    groups and related RBAC settings will be stored.

-   Get-Asac-\[Resource\] -- This retrieves a specific resource into a
    yaml file.

-   Process-Asac-\[Resource\] -- This applies the configuration that is
    defined in the yaml file.

Let's take a sample scenario to understand how this works.

**Scenario: Manage your RBAC on resource groups**

To make it a bit more tangible, let's walk you through a scenario in
which we want to baseline the security of our Azure Resource Group. We
want to let teams manage their own security without giving them the
keys. Therefore we need to baseline the resource group and store the
settings as a structured file in Git, where the development team can
then modify it.

> \# Get all the resource groups in the subscription and their RBAC
> settings in the current
>
> directory
>
> Get-Asac-AllResourceGroups -outputPath \$pwd
>
> \# Get settings for 1 resource group
>
> Get-Asac-ResourceGroup -resourcegroup xpirit-asac-article -outputPath
> \$pwd

When these commands are executed, a YAML file for each resource group is
created in the target folder as follows:

> resourcegroup: xpirit-asac-article
>
> rbac:
>
> \- userPrincipal: Asac-Group-Owners
>
> role: Owner
>
> \- userPrincipal: Asac-Group-Readers
>
> role: Reader
>
> \- userPrincipal: asac-user-01@roadtoalm.com
>
> role: Contributor

Let\'s assume we want to assign rights to asac-user-02, make them
Reader, and remove the Asac-Group-Readers from the resource group.

We update the YAML as follows:

> resourcegroup: xpirit-asac-article
>
> rbac:
>
> \- userPrincipal: Asac-Group-Owners
>
> role: Owner
>
> \- userPrincipal: asac-user-01@roadtoalm.com
>
> role: Contributor
>
> \- userPrincipal: asac-user-02@roadtoalm.com
>
> role: Reader

After updating the yaml, we call the following Asac cmdlet.

> Process-Asac-ResourceGroup -resourcegroup xpirit-asac-article
> -basePath \$pwd

The new settings are applied to the resource group.

This scenario is for resource groups, but the same actions can also be
executed for other resources, for example SQL Server, DataLakeStore, and
Key vault.

**Making this part of the development process**

Now that we have seen how easy and convenient it is to set roles and
users, we need to ask the question: \"How can we embed this in the
development process?\"

The answer is pretty simple. In exactly the same way as you treat your
other code and configuration files.

The first step is to set up a Git repository that can hold all the
configuration files.

A perfect way to make your Git repository accessible to everyone is to
use Azure DevOps Repos.

The next step is to set branch policies on your Git repository that
allow you to control check-ins to the Git Repository. If you want teams
to set and request their own security settings, but still want to have
control over the process, branch policies are a perfect way to do so.

In VSTS, navigate to the code repository and select branches. On the
master branch, select the Branch Policies option.

![](./media/image2.png){width="6.3in" height="3.2465813648293964in"}

Select the \[Require a number of reviewers\] policy in the branch
policy. Optionally add Automatic Reviewers to have someone from the
security team review all the changes.

![](./media/image3.png){width="6.3in" height="3.55209208223972in"}

Once you have set up the branch policy, you need to make sure the policy
is applied once you have changed this. Of course there are many ways to
do this. The easiest way is to have a Continuous Integration build that
runs every time a change is merged to the master branch.

Just configure a build with a Powershell script that runs the
Process-Asac-\[resource\] cmdlets.

**What else can we do?**

The way forward in a DevOps world where automation is key, is by moving
towards a model in which configuration is stored as code. By using Git
and Build Pipelines as a mechanism to move configuration changes to
production offers a lot of benefits. First and foremost the auditability
and traceability of a change, and secondly it is a nice and easy review
mechanism.

But Configuration as code can bring more benefits. You can use it to
describe end-state, which enables you to rebuild things from scratch
without having to know all history. And you can use it as living
documentation of your system, or use the files as baseline to validate
changes.

Azure Security as Code is one example of how to use development
methodologies as a way to enable development teams. But of course this
does not only apply to our specific scenario. Rethinking the way in
which people interact, replacing humans with machines and manual actions
with automated processes is the real take away. You can do *this by
storing settings in a structured format, choosing the* right technology
stack, and enabling your end-users to stay close to the tools they use
and love. Because in the end it is all about enabling people to be more
productive and deliver more value to the end-customers.

**Would you like to contribute?**

If you want to contribute to the library, please take a look at **our
Xpirit Github page**
