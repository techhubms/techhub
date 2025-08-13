## Start with Visual Studio Release Management vNext

Team Foundation Server 2013 Update 3 came with [Visual Studio Release
Management vNext. vNext is, next to the deployments with
agents](http://www.visualstudio.com/get-started/manage-your-release-vs),
another way of doing deployments with VSRM. Deployments to machines,
without having to install an agent, is the most important feature of
vNext.

This is because VSRM vNext uses [Powershell Desired State Configuration
(Powershell
DSC)](http://blogs.technet.com/b/privatecloud/archive/2013/08/30/introducing-powershell-desired-state-configuration-dsc.aspx)
as the tool/engine to execute powershell on different machines.

Although TFS 2015 will contain a whole new Release Management
implementation, this article guides you through a setup of the current
version of Release Management

#### 1. Check your prerequisites

In order to use RM vNext you will need the following prerequisites

-   A zure subscription where I can create a Virtual Machine

-   Team Foundation Server 2013.4 or 2015 RC installed

-   Release Management Server + Release Management Client 2013.4 or 2015
    RC installed

-   TFS Build Server + Agent 2013.4 or 2015 RC installed

-   Visual Studio 2013.4 (Pro/Premium or Ultimate) or 2015 RC

-   An empty Team Project (RMvNextForDummies)

### 2. Create the simplest web application ever

-   Start up your Visual Studio and create a new Web Application

-   Run to test the solution. If you want to make it pretty you can
    always change the color or the title.

### 3. Check in Solution and create a Build

Check in the solution in Source Control in your newly created Team
Project. Easiest way is to just right click the solution and choose
\[Add Solution to Source Control\], Choose the right Team Project to add
the solution to, Navigate to the \[Pending changes\] hub in Team
Explorer and check in your changes.

Navigate to the \[Build Hub\] in Team Explorer and create a \[New Build
Definition\]. Leave everything default. on the \[Build Defaults\], fill
in a valid drop location, and on the \[Process\] Tab, make sure you
selected the solution you just created.

[MSDN Documentation can be found
here](http://www.visualstudio.com/get-started/build-your-apps-vs)

Run the build to check if it builds successfully!

### 4. Set up Release Management to use your Azure account

Start up the Release Management Client and navigate to the
\[Administration \| Manage Azure\] Tab. Click \[New\] and fill in the
details that are asked. The information that should be filled in is
described perfectly on MSDN. [Click this
link](http://www.visualstudio.com/get-started/deploy-no-agents-vs#SetupAzure)
and follow the steps described here.

Now create a Virtual Machine with Windows 2012R2 on Azure. Do nothing
yet on this virtual!

<http://www.visualstudio.com/get-started/deploy-no-agents-vs#SetupAzure>

Get the SubscriptionID and ManagementCertificate from the
publishsettingsfile you downloaded. Create a new storage account on
Azure especially for using in Release Management. Use the name of the
storage account as \[Storage Account Name\]

[MSDN on how to create a storage
account](http://azure.microsoft.com/nl-nl/documentation/articles/storage-create-storage-account/#create)

Create a vNext Environment and link your newly created Azure Server

### 5. Create a Release Template and Path

No you need to create stages (our deployment pipeline) and a release
template by following the steps described on MSDN
(<http://www.visualstudio.com/get-started/deploy-no-agents-vs#CreateReleaseTemplate>).
Keep it simple and add only 1 stage !

This is my result for the Release Path

My Component

And my Template (still empty)

### 6. Start my first Release

Now..No further hassle, I want to start a release!

Drag the \[Deploy using PS/DSC\] to the surface.

When you try to choose the \[Componentname\] it cannot be chosen yet. In
order to do this, you need to add the component to the toolbox first.
Right click the components "folder"in the toolbox and add the component.

Now select the ComponentName. and trigger a \[New Release\]. Select the
latest build and start. You 'll end up in an approval screen where you
can approve the release of the first stage. Approve it and watch what
happens.

Sure ! : I know there is no PowerShell DSC script yet, let's try to make
the communication work.

### 7. Troubleshoot and make it work

When your deployment fails, take a look at the following things:

-   Open your firewall ports for PowerShell (default on 5986 and 5985)

-   When deploying to a non-domain machine, skip the certificate check
    in Release Management or upload a certificate from the target
    machine (as described here:
    <http://fabriccontroller.net/blog/posts/using-remote-powershell-with-windows-azure-virtual-machines/>)

```{=html}
<!-- -->
```
-   Make sure you type your username as .\\username. So with the period

### 8. Check some artefacts

We have our end-to-end scenario. Deploy (doing nothing yet) succeeded
and we are in the validation step. The steps performed are.

-   Upload all the build output to your azure storage. You can check
    this by logging in to your azure account, navigate to storage,
    select the storage you configured in RM and choose container. There
    you see the files from the build.

-   On the target machine you have a directory \[DtlDownloads\] in your
    c:\\windows (c:\\windows\\dtldownloads). Here you find all the files
    downloaded from storage and ready for further processing on your
    machine.

### 9. Create a simple DSC script that we can execute

Now that we have our connectivity, we can start building some DSC. There
are some good posts around the internet about DSC and also some in
combination with Release Management. You can find those in the resources
section at the bottom of this post.

For now , I will suffice by copying the website bits to the
inetpub/wwwroot directory on the target machine. The DSC script we want
to execute must be send to the server as well. The easiest way to do
this is to make the DSC part of the build.

Open your web application, and add a folder DSC to your web application.
Add a file CopyWebSite.ps1 to this folder and put this in the file.

configuration FullSetup

{

node MACHINENAME {

WindowsFeature IIS

{

Ensure = \"Present\"

Name = \"Web-Server\"

}

WindowsFeature ASPNet45

{

Ensure = \"Present\"

Name = \"Web-Asp-Net45\"

DependsOn = \"\[WindowsFeature\]IIS\"

}

File CopyDeploymentBits

{

Ensure = \"Present\"

Type = \"Directory\"

Recurse = \$true

SourcePath = join-path \$applicationPath \"\_PublishedWebsites\"

DestinationPath = \"C:\\inetpub\\wwwroot\"

DependsOn = \"\[WindowsFeature\]ASPNet45\"

}

}

}

FullSetup

 

 

Make sure you replace MACHINENAME with the name of your target Azure
machine.

Make sure you set the file properties in VS to Copy Always so that your
file ends up in the build.

In your Release Template, add this file to the PSScriptPath

Check in the file, run a build and start a new release.

When this succeeds, you have configured an IIS, Framework 4.5 and copied
the bits of your build to a directory.

### 10. Try more with DSC and RMvNext

Now try more with DSC. You can install applications, configure
firewalls, set security. All scenarios that are very useful when doing
your deployment. Follow the posts listed below for more advanced use.

## Summary

Release Management is the bridge between Development and Operations.
Using Powershell DSC and storing your server configuration as code in
Source Control allows you to really automate your deployment pipeline.
Release Management vNext allows you to do create these pipelines and
allows you to enable your approval workflow to production. And realize
this with TFS 2015 there is much more to come !

### Resources

[Jasper Gilhuis' -- Curah on Release Management (a selection of
links)](http://jaspergilhuis.nl/2013/12/18/visual-studio-release-management-on-curah/)

<http://blogs.msdn.com/b/visualstudioalm/archive/2014/07/07/how-to-deploy-to-standard-or-azure-environments-in-release-management-2013-with-update-3-rc.aspx>

<http://www.colinsalmcorner.com/post/using-powershell-dsc-in-release-management-the-hidden-manual>

<http://blogs.msdn.com/b/musings_on_alm_and_software_development_processes/archive/2014/11/21/release-management-and-dsc.aspx>

<http://fabriccontroller.net/blog/posts/using-remote-powershell-with-windows-azure-virtual-machines/>

<http://www.visualstudio.com/en-us/get-started/deploy-no-agents-vs.aspx>

<http://blogs.msdn.com/b/visualstudioalm/archive/2014/07/07/how-to-setup-environments-for-agent-less-deployments-in-release-management-release-management-2013-with-update-3-rc.aspx>

<http://blogs.msdn.com/b/visualstudioalm/archive/2014/07/22/deploying-using-powershell-desired-state-configuration-in-release-management.aspx>

<http://roadtoalm.com/2014/09/24/silently-install-and-configure-a-tfs-build-server-with-powershell-dsc/>

DSC

<https://gallery.technet.microsoft.com/scriptcenter/DSC-Resource-Kit-All-c449312d>

<http://blogs.msdn.com/b/powershell/archive/2014/08/07/introducing-the-azure-powershell-dsc-desired-state-configuration-extension.aspx>

<http://blogs.technet.com/b/privatecloud/archive/2013/08/30/introducing-powershell-desired-state-configuration-dsc.aspx>

<http://blogs.msdn.com/b/powershell/archive/2014/04/03/configuring-an-azure-vm-using-powershell-dsc.aspx>

<https://technet.microsoft.com/en-us/library/dn249921.aspx>

<http://blogs.technet.com/b/privatecloud/archive/2014/04/25/desired-state-configuration-blog-series-part-1-learning-about-dsc.aspx>

<http://www.colinsalmcorner.com/post/install-and-configure-sql-server-using-powershell-dsc>
