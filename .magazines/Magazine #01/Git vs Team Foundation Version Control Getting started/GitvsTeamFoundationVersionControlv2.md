Git vs Team Foundation Version Control: Getting started

When you are a Microsoft developer you probably are used to a Microsoft
Version Control system: In the early days Visual SourceSafe or nowadays
Team Foundation Server Version Control (TFVC). TFVC is fairly simple.
Once you understand that the server always must know what you are doing
it is simply a great and easy to use Version Control System.

When TFS 2012 introduced Local Workspaces, it became even better,
because now all the advantages of TFS and the advantage of not having to
check-out files, like in for example SVN, were combined. With a local
workspace you do not have to inform the server about everything you are
doing, you only check-in files. All changes are kept locally on your
disk. This change in TFS simplified it even more.

But Microsoft realized that their Source Control System was not embraced
by anyone other than developers using Microsoft tools. Within this
cross-platform, cross-language world they need to have something
different to offer. The best choice Microsoft made was not to build
something themselves, but embrace a system that was already very popular
within the open source and non Microsoft community: Git.

There are many great posts and blogs about what Git is. The book Git
Pro[^1], which is available online, does a great job in explaining the
concepts. The most important thing to know is that Git is a Distributed
Version Control System (DVCS) where TFS is a Centralized Version Control
System (CVCS). TFS knows about all the changes that happen. You check
out a file, edit the file and check it in. The server knows. History is
on the server. Therefore you need to be "online" in order to use a
Centralized Version Control System. A DCVS is different. You get a full
copy of a repository that is stored somewhere on a server, including all
the history etc. Then you work locally on this repository. You can
check-in changes, view history and rollback if you like. After working
for a while you can then push back all the changes to the repository
where you originated from.

Because it can be very hard to grasp Git as a hardcore TFVC user this
article contains some starting points to "translate" Git to TFVC. Be
aware that TFVC and Git are conceptually very different and hat a "reaI"
translation can actually not be made. But it will certainly help in
setting some reference points.

**Dictionary**

In this article we will talk about the following definitions

  -----------------------------------------------------------------------
  **Git Actions**                     **TFS Command**
  ----------------------------------- -----------------------------------
  **Clone**                           Create Workspace and Get Latest

  **Checkout**                        Switch workspace / branch

  **Commit**                          CheckIn / Shelve

  **Status**                          Pending Changes

  **Push**                            CheckIn

  **Pull**                            Get Latest Version

  **Sync (Push and Pull. Only exists  CheckIn and Get Latest Version
  in VS UI)**                         
  -----------------------------------------------------------------------

**Clone vs. Create Workspace and Get Latest**

\[Clone\] in Git means that you get a local copy of a full Version
Controlled repository that is stored somewhere. You basically pull
everything on to your local machine. Clone is the first thing you need
to do when you start working with a Git repository.

In TFS we first \[create a workspace\], map the server folder to a local
folder and \[Get the Latest Version\] of all the files in our workspace.

![](./media/image1.png)


*Create a repository clone in Git*

![C:\\Users\\REN\~1\\AppData\\Local\\Temp\\SNAGHTML1583d07d.PNG](./media/image2.png)


*Create a workspace in TFS Version Control*

**Checkout vs. Switch workspace / branch**

\[Checkout\] in Git, means change the branch you are working on. This is
something that we do not have in TFS. Sure, we have branches in TFS, but
they are in separate folders and locations on disk and in the
repository.

![](./media/image3.png)


*Branches in TFS Version Control*

In Git this is not the case. You just have 1 view on the repository and
the branches are "contained" within this repository. Switching branches
is very easy to do and files directly change.

In TFS a best practice is, that you create a separate workspace per
branch you are working on. When you want to work on another branch, you
\[change your workspace\], open the solution from this workspace and
start working. In Git you can use the command \[Git Checkout\] or just
use the DropDown lists in the UI.

![C:\\Users\\REN\~1\\AppData\\Local\\Temp\\SNAGHTML158e7e02.PNG](./media/image4.png)


*Switch Git branch in TFS Web Access*

![C:\\Users\\REN\~1\\AppData\\Local\\Temp\\SNAGHTML158fa135.PNG](./media/image5.png)


*Switch Git branch in Visual Studio*

**Commit vs. CheckIn / Shelve**

