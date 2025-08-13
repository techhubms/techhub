# Deployment pipelines for versioned Azure Resource Manager template deployments

Azure Resource Manager templates offer you a declarative way of
provisioning resources in the Azure cloud. Resource Manager templates
define how resources should be provisioned. When provisioning resources
on Azure with Azure Resource Manager, you want to be in control of which
resources are deployed and you want to control their life span. To
achieve this control, you need to standardize the templates and deploy
then in a repeatable way. This can be done by managing your resource
creation as [Infrastructure as Code](https://xpir.it/xprt5-iac).

The characteristics of Infrastructure as Code are:

-   Declarative

-   Single source of truth

-   Increase repeatability and testability

-   Decrease provisioning time

-   Rely less on availability of persons to perform tasks

-   Use proven software development practices for deploying
    infrastructure

-   Idempotent provisioning and configuration.

In this article, I will explain how to create Azure infrastructure with
versioned Azure Resource Manager templates (ARM templates). For the
deployments, the VSTS Build and Release pipelines were used. The code or
ARM templates are managed from a Git repository. Code in the Git
repository can use the same practices as any other development project.
By updating your code or templates, you can deploy, upgrade or remove
your infrastructure at any time. The Azure portal will no longer be used
to deploy your infrastructure because all ARM templates are deployed by
deployments pipelines. This will give you full traceability and control
over what is deployed into your Azure environment.

## Deployment pipelines for deploying versioned ARM templates

If you have a large number of infrastructure resources, it is good to
know what the exact footprint is. If you know this, you can easily
redeploy and create test environments without the constant question: to
what extent is this infrastructure the same as production? In order to
obtain adequate control over your infrastructure, you can apply
versioning to the deployments and their content. In this case, ARM
templates in combination with VSTS can help you. When applying
Infrastructure as Code this way, you can test an actual infrastructure
deployment and develop new templates at the same time. To do this you
need two deployment pipelines:

-   Deployment of the reusable ARM templates (see the article on Best
    Practices Azure Resource Manager templates)

-   Pipeline for deploying the resource base on the reusable ARM
    templates.

The templates used in the second pipeline are deployed in the first
pipeline. These are called linked ARM templates. A linked ARM template
allows you to decompose the code that deploys your resources. The
decomposed templates provide you with the benefit of testing, reusing
and readability. You can link to a single linked template or to a
composed one that deploys many resources like a VM, or a complete set of
PAAS resources.

### Deployment pipeline for reusable linked ARM templates

The goal of this pipeline is to deploy a set of tested linked templates
(a version) to a storage account from where they can be used. Each time
you perform an update to the templates (pull request), a deployment
pipeline is triggered (continuous integration), and once all tests are
successful, a new version is deployed and ready to use. The new
deployments exist side by side with the earlier deployment. In this way
the actual resource deployments can use a specific version. The
following figure provides an overview of the pipeline:

![](./media/image1.png)

#### Build

The deployment starts with a pull request to the master branch of the
Git repository. Then a new build is trigged. In this build pipeline, the
sources are copied to a build artifact to be used in a release. In
addition, a build number is generated that can be used as version number
of the released templates.

![](./media/image2.png)

#### Release

The release has several release steps to ensure that the ARM templates
are tested before they are published. Templates are tested by deploying
the resources. In the sample, all steps are an automatic process. When
the tests succeed, the release continues with the deployment of the
templates to the storage account where they can be used for the real
infrastructure deployments.

##### 

##### Deploy test

The first step is to deploy to a test location on the storage account.
This test location will be used to test the ARM templates. When you
deploy the templates, this can also help you in debugging errors by
running a test deployment from your local machine. The only task in the
environment is to do an Azure Blob File Copy. All the linked templates
(the artifact) are deployed to the Azure storage account.

![](./media/image3.png)


##### Test ARM templates

During the second step the ARM templates can be tested. First you get a
SAS token (link:
https://pascalnaber.wordpress.com/2016/08/31/vsts-task-to-create-a-sas-token/)
to access the storage account. The next step consists of deploying the
ARM templates. This runs a test ARM template that covers the parameters.
When the deployment fails, the pipeline is stopped (asserted). The last
step consists of removing the resource group where you have deployed
your test resources. If all steps succeed, the templates are approved
for release. You can perform these steps multiple times for different
types of resources and resource combination. When you perform this step
N times, you can run them in parallel. If you split this into multiple
templates, it is also clear where you have a problem if the step fails.

![](./media/image4.png)


##### Deploy the production version

The last step will deploy the templates to a location where the build
number is used in the naming convention. The task Azure Blob File Copy
is the same as in the first step, only the location where the files are
copied to is variable, depending on the build number. In this way the
templates can be referenced by using the build number in the URL.

![](./media/image5.png)


### 

### Pipeline for deploying the resource base on the reusable ARM templates

When all linked templates are deployed, they can be used to perform the
deployments of your Azure infrastructure. In the sample pipelines, I
have only one test environment, but the number of test environments can
be different for each pipeline. One pipeline will deploy the resources
of one Azure Resource group.

![](./media/image6.png)


#### Build

The goal of the build is to produce an artifact of the templates that
can be used in the release pipeline. The deployment starts with a pull
request to the master branch of the Git repository. Then a new build is
trigged. In this build pipeline, the sources are copied to a build
artifact to be used in a release.

#### Release

The release pipeline will validate, test and then deploy the resource to
the production environment. The templates in each environment are the
same; the only difference is the parameter files. The parameter file can
parameterize the sizes of the resources deployed in the different
environments. The sizes must be chosen wisely in order to represent the
production environment, but keep in mind the costs of running a test
environment. In the main template, you keep a variable build, which you
use to point to a specific release from the previous pipeline. In this
way you can control the deployed version of your shared linked
templates.

##### Validate templates

The first step consists of validating the templates for all
environments. This is done by running a deployment in Validation Mode.
Here, the template is compiled, it is checked to see whether it is
syntactically correct and will be accepted by Azure Resource Manager.
You have to do this for all environments to check whether the parameter
files are correct. When the step succeeds, the deployment to test will
start.

![](./media/image7.png)


##### Release test

The goal of this step is to deploy and test the resource. If there is a
need for a gatekeeper, approvals can be added at the beginning of this
step. If not, the deployment of your test environment starts
automatically. If possible, use the Deployment Mode option "Complete".
This ensures that the resources in the Azure Resource Group are the same
as those defined in the ARM template. The action consists of 2 tasks.
First create a SAS token for access to the Azure Storage. The next step
performs the actual deployment. If everything succeeds, you can
optionally do some manual testing on the resource itself, or even add a
script that does this.

##### ![](./media/image8.png)

##### Release production

The production release starts with a gatekeeper (approval). When you are
satisfied with the previous (test) resources, an approver can start the
production deployment. All resources in the production resource group
will be updated according to the ARM template. Try to run your
deployment in Complete mode, because then you know that all resources in
the resource group are the same as you defined in your Git repository.
You are running an infrastructure as Code scenario.

![](./media/image9.png)


## Final thoughts

Setting up deployment pipelines for your ARM templates is an up-front
investment aimed at giving you control over the resources that are
deployed in your Azure environment. When the pipelines are running,
changes to your Azure environment are fully controlled from the code,
and all changes are traceable from VSTS. When you have two staged
pipelines, you have an Infrastructure as Code scenario in which you are
in full control over what is deployed into Azure.
