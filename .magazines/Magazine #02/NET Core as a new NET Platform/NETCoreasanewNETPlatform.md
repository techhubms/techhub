Alex Thissen

The ASP.NET team has taken .NET in a new direction in order to achieve
the vision for a new cloud-optimized web framework. The team envisioned
a redesigned framework to overcome the limitations of the original
15-year-old .NET Framework 4.5 and a modern cross-platform story to be
able to quickly adapt to the changing Web. The Web stack was redesigned
and rebuilt nearly from scratch and along with it, the .NET Framework.
Now, more than a year down the road, the .NET rework has been adopted by
Microsoft to serve as the future of a brand new .NET Platform, starring
.NET Core.

## More than meets the eye

ASP.NET introduces a new implementation of .NET Framework libraries and
new runtimes plus additional tooling. Together these are named .NET
Core. A casual glance at .NET Core might give the impression that this
is merely a trimmed-down, lightweight version of the original .NET
Framework. To some degree that is true. A closer look at .NET Core
reveals a complete overhaul of the way .NET is engineered, implemented,
and offered as a modern platform to build a variety of application
types. The new version of.NET is called .NET Core 1.0. This new version
number of 1.0 is chosen to indicate the revolution of .NET, as opposed
to a natural evolution of the previous iterations of 1.0 through 4.6,
that 5.0 would suggest. The framework libraries largely remain
unaffected from a functional perspective and your current knowledge of
programming for .NET is still valid. Underneath the familiar surface of
.NET is a new runtime, NuGet as a packaging format for the distribution
of .NET libraries, and brand new tooling to manage the globally
installed runtimes for .NET. All in all, a lot of significant changes
that deserve attention that goes beyond .NET Core.

## A shift in strategy

The most obvious changes are easy to see and have been advocated by the
ASP.NET and .NET team: the new .NET Core is now a much smaller and
self-contained framework and runtime for applications. This means that
.NET Core does not have to be installed globally on machines that use a
particular version. Moreover, multiple versions can exist side-by-side
and be deployed as part of the application. Also, .NET Core can be used
on multiple operating systems and allows Windows, Linux, FreeBSD and
MacOS to host applications built on top of .NET Core.

The small footprint and isolated deployment combined with the
cross-platform story opens up new scenarios for .NET applications. One
important scenario is the ability to create and run Docker containers
for Linux and Windows with the appropriate version of .NET Core. This
makes .NET a valid choice when adopting a container approach for
building and hosting your applications.

Another important change is the decoupling of versions of the .NET
Runtime from the framework. This means that multiple versions of
runtimes for different operating systems and processor architectures can
run your executables and libraries. Your binaries might be based on .NET
Framework libraries that are not necessarily of the same version as the
runtime, nor compiled specifically for a platform on which the runtime
exists. This does require a new approach to the way libraries are
compiled and made compatible across runtimes and platforms.

## Multi-targeting

Traditionally the .NET Framework Software Development Kit (SDK) had a
specific version targeting a single version of the .NET Runtime and
Framework. Later editions of tooling such as Visual Studio introduced
multi-targeting. This allowed a code project for an application or
library to choose the version of the framework and runtime they depend
and build upon. Still, the compiler was able to build binary images
solely for a single framework version, one at a time.

With .NET Core come new compilers such as Roslyn, the
Compiler-as-a-Service facilities of .NET that allows actual compiling
your source code for multiple framework targets.

Each of these targeted frameworks is denoted by a Target Framework
Moniker (TFM) that is a short symbol representing the intended version.
For example, **net451** is the TFM for .NET Framework 4.5.1 and
**dnxcore50** represents .NET Core 1.0 with CoreCLR. The TFM is nothing
more than a well-defined name that is used by tooling to refer to a .NET
Framework implementation on a platform. You will encounter and use the
TFMs when you want to define a compilation target in your project
definition.

