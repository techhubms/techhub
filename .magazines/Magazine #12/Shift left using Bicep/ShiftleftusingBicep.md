# Blessed templates

It is a good practice to offer some building blocks of blessed templates
for the infrastructure within many organizations. A Cloud Competence
Center of Excellence commonly provides these templates. By the nature of
control, the focus is primarily on the Security, Architecture, and
Governance part of things, compared to Engineering enablement and
Operational Excellence. The organization can run the strategy "comply or
explain[^1]" by providing blessed templates, this means that it is easy
to comply by using the blessed templates and no questions are asked.
When you do need to deviate from the blessed templates you need to
explain and go through a review board. The templates are blessed in the
form that the involved parties have already approved for use. Many teams
will be able to run their solutions based upon these templates.\
\
Managing changes to the blessed templates can be a bit challenging. How
do we patch it efficiently on all resources during a security
vulnerability? What if a product team has moved on to the next project
and do not actively support the previous project? How would teams know
if they need to change or re-deploy their infrastructure? The
effectiveness stands or falls by the convenience for the teams to
comply. How easy is it to reach teams that use your template, and how
easy is it to change the template and re-deploy.

This article will explain why you can keep using your blessed templates
or easily convert them to bicep files and gain their benefits. For more
information about the general use of Bicep, you should read the article
"Stop wrestling with ARM Templates" written by Erwin Staal in this same
magazine.

Using the modular improvements introduced in Bicep v0.4.1008 to support
the Bicep registry, you can improve your support of blessed templates to
your consumers and have compile-time validation to support complete
CI/CD scenarios for your IAC (Infrastructure As Code). Let's see how
this impacts the ease of use and blessed templates lifecycle.

## Basic Bicep deployment

To understand the value of using a Bicep registry for your templates, we
first need to understand how things work without templates, as shown in
the diagram below.![Graphical user interface Description automatically
generated](./media/image1.png)


Figure . Basic Bicep deployment

A relatively standard CI/CD pipeline for infrastructure written in
Bicep, where you don't use templates:

1.  An engineer makes a change to a Bicep file in Git

2.  When pushing the change, a pipeline will be automatically triggered

3.  The first step in the pipeline is to transpile the Bicep template
    into an ARM template and store that as an immutable artifact for
    later use

4.  Deploy the artifact, with environment-specific parameters, to an
    Azure -ResourceGroup, -Subscription, -Tenant, or -Management group

The deployment is nothing more than running an az CLI command using the
"deployment group" arguments with the transpiled Bicep template file
passed with the \--template-file switch, as shown in the following
Powershell.\
![Text Description automatically
generated](./media/image2.png)


Figure . Deploy Bicep to a resourcegroup

##  Using templates

The use of templates helps you re-use definitions you already created.
When deploying a web service, you always want to deploy application
insights with a log-analytics workspace. Using templates this is done in
multiple ways, for example, by referencing;

-   a (local) folder in the same project

-   a storage account in Azure

-   a template-spec resource in Azure

-   a module in the Bicep registry

Template-spec\
Before the possibility to push Bicep templates to a Bicep registry
existed, the preferred way of sharing your templates was by publishing
the ARM template to a Template-Spec-Resource. Below you can see such a
template-spec resource in Azure.\
![Graphical user interface, application Description automatically
generated](./media/image3.png)


Figure 4. Template-spec resource example

Notice how this can leverage RBAC (a consumer needs only read-access),
versioning, and even release documentation when drilling down to a
specific version. A template-spec is published using the CLI like this:\
![Text Description automatically
generated](./media/image4.png)


Figure 5. Publish template-spec

Another Bicep file can then use this Template-Spec-Resource by using
"Microsoft.Resources/deployments" type with a "templateLink" property
referencing the Template-Spec-Resource with a specific version as shown
below:\
![](./media/image5.png)

Figure . Using a template link

The template link describes a full URI, including the desired
resource\'s name and version.

Compared to our Basic Bicep Pipeline, using the "Template Specs" helps
you already to achieve a blessed template structure, having semantic
versioning and release documentation together with your template
definition. Template specs are already altering the CI/CD flow using the
blessed template during deployment.\
![A screenshot of a computer Description automatically generated with
medium confidence](./media/image6.png)


