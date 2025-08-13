In the current days of serverless and k8s, you may forget about the dark
corners in the cloud that are filled with legacy and cloud-not-so-native
applications. Automatic deployments are the norm nowadays, and
Infrastructure as Code (IaC) has been on the rise. What if we told you
that it is possible to do the same for your age-old applications and get
almost the same benefits?

# Machine Images

The beauty of machine images is that they enable you to create Virtual
Machines in a repeatable manner and add new instances in a minimum of
time. Both Azure and AWS have the so-called notion of images: Managed
Image on Azure and Amazon Machine Image (or: AMI) on AWS. Deploying new
VMs (or EC2 Instances) using images is already possible with the given
infrastructure. You can even take this a step further by deploying
Virtual Machine Scale Set or use AWS Auto Scaling to create and destroy
instances on demand.

# Build Your Own

For speeding up and stabilizing deployment it is best to create your own
hand-rolled images with customizations that you need in every instance.
The normal procedure for creating such an image is:

1.  Spin up a fresh VM

2.  Perform your customizations, either remote or from login

3.  Generalize the machine (sysprep for Windows)

4.  Shutdown the VM

5.  Capture the disk.

Packer helps you by automating this process. By doing so, it enables you
to set up a CI/CD pipeline which ensures that the image is build,
stored, and deployed on the cloud provider of your choice.

Packer uses packer templates, which contain all the configuration and
instructions to build an image. Building images like this is like adding
layers to a docker image, except for the storage part. You can even
reuse your custom image to be used as the input of another packer build.

## Builders

The template contains the builders with the configuration for the target
platform(s) on which you want to create an image. This article will show
you how to do this for both Azure and AWS. On Azure you use the
\'azure-arm\' builder and on AWS the \'AMI builder\'. In short: the
builder is the configuration for your target platform for the
intermediary VM that is used, and the target location of the image that
is the build output.

## Provisioners

The other part of the template is about the provisioners. Provisioners
are the things you use to interact with the VM. For example, it can
execute some script running inside the VM to install some software.
Another provisioner will copy some content to the VM or download it from
the VM. In short, provisioners are about customizing the intermediate VM
from which the final image is built.

Installing software

The first thing you want to do when building a Windows Server image is
to install the package manager, for instance chocolatey or winget.
Having a package manager at your fingertips will greatly reduce the time
and effort spent on hand-rolling installation scripts.

For now we will use chocolatey, but the idea is the same for winget. To
install chocolatey you just follow the regular installation instructions
from the chocolatey website
[https://chocolatey.org/install]{.underline}. Put that in a script and
invoke it from a provisioner.

iex ((New-Object System.Net.WebClient)

.DownloadString(\'[https://chocolatey.org/install.ps1]{.underline}\'))

Install-Chocolatey.ps1

\"provisioners\": \[{

\"type\": \"powershell\",

\"scripts\": \[

\"{{ template_dir }}/Install-Chocolatey.ps1\"\
\]

}\]

packer-template.json

After that, most installation scripts will look like this:

choco install awscli -y

It is tempting to put all these installations in an inline script in the
packer template. However, we have found it valuable to put these small
snippets into self-contained files. Here you can include additional
validation for the expected application or configuration to be present
(apart from the exit-code), or you can also include these in separate
files.

For example, when you need sqlcmd on your image and you install the
following package:

choco install sqlserver-cmdlineutils -y

Install-SqlCmd.ps1

You then validate that the command is actually available on the VM:

if (!(Get-Command sqlcmd)) {

exit 1;

}

Validate-SqlCmd.ps1

These are the unit tests of your packer build!

# Generalize

To deploy a Windows image to different PCs, you first need to generalize
the image to remove computer-specific information such as installed
drivers and the computer security identifier (SID).

> Sysprep (System Preparation) prepares a Windows installation (Windows
> client and Windows Server) for imaging, allowing you to capture a
> customized installation. Sysprep removes PC-specific information from
> a Windows installation, \"generalizing\" the installation so it can be
> installed on different PCs.

The following provisioner is often used as the last step in the Packer
template to generalize the Azure VM before the image is captured:

{

\"type\": \"powershell\",

\"inline\": \[

\"if( Test-Path
\$Env:SystemRoot\\\\windows\\\\system32\\\\Sysprep\\\\unattend.xml ){ rm
\$Env:SystemRoot\\\\windows\\\\system32\\\\Sysprep\\\\unattend.xml
-Force}\",

\"& \$env:SystemRoot\\\\System32\\\\Sysprep\\\\Sysprep.exe /oobe
/generalize /quiet /quit\",

\"while(\$true) { \$imageState = Get-ItemProperty
HKLM:\\\\SOFTWARE\\\\Microsoft\\\\Windows\\\\CurrentVersion\\\\Setup\\\\State
\| Select ImageState; if(\$imageState.ImageState -ne
\'IMAGE_STATE_GENERALIZE_RESEAL_TO_OOBE\') { Write-Output
\$imageState.ImageState; Start-Sleep -s 10 } else { break } }\"