Preferably, you compile for only one target that allows your code to run
anywhere. But this is not as trivial as it may seem. There is more than
one .NET Framework and multiple platforms add to the complexity.

## Platforms and frameworks

The .NET Platform has long been a broad and general term to capture
several elements that together form a platform for building and running
applications. The .NET Framework, the Software Development Kit, the
Windows desktop and the server operating system were the most important
elements. More operating systems were supported at a later stage, not
only Windows, but also Windows Phone, Linux and OS X through Mono (a
partial cross-platform port of part of the full .NET framework). Most
recently the Windows 10 wave with Universal Windows Applications
spanning across desktop, server, mobile and Internet of Things (IoT)
devices were added. Each of these operating systems are separate .NET
Platforms with their own version (or multiple versions) of a .NET
Framework. This year .NET Core will be added to this list of platforms.

# .NET Platform Standard

Transitioning your code to .NET Core requires your code and the
libraries it depends upon to be available and compatible with the
environment you run in. If you use an open-source library, it might use
certain APIs that are not. For example, a library could have references
to assemblies from the full .NET Framework 4.0 that have not been ported
to run cross-platform on Linux or OS X. When the code tries to call such
assemblies they are not available, essentially causing your code to not
work. Luckily Microsoft has come up with a way to indicate how much of
.NET is supported where. Staying within these boundaries will guarantee
your code to run wherever you intend it to run.

Originally, Portable Class Libraries (PCL) was the first solution to
these challenges of a variety of different platforms, with their
different capabilities and specific .NET Framework implementations. PCL
introduced a reduced subset of the .NET Framework that works on each of
the compatibility-targeted .NET platforms. This subset is based on the
lowest common denominator of support per .NET platform, so there is a
guarantee that it will stay within the boundaries of the supported
functionality in the .NET API. The .NET API per platform is defined by a
set of assemblies that define the shape of .NET types without
implementation. These so-called 'reference assemblies' form the
build-time contracts of .NET Framework per platform.

The approach of PCL to provide cross-framework compatibility has
evolved. The **.NET Platform Standard** provides an open-ended way to
represent binary portability across platforms. It is a standard that
uses versioned sets of reference assemblies to define the surface API of
the .NET Framework that is available to library developers on each of
the platforms. The targeted platforms themselves have an actual
implementation per reference assembly that you as a developer can use in
your code. Reference assemblies are explicitly used to define a contract
for the API surface and are now referred to as 'contract assemblies'.
Contrary to PCL, which defines the included platforms at build time, a
.NET Platform Standard version defines the surface API that must be
supported. This allows newer platforms not available at the time of
build to also use the libraries compiled against a specific version of a
.NET Platform Standard reference set, without having to recompile the
app to support the new platform.

The .NET Platform Standard defines multiple versions of such reference
sets of contract assemblies, named .NET Standard 1.0, 1.1 up to 1.4.
More will be added in the future when new APIs are created and the
reference contracts in the surface API change in an incompatible manner.
Each of the reference sets promise and require compatibility with the
specific .NET Platforms and frameworks. This has the important
implication that platforms might be dropped from support for newer
version of the .NET Platform Standard. Fewer platforms are supported,
but a bigger .NET surface API will be available to those that remain.

The following matrix shows the supported .NET platforms for each of the
current .NET Platform Standard versions. Start from a .NET Platform
Standard version (e.g. version 1.3) and read that column downwards. Each
colored horizontal block that crossed the column indicates
compatibility. A library targeted at .NET Platform Standard 1.3 can run
on .NET 4.6 and 4.6.x, UWP 10.0 and .NET Core 5.0.

  ---------------------------------------------------------------------------------
                                               **.NET                         
                                               Platform                       
                                               Standard**                     
  -------------------------- ----------------- ------------ ----- ----- ----- -----
  **Target Platform Name**   **Alias/TFM**     1.0          1.1   1.2   1.3   1.4

  .NET Framework             net               4.6.x                          

                                               4.6                            

                                               4.5.2                          

                                               4.5.1                          

                                               4.5                            

  Universal Windows Platform uap               10.0                           

  Windows                    win               8.1                            

                                               8.0                            

  Windows Phone              wpa               8.1                            

                                               8.0                            

  Windows Phone Silverlight  wp                8.1                            

                                               8.0                            

  **DNX Core**               **dnxcore**       **5.0**                        

  Mono/Xamarin Platforms                       \*                             

  Mono                                         \*                             
  ---------------------------------------------------------------------------------

