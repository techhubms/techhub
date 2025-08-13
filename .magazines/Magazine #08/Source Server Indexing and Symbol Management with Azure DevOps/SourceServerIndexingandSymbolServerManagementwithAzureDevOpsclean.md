Source Server Indexing and Symbol Server Management with Azure DevOps

Developers debug their applications on a daily basis and everyone must
have experienced the power of debugging. But what if you want to debug a
crash dump or what if you want to debug a NuGet package in your
application? The concept of Source Server Indexing and Symbol Server
Management is still not a widely known practice in the field, but
setting up a Source Server and Symbol Server in an enterprise
development environment can be extremely valuable. If you see how easy
it is to set things up with Azure DevOps, it should be mandatory for
every software application you are working on. It can make your life so
much easier, and not only yours, but also the lives of many other
developers.

There are two main reasons why you should embrace the concept of Source
Server Indexing and Symbol Server Management:

1.  [Live Debugging]{.underline}

During development, it will help anyone who is referencing assemblies
that are built with a source-server-enabled build, to debug those
assemblies with the original source code. Think of NuGet packages for
example. How annoying can it be when you are using a NuGet package and
you cannot step into the original source code?

2.  [Debugging Crash Dump Files or Snapshots]{.underline}

Every application which is pushed to production should allow easy
troubleshooting and easy debugging when something bad happens. This can
simply not be done when you cannot rely on source server indexing and a
central symbol server. Rest assured, something bad will eventually
happen and you want to be ready for this when users are sending you
crash data, Snapshots, or IntelliTrace log files.

## Why are pdb files so important?

Let's get to the basics first and start with the importance of pdb files
(also known as symbol files). Every developer in the Microsoft ecosystem
probably has already seen these files, but to my surprise not a lot of
developers actually know how important these files can be and how they
work.

Definition Wikipedia (https://en.wikipedia.org/wiki/Program_database):

*Program database (PDB) is a* *proprietary file format (developed by
Microsoft) for storing debugging information about a program (.dll /
.exe) and* *is created from source* *files during compilation. It stores
a list of all symbols in a module with their addresses, together with
the name of the source file and the line on which the symbol was
declared. These files are only created once during the compilation
process and are uniquely matched with the binaries. This process cannot
be forged afterwards.*

In essence, the pdb files help developers to load all debugging
information (variables, function names, source line numbers) in the
development environment (Visual Studio) while "debugging". In addition,
they provide the capability to step into the original source code files
via breakpoints, watch variables, and perform many other useful tasks
related to the art of debugging. WinDbg (The Windows Debugger) can also
be used to debug application code and analyze crash dumps. In both
scenarios, you always must obtain the proper symbols for the code you
wish to debug, and load these symbols into the debugger. In short: no
debugging without a matching pdb file. With .NET Core we can now do
similar things on Linux (LLDB Debugger, ProcDump or SOS plugin), but
this will be out of scope for this article.

Creating a simple Console Application in Visual Studio and
compiling/building the project will drop this pdb file next to the
assembly file (exe/dll).

Looking more closely at the content of the pdb file, you will notice
that somewhere the file path to the Program.cs source file can be found.

![](./media/image1.png)

So, when debugging the MyConsole application in Visual Studio you will
notice that the symbols are loaded from the pdb file and this allows the
editor to dive into the source code while running the application.

![](./media/image2.png)

The editor can find a valid pdb file by means of the file name and the
location of the pdb file (probing). What's also key is that it must be
the exact pdb file that was composed during the compilation process, and
the *handshake* is done through a GUID that is embedded in the assembly
file (.dll) and the pdb file. If the GUID of the assembly and the pdb
file do not match, the editor won't be able to debug the module at the
source code level, and there's no way to override this. This emphasizes
the importance of storing your pdb files because without these files,
you are losing control over the entire debugging process.

Of course, this always automatically works for local development
(private builds), and there won't be a mismatch between the local
running application and the underlying pdb files. But what about public
builds where the sources are compiled on an independent build server,
and where the output assemblies are stored as artifacts?

## Consuming NuGet packages from Azure DevOps Artifacts

A good example to show the need for Source Server Indexing and a Symbol
Server is the use of (internal) NuGet packages. I have created a new Git
repository in Azure DevOps and added a .NET Core Class Library with a
Calculator class that provides two basic methods (Add and Subtract).

![](./media/image3.png)

I also have created a Build Pipeline in Azure DevOps to create a NuGet
package from this assembly and to publish the NuGet package to a feed in
Azure Artifacts. This NuGet package now becomes available for
consumption by all teams with access to this feed.

Adding the package feed url in Visual Studio (NuGet Package Manager \>
Package Sources) offers developers the option to select the appropriate
NuGet package and add it to the current application. No big deal and
business as usual, but imagine the functionality inside the NuGet
package is a bit more complex and you want to understand how the logic
has been implemented while debugging.

![](./media/image4.png)

Setting a breakpoint at one of the Calculator methods won't allow you by
default to step into the code and see what happens under the hood. This
is because Visual Studio doesn't have access to the exact pdb file that
was created during the Azure DevOps build process on the build server.

![](./media/image5.png)

The NuGet package used in Visual Studio delivers the binary file (.dll)
but the matching .pdb file is nowhere to be found on the local machine
where I'm trying to debug the Add method of the Calculator class. And
even worse, the pdb file can't be recovered because the build process
didn't take care of storing the file into a shared location (Symbol
Server), and a new build will potentially override the old version of
the build output in case the same private build agent was used.