Figure 6. Deploy Bicep using template specs

The maintenance, approval, and availability are taken care of by a
separate team, having the ability to publish new templates and versions.
The consumers only have read privileges on the template spec and can use
it during the deployment of their resources.

The use of the template-spec as a URI reference, as shown in figure 3,
is a bit clumsy. Due to not having IntelliSense over the Template-spec,
you have to know the names of the parameters to pass them. The same
applies to the output of the template spec. You also need to know the
name of the parameter to retrieve it. We can improve on this using a
module instead of a resource. A module can refer to a local file or a
template spec. To use a template spec, use the following format:

module
\<symbolic-name\>\'ts/\<alias\>:\<template-spec-name\>:\<version\>\' = {

When looking at the template below, you can see no direct reference to a
subscription anymore:

![Text Description automatically
generated](./media/image7.png)


Figure . Using template-spec module

The reference is abstracted away into a configuration file, making your
definitions more readable and easier to maintain.

![](./media/image8.png)


Figure . Configure bicepconfig.json for template-spec

You place the configuration file, called called "bicepconfig.json", in
the root of your project. Here you can define the "ts" "Template Spec".
The alias in this example is 'BlessedTemplates'. The other benefit of
using the module approach is Bicep will recognize the link, start
downloading the definition to your local user\'s folder, and provide
IntelliSense during your development. It, therefore, becomes much easier
to use parameters or the outputs of the resource.

There are a few shortcomings of using "Template Specs":

-   While using template specs, you reference it by a link. This causes
    validation to happen at deployment time, instead of build time,
    which is a bit late

-   The content of the template-spec is not known client-side. Looking
    at the ARM template, you will only see the reference to the
    template-spec instead of seeing the nested resources of the spec. In
    our example of the web services, you would only see a reference to a
    web service template-spec and not be aware that an application
    insight and a log analytics workspace would also be deployed.

### Bicep registry

Using the modular improvements introduced in Bicep v0.4.1008, we can now
use a Bicep module registry. This improvement enables us to publish
Bicep modules to an Azure Container Registry, as shown in the following
command.

az acr login \--name mybicepsharedregistry.azurecr.io\
bicep publish StorageAccount.bicep \--target
br:mybicepsharedregistry.azurecr.io/bicep/modules/storage:0.1

First, you need to log in to the Azure Container Registry and publish
the Bicep file. Next, add the configuration to the bicepconfig.json and
reference the module as you did for the template spec. This time, you
use the "br" keyword. This keyword helps Bicep to understand it can
retrieve the modules from the bicep registry and thus enabling
IntelliSense and **compile-time validation**.

![Text Description automatically
generated](./media/image9.png)


Figure . Configure bicepconfig.json for bicep-registry

![](./media/image10.png)


Figure . Using bicep registry module

Another significant benefit, compared to the template-spec, is that when
the Bicep file is transpiled into an ARM template, you will get nested
templates instead of a link to the template. A nested template explains
what resources will be modified. In contrast, a template link only
references a template spec that will be accessed during deployment,
making it harder to understand what is happening while reviewing an
artifact for deployment approval. As an example, the first screenshot
below shows the use of a template spec, the second one the use of a
module in the Bicep registry.

![A screenshot of a computer Description automatically generated with
medium confidence](./media/image11.png)


Figure . Template-spec in an ARM template, showing only
resource/deployment

![Text Description automatically
generated](./media/image12.png)


Figure . Bicep-registry usage in an ARM template, showing
resource/deployment with all child resources used

### Shift left

Using either the template-spec or bicep registry will gain the Shift
Left[^2] capability of compile-time validation instead of deploy time
validation as shown below.\
![Diagram Description automatically
generated](./media/image13.png)


Figure . Compile-time validation using container registry

Because Bicep will download the ARM templates and the Bicep files to the
local user's folder, it will validate during compile-time. Compile-time
validation will help you fail your pipeline in the build step before
creating the immutable artifact you want to deploy to your environments.

# Renovate-bot dependency automation