\* indicates that the platform version that is supported for Mono and
Mono/Xamarin is yet to be determined.

The surface API defined in the contract assemblies of the .NET Standard
1.0 is small enough to be available on the full .NET Frameworks 4.5, and
later on Windows, the Universal Windows Platform, Windows Phone 8.0 and
8.1, .NET Core and Mono. Higher versions of the .NET Standard have a
bigger surface API but are only available on the newer platforms (since
the older ones will not be updated anymore). .NET Core is the new kid on
the block, and offers an implementation for the highest .NET Standard
with the broadest surface API.

Once you create a library and target a Platform Standard of a certain
version, you know it is compatible and able to run on the supported .NET
Platforms. Like all platforms, the standard version also has a TFM:
**netstandard1.0**, **netstandard1.1** to **netstandard1.4**. You need
these monikers when you create a library assembly yourself and want to
have a certain level of compatibility.

## The .NET Standard Library

Circling back to .NET Core: the .NET Framework implementation targeted
at the .NET Core Platform is called CoreFX. It has an implementation
according to .NET Standard 1.4. The list of contract assemblies in 1.4
is captured in the .NET Standard Library: a combination of assemblies
specific to the Platform Standard for multiple platforms and
platform-agnostic assemblies that are the same across platform versions.
The following figure shows the platform structure.

![](./media/image1.png)

The center of the figure contains the Platform Standard libraries, which
comprises the implementation according to a .NET Platform Standard as
outlined before. However, some assemblies are implemented as part of the
Platform specific runtimes, such as the full .NET Framework set or .NET
Core. Usually these assemblies contain operating system calls that are
abstracted away in .NET libraries. An example would be System.IO that
has a different implementation for Windows, Linux and OS X. These
'anchored' assemblies can only be updated by updating the particular
framework. Some assemblies are agnostic of the platform, such as
System.Linq. Such assemblies build upon the Platform Standard and
framework assemblies, but are not different for different versions,
platforms or operating systems.

The .NET Standard Library is the combination of the .NET Platform
Standard designated contract APIs and the platform agnostic ones. By
referencing that library, you know that your code uses a .NET API that
allows you to target, compile and run your code on Full .NET, .NET Core
or Mono. You will start seeing the related NuGet package appear as a
convenient way to reference .NET Platform Standard implementations.

# Inside the core

.NET Core is a cross-platform and open-source implementation of .NET. It
consists of three components:

-   CoreFX: .NET Core base class libraries including implementation of
    .NET Standard Library

-   Multiple compilers: RyuJIT, .NET Native, Roslyn and LLILC

-   Two runtimes: CoreCLR and CoreRT

-   Command-line interface tooling

These components show the clear separation of the .NET Core Framework
and the runtimes. It is also visible in the ownership and location of
the implementations. Each of the components are separate entities with
their own Git repository, which allows each individual component to
evolve separately while providing a .NET Platform together.

We will not drill into the different compilers that exist for .NET Core,
but suffice it to say that depending on your scenario, you can choose
for compilation to Intermediate Language (IL), native code through C++
code generation. Ahead-of-Time (AOT) compilation for creating single
binaries that contain both your compiled code together with the required
runtime and libraries is also available.

Of the two runtimes CoreCLR is the most important and complete one. It
is a Common Language Runtime much like that of the full .NET Framework.
The main difference is the cross-platform (Windows, Linux, OS X and
FreeBSD) implementation that was created for CoreCLR. CoreRT is a
runtime for code compiled with .NET Native as it requires a different
way of executing.