However, there's another problem that must be solved in order to provide
seamless support for debugging. Let's look at the content of the latest
pdb file I could retrieve in the workspace of the private build agent.

![](./media/image6.png)

If I were able to use the matching version of the pdb file in my
debugging session, Visual Studio would be redirected to fetch the exact
source file (Calculator.cs) from the hardcoded file path that was used
in the build process on the build server. However, it is not our goal to
define a similar file structure on your local machine to fake the
retrieval of source files, and it can never guarantee that you are
providing the exact same source files that were used during the build
process.

Let's zoom into the solution to solve the issues above.

## Source Server Indexing

When compiling sources on the build agent and producing the pdb files,
we must find a way to avoid pointing to a fixed file path of the source
files being used in the build process. And that's exactly what Source
Server Indexing will do. It's a simple and efficient process to embed a
version-control path (including the version identifier) into the pdb
file, and ensure that it is readable by Visual Studio or Windbg. This
technique allows the editor to retrieve the exact source file directly
from the version control system instead of the fixed file path on the
build agent.

Since TFS 2010, the build system provides an out-of-the-box solution for
embedding this information into the pdb file. I remember using a Perl
script in the past to accomplish this manually for TFS 2008, but luckily
this has become a simple build task in Azure DevOps and TFS.

![](./media/image7.png)

The above image shows the required "Index Sources & Publish Symbols"
build task that will scan for pdb files, and this task will eventually
inject extra information into the pdb file to link towards the exact
versioned source files being used at compilation time.

Running this build and looking for the content inside the pdb files
reveals the magic that was being done inside the build process.

![](./media/image8.png)

A big chunk of extra data has been injected into the pdb file and it now
contains a tf.exe command to dynamically extract the source file from a
Git repository (via the commit id) inside a Team Project from Azure
DevOps or TFS. Note that the variables can still be overridden via a
srcsrv.ini file in case the collection url changes for TFS or Azure
DevOps.

Another method to enable a similar debugging experience is to use Source
Link (<https://github.com/dotnet/sourcelink/blob/master/README.md>),
which is a language-control and source-control agnostic system.
Microsoft libraries such as .NET Core and Roslyn have enabled Source
Link. For this article I have chosen to explain Source Server Indexing,
which doesn't require extra properties in the .NET project.

## Symbol Server Management

Source Server Indexing is only one part of the solution, because storing
this pdb file only in the workspace on the build server does not make
any sense. We need this particular pdb file in a central location that
can be easily searched when starting a debugging session.

This is where Symbol Server Management can play a valuable role. A
symbol server enables a debugger to automatically retrieve the correct
symbol file (pdb). This is based on the unique GUID that was used in the
compilation process on the build server to mark the assembly file and
the pdb file. Remember that this linking is a one-time operation and
cannot be reproduced after the facts. Losing the pdb file means that you
lose the opportunity to debug the output assembly. For ever!

Support for Symbol Server Management is now provided by the same "Index
Sources & Publish Symbols" build task. Until now it is only possible to
publish the pdb files to Azure DevOps, which is a full-blown Symbol
Server. Older versions of TFS or Azure DevOps Server only allows you to
push the pdb files to a network share.

![](./media/image9.png)


## Back to Visual Studio to activate debugging with symbols

In my TestConsole application in Visual Studio I already picked up the
latest NuGet package from the Azure Artifacts feed, which was made
available via the latest build that included Source Server Indexing and
the publication of the symbols to Azure DevOps.

To get the full debugging experience with symbols, you must verify a
number of settings that are not turned on by default in Visual Studio.

-   Connect/Register the Azure DevOps Symbol Server

![](./media/image10.png)

Your Azure DevOps organization will be just another symbol server next
to the Microsoft Symbol Servers and you can choose to enable/disable it
at any time.

-   Disable "Just My Code" and enable "Source Server Support"

> ![](./media/image11.png)
>
> The first toggle is important to not only debug the sources you manage
> inside your solution, and the second option is required to fetch the
> original source files from the pdb file when the debugging process
> needs the source code.

When trying to step into (F11) the Add method of the Calculator class,
Visual Studio will now help to search for the matching pdb file in the
Azure DevOps Symbol Server, and the content of the pdb file will
instruct Visual Studio to download the Calculator.cs file from the Git
repository inside Azure Repos. The pdb file and the Calculator.cs file
are now locally available in the cache folder of your computer, ready
for live debugging actions.

![](./media/image12.png)

## 

## Conclusion

This article will help you solve the issue of not being able to attach
the debugger in certain scenarios and step into the original source
code, It should provide you with enough information to assess why it's
so important to treat your pdb files in the same way as you treat your
assembly files that might go to production. Source Server Indexing and
the publication of the symbols (pdb files) go hand-in-hand and should
always be enabled in your automated build processes that produce output
for production. Azure Pipelines provides the right build task to
accomplish this for cross-platform applications and the rest of the
magic is done inside your favorite debugging tool.