One of the questions we asked at the beginning of this article was how
we manage changes to our blessed templates and enable our consumers to
detect changes that they need to deploy quickly. We can use a dependency
manager like Renovate-bot to detect new versions using semantically
versioned Template-specs or Bicep registries.\
![Diagram Description automatically
generated](./media/image14.png)


Figure . Renovate-bot dependency manager

Implementing the Renovate-bot will enable the following flow:

1.  Renovate-bot will scan the organization\'s repositories for out of
    date dependencies

2.  Renovate-bot submits a Pull Request into the repositories that use
    the blessed templates, enabling your consumers to approve or
    auto-approve the Pull Request and stay secure and compliant.

3.  Approval will automatically trigger your CI/CD pipeline and roll out
    the new templates to their environment.

Configure Renovate-bot\
To make use of the Renovate-bot, follow the website [^3] guidelines.
Renovate-bot can be integrated with industry-standard CI/CD tooling and
runs on a hosted or on-premise environment. The easiest way to enable
this is by installing it as a service into your GitHub account.

As a team managing the blessed templates, you want Renovate-bot to pick
up on published changes. To pick up those changes you can use an example
as shown below.

![Text Description automatically
generated](./media/image15.png)


Figure . Upgrade version, tag, and publish

To always have a valid version, this script does the following:

1.  Get the current version from the latest Bicep registry manifest

2.  Increment the minor version to create a new, unused version

3.  Publish the Bicep with the incremented version to the registry

4.  Create a GitHub tag on the current SHA[^4] used to run the build

Now that we've tagged the release in GitHub, we can use the Renovate
Managers[^5] to configure an override for bicep files. Renovate managers
are like package managers. These managers know, for a specific resource
(such as docker, dotnet, golang, etc.), how to determine the latest
published version and compare it to the version used in the repository.
Because there is no dedicated manager for Bicep, we need to configure
our own using the generic regex manager.

![Text Description automatically
generated](./media/image16.png)


Figure . Renovate regex-manager configuration

To configure the use of the regex manager, we change the renovate.json
and add a "regexManagers":

1.  Configuration to match all \*.bicep files. This configuration will
    limit this manager\'s configuration to only search in the .bicep
    files and not any other files you have set up.

2.  Define your "matchStrings" regular expression. This configuration
    will search for the semantic versioning in the "br:\*\*\*\*"
    annotation. A regular expression group 'currentValue' will contain
    the found version.

3.  Configure the "datasourceTemplate" so the regexmanager can compare
    the 'currentValue' to GitHub-tags. GitHub-tags is a known data
    source[^6] for renovate-bot

4.  Configure the GitHub repository to search for the tag in
    "depNameTemplate"

The result is an automatic Pull Request whenever a new blessed template
publishes in a foreign repository.\
![Graphical user interface, text, application, email Description
automatically
generated](./media/image17.png)


Figure . Renovate\'s Pull Request

# Conclusion

Many organizations already have blessed templates, and their success
depends on the ease of use. The blessed templates should be beneficial
for the organization to make sure that all consumers work in a secure
and compliant manner. Consumers should rely on the service offered by
the blessed templates to update their dependencies to maintain
compliance automatically. Using this provided service should not be
hard. It should be a golden path to take, enabling engineering
capability instead of restricting it.

### Take away

-   You can keep using the existing approved blessed templates with
    Bicep

-   

-   You can make use of IntelliSense by using bicep-configuration

-   Using bicep registries enables you to have compile-time validation
    (which is actually transpile time)

-   Use Container Registry over Template Specs to have a more explicit
    transpiled ARM template.

-   

-   Support all repositories with automated dependency updates on your
    blessed templates using Pull Request created by renovate-bot

[^1]: https://en.wikipedia.org/wiki/Comply_or_explain

[^2]: The capability to find defects earlier in your development
    lifecycle

[^3]: https://www.whitesourcesoftware.com/free-developer-tools/renovate/

[^4]: \"SHA\" stands for Simple Hashing Algorithm. The checksum is the
    result of combining all the changes in the commit and feeding them
    to an algorithm that generates these 40-character strings. A
    checksum uniquely identifies a commit.

[^5]: https://docs.renovatebot.com/modules/manager/

[^6]: https://docs.renovatebot.com/modules/datasource/
