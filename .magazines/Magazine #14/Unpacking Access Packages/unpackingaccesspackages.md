# Unpacking Access Packages.  {#unpacking-access-packages. .unnumbered}

Introduction To Azure AD Access Packages and How We Used Them In A
Real-World Customer Scenario

##  {#section .unnumbered}

*By Rik Groenewoud*

## Introduction {#introduction .unnumbered}

How great would it be if users could enroll to a set of Azure AD Groups,
Applications or SharePoint sites *themselves,* instead of jumping
through all kinds of bureaucratic hoops before access has been granted
and the user can do their actual job?

Not only would this be great for the end user, but also from the
administrator's point of view this would be the ideal scenario. Instead
of maintaining a custom enrollment process with lot of manual steps this
process shifts the action to the end users themselves. This makes it
possible for the administrator to focus on maintaining a secure and
compliant system, instead of doing repetitive simple tasks.

As I will show you in this article, access packages are here to do just
that! I will dive deeper into what these packages are all about.
Firstly, I will explain what these packages are, what choices you have
when setting them up and how a basic use flow will look like.

After the basics are clear, I will give a real-world customer use case
of how we at Xpirit Managed Service leveraged Access Packages to create
a highly automated enrollment process for a complex Identity and Access
Management scenario.

##  {#section-1 .unnumbered}

## What are Access Packages?  {#what-are-access-packages .unnumbered}

In essence, access packages are a way to manage access to Azure
resources in a streamlined and efficient manner. With these packages,
you can group together a set of Azure resources that are typically used
by the same team or group of users and assign specific access
permissions to that package.

In addition to streamlining access management, they also provide
self-service capabilities for end-users. This means that users can
request access to specific packages themselves, rather than having to go
through an IT administrator or department. When a user requests access
to package, an approver can review the request. This provides an
additional layer of control and ensures that access to resources is
always aligned with the organization\'s security and compliance
requirements.

To be able to leverage access packages you need an Azure AD P2 or
Enterprise Mobility + Security (EMS) E5 license.

##  {#section-2 .unnumbered}

## Configuration of an Access Package {#configuration-of-an-access-package .unnumbered}

The packages live in Catalogues which are containers for one or more
packages. After you have given the package a name and description you
proceed by adding the resources you want the users to enroll on. These
can be Azure AD Groups, Enterprise Applications and/or SharePoint sites.

The next step is to decide who can enroll to the package. There are 3
main options.

#### Users in your directory

This option makes it possible to further specify whether all members, or
specific users, may request access to this package. Furthermore, you can
decide if manual approval is needed. If yes, you can determine if users
need to write a justification, if the user's manager or a specific other
user (or users) will be the approver and in what timespan the decision
must be made. It is even possible to create a second line of approvers
for when the first approver did not decide in the given time span.

In short, with this option it becomes possible to really create a
granular least privilege structure for your access packages. By doing so
it becomes easy to align to the company's policies and governance
regarding User Access Management.

#### Users not in your directory 

With this option it becomes possible to specify particular external
organizations, open up enrollment to all connected organizations or to
select all users meaning all connected users plus any new external
users. With this option you can add or skip the approval flow.

#### None (administrator direct assignment only)

This last option means that only administrators can add people to the
access package. This is the best option if there is no approval flow in
place and users should not be able to enroll themselves in the packages.

If needed, you can ask users for additional information when applying
for access by using custom parameter fields. You can determine a
lifecycle for the access package assignment. And finally you can
determine whether an access review is needed. The idea behind these
access reviews is to check if all package enrollments are legitimate and
up-to-date.

## Self-Service Onboarding  {#self-service-onboarding .unnumbered}

After you have setup the catalogue and created one or more access
packages, a typical happy flow self-service scenario would look
something like this:

![Graphical user interface, application Description automatically
generated](./media/image1.png)


The end-user goes to <https://myaccess.microsoft.com> and after login
with his/her Azure AD Credentials, the landing page with all available
packages will be shown. The user can request access for the desired
packages after which an approver receives an e-mail with the pending
approval. After approval, the user receives feedback via e-mail and gets
the resource roles that are given to the access package.