# .NET Core tooling

Since .NET Core is cross-platform over Linux distributions, OS X,
FreeBSD and Windows, the development experience cannot rely on the
general availability of a rich Integrated Development Environment (IDE)
such as Visual Studio 2015. Microsoft has created Visual Studio Code as
a lightweight cross-platform IDE for Linux and Mac. Command-line tooling
is the common ground for development on the various operating systems.
This strategy allows you to pick your favorite environment (VI, Emacs,
Notepad, Sublime Text or Visual Studio Code on your OS, or Visual Studio
2015 on Windows) for working with code and perform operations from the
command-line.

## .NET Core Command-Line Interface

.NET Core includes a set of stand-alone command-line interface (CLI)
tools that allow you to complete various tasks related to .NET Core
components. Currently, the CLI tooling is a separate download and
installation.

The previous incarnation of this tooling used **dnu.cmd** and
**dnx.exe** as two separate pieces of command-line tooling. The .NET
Development Utility (dnu) was for compilation and packaging, while the
.NET Execution Environment (dnx) was the bootstrapper for running .NET
Core applications. Each of these tools is now bundled in a new CLI tool
dotnet.exe.

**dotnet** is the CLI for performing .NET Core related operations. It is
an extensible entry point to various operations related to .NET Core
projects and source code. dotnet.exe is a driver that takes a command to
indicate the actual operation. A couple of examples will show what it
can do:

  -----------------------------------------------------------------------
  Command             Operation
  ------------------- ---------------------------------------------------
  dotnet new          Scaffold a new empty .NET project

  dotnet restore      Restores dependencies of projects

  dotnet build        Performs a build of a project's source code

  dotnet pack         Creates a NuGet package for the compiled binaries

  dotnet repl         Interactive REPL session (Read, Eval, Print, Loop)
  -----------------------------------------------------------------------

Each of these commands can accept a number of arguments if different
options or behavior are needed. The **dotnet** tool has built-in
documentation for each of the commands, which can be viewed by passing
the **\--help** switch for the particular command.

Each of the commands is actually a separate executable that is spawned
from the dotnet.exe tool. The convention is that the executable for a
command is named dotnet-*command*.exe, such as **dotnet-build.exe**. The
commands can be executed directly, but the preferred way is to use the
top level driver **dotnet**.

Initially you typically run **dotnet** from an empty directory with the
name of your .NET project. The folder name will be the default name of
your assembly. After that you can use Visual Studio 2015, Visual Studio
Code or your preferred code editor to program your application or
library. The two Visual Studio editions have built-in support for the
command-line tooling as part of their build system. At the time of
writing this article, this is still the precursor tooling of **dnx.exe**
and **dnu.cmd**, but this will probably be updated to support the
**dotnet.exe** tooling.

## A short walk-through of Hello World in .NET Core

Let's have a look at .NET Core by creating and inspecting a simple Hello
World application. Make sure you have .NET Core installed on your
operating system. Instructions for doing this can be found on the GitHub
repository for .NET Core CLR. Also, install the .NET CLI tooling. The
required links are listed at the end of this article.

Start a command prompt in your environment. This might be a command or
terminal window depending on your operating system. Create a new folder
on the file system named HelloWorld and enter that folder from the
console. Run the command **dotnet new** and inspect the generated
contents.

**project.json**:

{

    \"version\": \"1.0.0-\*\",

    \"compilationOptions\": {

        \"emitEntryPoint\": true

    },

    \"dependencies\": {

        \"NETStandard.Library\": \"1.0.0-rc2-23616\"

    },

    \"frameworks\": {

        \"dnxcore50\": { }

    }

}