The comparison between commit and checkin/shelve is one that can
actually be not made. In TFS you can perform a \[Checkin\]. The client
sends all the files to the central TFS repository and the files are
available to everyone. In Git you can \[Commit\] your changes when
finished. Big difference here is that in Git, you always \[Commit\] to
your "local" repository. Changes are not available on the original
branch where you came from. You have to \[Push\] your changes as a
second action to "check-in" on the server.

In TFVC you cannot commit locally. The closest thing to checking in for
yourself is a shelveset.

![C:\\Users\\REN\~1\\AppData\\Local\\Temp\\SNAGHTML1592a954.PNG](./media/image6.png)
\
*Git Commit*

![](./media/image7.png)


*Git Push*

![C:\\Users\\REN\~1\\AppData\\Local\\Temp\\SNAGHTML15956b81.PNG](./media/image8.png)


*TFS Checkin*

**\
**

**Status vs. Pending Changes**

In Git you can use the command \[Git Status\]. It shows you all the
modified files in your local repository that have not been Pushed. In
TFS we have the view Pending changes windows. In the UI it looks quite
the same and has the same behavior.

Beware of the branches in Git.

![C:\\Users\\REN\~1\\AppData\\Local\\Temp\\SNAGHTML15e1108f.PNG](./media/image9.png)


*View Status in Git*

**Push vs. CheckIn**

When you want to send your changes to the remote repository where you
originated from in Git, you can use the \[Push\] command. With this
command, you send all your local commits to the remote repository. In
TFS you use checkin for this. Because there is only 1 central server you
always checkin on this server.

![C:\\Users\\REN\~1\\AppData\\Local\\Temp\\SNAGHTML15e276a6.PNG](./media/image10.png)


*Push in Git*

**Pull vs. Get Latest Version**

To get all commits from the originator repository that were made by
others, you can use the pull command. With this command you retrieve all
the changes.

In TFS we use the \[Get Latest Version\] of \[Get Specific Version\]
command to synchronize the workspace.

![](./media/image11.png)


*Pull in Git*

![C:\\Users\\REN\~1\\AppData\\Local\\Temp\\SNAGHTML15e3bdcc.PNG](./media/image12.png)


*Get Latest Version In TFS*

**Sync vs. CheckIn and Get Latest Version**

Sync is not really a Git Command. You use Push and Pull. Visual Studio
created a nice graphical button to do this together.

![](./media/image13.png)


*Sync button in Visual Studio*

## Summary

It is hard to compare 2 version control systems that are conceptually
different. But in essence it are both version control systems that you
can use to store your changes safely. Both TFVC and Git have their
advantages and disadvantages and it is hard to make a good choice. Git
is fast and flexible and most of all supported on all platforms. As a
downside it has a steep learning curve. TFVC is fast, robust, enterprise
ready and easy to use.

With the pointers in this article, try out Git. Use it in your daily
scenario and make the choice for yourself. After all it is a matter of
taste after all.

**Resources:**

-   Git Basics
    -- [[http://git-scm.com/book]{.underline}](http://git-scm.com/book)

-   Announcement of Git and TFS
    --[[http://blogs.msdn.com/b/bharry/archive/2012/08/13/announcing-git-integration-with-tfs.aspx]{.underline}](http://blogs.msdn.com/b/bharry/archive/2012/08/13/announcing-git-integration-with-tfs.aspx)

-   Wikipedia Definition
    -- [[http://en.wikipedia.org/wiki/Git\_(software)]{.underline}](http://en.wikipedia.org/wiki/Git_(software))

-   TechEd 2013 video of Martin Woodward
    --[[http://channel9.msdn.com/Events/TechEd/NorthAmerica/2013/DEV-B330#fbid=Lb7e4eYEDpC]{.underline}](http://channel9.msdn.com/Events/TechEd/NorthAmerica/2013/DEV-B330#fbid=Lb7e4eYEDpC)

Build 2015 video of Martin Woodward -
<http://channel9.msdn.com/events/Build/2015/3-746>

-   Use Git in Visual Studio
    --[[http://blogs.msdn.com/b/visualstudioalm/archive/2013/02/06/set-up-connect-and-publish-using-visual-studio-with-git.aspx]{.underline}](http://blogs.msdn.com/b/visualstudioalm/archive/2013/02/06/set-up-connect-and-publish-using-visual-studio-with-git.aspx)

-   A successful Git branching model
    -- [[http://nvie.com/posts/a-successful-git-branching-model/]{.underline}](http://nvie.com/posts/a-successful-git-branching-model/)

[^1]: http://git-scm.com/book