## Customer Use Case: Automated User Onboarding in a Web Application with Complex roles and rights structure. {#customer-use-case-automated-user-onboarding-in-a-web-application-with-complex-roles-and-rights-structure. .unnumbered}

So far, we have looked at the fairly straightforward self-service
scenario in which access packages can play a very useful role. But there
are more use cases for access packages. At Xpirit Managed Services (XMS)
we used them in a slightly different manner. We received a request from
our customer to build an automated onboarding process for their Azure AD
users. The user should be able to login to the web application using SSO
and should be automatically assigned to the correct roles and projects.

What makes this challenging is the fact that this application has a
complex roles and rights structure. It is made up of 20+ separate
projects, all with three or more separate roles per project. A quick
calculation shows that we are talking about 60+ project/role
combinations. Several hundred users should be able to be enrolled in
multiple projects and have potentially multiple roles within these
projects.

Then there are special key users who should be able to have elevated
rights in multiple projects and users who only need to have reading
rights in all projects. To make it even more complex, the users are all
from different companies working together in this project.

In the next part I will explain what we came up with to solve this
challenge.

###  {#section-3 .unnumbered}

### SSO and Roles Mapping {#sso-and-roles-mapping .unnumbered}

To make SSO possible an App Registration + Enterprise Application
already was in place. The first step was to map all the roles from the
application, to App Roles in the App Registration. In the JSON Manifest
this looks like this:

\"appRoles\": \[

        {

            \"allowedMemberTypes\": \[

                \"User\"

            \],

            \"description\": \"ProjectX_RoleY\",

            \"displayName\": \"Project X Role Y\",

            \"id\": \"8e9bd73b-e64f-46e5-b4b6-481234\",

            \"isEnabled\": true,

            \"lang\": null,

            \"origin\": \"Application\",

            \"value\": \" ProjectX_RoleY \"

        }

\]

The next step is to map Azure AD (AAD) Groups onto these App Roles.
There is a 1-on-1 mapping between the App Roles and the AAD Groups. The
mapping has to be done in the Enterprise Application:

![Graphical user interface, text, application, email Description
automatically generated](./media/image2.png)


To make this more rigid and maintainable, we placed the App Registration
JSON manifest in a Git repository and created an Azure CLI script to
update this manifest using the az ad app update cmdlet.

Finally, to make the mapping between the App Roles and AAD Groups, a
PowerShell script loops over all AD Groups like:

foreach (\$group in \$aadGroups) {

    \$role = \$roles \| Where-Object -Property value -eq
\$group.Displayname

    \$params = \@{

        PrincipalId = \"\$(\$group.Id)\"

        ResourceId  = \"\[Resource-Id\]\"

        AppRoleId   = \"\$(\$role.Id)\"

    }

    New-MgGroupAppRoleAssignment -GroupId \$group.Id -BodyParameter
\$params

}

With this mechanism the first part of the puzzle is solved. Now the
users need to be enrolled in the AAD Groups. It is time for the access
packages to make their entrance.

### Access Packages  {#access-packages .unnumbered}

For every project, 3 separate packages were created corresponding the
roles (and the AAD Groups) in the application: the Approver, Contributor
and Manager. In these packages the corresponding AAD Groups are selected
and also a Default Users AD Group is added, which is mapped to the
global reader role in the application.

Furthermore, for the Key Users and Reader Only users, separate packages
were created with all the appropriate AD Groups. For this use case this
functionality really shines because with a single enrollment the user is
added to all these AD Groups at once.

Because the end users in this scenario don't know to which access
packages they belong (at least for now), the self-enrollment options
were disabled and instead "administrator assignment only" was the way to
go. Also, because the administration and approval of user assignment is
done beforehand, the approval flow could be disabled as well. No
end-date on the enrollment was needed because users that are no longer
eligible to be in an Access Package are automatically removed (I will
explain more on this later).

The access packages are not created in an automated way because this was
a one-time job and can be done pretty easily from the Azure Portal.