The frameworks section of the project.json file lists all the targets
for which this project will be compiled. It targets a single framework
dnxcore50, which is the TFM for .NET Core 1.0 (and might change in the
future). Since this project is a console application and not a library,
it targets the .NET Core Platform specifically, not one of the
netstandard targets. It depends on NETStandard.Library, which is a
placeholder that bundles the dependencies for platform agnostic
assemblies (e.g. System.Linq, System.Runtime.Numerics) and the
NETStandard.Platform. In its turn the latter is a placeholder package
that contains contract assemblies representing the surface API of the
new .NET dependencies on the base class libraries in the Platform
Standard. The hierarchy of dependencies is shown below.

![](./media/image2.png)

The code for our skeleton application is as follows:

**Program.cs**

using System;

namespace ConsoleApplication

{

    public class Program

    {

        public static void Main(string\[\] args)

        {

            Console.WriteLine(\"Hello World!\");

        }

    }

}

The skeleton program is exactly the same as for .NET 1.0 back in 2002
since the basics of .NET have not changed much. It is only when you
start using more recent libraries that you will encounter the
differences. This is most noticeable when creating ASP.NET Core 1.0
applications, as it is targeted at netstandard1.4. Referring to the
matrix with the netstandards, you can tell that libraries from ASP.NET
Core 1.0 can only be run on full .NET Framework 4.6.x and .NET Core 1.0
(referred to in the picture as DNX Core 5.0). These require the specific
runtimes CLR and CoreCLR respectively, where CoreCLR is the only one
that has cross-platform implementations for Linux, OS X and FreeBSD.

Now, run the commands

**dotnet restore\
dotnet build**

This will restore the dependencies and build the code against the
packages that will be downloaded from the specified NuGet feeds. The
NuGet feeds are specified in the NuGet.Config file that was also
generated by the .NET Initializer tool (**dotnet new**). Most notable is
the inclusion of the feed for .NET Core itself at
<https://www.myget.org/F/dotnet-core/api/v3/index.json>.

Assuming the restore and build are successful, you should be able to run
the HelloWorld.exe executable by issuing the following command from the
project folder:

**dotnet run**

This will only display the infamous words "Hello World!". Although not
very impressive by output it is rather remarkable that this flow will
work regardless of the operating system you were working on. Your
executable was compiled targeted at dnxcore50 (i.e. .NET Core 1.0). .NET
Core has multiple CoreCLR runtimes for the various OSes and can even use
the .NET CLR runtime as an alternative on Windows.

## Managing runtime environments

The final piece of the puzzle of .NET Core is the management of the
various execution environments that have the runtimes. The .NET
execution environments (DNX) are versioned and targeted at an operating
system, processor architecture and runtime. The .NET Version Management
tool (DNVM) manages the environments that are installed on a machine.
Try running the tool from a command prompt to print a list of the
installed DNX versions:

**dnvm list**

The output is similar to this, but will vary based on what was
previously installed.

![](./media/image3.png)

The tool also allows you to choose the active DNX that determines the
actual runtime (CLR or CoreCLR) for executing your application. You can
change the active DNX by commands like

**dnvm use 1.0.0-rc1-final -r coreclr -a X64**

where you specify the version, runtime and architecture of the
processor. This must be one that can run on the current operating system
if you want to execute your application. However, you are free to
install and package runtimes of other operating systems to prepare
deployments of the application to those OSes.

# Conclusion

.NET Core is the latest addition to the existing set of .NET Platforms.
It is cross-platform and open source and a modernized implementation
that enables scenarios such as containerization of the applications
built with it. Although much is still the same, there are substantial
differences under the covers, including the existence of multiple
runtimes, compilers and tooling that come with .NET Core. The future of
.NET is looking bright and .NET Core is a first impression of what's in
store. The way .NET Core is designed allows a quick evolution of the
.NET Framework in new directions and across platforms never deemed
possible before.

# Links

Install .NET Core: <http://bit.ly/coreclr>

.NET CLI tooling: <http://bit.ly/dotnetcli>

.NET Platform Standard: <http://bit.ly/netplatformstd>