\]

}

Full script: [https://bit.ly/xprt-packer]{.underline}

Amazon provides the following scripts in the EC2 instances:

{

\"type\": \"powershell\",

\"inline\": \[

\"C:/ProgramData/Amazon/EC2-Windows/Launch/Scripts/InitializeInstance.ps1
-Schedule\",

\"C:/ProgramData/Amazon/EC2-Windows/Launch/Scripts/SysprepInstance.ps1
-NoShutdown\"

\]

}

Initialize and generalize an EC2 instance

[https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/sysprep\--system-preparation\--overview]{.underline}

# Deploy

After the image has been created, Packer stores the image in the Shared
Image Gallery on Azure, or in the EC2 Console in AWS. From here on the
work for Packer is done. Now the image is available in the cloud
provider you use, and you can create new VMs from it!

An example script for creating a VM from either a Shared Image Gallery
or Managed Image directly on Azure would be:

az vm create \\

-n MyVm \\

-g MyResourceGroup \\

\--image
/subscriptions/xxx/resourceGroups/xxx/providers/Microsoft.Compute/galleries/xxx/images/xxx

az vm create \\

-n MyVm \\

-g MyResourceGroup \\

\--image
[/subscriptions/xxx/resourceGroups/xxx/providers/Microsoft.Compute/images/xxx]{.mark}

On AWS you use a Cloudformation yaml file to deploy the created AMI file
as an EC2 Instance. In the file, set the id of the created AMI as the
imageid option.

<https://docs.aws.amazon.com/AWSCloudFormation/latest/UserGuide/aws-properties-ec2-instance.html>

# CI/CD

Since we now have a fully automated build process for creating machine
images, it\'s quite easy to set up a build pipeline like we\'re used to
doing for software development.

## Azure Pipelines

The most convenient way to build from an Azure DevOps pipeline is to
install and use the packer extension. First of all there is a task to
download a specific or, when not specified, latest version of Packer and
put it on the path.

\- task: riezebosch.Packer.PackerTool.PackerTool@0

displayName: Download packer

Then there is the second task of executing packer commands and using a
service connection to provide Packer with the credentials for the
selected cloud provider.

\- task: riezebosch.Packer.Packer.Packer@1

displayName: Packer build

inputs:

azureSubscription: \$(serviceConnection)

templatePath: packer.json

force: true

variables: \|

resource_group=\$(resource_group)

## AWS CodeBuild

You can use Linux containers to build source from a git repository on
AWS CodeBuild. You can use a .yaml file to set up a build definition
just like in a git repository in Azure DevOps. A build definition on AWS
consists of several phases, for example \'install\', \'pre-build\' and
\'build\'. Before we can build the packer json file, we need to install
Packer on the Linux container.

pre_build:

commands:

\- curl -sS -o packer.zip
https://releases.hashicorp.com/packer/1.5.1/packer_1.5.1_linux_amd64.zip

\- unzip packer.zip

\- mkdir -p /usr/local/bin

\- mv packer /usr/local/bin

After installing Packer we can use the packer command line to build the
Amazon Machine Image (AMI). A packer EC2 instance will start, based on
the packer.json file provided in the build definition. This EC2 instance
will execute all the scripts and files in the provider section of the
packer file. To build the packer file, add the following lines to the
yaml file.

build:

commands:

\- packer validate packer.json

\- packer build packer.json

# That's it

If you treat the infrastructure like code, then you can set up a fully
functioning deployment pipeline to create managed images and deploy
virtual machines from this. By doing so, you can put all sorts of
programming practices in place, for instance pull request validation and
automated smoke testing. Ultimately, you will end up with an environment
in which all servers are immutable and disposable. When you can recreate
or update VM\'s with the push of a button, no one should really care
about specific instances. Each VM will be the same on each environment,
and deployments will be repeatable.