With the access packages in place, we came to our final step: the
enrollment of users into the packages.

### User enrollment  {#user-enrollment .unnumbered}

For the user enrollment the goal was to create a solution which fits the
XMS way-of-working. At XMS we always seek to work together with our
customers and thereby *enable* a customer to work in a DevOps way. By
doing so. It is important that we don't create boundaries between
different stakeholders or between the business and IT, but work
*together* and build smart processes in which repeated tasks are
automated as much as possible.

In this case, where a traditional service provider would setup a
ticketing system in which the customer can ask to enroll new users, we
wanted to make this a collaborative and smooth process. This process now
consists of 3 steps:

1.  For every access package a simple .CSV file was created in which the
    customer can add or delete users as desired:

Displayname;Email;AccessPackage

User Z;userz@example.com;Project X \| Role Y

2.  These .CSV files are part of a repository and the customer can
    create a PR with the new changes. These PRs are validated by the XMS
    team.

3.  After the PR is merged, an Azure DevOps pipeline is triggered that
    runs a script to enroll the users from the CSVs. It also creates a
    new AD Guest User if the user does not exists in our tenant.
    Finally, a check is done on removed users from the CSV file. These
    get removed from the packages as well.

Some code snippets from this script:\
\
\# Invite user if not yet exists

New-MgInvitation -InvitedUserEmailAddress \"\$(\$user.Email.Trim())\"
-InviteRedirectUrl \"https://example.com/invite
-SendInvitationMessage:\$true

\# We check if the user already is member of the package and if not, the
user is added.

\$check = Compare-Object -DifferenceObject \$assignments.Target.ObjectId
-ReferenceObject

\$AADuser.Id -ExcludeDifferent

if (\$null -eq \$check) {

> \$policy = \$accessPackage.AccessPackageAssignmentPolicies\[0\]
>
> New-MgEntitlementManagementAccessPackageAssignmentRequest
> -AccessPackageId \$accessPackage.Id -AssignmentPolicyId \$policy.Id
> -TargetId \$AADuser.Id
>
> Write-Host \"User \$(\$user.Displayname) is added to Access Package\"

}

\# We check if there are differences between the CSV and the assignments
and if yes, we remove the users.

\$check = Compare-Object -DifferenceObject \$assignments2.TargetId
-ReferenceObject \$userid

Write-Host \"Differences in IDs: \$(\$check2.InputObject)\"

if (\$null -ne \$check2) {

foreach (\$assignment in \$check2.InputObject) {

#Get AssignmentId for user that has to be removed

\$assignmentId = (\$assignments2 \| Where-Object { \$\_.TargetId -eq
\$assignment }).Id

New-MgEntitlementManagementAccessPackageAssignmentRequest
-AccessPackageAssignmentId \$assignmentId -RequestType \"AdminRemove\"

Write-Host \"Removed \$assignment\"

}

}

else {

Write-Host \'No users removed\'

}

![](./media/image3.png)
The diagram below summarizes the process
flow of this solution

###  {#section-4 .unnumbered}

### Future improvements  {#future-improvements .unnumbered}

This process has been running for a few months and we are quite happy
with it. Also, it is important to state that we take on these kind of
challenges iteratively. What started with the customer sending us Excel
sheets and us enrolling the users manually to the access packages (and
thereby already adding value for our customer because the users could
access the application with the correct roles), evolved towards the
process it is now.

This does not mean it is a perfect solution and there is still room for
further improvement. The next step would be to move towards
self-service. We must make sure the access packages correspond to what
the end users understand. We can appoint approvers at every separate
company. With this in place we should be able to remove the CSV-files
(and more importantly the maintenance of these CSVs) entirely. This will
result in a simpler, leaner process.

## To conclude {#to-conclude .unnumbered}

This article has shown what access packages are, what is their potential
and how they can be useful in self-service scenarios.

In our customer use case, I showed that these packages can be a valuable
piece of the puzzle when it comes to creating a maintainable solution
for a complex Identity and Access Management scenario.

If you have any questions about this subject or how you could use access
packages in your environment, don't hesitate to reach out!
